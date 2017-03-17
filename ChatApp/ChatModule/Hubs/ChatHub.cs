using System;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SignalRChat
{
    public class ChatHub : Hub
    {
        public void Send(string sender_name, int empfaenger_id, string message, string timestamp)
        {

            // Call the addNewMessageToPage method to update clients.
            Clients.All.addNewMessageToPage(sender_name, empfaenger_id, message, timestamp);
        }
        public void IsTyping(string name, int sender_id, int empfaenger_id, int currentUserId)
        {
            // do stuff with the html
            Clients.All.sayWhoIsTyping(name, sender_id, empfaenger_id, currentUserId); //call the function to send the html to the other clients
        }

        //public void SayWhoIsTyping(string html)
        //{
        //    IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
        //    context.Clients.All.sayWhoIsTyping(html);
        //}

    }
}