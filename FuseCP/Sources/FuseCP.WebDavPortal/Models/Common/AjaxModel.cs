using System.Collections.Generic;
using FuseCP.WebDavPortal.Models.Common.Enums;

namespace FuseCP.WebDavPortal.Models.Common
{
    public class AjaxModel
    {
        public AjaxModel()
        {
            Messages = new List<Message>();
        }

        public List<Message> Messages { get; private set; }

        public void AddMessage(MessageType type, string value)
        {
            Messages.Add(new Message
            {
                Type = type,
                Value = value
            });
        } 
    }
}
