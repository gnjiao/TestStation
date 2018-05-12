using Utils;

namespace TestStation
{
    partial class TestStation
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
            this.SuspendLayout();
            // 
            // BTN_Read
            // 
            this.BTN_Read.Location = new System.Drawing.Point(12, 20);
            this.BTN_Read.Name = "BTN_Read";
            this.BTN_Read.Size = new System.Drawing.Size(128, 23);
            this.BTN_Read.TabIndex = 0;
            this.BTN_Read.Text = "Read";
            this.BTN_Read.UseVisualStyleBackColor = true;
            this.BTN_Read.Click += new System.EventHandler(this.BTN_Read_Click);
            // 
            // TB_Log
            // 
            this.TB_Log.Location = new System.Drawing.Point(204, 12);
            this.TB_Log.Multiline = true;
            this.TB_Log.Name = "TB_Log";
            this.TB_Log.Size = new System.Drawing.Size(584, 426);
            this.TB_Log.TabIndex = 2;
            // 
            // BTN_Calculate
            // 
            this.BTN_Calculate.Location = new System.Drawing.Point(12, 49);
            this.BTN_Calculate.Name = "BTN_Calculate";
            this.BTN_Calculate.Size = new System.Drawing.Size(128, 23);
            this.BTN_Calculate.TabIndex = 3;
            this.BTN_Calculate.Text = "Calculate";
            this.BTN_Calculate.UseVisualStyleBackColor = true;
            this.BTN_Calculate.Click += new System.EventHandler(this.BTN_Calculate_Click);
            // 
            // TestStation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.BTN_Calculate);
            this.Controls.Add(this.TB_Log);
            this.Controls.Add(this.BTN_Read);
            this.Name = "TestStation";
            this.Text = "Test Station";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BTN_Read;
        private System.Windows.Forms.TextBox TB_Log;
        private System.Windows.Forms.Button BTN_Calculate;
    }
}

