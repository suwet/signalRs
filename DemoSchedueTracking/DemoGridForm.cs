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
using System.Transactions;
using System.Windows.Forms;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;

namespace DemoDxGridControl
{
    public partial class DemoGridForm : Form
    {
        private RepositoryItemButtonEdit repositoryItemTicketButtonStatus = new RepositoryItemButtonEdit();
        private IHubProxy hubproxy { get; set; }
        string ServerURL = ConfigurationManager.AppSettings["hub_url"];
        private HubConnection connection { get; set; }
        private string log_path = ConfigurationManager.AppSettings["logpath"] + DateTime.Now.ToString("yyyy_MM_dd") + ".log";
        string connection_string =
                                    System.Configuration.ConfigurationManager.
                                    ConnectionStrings["Test"].ConnectionString;

        public DemoGridForm()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("II MainUIThread Thread Id is " + System.Threading.Thread.CurrentThread.ManagedThreadId);

            gridView1.CustomRowCellEdit += GridView1_CustomRowCellEdit;
            gridView1.CustomUnboundColumnData += GridView1_CustomUnboundColumnData;
            gridView1.RowUpdated += GridView1_RowUpdated;
            gridView1.RowLoaded += GridView1_RowLoaded;

            Refresh_tricket_table();
            Refresh_loads_table();
            Refresh_truck_table();


            Task.Run(async () =>
            {
                /*
                RunOnUiThread(() => 
                {
                    //System.Diagnostics.Debug.WriteLine("II RunOnUiThread Form1_Load Thread Id is " + System.Threading.Thread.CurrentThread.ManagedThreadId);
                    // register event
                    gridView1.CustomRowCellEdit += GridView1_CustomRowCellEdit;
                    gridView1.CustomUnboundColumnData += GridView1_CustomUnboundColumnData;
                    gridView1.RowUpdated += GridView1_RowUpdated;
                    gridView1.RowLoaded += GridView1_RowLoaded;
                });

                RunOnUiThread(() => Refresh_tricket_table());
                RunOnUiThread(() => Refresh_loads_table());
                RunOnUiThread(() => Refresh_truck_table());
                */
                await ConnectAsync();
            });
        }
        private async Task ConnectAsync()
        {
            connection = new HubConnection(ServerURL.Trim());
            connection.Closed += Connection_Closed;
            connection.ConnectionSlow += Connection_ConnectionSlow;
            hubproxy = connection.CreateHubProxy("NotiHub");

            hubproxy.On<Ticket, string>("ReceiveTicketMessage", (entity, action_name)
                                      =>
            {
                System.Diagnostics.Debug.WriteLine("III ReceiveTicketMessage Thread Id is " + System.Threading.Thread.CurrentThread.ManagedThreadId);
                Task.Run(async () => 
                {
                   await WriteLogThreadSafe.WriteToFile(entity.CustomerNo + "  " + action_name + "\n" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), log_path);
                });
                
               
                // jumb back to ui thread
                RunOnUiThread(() =>
                {
                    System.Diagnostics.Debug.WriteLine("RunOnUiThread Thread Id is " + System.Threading.Thread.CurrentThread.ManagedThreadId);
                    Refresh_tricket_table();
                    //System.Diagnostics.Debug.WriteLine(entity.CustomerNo + "  " + action_name + " " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff tt \n"));
                });
            });

