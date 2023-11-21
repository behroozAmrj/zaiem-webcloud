using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatService.Models
{
    public class Message
    {
        public string DestinationURL;
        public string SenderUserID;
        public string ReceiverUserID;
        public string Text;
        public string RecieverApp;
        public List<RepositoryService.RUserProSecurity> userList;
        public string[] Funcs;
        
    }
}