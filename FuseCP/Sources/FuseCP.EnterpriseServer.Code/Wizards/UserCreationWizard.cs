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
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;

using FuseCP.Providers;
using FuseCP.Providers.Web;
using FuseCP.Providers.FTP;
using FuseCP.Providers.Mail;
using FuseCP.Providers.OS;

namespace FuseCP.EnterpriseServer
{
    public class UserCreationWizard: ControllerBase
    {
        public UserCreationWizard(): this(null) { }
        public UserCreationWizard(ControllerBase provider) : base(provider) { }

        public int CreateUserAccount(int parentPackageId, string username, string password,
            int roleId, string firstName, string lastName, string email, string secondaryEmail, bool htmlMail,
            bool sendAccountLetter,
            bool createPackage, int planId, bool sendPackageLetter,
            string domainName, bool tempDomain, bool createWebSite,
            bool createFtpAccount, string ftpAccountName, bool createMailAccount, string hostName, bool createZoneRecord)
        {
            UserCreationWizard wizard = new UserCreationWizard(this);

            return wizard.CreateUserAccountInternal(parentPackageId, username, password,
                roleId, firstName, lastName, email, secondaryEmail, htmlMail,
                sendAccountLetter,
                createPackage, planId, sendPackageLetter,
                domainName, tempDomain, createWebSite,
                createFtpAccount, ftpAccountName, createMailAccount, hostName, createZoneRecord);
        }

        // private fields
        bool userCreated = false;
        int createdUserId = 0;
        int createdPackageId = 0;

        public int CreateUserAccountInternal(int parentPackageId, string username, string password,
            int roleId, string firstName, string lastName, string email, string secondaryEmail, bool htmlMail,
            bool sendAccountLetter,
            bool createPackage, int planId, bool sendPackageLetter,
            string domainName, bool tempDomain, bool createWebSite,
            bool createFtpAccount, string ftpAccountName, bool createMailAccount, string hostName, bool createZoneRecord)
        {

            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive
                | DemandAccount.IsReseller);
            if (accountCheck < 0) return accountCheck;

            // check package
            int packageCheck = SecurityContext.CheckPackage(parentPackageId, DemandPackage.IsActive);
            if (packageCheck < 0) return packageCheck;

            // check if username exists
            if (UserController.UserExists(username))
                return BusinessErrorCodes.ERROR_ACCOUNT_WIZARD_USER_EXISTS;

            // check if domain exists
            int checkDomainResult = ServerController.CheckDomain(domainName);
            if (checkDomainResult < 0)
                return checkDomainResult;

            // check if FTP account exists
            if (String.IsNullOrEmpty(ftpAccountName))
                ftpAccountName = username;

            if (FtpServerController.FtpAccountExists(ftpAccountName))
                return BusinessErrorCodes.ERROR_ACCOUNT_WIZARD_FTP_ACCOUNT_EXISTS;

            // load parent package
            PackageInfo parentPackage = PackageController.GetPackage(parentPackageId);

            /********************************************
             *  CREATE USER ACCOUNT
             * *****************************************/
            UserInfo user = new UserInfo();
            user.RoleId = roleId;
            user.StatusId = (int)UserStatus.Active;
            user.OwnerId = parentPackage.UserId;
            user.IsDemo = false;
            user.IsPeer = false;

            // account info
            user.FirstName = firstName;
            user.LastName = lastName;
            user.Email = email;
            user.SecondaryEmail = secondaryEmail;
            user.Username = username;
//            user.Password = password;
            user.HtmlMail = htmlMail;

            // add a new user
            createdUserId = UserController.AddUser(user, false, password);
            if (createdUserId < 0)
            {
                // exit
                return createdUserId;
            }
            userCreated = true;

