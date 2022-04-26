using HubServer.Models;
using Microsoft.AspNet.SignalR;
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
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;

namespace HubServer
{
    public partial class HubServerForm : Form
    {
        public SqlTableDependency<Ticket> ticket_table_dependency;
        public SqlTableDependency<LoadTruckMatch> load_truck_match_table_dependency;

        string connection_string = @"Data Source=suwet_phr2\SQL2014;Initial Catalog = MyTestdb; Persist Security Info=True;User ID = sa; Password=1234qwer";

        private IDisposable SignalR { get; set; }
        const string ServerURI = "http://localhost:8235";
        public HubServerForm()
        {
            InitializeComponent();
        }

        // start , stop , error, changed 
        private bool start_table_dependency()
        {
            try
            {
                ticket_table_dependency = new SqlTableDependency<Ticket>(connection_string);
                ticket_table_dependency.OnChanged += ticket_table_dependency_Changed;
                ticket_table_dependency.OnError += ticket_table_dependency_OnError;
                ticket_table_dependency.Start();

                load_truck_match_table_dependency = new SqlTableDependency<LoadTruckMatch>(connection_string);
                load_truck_match_table_dependency.OnChanged += Load_truck_match_table_dependency_OnChanged;
                load_truck_match_table_dependency.OnError += Load_truck_match_table_dependency_OnError;
                load_truck_match_table_dependency.Start();
               
                return true;
            }
            catch (Exception ex)
            {

                log_file(ex.ToString());
            }
            return false;

        }

        private void Load_truck_match_table_dependency_OnError(object sender, ErrorEventArgs e)
        {
            log_file(e.Error.Message);
        }

       

        private bool stop_table_dependency()
        {
            try
            {
                if (ticket_table_dependency != null && load_truck_match_table_dependency != null)
                {
                    ticket_table_dependency.Stop();
                    load_truck_match_table_dependency.Stop();
                    return true;
                }
            }
            catch (Exception ex) { log_file(ex.ToString()); }

            return false;

        }
        private void ticket_table_dependency_OnError(object sender, ErrorEventArgs e)
        {
            log_file(e.Error.Message);
        }

        private void Load_truck_match_table_dependency_OnChanged(object sender, RecordChangedEventArgs<LoadTruckMatch> e)
        {
            try
            {
                var changedEntity = e.Entity;

                switch (e.ChangeType)
                {
                    case ChangeType.Insert:
                        {

                            log_file("Insert values:\tname:" + changedEntity.SaleOrderNo.ToString() + "\tage:" + changedEntity.SaleOrderNo.ToString());

                            var hub = GlobalHost.ConnectionManager.GetHubContext("NotiHub");
                            hub.Clients.All.ReceiveMatchTruckLoad(changedEntity, "Insert");
                        }
                        break;

                    case ChangeType.Update:
                        {
                            log_file("Update values:\tname:" + changedEntity.SaleOrderNo.ToString() + "\tage:" + changedEntity.SaleOrderNo.ToString());
                            var hub = GlobalHost.ConnectionManager.GetHubContext("NotiHub");
                            hub.Clients.All.ReceiveMatchTruckLoad(changedEntity, "Update");

                        }
                        break;

                    case ChangeType.Delete:
                        {
                            log_file("Delete values:\tname:" + changedEntity.SaleOrderNo.ToString() + "\tage:" + changedEntity.SaleOrderNo.ToString());
                            var hub = GlobalHost.ConnectionManager.GetHubContext("NotiHub");
                            hub.Clients.All.ReceiveMatchTruckLoad(changedEntity, "Delete");
                        }
                        break;
                };

            }
            catch (Exception ex) { log_file(ex.ToString()); }
        }
        private void ticket_table_dependency_Changed(object sender, RecordChangedEventArgs<Ticket> e)
        {
            try
            {
                var changedEntity = e.Entity;

                switch (e.ChangeType)
                {
                    case ChangeType.Insert:
                        {

                            log_file("Insert values:\tname:" + changedEntity.CustomerNo.ToString() + "\tage:" + changedEntity.CustomerNo.ToString());

                            var hub = GlobalHost.ConnectionManager.GetHubContext("NotiHub");
                            hub.Clients.All.ReceiveTicketMessage(changedEntity, "Insert");
                        }
                        break;

                    case ChangeType.Update:
                        {
                            log_file("Update values:\tname:" + changedEntity.CustomerNo.ToString() + "\tage:" + changedEntity.CustomerNo.ToString());
                            var hub = GlobalHost.ConnectionManager.GetHubContext("NotiHub");
                            hub.Clients.All.ReceiveTicketMessage(changedEntity,"Update");

                        }
                        break;

                    case ChangeType.Delete:
                        {
                            log_file("Delete values:\tname:" + changedEntity.CustomerNo.ToString() + "\tage:" + changedEntity.CustomerNo.ToString());
                            var hub = GlobalHost.ConnectionManager.GetHubContext("NotiHub");
                            hub.Clients.All.ReceiveTicketMessage(changedEntity,"Delete");
                        }
                        break;
                };

            }
            catch (Exception ex) { log_file(ex.ToString()); }

        }

        public void log_file(string logText)
        {
            // logText += DateTime.Now.ToString() + Environment.NewLine + logText;
            //ThreadSafe(() => richTextBox1.AppendText(DateTime.Now.ToString("HH:mm:ss:fff") + "\t" + logText + Environment.NewLine));
            System.IO.File.AppendAllText(Application.StartupPath + "\\log.txt", logText);

        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            btn_start.Enabled = false;
            Task.Run(() => GoStartServer());
            Task.Run(() =>
            {
                bool success = start_table_dependency();
                if (success)
                {
                    ThreadSafe(() => richBoxResult.AppendText("\n" + "sql dependency change started \n"));
                }
                else
                {
                    ThreadSafe(() => richBoxResult.AppendText("\n" + "sql dependency change cannot start \n"));
                }
            });
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

        private void ThreadSafe(MethodInvoker method)
        {
            try
            {
                if (InvokeRequired)
                    Invoke(method);
                else
                    method();
            }
            catch (ObjectDisposedException) { }
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
        private void btn_stop_Click(object sender, EventArgs e)
        {
            Task.Run(() => stop_table_dependency());
            Close();
        }
    }
}
