namespace AutoFrame
{
    partial class Form_Alarm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Alarm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItem_Clean = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_CleanAll = new System.Windows.Forms.ToolStripMenuItem();
            this.timer_flash = new System.Windows.Forms.Timer(this.components);
            this.roundPanel1 = new AutoFrameUI.RoundPanel(this.components);
            this.chart_warn = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.roundPanel4 = new AutoFrameUI.RoundPanel(this.components);
            this.roundButton2 = new AutoFrameUI.RoundButton();
            this.chart_downtime = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.roundPanel2 = new AutoFrameUI.RoundPanel(this.components);
            this.roundButton1 = new AutoFrameUI.RoundButton();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView_history = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridView_current = new System.Windows.Forms.DataGridView();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1.SuspendLayout();
            this.roundPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart_warn)).BeginInit();
            this.roundPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart_downtime)).BeginInit();
            this.roundPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_history)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_current)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_Clean,
            this.ToolStripMenuItem_CleanAll});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(145, 56);
            // 
            // ToolStripMenuItem_Clean
            // 
            this.ToolStripMenuItem_Clean.Name = "ToolStripMenuItem_Clean";
            this.ToolStripMenuItem_Clean.Size = new System.Drawing.Size(144, 26);
            this.ToolStripMenuItem_Clean.Text = "清除";
            this.ToolStripMenuItem_Clean.Click += new System.EventHandler(this.ToolStripMenuItem_Clean_Click);
            // 
            // ToolStripMenuItem_CleanAll
            // 
            this.ToolStripMenuItem_CleanAll.Name = "ToolStripMenuItem_CleanAll";
            this.ToolStripMenuItem_CleanAll.Size = new System.Drawing.Size(144, 26);
            this.ToolStripMenuItem_CleanAll.Text = "清除全部";
            this.ToolStripMenuItem_CleanAll.Click += new System.EventHandler(this.ToolStripMenuItem_CleanAll_Click);
            // 
            // timer_flash
            // 
            this.timer_flash.Interval = 1000;
            this.timer_flash.Tick += new System.EventHandler(this.timer_flash_Tick);
            // 
            // roundPanel1
            // 
            this.roundPanel1._setRoundRadius = 8;
            this.roundPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.roundPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(235)))));
            this.roundPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.roundPanel1.Controls.Add(this.chart_warn);
            this.roundPanel1.Location = new System.Drawing.Point(993, 5);
            this.roundPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.roundPanel1.Name = "roundPanel1";
            this.roundPanel1.Size = new System.Drawing.Size(348, 301);
            this.roundPanel1.TabIndex = 16;
            // 
            // chart_warn
            // 
            this.chart_warn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.Name = "ChartArea1";
            this.chart_warn.ChartAreas.Add(chartArea1);
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend1.Name = "Legend1";
            this.chart_warn.Legends.Add(legend1);
            this.chart_warn.Location = new System.Drawing.Point(3, 6);
            this.chart_warn.Name = "chart_warn";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series1.IsValueShownAsLabel = true;
            series1.Label = "#PERCENT";
            series1.Legend = "Legend1";
            series1.LegendText = "#AXISLABEL";
            series1.Name = "Series1";
            this.chart_warn.Series.Add(series1);
            this.chart_warn.Size = new System.Drawing.Size(342, 292);
            this.chart_warn.TabIndex = 0;
            this.chart_warn.Text = "chart1";
            // 
            // roundPanel4
            // 
            this.roundPanel4._setRoundRadius = 8;
            this.roundPanel4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.roundPanel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(235)))));
            this.roundPanel4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.roundPanel4.Controls.Add(this.roundButton2);
            this.roundPanel4.Controls.Add(this.chart_downtime);
            this.roundPanel4.Location = new System.Drawing.Point(993, 318);
            this.roundPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.roundPanel4.Name = "roundPanel4";
            this.roundPanel4.Size = new System.Drawing.Size(348, 301);
            this.roundPanel4.TabIndex = 17;
            // 
            // roundButton2
            // 
            this.roundButton2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.roundButton2.BackColor = System.Drawing.Color.Transparent;
            this.roundButton2.BaseColor = System.Drawing.Color.White;
            this.roundButton2.BaseColorEnd = System.Drawing.Color.White;
            this.roundButton2.FlatAppearance.BorderSize = 0;
            this.roundButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roundButton2.ImageHeight = 80;
            this.roundButton2.ImageWidth = 80;
            this.roundButton2.Location = new System.Drawing.Point(57, 14);
            this.roundButton2.Name = "roundButton2";
            this.roundButton2.Radius = 24;
            this.roundButton2.Size = new System.Drawing.Size(219, 49);
            this.roundButton2.SpliteButtonWidth = 18;
            this.roundButton2.TabIndex = 8;
            this.roundButton2.Text = "Alarm Downtime";
            this.roundButton2.UseVisualStyleBackColor = false;
            // 
            // chart_downtime
            // 
            this.chart_downtime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chart_downtime.BorderlineColor = System.Drawing.Color.Silver;
            chartArea2.AxisX.MajorGrid.Enabled = false;
            chartArea2.AxisX.MajorTickMark.TickMarkStyle = System.Windows.Forms.DataVisualization.Charting.TickMarkStyle.InsideArea;
            chartArea2.AxisX.MinorTickMark.Interval = 1D;
            chartArea2.AxisX.MinorTickMark.TickMarkStyle = System.Windows.Forms.DataVisualization.Charting.TickMarkStyle.InsideArea;
            chartArea2.AxisX.ScaleBreakStyle.Enabled = true;
            chartArea2.AxisY.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.Lines;
            chartArea2.AxisY.LabelStyle.Enabled = false;
            chartArea2.AxisY.MajorGrid.LineColor = System.Drawing.Color.Salmon;
            chartArea2.AxisY.MajorTickMark.TickMarkStyle = System.Windows.Forms.DataVisualization.Charting.TickMarkStyle.InsideArea;
            chartArea2.AxisY.MinorTickMark.Enabled = true;
            chartArea2.AxisY.MinorTickMark.TickMarkStyle = System.Windows.Forms.DataVisualization.Charting.TickMarkStyle.InsideArea;
            chartArea2.InnerPlotPosition.Auto = false;
            chartArea2.InnerPlotPosition.Height = 95F;
            chartArea2.InnerPlotPosition.Width = 96F;
            chartArea2.InnerPlotPosition.X = 1F;
            chartArea2.IsSameFontSizeForAllAxes = true;
            chartArea2.Name = "ChartArea1";
            chartArea2.Position.Auto = false;
            chartArea2.Position.Height = 100F;
            chartArea2.Position.Width = 100F;
            this.chart_downtime.ChartAreas.Add(chartArea2);
            this.chart_downtime.Location = new System.Drawing.Point(16, 69);
            this.chart_downtime.Name = "chart_downtime";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.IsVisibleInLegend = false;
            series2.Name = "Series1";
            series2.SmartLabelStyle.AllowOutsidePlotArea = System.Windows.Forms.DataVisualization.Charting.LabelOutsidePlotAreaStyle.Yes;
            series2.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series2.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            this.chart_downtime.Series.Add(series2);
            this.chart_downtime.Size = new System.Drawing.Size(317, 224);
            this.chart_downtime.TabIndex = 7;
            this.chart_downtime.Text = "chart1";
            // 
            // roundPanel2
            // 
            this.roundPanel2._setRoundRadius = 8;
            this.roundPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.roundPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(235)))));
            this.roundPanel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.roundPanel2.Controls.Add(this.roundButton1);
            this.roundPanel2.Controls.Add(this.label1);
            this.roundPanel2.Controls.Add(this.dataGridView_history);
            this.roundPanel2.Controls.Add(this.dataGridView_current);
            this.roundPanel2.Location = new System.Drawing.Point(5, 5);
            this.roundPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.roundPanel2.Name = "roundPanel2";
            this.roundPanel2.Size = new System.Drawing.Size(979, 614);
            this.roundPanel2.TabIndex = 4;
            // 
            // roundButton1
            // 
            this.roundButton1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.roundButton1.BackColor = System.Drawing.Color.Transparent;
            this.roundButton1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("roundButton1.BackgroundImage")));
            this.roundButton1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.roundButton1.BaseColor = System.Drawing.Color.Transparent;
            this.roundButton1.BaseColorEnd = System.Drawing.Color.Transparent;
            this.roundButton1.FlatAppearance.BorderSize = 0;
            this.roundButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roundButton1.ImageHeight = 80;
            this.roundButton1.ImageWidth = 80;
            this.roundButton1.Location = new System.Drawing.Point(517, 535);
            this.roundButton1.Name = "roundButton1";
            this.roundButton1.Radius = 5;
            this.roundButton1.Size = new System.Drawing.Size(82, 72);
            this.roundButton1.SpliteButtonWidth = 18;
            this.roundButton1.TabIndex = 2;
            this.roundButton1.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(5, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(966, 30);
            this.label1.TabIndex = 1;
            this.label1.Text = "最近30天错误历史记录";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dataGridView_history
            // 
            this.dataGridView_history.AllowUserToAddRows = false;
            this.dataGridView_history.AllowUserToResizeRows = false;
            this.dataGridView_history.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView_history.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView_history.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView_history.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView_history.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 12F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_history.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView_history.ColumnHeadersHeight = 35;
            this.dataGridView_history.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView_history.ColumnHeadersVisible = false;
            this.dataGridView_history.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 12F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_history.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView_history.EnableHeadersVisualStyles = false;
            this.dataGridView_history.Location = new System.Drawing.Point(7, 81);
            this.dataGridView_history.MultiSelect = false;
            this.dataGridView_history.Name = "dataGridView_history";
            this.dataGridView_history.ReadOnly = true;
            this.dataGridView_history.RowHeadersVisible = false;
            this.dataGridView_history.RowHeadersWidth = 32;
            this.dataGridView_history.RowTemplate.Height = 30;
            this.dataGridView_history.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_history.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView_history.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_history.ShowEditingIcon = false;
            this.dataGridView_history.Size = new System.Drawing.Size(964, 445);
            this.dataGridView_history.TabIndex = 2;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewTextBoxColumn1.HeaderText = "日期";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn2.HeaderText = "时间";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn2.Width = 90;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn3.HeaderText = "错误代码";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn3.Width = 80;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn4.HeaderText = "错误编组";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn5.HeaderText = "消息";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn6.HeaderText = "持续时间";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // dataGridView_current
            // 
            this.dataGridView_current.AllowUserToAddRows = false;
            this.dataGridView_current.AllowUserToResizeRows = false;
            this.dataGridView_current.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView_current.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView_current.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView_current.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView_current.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 12F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_current.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView_current.ColumnHeadersHeight = 35;
            this.dataGridView_current.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView_current.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column1});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("宋体", 12F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_current.DefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridView_current.EnableHeadersVisualStyles = false;
            this.dataGridView_current.Location = new System.Drawing.Point(6, 6);
            this.dataGridView_current.MultiSelect = false;
            this.dataGridView_current.Name = "dataGridView_current";
            this.dataGridView_current.ReadOnly = true;
            this.dataGridView_current.RowHeadersVisible = false;
            this.dataGridView_current.RowHeadersWidth = 32;
            this.dataGridView_current.RowTemplate.Height = 30;
            this.dataGridView_current.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_current.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView_current.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_current.ShowEditingIcon = false;
            this.dataGridView_current.Size = new System.Drawing.Size(966, 36);
            this.dataGridView_current.TabIndex = 3;
            this.dataGridView_current.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_current_CellMouseDown);
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column2.DefaultCellStyle = dataGridViewCellStyle5;
            this.Column2.HeaderText = "日期";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column3.HeaderText = "时间";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column3.Width = 90;
            // 
            // Column4
            // 
            this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column4.HeaderText = "错误代码";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column4.Width = 80;
            // 
            // Column5
            // 
            this.Column5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column5.HeaderText = "错误编组";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column6
            // 
            this.Column6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column6.HeaderText = "消息";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column1.HeaderText = "持续时间";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Form_Alarm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1350, 628);
            this.Controls.Add(this.roundPanel1);
            this.Controls.Add(this.roundPanel4);
            this.Controls.Add(this.roundPanel2);
            this.Font = new System.Drawing.Font("宋体", 12F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form_Alarm";
            this.Text = "信息查看";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Alarm_FormClosing);
            this.Load += new System.EventHandler(this.Form_Alarm_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.roundPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart_warn)).EndInit();
            this.roundPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart_downtime)).EndInit();
            this.roundPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_history)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_current)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridView_current;
        private AutoFrameUI.RoundPanel roundPanel2;
        private System.Windows.Forms.Timer timer_flash;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_warn;
        private System.Windows.Forms.DataGridView dataGridView_history;
        private System.Windows.Forms.Label label1;
        private AutoFrameUI.RoundButton roundButton1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_downtime;
        private AutoFrameUI.RoundButton roundButton2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Clean;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_CleanAll;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private AutoFrameUI.RoundPanel roundPanel1;
        private AutoFrameUI.RoundPanel roundPanel4;
    }
}