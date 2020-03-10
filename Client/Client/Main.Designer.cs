namespace Client
{
    partial class Main
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
            this.textboxOutput = new System.Windows.Forms.RichTextBox();
            this.textboxInput = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.listViewUsers = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // textboxOutput
            // 
            this.textboxOutput.Location = new System.Drawing.Point(207, 22);
            this.textboxOutput.Name = "textboxOutput";
            this.textboxOutput.ReadOnly = true;
            this.textboxOutput.Size = new System.Drawing.Size(384, 316);
            this.textboxOutput.TabIndex = 0;
            this.textboxOutput.Text = "";
            // 
            // textboxInput
            // 
            this.textboxInput.Location = new System.Drawing.Point(207, 368);
            this.textboxInput.Name = "textboxInput";
            this.textboxInput.Size = new System.Drawing.Size(384, 25);
            this.textboxInput.TabIndex = 1;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(207, 399);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(384, 25);
            this.btnSend.TabIndex = 2;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // listViewUsers
            // 
            this.listViewUsers.Location = new System.Drawing.Point(27, 22);
            this.listViewUsers.Name = "listViewUsers";
            this.listViewUsers.Size = new System.Drawing.Size(163, 402);
            this.listViewUsers.TabIndex = 3;
            this.listViewUsers.UseCompatibleStateImageBehavior = false;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 464);
            this.Controls.Add(this.listViewUsers);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.textboxInput);
            this.Controls.Add(this.textboxOutput);
            this.Name = "Main";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox textboxOutput;
        private System.Windows.Forms.TextBox textboxInput;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.ListView listViewUsers;
    }
}

