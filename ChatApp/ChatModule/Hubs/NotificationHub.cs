using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace ChatModule.Hubs
{
    //Now, we will create a SignalR Hub class, which makes possible to invoke the client side JavaScript method from the server side.Here in this application, we will use this for showing notification.
    //SignalR uses ‘Hub’ objects to communicate between the client and the server.
    public class NotificationHub : Hub
    {
        public void SendNotification(string notification, int sender_id, int empfaenger_id)
        {
            GlobalHost.ConnectionManager.GetHubContext<NotificationHub>().Clients.All.getNotification(notification, sender_id, empfaenger_id);
        }
    }
    //you can see here, the NotificationHub.cs class is empty. Left the class empty as we will use the class later from another place.
}