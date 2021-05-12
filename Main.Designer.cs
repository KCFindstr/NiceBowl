
namespace NiceBowl
{
    partial class Main
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
            this.button获取窗口 = new System.Windows.Forms.Button();
            this.textBoxPID = new System.Windows.Forms.TextBox();
            this.labelPID = new System.Windows.Forms.Label();
            this.label轴 = new System.Windows.Forms.Label();
            this.textBox轴 = new System.Windows.Forms.TextBox();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.labelLog = new System.Windows.Forms.Label();
            this.buttonStart = new System.Windows.Forms.Button();
            this.textTime = new System.Windows.Forms.Label();
            this.labelTime = new System.Windows.Forms.Label();
            this.buttonCheck = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button获取窗口
            // 
            this.button获取窗口.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button获取窗口.Location = new System.Drawing.Point(206, 13);
            this.button获取窗口.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button获取窗口.Name = "button获取窗口";
            this.button获取窗口.Size = new System.Drawing.Size(122, 28);
            this.button获取窗口.TabIndex = 0;
            this.button获取窗口.Text = "获取窗口";
            this.button获取窗口.UseVisualStyleBackColor = true;
            this.button获取窗口.Click += new System.EventHandler(this.button获取窗口_Click);
            // 
            // textBoxPID
            // 
            this.textBoxPID.Location = new System.Drawing.Point(62, 18);
            this.textBoxPID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxPID.Name = "textBoxPID";
            this.textBoxPID.Size = new System.Drawing.Size(126, 23);
            this.textBoxPID.TabIndex = 1;
            // 
            // labelPID
            // 
            this.labelPID.AutoSize = true;
            this.labelPID.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPID.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelPID.Location = new System.Drawing.Point(26, 22);
            this.labelPID.Name = "labelPID";
            this.labelPID.Size = new System.Drawing.Size(28, 17);
            this.labelPID.TabIndex = 2;
            this.labelPID.Text = "PID";
            // 
            // label轴
            // 
            this.label轴.AutoSize = true;
            this.label轴.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label轴.Location = new System.Drawing.Point(34, 52);
            this.label轴.Name = "label轴";
            this.label轴.Size = new System.Drawing.Size(20, 17);
            this.label轴.TabIndex = 3;
            this.label轴.Text = "轴";
            // 
            // textBox轴
            // 
            this.textBox轴.Location = new System.Drawing.Point(62, 49);
            this.textBox轴.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox轴.Multiline = true;
            this.textBox轴.Name = "textBox轴";
            this.textBox轴.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox轴.Size = new System.Drawing.Size(266, 172);
            this.textBox轴.TabIndex = 4;
            // 
            // textBoxLog
            // 
            this.textBoxLog.Location = new System.Drawing.Point(63, 279);
            this.textBoxLog.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ReadOnly = true;
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLog.Size = new System.Drawing.Size(266, 135);
            this.textBoxLog.TabIndex = 5;
            // 
            // labelLog
            // 
            this.labelLog.AutoSize = true;
            this.labelLog.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLog.Location = new System.Drawing.Point(19, 282);
            this.labelLog.Name = "labelLog";
            this.labelLog.Size = new System.Drawing.Size(32, 17);
            this.labelLog.TabIndex = 6;
            this.labelLog.Text = "日志";
            // 
            // buttonStart
            // 
            this.buttonStart.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonStart.Location = new System.Drawing.Point(240, 229);
            this.buttonStart.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(89, 42);
            this.buttonStart.TabIndex = 7;
            this.buttonStart.Text = "开始";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // textTime
            // 
            this.textTime.AutoSize = true;
            this.textTime.Font = new System.Drawing.Font("Microsoft YaHei", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textTime.Location = new System.Drawing.Point(63, 418);
            this.textTime.Name = "textTime";
            this.textTime.Size = new System.Drawing.Size(171, 62);
            this.textTime.TabIndex = 8;
            this.textTime.Text = "未就绪";
            // 
            // labelTime
            // 
            this.labelTime.AutoSize = true;
            this.labelTime.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTime.Location = new System.Drawing.Point(19, 438);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(32, 17);
            this.labelTime.TabIndex = 9;
            this.labelTime.Text = "时间";
            // 
            // buttonCheck
            // 
            this.buttonCheck.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCheck.Location = new System.Drawing.Point(145, 229);
            this.buttonCheck.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonCheck.Name = "buttonCheck";
            this.buttonCheck.Size = new System.Drawing.Size(89, 42);
            this.buttonCheck.TabIndex = 10;
            this.buttonCheck.Text = "轴检";
            this.buttonCheck.UseVisualStyleBackColor = true;
            this.buttonCheck.Click += new System.EventHandler(this.buttonCheck_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(347, 488);
            this.Controls.Add(this.buttonCheck);
            this.Controls.Add(this.labelTime);
            this.Controls.Add(this.textTime);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.labelLog);
            this.Controls.Add(this.textBoxLog);
            this.Controls.Add(this.textBox轴);
            this.Controls.Add(this.label轴);
            this.Controls.Add(this.labelPID);
            this.Controls.Add(this.textBoxPID);
            this.Controls.Add(this.button获取窗口);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Main";
            this.Text = "蓝白碗";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button获取窗口;
        private System.Windows.Forms.TextBox textBoxPID;
        private System.Windows.Forms.Label labelPID;
        private System.Windows.Forms.Label label轴;
        private System.Windows.Forms.TextBox textBox轴;
        private System.Windows.Forms.TextBox textBoxLog;
        private System.Windows.Forms.Label labelLog;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Label textTime;
        private System.Windows.Forms.Label labelTime;
        private System.Windows.Forms.Button buttonCheck;
    }
}

