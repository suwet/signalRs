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
    public partial class EditTricket : Form
    {
        string connection_string = 
                                    System.Configuration.ConfigurationManager.
                                    ConnectionStrings["Test"].ConnectionString;
            
            //@"Data Source=suwet_phr2\SQL2014;Initial Catalog = MyTestdb; Persist Security Info=True;User ID = sa; Password=1234qwer";
        private int id = 0;

        public EditTricket(int id)
        {
            this.id = id;
            InitializeComponent();
        }
        public EditTricket()
        {
            InitializeComponent();
        }

        private void btn_save_new_ticket_Click(object sender, EventArgs e)
        {
            string sql = @"update ticket set SaleOrderNo=@SaleOrderNo,
                                             CustomerNo=@CustomerNo,
                                             Amount=@Amount,
                                             Load_Status=@Load_Status,
                                             LoadNo=@LoadNo
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
                        cmd.Parameters.Add("@CustomerNo", SqlDbType.VarChar, 50).Value = txt_customer_no.Text;
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = Decimal.Parse(txt_amount.Text);
                        cmd.Parameters.Add("@Load_Status", SqlDbType.Int).Value = Int32.Parse(txt_status.Text);
                        cmd.Parameters.Add("@LoadNo", SqlDbType.Int).Value = Int32.Parse(txt_load_no.Text);
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Success update ticket", "Success update ticket", MessageBoxButtons.OK);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("error " + ex.Message);
                return;
            }
        }

        private void btn_cancel_new_ticket_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void EditTricket_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                string sql = @"select SaleOrderNo,CustomerNo,Amount,Load_Status,LoadNo
                                from ticket where Id=@id";
                using (SqlConnection connection = new SqlConnection(connection_string))
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = this.id;
                        cmd.CommandType = CommandType.Text;

                        var reader =  cmd.ExecuteReader();

                        if (!reader.Read())
                            throw new InvalidOperationException("No records were returned.");

                        //.Id = reader.GetInt32(0)
                        string SaleOrderNo = reader.GetString(0);
                        string CustomerNo  = reader.GetString(1);
                        decimal Amount = reader.GetDecimal(2);
                        int _Status = reader.GetInt32(3);
                        int LoadNo = reader.GetInt32(4);

                        txt_amount.Text = Amount.ToString();
                        txt_customer_no.Text = CustomerNo;
                        txt_load_no.Text = LoadNo.ToString();
                        txt_status.Text = _Status.ToString();
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
