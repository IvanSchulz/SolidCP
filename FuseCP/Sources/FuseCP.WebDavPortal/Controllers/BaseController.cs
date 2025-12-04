using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FuseCP.Server.Utils;
using FuseCP.WebDavPortal.Models.Common;
using FuseCP.WebDavPortal.Models.Common.Enums;

namespace FuseCP.WebDavPortal.Controllers
{
    public class BaseController : Controller
    {
        public const string MessagesKey = "messagesKey";

        public void AddMessage(MessageType type, string value)
        {
            Log.WriteStart("AddMessage");

            var messages = TempData[MessagesKey] as List<Message>;

            if (messages == null)
            {
                messages = new List<Message>();
            }

            messages.Add(new Message
            {
                Type = type,
                Value = value
            });

            TempData[MessagesKey] = messages;

            Log.WriteEnd("AddMessage");
        }
    }
}
