namespace ToolEx
{
    partial class Form_MachineDataConfig
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
            this.dataGridView_Step = new System.Windows.Forms.DataGridView();
            this.button_LoadFromFile = new System.Windows.Forms.Button();
            this.button_Save = new System.Windows.Forms.Button();
            this.button_DSC = new System.Windows.Forms.Button();
            this.button_DA = new System.Windows.Forms.Button();
            this.名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.数据索引 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.数据类型 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Step)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView_Step
            // 
            this.dataGridView_Step.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Step.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.名称,
            this.数据索引,
            this.数据类型});
            this.dataGridView_Step.Location = new System.Drawing.Point(4, 2);
            this.dataGridView_Step.Name = "dataGridView_Step";
            this.dataGridView_Step.RowTemplate.Height = 30;
            this.dataGridView_Step.Size = new System.Drawing.Size(864, 594);
            this.dataGridView_Step.TabIndex = 1;
            // 
            // button_LoadFromFile
            // 
            this.button_LoadFromFile.Location = new System.Drawing.Point(874, 79);
            this.button_LoadFromFile.Name = "button_LoadFromFile";
            this.button_LoadFromFile.Size = new System.Drawing.Size(129, 42);
            this.button_LoadFromFile.TabIndex = 2;
            this.button_LoadFromFile.Text = "加载";
            this.button_LoadFromFile.UseVisualStyleBackColor = true;
            this.button_LoadFromFile.Click += new System.EventHandler(this.button_LoadFromFile_Click);
            // 
            // button_Save
            // 
            this.button_Save.Location = new System.Drawing.Point(874, 322);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(129, 42);
            this.button_Save.TabIndex = 3;
            this.button_Save.Text = "保存";
            this.button_Save.UseVisualStyleBackColor = true;
            this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // button_DSC
            // 
            this.button_DSC.Location = new System.Drawing.Point(874, 158);
            this.button_DSC.Name = "button_DSC";
            this.button_DSC.Size = new System.Drawing.Size(129, 42);
            this.button_DSC.TabIndex = 4;
            this.button_DSC.Text = "删除选中项";
            this.button_DSC.UseVisualStyleBackColor = true;
            this.button_DSC.Click += new System.EventHandler(this.button_DSC_Click);
            // 
            // button_DA
            // 
            this.button_DA.Location = new System.Drawing.Point(874, 239);
            this.button_DA.Name = "button_DA";
            this.button_DA.Size = new System.Drawing.Size(129, 42);
            this.button_DA.TabIndex = 5;
            this.button_DA.Text = "清空所有项";
            this.button_DA.UseVisualStyleBackColor = true;
            this.button_DA.Click += new System.EventHandler(this.button_DA_Click);
            // 
            // 名称
            // 
            this.名称.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.名称.HeaderText = "名称";
            this.名称.Name = "名称";
            this.名称.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // 数据索引
            // 
            this.数据索引.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.数据索引.HeaderText = "数据索引";
            this.数据索引.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30"});
            this.数据索引.Name = "数据索引";
            this.数据索引.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.数据索引.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // 数据类型
            // 
            this.数据类型.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.数据类型.HeaderText = "数据类型";
            this.数据类型.Items.AddRange(new object[] {
            "serials",
            "wip_serials",
            "input_time",
            "output_time",
            "pass",
            "data",
            "Tossing",
            "Tossing_reason"});
            this.数据类型.Name = "数据类型";
            this.数据类型.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.数据类型.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // Form_MachineDataConfig
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(1026, 600);
            this.Controls.Add(this.button_DA);
            this.Controls.Add(this.button_DSC);
            this.Controls.Add(this.button_Save);
            this.Controls.Add(this.button_LoadFromFile);
            this.Controls.Add(this.dataGridView_Step);
            this.Cursor = System.Windows.Forms.Cursors.Cross;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form_MachineDataConfig";
            this.Text = "Form_MachineDataConfig";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Step)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView_Step;
        private System.Windows.Forms.Button button_LoadFromFile;
        private System.Windows.Forms.Button button_Save;
        private System.Windows.Forms.Button button_DSC;
        private System.Windows.Forms.Button button_DA;
        private System.Windows.Forms.DataGridViewTextBoxColumn 名称;
        private System.Windows.Forms.DataGridViewComboBoxColumn 数据索引;
        private System.Windows.Forms.DataGridViewComboBoxColumn 数据类型;
    }
}