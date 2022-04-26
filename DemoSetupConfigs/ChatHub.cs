using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace DemoSetupConfigs
{
    public class ChatHub:Hub
    {
        // hub method that client can call. 
        public async Task SendMessage(string user, string message)
        {

            // client method that hub can call.
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}