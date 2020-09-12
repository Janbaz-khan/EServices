using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace EServices
{
    public class MyHub : Hub
    {
        public void Send(string name)
        {
          
            var host = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
            host.Clients.User(name).notify();
        }
    }
}