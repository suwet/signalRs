using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace DemoCallWithParams
{
    public class ChatHub:Hub
    {
        public override async Task OnConnectedAsync()
        {
            string message = "Hello from hub";
            await Clients.All.SendAsync("demoClientMethod",message);
            await base.OnConnectedAsync();
        }
        /*
       public async Task<string> DemoCallFromClient(string name)
       {
           return await Task.FromResult("Hi client you send param to hun is "+name);
       }
       */
        
    }
}