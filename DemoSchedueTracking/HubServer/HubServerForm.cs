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
        public SqlTableDependency<Loads> load_table_dependency;
        public SqlTableDependency<Trucks> truck_table_dependency;
        string path = System.Configuration.ConfigurationManager.AppSettings["log_path"];
        string connection_string = System.Configuration.ConfigurationManager.
                                    ConnectionStrings["Test"].ConnectionString;
        bool enable_log = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["enable_log"]);
        //@"Data Source=suwet_phr2\SQL2014;Initial Catalog = MyTestdb; Persist Security Info=True;User ID = sa; Password=1234qwer";

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

                load_table_dependency = new SqlTableDependency<Loads>(connection_string);
                load_table_dependency.OnChanged += Load_truck_match_table_dependency_OnChanged;
                load_table_dependency.OnError += Load_truck_match_table_dependency_OnError;
                load_table_dependency.Start();

                truck_table_dependency = new SqlTableDependency<Trucks>(connection_string);
                truck_table_dependency.OnChanged += Truck_table_dependency_OnChanged;
                truck_table_dependency.OnError += Truck_table_dependency_OnError;
                truck_table_dependency.Start();


                return true;
            }
            catch (Exception ex)
            {

                Task.Run(() => LogToFile(ex.ToString()));
            }
            return false;

        }

        private void Truck_table_dependency_OnError(object sender, ErrorEventArgs e)
        {
            //throw new NotImplementedException();
            Task.Run(() => LogToFile(e.Error.Message));
        }

        private  void Truck_table_dependency_OnChanged(object sender, RecordChangedEventArgs<Trucks> e)
        {
            try
            {
                var changedEntity = e.Entity;

                switch (e.ChangeType)
                {
                    case ChangeType.Insert:
                        {

                            Task.Run(() => LogToFile("Insert values: " + " \t TruckCode:" + changedEntity.TruckCode.ToString() + "on table " + nameof(Trucks)));
                            var hub = GlobalHost.ConnectionManager.GetHubContext("NotiHub");
                            hub.Clients.All.ReceiveTruckMessage(changedEntity, "Insert");
                        }
                        break;

                    case ChangeType.Update:
                        {
                            Task.Run(() => LogToFile("Update values: " + "\t TruckCode:" + changedEntity.TruckCode.ToString() + "on table " + nameof(Trucks)));
                            var hub = GlobalHost.ConnectionManager.GetHubContext("NotiHub");
                            hub.Clients.All.ReceiveTruckMessage(changedEntity, "Update");

                        }
                        break;

                    case ChangeType.Delete:
                        {
                            Task.Run(() => LogToFile("Delete values: " + "\t TruckCode:" + changedEntity.TruckCode.ToString() + "on table " + nameof(Trucks)));
                            var hub = GlobalHost.ConnectionManager.GetHubContext("NotiHub");
                            hub.Clients.All.ReceiveTruckMessage(changedEntity, "Delete");
                        }
                        break;
                };

            }
            catch (Exception ex) 
            {
                Task.Run(() => LogToFile(ex.ToString()));
            }
        }

        private void Load_truck_match_table_dependency_OnError(object sender, ErrorEventArgs e)
        {
            Task.Run(() => LogToFile(e.Error.Message));
        }

       

        private async Task <bool> stop_table_dependency()
        {
            try
            {
                if (ticket_table_dependency != null && load_table_dependency != null)
                {
                    ticket_table_dependency.Stop();
                    load_table_dependency.Stop();
                    truck_table_dependency.Stop();
                    return true;
                }
            }
            catch (Exception ex) 
            { 
                await LogToFile(ex.ToString());
            }

            return false;

        }
        private void ticket_table_dependency_OnError(object sender, ErrorEventArgs e)
        {
            Task.Run(() => LogToFile(e.Error.Message));
        }

        private void Load_truck_match_table_dependency_OnChanged(object sender, RecordChangedEventArgs<Loads> e)
        {
            try
            {
                var changedEntity = e.Entity;

                switch (e.ChangeType)
                {
                    case ChangeType.Insert:
                        {

                            Task.Run(() => LogToFile("Insert values:" + "\t SaleOrderNo:" + changedEntity.SaleOrderNo.ToString() + "on table " + nameof(Loads)));

                            var hub = GlobalHost.ConnectionManager.GetHubContext("NotiHub");
                            hub.Clients.All.ReceiveLoadMessage(changedEntity, "Insert");
                        }
                        break;

                    case ChangeType.Update:
                        {
                            Task.Run(() => LogToFile("Update values:" + "\t SaleOrderNo:" + changedEntity.SaleOrderNo.ToString() + "on table " + nameof(Loads)));
                            var hub = GlobalHost.ConnectionManager.GetHubContext("NotiHub");
                            hub.Clients.All.ReceiveLoadMessage(changedEntity, "Update");

                        }
                        break;

                    case ChangeType.Delete:
                        {
                            Task.Run(() => LogToFile("Delete values:" + "\t  SaleOrderNo:" + changedEntity.SaleOrderNo.ToString() + "on table " + nameof(Loads)));
                            var hub = GlobalHost.ConnectionManager.GetHubContext("NotiHub");
                            hub.Clients.All.ReceiveLoadMessage(changedEntity, "Delete");
                        }
                        break;
                };

            }
            catch (Exception ex) 
            {
                Task.Run(() => LogToFile(ex.ToString()));
            }
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

                            Task.Run(() => LogToFile("Insert values:" + "\t CustomerNo:" + changedEntity.CustomerNo.ToString() + "on table " + nameof(Ticket)));

                            var hub = GlobalHost.ConnectionManager.GetHubContext("NotiHub");
                            hub.Clients.All.ReceiveTicketMessage(changedEntity, "Insert");
                        }
                        break;

                    case ChangeType.Update:
                        {
                            Task.Run(() => LogToFile("Update values:" + "\t CustomerNo:" + changedEntity.CustomerNo.ToString() + "on table " + nameof(Ticket)));
                            var hub = GlobalHost.ConnectionManager.GetHubContext("NotiHub");
                            hub.Clients.All.ReceiveTicketMessage(changedEntity,"Update");

                        }
                        break;

                    case ChangeType.Delete:
                        {
                            Task.Run(() => LogToFile("Delete values:" + "\t CustomerNo:" + changedEntity.CustomerNo.ToString() + "on table " + nameof(Ticket)));
                            var hub = GlobalHost.ConnectionManager.GetHubContext("NotiHub");
                            hub.Clients.All.ReceiveTicketMessage(changedEntity,"Delete");
                        }
                        break;
                };

            }
            catch (Exception ex) 
            {
                Task.Run(() => LogToFile(ex.ToString()));
            }

        }

        public async Task LogToFile(string logText)
        {
            // logText += DateTime.Now.ToString() + Environment.NewLine + logText;
            //ThreadSafe(() => richTextBox1.AppendText(DateTime.Now.ToString("HH:mm:ss:fff") + "\t" + logText + Environment.NewLine));
            //System.IO.File.AppendAllText(Application.StartupPath + "\\log.txt", logText);

            if(enable_log)
            {
                string filename = DateTime.Now.ToString("yyyy_MM_dd") + ".log";
                await 
                Task.Factory.StartNew(async () =>
                {
                    await WriteLogThreadSafe.WriteToFileThreadSafe(DateTime.Now.ToString("hh:mm:ss.fff tt") + "\t" + logText + Environment.NewLine, path + filename);
                });
            }
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
                {
                    //Invoke(method);
                    BeginInvoke(method); // async
                }
                   
                else
                {
                    method();
                }
                    
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
