using DemoGridControl1.Model;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;

namespace DemoGridControl1
{
    public partial class XtraForm1 : DevExpress.XtraEditors.XtraForm
    {
        public SqlTableDependency<People> people_table_dependency;
        string connection_string_people = @"Data Source=suwet_phr2\SQL2014;Initial Catalog = MyTestdb; Persist Security Info=True;User ID = sa; Password=1234qwer";
        public XtraForm1()
        {
            InitializeComponent();
        }

        private void refresh_table()
        {
            string sql = "SELECT * FROM People";
            SqlConnection connection = new SqlConnection(connection_string_people);
            SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();
            connection.Open();
            dataadapter.Fill(ds, "People");
            connection.Close();
            ThreadSafe(() => gridControl1.DataSource = ds);
            ThreadSafe(() => gridControl1.DataMember = "People");
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

                log_file(ex.ToString());
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
            catch (Exception ex) { log_file(ex.ToString()); }

            return false;

        }
        private void people_table_dependency_OnError(object sender, ErrorEventArgs e)
        {
            log_file(e.Error.Message);
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

                            log_file("Insert values:\tname:" + changedEntity.name.ToString() + "\tage:" + changedEntity.age.ToString());
                            refresh_table();

                        }
                        break;

                    case ChangeType.Update:
                        {
                            log_file("Update values:\tname:" + changedEntity.name.ToString() + "\tage:" + changedEntity.age.ToString());
                            refresh_table();

                        }
                        break;

                    case ChangeType.Delete:
                        {
                            log_file("Delete values:\tname:" + changedEntity.name.ToString() + "\tage:" + changedEntity.age.ToString());
                            refresh_table();
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

        private void XtraForm1_Load(object sender, EventArgs e)
        {
            refresh_table();
            start_people_table_dependency();
        }
    }
}