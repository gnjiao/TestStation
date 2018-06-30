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
            this.TBL_1RightLeft = new System.Windows.Forms.TableLayoutPanel();
            this.PB_Preview = new System.Windows.Forms.PictureBox();
            this.TBL_2UpDown = new System.Windows.Forms.TableLayoutPanel();
            this.UC_CameraCtrl = new TestStation.CameraCtrlUC();
            this.TBL_1RightLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Preview)).BeginInit();
            this.TBL_2UpDown.SuspendLayout();
            this.SuspendLayout();
            // 
            // TBL_1RightLeft
            // 
            this.TBL_1RightLeft.ColumnCount = 2;
            this.TBL_1RightLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 148F));
            this.TBL_1RightLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TBL_1RightLeft.Controls.Add(this.PB_Preview, 1, 0);
            this.TBL_1RightLeft.Controls.Add(this.TBL_2UpDown, 0, 0);
            this.TBL_1RightLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TBL_1RightLeft.Location = new System.Drawing.Point(0, 0);
            this.TBL_1RightLeft.Name = "TBL_1RightLeft";
            this.TBL_1RightLeft.RowCount = 1;
            this.TBL_1RightLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TBL_1RightLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 601F));
            this.TBL_1RightLeft.Size = new System.Drawing.Size(982, 601);
            this.TBL_1RightLeft.TabIndex = 0;
            // 
            // PB_Preview
            // 
            this.PB_Preview.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.PB_Preview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PB_Preview.Location = new System.Drawing.Point(151, 3);
            this.PB_Preview.Name = "PB_Preview";
            this.PB_Preview.Size = new System.Drawing.Size(828, 595);
            this.PB_Preview.TabIndex = 6;
            this.PB_Preview.TabStop = false;
            this.PB_Preview.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PB_Preview_MouseDown);
            this.PB_Preview.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PB_Preview_MouseMove);
            this.PB_Preview.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PB_Preview_MouseUp);
            // 
            // TBL_2UpDown
            // 
            this.TBL_2UpDown.ColumnCount = 1;
            this.TBL_2UpDown.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TBL_2UpDown.Controls.Add(this.UC_CameraCtrl, 0, 0);
            this.TBL_2UpDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TBL_2UpDown.Location = new System.Drawing.Point(3, 3);
            this.TBL_2UpDown.Name = "TBL_2UpDown";
            this.TBL_2UpDown.RowCount = 2;
            this.TBL_2UpDown.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TBL_2UpDown.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TBL_2UpDown.Size = new System.Drawing.Size(142, 595);
            this.TBL_2UpDown.TabIndex = 7;
            // 
            // UC_CameraCtrl
            // 
            this.UC_CameraCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UC_CameraCtrl.Location = new System.Drawing.Point(3, 3);
            this.UC_CameraCtrl.Name = "UC_CameraCtrl";
            this.UC_CameraCtrl.Size = new System.Drawing.Size(136, 291);
            this.UC_CameraCtrl.TabIndex = 0;
            // 
            // CameraCtrlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(982, 601);
            this.Controls.Add(this.TBL_1RightLeft);
            this.Name = "CameraCtrlForm";
            this.Text = "Camera Ctrl Form";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.TBL_1RightLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PB_Preview)).EndInit();
            this.TBL_2UpDown.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel TBL_1RightLeft;
        private System.Windows.Forms.PictureBox PB_Preview;
        private System.Windows.Forms.TableLayoutPanel TBL_2UpDown;
        private CameraCtrlUC UC_CameraCtrl;
    }
}