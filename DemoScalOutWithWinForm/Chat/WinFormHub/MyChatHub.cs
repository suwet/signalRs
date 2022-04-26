using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormHub
{
    public class MyChatHub:Hub
    {
        public void Send(string name,string message)
        {
            Clients.All.addMessage(name, message);
        }

        public override Task OnConnected()
        {
            Program.MainForm.WriteToConsole("Client connected :" + Context.ConnectionId);
            //Debug.WriteLine("Client connected :" + Context.ConnectionId);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Program.MainForm.WriteToConsole("Client disconnected :" + Context.ConnectionId);
            return base.OnDisconnected(true);
        }
    }
}
