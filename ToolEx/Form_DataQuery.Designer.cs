namespace ToolEx
{
    partial class Form_DataQuery
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl_Main = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox_Query = new System.Windows.Forms.GroupBox();
            this.button_CurExport = new System.Windows.Forms.Button();
            this.comboBox_Count = new System.Windows.Forms.ComboBox();
            this.textBox_QueryText = new System.Windows.Forms.TextBox();
            this.button_RecentQuery = new System.Windows.Forms.Button();
            this.button_TextBoxQuery = new System.Windows.Forms.Button();
            this.button_DataGridQuery = new System.Windows.Forms.Button();
            this.dataGridView_QueryTerm = new System.Windows.Forms.DataGridView();
            this.Column_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Operator = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Logic = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem_Add = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_Del = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_Clear = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox_CurrentResult = new System.Windows.Forms.GroupBox();
            this.dataGridView_Current = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox_SummaryQuery = new System.Windows.Forms.GroupBox();
            this.button_SummaryExport = new System.Windows.Forms.Button();
            this.textBox_SummaryQuery = new System.Windows.Forms.TextBox();
            this.button_TextBoxSummaryQuery = new System.Windows.Forms.Button();
            this.button_DataGridSummaryQuery = new System.Windows.Forms.Button();
            this.dataGridView_SummaryQueryTerm = new System.Windows.Forms.DataGridView();
            this.Column_TableName1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_TableItem1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_TableName2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_TableItem2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_JOIN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox_SummaryResult = new System.Windows.Forms.GroupBox();
            this.dataGridView_Summary = new System.Windows.Forms.DataGridView();
            this.tabControl_Main.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox_Query.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_QueryTerm)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox_CurrentResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Current)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox_SummaryQuery.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_SummaryQueryTerm)).BeginInit();
            this.groupBox_SummaryResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Summary)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl_Main
            // 
            this.tabControl_Main.Controls.Add(this.tabPage1);
            this.tabControl_Main.Controls.Add(this.tabPage2);
            this.tabControl_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_Main.Location = new System.Drawing.Point(0, 0);
            this.tabControl_Main.Name = "tabControl_Main";
            this.tabControl_Main.SelectedIndex = 0;
            this.tabControl_Main.Size = new System.Drawing.Size(1086, 558);
            this.tabControl_Main.TabIndex = 0;
            this.tabControl_Main.SelectedIndexChanged += new System.EventHandler(this.tabControl_Main_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tableLayoutPanel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 28);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1078, 526);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "本站查询";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox_Query, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox_CurrentResult, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1072, 520);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox_Query
            // 
            this.groupBox_Query.Controls.Add(this.button_CurExport);
            this.groupBox_Query.Controls.Add(this.comboBox_Count);
            this.groupBox_Query.Controls.Add(this.textBox_QueryText);
            this.groupBox_Query.Controls.Add(this.button_RecentQuery);
            this.groupBox_Query.Controls.Add(this.button_TextBoxQuery);
            this.groupBox_Query.Controls.Add(this.button_DataGridQuery);
            this.groupBox_Query.Controls.Add(this.dataGridView_QueryTerm);
            this.groupBox_Query.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_Query.Location = new System.Drawing.Point(3, 3);
            this.groupBox_Query.Name = "groupBox_Query";
            this.groupBox_Query.Size = new System.Drawing.Size(1066, 176);
            this.groupBox_Query.TabIndex = 1;
            this.groupBox_Query.TabStop = false;
            this.groupBox_Query.Text = "查询条件";
            // 
            // button_CurExport
            // 
            this.button_CurExport.Location = new System.Drawing.Point(662, 73);
            this.button_CurExport.Name = "button_CurExport";
            this.button_CurExport.Size = new System.Drawing.Size(75, 41);
            this.button_CurExport.TabIndex = 4;
            this.button_CurExport.Text = "导出";
            this.button_CurExport.UseVisualStyleBackColor = true;
            this.button_CurExport.Click += new System.EventHandler(this.button_CurExport_Click);
            // 
            // comboBox_Count
            // 
            this.comboBox_Count.FormattingEnabled = true;
            this.comboBox_Count.Location = new System.Drawing.Point(942, 20);
            this.comboBox_Count.Name = "comboBox_Count";
            this.comboBox_Count.Size = new System.Drawing.Size(121, 26);
            this.comboBox_Count.TabIndex = 3;
            // 
            // textBox_QueryText
            // 
            this.textBox_QueryText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_QueryText.Location = new System.Drawing.Point(6, 117);
            this.textBox_QueryText.Multiline = true;
            this.textBox_QueryText.Name = "textBox_QueryText";
            this.textBox_QueryText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_QueryText.Size = new System.Drawing.Size(1054, 53);
            this.textBox_QueryText.TabIndex = 2;
            // 
            // button_RecentQuery
            // 
            this.button_RecentQuery.Location = new System.Drawing.Point(824, 20);
            this.button_RecentQuery.Name = "button_RecentQuery";
            this.button_RecentQuery.Size = new System.Drawing.Size(111, 47);
            this.button_RecentQuery.TabIndex = 1;
            this.button_RecentQuery.Text = "查询最近";
            this.button_RecentQuery.UseVisualStyleBackColor = true;
            this.button_RecentQuery.Click += new System.EventHandler(this.button_RecentQuery_Click);
            // 
            // button_TextBoxQuery
            // 
            this.button_TextBoxQuery.Location = new System.Drawing.Point(743, 20);
            this.button_TextBoxQuery.Name = "button_TextBoxQuery";
            this.button_TextBoxQuery.Size = new System.Drawing.Size(75, 47);
            this.button_TextBoxQuery.TabIndex = 1;
            this.button_TextBoxQuery.Text = "查询2";
            this.button_TextBoxQuery.UseVisualStyleBackColor = true;
            this.button_TextBoxQuery.Click += new System.EventHandler(this.button_TextBoxQuery_Click);
            // 
            // button_DataGridQuery
            // 
            this.button_DataGridQuery.Location = new System.Drawing.Point(662, 20);
            this.button_DataGridQuery.Name = "button_DataGridQuery";
            this.button_DataGridQuery.Size = new System.Drawing.Size(75, 47);
            this.button_DataGridQuery.TabIndex = 1;
            this.button_DataGridQuery.Text = "查询1";
            this.button_DataGridQuery.UseVisualStyleBackColor = true;
            this.button_DataGridQuery.Click += new System.EventHandler(this.button_DataGridQuery_Click);
            // 
            // dataGridView_QueryTerm
            // 
            this.dataGridView_QueryTerm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGridView_QueryTerm.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_QueryTerm.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView_QueryTerm.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_QueryTerm.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_Name,
            this.Column_Operator,
            this.Column_Value,
            this.Column_Logic});
            this.dataGridView_QueryTerm.ContextMenuStrip = this.contextMenuStrip1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_QueryTerm.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView_QueryTerm.Location = new System.Drawing.Point(6, 20);
            this.dataGridView_QueryTerm.Name = "dataGridView_QueryTerm";
            this.dataGridView_QueryTerm.RowTemplate.Height = 30;
            this.dataGridView_QueryTerm.Size = new System.Drawing.Size(650, 91);
            this.dataGridView_QueryTerm.TabIndex = 0;
            this.dataGridView_QueryTerm.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView_QueryTerm_ColumnWidthChanged);
            this.dataGridView_QueryTerm.CurrentCellChanged += new System.EventHandler(this.dataGridView_QueryTerm_CurrentCellChanged);
            this.dataGridView_QueryTerm.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dataGridView_QueryTerm_Scroll);
            // 
            // Column_Name
            // 
            this.Column_Name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column_Name.HeaderText = "名称";
            this.Column_Name.Name = "Column_Name";
            this.Column_Name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column_Name.Width = 150;
            // 
            // Column_Operator
            // 
            this.Column_Operator.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column_Operator.HeaderText = "操作符";
            this.Column_Operator.Name = "Column_Operator";
            this.Column_Operator.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column_Operator.Width = 80;
            // 
            // Column_Value
            // 
            this.Column_Value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column_Value.HeaderText = "数值";
            this.Column_Value.Name = "Column_Value";
            this.Column_Value.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column_Logic
            // 
            this.Column_Logic.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column_Logic.HeaderText = "逻辑运算";
            this.Column_Logic.Name = "Column_Logic";
            this.Column_Logic.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column_Logic.Width = 120;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_Add,
            this.toolStripMenuItem_Del,
            this.toolStripMenuItem_Clear});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(117, 88);
            // 
            // toolStripMenuItem_Add
            // 
            this.toolStripMenuItem_Add.Name = "toolStripMenuItem_Add";
            this.toolStripMenuItem_Add.Size = new System.Drawing.Size(116, 28);
            this.toolStripMenuItem_Add.Text = "新增";
            this.toolStripMenuItem_Add.Click += new System.EventHandler(this.toolStripMenuItem_Add_Click);
            // 
            // toolStripMenuItem_Del
            // 
            this.toolStripMenuItem_Del.Name = "toolStripMenuItem_Del";
            this.toolStripMenuItem_Del.Size = new System.Drawing.Size(116, 28);
            this.toolStripMenuItem_Del.Text = "删除";
            this.toolStripMenuItem_Del.Click += new System.EventHandler(this.toolStripMenuItem_Del_Click);
            // 
            // toolStripMenuItem_Clear
            // 
            this.toolStripMenuItem_Clear.Name = "toolStripMenuItem_Clear";
            this.toolStripMenuItem_Clear.Size = new System.Drawing.Size(116, 28);
            this.toolStripMenuItem_Clear.Text = "清空";
            this.toolStripMenuItem_Clear.Click += new System.EventHandler(this.toolStripMenuItem_Clear_Click);
            // 
            // groupBox_CurrentResult
            // 
            this.groupBox_CurrentResult.Controls.Add(this.dataGridView_Current);
            this.groupBox_CurrentResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_CurrentResult.Location = new System.Drawing.Point(3, 185);
            this.groupBox_CurrentResult.Name = "groupBox_CurrentResult";
            this.groupBox_CurrentResult.Size = new System.Drawing.Size(1066, 332);
            this.groupBox_CurrentResult.TabIndex = 2;
            this.groupBox_CurrentResult.TabStop = false;
            this.groupBox_CurrentResult.Text = "查询结果";
            // 
            // dataGridView_Current
            // 
            this.dataGridView_Current.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_Current.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView_Current.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_Current.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView_Current.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_Current.Location = new System.Drawing.Point(3, 24);
            this.dataGridView_Current.Name = "dataGridView_Current";
            this.dataGridView_Current.ReadOnly = true;
            this.dataGridView_Current.RowTemplate.Height = 30;
            this.dataGridView_Current.Size = new System.Drawing.Size(1060, 305);
            this.dataGridView_Current.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tableLayoutPanel2);
            this.tabPage2.Location = new System.Drawing.Point(4, 28);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1078, 526);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "总查询";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.groupBox_SummaryQuery, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.groupBox_SummaryResult, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1072, 520);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // groupBox_SummaryQuery
            // 
            this.groupBox_SummaryQuery.Controls.Add(this.button_SummaryExport);
            this.groupBox_SummaryQuery.Controls.Add(this.textBox_SummaryQuery);
            this.groupBox_SummaryQuery.Controls.Add(this.button_TextBoxSummaryQuery);
            this.groupBox_SummaryQuery.Controls.Add(this.button_DataGridSummaryQuery);
            this.groupBox_SummaryQuery.Controls.Add(this.dataGridView_SummaryQueryTerm);
            this.groupBox_SummaryQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_SummaryQuery.Location = new System.Drawing.Point(3, 3);
            this.groupBox_SummaryQuery.Name = "groupBox_SummaryQuery";
            this.groupBox_SummaryQuery.Size = new System.Drawing.Size(1066, 176);
            this.groupBox_SummaryQuery.TabIndex = 1;
            this.groupBox_SummaryQuery.TabStop = false;
            this.groupBox_SummaryQuery.Text = "查询条件";
            // 
            // button_SummaryExport
            // 
            this.button_SummaryExport.Location = new System.Drawing.Point(662, 69);
            this.button_SummaryExport.Name = "button_SummaryExport";
            this.button_SummaryExport.Size = new System.Drawing.Size(75, 41);
            this.button_SummaryExport.TabIndex = 5;
            this.button_SummaryExport.Text = "导出";
            this.button_SummaryExport.UseVisualStyleBackColor = true;
            this.button_SummaryExport.Click += new System.EventHandler(this.button_SumExport_Click);
            // 
            // textBox_SummaryQuery
            // 
            this.textBox_SummaryQuery.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_SummaryQuery.Location = new System.Drawing.Point(6, 116);
            this.textBox_SummaryQuery.Multiline = true;
            this.textBox_SummaryQuery.Name = "textBox_SummaryQuery";
            this.textBox_SummaryQuery.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_SummaryQuery.Size = new System.Drawing.Size(1053, 54);
            this.textBox_SummaryQuery.TabIndex = 2;
            // 
            // button_TextBoxSummaryQuery
            // 
            this.button_TextBoxSummaryQuery.Location = new System.Drawing.Point(743, 20);
            this.button_TextBoxSummaryQuery.Name = "button_TextBoxSummaryQuery";
            this.button_TextBoxSummaryQuery.Size = new System.Drawing.Size(75, 47);
            this.button_TextBoxSummaryQuery.TabIndex = 1;
            this.button_TextBoxSummaryQuery.Text = "查询2";
            this.button_TextBoxSummaryQuery.UseVisualStyleBackColor = true;
            this.button_TextBoxSummaryQuery.Click += new System.EventHandler(this.button_TextBoxSummaryQuery_Click);
            // 
            // button_DataGridSummaryQuery
            // 
            this.button_DataGridSummaryQuery.Location = new System.Drawing.Point(662, 20);
            this.button_DataGridSummaryQuery.Name = "button_DataGridSummaryQuery";
            this.button_DataGridSummaryQuery.Size = new System.Drawing.Size(75, 47);
            this.button_DataGridSummaryQuery.TabIndex = 1;
            this.button_DataGridSummaryQuery.Text = "查询1";
            this.button_DataGridSummaryQuery.UseVisualStyleBackColor = true;
            this.button_DataGridSummaryQuery.Click += new System.EventHandler(this.button_DataGridSummaryQuery_Click);
            // 
            // dataGridView_SummaryQueryTerm
            // 
            this.dataGridView_SummaryQueryTerm.AllowUserToAddRows = false;
            this.dataGridView_SummaryQueryTerm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGridView_SummaryQueryTerm.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_SummaryQueryTerm.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridView_SummaryQueryTerm.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_SummaryQueryTerm.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_TableName1,
            this.Column_TableItem1,
            this.Column_TableName2,
            this.Column_TableItem2,
            this.Column_JOIN});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_SummaryQueryTerm.DefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridView_SummaryQueryTerm.Location = new System.Drawing.Point(6, 20);
            this.dataGridView_SummaryQueryTerm.Name = "dataGridView_SummaryQueryTerm";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_SummaryQueryTerm.RowHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridView_SummaryQueryTerm.RowTemplate.Height = 30;
            this.dataGridView_SummaryQueryTerm.Size = new System.Drawing.Size(650, 91);
            this.dataGridView_SummaryQueryTerm.TabIndex = 0;
            this.dataGridView_SummaryQueryTerm.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView_QueryTerm_ColumnWidthChanged);
            this.dataGridView_SummaryQueryTerm.CurrentCellChanged += new System.EventHandler(this.dataGridView_QueryTerm_CurrentCellChanged);
            this.dataGridView_SummaryQueryTerm.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dataGridView_QueryTerm_Scroll);
            // 
            // Column_TableName1
            // 
            this.Column_TableName1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column_TableName1.HeaderText = "表1名称";
            this.Column_TableName1.Name = "Column_TableName1";
            this.Column_TableName1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column_TableName1.Width = 120;
            // 
            // Column_TableItem1
            // 
            this.Column_TableItem1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column_TableItem1.HeaderText = "表1项";
            this.Column_TableItem1.Name = "Column_TableItem1";
            this.Column_TableItem1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column_TableName2
            // 
            this.Column_TableName2.HeaderText = "表2名称";
            this.Column_TableName2.Name = "Column_TableName2";
            this.Column_TableName2.Width = 120;
            // 
            // Column_TableItem2
            // 
            this.Column_TableItem2.HeaderText = "表2项";
            this.Column_TableItem2.Name = "Column_TableItem2";
            // 
            // Column_JOIN
            // 
            this.Column_JOIN.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column_JOIN.HeaderText = "连接方式";
            this.Column_JOIN.Name = "Column_JOIN";
            this.Column_JOIN.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // groupBox_SummaryResult
            // 
            this.groupBox_SummaryResult.Controls.Add(this.dataGridView_Summary);
            this.groupBox_SummaryResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_SummaryResult.Location = new System.Drawing.Point(3, 185);
            this.groupBox_SummaryResult.Name = "groupBox_SummaryResult";
            this.groupBox_SummaryResult.Size = new System.Drawing.Size(1066, 332);
            this.groupBox_SummaryResult.TabIndex = 2;
            this.groupBox_SummaryResult.TabStop = false;
            this.groupBox_SummaryResult.Text = "查询结果";
            // 
            // dataGridView_Summary
            // 
            this.dataGridView_Summary.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_Summary.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridView_Summary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_Summary.DefaultCellStyle = dataGridViewCellStyle9;
            this.dataGridView_Summary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_Summary.Location = new System.Drawing.Point(3, 24);
            this.dataGridView_Summary.Name = "dataGridView_Summary";
            this.dataGridView_Summary.ReadOnly = true;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_Summary.RowHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.dataGridView_Summary.RowTemplate.Height = 30;
            this.dataGridView_Summary.Size = new System.Drawing.Size(1060, 305);
            this.dataGridView_Summary.TabIndex = 0;
            // 
            // Form_DataQuery
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1086, 558);
            this.Controls.Add(this.tabControl_Main);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "Form_DataQuery";
            this.Text = "数据查询";
            this.Load += new System.EventHandler(this.Form_DataQuery_Load);
            this.tabControl_Main.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox_Query.ResumeLayout(false);
            this.groupBox_Query.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_QueryTerm)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox_CurrentResult.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Current)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.groupBox_SummaryQuery.ResumeLayout(false);
            this.groupBox_SummaryQuery.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_SummaryQueryTerm)).EndInit();
            this.groupBox_SummaryResult.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Summary)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl_Main;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView dataGridView_Current;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox_Query;
        private System.Windows.Forms.Button button_DataGridQuery;
        private System.Windows.Forms.DataGridView dataGridView_QueryTerm;
        private System.Windows.Forms.TextBox textBox_QueryText;
        private System.Windows.Forms.Button button_TextBoxQuery;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Operator;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Value;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Logic;
        private System.Windows.Forms.Button button_RecentQuery;
        private System.Windows.Forms.ComboBox comboBox_Count;
        private System.Windows.Forms.GroupBox groupBox_CurrentResult;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBox_SummaryQuery;
        private System.Windows.Forms.TextBox textBox_SummaryQuery;
        private System.Windows.Forms.Button button_TextBoxSummaryQuery;
        private System.Windows.Forms.Button button_DataGridSummaryQuery;
        private System.Windows.Forms.DataGridView dataGridView_SummaryQueryTerm;
        private System.Windows.Forms.GroupBox groupBox_SummaryResult;
        private System.Windows.Forms.DataGridView dataGridView_Summary;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Add;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Del;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Clear;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_TableName1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_TableItem1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_TableName2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_TableItem2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_JOIN;
        private System.Windows.Forms.Button button_CurExport;
        private System.Windows.Forms.Button button_SummaryExport;
    }
}