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
            this.BTN_Calculate.Location = new System.Drawing.Point(12, 98);
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
            this.PB_Preview.Location = new System.Drawing.Point(146, 12);
            this.PB_Preview.Name = "PB_Preview";
            this.PB_Preview.Size = new System.Drawing.Size(587, 426);
            this.PB_Preview.TabIndex = 5;
            this.PB_Preview.TabStop = false;
            // 
            // BTN_Close
            // 
            this.BTN_Close.Location = new System.Drawing.Point(12, 142);
            this.BTN_Close.Name = "BTN_Close";
            this.BTN_Close.Size = new System.Drawing.Size(128, 38);
            this.BTN_Close.TabIndex = 6;
            this.BTN_Close.Text = "Close";
            this.BTN_Close.UseVisualStyleBackColor = true;
            this.BTN_Close.Click += new System.EventHandler(this.BTN_Close_Click);
            // 
            // FormCameraCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
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
    }
}

