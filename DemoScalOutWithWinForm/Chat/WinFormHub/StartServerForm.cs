using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormHub
{
    // https://www.thaicreate.com/community/signalr-window-form.html
    public partial class StartServerForm : Form
    {
        private IDisposable SignalR { get; set; }
        const string ServerURI = "http://localhost:8235";
        public StartServerForm()
        {
            InitializeComponent();
        }

        private void GoStartServer()
        {
            try
            {
                SignalR = WebApp.Start(ServerURI.Trim());
            }
            catch (TargetInvocationException ex)
            {
                Action ac_start = () => btn_start.Enabled = true;
                WriteToConsole("Server failed to start. A server is already running on " + ServerURI);
                this.Invoke(ac_start);
                return;
            }
            Action ac_stop = () => btn_stop.Enabled = true;
            this.Invoke(ac_stop);
            WriteToConsole("Server started at " + ServerURI);
        }

        internal void WriteToConsole(string msg)
        {
            Action ac = () => WriteToConsole(msg);

            if (richBoxResult.InvokeRequired)
            {
                this.Invoke(ac);
                return;
            }
            richBoxResult.AppendText(msg + Environment.NewLine);
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            WriteToConsole("Starting server... ");
            btn_start.Enabled = false;
            Task.Run(() => GoStartServer());
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void StartServerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(SignalR != null)
            {
                SignalR.Dispose();
            }
        }
    }
}
