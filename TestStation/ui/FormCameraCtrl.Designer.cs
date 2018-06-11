using Utils;

namespace TestStation
{
    partial class FormCameraCtrl
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.BTN_Read = new System.Windows.Forms.Button();
            this.TB_Log = new System.Windows.Forms.TextBox();
            this.BTN_Calculate = new System.Windows.Forms.Button();
            this.BTN_Open = new System.Windows.Forms.Button();
            this.PB_Preview = new System.Windows.Forms.PictureBox();
            this.BTN_Close = new System.Windows.Forms.Button();
            this.BTN_SetBin = new System.Windows.Forms.Button();
            this.CB_SetRoi = new System.Windows.Forms.CheckBox();
            this.CB_Color = new System.Windows.Forms.CheckBox();
            this.BTN_Load = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Preview)).BeginInit();
            this.SuspendLayout();
            // 
            // BTN_Read
            // 
            this.BTN_Read.Location = new System.Drawing.Point(12, 56);
            this.BTN_Read.Name = "BTN_Read";
            this.BTN_Read.Size = new System.Drawing.Size(128, 36);
            this.BTN_Read.TabIndex = 0;
            this.BTN_Read.Text = "Read";
            this.BTN_Read.UseVisualStyleBackColor = true;
            this.BTN_Read.Click += new System.EventHandler(this.BTN_Read_Click);
            // 
            // TB_Log
            // 
            this.TB_Log.Location = new System.Drawing.Point(739, 12);
            this.TB_Log.Multiline = true;
            this.TB_Log.Name = "TB_Log";
            this.TB_Log.Size = new System.Drawing.Size(49, 426);
            this.TB_Log.TabIndex = 2;
            // 
            // BTN_Calculate
            // 
            this.BTN_Calculate.Location = new System.Drawing.Point(12, 140);
            this.BTN_Calculate.Name = "BTN_Calculate";
            this.BTN_Calculate.Size = new System.Drawing.Size(128, 38);
            this.BTN_Calculate.TabIndex = 3;
            this.BTN_Calculate.Text = "Calculate";
            this.BTN_Calculate.UseVisualStyleBackColor = true;
            this.BTN_Calculate.Click += new System.EventHandler(this.BTN_Calculate_Click);
            // 
            // BTN_Open
            // 
            this.BTN_Open.Location = new System.Drawing.Point(12, 13);
            this.BTN_Open.Name = "BTN_Open";
            this.BTN_Open.Size = new System.Drawing.Size(128, 37);
            this.BTN_Open.TabIndex = 4;
            this.BTN_Open.Text = "Open";
            this.BTN_Open.UseVisualStyleBackColor = true;
            this.BTN_Open.Click += new System.EventHandler(this.BTN_Open_Click);
            // 
            // PB_Preview
            // 
            this.PB_Preview.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.PB_Preview.Location = new System.Drawing.Point(146, 12);
            this.PB_Preview.Name = "PB_Preview";
            this.PB_Preview.Size = new System.Drawing.Size(587, 426);
            this.PB_Preview.TabIndex = 5;
            this.PB_Preview.TabStop = false;
            this.PB_Preview.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PB_Preview_MouseDown);
            this.PB_Preview.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PB_Preview_MouseMove);
            this.PB_Preview.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PB_Preview_MouseUp);
            // 
            // BTN_Close
            // 
            this.BTN_Close.Location = new System.Drawing.Point(12, 184);
            this.BTN_Close.Name = "BTN_Close";
            this.BTN_Close.Size = new System.Drawing.Size(128, 38);
            this.BTN_Close.TabIndex = 6;
            this.BTN_Close.Text = "Close";
            this.BTN_Close.UseVisualStyleBackColor = true;
            this.BTN_Close.Click += new System.EventHandler(this.BTN_Close_Click);
            // 
            // BTN_SetBin
            // 
            this.BTN_SetBin.Location = new System.Drawing.Point(12, 400);
            this.BTN_SetBin.Name = "BTN_SetBin";
            this.BTN_SetBin.Size = new System.Drawing.Size(128, 38);
            this.BTN_SetBin.TabIndex = 8;
            this.BTN_SetBin.Text = "Set Bin";
            this.BTN_SetBin.UseVisualStyleBackColor = true;
            this.BTN_SetBin.Click += new System.EventHandler(this.BTN_SetBin_Click);
            // 
            // CB_SetRoi
            // 
            this.CB_SetRoi.Appearance = System.Windows.Forms.Appearance.Button;
            this.CB_SetRoi.Location = new System.Drawing.Point(12, 355);
            this.CB_SetRoi.Name = "CB_SetRoi";
            this.CB_SetRoi.Size = new System.Drawing.Size(128, 38);
            this.CB_SetRoi.TabIndex = 9;
            this.CB_SetRoi.Text = "Set Roi";
            this.CB_SetRoi.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.CB_SetRoi.UseVisualStyleBackColor = true;
            this.CB_SetRoi.CheckedChanged += new System.EventHandler(this.CB_SetRoi_CheckedChanged);
            // 
            // CB_Color
            // 
            this.CB_Color.AutoSize = true;
            this.CB_Color.Location = new System.Drawing.Point(12, 319);
            this.CB_Color.Name = "CB_Color";
            this.CB_Color.Size = new System.Drawing.Size(79, 22);
            this.CB_Color.TabIndex = 10;
            this.CB_Color.Text = "Color";
            this.CB_Color.UseVisualStyleBackColor = true;
            this.CB_Color.CheckedChanged += new System.EventHandler(this.CB_Color_CheckedChanged);
            // 
            // BTN_Load
            // 
            this.BTN_Load.Location = new System.Drawing.Point(12, 98);
            this.BTN_Load.Name = "BTN_Load";
            this.BTN_Load.Size = new System.Drawing.Size(128, 36);
            this.BTN_Load.TabIndex = 11;
            this.BTN_Load.Text = "Load";
            this.BTN_Load.UseVisualStyleBackColor = true;
            this.BTN_Load.Click += new System.EventHandler(this.BTN_Load_Click);
            // 
            // FormCameraCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.BTN_Load);
            this.Controls.Add(this.CB_Color);
            this.Controls.Add(this.CB_SetRoi);
            this.Controls.Add(this.BTN_SetBin);
            this.Controls.Add(this.BTN_Close);
            this.Controls.Add(this.PB_Preview);
            this.Controls.Add(this.BTN_Open);
            this.Controls.Add(this.BTN_Calculate);
            this.Controls.Add(this.TB_Log);
            this.Controls.Add(this.BTN_Read);
            this.Name = "FormCameraCtrl";
            this.Text = "Camera Control";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCameraCtrl_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.PB_Preview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BTN_Read;
        private System.Windows.Forms.TextBox TB_Log;
        private System.Windows.Forms.Button BTN_Calculate;
        private System.Windows.Forms.Button BTN_Open;
        private System.Windows.Forms.PictureBox PB_Preview;
        private System.Windows.Forms.Button BTN_Close;
        private System.Windows.Forms.Button BTN_SetBin;
        private System.Windows.Forms.CheckBox CB_SetRoi;
        private System.Windows.Forms.CheckBox CB_Color;
        private System.Windows.Forms.Button BTN_Load;
    }
}

