using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;
using TableMonitor.Class;

namespace TableMonitor.Forms
{
    public partial class FrmHome : Form
    {
        public SqlTableDependency<People> people_table_dependency;
        string connection_string_people = @"Data Source=suwet_phr2\SQL2014;Initial Catalog = MyTestdb; Persist Security Info=True;User ID = sa; Password=1234qwer";


        public FrmHome()
        {
            InitializeComponent();
        }

        private void FrmHome_Load(object sender, EventArgs e)
        {
            //form load event
            refresh_table();
            start_people_table_dependency();
        }

        private void FrmHome_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                stop_people_table_dependency();
            }
            catch (Exception ex) { log_file(ex.ToString()); }
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
            ThreadSafe(() => dataGridView1.DataSource = ds);
            ThreadSafe(() => dataGridView1.DataMember = "People");
        }
        public void log_file(string logText)
        {
            // logText += DateTime.Now.ToString() + Environment.NewLine + logText;
            ThreadSafe(() => richTextBox1.AppendText(DateTime.Now.ToString("HH:mm:ss:fff") + "\t" + logText + Environment.NewLine));
            System.IO.File.AppendAllText(Application.StartupPath + "\\log.txt", logText);

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

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex == -1)
            {
                //Color c1 = Color.FromArgb(255, 54, 54, 54);
                //Color c2 = Color.FromArgb(255, 62, 62, 62);
                //Color c3 = Color.FromArgb(255, 98, 98, 98);

                Color c1 = Color.FromArgb(21, 66, 139);
                Color c2 = Color.FromArgb(21, 76, 76);
                Color c3 = Color.FromArgb(21, 98, 98);                

                LinearGradientBrush br = new LinearGradientBrush(e.CellBounds, c1, c3, 90, true);
                ColorBlend cb = new ColorBlend();
                cb.Positions = new[] { 0, (float)0.5, 1 };
                cb.Colors = new[] { c1, c2, c3 };
                br.InterpolationColors = cb;




               
                e.Graphics.FillRectangle(Brushes.Aqua, e.CellBounds);
                e.PaintContent(e.ClipBounds);
                e.Handled = true;
            }
        }
    }
}
