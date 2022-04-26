using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace DemoCallWithParams
{
    public interface IChatClient
    {
        Task RecieveMessage(string user, string message);
    }
    public class DemoStronglyTypeHub:Hub<IChatClient>
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.All.RecieveMessage("meephoo ","hello");
            await base.OnConnectedAsync();
        }
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.RecieveMessage(user, message);
        }

        public Task SendMessageToCaller(string user, string message)
        {
            return Clients.Caller.RecieveMessage(user, message);
        }
    }
}