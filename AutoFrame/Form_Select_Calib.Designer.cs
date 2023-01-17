namespace AutoFrame
{
    partial class Form_Select_Calib
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
            this.button_OK = new System.Windows.Forms.Button();
            this.button_Exit = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.button_Save = new System.Windows.Forms.Button();
            this.button_Apply = new System.Windows.Forms.Button();
            this.button_Del = new System.Windows.Forms.Button();
            this.dataGridView_Calib = new System.Windows.Forms.DataGridView();
            this.panel_Calib = new System.Windows.Forms.Panel();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Calib)).BeginInit();
            this.SuspendLayout();
            // 
            // button_OK
            // 
            this.button_OK.Location = new System.Drawing.Point(583, 24);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(83, 40);
            this.button_OK.TabIndex = 1;
            this.button_OK.Text = "确定";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // button_Exit
            // 
            this.button_Exit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Exit.Location = new System.Drawing.Point(583, 70);
            this.button_Exit.Name = "button_Exit";
            this.button_Exit.Size = new System.Drawing.Size(83, 40);
            this.button_Exit.TabIndex = 1;
            this.button_Exit.Text = "退出";
            this.button_Exit.UseVisualStyleBackColor = true;
            this.button_Exit.Click += new System.EventHandler(this.button2_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(682, 332);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.button_OK);
            this.tabPage1.Controls.Add(this.button_Exit);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(674, 306);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "选择标定项";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.panel_Calib);
            this.groupBox1.Location = new System.Drawing.Point(9, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(568, 291);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "标定项";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.button_Save);
            this.tabPage2.Controls.Add(this.button_Apply);
            this.tabPage2.Controls.Add(this.button_Del);
            this.tabPage2.Controls.Add(this.dataGridView_Calib);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(674, 306);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "管理标定项";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // button_Save
            // 
            this.button_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Save.Location = new System.Drawing.Point(581, 89);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(85, 35);
            this.button_Save.TabIndex = 1;
            this.button_Save.Text = "保存";
            this.button_Save.UseVisualStyleBackColor = true;
            this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // button_Apply
            // 
            this.button_Apply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Apply.Location = new System.Drawing.Point(581, 48);
            this.button_Apply.Name = "button_Apply";
            this.button_Apply.Size = new System.Drawing.Size(85, 35);
            this.button_Apply.TabIndex = 1;
            this.button_Apply.Text = "应用";
            this.button_Apply.UseVisualStyleBackColor = true;
            this.button_Apply.Click += new System.EventHandler(this.button_Apply_Click);
            // 
            // button_Del
            // 
            this.button_Del.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Del.Location = new System.Drawing.Point(581, 7);
            this.button_Del.Name = "button_Del";
            this.button_Del.Size = new System.Drawing.Size(85, 35);
            this.button_Del.TabIndex = 1;
            this.button_Del.Text = "删除";
            this.button_Del.UseVisualStyleBackColor = true;
            this.button_Del.Click += new System.EventHandler(this.button_Del_Click);
            // 
            // dataGridView_Calib
            // 
            this.dataGridView_Calib.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView_Calib.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Calib.Location = new System.Drawing.Point(3, 3);
            this.dataGridView_Calib.Name = "dataGridView_Calib";
            this.dataGridView_Calib.RowTemplate.Height = 23;
            this.dataGridView_Calib.Size = new System.Drawing.Size(572, 300);
            this.dataGridView_Calib.TabIndex = 0;
            this.dataGridView_Calib.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView_Calib_ColumnWidthChanged);
            this.dataGridView_Calib.CurrentCellChanged += new System.EventHandler(this.dataGridView_Calib_CurrentCellChanged);
            this.dataGridView_Calib.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dataGridView_Calib_Scroll);
            // 
            // panel_Calib
            // 
            this.panel_Calib.BackColor = System.Drawing.Color.Gainsboro;
            this.panel_Calib.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_Calib.Location = new System.Drawing.Point(3, 17);
            this.panel_Calib.Name = "panel_Calib";
            this.panel_Calib.Size = new System.Drawing.Size(562, 271);
            this.panel_Calib.TabIndex = 0;
            // 
            // Form_Select_Calib
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 332);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form_Select_Calib";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "请选择标定项";
            this.Load += new System.EventHandler(this.Form_Select_Calib_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Calib)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.Button button_Exit;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button button_Save;
        private System.Windows.Forms.Button button_Apply;
        private System.Windows.Forms.Button button_Del;
        private System.Windows.Forms.DataGridView dataGridView_Calib;
        private System.Windows.Forms.Panel panel_Calib;
    }
}