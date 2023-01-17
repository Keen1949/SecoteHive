namespace AutoFrame
{
    partial class Form_Image
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Image));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.roundPanel1 = new AutoFrameUI.RoundPanel(this.components);
            this.button_next = new AutoFrameUI.RoundButton();
            this.button_prev = new AutoFrameUI.RoundButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.roundPanel2 = new AutoFrameUI.RoundPanel(this.components);
            this.roundPanel3 = new AutoFrameUI.RoundPanel(this.components);
            this.roundPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.roundPanel2.SuspendLayout();
            this.roundPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "ico_folder.png");
            // 
            // roundPanel1
            // 
            this.roundPanel1._setRoundRadius = 12;
            this.roundPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.roundPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(235)))));
            this.roundPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.roundPanel1.Controls.Add(this.button_next);
            this.roundPanel1.Controls.Add(this.button_prev);
            this.roundPanel1.Controls.Add(this.pictureBox1);
            this.roundPanel1.Location = new System.Drawing.Point(5, 5);
            this.roundPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.roundPanel1.Name = "roundPanel1";
            this.roundPanel1.Size = new System.Drawing.Size(976, 614);
            this.roundPanel1.TabIndex = 12;
            // 
            // button_next
            // 
            this.button_next.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button_next.BackColor = System.Drawing.Color.Transparent;
            this.button_next.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.button_next.BaseColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.button_next.FlatAppearance.BorderSize = 0;
            this.button_next.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_next.ImageHeight = 80;
            this.button_next.ImageWidth = 80;
            this.button_next.Location = new System.Drawing.Point(544, 569);
            this.button_next.Name = "button_next";
            this.button_next.Radius = 10;
            this.button_next.Size = new System.Drawing.Size(91, 37);
            this.button_next.SpliteButtonWidth = 18;
            this.button_next.TabIndex = 6;
            this.button_next.Text = "Next";
            this.button_next.UseVisualStyleBackColor = false;
            this.button_next.Click += new System.EventHandler(this.button_next_Click);
            // 
            // button_prev
            // 
            this.button_prev.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button_prev.BackColor = System.Drawing.Color.Transparent;
            this.button_prev.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.button_prev.BaseColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.button_prev.FlatAppearance.BorderSize = 0;
            this.button_prev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_prev.ImageHeight = 80;
            this.button_prev.ImageWidth = 80;
            this.button_prev.Location = new System.Drawing.Point(352, 569);
            this.button_prev.Name = "button_prev";
            this.button_prev.Radius = 10;
            this.button_prev.Size = new System.Drawing.Size(91, 37);
            this.button_prev.SpliteButtonWidth = 18;
            this.button_prev.TabIndex = 7;
            this.button_prev.Text = "Prev";
            this.button_prev.UseVisualStyleBackColor = false;
            this.button_prev.Click += new System.EventHandler(this.button_prev_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.MidnightBlue;
            this.pictureBox1.Location = new System.Drawing.Point(3, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(970, 559);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBox1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 25;
            this.listBox1.Location = new System.Drawing.Point(3, 6);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(342, 277);
            this.listBox1.Sorted = true;
            this.listBox1.TabIndex = 1;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeView1.HideSelection = false;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.Indent = 16;
            this.treeView1.ItemHeight = 28;
            this.treeView1.Location = new System.Drawing.Point(3, 4);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(342, 291);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // roundPanel2
            // 
            this.roundPanel2._setRoundRadius = 8;
            this.roundPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.roundPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(235)))));
            this.roundPanel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.roundPanel2.Controls.Add(this.listBox1);
            this.roundPanel2.Location = new System.Drawing.Point(993, 321);
            this.roundPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.roundPanel2.Name = "roundPanel2";
            this.roundPanel2.Size = new System.Drawing.Size(348, 298);
            this.roundPanel2.TabIndex = 13;
            // 
            // roundPanel3
            // 
            this.roundPanel3._setRoundRadius = 8;
            this.roundPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.roundPanel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(235)))));
            this.roundPanel3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.roundPanel3.Controls.Add(this.treeView1);
            this.roundPanel3.Location = new System.Drawing.Point(993, 5);
            this.roundPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.roundPanel3.Name = "roundPanel3";
            this.roundPanel3.Size = new System.Drawing.Size(348, 301);
            this.roundPanel3.TabIndex = 13;
            // 
            // Form_Image
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1350, 628);
            this.Controls.Add(this.roundPanel3);
            this.Controls.Add(this.roundPanel2);
            this.Controls.Add(this.roundPanel1);
            this.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "Form_Image";
            this.Text = "手动模式";
            this.Load += new System.EventHandler(this.Form_Image_Load);
            this.roundPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.roundPanel2.ResumeLayout(false);
            this.roundPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AutoFrameUI.RoundPanel roundPanel1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ImageList imageList1;
        private AutoFrameUI.RoundButton button_next;
        private AutoFrameUI.RoundButton button_prev;
        private AutoFrameUI.RoundPanel roundPanel2;
        private AutoFrameUI.RoundPanel roundPanel3;
    }
}