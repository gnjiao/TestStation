namespace TestStation
{
    partial class CameraCtrlUC
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.BTN_Read = new System.Windows.Forms.Button();
            this.BTN_Calculate = new System.Windows.Forms.Button();
            this.BTN_Open = new System.Windows.Forms.Button();
            this.BTN_Close = new System.Windows.Forms.Button();
            this.BTN_SetBin = new System.Windows.Forms.Button();
            this.CB_SetRoi = new System.Windows.Forms.CheckBox();
            this.CB_Color = new System.Windows.Forms.CheckBox();
            this.BTN_Load = new System.Windows.Forms.Button();
            this.BTN_ResetRoi = new System.Windows.Forms.Button();
            this.TBP_Infomation = new System.Windows.Forms.TableLayoutPanel();
            this.LB_CircleInfo = new System.Windows.Forms.Label();
            this.LB_RoiInfo = new System.Windows.Forms.Label();
            this.TBP_Infomation.SuspendLayout();
            this.SuspendLayout();
            // 
            // BTN_Read
            // 
            this.BTN_Read.Dock = System.Windows.Forms.DockStyle.Top;
            this.BTN_Read.Location = new System.Drawing.Point(0, 37);
            this.BTN_Read.Name = "BTN_Read";
            this.BTN_Read.Size = new System.Drawing.Size(147, 36);
            this.BTN_Read.TabIndex = 0;
            this.BTN_Read.Text = "Read";
            this.BTN_Read.UseVisualStyleBackColor = true;
            this.BTN_Read.Click += new System.EventHandler(this.BTN_Read_Click);
            // 
            // BTN_Calculate
            // 
            this.BTN_Calculate.Dock = System.Windows.Forms.DockStyle.Top;
            this.BTN_Calculate.Location = new System.Drawing.Point(0, 109);
            this.BTN_Calculate.Name = "BTN_Calculate";
            this.BTN_Calculate.Size = new System.Drawing.Size(147, 38);
            this.BTN_Calculate.TabIndex = 3;
            this.BTN_Calculate.Text = "Calculate";
            this.BTN_Calculate.UseVisualStyleBackColor = true;
            this.BTN_Calculate.Click += new System.EventHandler(this.BTN_Calculate_Click);
            // 
            // BTN_Open
            // 
            this.BTN_Open.Dock = System.Windows.Forms.DockStyle.Top;
            this.BTN_Open.Location = new System.Drawing.Point(0, 0);
            this.BTN_Open.Name = "BTN_Open";
            this.BTN_Open.Size = new System.Drawing.Size(147, 37);
            this.BTN_Open.TabIndex = 4;
            this.BTN_Open.Text = "Open";
            this.BTN_Open.UseVisualStyleBackColor = true;
            this.BTN_Open.Click += new System.EventHandler(this.BTN_Open_Click);
            // 
            // BTN_Close
            // 
            this.BTN_Close.Dock = System.Windows.Forms.DockStyle.Top;
            this.BTN_Close.Location = new System.Drawing.Point(0, 147);
            this.BTN_Close.Name = "BTN_Close";
            this.BTN_Close.Size = new System.Drawing.Size(147, 38);
            this.BTN_Close.TabIndex = 6;
            this.BTN_Close.Text = "Close";
            this.BTN_Close.UseVisualStyleBackColor = true;
            this.BTN_Close.Click += new System.EventHandler(this.BTN_Close_Click);
            // 
            // BTN_SetBin
            // 
            this.BTN_SetBin.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BTN_SetBin.Location = new System.Drawing.Point(0, 382);
            this.BTN_SetBin.Name = "BTN_SetBin";
            this.BTN_SetBin.Size = new System.Drawing.Size(147, 38);
            this.BTN_SetBin.TabIndex = 8;
            this.BTN_SetBin.Text = "Set Bin";
            this.BTN_SetBin.UseVisualStyleBackColor = true;
            this.BTN_SetBin.Click += new System.EventHandler(this.BTN_SetBin_Click);
            // 
            // CB_SetRoi
            // 
            this.CB_SetRoi.Appearance = System.Windows.Forms.Appearance.Button;
            this.CB_SetRoi.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.CB_SetRoi.Location = new System.Drawing.Point(0, 344);
            this.CB_SetRoi.Name = "CB_SetRoi";
            this.CB_SetRoi.Size = new System.Drawing.Size(147, 38);
            this.CB_SetRoi.TabIndex = 9;
            this.CB_SetRoi.Text = "Set Roi";
            this.CB_SetRoi.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.CB_SetRoi.UseVisualStyleBackColor = true;
            this.CB_SetRoi.CheckedChanged += new System.EventHandler(this.CB_SetRoi_CheckedChanged);
            // 
            // CB_Color
            // 
            this.CB_Color.AutoSize = true;
            this.CB_Color.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.CB_Color.Location = new System.Drawing.Point(0, 322);
            this.CB_Color.Name = "CB_Color";
            this.CB_Color.Size = new System.Drawing.Size(147, 22);
            this.CB_Color.TabIndex = 10;
            this.CB_Color.Text = "Color";
            this.CB_Color.UseVisualStyleBackColor = true;
            this.CB_Color.CheckedChanged += new System.EventHandler(this.CB_Color_CheckedChanged);
            // 
            // BTN_Load
            // 
            this.BTN_Load.Dock = System.Windows.Forms.DockStyle.Top;
            this.BTN_Load.Location = new System.Drawing.Point(0, 73);
            this.BTN_Load.Name = "BTN_Load";
            this.BTN_Load.Size = new System.Drawing.Size(147, 36);
            this.BTN_Load.TabIndex = 11;
            this.BTN_Load.Text = "Load";
            this.BTN_Load.UseVisualStyleBackColor = true;
            this.BTN_Load.Click += new System.EventHandler(this.BTN_Load_Click);
            // 
            // BTN_ResetRoi
            // 
            this.BTN_ResetRoi.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BTN_ResetRoi.Location = new System.Drawing.Point(0, 420);
            this.BTN_ResetRoi.Name = "BTN_ResetRoi";
            this.BTN_ResetRoi.Size = new System.Drawing.Size(147, 38);
            this.BTN_ResetRoi.TabIndex = 12;
            this.BTN_ResetRoi.Text = "Reset";
            this.BTN_ResetRoi.UseVisualStyleBackColor = true;
            // 
            // TBP_Infomation
            // 
            this.TBP_Infomation.ColumnCount = 1;
            this.TBP_Infomation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TBP_Infomation.Controls.Add(this.LB_RoiInfo, 0, 1);
            this.TBP_Infomation.Controls.Add(this.LB_CircleInfo, 0, 0);
            this.TBP_Infomation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TBP_Infomation.Location = new System.Drawing.Point(0, 185);
            this.TBP_Infomation.Name = "TBP_Infomation";
            this.TBP_Infomation.RowCount = 2;
            this.TBP_Infomation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TBP_Infomation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TBP_Infomation.Size = new System.Drawing.Size(147, 137);
            this.TBP_Infomation.TabIndex = 13;
            // 
            // LB_CircleInfo
            // 
            this.LB_CircleInfo.AutoSize = true;
            this.LB_CircleInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LB_CircleInfo.Location = new System.Drawing.Point(3, 0);
            this.LB_CircleInfo.Name = "LB_CircleInfo";
            this.LB_CircleInfo.Size = new System.Drawing.Size(141, 68);
            this.LB_CircleInfo.TabIndex = 0;
            this.LB_CircleInfo.Text = "CircleInfo";
            // 
            // LB_RoiInfo
            // 
            this.LB_RoiInfo.AutoSize = true;
            this.LB_RoiInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LB_RoiInfo.Location = new System.Drawing.Point(3, 68);
            this.LB_RoiInfo.Name = "LB_RoiInfo";
            this.LB_RoiInfo.Size = new System.Drawing.Size(141, 69);
            this.LB_RoiInfo.TabIndex = 1;
            this.LB_RoiInfo.Text = "RoiInfo";
            // 
            // CameraCtrlUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TBP_Infomation);
            this.Controls.Add(this.CB_Color);
            this.Controls.Add(this.CB_SetRoi);
            this.Controls.Add(this.BTN_SetBin);
            this.Controls.Add(this.BTN_ResetRoi);
            this.Controls.Add(this.BTN_Close);
            this.Controls.Add(this.BTN_Calculate);
            this.Controls.Add(this.BTN_Load);
            this.Controls.Add(this.BTN_Read);
            this.Controls.Add(this.BTN_Open);
            this.Name = "CameraCtrlUC";
            this.Size = new System.Drawing.Size(147, 458);
            this.TBP_Infomation.ResumeLayout(false);
            this.TBP_Infomation.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BTN_Read;
        private System.Windows.Forms.Button BTN_Calculate;
        private System.Windows.Forms.Button BTN_Open;
        private System.Windows.Forms.Button BTN_Close;
        private System.Windows.Forms.Button BTN_SetBin;
        private System.Windows.Forms.CheckBox CB_SetRoi;
        private System.Windows.Forms.CheckBox CB_Color;
        private System.Windows.Forms.Button BTN_Load;
        private System.Windows.Forms.Button BTN_ResetRoi;
        private System.Windows.Forms.TableLayoutPanel TBP_Infomation;
        private System.Windows.Forms.Label LB_RoiInfo;
        private System.Windows.Forms.Label LB_CircleInfo;
    }
}
