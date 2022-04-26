
namespace DemoDxGridControl
{
    partial class DemoGridForm
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
            this.ticket_grid = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.truck_load_grid = new DevExpress.XtraGrid.GridControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.btn_add_new_ticket = new System.Windows.Forms.Button();
            this.btn_new_truck_load = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ticket_grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.truck_load_grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // ticket_grid
            // 
            this.ticket_grid.Location = new System.Drawing.Point(0, 55);
            this.ticket_grid.MainView = this.gridView1;
            this.ticket_grid.Name = "ticket_grid";
            this.ticket_grid.Size = new System.Drawing.Size(872, 193);
            this.ticket_grid.TabIndex = 0;
            this.ticket_grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.ticket_grid;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsCustomization.AllowFilter = false;
            this.gridView1.OptionsFilter.AllowFilterEditor = false;
            this.gridView1.OptionsFind.AllowFindPanel = false;
            this.gridView1.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridView1.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gridView1.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // truck_load_grid
            // 
            this.truck_load_grid.Location = new System.Drawing.Point(0, 300);
            this.truck_load_grid.MainView = this.gridView2;
            this.truck_load_grid.Name = "truck_load_grid";
            this.truck_load_grid.Size = new System.Drawing.Size(872, 210);
            this.truck_load_grid.TabIndex = 1;
            this.truck_load_grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            // 
            // gridView2
            // 
            this.gridView2.GridControl = this.truck_load_grid;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsView.ShowGroupPanel = false;
            // 
            // btn_add_new_ticket
            // 
            this.btn_add_new_ticket.Location = new System.Drawing.Point(738, 26);
            this.btn_add_new_ticket.Name = "btn_add_new_ticket";
            this.btn_add_new_ticket.Size = new System.Drawing.Size(122, 23);
            this.btn_add_new_ticket.TabIndex = 2;
            this.btn_add_new_ticket.Text = "New Ticket";
            this.btn_add_new_ticket.UseVisualStyleBackColor = true;
            this.btn_add_new_ticket.Click += new System.EventHandler(this.btn_add_new_ticket_Click);
            // 
            // btn_new_truck_load
            // 
            this.btn_new_truck_load.Location = new System.Drawing.Point(738, 271);
            this.btn_new_truck_load.Name = "btn_new_truck_load";
            this.btn_new_truck_load.Size = new System.Drawing.Size(122, 23);
            this.btn_new_truck_load.TabIndex = 3;
            this.btn_new_truck_load.Text = "New TruckLoad";
            this.btn_new_truck_load.UseVisualStyleBackColor = true;
            this.btn_new_truck_load.Click += new System.EventHandler(this.btn_new_truck_load_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "รายละเอียดการผลิต(Ticket)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 284);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "โหลด";
            // 
            // DemoGridForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(872, 522);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_new_truck_load);
            this.Controls.Add(this.btn_add_new_ticket);
            this.Controls.Add(this.truck_load_grid);
            this.Controls.Add(this.ticket_grid);
            this.Name = "DemoGridForm";
            this.Text = "DemoDevExpressGrid";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ticket_grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.truck_load_grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl ticket_grid;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.GridControl truck_load_grid;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private System.Windows.Forms.Button btn_add_new_ticket;
        private System.Windows.Forms.Button btn_new_truck_load;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

