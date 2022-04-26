
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;
using Microsoft.AspNet.SignalR;
using SignalRService.Models;
using Microsoft.Owin.Hosting;
using SignalRService.Helper;
using System.IO;

namespace SignalRService
{
    public  class HubServer
    {
        private SqlTableDependency<Ticket> ticket_table_dependency;
        private SqlTableDependency<Loads> load_truck_match_table_dependency;

        string connection_string = @"Data Source=suwet_phr2\SQL2014;Initial Catalog = MyTestdb; Persist Security Info=True;User ID = sa; Password=1234qwer";

        private IDisposable SignalR { get; set; }
        const string ServerURI = "http://localhost:8236";



        // start , stop , error, changed 
        public bool Start_Table_Dependency()
        {
            try
            {
                ticket_table_dependency = new SqlTableDependency<Ticket>(connection_string);
                ticket_table_dependency.OnChanged += ticket_table_dependency_Changed;
                ticket_table_dependency.OnError += ticket_table_dependency_OnError;
                ticket_table_dependency.Start();

                load_truck_match_table_dependency = new SqlTableDependency<Loads>(connection_string);
                load_truck_match_table_dependency.OnChanged += Load_truck_match_table_dependency_OnChanged;
                load_truck_match_table_dependency.OnError += Load_truck_match_table_dependency_OnError;
                load_truck_match_table_dependency.Start();

                return true;
            }
            catch (Exception ex)
            {

                //Log_To_File(ex.ToString());
                WriteLog.WriteWindowsEventLog("Start_Table_Dependency exception" + ex.Message);
            }
            return false;

        }

        private void Load_truck_match_table_dependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            //Log_To_File(e.Error.Message);
            WriteLog.WriteWindowsEventLog(e.Message);
        }



        public bool Stop_Table_Dependency()
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
            catch (Exception ex) { WriteLog.WriteWindowsEventLog(ex.Message); }

            return false;

        }
        private void ticket_table_dependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            //Log_To_File(e.Error.Message);
            WriteLog.WriteWindowsEventLog(e.Error.Message);
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

                            //Log_To_File("Insert values:\tname:" + changedEntity.SaleOrderNo.ToString() + "\tage:" + changedEntity.SaleOrderNo.ToString());
                            WriteLog.WriteWindowsEventLog("Insert values:\tname:" + changedEntity.SaleOrderNo.ToString() + "\tage:" + changedEntity.SaleOrderNo.ToString());
                            var hub = GlobalHost.ConnectionManager.GetHubContext("NotiHub");
                            hub.Clients.All.ReceiveMatchTruckLoad(changedEntity, "Insert");
                        }
                        break;

                    case ChangeType.Update:
                        {
                            WriteLog.WriteWindowsEventLog("Update values:\tname:" + changedEntity.SaleOrderNo.ToString() + "\tage:" + changedEntity.SaleOrderNo.ToString());
                            var hub = GlobalHost.ConnectionManager.GetHubContext("NotiHub");
                            hub.Clients.All.ReceiveMatchTruckLoad(changedEntity, "Update");

                        }
                        break;

                    case ChangeType.Delete:
                        {
                            WriteLog.WriteWindowsEventLog("Delete values:\tname:" + changedEntity.SaleOrderNo.ToString() + "\tage:" + changedEntity.SaleOrderNo.ToString());
                            var hub = GlobalHost.ConnectionManager.GetHubContext("NotiHub");
                            hub.Clients.All.ReceiveMatchTruckLoad(changedEntity, "Delete");
                        }
                        break;
                };

            }
            catch (Exception ex) { WriteLog.WriteWindowsEventLog(ex.ToString()); }
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

                            WriteLog.WriteWindowsEventLog("Insert values:\tname:" + changedEntity.CustomerNo.ToString() + "\tage:" + changedEntity.CustomerNo.ToString());

                            var hub = GlobalHost.ConnectionManager.GetHubContext("NotiHub");
                            hub.Clients.All.ReceiveTicketMessage(changedEntity, "Insert");
                        }
                        break;

                    case ChangeType.Update:
                        {
                            WriteLog.WriteWindowsEventLog("Update values:\tname:" + changedEntity.CustomerNo.ToString() + "\tage:" + changedEntity.CustomerNo.ToString());
                            var hub = GlobalHost.ConnectionManager.GetHubContext("NotiHub");
                            hub.Clients.All.ReceiveTicketMessage(changedEntity, "Update");

                        }
                        break;

                    case ChangeType.Delete:
                        {
                            WriteLog.WriteWindowsEventLog("Delete values:\tname:" + changedEntity.CustomerNo.ToString() + "\tage:" + changedEntity.CustomerNo.ToString());
                            var hub = GlobalHost.ConnectionManager.GetHubContext("NotiHub");
                            hub.Clients.All.ReceiveTicketMessage(changedEntity, "Delete");
                        }
                        break;
                };

            }
            catch (Exception ex) { WriteLog.WriteWindowsEventLog(ex.ToString()); }

        }



        public void  Log_To_File(string logText)
        {
            // logText += DateTime.Now.ToString() + Environment.NewLine + logText;
            //ThreadSafe(() => richTextBox1.AppendText(DateTime.Now.ToString("HH:mm:ss:fff") + "\t" + logText + Environment.NewLine));
            //string logDate = DateTime.Now.ToString("yyyyMMdd", new System.Globalization.CultureInfo("en-US"));
             
                Task.Factory.StartNew(() => 
            {
                string path = System.IO.Directory.GetCurrentDirectory();
                string filename = DateTime.Now.ToString("yyyyMMdd", new System.Globalization.CultureInfo("en-US")) + ".txt";
                string pre_connent = DateTime.Now.ToString("yyyy:MM:dd HH:mm:ss") + " : ";
                try
                {
                    using (StreamWriter wr = System.IO.File.AppendText(path + "//" + filename))
                    {
                        var w = StreamWriter.Synchronized(wr);
                        w.WriteLine(pre_connent + " " + logText);
                        w.Close();
                    }
                }
                catch (Exception ex)
                {
                    //System.IO.File.AppendAllText(path + "//" + filename, pre_connent + " " + logText + "error is " + ex.Message);
                    throw;
                }
            });
        }

        public void StartSignalRServer()
        {
            try
            {
                SignalR = WebApp.Start(ServerURI.Trim());
            }
            catch (TargetInvocationException ex)
            {
                WriteLog.WriteWindowsEventLog("Server failed to start. A server is already running on " + ServerURI + "  " + ex.Message);
                //Action ac_start = () => btn_start.Enabled = true;
                //WriteToConsole("Server failed to start. A server is already running on " + ServerURI);
                //this.Invoke(ac_start);
                return;
            }
            //Action ac_stop = () => btn_stop.Enabled = true;
            //this.Invoke(ac_stop);
            //WriteToConsole("Server started at " + ServerURI);
            WriteLog.WriteWindowsEventLog("Server started at " + ServerURI);
        }
    }
}
