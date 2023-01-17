namespace AutoFrame
{
    partial class Form_Manual
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
            this.tabControl_manual = new AutoFrameUI.RightTab();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabControl_manual.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl_manual
            // 
            this.tabControl_manual.Alignment = System.Windows.Forms.TabAlignment.Right;
            this.tabControl_manual.Controls.Add(this.tabPage1);
            this.tabControl_manual.Controls.Add(this.tabPage2);
            this.tabControl_manual.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_manual.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl_manual.ItemSize = new System.Drawing.Size(35, 150);
            this.tabControl_manual.Location = new System.Drawing.Point(0, 0);
            this.tabControl_manual.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl_manual.Multiline = true;
            this.tabControl_manual.Name = "tabControl_manual";
            this.tabControl_manual.SelectedIndex = 0;
            this.tabControl_manual.Size = new System.Drawing.Size(1280, 1006);
            this.tabControl_manual.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl_manual.TabColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(176)))), ((int)(((byte)(177)))));
            this.tabControl_manual.TabIndex = 3;
            this.tabControl_manual.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabControl_manual_DrawItem);
            this.tabControl_manual.SelectedIndexChanged += new System.EventHandler(this.tabControl_manual_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1122, 998);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1122, 1016);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // Form_Manual
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1280, 1006);
            this.Controls.Add(this.tabControl_manual);
            this.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "Form_Manual";
            this.Text = "手动模式";
            this.Load += new System.EventHandler(this.Form_Manual_Load);
            this.tabControl_manual.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private AutoFrameUI.RightTab tabControl_manual;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
    }
}