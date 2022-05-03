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
    public partial class AddLoadTruck : Form
    {
        string connection_string =
                                    System.Configuration.ConfigurationManager.
                                    ConnectionStrings["Test"].ConnectionString;
        public AddLoadTruck()
        {
            InitializeComponent();
        }

        private bool ValidateTicket()
        {
            if (string.IsNullOrEmpty(txt_sale_order_no.Text) == false &&
                string.IsNullOrEmpty(txt_soldto_no.Text) == false &&
                string.IsNullOrEmpty(txt_amount.Text) == false &&
                string.IsNullOrEmpty(txt_sale_order_no.Text) == false
                )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void btn_save_new_loadtruck_Click(object sender, EventArgs e)
        {
            // validate 
            if (ValidateTicket())
            {
                string sql = @"insert into LoadTruckMatch(SaleOrderNo,SoldToNo,MixdesignNo,Amount)
                               values(@SaleOrderNo,@SoldToNo,@MixdesignNo,@Amount)";
                try
                {
                    using (SqlConnection connection = new SqlConnection(connection_string))
                    {
                        if (connection.State != ConnectionState.Open)
                            connection.Open();

                        using (SqlCommand cmd = new SqlCommand(sql, connection))
                        {
                            cmd.Parameters.Add("@SaleOrderNo", SqlDbType.VarChar, 50).Value = txt_sale_order_no.Text;
                            cmd.Parameters.Add("@SoldToNo", SqlDbType.VarChar, 50).Value = txt_soldto_no.Text;
                            cmd.Parameters.Add("@MixdesignNo", SqlDbType.VarChar, 50).Value = txt_mixdesign.Text;
                            cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = Decimal.Parse(txt_amount.Text);
                            cmd.CommandType = CommandType.Text;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("Success add new LoadTruckMatch", "Success add new LoadTruckMatch", MessageBoxButtons.OK);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("error " + ex.Message);
                    return;
                }
            }
            else
            {
                MessageBox.Show("please check input feild");
            }
        }

        private void btn_cancel_new_loadtruck_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AddLoadTruck_Load(object sender, EventArgs e)
        {

        }
    }
}
