using DemoDxGridControl.Dialogs;
using DemoDxGridControl.Helper;
using DemoDxGridControl.MockData;
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
        private RepositoryItemButtonEdit repositoryItemTicketButtonStatus = new RepositoryItemButtonEdit();

        private string username { get; set; }
        private IHubProxy hubproxy { get; set; }
        const string ServerURL = "http://localhost:8235/signalr";
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
                ThreadSafe(() => refresh_truck_table());
                await Task.Delay(10);
                //await ConnectAsync();
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

            hubproxy.On<LoadTruckMatch, string>("ReceiveMatchTruckLoad", (entity, action_name)
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
            //var tickets = TicketMock.GetEmptyTickets();
            var tickets = TicketMock.GetTickets();
            ThreadSafe(() =>
            {
                gridView1.CustomRowCellEdit += GridView1_CustomRowCellEdit;
                gridView1.CustomUnboundColumnData += GridView1_CustomUnboundColumnData;
                gridView1.RowUpdated += GridView1_RowUpdated;
                gridView1.RowLoaded += GridView1_RowLoaded;

                ticket_grid.DataSource = tickets;

                if (ticket_grid.RepositoryItems.Contains(repositoryItemTicketButtonStatus) == false)
                {
                    gridView1.OptionsBehavior.ReadOnly = true;

                    GridColumn gridColumn = gridView1.Columns.AddVisible("ButtonStatus");
                    //gridColumn.UnboundDataType = typeof(DevExpress.Data.);
                    //repositoryItemTicketButtonEdit.NullText = "Edit";
                    repositoryItemTicketButtonStatus.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
                    repositoryItemTicketButtonStatus.Buttons[0].Caption = "Preparing";
                    repositoryItemTicketButtonStatus.Buttons[0].Kind = ButtonPredefines.Glyph;
                    repositoryItemTicketButtonStatus.ButtonClick += RepositoryItemButtonStatus_ButtonClick;
                    ticket_grid.RepositoryItems.Add(repositoryItemTicketButtonStatus);
                    gridColumn.ColumnEdit = repositoryItemTicketButtonStatus;
                    gridColumn.Caption = "";
                    gridColumn.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
                } 
            }
            );

            ThreadSafe(() =>
            {
                gridView1.Columns["Id"].Visible = false;
                gridView1.Columns["ButtonStatus"].VisibleIndex = 0;
                gridView1.Columns["Load_Status"].Visible = false;

                gridView1.Columns["TruckCode"].Caption = "รหัสรถโม่";
                gridView1.Columns["CustomerNo"].Caption = "ลูกค้า";
                gridView1.Columns["LoadNo"].Caption = "รหัสโหลด";
                gridView1.Columns["DriverName"].Caption = "คนขับรถ";
                gridView1.Columns["ShipToNo"].Caption = "สถานที่ส่งสินค้า";
                gridView1.Columns["MixDesignNo"].Caption = "สูตรการผลิต";
                gridView1.Columns["Amount"].Caption = "จำนวน";
                gridView1.Columns["ShipToSlip"].Caption = "ใบส่งของ";
                gridView1.Columns["ReturnTruckToPlantCode"].Caption = "รถกลับหน่อยผลิต";
                gridView1.Columns["AroundStatus"].Caption = "สถานะ..";
                gridView1.Columns["SaleOrderNo"].Caption = "เลขที่ใบสั่งขาย";
                gridView1.Columns["ButtonStatus"].Caption = "";

            });
           

          
        }

        private void GridView1_RowLoaded(object sender, DevExpress.XtraGrid.Views.Base.RowEventArgs e)
        {
            var row = gridView1.GetDataRow(e.RowHandle);
            if (row != null)
            {
                var load_status = (int)row["Load_Status"];
                if (load_status == 1)
                {
                    repositoryItemTicketButtonStatus.Buttons[0].Caption = "Unassigned";
                }
                if (load_status == 2)
                {
                    repositoryItemTicketButtonStatus.Buttons[0].Caption = "Preparing";
                }
                if (load_status == 3)
                {
                    repositoryItemTicketButtonStatus.Buttons[0].Caption = "Loading";
                }
                if (load_status == 4)
                {
                    repositoryItemTicketButtonStatus.Buttons[0].Caption = "Queue";
                }
                if (load_status == 5)
                {
                    repositoryItemTicketButtonStatus.Buttons[0].Caption = "Batching";
                }
                if (load_status == 6)
                {
                    repositoryItemTicketButtonStatus.Buttons[0].Caption = "Loaded";
                }
                if (load_status == 7)
                {
                    repositoryItemTicketButtonStatus.Buttons[0].Caption = "Leaved ";
                }
                if (load_status == 8)
                {
                    repositoryItemTicketButtonStatus.Buttons[0].Caption = "Complete";
                }
                if (load_status == 9)
                {
                    //repositoryItemTicketButtonStatus.Buttons[0].Caption = "Deleted";
                    //TicketMock.GetTickets()[0] = null;
                    //ThreadSafe(() =>
                    //{
                    //    refresh_tricket_table();
                    //});
                }
            }

        }

        private void GridView1_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            var row = gridView1.GetDataRow(e.RowHandle);
            if (row != null)
            {
                var load_status = (int)row["Load_Status"];
                if (load_status == 1)
                {
                    repositoryItemTicketButtonStatus.Buttons[0].Caption = "Unassigned";
                }
                if (load_status == 2)
                {
                    repositoryItemTicketButtonStatus.Buttons[0].Caption = "Preparing";
                }
                if (load_status == 3)
                {
                    repositoryItemTicketButtonStatus.Buttons[0].Caption = "Loading";
                }
                if (load_status == 4)
                {
                    repositoryItemTicketButtonStatus.Buttons[0].Caption = "Queue";
                }
                if (load_status == 5)
                {
                    repositoryItemTicketButtonStatus.Buttons[0].Caption = "Batching";
                }
                if (load_status == 6)
                {
                    repositoryItemTicketButtonStatus.Buttons[0].Caption = "Loaded";
                }
                if (load_status == 7)
                {
                    repositoryItemTicketButtonStatus.Buttons[0].Caption = "Leaved ";
                }
                if (load_status == 8)
                {
                    repositoryItemTicketButtonStatus.Buttons[0].Caption = "Complete";
                }
                if (load_status == 9)
                {
                    //repositoryItemTicketButtonStatus.Buttons[0].Caption = "Deleted";
                    //TicketMock.GetTickets()[0] = null;
                    //ThreadSafe(() =>
                    //{
                    //    refresh_tricket_table();
                    //});

                }
            }
        }

        private void GridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {

        }

        private void GridView1_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            //throw new NotImplementedException();
            GridView view = sender as GridView;
            if (e.Column.FieldName == "ButtonStatus" && e.RowHandle == gridView1.FocusedRowHandle)
            {
                var row = gridView1.GetDataRow(e.RowHandle);
                if(row != null)
                {
                    var load_status = (int)row["Load_Status"];
                    if (load_status == 1)
                    {
                        repositoryItemTicketButtonStatus.Buttons[0].Caption = "Unassigned";
                    }
                    if (load_status == 2)
                    {
                        repositoryItemTicketButtonStatus.Buttons[0].Caption = "Preparing";
                    }
                    if (load_status == 3)
                    {
                        repositoryItemTicketButtonStatus.Buttons[0].Caption = "Loading";
                    }
                    if (load_status == 4)
                    {
                        repositoryItemTicketButtonStatus.Buttons[0].Caption = "Queue";
                    }
                    if (load_status == 5)
                    {
                        repositoryItemTicketButtonStatus.Buttons[0].Caption = "Batching";
                    }
                    if (load_status == 6)
                    {
                        repositoryItemTicketButtonStatus.Buttons[0].Caption = "Loaded";
                    }
                    if (load_status == 7)
                    {
                        repositoryItemTicketButtonStatus.Buttons[0].Caption = "Leaved ";
                    }
                    if (load_status == 8)
                    {
                        repositoryItemTicketButtonStatus.Buttons[0].Caption = "Complete";
                    }
                    if (load_status == 9)
                    {
                        //repositoryItemTicketButtonStatus.Buttons[0].Caption = "Deleted";
                        //TicketMock.GetTickets()[0] = null;
                        //ThreadSafe(() =>
                        //{
                        //    refresh_tricket_table();
                        //});
                        ticket_grid.DataSource = null;
                    }
                }
                
                e.RepositoryItem = repositoryItemTicketButtonStatus;
               
            }
                
        }

        private void refresh_load_truck_table()
        {
            //var loads = LoadTruckMatchMock.GetEmptyLoadTruckMatches();

            var loads = LoadTruckMatchMock.GetLoadTruckMatches();
            ThreadSafe(() => truck_load_grid.DataSource = loads);
            gridView2.Columns["Id"].Visible = false;
            //ThreadSafe(() => truck_load_grid.DataMember = "LoadTruckMatch");

        }

        private void refresh_truck_table()
        {
            var trucks = TruckMock.GetTrucks();

            ThreadSafe(() => truck_grid.DataSource = trucks);
            gridView3.Columns["Id"].Visible = false;
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

       

        //private void btn_add_new_ticket_Click(object sender, EventArgs e)
        //{
        //    AddTricket frm = new AddTricket();
        //    frm.ShowDialog();
        //}

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
                    string sql = "delete from LoadTruckMatch where Id=@Id";
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

        private void RepositoryItemButtonStatus_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            //int id = (int)gridView1.GetFocusedRowCellValue("Id");
            //EditTricket frm = new EditTricket(id);
            //frm.Show();
            //MessageBox.Show("you select id "+id);

            var load_status = (int)gridView1.GetFocusedRowCellValue("Load_Status");
            if (load_status == 1)
            {
                repositoryItemTicketButtonStatus.Buttons[0].Caption = "Unassigned";
            }
            if (load_status == 2)
            {
                repositoryItemTicketButtonStatus.Buttons[0].Caption = "Preparing";
            }
            if (load_status == 3)
            {
                repositoryItemTicketButtonStatus.Buttons[0].Caption = "Loading";
            }
            if (load_status == 4)
            {
                repositoryItemTicketButtonStatus.Buttons[0].Caption = "Queue";
            }
            if (load_status == 5)
            {
                repositoryItemTicketButtonStatus.Buttons[0].Caption = "Batching";
            }
            if (load_status == 6)
            {
                repositoryItemTicketButtonStatus.Buttons[0].Caption = "Loaded";
            }
            if (load_status == 7)
            {
                repositoryItemTicketButtonStatus.Buttons[0].Caption = "Leaved ";
            }
            if (load_status == 8)
            {
                repositoryItemTicketButtonStatus.Buttons[0].Caption = "Complete";
            }
            if (load_status == 9)
            {
                ticket_grid.DataSource = null;
                ticket_grid.RefreshDataSource();
            }
            if(load_status <= 9)
            {
                TicketMock.GetTickets()[0].Load_Status++;
            }
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

        private void btn_generate_load_Click(object sender, EventArgs e)
        {
            FrmGenerateLoad frm = new FrmGenerateLoad();
            frm.ShowDialog();
        }
    }
}
