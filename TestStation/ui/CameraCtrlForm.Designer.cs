namespace TestStation
{
    partial class CameraCtrlForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.PB_Preview = new System.Windows.Forms.PictureBox();
            this.UC_CameraCtrl = new TestStation.CameraCtrlUC();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Preview)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 148F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.PB_Preview, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.UC_CameraCtrl, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 450);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // PB_Preview
            // 
            this.PB_Preview.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.PB_Preview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PB_Preview.Location = new System.Drawing.Point(151, 3);
            this.PB_Preview.Name = "PB_Preview";
            this.PB_Preview.Size = new System.Drawing.Size(646, 444);
            this.PB_Preview.TabIndex = 6;
            this.PB_Preview.TabStop = false;
            this.PB_Preview.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PB_Preview_MouseDown);
            this.PB_Preview.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PB_Preview_MouseMove);
            this.PB_Preview.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PB_Preview_MouseUp);
            // 
            // UC_CameraCtrl
            // 
            this.UC_CameraCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UC_CameraCtrl.Location = new System.Drawing.Point(3, 3);
            this.UC_CameraCtrl.Name = "UC_CameraCtrl";
            this.UC_CameraCtrl.Size = new System.Drawing.Size(142, 444);
            this.UC_CameraCtrl.TabIndex = 0;
            // 
            // CameraCtrlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "CameraCtrlForm";
            this.Text = "Camera Ctrl Form";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PB_Preview)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private CameraCtrlUC UC_CameraCtrl;
        private System.Windows.Forms.PictureBox PB_Preview;
    }
}