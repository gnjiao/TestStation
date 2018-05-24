namespace ImageTool
{
    partial class FormImgTool
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
            this.BTN_Open = new System.Windows.Forms.Button();
            this.BTN_Process = new System.Windows.Forms.Button();
            this.TB_Filepath = new System.Windows.Forms.TextBox();
            this.PB_Result = new System.Windows.Forms.PictureBox();
            this.TB_OutputPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Result)).BeginInit();
            this.SuspendLayout();
            // 
            // BTN_Open
            // 
            this.BTN_Open.Location = new System.Drawing.Point(12, 12);
            this.BTN_Open.Name = "BTN_Open";
            this.BTN_Open.Size = new System.Drawing.Size(88, 30);
            this.BTN_Open.TabIndex = 0;
            this.BTN_Open.Text = "Open";
            this.BTN_Open.UseVisualStyleBackColor = true;
            this.BTN_Open.Click += new System.EventHandler(this.BTN_Open_Click);
            // 
            // BTN_Process
            // 
            this.BTN_Process.Location = new System.Drawing.Point(12, 48);
            this.BTN_Process.Name = "BTN_Process";
            this.BTN_Process.Size = new System.Drawing.Size(88, 30);
            this.BTN_Process.TabIndex = 1;
            this.BTN_Process.Text = "Process";
            this.BTN_Process.UseVisualStyleBackColor = true;
            this.BTN_Process.Click += new System.EventHandler(this.BTN_Process_Click);
            // 
            // TB_Filepath
            // 
            this.TB_Filepath.Location = new System.Drawing.Point(116, 12);
            this.TB_Filepath.Name = "TB_Filepath";
            this.TB_Filepath.ReadOnly = true;
            this.TB_Filepath.Size = new System.Drawing.Size(672, 28);
            this.TB_Filepath.TabIndex = 2;
            this.TB_Filepath.Text = "D:\\work\\TestStation\\ImageTool\\Samples\\Test.jpg";
            // 
            // PB_Result
            // 
            this.PB_Result.Location = new System.Drawing.Point(12, 116);
            this.PB_Result.Name = "PB_Result";
            this.PB_Result.Size = new System.Drawing.Size(776, 459);
            this.PB_Result.TabIndex = 3;
            this.PB_Result.TabStop = false;
            // 
            // TB_OutputPath
            // 
            this.TB_OutputPath.Location = new System.Drawing.Point(116, 48);
            this.TB_OutputPath.Name = "TB_OutputPath";
            this.TB_OutputPath.ReadOnly = true;
            this.TB_OutputPath.Size = new System.Drawing.Size(672, 28);
            this.TB_OutputPath.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 18);
            this.label1.TabIndex = 5;
            this.label1.Text = "Preview";
            // 
            // FormImgTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 587);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TB_OutputPath);
            this.Controls.Add(this.PB_Result);
            this.Controls.Add(this.TB_Filepath);
            this.Controls.Add(this.BTN_Process);
            this.Controls.Add(this.BTN_Open);
            this.Name = "FormImgTool";
            this.Text = "Image Process";
            ((System.ComponentModel.ISupportInitialize)(this.PB_Result)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BTN_Open;
        private System.Windows.Forms.Button BTN_Process;
        private System.Windows.Forms.TextBox TB_Filepath;
        private System.Windows.Forms.PictureBox PB_Result;
        private System.Windows.Forms.TextBox TB_OutputPath;
        private System.Windows.Forms.Label label1;
    }
}

