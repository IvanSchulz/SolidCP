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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using FuseCP.Providers.DomainLookup;
using FuseCP.EnterpriseServer.Data;
using Whois.NET;

namespace FuseCP.EnterpriseServer
{
    public class DomainExpirationTask: SchedulerTask
    {
        private static readonly string TaskId = "SCHEDULE_TASK_DOMAIN_EXPIRATION";

        // Input parameters:
        private static readonly string DaysBeforeNotify = "DAYS_BEFORE";
        private static readonly string MailToParameter = "MAIL_TO";
        private static readonly string EnableNotification = "ENABLE_NOTIFICATION";
        private static readonly string IncludeNonExistenDomains = "INCLUDE_NONEXISTEN_DOMAINS";


        private static readonly string MailBodyTemplateParameter = "MAIL_BODY";
        private static readonly string MailBodyDomainRecordTemplateParameter = "MAIL_DOMAIN_RECORD";

        public override void DoWork()
        {
            BackgroundTask topTask = TaskManager.TopTask;
            var domainUsers = new Dictionary<int, UserInfo>();
            var checkedDomains = new List<DomainInfo>();
            var expiredDomains = new List<DomainInfo>();
            var nonExistenDomains = new List<DomainInfo>();
            var allDomains = new List<DomainInfo>();
            var allTopLevelDomains = new List<DomainInfo>();

            // get input parameters
            int daysBeforeNotify;
            bool sendEmailNotifcation = Convert.ToBoolean( topTask.GetParamValue(EnableNotification));
            bool includeNonExistenDomains = Convert.ToBoolean(topTask.GetParamValue(IncludeNonExistenDomains));

            // check input parameters
            if (String.IsNullOrEmpty((string)topTask.GetParamValue("MAIL_TO")))
            {
                TaskManager.WriteWarning("The e-mail message has not been sent because 'Mail To' is empty.");
                return;
            }

            int.TryParse((string)topTask.GetParamValue(DaysBeforeNotify), out daysBeforeNotify);

            var user = UserController.GetUser(topTask.EffectiveUserId);

            var packages = GetUserPackages(user.UserId, user.Role);


            foreach (var package in packages)
            {
                var domains = ServerController.GetDomains(package.PackageId);

                allDomains.AddRange(domains);

                domains = domains.Where(x => !x.IsSubDomain && !x.IsDomainPointer).ToList(); //Selecting top-level domains

                allTopLevelDomains.AddRange(domains);

                var domainUser = UserController.GetUser(package.UserId);

                if (!domainUsers.ContainsKey(package.PackageId))
                {
                    domainUsers.Add(package.PackageId, domainUser);
                }

                foreach (var domain in domains)
                {
                    if (checkedDomains.Any(x=> x.DomainId == domain.DomainId))
                    {
                        continue;
                    }

                    checkedDomains.Add(domain);

                    ServerController.UpdateDomainWhoisData(domain);

                    if (CheckDomainExpiration(domain.ExpirationDate, daysBeforeNotify))
                    {
                        expiredDomains.Add(domain);
                    }

                    if (domain.ExpirationDate == null && domain.CreationDate == null)
                    {
                        nonExistenDomains.Add(domain);
                    }

                    Thread.Sleep(100);
                }
            }

            var subDomains = allDomains.Where(x => !checkedDomains.Any(z => z.DomainId == x.DomainId && z.ExpirationDate != null)).GroupBy(p => p.DomainId).Select(g => g.First()).ToList();

            foreach (var subDomain in subDomains)
            {
                var mainDomain = checkedDomains.Where(x => subDomain.DomainId != x.DomainId && subDomain.DomainName.ToLowerInvariant().Contains(x.DomainName.ToLowerInvariant())).OrderByDescending(s => s.DomainName.Length).FirstOrDefault(); ;

                if (mainDomain != null)
                {
                    ServerController.UpdateDomainWhoisData(subDomain, mainDomain.CreationDate, mainDomain.ExpirationDate, mainDomain.RegistrarName);

                    var nonExistenDomain = nonExistenDomains.FirstOrDefault(x => subDomain.DomainId == x.DomainId);

                    if (nonExistenDomain != null)
                    {
                        nonExistenDomains.Remove(nonExistenDomain);
                    }

                    Thread.Sleep(100);
                }
            }

            expiredDomains = expiredDomains.GroupBy(p => p.DomainId).Select(g => g.First()).ToList();

            if (expiredDomains.Count > 0 && sendEmailNotifcation)
            {
                SendMailMessage(user, expiredDomains, domainUsers, nonExistenDomains, includeNonExistenDomains);
            }
        }

        private IEnumerable<PackageInfo> GetUserPackages(int userId,UserRole userRole)
        {
            var packages = new List<PackageInfo>();

            switch (userRole)
            {
                case UserRole.Administrator:
                {
                    packages = ObjectUtils.CreateListFromDataReader<PackageInfo>(Database.GetAllPackages());
                    break;
                }
                default:
                {
                    packages = PackageController.GetMyPackages(userId);
                    break; 
                }
            }

            return packages;
        }

        private bool CheckDomainExpiration(DateTime? date, int daysBeforeNotify)
        {
            if (date == null)
            {
                return false;
            }

            return (date.Value - DateTime.Now).Days < daysBeforeNotify;
        }

        private void SendMailMessage(UserInfo user, IEnumerable<DomainInfo> domains, Dictionary<int, UserInfo> domainUsers, IEnumerable<DomainInfo> nonExistenDomains, bool includeNonExistenDomains)
        {
            BackgroundTask topTask = TaskManager.TopTask;

            UserSettings settings = UserController.GetUserSettings(user.UserId, UserSettings.DOMAIN_EXPIRATION_LETTER);

            string from = settings["From"];

            var bcc = settings["CC"];

            string subject = settings["Subject"];
            string body = user.HtmlMail ? settings["HtmlBody"] : settings["TextBody"];
            bool isHtml = user.HtmlMail;

            MailPriority priority = MailPriority.Normal;
            if (!String.IsNullOrEmpty(settings["Priority"]))
                priority = (MailPriority)Enum.Parse(typeof(MailPriority), settings["Priority"], true);

            // input parameters
            string mailTo = (string)topTask.GetParamValue("MAIL_TO");

            Hashtable items = new Hashtable();

            items["user"] = user;

            items["Domains"] = domains
                .Select(x => new {
                    x.DomainName, 
                    ExpirationDate = x.ExpirationDate  < DateTime.Now ? "Expired" : x.ExpirationDate.ToString(),
                    ExpirationDateOrdering = x.ExpirationDate, 
                    Registrar = x.RegistrarName,
                    Customer = string.Format("{0} {1}", domainUsers[x.PackageId].FirstName, domainUsers[x.PackageId].LastName) })
                .OrderBy(x => x.ExpirationDateOrdering).ThenBy(x => x.Customer).ThenBy(x => x.DomainName);
            
            items["IncludeNonExistenDomains"] = includeNonExistenDomains;

            items["NonExistenDomains"] = nonExistenDomains.Select(x => new
            {
                DomainName = x.DomainName,
                Customer = string.Format("{0} {1}", domainUsers[x.PackageId].FirstName, domainUsers[x.PackageId].LastName)
            }).OrderBy(x => x.Customer).ThenBy(x => x.DomainName);


            body = PackageController.EvaluateTemplate(body, items);

            // send mail message
            MailHelper.SendMessage(from, mailTo, bcc, subject, body, priority, isHtml);
        }



    }
}
