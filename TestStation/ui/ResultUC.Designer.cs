namespace TestStation.ui
{
    partial class ResultUC
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
            this.DGV_Result = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Result)).BeginInit();
            this.SuspendLayout();
            // 
            // DGV_Result
            // 
            this.DGV_Result.AllowUserToAddRows = false;
            this.DGV_Result.AllowUserToDeleteRows = false;
            this.DGV_Result.AllowUserToResizeColumns = false;
            this.DGV_Result.AllowUserToResizeRows = false;
            this.DGV_Result.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.DGV_Result.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.DGV_Result.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGV_Result.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_Result.ColumnHeadersVisible = false;
            this.DGV_Result.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGV_Result.Location = new System.Drawing.Point(0, 0);
            this.DGV_Result.Name = "DGV_Result";
            this.DGV_Result.ReadOnly = true;
            this.DGV_Result.RowHeadersVisible = false;
            this.DGV_Result.RowTemplate.Height = 30;
            this.DGV_Result.Size = new System.Drawing.Size(639, 84);
            this.DGV_Result.TabIndex = 0;
            // 
            // ResultUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.DGV_Result);
            this.Name = "ResultUC";
            this.Size = new System.Drawing.Size(639, 84);
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Result)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView DGV_Result;
    }
}
