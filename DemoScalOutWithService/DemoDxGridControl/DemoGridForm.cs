using DemoDxGridControl.Dialogs;
using DemoDxGridControl.Helper;
using DemoDxGridControl.Models;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;

namespace DemoDxGridControl
{
    public partial class DemoGridForm : Form
    {
        private RepositoryItemButtonEdit repositoryItemTicketButtonEdit = new RepositoryItemButtonEdit();
        private RepositoryItemButtonEdit repositoryItemTicketButtonDelete = new RepositoryItemButtonEdit();

        private RepositoryItemButtonEdit repositoryItemLoadTruckMatchButtonEdit = new RepositoryItemButtonEdit();
        private RepositoryItemButtonEdit repositoryItemLoadTruckMatchButtonDelete = new RepositoryItemButtonEdit();
        private string username { get; set; }
        private IHubProxy hubproxy { get; set; }
        const string ServerURL = "http://localhost:8236/signalr";
        private HubConnection connection { get; set; }
        private string log_path = ConfigurationManager.AppSettings["logpath"];
        string connection_string =
                                    System.Configuration.ConfigurationManager.
                                    ConnectionStrings["Test"].ConnectionString;

        public DemoGridForm()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Task.Run(async () =>
            {
                ThreadSafe(() => refresh_tricket_table());
                ThreadSafe(() => refresh_load_truck_table());
                await ConnectAsync();
            });
        }
        private async Task ConnectAsync()
        {
            connection = new HubConnection(ServerURL.Trim());
            connection.Closed += Connection_Closed;
            hubproxy = connection.CreateHubProxy("NotiHub");

            hubproxy.On<Ticket, string>("ReceiveTicketMessage", (entity, action_name) 
                                      =>
            {
                ThreadSafe(() => 
                {
                    WriteLog.WriteLogToFile(log_path,entity.CustomerNo + "  " + action_name + "\n");
                });
                
                ThreadSafe(() => 
                {
                    refresh_tricket_table();
                });
            });

            hubproxy.On<Loads, string>("ReceiveMatchTruckLoad", (entity, action_name)
                                      =>
            {
                ThreadSafe(() =>
                {
                    WriteLog.WriteLogToFile(log_path,entity.SaleOrderNo + "  " + action_name + "\n");
                });

                ThreadSafe(() =>
                {
                    refresh_load_truck_table();
                });
            });

            try
            {
                await connection.Start();
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show("Cannot connect to signalR hub", "Warring", MessageBoxButtons.OK);
                WriteLog.WriteLogToFile(log_path,"Unable to connect to server : Start server before connectiong clients.");
                return;

            }
            
        }

        private void Connection_Closed()
        {
           // this.Invoke((Action)(() => btn_send.Enabled = false));
            this.Invoke((Action)(() => WriteLog.WriteLogToFile(log_path,"You have been disconnected.")));
        }

       

        //common function 
        private void refresh_tricket_table()
        {
            string sql = "SELECT * FROM Ticket";
            SqlConnection connection = new SqlConnection(connection_string);
            SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();
            connection.Open();
            dataadapter.Fill(ds, "Ticket");
            connection.Close();
            ThreadSafe(() => ticket_grid.DataSource = ds);
            ThreadSafe(() => ticket_grid.DataMember = "Ticket");
            ThreadSafe(() => 
            {
                gridView1.CustomRowCellEdit += GridView1_CustomRowCellEdit;
                gridView1.CustomUnboundColumnData += GridView1_CustomUnboundColumnData;
                if (ticket_grid.RepositoryItems.Contains(repositoryItemTicketButtonEdit) == false)
                {
                    gridView1.OptionsBehavior.ReadOnly = true;
                   
                    GridColumn gridColumn = gridView1.Columns.AddVisible("EditButton", "");
                    //gridColumn.UnboundDataType = typeof(DevExpress.Data.);
                    //repositoryItemTicketButtonEdit.NullText = "Edit";
                    repositoryItemTicketButtonEdit.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
                    repositoryItemTicketButtonEdit.Buttons[0].Caption = "Edit";
                    repositoryItemTicketButtonEdit.Buttons[0].Kind = ButtonPredefines.Glyph;
                    repositoryItemTicketButtonEdit.ButtonClick += RepositoryItemButtonEdit_ButtonClick;
                    ticket_grid.RepositoryItems.Add(repositoryItemTicketButtonEdit);
                    gridColumn.ColumnEdit = repositoryItemTicketButtonEdit;
                    gridColumn.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
                }

                if (ticket_grid.RepositoryItems.Contains(repositoryItemTicketButtonDelete) == false)
                {
                    gridView1.OptionsBehavior.ReadOnly = true;
                    GridColumn gridColumn = gridView1.Columns.AddVisible("DeleteButton", "Delete");
                    repositoryItemTicketButtonDelete.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
                    repositoryItemTicketButtonDelete.Buttons[0].Caption = "Delete";
                    repositoryItemTicketButtonDelete.Buttons[0].Kind = ButtonPredefines.Glyph;
                    repositoryItemTicketButtonDelete.ButtonClick += RepositoryItemButtonDelete_ButtonClick;
                    ticket_grid.RepositoryItems.Add(repositoryItemTicketButtonDelete);
                    gridColumn.ColumnEdit = repositoryItemTicketButtonDelete;
                    gridColumn.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
                }
                    
            });
        }

