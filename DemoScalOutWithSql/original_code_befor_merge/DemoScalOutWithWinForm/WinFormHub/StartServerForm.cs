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

using System.Data.SqlClient;
using System.Drawing.Drawing2D;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;
using WinFormHub.Models;
using Microsoft.AspNet.SignalR;

namespace WinFormHub
{
    // https://www.thaicreate.com/community/signalr-window-form.html
    // https://www.youtube.com/watch?v=5uXRrYE5q2s
    public partial class StartServerForm : Form
    {
        private IDisposable SignalR { get; set; }
        const string ServerURI = "http://localhost:8235";

        public SqlTableDependency<People> people_table_dependency;
        string connection_string_people = @"Data Source=suwet_phr2\SQL2014;Initial Catalog = MyTestdb; Persist Security Info=True;User ID = sa; Password=1234qwer";

        private IHubContext _context;

        public StartServerForm(IHubContext context)
        {
            _context = context;
            InitializeComponent();
        }
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
            Task.Run(() => 
            {
                bool success = start_people_table_dependency();
                if(success)
                {
                    ThreadSafe(() => richBoxResult.AppendText("\n" + "sql dependency change started \n"));
                }
                else
                {
                    ThreadSafe(() => richBoxResult.AppendText("\n" + "sql dependency change cannot start \n"));
                }
            });
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            Task.Run(() => stop_people_table_dependency());
            Close();
            
        }

        private void StartServerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(SignalR != null)
            {
                SignalR.Dispose();
                //Task.Run(() => stop_people_table_dependency());
            }
        }

        #region SqlDepency
        //common function 
        private void refresh_table()
        {
            string sql = "SELECT * FROM People";
            SqlConnection connection = new SqlConnection(connection_string_people);
            SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();
            connection.Open();
            dataadapter.Fill(ds, "People");
            connection.Close();
            //ThreadSafe(() => dataGridView1.DataSource = ds);
            //ThreadSafe(() => dataGridView1.DataMember = "People");
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

        // start , stop , error, changed 
        private bool start_people_table_dependency()
        {
            try
            {
                people_table_dependency = new SqlTableDependency<People>(connection_string_people);
                people_table_dependency.OnChanged += people_table_dependency_Changed;
                people_table_dependency.OnError += people_table_dependency_OnError;
                people_table_dependency.Start();
                return true;
            }
            catch (Exception ex)
            {

                WriteToConsole(ex.ToString());
            }
            return false;

        }
        private bool stop_people_table_dependency()
        {
            try
            {
                if (people_table_dependency != null)
                {
                    people_table_dependency.Stop();

                    return true;
                }
            }
            catch (Exception ex) { WriteToConsole(ex.ToString()); }

            return false;

        }
        private void people_table_dependency_OnError(object sender, ErrorEventArgs e)
        {
            WriteToConsole(e.Error.Message);
        }
        private void people_table_dependency_Changed(object sender, RecordChangedEventArgs<People> e)
        {
            try
            {
                var changedEntity = e.Entity;

                switch (e.ChangeType)
                {
                    case ChangeType.Insert:
                        {

                            WriteToConsole("Insert values:\tname:" + changedEntity.name.ToString() + "\tage:" + changedEntity.age.ToString());
                            // Todo
                            // SignalR hub call client
                            var hub = GlobalHost.ConnectionManager.GetHubContext<NotiHub>();

                            hub.Clients.All.addMessage("hub server send ", "Insert values:\tname:" + changedEntity.name.ToString() + "\tage:" + changedEntity.age.ToString());
                            //refresh_table();

                        }
                        break;

                    case ChangeType.Update:
                        {
                            WriteToConsole("Update values:\tname:" + changedEntity.name.ToString() + "\tage:" + changedEntity.age.ToString());
                            // Todo
                            // SignalR hub call client
                            var hub = GlobalHost.ConnectionManager.GetHubContext("NotiHub");
                            hub.Clients.All.addMessage("hub server send ", "Update values:\tname:" + changedEntity.name.ToString() + "\tage:" + changedEntity.age.ToString());
                        }
                        break;

                    case ChangeType.Delete:
                        {
                            WriteToConsole("Delete values:\tname:" + changedEntity.name.ToString() + "\tage:" + changedEntity.age.ToString());
                            // Todo
                            // SignalR hub call client
                            var hub = GlobalHost.ConnectionManager.GetHubContext("NotiHub");
                            hub.Clients.All.addMessage("hub server send ", "Delete values:\tname:" + changedEntity.name.ToString() + "\tage:" + changedEntity.age.ToString());
                        }
                        break;
                };

            }
            catch (Exception ex) { WriteToConsole(ex.ToString()); }

        }

        #endregion
    }
}
