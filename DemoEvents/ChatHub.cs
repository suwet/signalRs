using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace DemoEvents
{
    public class ChatHub:Hub
    {
        public override async Task OnConnectedAsync()
        {
            Debug.WriteLine("OnConnectedAsync event");
            await base.OnConnectedAsync();
        }
      
        public async Task Apple()
        {
            await Task.Delay(10);
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            Debug.WriteLine("OnDisconnectedAsync event");
            await base.OnDisconnectedAsync(exception);
        }
    }
}