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
            this.TBL_2Left = new System.Windows.Forms.TableLayoutPanel();
            this.TBL_2Right = new System.Windows.Forms.TableLayoutPanel();
            this.UC_CameraCtrl = new TestStation.CameraCtrlUC();
            this.UC_MotorCtrl = new TestStation.ui.MotorCtrlUC();
            this.UC_Result = new TestStation.ui.ResultUC();
            this.TBL_1RightLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Preview)).BeginInit();
            this.TBL_2Left.SuspendLayout();
            this.TBL_2Right.SuspendLayout();
            this.SuspendLayout();
            // 
            // TBL_1RightLeft
            // 
            this.TBL_1RightLeft.ColumnCount = 2;
            this.TBL_1RightLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 148F));
            this.TBL_1RightLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TBL_1RightLeft.Controls.Add(this.TBL_2Left, 0, 0);
            this.TBL_1RightLeft.Controls.Add(this.TBL_2Right, 1, 0);
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
            this.PB_Preview.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.PB_Preview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PB_Preview.Location = new System.Drawing.Point(3, 3);
            this.PB_Preview.Name = "PB_Preview";
            this.PB_Preview.Size = new System.Drawing.Size(822, 489);
            this.PB_Preview.TabIndex = 6;
            this.PB_Preview.TabStop = false;
            this.PB_Preview.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PB_Preview_MouseDown);
            this.PB_Preview.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PB_Preview_MouseMove);
            this.PB_Preview.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PB_Preview_MouseUp);
            // 
            // TBL_2Left
            // 
            this.TBL_2Left.ColumnCount = 1;
            this.TBL_2Left.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TBL_2Left.Controls.Add(this.UC_CameraCtrl, 0, 0);
            this.TBL_2Left.Controls.Add(this.UC_MotorCtrl, 0, 1);
            this.TBL_2Left.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TBL_2Left.Location = new System.Drawing.Point(3, 3);
            this.TBL_2Left.Name = "TBL_2Left";
            this.TBL_2Left.RowCount = 2;
            this.TBL_2Left.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TBL_2Left.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TBL_2Left.Size = new System.Drawing.Size(142, 595);
            this.TBL_2Left.TabIndex = 7;
            // 
            // TBL_2Right
            // 
            this.TBL_2Right.ColumnCount = 1;
            this.TBL_2Right.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TBL_2Right.Controls.Add(this.PB_Preview, 0, 0);
            this.TBL_2Right.Controls.Add(this.UC_Result, 0, 1);
            this.TBL_2Right.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TBL_2Right.Location = new System.Drawing.Point(151, 3);
            this.TBL_2Right.Name = "TBL_2Right";
            this.TBL_2Right.RowCount = 2;
            this.TBL_2Right.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 83.36134F));
            this.TBL_2Right.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.63865F));
            this.TBL_2Right.Size = new System.Drawing.Size(828, 595);
            this.TBL_2Right.TabIndex = 8;
            // 
            // UC_CameraCtrl
            // 
            this.UC_CameraCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UC_CameraCtrl.Location = new System.Drawing.Point(3, 3);
            this.UC_CameraCtrl.Name = "UC_CameraCtrl";
            this.UC_CameraCtrl.Size = new System.Drawing.Size(136, 291);
            this.UC_CameraCtrl.TabIndex = 0;
            // 
            // UC_MotorCtrl
            // 
            this.UC_MotorCtrl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.UC_MotorCtrl.Location = new System.Drawing.Point(3, 339);
            this.UC_MotorCtrl.Name = "UC_MotorCtrl";
            this.UC_MotorCtrl.Size = new System.Drawing.Size(136, 253);
            this.UC_MotorCtrl.TabIndex = 1;
            // 
            // UC_Result
            // 
            this.UC_Result.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UC_Result.Location = new System.Drawing.Point(3, 498);
            this.UC_Result.Name = "UC_Result";
            this.UC_Result.Size = new System.Drawing.Size(822, 94);
            this.UC_Result.TabIndex = 7;
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
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CameraCtrlForm_FormClosing);
            this.TBL_1RightLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PB_Preview)).EndInit();
            this.TBL_2Left.ResumeLayout(false);
            this.TBL_2Right.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel TBL_1RightLeft;
        private System.Windows.Forms.PictureBox PB_Preview;
        private System.Windows.Forms.TableLayoutPanel TBL_2Left;
        private CameraCtrlUC UC_CameraCtrl;
        private ui.MotorCtrlUC UC_MotorCtrl;
        private System.Windows.Forms.TableLayoutPanel TBL_2Right;
        private ui.ResultUC UC_Result;
    }
}