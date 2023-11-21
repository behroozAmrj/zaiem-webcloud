using ChatService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatService.Domain
{
    public class MessageBuilder
    {
        public MessageBuilder()
        {

        }

        public void Send(Message message)
        {
            try
            {
                if (message.Funcs.Any(x => x == "getOnlineUsers"))
                    getOnlineUsers(message);
                var repService = new RepositoryService.RepositoryClient();
                var user = repService.getUser(message.ReceiverUserID);
                message.ReceiverUserID = user.ConnectionID;
                //message.userList = repService.getOnlineUsers().ToList<RepositoryService.RUserProSecurity>();
                message.RecieverApp = "chat";
            }
            catch (Exception ex)
            {
                message.ReceiverUserID = "No user is available";
            }
            using (var network = new NetworkAdap(message.DestinationURL, message.DestinationURL, new string[] { }))
            {
                network.SendMessage(message);
            }
        }

        public void getOnlineUsers(Message message)
        {
            var repService = new RepositoryService.RepositoryClient();
            var user = repService.getUser(message.ReceiverUserID);
            if (user == null)
                throw new Exception("this user not exist");
            message.userList = repService.getOnlineUsers().Select((x) => 
            {
                var nR = new RepositoryService.RUserProSecurity();
                nR.ConnectionID = x.ConnectionID;
                nR.UserID = x.UserID;
                nR.UserName = x.UserName;
                return (nR);
            }).ToList<RepositoryService.RUserProSecurity>();
        }
    }
}