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
            this.components = new System.ComponentModel.Container();
            this.BTN_SetBin = new System.Windows.Forms.Button();
            this.CB_SetRoi = new System.Windows.Forms.CheckBox();
            this.CB_Color = new System.Windows.Forms.CheckBox();
            this.BTN_ResetRoi = new System.Windows.Forms.Button();
            this.LB_RoiInfo = new System.Windows.Forms.Label();
            this.LB_CircleInfo = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.CMB_CameraType = new System.Windows.Forms.ComboBox();
            this.BTN_Open = new System.Windows.Forms.Button();
            this.BTN_Read = new System.Windows.Forms.Button();
            this.BTN_Load = new System.Windows.Forms.Button();
            this.BTN_ImgAnalyze = new System.Windows.Forms.Button();
            this.BTN_Close = new System.Windows.Forms.Button();
            this.TB_Distance = new System.Windows.Forms.TextBox();
            this.BTN_Calculate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BTN_SetBin
            // 
            this.BTN_SetBin.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BTN_SetBin.Enabled = false;
            this.BTN_SetBin.Location = new System.Drawing.Point(0, 431);
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
            this.CB_SetRoi.Enabled = false;
            this.CB_SetRoi.Location = new System.Drawing.Point(0, 393);
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
            this.CB_Color.Enabled = false;
            this.CB_Color.Location = new System.Drawing.Point(0, 371);
            this.CB_Color.Name = "CB_Color";
            this.CB_Color.Size = new System.Drawing.Size(147, 22);
            this.CB_Color.TabIndex = 10;
            this.CB_Color.Text = "Color";
            this.CB_Color.UseVisualStyleBackColor = true;
            this.CB_Color.CheckedChanged += new System.EventHandler(this.CB_Color_CheckedChanged);
            // 
            // BTN_ResetRoi
            // 
            this.BTN_ResetRoi.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BTN_ResetRoi.Enabled = false;
            this.BTN_ResetRoi.Location = new System.Drawing.Point(0, 469);
            this.BTN_ResetRoi.Name = "BTN_ResetRoi";
            this.BTN_ResetRoi.Size = new System.Drawing.Size(147, 38);
            this.BTN_ResetRoi.TabIndex = 12;
            this.BTN_ResetRoi.Text = "Reset";
            this.BTN_ResetRoi.UseVisualStyleBackColor = true;
            // 
            // LB_RoiInfo
            // 
            this.LB_RoiInfo.AutoSize = true;
            this.LB_RoiInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.LB_RoiInfo.Location = new System.Drawing.Point(0, 353);
            this.LB_RoiInfo.Name = "LB_RoiInfo";
            this.LB_RoiInfo.Size = new System.Drawing.Size(71, 18);
            this.LB_RoiInfo.TabIndex = 1;
            this.LB_RoiInfo.Text = "RoiInfo";
            // 
            // LB_CircleInfo
            // 
            this.LB_CircleInfo.AutoSize = true;
            this.LB_CircleInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.LB_CircleInfo.Location = new System.Drawing.Point(0, 277);
            this.LB_CircleInfo.Name = "LB_CircleInfo";
            this.LB_CircleInfo.Size = new System.Drawing.Size(98, 18);
            this.LB_CircleInfo.TabIndex = 0;
            this.LB_CircleInfo.Text = "CircleInfo";
            // 
            // CMB_CameraType
            // 
            this.CMB_CameraType.Dock = System.Windows.Forms.DockStyle.Top;
            this.CMB_CameraType.FormattingEnabled = true;
            this.CMB_CameraType.Items.AddRange(new object[] {
            "Camera Type",
            "Type A",
            "Type B"});
            this.CMB_CameraType.Location = new System.Drawing.Point(0, 0);
            this.CMB_CameraType.Name = "CMB_CameraType";
            this.CMB_CameraType.Size = new System.Drawing.Size(147, 26);
            this.CMB_CameraType.TabIndex = 14;
            // 
            // BTN_Open
            // 
            this.BTN_Open.Dock = System.Windows.Forms.DockStyle.Top;
            this.BTN_Open.Location = new System.Drawing.Point(0, 26);
            this.BTN_Open.Name = "BTN_Open";
            this.BTN_Open.Size = new System.Drawing.Size(147, 37);
            this.BTN_Open.TabIndex = 4;
            this.BTN_Open.Text = "Open";
            this.BTN_Open.UseVisualStyleBackColor = true;
            this.BTN_Open.Click += new System.EventHandler(this.BTN_Open_Click);
            // 
            // BTN_Read
            // 
            this.BTN_Read.Dock = System.Windows.Forms.DockStyle.Top;
            this.BTN_Read.Location = new System.Drawing.Point(0, 63);
            this.BTN_Read.Name = "BTN_Read";
            this.BTN_Read.Size = new System.Drawing.Size(147, 36);
            this.BTN_Read.TabIndex = 0;
            this.BTN_Read.Text = "Read";
            this.BTN_Read.UseVisualStyleBackColor = true;
            this.BTN_Read.Click += new System.EventHandler(this.BTN_Read_Click);
            // 
            // BTN_Load
            // 
            this.BTN_Load.Dock = System.Windows.Forms.DockStyle.Top;
            this.BTN_Load.Location = new System.Drawing.Point(0, 99);
            this.BTN_Load.Name = "BTN_Load";
            this.BTN_Load.Size = new System.Drawing.Size(147, 36);
            this.BTN_Load.TabIndex = 11;
            this.BTN_Load.Text = "Load";
            this.BTN_Load.UseVisualStyleBackColor = true;
            this.BTN_Load.Click += new System.EventHandler(this.BTN_Load_Click);
            // 
            // BTN_ImgAnalyze
            // 
            this.BTN_ImgAnalyze.Dock = System.Windows.Forms.DockStyle.Top;
            this.BTN_ImgAnalyze.Location = new System.Drawing.Point(0, 135);
            this.BTN_ImgAnalyze.Name = "BTN_ImgAnalyze";
            this.BTN_ImgAnalyze.Size = new System.Drawing.Size(147, 38);
            this.BTN_ImgAnalyze.TabIndex = 3;
            this.BTN_ImgAnalyze.Text = "Image Analyze";
            this.BTN_ImgAnalyze.UseVisualStyleBackColor = true;
            this.BTN_ImgAnalyze.Click += new System.EventHandler(this.BTN_ImgAnalyze_Click);
            // 
            // BTN_Close
            // 
            this.BTN_Close.Dock = System.Windows.Forms.DockStyle.Top;
            this.BTN_Close.Location = new System.Drawing.Point(0, 211);
            this.BTN_Close.Name = "BTN_Close";
            this.BTN_Close.Size = new System.Drawing.Size(147, 38);
            this.BTN_Close.TabIndex = 6;
            this.BTN_Close.Text = "Close";
            this.BTN_Close.UseVisualStyleBackColor = true;
            this.BTN_Close.Click += new System.EventHandler(this.BTN_Close_Click);
            // 
            // TB_Distance
            // 
            this.TB_Distance.Dock = System.Windows.Forms.DockStyle.Top;
            this.TB_Distance.Location = new System.Drawing.Point(0, 249);
            this.TB_Distance.Name = "TB_Distance";
            this.TB_Distance.Size = new System.Drawing.Size(147, 28);
            this.TB_Distance.TabIndex = 15;
            this.TB_Distance.Text = "Distance(mm)";
            this.TB_Distance.Click += new System.EventHandler(this.TB_Distance_Click);
            // 
            // BTN_Calculate
            // 
            this.BTN_Calculate.Dock = System.Windows.Forms.DockStyle.Top;
            this.BTN_Calculate.Location = new System.Drawing.Point(0, 173);
            this.BTN_Calculate.Name = "BTN_Calculate";
            this.BTN_Calculate.Size = new System.Drawing.Size(147, 38);
            this.BTN_Calculate.TabIndex = 16;
            this.BTN_Calculate.Text = "Calculate";
            this.BTN_Calculate.UseVisualStyleBackColor = true;
            this.BTN_Calculate.Click += new System.EventHandler(this.BTN_Calculate_Click);
            // 
            // CameraCtrlUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.LB_CircleInfo);
            this.Controls.Add(this.TB_Distance);
            this.Controls.Add(this.BTN_Close);
            this.Controls.Add(this.BTN_Calculate);
            this.Controls.Add(this.BTN_ImgAnalyze);
            this.Controls.Add(this.BTN_Load);
            this.Controls.Add(this.BTN_Read);
            this.Controls.Add(this.BTN_Open);
            this.Controls.Add(this.CMB_CameraType);
            this.Controls.Add(this.LB_RoiInfo);
            this.Controls.Add(this.CB_Color);
            this.Controls.Add(this.CB_SetRoi);
            this.Controls.Add(this.BTN_SetBin);
            this.Controls.Add(this.BTN_ResetRoi);
            this.Name = "CameraCtrlUC";
            this.Size = new System.Drawing.Size(147, 507);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button BTN_SetBin;
        private System.Windows.Forms.CheckBox CB_SetRoi;
        private System.Windows.Forms.CheckBox CB_Color;
        private System.Windows.Forms.Button BTN_ResetRoi;
        private System.Windows.Forms.Label LB_RoiInfo;
        private System.Windows.Forms.Label LB_CircleInfo;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ComboBox CMB_CameraType;
        private System.Windows.Forms.Button BTN_Open;
        private System.Windows.Forms.Button BTN_Read;
        private System.Windows.Forms.Button BTN_Load;
        private System.Windows.Forms.Button BTN_ImgAnalyze;
        private System.Windows.Forms.Button BTN_Close;
        private System.Windows.Forms.TextBox TB_Distance;
        private System.Windows.Forms.Button BTN_Calculate;
    }
}
