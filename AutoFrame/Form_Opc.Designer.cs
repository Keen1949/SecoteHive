namespace AutoFrame
{
    partial class Form_Opc
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
            this.label_ServerIP = new System.Windows.Forms.Label();
            this.textBox_ServerIP = new System.Windows.Forms.TextBox();
            this.label_ServerName = new System.Windows.Forms.Label();
            this.comboBox_ServerName = new System.Windows.Forms.ComboBox();
            this.label_GroupName = new System.Windows.Forms.Label();
            this.textBox_GroupName = new System.Windows.Forms.TextBox();
            this.label_UpdateRate = new System.Windows.Forms.Label();
            this.textBox_UpdateRate = new System.Windows.Forms.TextBox();
            this.dataGridView_opc = new System.Windows.Forms.DataGridView();
            this.ColumnTag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnQuality = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button_Load = new System.Windows.Forms.Button();
            this.button_Save = new System.Windows.Forms.Button();
            this.button_UpdateSelect = new System.Windows.Forms.Button();
            this.button_UpdateAll = new System.Windows.Forms.Button();
            this.checkBoxOpc = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_opc)).BeginInit();
            this.SuspendLayout();
            // 
            // label_ServerIP
            // 
            this.label_ServerIP.AutoSize = true;
            this.label_ServerIP.Location = new System.Drawing.Point(26, 19);
            this.label_ServerIP.Name = "label_ServerIP";
            this.label_ServerIP.Size = new System.Drawing.Size(77, 12);
            this.label_ServerIP.TabIndex = 0;
            this.label_ServerIP.Text = "OPC服务器IP:";
            // 
            // textBox_ServerIP
            // 
            this.textBox_ServerIP.Location = new System.Drawing.Point(116, 16);
            this.textBox_ServerIP.Name = "textBox_ServerIP";
            this.textBox_ServerIP.Size = new System.Drawing.Size(146, 21);
            this.textBox_ServerIP.TabIndex = 1;
            // 
            // label_ServerName
            // 
            this.label_ServerName.AutoSize = true;
            this.label_ServerName.Location = new System.Drawing.Point(298, 20);
            this.label_ServerName.Name = "label_ServerName";
            this.label_ServerName.Size = new System.Drawing.Size(89, 12);
            this.label_ServerName.TabIndex = 0;
            this.label_ServerName.Text = "OPC服务器名称:";
            // 
            // comboBox_ServerName
            // 
            this.comboBox_ServerName.FormattingEnabled = true;
            this.comboBox_ServerName.Location = new System.Drawing.Point(388, 17);
            this.comboBox_ServerName.Name = "comboBox_ServerName";
            this.comboBox_ServerName.Size = new System.Drawing.Size(298, 20);
            this.comboBox_ServerName.TabIndex = 2;
            // 
            // label_GroupName
            // 
            this.label_GroupName.AutoSize = true;
            this.label_GroupName.Location = new System.Drawing.Point(26, 58);
            this.label_GroupName.Name = "label_GroupName";
            this.label_GroupName.Size = new System.Drawing.Size(77, 12);
            this.label_GroupName.TabIndex = 0;
            this.label_GroupName.Text = "OPC群组名称:";
            // 
            // textBox_GroupName
            // 
            this.textBox_GroupName.Location = new System.Drawing.Point(116, 54);
            this.textBox_GroupName.Name = "textBox_GroupName";
            this.textBox_GroupName.Size = new System.Drawing.Size(146, 21);
            this.textBox_GroupName.TabIndex = 1;
            // 
            // label_UpdateRate
            // 
            this.label_UpdateRate.AutoSize = true;
            this.label_UpdateRate.Location = new System.Drawing.Point(298, 58);
            this.label_UpdateRate.Name = "label_UpdateRate";
            this.label_UpdateRate.Size = new System.Drawing.Size(83, 12);
            this.label_UpdateRate.TabIndex = 0;
            this.label_UpdateRate.Text = "刷新频率(ms):";
            // 
            // textBox_UpdateRate
            // 
            this.textBox_UpdateRate.Location = new System.Drawing.Point(388, 54);
            this.textBox_UpdateRate.Name = "textBox_UpdateRate";
            this.textBox_UpdateRate.Size = new System.Drawing.Size(108, 21);
            this.textBox_UpdateRate.TabIndex = 1;
            // 
            // dataGridView_opc
            // 
            this.dataGridView_opc.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_opc.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnTag,
            this.ColumnDesc,
            this.ColumnValue,
            this.ColumnQuality,
            this.ColumnTime});
            this.dataGridView_opc.Location = new System.Drawing.Point(12, 84);
            this.dataGridView_opc.Name = "dataGridView_opc";
            this.dataGridView_opc.RowTemplate.Height = 23;
            this.dataGridView_opc.Size = new System.Drawing.Size(674, 296);
            this.dataGridView_opc.TabIndex = 3;
            // 
            // ColumnTag
            // 
            this.ColumnTag.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnTag.FillWeight = 130F;
            this.ColumnTag.HeaderText = "Tag";
            this.ColumnTag.Name = "ColumnTag";
            this.ColumnTag.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnDesc
            // 
            this.ColumnDesc.FillWeight = 130F;
            this.ColumnDesc.HeaderText = "Desc";
            this.ColumnDesc.Name = "ColumnDesc";
            this.ColumnDesc.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnValue
            // 
            this.ColumnValue.HeaderText = "Value";
            this.ColumnValue.Name = "ColumnValue";
            this.ColumnValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnValue.Width = 70;
            // 
            // ColumnQuality
            // 
            this.ColumnQuality.HeaderText = "Quality";
            this.ColumnQuality.Name = "ColumnQuality";
            this.ColumnQuality.ReadOnly = true;
            this.ColumnQuality.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnQuality.Width = 70;
            // 
            // ColumnTime
            // 
            this.ColumnTime.HeaderText = "Time";
            this.ColumnTime.Name = "ColumnTime";
            this.ColumnTime.ReadOnly = true;
            this.ColumnTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnTime.Width = 120;
            // 
            // button_Load
            // 
            this.button_Load.Location = new System.Drawing.Point(29, 386);
            this.button_Load.Name = "button_Load";
            this.button_Load.Size = new System.Drawing.Size(117, 39);
            this.button_Load.TabIndex = 4;
            this.button_Load.Text = "从配置文件中加载";
            this.button_Load.UseVisualStyleBackColor = true;
            this.button_Load.Click += new System.EventHandler(this.button_Load_Click);
            // 
            // button_Save
            // 
            this.button_Save.Location = new System.Drawing.Point(195, 386);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(117, 39);
            this.button_Save.TabIndex = 4;
            this.button_Save.Text = "保存到配置文件";
            this.button_Save.UseVisualStyleBackColor = true;
            this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // button_UpdateSelect
            // 
            this.button_UpdateSelect.Location = new System.Drawing.Point(361, 386);
            this.button_UpdateSelect.Name = "button_UpdateSelect";
            this.button_UpdateSelect.Size = new System.Drawing.Size(117, 39);
            this.button_UpdateSelect.TabIndex = 4;
            this.button_UpdateSelect.Text = "更新当前选择项";
            this.button_UpdateSelect.UseVisualStyleBackColor = true;
            this.button_UpdateSelect.Click += new System.EventHandler(this.button_UpdateSelect_Click);
            // 
            // button_UpdateAll
            // 
            this.button_UpdateAll.Location = new System.Drawing.Point(527, 386);
            this.button_UpdateAll.Name = "button_UpdateAll";
            this.button_UpdateAll.Size = new System.Drawing.Size(117, 39);
            this.button_UpdateAll.TabIndex = 4;
            this.button_UpdateAll.Text = "更新所有";
            this.button_UpdateAll.UseVisualStyleBackColor = true;
            this.button_UpdateAll.Click += new System.EventHandler(this.button_UpdateAll_Click);
            // 
            // checkBoxOpc
            // 
            this.checkBoxOpc.AutoSize = true;
            this.checkBoxOpc.Location = new System.Drawing.Point(512, 56);
            this.checkBoxOpc.Name = "checkBoxOpc";
            this.checkBoxOpc.Size = new System.Drawing.Size(66, 16);
            this.checkBoxOpc.TabIndex = 5;
            this.checkBoxOpc.Text = "启用OPC";
            this.checkBoxOpc.UseVisualStyleBackColor = true;
            // 
            // Form_Opc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(698, 437);
            this.Controls.Add(this.checkBoxOpc);
            this.Controls.Add(this.button_Save);
            this.Controls.Add(this.button_UpdateAll);
            this.Controls.Add(this.button_UpdateSelect);
            this.Controls.Add(this.button_Load);
            this.Controls.Add(this.dataGridView_opc);
            this.Controls.Add(this.comboBox_ServerName);
            this.Controls.Add(this.textBox_UpdateRate);
            this.Controls.Add(this.textBox_GroupName);
            this.Controls.Add(this.label_UpdateRate);
            this.Controls.Add(this.textBox_ServerIP);
            this.Controls.Add(this.label_GroupName);
            this.Controls.Add(this.label_ServerName);
            this.Controls.Add(this.label_ServerIP);
            this.Name = "Form_Opc";
            this.Text = "Form_Opc";
            this.Load += new System.EventHandler(this.Form_Opc_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_opc)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_ServerIP;
        private System.Windows.Forms.TextBox textBox_ServerIP;
        private System.Windows.Forms.Label label_ServerName;
        private System.Windows.Forms.ComboBox comboBox_ServerName;
        private System.Windows.Forms.Label label_GroupName;
        private System.Windows.Forms.TextBox textBox_GroupName;
        private System.Windows.Forms.Label label_UpdateRate;
        private System.Windows.Forms.TextBox textBox_UpdateRate;
        private System.Windows.Forms.DataGridView dataGridView_opc;
        private System.Windows.Forms.Button button_Load;
        private System.Windows.Forms.Button button_Save;
        private System.Windows.Forms.Button button_UpdateSelect;
        private System.Windows.Forms.Button button_UpdateAll;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTag;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnQuality;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTime;
        private System.Windows.Forms.CheckBox checkBoxOpc;
    }
}