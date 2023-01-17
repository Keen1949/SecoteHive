namespace AutoFrame
{
    partial class Form_Data
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
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Data));
            this.roundPanel2 = new AutoFrameUI.RoundPanel(this.components);
            this.roundPanel_force = new AutoFrameUI.RoundPanel(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.halfRing1 = new AutoFrameUI.HalfRing(this.components);
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.roundPanel1 = new AutoFrameUI.RoundPanel(this.components);
            this.button_data = new System.Windows.Forms.Button();
            this.button_press = new System.Windows.Forms.Button();
            this.roundPanel2.SuspendLayout();
            this.roundPanel_force.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.roundPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // roundPanel2
            // 
            this.roundPanel2._setRoundRadius = 8;
            this.roundPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.roundPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(235)))));
            this.roundPanel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.roundPanel2.Controls.Add(this.roundPanel_force);
            this.roundPanel2.Location = new System.Drawing.Point(5, 5);
            this.roundPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.roundPanel2.Name = "roundPanel2";
            this.roundPanel2.Size = new System.Drawing.Size(1190, 614);
            this.roundPanel2.TabIndex = 4;
            // 
            // roundPanel_force
            // 
            this.roundPanel_force._setRoundRadius = 8;
            this.roundPanel_force.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.roundPanel_force.BackColor = System.Drawing.Color.White;
            this.roundPanel_force.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.roundPanel_force.Controls.Add(this.label1);
            this.roundPanel_force.Controls.Add(this.halfRing1);
            this.roundPanel_force.Controls.Add(this.chart1);
            this.roundPanel_force.Location = new System.Drawing.Point(3, 3);
            this.roundPanel_force.Margin = new System.Windows.Forms.Padding(0);
            this.roundPanel_force.Name = "roundPanel_force";
            this.roundPanel_force.Size = new System.Drawing.Size(1183, 608);
            this.roundPanel_force.TabIndex = 0;
            this.roundPanel_force.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(462, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 26);
            this.label1.TabIndex = 1;
            this.label1.Text = "Test Sample";
            // 
            // halfRing1
            // 
            this.halfRing1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.halfRing1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(235)))));
            this.halfRing1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.halfRing1.ForeColor = System.Drawing.Color.White;
            this.halfRing1.Location = new System.Drawing.Point(307, 47);
            this.halfRing1.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.halfRing1.Name = "halfRing1";
            this.halfRing1.setCircleRadius = 80;
            this.halfRing1.setColorGreen = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(187)))), ((int)(((byte)(63)))));
            this.halfRing1.setColorRed = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(37)))), ((int)(((byte)(6)))));
            this.halfRing1.setInRadius = 200;
            this.halfRing1.setLength = 45;
            this.halfRing1.setOutRadius = 250;
            this.halfRing1.setRateIn = 0.986F;
            this.halfRing1.setRateOut = 0.953F;
            this.halfRing1.setResult = false;
            this.halfRing1.setResultFont = new System.Drawing.Font("宋体", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.halfRing1.setScriptIn = "内环字符串测试";
            this.halfRing1.setScriptOut = "外环字符串测试";
            this.halfRing1.Size = new System.Drawing.Size(534, 509);
            this.halfRing1.TabIndex = 7;
            this.halfRing1.TabStop = false;
            this.halfRing1.Visible = false;
            // 
            // chart1
            // 
            this.chart1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.AxisX.MajorTickMark.TickMarkStyle = System.Windows.Forms.DataVisualization.Charting.TickMarkStyle.AcrossAxis;
            chartArea1.AxisX.MinorTickMark.Enabled = true;
            chartArea1.AxisX.MinorTickMark.TickMarkStyle = System.Windows.Forms.DataVisualization.Charting.TickMarkStyle.AcrossAxis;
            chartArea1.AxisX.Title = "时间(S)";
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.Silver;
            chartArea1.AxisY.MajorTickMark.TickMarkStyle = System.Windows.Forms.DataVisualization.Charting.TickMarkStyle.AcrossAxis;
            chartArea1.AxisY.MinorTickMark.Enabled = true;
            chartArea1.AxisY.MinorTickMark.TickMarkStyle = System.Windows.Forms.DataVisualization.Charting.TickMarkStyle.AcrossAxis;
            chartArea1.AxisY.Title = "压力(N)";
            chartArea1.BorderColor = System.Drawing.Color.Silver;
            chartArea1.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chartArea1.InnerPlotPosition.Auto = false;
            chartArea1.InnerPlotPosition.Height = 85.93763F;
            chartArea1.InnerPlotPosition.Width = 90F;
            chartArea1.InnerPlotPosition.X = 8F;
            chartArea1.InnerPlotPosition.Y = 2F;
            chartArea1.Name = "ChartArea1";
            chartArea1.Position.Auto = false;
            chartArea1.Position.Height = 100F;
            chartArea1.Position.Width = 88F;
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Alignment = System.Drawing.StringAlignment.Center;
            legend1.Name = "Legend1";
            legend1.Position.Auto = false;
            legend1.Position.Height = 15F;
            legend1.Position.Width = 10F;
            legend1.Position.X = 90F;
            legend1.Position.Y = 40F;
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(27, 51);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series1.Legend = "Legend1";
            series1.LegendText = "压力(Fx)";
            series1.Name = "Series1";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series2.Legend = "Legend1";
            series2.LegendText = "压力(Fy)";
            series2.Name = "Series2";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series3.Legend = "Legend1";
            series3.LegendText = "压力(Fz)";
            series3.Name = "Series3";
            this.chart1.Series.Add(series1);
            this.chart1.Series.Add(series2);
            this.chart1.Series.Add(series3);
            this.chart1.Size = new System.Drawing.Size(1105, 492);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            title1.Name = "Title1";
            this.chart1.Titles.Add(title1);
            // 
            // roundPanel1
            // 
            this.roundPanel1._setRoundRadius = 8;
            this.roundPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.roundPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(235)))));
            this.roundPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.roundPanel1.Controls.Add(this.button_data);
            this.roundPanel1.Controls.Add(this.button_press);
            this.roundPanel1.Location = new System.Drawing.Point(1206, 5);
            this.roundPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.roundPanel1.Name = "roundPanel1";
            this.roundPanel1.Size = new System.Drawing.Size(135, 614);
            this.roundPanel1.TabIndex = 16;
            // 
            // button_data
            // 
            this.button_data.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_data.BackgroundImage")));
            this.button_data.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_data.FlatAppearance.BorderSize = 0;
            this.button_data.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_data.Location = new System.Drawing.Point(5, 199);
            this.button_data.Name = "button_data";
            this.button_data.Size = new System.Drawing.Size(127, 114);
            this.button_data.TabIndex = 0;
            this.button_data.Text = "\r\n成功率图表";
            this.button_data.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button_data.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button_data.UseVisualStyleBackColor = true;
            this.button_data.Click += new System.EventHandler(this.button_data_Click);
            // 
            // button_press
            // 
            this.button_press.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_press.BackgroundImage")));
            this.button_press.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_press.FlatAppearance.BorderSize = 0;
            this.button_press.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_press.Location = new System.Drawing.Point(5, 25);
            this.button_press.Name = "button_press";
            this.button_press.Size = new System.Drawing.Size(127, 154);
            this.button_press.TabIndex = 0;
            this.button_press.Text = "\r\n\r\n压力图表";
            this.button_press.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button_press.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button_press.UseVisualStyleBackColor = true;
            this.button_press.Click += new System.EventHandler(this.button_press_Click);
            // 
            // Form_Data
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1350, 628);
            this.Controls.Add(this.roundPanel1);
            this.Controls.Add(this.roundPanel2);
            this.Font = new System.Drawing.Font("宋体", 12F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form_Data";
            this.Text = "信息查看";
            this.Load += new System.EventHandler(this.Form_Data_Load);
            this.roundPanel2.ResumeLayout(false);
            this.roundPanel_force.ResumeLayout(false);
            this.roundPanel_force.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.roundPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private AutoFrameUI.RoundPanel roundPanel2;
        private AutoFrameUI.RoundPanel roundPanel1;
        private System.Windows.Forms.Button button_press;
        private System.Windows.Forms.Button button_data;
        private AutoFrameUI.RoundPanel roundPanel_force;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Label label1;
        private AutoFrameUI.HalfRing halfRing1;
    }
}