        private void GridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {

        }

        private void GridView1_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            //throw new NotImplementedException();
            GridView view = sender as GridView;
            //if (e.Column.FieldName == "EditButton" && e.RowHandle == gridView1.FocusedRowHandle)
            //    e.RepositoryItem = spinEdit;
        }

        private void refresh_load_truck_table()
        {
            string sql = "SELECT * FROM Loads";
            SqlConnection connection = new SqlConnection(connection_string);
            SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();
            connection.Open();
            dataadapter.Fill(ds, "Loads");
            connection.Close();
            ThreadSafe(() => truck_load_grid.DataSource = ds);
            ThreadSafe(() => truck_load_grid.DataMember = "Loads");

            ThreadSafe(() =>
            {
                if (truck_load_grid.RepositoryItems.Contains(repositoryItemLoadTruckMatchButtonEdit) == false)
                {
                    gridView2.OptionsBehavior.ReadOnly = true;
                    GridColumn gridColumn = gridView2.Columns.AddVisible("EditButton", "Edit");
                    repositoryItemLoadTruckMatchButtonEdit.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
                    repositoryItemLoadTruckMatchButtonEdit.Buttons[0].Caption = "Edit";
                    repositoryItemLoadTruckMatchButtonEdit.Buttons[0].Kind = ButtonPredefines.Glyph;
                    repositoryItemLoadTruckMatchButtonEdit.ButtonClick += RepositoryItemLoadTruckMatchButtonEdit_ButtonClick;
                    truck_load_grid.RepositoryItems.Add(repositoryItemLoadTruckMatchButtonEdit);
                    gridColumn.ColumnEdit = repositoryItemLoadTruckMatchButtonEdit;
                    gridColumn.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.Default;
                }

                if (truck_load_grid.RepositoryItems.Contains(repositoryItemLoadTruckMatchButtonDelete) == false)
                {
                    gridView2.OptionsBehavior.ReadOnly = true;
                    GridColumn gridColumn = gridView2.Columns.AddVisible("DeleteButton", "Delete");
                    repositoryItemLoadTruckMatchButtonDelete.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
                    repositoryItemLoadTruckMatchButtonDelete.Buttons[0].Caption = "Delete";
                    repositoryItemLoadTruckMatchButtonDelete.Buttons[0].Kind = ButtonPredefines.Glyph;
                    repositoryItemLoadTruckMatchButtonDelete.ButtonClick += RepositoryItemLoadTruckMatchButtonDelete_ButtonClick;
                    truck_load_grid.RepositoryItems.Add(repositoryItemLoadTruckMatchButtonDelete);
                    gridColumn.ColumnEdit = repositoryItemLoadTruckMatchButtonDelete;
                    gridColumn.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.Default;
                }

            });
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

        private void btn_new_truck_load_Click(object sender, EventArgs e)
        {
            AddLoadTruck frm = new AddLoadTruck();
            frm.ShowDialog();
        }

        private void btn_add_new_ticket_Click(object sender, EventArgs e)
        {
            AddTricket frm = new AddTricket();
            frm.ShowDialog();
        }

        private void RepositoryItemLoadTruckMatchButtonDelete_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int id = (int)gridView2.GetFocusedRowCellValue("Id");
            //EditLoadTruck frm = new EditLoadTruck(id);
            //frm.Show();
            //MessageBox.Show("you select id " + id);

            if (MessageBox.Show("Do you want to delete ?", "Do you want to delete ", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    string sql = "delete from Loads where Id=@Id";
                    using (SqlConnection connection = new SqlConnection(connection_string))
                    {
                        if (connection.State != System.Data.ConnectionState.Open)
                            connection.Open();

                        using (SqlCommand cmd = new SqlCommand(sql, connection))
                        {
                            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                            cmd.CommandType = CommandType.Text;
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("error " + ex.Message);
                    return;
                }
            }
        }

        private void RepositoryItemLoadTruckMatchButtonEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int id = (int)gridView2.GetFocusedRowCellValue("Id");
            EditLoadTruck frm = new EditLoadTruck(id);
            frm.Show();
            //MessageBox.Show("you select id "+id);
        }

        private void RepositoryItemButtonEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int id = (int)gridView1.GetFocusedRowCellValue("Id");
            EditTricket frm = new EditTricket(id);
            frm.Show();
            //MessageBox.Show("you select id "+id);
        }

        private void RepositoryItemButtonDelete_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int id = (int)gridView1.GetFocusedRowCellValue("Id");
            //EditTricket frm = new EditTricket(id);
            //frm.Show();
            //MessageBox.Show("you select id "+id);
            if (MessageBox.Show("Do you want to delete ?", "Do you want to delete ",MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    string sql = "delete from ticket where Id=@Id";
                    using (SqlConnection connection = new SqlConnection(connection_string))
                    {
                        if (connection.State != System.Data.ConnectionState.Open)
                            connection.Open();

                        using (SqlCommand cmd = new SqlCommand(sql, connection))
                        {
                            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                           
                            cmd.CommandType = CommandType.Text;
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("error " + ex.Message);
                    return;
                }
            }
        }
    }
}
