namespace AutoFrame
{
    partial class Form_ParmSaveAs
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
            this.button_ok = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.textBox_directory = new System.Windows.Forms.TextBox();
            this.button_saveDir = new System.Windows.Forms.Button();
            this.textBox_Modifier = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_fileDescribe = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(33, 227);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(69, 33);
            this.button_ok.TabIndex = 0;
            this.button_ok.Text = "确定 ";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_cancel.Location = new System.Drawing.Point(188, 227);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(69, 33);
            this.button_cancel.TabIndex = 1;
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // textBox_directory
            // 
            this.textBox_directory.Font = new System.Drawing.Font("宋体", 9F);
            this.textBox_directory.Location = new System.Drawing.Point(6, 12);
            this.textBox_directory.Multiline = true;
            this.textBox_directory.Name = "textBox_directory";
            this.textBox_directory.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_directory.Size = new System.Drawing.Size(200, 54);
            this.textBox_directory.TabIndex = 2;
            // 
            // button_saveDir
            // 
            this.button_saveDir.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button_saveDir.Font = new System.Drawing.Font("宋体", 9F);
            this.button_saveDir.Location = new System.Drawing.Point(210, 12);
            this.button_saveDir.Name = "button_saveDir";
            this.button_saveDir.Size = new System.Drawing.Size(70, 23);
            this.button_saveDir.TabIndex = 3;
            this.button_saveDir.Text = "保存路径";
            this.button_saveDir.UseVisualStyleBackColor = true;
            this.button_saveDir.Click += new System.EventHandler(this.button_saveDir_Click);
            // 
            // textBox_Modifier
            // 
            this.textBox_Modifier.Font = new System.Drawing.Font("宋体", 12F);
            this.textBox_Modifier.Location = new System.Drawing.Point(6, 90);
            this.textBox_Modifier.Name = "textBox_Modifier";
            this.textBox_Modifier.Size = new System.Drawing.Size(200, 26);
            this.textBox_Modifier.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F);
            this.label2.Location = new System.Drawing.Point(210, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "修改者";
            // 
            // textBox_fileDescribe
            // 
            this.textBox_fileDescribe.Font = new System.Drawing.Font("宋体", 12F);
            this.textBox_fileDescribe.Location = new System.Drawing.Point(6, 140);
            this.textBox_fileDescribe.Multiline = true;
            this.textBox_fileDescribe.Name = "textBox_fileDescribe";
            this.textBox_fileDescribe.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_fileDescribe.Size = new System.Drawing.Size(200, 80);
            this.textBox_fileDescribe.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 12F);
            this.label3.Location = new System.Drawing.Point(210, 151);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "文件描述";
            // 
            // Form_ParmSaveAs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button_saveDir);
            this.Controls.Add(this.textBox_fileDescribe);
            this.Controls.Add(this.textBox_Modifier);
            this.Controls.Add(this.textBox_directory);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_ok);
            this.Name = "Form_ParmSaveAs";
            this.Text = "Form_ParmSaveAs";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Form_ParmSaveAs_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.TextBox textBox_directory;
        private System.Windows.Forms.Button button_saveDir;
        private System.Windows.Forms.TextBox textBox_Modifier;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox textBox_fileDescribe;
    }
}