            hubproxy.On<Loads, string>("ReceiveLoadMessage", (entity, action_name)
                                      =>
            {
                System.Diagnostics.Debug.WriteLine("III ReceiveLoadMessage Thread Id is " + System.Threading.Thread.CurrentThread.ManagedThreadId);
                Task.Run(async () =>
                {
                    await WriteLogThreadSafe.WriteToFile(entity.CustomerNo + "  " + action_name + "\n" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), log_path);
                });

                RunOnUiThread(() =>
                {
                     Refresh_loads_table();
                    //System.Diagnostics.Debug.WriteLine(entity.CustomerNo + "  " + action_name + " " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff tt  \n"));
                });
            });

            hubproxy.On<Trucks, string>("ReceiveTruckMessage", (entity, action_name)
                                      =>
            {
                Task.Run(async () =>
                {
                    await WriteLogThreadSafe.WriteToFile(entity.TruckCode + "  " + action_name + "\n" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), log_path);
                });
                    RunOnUiThread(() =>
                    {
                        Refresh_truck_table();
                        //System.Diagnostics.Debug.WriteLine(entity.TruckCode + "  " + action_name + " " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff tt \n"));
                    });
            });

            try
            {
                await connection.Start();
            }
            catch (HttpRequestException ex)
            {
                RunOnUiThread(() => MessageBox.Show("Cannot connect to signalR hub", "Warring", MessageBoxButtons.OK));
                
                await Task.Run(async () =>
                {
                    await WriteLogThreadSafe.WriteToFile("Unable to connect to server : Start server before connectiong clients.", log_path);
                });
                   
                return;

            }
            
        }

        private void Connection_ConnectionSlow()
        {
            RunOnUiThread(() => MessageBox.Show("You have been connectionSlow ", "You have been connectionSlow", MessageBoxButtons.OK, MessageBoxIcon.Warning));

            Task.Run(async () => await WriteLogThreadSafe.WriteToFile("You have been connectionSlow.", log_path));
        }

        private void Connection_Closed()
        {
            RunOnUiThread(() => MessageBox.Show("You have been disconnected ", "You have been disconnected",MessageBoxButtons.OK,MessageBoxIcon.Warning));
            
            Task.Run(async() => await WriteLogThreadSafe.WriteToFile("You have been disconnected.", log_path));
        }

       
        private void GridView1_RowLoaded(object sender, DevExpress.XtraGrid.Views.Base.RowEventArgs e)
        {
           

        }

        private void GridView1_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
           
        }

        private void GridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {

        }

        private void GridView1_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            
            GridView view = sender as GridView;
            if (e.Column.FieldName == "ButtonStatus"/* && e.RowHandle == gridView1.FocusedRowHandle*/)
            {
                e.Column.Caption = "";

                //RepositoryItemButtonEdit repositoryButtonStatus = (RepositoryItemButtonEdit)repositoryItemTicketButtonStatus.Clone();

                RepositoryItemButtonEdit repositoryButtonStatus = new RepositoryItemButtonEdit();
                repositoryButtonStatus.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
                repositoryButtonStatus.Buttons[0].Kind = ButtonPredefines.Glyph;
                repositoryButtonStatus.ButtonClick += RepositoryItemButtonStatus_ButtonClick;

                

                var l_status = (int)view.GetRowCellValue(e.RowHandle, "Load_Status");
                if (l_status == 1)
                {
                    repositoryButtonStatus.Buttons[0].Caption = "Unassigned";
                }
                if (l_status == 2)
                {
                    repositoryButtonStatus.Buttons[0].Caption = "Preparing";
                }
                if (l_status == 3)
                {
                    repositoryButtonStatus.Buttons[0].Caption = "Loading";
                }
                if (l_status == 4)
                {
                    repositoryButtonStatus.Buttons[0].Caption = "Queue";
                }
                if (l_status == 5)
                {
                    repositoryButtonStatus.Buttons[0].Caption = "Batching";
                }
                if (l_status == 6)
                {
                    repositoryButtonStatus.Buttons[0].Caption = "Loaded";
                }
                if (l_status == 7)
                {
                    repositoryButtonStatus.Buttons[0].Caption = "Leaved ";
                }
                if (l_status == 8)
                {
                    repositoryButtonStatus.Buttons[0].Caption = "Complete";
                }
                if (l_status == 9)
                {
                    repositoryButtonStatus.Buttons[0].Caption = "Deleted";
                }

                e.RepositoryItem = repositoryButtonStatus; 
               
            }
              
        }

        private void Refresh_loads_table()
        {
            string sql = "SELECT * FROM Loads WHERE Load_Status = 1";
            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                if(connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                }
               
                using (SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection))
                {
                    DataSet ds = new DataSet();

                    dataadapter.Fill(ds, "Loads");
                    connection.Close();

                    RunOnUiThread(() =>
                    {
                        truck_load_grid.DataSource = ds;
                        truck_load_grid.DataMember = "Loads";

                        gridView2.Columns["Id"].Visible = false;
                    }
                    );
                }
            }
        }

        private void Refresh_truck_table()
        {
            //var trucks = TruckMock.GetTrucks();
            string sql = "SELECT * FROM Trucks WHERE Load_Status = 1";
            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                using (SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection))
                {
                    DataSet ds = new DataSet();
                    if(connection.State != System.Data.ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    dataadapter.Fill(ds, "Trucks");
                    connection.Close();

                    RunOnUiThread(() =>
                    {
                        truck_grid.DataSource = ds;
                        truck_grid.DataMember = "Trucks";
                        gridView3.Columns["Id"].Visible = false;
                    }
                    );
                }
            }
        }

        //common function 
        private void Refresh_tricket_table()
        {
            string sql = "SELECT * FROM Ticket WHERE Load_Status < 9";
            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                using (SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection))
                {
                    DataSet ds = new DataSet();
                    if(connection.State != System.Data.ConnectionState.Open)
                    {
                        connection.Open();
                    }
                   
                    dataadapter.Fill(ds, "Ticket");
                    connection.Close();

                    RunOnUiThread(() =>
                    {
                        ticket_grid.DataSource = ds;
                        ticket_grid.DataMember = "Ticket";


                        if (ticket_grid.RepositoryItems.Contains(repositoryItemTicketButtonStatus) == false)
                        {
                            gridView1.OptionsBehavior.ReadOnly = true;

                            GridColumn gridColumn = gridView1.Columns.AddVisible("ButtonStatus");
                            //gridColumn.UnboundDataType = typeof(DevExpress.Data.);
                            //repositoryItemTicketButtonEdit.NullText = "Edit";
                            repositoryItemTicketButtonStatus.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
                            repositoryItemTicketButtonStatus.Buttons[0].Kind = ButtonPredefines.Glyph;
                            repositoryItemTicketButtonStatus.ButtonClick += RepositoryItemButtonStatus_ButtonClick;
                            ticket_grid.RepositoryItems.Add(repositoryItemTicketButtonStatus);
                            gridColumn.ColumnEdit = repositoryItemTicketButtonStatus;
                            gridColumn.Caption = "";
                            gridColumn.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
                        }


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
                    }
                );
                }
            }
        }
        private void RunOnUiThread(MethodInvoker method)
        {
            try
            {
                if (InvokeRequired)
                {
                    //Invoke(method); //sync
                    BeginInvoke(method);// async
                }
                    
                else
                {
                    method();
                }
                    
            }
            catch (ObjectDisposedException) { }
        }


        private void RepositoryItemButtonStatus_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {

            int id = (int)gridView1.GetFocusedRowCellValue("Id");
            int load_status = (int)gridView1.GetFocusedRowCellValue("Load_Status");
            load_status++;
            using (var scope = new TransactionScope())
            {
                if (load_status <= 9)
                {
                    // update Ticket.Load_Status
                    //Ticket.Load_Status = Ticket.Load_Status + 1;

                    UpdateTicketLoadStatus(id, load_status);
                }
                if (load_status == 9)
                {
                    // free truck
                    string TruckCode = (string)gridView1.GetFocusedRowCellValue("TruckCode");
                    UpdateTruckLoadStatus(TruckCode, 1);
                }

                scope.Complete();
            }
        }

        private void UpdateTruckLoadStatus(string truckCode, int status)
        {
            try
            {
                string sql = "update Trucks set Load_Status=@Load_Status  where TruckCode=@TruckCode";
                using (SqlConnection connection = new SqlConnection(connection_string))
                {
                    if (connection.State != System.Data.ConnectionState.Open)
                        connection.Open();

                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.Add("@TruckCode", SqlDbType.VarChar,50).Value = truckCode.Trim();
                        cmd.Parameters.Add("@Load_Status", SqlDbType.Int).Value = status;
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

        private void UpdateLoadsLoadStatus(string SaleOrderNo,int loadnumber, int status)
        {
            try
            {
                string sql = "update Loads set Load_Status=@Load_Status  where SaleOrderNo=@SaleOrderNo and LoadNumber=@LoadNumber";
                using (SqlConnection connection = new SqlConnection(connection_string))
                {
                    if (connection.State != System.Data.ConnectionState.Open)
                        connection.Open();

                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.Add("@SaleOrderNo", SqlDbType.VarChar,50).Value = SaleOrderNo.Trim();
                        cmd.Parameters.Add("@LoadNumber", SqlDbType.Int).Value = loadnumber;
                        cmd.Parameters.Add("@Load_Status", SqlDbType.Int).Value = status;
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

        private void UpdateTicketLoadStatus(int id,int status)
        {
            try
            {
                string sql = "update ticket set Load_Status=@Load_Status  where Id=@Id";
                using (SqlConnection connection = new SqlConnection(connection_string))
                {
                    if (connection.State != System.Data.ConnectionState.Open)
                        connection.Open();

                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                        cmd.Parameters.Add("@Load_Status", SqlDbType.Int).Value = status;
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

        private void btn_generate_load_Click(object sender, EventArgs e)
        {
            FrmGenerateLoad frm = new FrmGenerateLoad();
            frm.ShowDialog();
        }

        private void btn_assign_Click(object sender, EventArgs e)
        {
            // 1 get selected row of left grid
            // 2 get selected row of rigth grid

            // 3 insert to ticket table

            // 4 update Load Load_Status = 2
            // 5 update Truck Load_Status = 2
            try
            {
                //string SaleOrderNo = (string)gridView2.GetFocusedRowCellValue("SaleOrderNo");
                //int LoadNumber = (int)gridView2.GetFocusedRowCellValue("LoadNumber");
                //string TruckCode = (string)gridView3.GetFocusedRowCellValue("TruckCode");
                if(gridView2.RowCount == 0 || gridView3.RowCount == 0)
                {
                    MessageBox.Show("Empty data cannot match");
                }
                else
                {
                    Match();
                }
            }
            catch (Exception x)
            {

                MessageBox.Show("error " + x.Message);
            }
        }

        private void Match()
        {

            string sql = @"insert into ticket(SaleOrderNo,TruckCode,CustomerNo,Amount,LoadNo,DriverName,Load_Status,ShipToNo,AroundStatus,MixDesignNo,ReturnTruckToPlantCode,ShipToSlip)
                           values(@SaleOrderNo,@TruckCode,@CustomerNo,@Amount,@LoadNo,@DriverName,@Load_Status,@ShipToNo,@AroundStatus,@MixDesignNo,@ReturnTruckToPlantCode,@ShipToSlip)";
            try
            {
                //int leftid = (int)gridView2.GetFocusedRowCellValue("Id");
                //int rigthid = (int)gridView3.GetFocusedRowCellValue("Id");

                string SaleOrderNo = (string)gridView2.GetFocusedRowCellValue("SaleOrderNo");
                string TruckCode = (string)gridView3.GetFocusedRowCellValue("TruckCode"); // right
                decimal Amount = (decimal)gridView2.GetFocusedRowCellValue("Amount");
                int LoadNo = (int)gridView2.GetFocusedRowCellValue("LoadNumber");
                string ShipToNo = (string)gridView2.GetFocusedRowCellValue("ShipToNo");
                string MixDesignNo = (string)gridView2.GetFocusedRowCellValue("MixdesignNo");

                using (var scope = new TransactionScope())
                {
                    using (SqlConnection connection = new SqlConnection(connection_string))
                    {
                        if (connection.State != System.Data.ConnectionState.Open)
                            connection.Open();

                        using (SqlCommand cmd = new SqlCommand(sql, connection))
                        {
                            cmd.Parameters.Add("@SaleOrderNo", SqlDbType.VarChar, 50).Value = SaleOrderNo;
                            cmd.Parameters.Add("@TruckCode", SqlDbType.VarChar, 50).Value = TruckCode;
                            cmd.Parameters.Add("@CustomerNo", SqlDbType.VarChar, 50).Value = "Cus_Name138";
                            cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = Amount;
                            cmd.Parameters.Add("@LoadNo", SqlDbType.Int).Value = LoadNo;
                            cmd.Parameters.Add("@DriverName", SqlDbType.VarChar, 50).Value = "DVFWS";
                            cmd.Parameters.Add("@Load_Status", SqlDbType.Int).Value = 4;
                            cmd.Parameters.Add("@ShipToNo", SqlDbType.VarChar, 50).Value = ShipToNo;
                            cmd.Parameters.Add("@AroundStatus", SqlDbType.VarChar, 50).Value = $"{LoadNo}/{(int)Amount}";
                            cmd.Parameters.Add("@MixDesignNo", SqlDbType.VarChar, 50).Value = MixDesignNo;
                            cmd.Parameters.Add("@ReturnTruckToPlantCode", SqlDbType.VarChar, 50).Value = "S101:S101";
                            cmd.Parameters.Add("@ShipToSlip", SqlDbType.VarChar, 50).Value = "101220170001";
                            cmd.CommandType = CommandType.Text;
                            cmd.ExecuteNonQuery();
                        }
                    }

                    UpdateLoadsLoadStatus(SaleOrderNo, LoadNo, 2);
                    UpdateTruckLoadStatus(TruckCode, 2);
                    scope.Complete();
               }   
            }
            catch (Exception ex)
            {
                MessageBox.Show("error " + ex.Message);
                return;
            }
        }

        private void btn_clear_data_Click(object sender, EventArgs e)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    RestoreTruckStatusData();
                    DeleteLoadData();
                    DeleteTicketData();

                    scope.Complete();
                    MessageBox.Show("Success");
                }
            }
            catch (Exception exe)
            {
                MessageBox.Show("error " + exe.Message);
            }
            
        }


        private void RestoreTruckStatusData()
        {
            try
            {
                string sql = "update trucks set Load_Status=1";
                using (SqlConnection connection = new SqlConnection(connection_string))
                {
                    if (connection.State != System.Data.ConnectionState.Open)
                        connection.Open();

                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        //cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

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
        private void DeleteLoadData()
        {
            try
            {
                string sql = "delete from Loads";
                using (SqlConnection connection = new SqlConnection(connection_string))
                {
                    if (connection.State != System.Data.ConnectionState.Open)
                        connection.Open();

                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        //cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

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

        private void DeleteTicketData()
        {
            try
            {
                string sql = "delete from ticket";
                using (SqlConnection connection = new SqlConnection(connection_string))
                {
                    if (connection.State != System.Data.ConnectionState.Open)
                        connection.Open();

                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        //cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

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
