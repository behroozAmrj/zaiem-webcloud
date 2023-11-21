using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Cloud.UI.Train.Hubs
{
    [HubName("Master")]
    public class Master : Hub
    {
        [HubMethodName("Hello")]
        public void Hello()
        {
            Clients.All.hello();
        }
    }
}