
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
            this.btn_generate_load = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.truck_grid = new DevExpress.XtraGrid.GridControl();
            this.gridView3 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_assign = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btn_clear_data = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ticket_grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.truck_load_grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.truck_grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).BeginInit();
            this.SuspendLayout();
            // 
            // ticket_grid
            // 
            this.ticket_grid.Location = new System.Drawing.Point(5, 55);
            this.ticket_grid.MainView = this.gridView1;
            this.ticket_grid.Name = "ticket_grid";
            this.ticket_grid.Size = new System.Drawing.Size(926, 193);
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
            this.truck_load_grid.Location = new System.Drawing.Point(5, 300);
            this.truck_load_grid.MainView = this.gridView2;
            this.truck_load_grid.Name = "truck_load_grid";
            this.truck_load_grid.Size = new System.Drawing.Size(449, 210);
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
            // btn_generate_load
            // 
            this.btn_generate_load.Location = new System.Drawing.Point(208, 271);
            this.btn_generate_load.Name = "btn_generate_load";
            this.btn_generate_load.Size = new System.Drawing.Size(122, 23);
            this.btn_generate_load.TabIndex = 3;
            this.btn_generate_load.Text = "gennerate load";
            this.btn_generate_load.UseVisualStyleBackColor = true;
            this.btn_generate_load.Click += new System.EventHandler(this.btn_generate_load_Click);
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
            // truck_grid
            // 
            this.truck_grid.Location = new System.Drawing.Point(544, 300);
            this.truck_grid.MainView = this.gridView3;
            this.truck_grid.Name = "truck_grid";
            this.truck_grid.Size = new System.Drawing.Size(387, 210);
            this.truck_grid.TabIndex = 6;
            this.truck_grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView3});
            // 
            // gridView3
            // 
            this.gridView3.GridControl = this.truck_grid;
            this.gridView3.Name = "gridView3";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(541, 284);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "รถโม่";
            // 
            // btn_assign
            // 
            this.btn_assign.Location = new System.Drawing.Point(462, 373);
            this.btn_assign.Name = "btn_assign";
            this.btn_assign.Size = new System.Drawing.Size(75, 23);
            this.btn_assign.TabIndex = 8;
            this.btn_assign.Text = "จ่ายงาน";
            this.btn_assign.UseVisualStyleBackColor = true;
            this.btn_assign.Click += new System.EventHandler(this.btn_assign_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 262);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "จับคู่โหลดและรถโม่";
            // 
            // btn_clear_data
            // 
            this.btn_clear_data.Location = new System.Drawing.Point(379, 271);
            this.btn_clear_data.Name = "btn_clear_data";
            this.btn_clear_data.Size = new System.Drawing.Size(75, 23);
            this.btn_clear_data.TabIndex = 10;
            this.btn_clear_data.Text = "Clear Data";
            this.btn_clear_data.UseVisualStyleBackColor = true;
            this.btn_clear_data.Click += new System.EventHandler(this.btn_clear_data_Click);
            // 
            // DemoGridForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(939, 522);
            this.Controls.Add(this.btn_clear_data);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btn_assign);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.truck_grid);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_generate_load);
            this.Controls.Add(this.truck_load_grid);
            this.Controls.Add(this.ticket_grid);
            this.Name = "DemoGridForm";
            this.Text = "DemoDevExpressGrid";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ticket_grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.truck_load_grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.truck_grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl ticket_grid;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.GridControl truck_load_grid;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private System.Windows.Forms.Button btn_generate_load;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraGrid.GridControl truck_grid;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_assign;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_clear_data;
    }
}

