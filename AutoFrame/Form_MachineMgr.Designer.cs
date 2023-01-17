namespace AutoFrame
{
    partial class Form_MachineMgr
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_Reset = new System.Windows.Forms.Button();
            this.textBox_MachineTime = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_SoftwareTime = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button_Del = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.comboBox_JobMode = new System.Windows.Forms.ComboBox();
            this.comboBox_JobCount = new System.Windows.Forms.ComboBox();
            this.textBox_DeviceID = new System.Windows.Forms.TextBox();
            this.textBox_Air = new System.Windows.Forms.TextBox();
            this.textBox_Power = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_Current = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_DeviceName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox_Voltage = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button_Save = new System.Windows.Forms.Button();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_Reset);
            this.groupBox1.Controls.Add(this.textBox_MachineTime);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox_SoftwareTime);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(427, 78);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "时间";
            // 
            // button_Reset
            // 
            this.button_Reset.Location = new System.Drawing.Point(344, 14);
            this.button_Reset.Name = "button_Reset";
            this.button_Reset.Size = new System.Drawing.Size(75, 51);
            this.button_Reset.TabIndex = 2;
            this.button_Reset.Text = "重置";
            this.button_Reset.UseVisualStyleBackColor = true;
            this.button_Reset.Click += new System.EventHandler(this.button_Reset_Click);
            // 
            // textBox_MachineTime
            // 
            this.textBox_MachineTime.Location = new System.Drawing.Point(103, 44);
            this.textBox_MachineTime.Name = "textBox_MachineTime";
            this.textBox_MachineTime.ReadOnly = true;
            this.textBox_MachineTime.Size = new System.Drawing.Size(235, 21);
            this.textBox_MachineTime.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "设备运行时间:";
            // 
            // textBox_SoftwareTime
            // 
            this.textBox_SoftwareTime.Location = new System.Drawing.Point(103, 17);
            this.textBox_SoftwareTime.Name = "textBox_SoftwareTime";
            this.textBox_SoftwareTime.ReadOnly = true;
            this.textBox_SoftwareTime.Size = new System.Drawing.Size(235, 21);
            this.textBox_SoftwareTime.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "软件运行时间:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button_Del);
            this.groupBox2.Controls.Add(this.dataGridView1);
            this.groupBox2.Controls.Add(this.comboBox_JobMode);
            this.groupBox2.Controls.Add(this.comboBox_JobCount);
            this.groupBox2.Controls.Add(this.textBox_DeviceID);
            this.groupBox2.Controls.Add(this.textBox_Air);
            this.groupBox2.Controls.Add(this.textBox_Power);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.textBox_Current);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.textBox_DeviceName);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.textBox_Voltage);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(13, 98);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(569, 337);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "设备信息";
            // 
            // button_Del
            // 
            this.button_Del.Location = new System.Drawing.Point(484, 51);
            this.button_Del.Name = "button_Del";
            this.button_Del.Size = new System.Drawing.Size(75, 51);
            this.button_Del.TabIndex = 2;
            this.button_Del.Text = "删除";
            this.button_Del.UseVisualStyleBackColor = true;
            this.button_Del.Click += new System.EventHandler(this.button_Del_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column5,
            this.Column4});
            this.dataGridView1.Location = new System.Drawing.Point(11, 108);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(551, 223);
            this.dataGridView1.TabIndex = 3;
            // 
            // comboBox_JobMode
            // 
            this.comboBox_JobMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_JobMode.FormattingEnabled = true;
            this.comboBox_JobMode.Location = new System.Drawing.Point(224, 45);
            this.comboBox_JobMode.Name = "comboBox_JobMode";
            this.comboBox_JobMode.Size = new System.Drawing.Size(70, 20);
            this.comboBox_JobMode.TabIndex = 2;
            // 
            // comboBox_JobCount
            // 
            this.comboBox_JobCount.FormattingEnabled = true;
            this.comboBox_JobCount.Location = new System.Drawing.Point(74, 45);
            this.comboBox_JobCount.Name = "comboBox_JobCount";
            this.comboBox_JobCount.Size = new System.Drawing.Size(70, 20);
            this.comboBox_JobCount.TabIndex = 2;
            // 
            // textBox_DeviceID
            // 
            this.textBox_DeviceID.Location = new System.Drawing.Point(291, 17);
            this.textBox_DeviceID.Name = "textBox_DeviceID";
            this.textBox_DeviceID.Size = new System.Drawing.Size(149, 21);
            this.textBox_DeviceID.TabIndex = 1;
            // 
            // textBox_Air
            // 
            this.textBox_Air.Location = new System.Drawing.Point(343, 45);
            this.textBox_Air.Name = "textBox_Air";
            this.textBox_Air.Size = new System.Drawing.Size(97, 21);
            this.textBox_Air.TabIndex = 1;
            // 
            // textBox_Power
            // 
            this.textBox_Power.Location = new System.Drawing.Point(343, 76);
            this.textBox_Power.Name = "textBox_Power";
            this.textBox_Power.Size = new System.Drawing.Size(97, 21);
            this.textBox_Power.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(305, 49);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 12);
            this.label10.TabIndex = 0;
            this.label10.Text = "气压:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(238, 21);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 12);
            this.label8.TabIndex = 0;
            this.label8.Text = "设备ID:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(305, 80);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "功率:";
            // 
            // textBox_Current
            // 
            this.textBox_Current.Location = new System.Drawing.Point(197, 76);
            this.textBox_Current.Name = "textBox_Current";
            this.textBox_Current.Size = new System.Drawing.Size(97, 21);
            this.textBox_Current.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(159, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "电流:";
            // 
            // textBox_DeviceName
            // 
            this.textBox_DeviceName.Location = new System.Drawing.Point(74, 17);
            this.textBox_DeviceName.Name = "textBox_DeviceName";
            this.textBox_DeviceName.Size = new System.Drawing.Size(149, 21);
            this.textBox_DeviceName.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "设备名称:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(159, 49);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 12);
            this.label9.TabIndex = 0;
            this.label9.Text = "作业模式:";
            // 
            // textBox_Voltage
            // 
            this.textBox_Voltage.Location = new System.Drawing.Point(47, 76);
            this.textBox_Voltage.Name = "textBox_Voltage";
            this.textBox_Voltage.Size = new System.Drawing.Size(97, 21);
            this.textBox_Voltage.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 49);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "工位数量:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "电压:";
            // 
            // button_Save
            // 
            this.button_Save.Location = new System.Drawing.Point(497, 27);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(75, 51);
            this.button_Save.TabIndex = 2;
            this.button_Save.Text = "保存";
            this.button_Save.UseVisualStyleBackColor = true;
            this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Motion";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Robot";
            this.Column2.Name = "Column2";
            // 
            // Column3
            // 
            this.Column3.HeaderText = "CCD";
            this.Column3.Name = "Column3";
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Lens";
            this.Column5.Name = "Column5";
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Light";
            this.Column4.Name = "Column4";
            // 
            // Form_MachineMgr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 447);
            this.Controls.Add(this.button_Save);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form_MachineMgr";
            this.Text = "设备信息";
            this.Load += new System.EventHandler(this.Form_MachineMgr_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_Reset;
        private System.Windows.Forms.TextBox textBox_MachineTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_SoftwareTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox_Power;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_Current;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_Voltage;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_DeviceID;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox_DeviceName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBox_JobMode;
        private System.Windows.Forms.ComboBox comboBox_JobCount;
        private System.Windows.Forms.TextBox textBox_Air;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button_Save;
        private System.Windows.Forms.Button button_Del;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
    }
}