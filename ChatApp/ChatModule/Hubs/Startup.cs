// ***********************************************************************
// Assembly         : ChatModule
// Author           : grieger
// Created          : 03-09-2017
//
// Last Modified By : grieger
// Last Modified On : 03-09-2017
// ***********************************************************************
// <copyright file="Startup.cs" company="Berenberg">
//     Copyright © Berenberg 2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using Owin;
using Microsoft.Owin;
[assembly: OwinStartup(typeof(SignalRChat.Startup))]
namespace SignalRChat
{
    /// <summary>
    /// Class Startup.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configurations the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        public void Configuration(IAppBuilder app)
        {

            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
        }

        public void Method()
        {
            throw new System.NotImplementedException();
        }
    }
}