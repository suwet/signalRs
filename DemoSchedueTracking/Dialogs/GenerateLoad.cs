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
    public partial class FrmGenerateLoad : Form
    {
        string connection_string =
                                    System.Configuration.ConfigurationManager.
                                    ConnectionStrings["Test"].ConnectionString;
        public FrmGenerateLoad()
        {
            InitializeComponent();
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if(txt_quantity.Text != "" && txt_truck_size.Text != "")
            {
                int quantity = Int32.Parse(txt_quantity.Text.Trim());
                int truck_size = Int32.Parse(txt_truck_size.Text.Trim());

                int NoOfLoad = (int) (quantity / truck_size);

                GennerateLoad(NoOfLoad);
                Close();
            }
            else
            {
                MessageBox.Show("Please fill input");
            }
        }


        private void GennerateLoad(int loadSize)
        {

            string sql = @"insert into Loads(Amount,LoadNumber,MixdesignNo,SaleOrderNo,CustomerNo,ShipToNo,Load_Status)
                               values(@Amount,@LoadNumber,@MixdesignNo,@SaleOrderNo,@CustomerNo,@ShipToNo,@Load_Status)";
            try
            {
                using (SqlConnection connection = new SqlConnection(connection_string))
                {
                    if (connection.State != ConnectionState.Open)
                    { 
                        connection.Open(); 
                    }
                    // loop
                    for (int i = 1; i <= loadSize; i++)
                    {
                        using (SqlCommand cmd = new SqlCommand(sql, connection))
                        {
                            cmd.Parameters.Add("@Amount", SqlDbType.Int).Value = loadSize;
                            cmd.Parameters.Add("@LoadNumber", SqlDbType.Int).Value =i;
                            cmd.Parameters.Add("@MixdesignNo", SqlDbType.VarChar, 50).Value = "990620835";
                            cmd.Parameters.Add("@SaleOrderNo", SqlDbType.VarChar, 50).Value = "S03P000-000000117";
                            cmd.Parameters.Add("@CustomerNo", SqlDbType.VarChar, 50).Value = "Cus_000001125";
                            cmd.Parameters.Add("@ShipToNo", SqlDbType.VarChar, 50).Value = "SH_Name193";
                            cmd.Parameters.Add("@Load_Status", SqlDbType.Int).Value = 1;
                            cmd.CommandType = CommandType.Text;
                            cmd.ExecuteNonQuery();
                        }
                    }
                   
                }
                MessageBox.Show("Success generate load", "Success generate load", MessageBoxButtons.OK);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("error " + ex.Message);
                return;
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
