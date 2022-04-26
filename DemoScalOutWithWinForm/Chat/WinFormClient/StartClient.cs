using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormClient
{
    //https://www.thaicreate.com/community/signalr-window-form.html
    public partial class StartClient : Form
    {
        private string username { get; set; }
        private IHubProxy hubproxy { get; set; }
        const string ServerURL = "http://localhost:8235/signalr";
        private HubConnection connection { get; set; }

        public StartClient()
        {

            InitializeComponent();
        }

        private async void btn_connect_Click(object sender, EventArgs e)
        {
            username = txt_user.Text;
            if(!string.IsNullOrEmpty(username))
            {
                lbl_status.Text = "Connecting to hub ...";
                lbl_status.Visible = true;
                await ConnectAsync();
            }
        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            hubproxy.Invoke("Send", username, txt_message.Text);

            txt_message.Text = String.Empty;

            txt_message.Focus();
        }

        private async Task ConnectAsync()
        {
            connection = new HubConnection(ServerURL.Trim());
            connection.Closed += Connection_Closed;
            hubproxy = connection.CreateHubProxy("MyChatHub");
            hubproxy.On<string,string>("AddMessage",(name,message) 
                                    => 
                                    {
                                        this.Invoke((Action)(() => rich_result.AppendText(name + message+"\n")));
                                    });
            try
            {
                await connection.Start();
            }
            catch (HttpRequestException ex)
            {
                lbl_status.Text = "Unable to connect to server : Start server before connectiong clients.";
                return;

            }
            btn_send.Enabled = true;
            txt_message.Enabled = true;
            txt_message.Focus();
            txt_user.Enabled = false;
            rich_result.AppendText("Connectd to server at " + ServerURL + "\n");
        }

        private void Connection_Closed()
        {
            this.Invoke((Action)(() => btn_send.Enabled = false));
            this.Invoke((Action)(() => lbl_status.Text = "You have been disconnected."));
        }
    }
}
