namespace TestStation.ui
{
    partial class MotorCtrlUC
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
            this.label1 = new System.Windows.Forms.Label();
            this.BTN_Z1GoHome = new System.Windows.Forms.Button();
            this.TB_Z1Distance = new System.Windows.Forms.TextBox();
            this.BTN_Z1MoveUp = new System.Windows.Forms.Button();
            this.BTN_Z1MoveDown = new System.Windows.Forms.Button();
            this.BTN_Z2MoveDown = new System.Windows.Forms.Button();
            this.BTN_Z2MoveUp = new System.Windows.Forms.Button();
            this.TB_Z2Distance = new System.Windows.Forms.TextBox();
            this.BTN_Z2GoHome = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.LB_Z1Position = new System.Windows.Forms.Label();
            this.LB_Z2Position = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(7, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Z1";
            // 
            // BTN_Z1GoHome
            // 
            this.BTN_Z1GoHome.Location = new System.Drawing.Point(3, 96);
            this.BTN_Z1GoHome.Name = "BTN_Z1GoHome";
            this.BTN_Z1GoHome.Size = new System.Drawing.Size(135, 28);
            this.BTN_Z1GoHome.TabIndex = 1;
            this.BTN_Z1GoHome.Text = "Home";
            this.BTN_Z1GoHome.UseVisualStyleBackColor = true;
            this.BTN_Z1GoHome.Click += new System.EventHandler(this.BTN_Z1GoHome_Click);
            // 
            // TB_Z1Distance
            // 
            this.TB_Z1Distance.Location = new System.Drawing.Point(3, 31);
            this.TB_Z1Distance.Name = "TB_Z1Distance";
            this.TB_Z1Distance.Size = new System.Drawing.Size(131, 28);
            this.TB_Z1Distance.TabIndex = 2;
            // 
            // BTN_Z1MoveUp
            // 
            this.BTN_Z1MoveUp.Location = new System.Drawing.Point(3, 63);
            this.BTN_Z1MoveUp.Name = "BTN_Z1MoveUp";
            this.BTN_Z1MoveUp.Size = new System.Drawing.Size(59, 34);
            this.BTN_Z1MoveUp.TabIndex = 3;
            this.BTN_Z1MoveUp.Text = "Up";
            this.BTN_Z1MoveUp.UseVisualStyleBackColor = true;
            this.BTN_Z1MoveUp.Click += new System.EventHandler(this.BTN_Z1MoveUp_Click);
            // 
            // BTN_Z1MoveDown
            // 
            this.BTN_Z1MoveDown.Location = new System.Drawing.Point(80, 63);
            this.BTN_Z1MoveDown.Name = "BTN_Z1MoveDown";
            this.BTN_Z1MoveDown.Size = new System.Drawing.Size(58, 34);
            this.BTN_Z1MoveDown.TabIndex = 4;
            this.BTN_Z1MoveDown.Text = "Down";
            this.BTN_Z1MoveDown.UseVisualStyleBackColor = true;
            this.BTN_Z1MoveDown.Click += new System.EventHandler(this.BTN_Z1MoveDown_Click);
            // 
            // BTN_Z2MoveDown
            // 
            this.BTN_Z2MoveDown.Location = new System.Drawing.Point(80, 186);
            this.BTN_Z2MoveDown.Name = "BTN_Z2MoveDown";
            this.BTN_Z2MoveDown.Size = new System.Drawing.Size(58, 32);
            this.BTN_Z2MoveDown.TabIndex = 9;
            this.BTN_Z2MoveDown.Text = "Down";
            this.BTN_Z2MoveDown.UseVisualStyleBackColor = true;
            this.BTN_Z2MoveDown.Click += new System.EventHandler(this.BTN_Z2MoveDown_Click);
            // 
            // BTN_Z2MoveUp
            // 
            this.BTN_Z2MoveUp.Location = new System.Drawing.Point(3, 186);
            this.BTN_Z2MoveUp.Name = "BTN_Z2MoveUp";
            this.BTN_Z2MoveUp.Size = new System.Drawing.Size(59, 32);
            this.BTN_Z2MoveUp.TabIndex = 8;
            this.BTN_Z2MoveUp.Text = "Up";
            this.BTN_Z2MoveUp.UseVisualStyleBackColor = true;
            this.BTN_Z2MoveUp.Click += new System.EventHandler(this.BTN_Z2MoveUp_Click);
            // 
            // TB_Z2Distance
            // 
            this.TB_Z2Distance.Location = new System.Drawing.Point(3, 154);
            this.TB_Z2Distance.Name = "TB_Z2Distance";
            this.TB_Z2Distance.Size = new System.Drawing.Size(131, 28);
            this.TB_Z2Distance.TabIndex = 7;
            // 
            // BTN_Z2GoHome
            // 
            this.BTN_Z2GoHome.Location = new System.Drawing.Point(3, 217);
            this.BTN_Z2GoHome.Name = "BTN_Z2GoHome";
            this.BTN_Z2GoHome.Size = new System.Drawing.Size(135, 32);
            this.BTN_Z2GoHome.TabIndex = 6;
            this.BTN_Z2GoHome.Text = "Home";
            this.BTN_Z2GoHome.UseVisualStyleBackColor = true;
            this.BTN_Z2GoHome.Click += new System.EventHandler(this.BTN_Z2GoHome_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(8, 133);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 18);
            this.label2.TabIndex = 5;
            this.label2.Text = "Z2";
            // 
            // LB_Z1Position
            // 
            this.LB_Z1Position.AutoSize = true;
            this.LB_Z1Position.Location = new System.Drawing.Point(42, 10);
            this.LB_Z1Position.Name = "LB_Z1Position";
            this.LB_Z1Position.Size = new System.Drawing.Size(44, 18);
            this.LB_Z1Position.TabIndex = 10;
            this.LB_Z1Position.Text = "0 mm";
            // 
            // LB_Z2Position
            // 
            this.LB_Z2Position.AutoSize = true;
            this.LB_Z2Position.Location = new System.Drawing.Point(42, 133);
            this.LB_Z2Position.Name = "LB_Z2Position";
            this.LB_Z2Position.Size = new System.Drawing.Size(44, 18);
            this.LB_Z2Position.TabIndex = 11;
            this.LB_Z2Position.Text = "0 mm";
            // 
            // MotorCtrlUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.LB_Z2Position);
            this.Controls.Add(this.LB_Z1Position);
            this.Controls.Add(this.BTN_Z2MoveDown);
            this.Controls.Add(this.BTN_Z2MoveUp);
            this.Controls.Add(this.TB_Z2Distance);
            this.Controls.Add(this.BTN_Z2GoHome);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BTN_Z1MoveDown);
            this.Controls.Add(this.BTN_Z1MoveUp);
            this.Controls.Add(this.TB_Z1Distance);
            this.Controls.Add(this.BTN_Z1GoHome);
            this.Controls.Add(this.label1);
            this.Name = "MotorCtrlUC";
            this.Size = new System.Drawing.Size(141, 252);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BTN_Z1GoHome;
        private System.Windows.Forms.TextBox TB_Z1Distance;
        private System.Windows.Forms.Button BTN_Z1MoveUp;
        private System.Windows.Forms.Button BTN_Z1MoveDown;
        private System.Windows.Forms.Button BTN_Z2MoveDown;
        private System.Windows.Forms.Button BTN_Z2MoveUp;
        private System.Windows.Forms.TextBox TB_Z2Distance;
        private System.Windows.Forms.Button BTN_Z2GoHome;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label LB_Z1Position;
        private System.Windows.Forms.Label LB_Z2Position;
    }
}
