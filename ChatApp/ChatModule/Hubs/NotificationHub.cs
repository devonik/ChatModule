// ***********************************************************************
// Assembly         : ChatModule
// Author           : grieger
// Created          : 03-09-2017
//
// Last Modified By : grieger
// Last Modified On : 03-06-2017
// ***********************************************************************
// <copyright file="NotificationHub.cs" company="Berenberg">
//     Copyright © Berenberg 2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace ChatModule.Hubs
{
    //Now, we will create a SignalR Hub class, which makes possible to invoke the client side JavaScript method from the server side.Here in this application, we will use this for showing notification.
    //SignalR uses ‘Hub’ objects to communicate between the client and the server.
    /// <summary>
    /// Class NotificationHub.
    /// </summary>
    /// <seealso cref="Microsoft.AspNet.SignalR.Hub" />
    public class NotificationHub : Hub
    {
        /// <summary>
        /// Sends the notification.
        /// </summary>
        /// <param name="notification">The notification.</param>
        /// <param name="sender_id">The sender identifier.</param>
        /// <param name="empfaenger_id">The empfaenger identifier.</param>
        public void SendNotification(string notification, int sender_id, int empfaenger_id)
        {
            GlobalHost.ConnectionManager.GetHubContext<NotificationHub>().Clients.All.getNotification(notification, sender_id, empfaenger_id);
        }
    }
    //you can see here, the NotificationHub.cs class is empty. Left the class empty as we will use the class later from another place.
}