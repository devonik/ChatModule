// ***********************************************************************
// Assembly         : ChatModule
// Author           : grieger
// Created          : 03-09-2017
//
// Last Modified By : grieger
// Last Modified On : 04-03-2017
// ***********************************************************************
// <copyright file="ChatHub.cs" company="Berenberg">
//     Copyright © Berenberg 2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SignalRChat
{
    /// <summary>
    /// Class ChatHub.
    /// </summary>
    /// <seealso cref="Microsoft.AspNet.SignalR.Hub" />
    public class ChatHub : Hub
    {
        //Enthält Daten für eine Chat message
        /// <summary>
        /// Sends the specified sender name.
        /// </summary>
        /// <param name="sender_name">Name of the sender.</param>
        /// <param name="empfaenger_id">The empfaenger identifier.</param>
        /// <param name="message">The message.</param>
        /// <param name="timestamp">The timestamp.</param>
        public void Send(string sender_name, int empfaenger_id, string message, string timestamp)
        {

            //Sendet die Message an die Oberfläche aller verbundenen Clients
            Clients.All.addNewMessageToPage(sender_name, empfaenger_id, message, timestamp);
        }
        //Enthält Daten wer gerade schreibt
        /// <summary>
        /// Determines whether the specified name is typing.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="sender_id">The sender identifier.</param>
        /// <param name="empfaenger_id">The empfaenger identifier.</param>
        /// <param name="currentUserId">The current user identifier.</param>
        public void IsTyping(string name, int sender_id, int empfaenger_id, int currentUserId)
        {
            //Sendet die Info, wer gerade schreibt, an alle verbundenen Clients
            Clients.All.sayWhoIsTyping(name, sender_id, empfaenger_id, currentUserId);
        }


    }
}