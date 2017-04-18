// ***********************************************************************
// Assembly         : ChatModule
// Author           : grieger
// Created          : 03-09-2017
//
// Last Modified By : grieger
// Last Modified On : 04-03-2017
// ***********************************************************************
// <copyright file="UserHub.cs" company="Berenberg">
//     Copyright © Berenberg 2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatModule.Hubs
{
    /// <summary>
    /// Class UserHub.
    /// </summary>
    /// <seealso cref="Microsoft.AspNet.SignalR.Hub" />
    public class UserHub : Hub
    {
        //Enthlt den Status der Users
        /// <summary>
        /// Reals the time status.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="status">The status.</param>
        public void RealTimeStatus(int userId, string status)
        {
            //Sendet den Status des User an alle Clients
            Clients.All.getRealTimeStatus(userId, status);
        }

        public void Method()
        {
            throw new System.NotImplementedException();
        }
    }
}