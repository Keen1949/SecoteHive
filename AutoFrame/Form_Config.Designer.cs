namespace AutoFrame
{
    partial class Form_Config
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label_Tips = new System.Windows.Forms.Label();
            this.pictureBox_HomeMode = new System.Windows.Forms.PictureBox();
            this.dataGridView_Axis = new System.Windows.Forms.DataGridView();
            this.button_Apply = new System.Windows.Forms.Button();
            this.button_Save = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_HomeMode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Axis)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(2, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(848, 689);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label_Tips);
            this.tabPage1.Controls.Add(this.pictureBox_HomeMode);
            this.tabPage1.Controls.Add(this.dataGridView_Axis);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(840, 663);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "轴配置";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label_Tips
            // 
            this.label_Tips.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Tips.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_Tips.ForeColor = System.Drawing.Color.Blue;
            this.label_Tips.Location = new System.Drawing.Point(537, 375);
            this.label_Tips.Name = "label_Tips";
            this.label_Tips.Size = new System.Drawing.Size(300, 285);
            this.label_Tips.TabIndex = 3;
            // 
            // pictureBox_HomeMode
            // 
            this.pictureBox_HomeMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_HomeMode.Location = new System.Drawing.Point(3, 375);
            this.pictureBox_HomeMode.Name = "pictureBox_HomeMode";
            this.pictureBox_HomeMode.Size = new System.Drawing.Size(528, 288);
            this.pictureBox_HomeMode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_HomeMode.TabIndex = 2;
            this.pictureBox_HomeMode.TabStop = false;
            // 
            // dataGridView_Axis
            // 
            this.dataGridView_Axis.AllowUserToAddRows = false;
            this.dataGridView_Axis.AllowUserToDeleteRows = false;
            this.dataGridView_Axis.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView_Axis.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Axis.Location = new System.Drawing.Point(3, 3);
            this.dataGridView_Axis.Name = "dataGridView_Axis";
            this.dataGridView_Axis.RowTemplate.Height = 23;
            this.dataGridView_Axis.Size = new System.Drawing.Size(834, 366);
            this.dataGridView_Axis.TabIndex = 0;
            this.dataGridView_Axis.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_Axis_CellValueChanged);
            this.dataGridView_Axis.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView_Axis_ColumnWidthChanged);
            this.dataGridView_Axis.CurrentCellChanged += new System.EventHandler(this.dataGridView_Axis_CurrentCellChanged);
            this.dataGridView_Axis.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dataGridView_Axis_Scroll);
            // 
            // button_Apply
            // 
            this.button_Apply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Apply.Location = new System.Drawing.Point(857, 35);
            this.button_Apply.Name = "button_Apply";
            this.button_Apply.Size = new System.Drawing.Size(75, 36);
            this.button_Apply.TabIndex = 1;
            this.button_Apply.Text = "应用";
            this.button_Apply.UseVisualStyleBackColor = true;
            this.button_Apply.Click += new System.EventHandler(this.button_Apply_Click);
            // 
            // button_Save
            // 
            this.button_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Save.Location = new System.Drawing.Point(857, 97);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(75, 36);
            this.button_Save.TabIndex = 1;
            this.button_Save.Text = "保存";
            this.button_Save.UseVisualStyleBackColor = true;
            this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // Form_Config
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(939, 694);
            this.Controls.Add(this.button_Save);
            this.Controls.Add(this.button_Apply);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form_Config";
            this.Text = "配置";
            this.Load += new System.EventHandler(this.Form_Config_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_HomeMode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Axis)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dataGridView_Axis;
        private System.Windows.Forms.Button button_Apply;
        private System.Windows.Forms.Button button_Save;
        private System.Windows.Forms.PictureBox pictureBox_HomeMode;
        private System.Windows.Forms.Label label_Tips;
    }
}