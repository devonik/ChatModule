using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatModule.Hubs
{
    public class UserHub : Hub
    {
        public void RealTimeStatus(int userId, string status)
        {

            Clients.All.getRealTimeStatus(userId, status);
        }
    }
}