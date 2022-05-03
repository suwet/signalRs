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
    public partial class AddTricket : Form
    {
        string connection_string =
                                    System.Configuration.ConfigurationManager.
                                    ConnectionStrings["Test"].ConnectionString;
        public AddTricket()
        {
            InitializeComponent();
        }

        private void AddTricket_Load(object sender, EventArgs e)
        {

        }

        private bool ValidateTicket()
        {
            if(string.IsNullOrEmpty(txt_sale_order_no.Text) == false &&
                string.IsNullOrEmpty(txt_customer_no.Text) == false &&
                string.IsNullOrEmpty(txt_amount.Text) == false &&
                string.IsNullOrEmpty(txt_status.Text) == false &&
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

        private void btn_save_new_ticket_Click(object sender, EventArgs e)
        {
            // validate 
            if (ValidateTicket())
            {
                string sql = @"insert into ticket(SaleOrderNo,CustomerNo,Amount,_Status,LoadNo)
                               values(@SaleOrderNo,@CustomerNo,@Amount,@_Status,@LoadNo)";
                try
                {
                    using (SqlConnection connection = new SqlConnection(connection_string))
                    {
                        if (connection.State != ConnectionState.Open)
                            connection.Open();

                        using (SqlCommand cmd = new SqlCommand(sql, connection))
                        {
                            cmd.Parameters.Add("@SaleOrderNo", SqlDbType.VarChar, 50).Value = txt_sale_order_no.Text;
                            cmd.Parameters.Add("@CustomerNo", SqlDbType.VarChar, 50).Value = txt_customer_no.Text;
                            cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = Decimal.Parse(txt_amount.Text);
                            cmd.Parameters.Add("@_Status", SqlDbType.Int).Value = Int32.Parse(txt_status.Text);
                            cmd.Parameters.Add("@LoadNo", SqlDbType.VarChar, 50).Value = txt_sale_order_no.Text;
                            cmd.CommandType = CommandType.Text;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("Success add new ticket", "Success add new ticket", MessageBoxButtons.OK);
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
    }
}
