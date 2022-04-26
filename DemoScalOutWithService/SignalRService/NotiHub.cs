﻿

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using SignalRService.Models;

namespace SignalRService
{
    public interface INotificationTableChange
    {
        Task ReceiveTicketMessage(Ticket ticket, string action);

        Task ReceiveMatchTruckLoad(Loads ltm, string action);

    }
    public class NotiHub : Hub<INotificationTableChange>
    {

        public void Send(Ticket ticket, string action)
        {
            Clients.All.ReceiveTicketMessage(ticket, action);
        }

        public override Task OnConnected()
        {
            //Program.MainForm.WriteToConsole("Client connected :" + Context.ConnectionId);
            Debug.WriteLine("Client connected :" + Context.ConnectionId);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            //Program.MainForm.WriteToConsole("Client disconnected :" + Context.ConnectionId);
            return base.OnDisconnected(true);
        }
    }
}
