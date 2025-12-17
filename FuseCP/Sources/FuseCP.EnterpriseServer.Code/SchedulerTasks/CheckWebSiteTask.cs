// Copyright (C) 2025 FuseCP
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace FuseCP.EnterpriseServer
{
    public class CheckWebSiteTask : SchedulerTask
    {
        private class WebSiteResponse
        {
            public int Status;
            public string Text;
        }

        public override void DoWork()
        {
            // Input parameters:
            //  - URL
            //  - USERNAME
            //  - PASSWORD
            //  - RESPONSE_STATUS
            //  - RESPONSE_CONTAIN
            //  - RESPONSE_DOESNT_CONTAIN
            //  - MAIL_FROM
            //  - MAIL_TO
            //  - MAIL_SUBJECT
            //  - MAIL_BODY

            BackgroundTask topTask = TaskManager.TopTask;

            // get input parameters
            string url = (string)topTask.GetParamValue("URL");
            string username = (string)topTask.GetParamValue("USERNAME");
            string password = (string)topTask.GetParamValue("PASSWORD");
            string strResponseStatus = (string)topTask.GetParamValue("RESPONSE_STATUS");
            string responseContains = (string)topTask.GetParamValue("RESPONSE_CONTAIN");
            string responseNotContains = (string)topTask.GetParamValue("RESPONSE_DOESNT_CONTAIN");

			bool useResponseStatus = Convert.ToBoolean(topTask.GetParamValue("USE_RESPONSE_STATUS"));
			bool useResponseContains = Convert.ToBoolean(topTask.GetParamValue("USE_RESPONSE_CONTAIN"));
			bool useResponseDoesntContain = Convert.ToBoolean(topTask.GetParamValue("USE_RESPONSE_DOESNT_CONTAIN"));

            // check input parameters
            if (String.IsNullOrEmpty(url))
            {
                TaskManager.WriteWarning("Specify 'Web Site URL' task parameter.");
                return;
            }

            if ((String.IsNullOrEmpty(strResponseStatus) || !useResponseStatus)
                && (String.IsNullOrEmpty(responseContains) || !useResponseContains)
                && (String.IsNullOrEmpty(responseNotContains) || !useResponseDoesntContain))
            {
                TaskManager.WriteWarning("Specify one of 'Response Status', 'Response Contain' or 'Response Doesn't Contain' parameters.");
                return;
            }

            int responseStatus = Utils.ParseInt(strResponseStatus, -1);
            if (!String.IsNullOrEmpty(strResponseStatus) && responseStatus == -1)
            {
                TaskManager.WriteWarning("Specify correct response HTTP status, e.g. 404, 500, 503, etc.");
                return;
            }

            // load web site
            WebSiteResponse resp = GetWebDocument(url, username, password);

            // check if there was a generic error
            if (resp.Status == -1)
            {
                SendMailMessage(url, resp.Text, "");
            }

            bool sendMessage = false;

            // check status
            if (responseStatus != -1)
            {
            	sendMessage |= ((resp.Status == responseStatus) && useResponseStatus);
            }

            // check "contains"
            if (!String.IsNullOrEmpty(responseContains))
            {
            	sendMessage |= ((resp.Text.ToLower().IndexOf(responseContains.ToLower()) != -1) && useResponseContains);
            }

            // check "not contains"
            if (!String.IsNullOrEmpty(responseNotContains))
            {
            	sendMessage |= ((resp.Text.ToLower().IndexOf(responseNotContains.ToLower()) == -1) && useResponseDoesntContain);
            }

            if (sendMessage)
                SendMailMessage(url, "", resp.Text);
        }

        private void SendMailMessage(string url, string message, string content)
        {
            BackgroundTask topTask = TaskManager.TopTask;

            // input parameters
            string mailFrom = (string)topTask.GetParamValue("MAIL_FROM");
            string mailTo = (string)topTask.GetParamValue("MAIL_TO");
            string mailSubject = (string)topTask.GetParamValue("MAIL_SUBJECT");
            string mailBody = (string)topTask.GetParamValue("MAIL_BODY");

            if (String.IsNullOrEmpty(mailTo))
            {
                TaskManager.WriteWarning("The e-mail message has not been sent because 'Mail To' is empty.");
            }
            else
            {
                if (String.IsNullOrEmpty(mailFrom))
                    mailFrom = "automatic@localhost";

                if (!String.IsNullOrEmpty(mailSubject))
                {
                    mailSubject = Utils.ReplaceStringVariable(mailSubject, "url", url);
                }

                if (!String.IsNullOrEmpty(mailBody))
                {
                    mailBody = Utils.ReplaceStringVariable(mailBody, "url", url);
                    mailBody = Utils.ReplaceStringVariable(mailBody, "message", message);
                    mailBody = Utils.ReplaceStringVariable(mailBody, "content", content);
                }
                else
                {
                    mailBody = message;
                }

                // send mail message
                MailHelper.SendMessage(mailFrom, mailTo, mailSubject, mailBody, false);
            }
        }

        private WebSiteResponse GetWebDocument(string url, string username, string password)
        {
            WebSiteResponse result = new WebSiteResponse();
            HttpWebResponse resp = null;
            StringBuilder sb = new StringBuilder();
            Stream respStream = null;
            try
            {
                // Enable TLS1.2 support if its https
                if (url.StartsWith("https://"))
                {
                    TaskManager.Write("Identified as SSL Website");
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                }

                WebRequest req = WebRequest.Create(url);

                // set site credentials if required
                if (!String.IsNullOrEmpty(username))
                {
                    req.Credentials = new NetworkCredential(username, password);
                }

                resp = (HttpWebResponse)req.GetResponse();
                respStream = resp.GetResponseStream();
                string charSet = !String.IsNullOrEmpty(resp.CharacterSet) ? resp.CharacterSet : "utf-8";
                Encoding encode = System.Text.Encoding.GetEncoding(charSet);

                StreamReader sr = new StreamReader(respStream, encode);

                Char[] read = new Char[256];
                int count = sr.Read(read, 0, 256);

                while (count > 0)
                {
                    String str = new String(read, 0, count);
                    sb.Append(str);
                    count = sr.Read(read, 0, 256);
                }

                result.Status = (int)resp.StatusCode;
                result.Text = sb.ToString();
            }
            catch (ThreadAbortException)
            {
            }
            catch (WebException ex)
            {
                result.Status = (int)((HttpWebResponse)ex.Response).StatusCode;
                result.Text = ex.ToString();
                TaskManager.WriteError(ex.ToString());
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Text = ex.ToString();
                TaskManager.WriteError(ex.ToString());
            }
            finally
            {
                if (respStream != null)
                {
                    respStream.Close();
                }
                
                if (resp != null)
                {
                    resp.Close();
                }

            }

            return result;
        }
    }
}
