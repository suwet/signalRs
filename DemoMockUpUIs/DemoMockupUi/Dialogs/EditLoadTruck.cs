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

namespace DemoDxGridControl.Dialogs
{
    public partial class EditLoadTruck : Form
    {
        string connection_string =
                                    System.Configuration.ConfigurationManager.
                                    ConnectionStrings["Test"].ConnectionString;

        private int id;
        public EditLoadTruck(int id)
        {
            this.id = id;
            InitializeComponent();
        }
        public EditLoadTruck()
        {
            InitializeComponent();
        }

        private void EditLoadTruck_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btn_save_new_loadtruck_Click(object sender, EventArgs e)
        {
            string sql = @"update LoadTruckMatch set SaleOrderNo=@SaleOrderNo,
                                             SoldToNo=@SoldToNo,
                                             Amount=@Amount,
                                             MixdesignNo=@MixdesignNo
                                             where Id=@Id
                                            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(connection_string))
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = this.id;
                        cmd.Parameters.Add("@SaleOrderNo", SqlDbType.VarChar, 50).Value = txt_sale_order_no.Text;
                        cmd.Parameters.Add("@SoldToNo", SqlDbType.VarChar, 50).Value = txt_soldto_no.Text;
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = Decimal.Parse(txt_amount.Text);
                        cmd.Parameters.Add("@MixdesignNo", SqlDbType.VarChar, 50).Value = txt_mixdesign.Text;
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Success update LoadTruckMatch", "Success update LoadTruckMatch", MessageBoxButtons.OK);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("error " + ex.Message);
                return;
            }
        }

        private void btn_cancel_new_loadtruck_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LoadData()
        {
            try
            {
                string sql = @"select SaleOrderNo,SoldToNo,MixdesignNo,Amount
                                from LoadTruckMatch where Id=@id";
                using (SqlConnection connection = new SqlConnection(connection_string))
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = this.id;
                        cmd.CommandType = CommandType.Text;

                        var reader = cmd.ExecuteReader();

                        if (!reader.Read())
                            throw new InvalidOperationException("No records were returned.");

                        //.Id = reader.GetInt32(0)
                        string SaleOrderNo = reader.GetString(0);
                        string SoldToNo = reader.GetString(1);
                        decimal Amount = reader.GetDecimal(3);;
                        string MixdesignNo = reader.GetString(2);

                        txt_amount.Text = Amount.ToString();
                        txt_mixdesign.Text = MixdesignNo;
                        txt_soldto_no.Text = SaleOrderNo.ToString();
                        txt_sale_order_no.Text = SaleOrderNo;
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
