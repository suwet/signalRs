
namespace DemoDxGridControl.Dialogs
{
    partial class AddTricket
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_save_new_ticket = new System.Windows.Forms.Button();
            this.btn_cancel_new_ticket = new System.Windows.Forms.Button();
            this.lbl_sale_order = new System.Windows.Forms.Label();
            this.txt_sale_order_no = new System.Windows.Forms.TextBox();
            this.gbx_ticket = new System.Windows.Forms.GroupBox();
            this.txt_customer_no = new System.Windows.Forms.TextBox();
            this.lbl_customerno = new System.Windows.Forms.Label();
            this.txt_load_no = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_amount = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_status = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.gbx_ticket.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_save_new_ticket
            // 
            this.btn_save_new_ticket.Location = new System.Drawing.Point(239, 222);
            this.btn_save_new_ticket.Name = "btn_save_new_ticket";
            this.btn_save_new_ticket.Size = new System.Drawing.Size(75, 23);
            this.btn_save_new_ticket.TabIndex = 0;
            this.btn_save_new_ticket.Text = "Save";
            this.btn_save_new_ticket.UseVisualStyleBackColor = true;
            this.btn_save_new_ticket.Click += new System.EventHandler(this.btn_save_new_ticket_Click);
            // 
            // btn_cancel_new_ticket
            // 
            this.btn_cancel_new_ticket.Location = new System.Drawing.Point(407, 222);
            this.btn_cancel_new_ticket.Name = "btn_cancel_new_ticket";
            this.btn_cancel_new_ticket.Size = new System.Drawing.Size(75, 23);
            this.btn_cancel_new_ticket.TabIndex = 1;
            this.btn_cancel_new_ticket.Text = "Cancel";
            this.btn_cancel_new_ticket.UseVisualStyleBackColor = true;
            // 
            // lbl_sale_order
            // 
            this.lbl_sale_order.AutoSize = true;
            this.lbl_sale_order.Location = new System.Drawing.Point(29, 50);
            this.lbl_sale_order.Name = "lbl_sale_order";
            this.lbl_sale_order.Size = new System.Drawing.Size(68, 13);
            this.lbl_sale_order.TabIndex = 2;
            this.lbl_sale_order.Text = "SaleOrderNo";
            // 
            // txt_sale_order_no
            // 
            this.txt_sale_order_no.Location = new System.Drawing.Point(112, 50);
            this.txt_sale_order_no.Name = "txt_sale_order_no";
            this.txt_sale_order_no.Size = new System.Drawing.Size(213, 20);
            this.txt_sale_order_no.TabIndex = 3;
            // 
            // gbx_ticket
            // 
            this.gbx_ticket.Controls.Add(this.txt_status);
            this.gbx_ticket.Controls.Add(this.btn_cancel_new_ticket);
            this.gbx_ticket.Controls.Add(this.label3);
            this.gbx_ticket.Controls.Add(this.btn_save_new_ticket);
            this.gbx_ticket.Controls.Add(this.txt_amount);
            this.gbx_ticket.Controls.Add(this.label2);
            this.gbx_ticket.Controls.Add(this.txt_load_no);
            this.gbx_ticket.Controls.Add(this.label1);
            this.gbx_ticket.Controls.Add(this.txt_customer_no);
            this.gbx_ticket.Controls.Add(this.lbl_customerno);
            this.gbx_ticket.Controls.Add(this.txt_sale_order_no);
            this.gbx_ticket.Controls.Add(this.lbl_sale_order);
            this.gbx_ticket.Location = new System.Drawing.Point(25, 27);
            this.gbx_ticket.Name = "gbx_ticket";
            this.gbx_ticket.Size = new System.Drawing.Size(749, 298);
            this.gbx_ticket.TabIndex = 4;
            this.gbx_ticket.TabStop = false;
            this.gbx_ticket.Text = "Ticket Information";
            // 
            // txt_customer_no
            // 
            this.txt_customer_no.Location = new System.Drawing.Point(500, 50);
            this.txt_customer_no.Name = "txt_customer_no";
            this.txt_customer_no.Size = new System.Drawing.Size(213, 20);
            this.txt_customer_no.TabIndex = 5;
            // 
            // lbl_customerno
            // 
            this.lbl_customerno.AutoSize = true;
            this.lbl_customerno.Location = new System.Drawing.Point(417, 50);
            this.lbl_customerno.Name = "lbl_customerno";
            this.lbl_customerno.Size = new System.Drawing.Size(65, 13);
            this.lbl_customerno.TabIndex = 4;
            this.lbl_customerno.Text = "CustomerNo";
            // 
            // txt_load_no
            // 
            this.txt_load_no.Location = new System.Drawing.Point(112, 98);
            this.txt_load_no.Name = "txt_load_no";
            this.txt_load_no.Size = new System.Drawing.Size(213, 20);
            this.txt_load_no.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 98);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "LoadNo";
            // 
            // txt_amount
            // 
            this.txt_amount.Location = new System.Drawing.Point(500, 95);
            this.txt_amount.Name = "txt_amount";
            this.txt_amount.Size = new System.Drawing.Size(213, 20);
            this.txt_amount.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(417, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Amount";
            // 
            // txt_status
            // 
            this.txt_status.Location = new System.Drawing.Point(112, 145);
            this.txt_status.Name = "txt_status";
            this.txt_status.Size = new System.Drawing.Size(213, 20);
            this.txt_status.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(29, 145);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Status";
            // 
            // AddTricket
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 337);
            this.Controls.Add(this.gbx_ticket);
            this.Name = "AddTricket";
            this.Text = "AddTricket";
            this.Load += new System.EventHandler(this.AddTricket_Load);
            this.gbx_ticket.ResumeLayout(false);
            this.gbx_ticket.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_save_new_ticket;
        private System.Windows.Forms.Button btn_cancel_new_ticket;
        private System.Windows.Forms.Label lbl_sale_order;
        private System.Windows.Forms.TextBox txt_sale_order_no;
        private System.Windows.Forms.GroupBox gbx_ticket;
        private System.Windows.Forms.TextBox txt_status;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_amount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_load_no;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_customer_no;
        private System.Windows.Forms.Label lbl_customerno;
    }
}