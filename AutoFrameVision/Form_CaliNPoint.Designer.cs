namespace AutoFrameVision
{
    partial class Form_CaliNPoint
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle24 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle23 = new System.Windows.Forms.DataGridViewCellStyle();
            this.textBox_path = new System.Windows.Forms.TextBox();
            this.button_load = new System.Windows.Forms.Button();
            this.button_save = new System.Windows.Forms.Button();
            this.dataGridView_point = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button_calc = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button_save_as = new System.Windows.Forms.Button();
            this.textBox_xPix = new System.Windows.Forms.TextBox();
            this.textBox_yPix = new System.Windows.Forms.TextBox();
            this.button_test = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_yPos = new System.Windows.Forms.TextBox();
            this.textBox_xPos = new System.Windows.Forms.TextBox();
            this.listBox_point_info = new System.Windows.Forms.ListBox();
            this.dataGridView_center = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.listBox_center_info = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_point)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_center)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox_path
            // 
            this.textBox_path.Location = new System.Drawing.Point(87, 10);
            this.textBox_path.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox_path.Name = "textBox_path";
            this.textBox_path.Size = new System.Drawing.Size(533, 26);
            this.textBox_path.TabIndex = 2;
            // 
            // button_load
            // 
            this.button_load.Location = new System.Drawing.Point(2, 8);
            this.button_load.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_load.Name = "button_load";
            this.button_load.Size = new System.Drawing.Size(77, 30);
            this.button_load.TabIndex = 4;
            this.button_load.Text = "加载文件";
            this.button_load.UseVisualStyleBackColor = true;
            this.button_load.Click += new System.EventHandler(this.button_load_Click);
            // 
            // button_save
            // 
            this.button_save.Location = new System.Drawing.Point(628, 8);
            this.button_save.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(60, 30);
            this.button_save.TabIndex = 4;
            this.button_save.Text = "保存";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.button_save_Click);
            // 
            // dataGridView_point
            // 
            this.dataGridView_point.AllowUserToResizeColumns = false;
            this.dataGridView_point.AllowUserToResizeRows = false;
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle17.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle17.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle17.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle17.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_point.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle17;
            this.dataGridView_point.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView_point.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.RaisedVertical;
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(202)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle18.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle18.ForeColor = System.Drawing.SystemColors.Info;
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(202)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_point.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle18;
            this.dataGridView_point.ColumnHeadersHeight = 32;
            this.dataGridView_point.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView_point.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5});
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle20.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle20.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle20.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle20.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle20.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle20.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_point.DefaultCellStyle = dataGridViewCellStyle20;
            this.dataGridView_point.EnableHeadersVisualStyles = false;
            this.dataGridView_point.Location = new System.Drawing.Point(2, 44);
            this.dataGridView_point.Name = "dataGridView_point";
            this.dataGridView_point.RowHeadersVisible = false;
            this.dataGridView_point.RowHeadersWidth = 30;
            this.dataGridView_point.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView_point.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dataGridView_point.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dataGridView_point.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGridView_point.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_point.RowTemplate.Height = 26;
            this.dataGridView_point.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView_point.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView_point.Size = new System.Drawing.Size(458, 271);
            this.dataGridView_point.TabIndex = 5;
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle19.Format = "N0";
            dataGridViewCellStyle19.NullValue = null;
            dataGridViewCellStyle19.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle19;
            this.Column1.HeaderText = "No.";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column2.HeaderText = "xPixel";
            this.Column2.Name = "Column2";
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column3.HeaderText = "yPixel";
            this.Column3.Name = "Column3";
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "xPos";
            this.Column4.Name = "Column4";
            // 
            // Column5
            // 
            this.Column5.HeaderText = "yPos";
            this.Column5.Name = "Column5";
            // 
            // button_calc
            // 
            this.button_calc.Location = new System.Drawing.Point(385, 321);
            this.button_calc.Name = "button_calc";
            this.button_calc.Size = new System.Drawing.Size(75, 72);
            this.button_calc.TabIndex = 6;
            this.button_calc.Text = "标定计算";
            this.button_calc.UseVisualStyleBackColor = true;
            this.button_calc.Click += new System.EventHandler(this.button_calc_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "calib";
            this.openFileDialog1.Filter = "标定文件|*.ini";
            // 
            // button_save_as
            // 
            this.button_save_as.Location = new System.Drawing.Point(696, 8);
            this.button_save_as.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_save_as.Name = "button_save_as";
            this.button_save_as.Size = new System.Drawing.Size(64, 30);
            this.button_save_as.TabIndex = 4;
            this.button_save_as.Text = "另存为";
            this.button_save_as.UseVisualStyleBackColor = true;
            this.button_save_as.Click += new System.EventHandler(this.button_save_as_Click);
            // 
            // textBox_xPix
            // 
            this.textBox_xPix.Location = new System.Drawing.Point(29, 410);
            this.textBox_xPix.Name = "textBox_xPix";
            this.textBox_xPix.Size = new System.Drawing.Size(86, 26);
            this.textBox_xPix.TabIndex = 7;
            // 
            // textBox_yPix
            // 
            this.textBox_yPix.Location = new System.Drawing.Point(147, 411);
            this.textBox_yPix.Name = "textBox_yPix";
            this.textBox_yPix.Size = new System.Drawing.Size(86, 26);
            this.textBox_yPix.TabIndex = 7;
            // 
            // button_test
            // 
            this.button_test.Location = new System.Drawing.Point(239, 408);
            this.button_test.Name = "button_test";
            this.button_test.Size = new System.Drawing.Size(75, 33);
            this.button_test.TabIndex = 8;
            this.button_test.Text = "测算";
            this.button_test.UseVisualStyleBackColor = true;
            this.button_test.Click += new System.EventHandler(this.button_test_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 414);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 20);
            this.label1.TabIndex = 9;
            this.label1.Text = "X:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(121, 414);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 20);
            this.label2.TabIndex = 9;
            this.label2.Text = "Y:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(440, 414);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 20);
            this.label3.TabIndex = 12;
            this.label3.Text = "Y:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(321, 414);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(21, 20);
            this.label4.TabIndex = 13;
            this.label4.Text = "X:";
            // 
            // textBox_yPos
            // 
            this.textBox_yPos.Location = new System.Drawing.Point(466, 411);
            this.textBox_yPos.Name = "textBox_yPos";
            this.textBox_yPos.Size = new System.Drawing.Size(86, 26);
            this.textBox_yPos.TabIndex = 10;
            // 
            // textBox_xPos
            // 
            this.textBox_xPos.Location = new System.Drawing.Point(348, 410);
            this.textBox_xPos.Name = "textBox_xPos";
            this.textBox_xPos.Size = new System.Drawing.Size(86, 26);
            this.textBox_xPos.TabIndex = 11;
            // 
            // listBox_point_info
            // 
            this.listBox_point_info.FormattingEnabled = true;
            this.listBox_point_info.ItemHeight = 20;
            this.listBox_point_info.Location = new System.Drawing.Point(2, 318);
            this.listBox_point_info.Name = "listBox_point_info";
            this.listBox_point_info.Size = new System.Drawing.Size(377, 84);
            this.listBox_point_info.TabIndex = 14;
            // 
            // dataGridView_center
            // 
            this.dataGridView_center.AllowUserToResizeColumns = false;
            this.dataGridView_center.AllowUserToResizeRows = false;
            dataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle21.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle21.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle21.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle21.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle21.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_center.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle21;
            this.dataGridView_center.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView_center.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.RaisedVertical;
            dataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle22.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(202)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle22.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle22.ForeColor = System.Drawing.SystemColors.Info;
            dataGridViewCellStyle22.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(202)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle22.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle22.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_center.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle22;
            this.dataGridView_center.ColumnHeadersHeight = 32;
            this.dataGridView_center.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView_center.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3});
            dataGridViewCellStyle24.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle24.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle24.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle24.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle24.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle24.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle24.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_center.DefaultCellStyle = dataGridViewCellStyle24;
            this.dataGridView_center.EnableHeadersVisualStyles = false;
            this.dataGridView_center.Location = new System.Drawing.Point(466, 44);
            this.dataGridView_center.Name = "dataGridView_center";
            this.dataGridView_center.RowHeadersVisible = false;
            this.dataGridView_center.RowHeadersWidth = 30;
            this.dataGridView_center.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView_center.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dataGridView_center.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dataGridView_center.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGridView_center.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_center.RowTemplate.Height = 26;
            this.dataGridView_center.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView_center.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView_center.Size = new System.Drawing.Size(294, 271);
            this.dataGridView_center.TabIndex = 15;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle23.Format = "N0";
            dataGridViewCellStyle23.NullValue = null;
            dataGridViewCellStyle23.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle23;
            this.dataGridViewTextBoxColumn1.HeaderText = "No.";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn1.Width = 40;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.HeaderText = "item";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn3.HeaderText = "data";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // listBox_center_info
            // 
            this.listBox_center_info.FormattingEnabled = true;
            this.listBox_center_info.ItemHeight = 20;
            this.listBox_center_info.Location = new System.Drawing.Point(466, 321);
            this.listBox_center_info.Name = "listBox_center_info";
            this.listBox_center_info.Size = new System.Drawing.Size(284, 84);
            this.listBox_center_info.TabIndex = 14;
            // 
            // Form_CaliNPoint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(762, 451);
            this.Controls.Add(this.dataGridView_center);
            this.Controls.Add(this.listBox_center_info);
            this.Controls.Add(this.listBox_point_info);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox_yPos);
            this.Controls.Add(this.textBox_xPos);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_test);
            this.Controls.Add(this.textBox_yPix);
            this.Controls.Add(this.textBox_xPix);
            this.Controls.Add(this.button_calc);
            this.Controls.Add(this.dataGridView_point);
            this.Controls.Add(this.button_save_as);
            this.Controls.Add(this.button_save);
            this.Controls.Add(this.button_load);
            this.Controls.Add(this.textBox_path);
            this.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_CaliNPoint";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "坐标转换";
            this.Load += new System.EventHandler(this.CaliNPoint_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_point)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_center)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBox_path;
        private System.Windows.Forms.Button button_load;
        private System.Windows.Forms.Button button_save;
        private System.Windows.Forms.DataGridView dataGridView_point;
        private System.Windows.Forms.Button button_calc;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button_save_as;
        private System.Windows.Forms.TextBox textBox_xPix;
        private System.Windows.Forms.TextBox textBox_yPix;
        private System.Windows.Forms.Button button_test;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_yPos;
        private System.Windows.Forms.TextBox textBox_xPos;
        private System.Windows.Forms.ListBox listBox_point_info;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridView dataGridView_center;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.ListBox listBox_center_info;
    }
}