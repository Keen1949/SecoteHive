namespace AutoFrame
{
    partial class Form_Notify
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
            this.components = new System.ComponentModel.Container();
            this.listBoxEx1 = new ToolEx.ListBoxEx();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // listBoxEx1
            // 
            this.listBoxEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxEx1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.listBoxEx1.FormattingEnabled = true;
            this.listBoxEx1.HorizontalScrollbar = true;
            this.listBoxEx1.Location = new System.Drawing.Point(0, 0);
            this.listBoxEx1.Name = "listBoxEx1";
            this.listBoxEx1.Size = new System.Drawing.Size(257, 270);
            this.listBoxEx1.TabIndex = 0;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form_Notify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(257, 270);
            this.Controls.Add(this.listBoxEx1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form_Notify";
            this.Text = "Form_Notify";
            this.ResumeLayout(false);

        }

        #endregion

        private ToolEx.ListBoxEx listBoxEx1;
        private System.Windows.Forms.Timer timer1;
    }
}