            // create package
            // load hosting plan
            createdPackageId = -1;
            if (createPackage)
            {
                try
                {
                    HostingPlanInfo plan = PackageController.GetHostingPlan(planId);

                    PackageResult packageResult = PackageController.AddPackage(
                        createdUserId, planId, plan.PlanName, "", (int)PackageStatus.Active, DateTime.Now, false);
                    createdPackageId = packageResult.Result;
                }
                catch (Exception ex)
                {
                    // error while adding package

                    // remove user account
                    UserController.DeleteUser(createdUserId);

                    throw ex;
                }

                if (createdPackageId < 0)
                {
                    // rollback wizard
                    Rollback();

                    // return code
                    return createdPackageId;
                }

                // create domain
                int domainId = 0;
                if ((createWebSite || createMailAccount || createZoneRecord) && !String.IsNullOrEmpty(domainName))
                {
                    try
                    {
                        DomainInfo domain = new DomainInfo();
                        domain.PackageId = createdPackageId;
                        domain.DomainName = domainName;
                        domain.HostingAllowed = false;
                        domainId = ServerController.AddDomain(domain, false, false);
                        if (domainId < 0)
                        {
                            // rollback wizard
                            Rollback();

                            // return
                            return domainId;
                        }
                    }
                    catch (Exception ex)
                    {
                        // rollback wizard
                        Rollback();

                        // error while adding domain
                        throw new Exception("Could not add domain", ex);
                    }
                }

                if (createWebSite && (domainId > 0))
                {
                    // create web site
                    try
                    {
                        int webSiteId = WebServerController.AddWebSite(
                            createdPackageId, hostName, domainId, 0, true, false);
                        if (webSiteId < 0)
                        {
                            // rollback wizard
                            Rollback();

                            // return
                            return webSiteId;
                        }
                    }
                    catch (Exception ex)
                    {
                        // rollback wizard
                        Rollback();

                        // error while creating web site
                        throw new Exception("Could not create web site", ex);
                    }
                }

                // create FTP account
                if (createFtpAccount)
                {
                    try
                    {
                        FtpAccount ftpAccount = new FtpAccount();
                        ftpAccount.PackageId = createdPackageId;
                        ftpAccount.Name = ftpAccountName;
                        ftpAccount.Password = password;
                        ftpAccount.Folder = "\\";
                        ftpAccount.CanRead = true;
                        ftpAccount.CanWrite = true;

                        int ftpAccountId = FtpServerController.AddFtpAccount(ftpAccount);
                        if (ftpAccountId < 0)
                        {
                            // rollback wizard
                            Rollback();

                            // return
                            return ftpAccountId;
                        }
                    }
                    catch (Exception ex)
                    {
                        // rollback wizard
                        Rollback();

                        // error while creating ftp account
                        throw new Exception("Could not create FTP account", ex);
                    }
                }

                if (createMailAccount && (domainId > 0))
                {
                    // create default mailbox
                    try
                    {
                        // load mail policy
                        UserSettings settings = UserController.GetUserSettings(createdUserId, UserSettings.MAIL_POLICY);
                        string catchAllName = !String.IsNullOrEmpty(settings["CatchAllName"])
                            ? settings["CatchAllName"] : "mail";

                        MailAccount mailbox = new MailAccount();
                        mailbox.Name = catchAllName + "@" + domainName;
                        mailbox.PackageId = createdPackageId;

                        // gather information from the form
                        mailbox.Enabled = true;

                        mailbox.ResponderEnabled = false;
                        mailbox.ReplyTo = "";
                        mailbox.ResponderSubject = "";
                        mailbox.ResponderMessage = "";

                        // password
                        mailbox.Password = password;

                        // redirection
                        mailbox.ForwardingAddresses = new string[] { };
                        mailbox.DeleteOnForward = false;
                        mailbox.MaxMailboxSize = 0;

                        int mailAccountId = MailServerController.AddMailAccount(mailbox);

                        if (mailAccountId < 0)
                        {
                            // rollback wizard
                            Rollback();

                            // return
                            return mailAccountId;
                        }

                        // set catch-all account
                        MailDomain mailDomain = MailServerController.GetMailDomain(createdPackageId, domainName);
                        mailDomain.CatchAllAccount = "mail";
                        mailDomain.PostmasterAccount = "mail";
                        mailDomain.AbuseAccount = "mail";
                        MailServerController.UpdateMailDomain(mailDomain);

                        int mailDomainId = mailDomain.Id;
                    }
                    catch (Exception ex)
                    {
                        // rollback wizard
                        Rollback();

                        // error while creating mail account
                        throw new Exception("Could not create mail account", ex);
                    }
                }

                // Preview Domain / Temporary URL
                if (tempDomain && (domainId > 0))
                {
                    int previewDomainId = ServerController.CreateDomainPreviewDomain("", domainId);
                    if (previewDomainId < 0)
                    {
                        // rollback wizard
                        Rollback();

                        return previewDomainId;
                    }
                }

                // Domain DNS Zone
                if (createZoneRecord && (domainId > 0))
                {
                    ServerController.EnableDomainDns(domainId);
                }
            }

            // send welcome letters
            if (sendAccountLetter)
            {
                int result = PackageController.SendAccountSummaryLetter(createdUserId, null, null, true);
                if (result < 0)
                {
                    // rollback wizard
                    Rollback();

                    // return
                    return result;
                }
            }

            if (createPackage && sendPackageLetter)
            {
                int result = PackageController.SendPackageSummaryLetter(createdPackageId, null, null, true);
                if (result < 0)
                {
                    // rollback wizard
                    Rollback();

                    // return
                    return result;
                }
            }

            return createdUserId;
        }

        public void Rollback()
        {
            if (userCreated)
            {
                // delete user account and all its packages
                UserController.DeleteUser(createdUserId);
            }
        }
    }
}
