using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace Elias.Web.Hubs
{
    public class NotificationHub : BaseHub
    {
        public static void UpdateLeaveRequests()
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            hubContext.Clients.All.updateLeaveRequests();
        }
    }
}