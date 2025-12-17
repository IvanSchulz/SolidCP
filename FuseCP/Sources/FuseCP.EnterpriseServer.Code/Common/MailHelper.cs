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
using System.Collections.Generic;
using System.Text;
using MimeKit;
using MailKit;
using MailKit.Security;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using System.Security.Authentication;
using System.IO;

namespace FuseCP.EnterpriseServer
{
    public class MailHelper
    {
        public static async Task<int> SendMessageAsync(string from, string to, string bcc, string subject, string body,
            MimeKit.MessagePriority priority, bool isHtml, IEnumerable<MimePart> attachments)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    SystemSettings settings;

					using (var systemController = new SystemController()) {
                        settings = systemController.GetSystemSettingsInternal(
                           SystemSettings.SMTP_SETTINGS,
                           true
                       );
                    }
                    string smtpServer = settings["SmtpServer"];
                    int smtpPort = settings.GetInt("SmtpPort");
                    string smtpUsername = settings["SmtpUsername"];
                    string smtpPassword = settings["SmtpPassword"];
                    bool enableSsl = Utils.ParseBool(settings["SmtpEnableSsl"], false);
                    bool enableLegacySSL = Utils.ParseBool(settings["SmtpEnableLegacySSL"], false);

                    // Determine SecureSocketOptions based on EnableSsl and Port
                    // Common logic: Port 465 usually implies SslOnConnect, others often use StartTls. Adjust as needed.
                    SecureSocketOptions secureSocketOptions = SecureSocketOptions.Auto;
                    if (enableSsl)
                    {
                        secureSocketOptions = smtpPort == 465 ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls;
                    }

                    if (enableLegacySSL)
                    {
                        client.SslProtocols = SslProtocols.Tls11 | SslProtocols.Tls | SslProtocols.Tls12 | SslProtocols.Tls13;
                    }
                    else
                    {
                        client.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13;
                    }

                    // Connect
                    await client.ConnectAsync(smtpServer, smtpPort, secureSocketOptions);

                    // Authenticate if username is provided
                    if (!String.IsNullOrEmpty(smtpUsername))
                    {
                        await client.AuthenticateAsync(smtpUsername, smtpPassword);
                    }

                    var message = new MimeMessage();
                    message.From.Add(MailboxAddress.Parse(from));
                    message.To.Add(MailboxAddress.Parse(to));
                    if (!String.IsNullOrEmpty(bcc))
                    {
                        message.Bcc.Add(MailboxAddress.Parse(bcc));
                    }
                    message.Subject = subject;
                    message.Priority = priority;

                    // Create body
                    var bodyBuilder = new BodyBuilder();
                    if (isHtml)
                    {
                        bodyBuilder.HtmlBody = body;
                    }
                    else
                    {
                        bodyBuilder.TextBody = body;
                    }

                    // Add attachments if any
                    if (attachments != null) //
                    {
                        foreach (var attachment in attachments) // Now iterating MimePart objects
                        {
                            if (attachment.IsAttachment) // Check if it's meant as an attachment or inline
                            {
                                bodyBuilder.Attachments.Add(attachment); // Add to regular attachments
                            }
                            else
                            {
                                // If it's inline (ContentDisposition.Inline), add to LinkedResources
                                // This assumes the HTML body references it via cid:ContentId
                                bodyBuilder.LinkedResources.Add(attachment); //
                            }
                        }
                    }

                    message.Body = bodyBuilder.ToMessageBody();


                    // Send messages
                    await client.SendAsync(message);

                    return 0;
                }
                catch (SmtpCommandException ex)
                {
                    return BusinessErrorCodes.SMTP_GENERAL_FAILURE;
                }
                catch (MailKit.Security.AuthenticationException ex)
                {
                    return BusinessErrorCodes.SMTP_CLIENT_NOT_PERMITTED;
                }
                catch (Exception ex)
                {
                    return BusinessErrorCodes.SMTP_UNKNOWN_ERROR;
                }
                finally
                {
                    // Disconnect
                    if (client.IsConnected)
                    {
                        await client.DisconnectAsync(true);
                    }
                }
            }
        }

        public static int SendMessage(string from, string to, string bcc, string subject, string body,
           System.Net.Mail.MailPriority priority, bool isHtml, System.Net.Mail.Attachment[] attachments)
        {
            // Convert priority
            MimeKit.MessagePriority mailkitPriority;
            switch (priority)
            {
                case System.Net.Mail.MailPriority.High:
                    mailkitPriority = MessagePriority.Urgent;
                    break;
                case System.Net.Mail.MailPriority.Low:
                    mailkitPriority = MessagePriority.NonUrgent;
                    break;
                default:
                    mailkitPriority = MessagePriority.Normal;
                    break;
            }

            var mailkitAttachments = new List<MimePart>();
            if (attachments != null) //
            {
                foreach (var attachment in attachments) //
                {
                    try
                    {
                        // Determine ContentType - MimeKit has better auto-detection but respecting original if possible
                        var contentType = MimeKit.ContentType.Parse(attachment.ContentType?.ToString() ?? "application/octet-stream"); //

                        // Create MimePart - primarily handles streams.
                        // System.Net.Mail.Attachment exposes its content via ContentStream.
                        var mimePart = new MimePart(contentType)
                        {
                            Content = new MimeContent(attachment.ContentStream), // Get stream
                            ContentDisposition = new ContentDisposition(ContentDisposition.Attachment), // Mark as attachment
                            ContentTransferEncoding = attachment.TransferEncoding == System.Net.Mime.TransferEncoding.Base64 ? ContentEncoding.Base64 : ContentEncoding.Default, // Map encoding
                            FileName = attachment.Name // Set filename
                        };

                        // Handle ContentId if present (for inline attachments)
                        if (!string.IsNullOrEmpty(attachment.ContentId)) //
                        {
                            mimePart.ContentId = attachment.ContentId; //
                                                                       // Override ContentDisposition for inline
                            mimePart.ContentDisposition = new ContentDisposition(ContentDisposition.Inline); //
                        }

                        mailkitAttachments.Add(mimePart); //
                    }
                    catch (Exception ex)
                    {
                        // Log the error if a specific attachment fails conversion
                        Console.WriteLine($"Error converting attachment '{attachment.Name}': {ex.Message}"); //
                                                                                                             // Decide whether to continue without this attachment or fail the whole send
                    }
                    finally
                    {
                        // IMPORTANT: Dispose the original System.Net.Mail.Attachment
                        // to release any resources it holds (like file handles or streams
                        // if it created them internally). MailKit takes ownership of the stream passed to MimeContent.
                        attachment.Dispose(); //
                    }
                }
            }


            try
            {
                return SendMessageAsync(from, to, bcc, subject, body, mailkitPriority, isHtml, mailkitAttachments).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                return BusinessErrorCodes.SMTP_UNKNOWN_ERROR;
            }
        }

        public static int SendMessage(string from, string to, string subject, string body, bool isHtml)
        {
            return SendMessage(from, to, null, subject, body, System.Net.Mail.MailPriority.Normal, isHtml, null);
        }

        public static int SendMessage(string from, string to, string bcc, string subject, string body, bool isHtml)
        {
            return SendMessage(from, to, bcc, subject, body, System.Net.Mail.MailPriority.Normal, isHtml, null);
        }

        public static int SendMessage(string from, string to, string bcc, string subject, string body,
            System.Net.Mail.MailPriority priority, bool isHtml)
        {
            return SendMessage(from, to, bcc, subject, body, priority, isHtml, null);
        }

    }
}
