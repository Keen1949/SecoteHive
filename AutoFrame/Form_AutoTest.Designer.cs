namespace AutoFrame
{
    partial class Form_AutoTest
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox_TestType = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dataGridView_Cylinder = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dataGridView_DI = new System.Windows.Forms.DataGridView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.dataGridView_DO = new System.Windows.Forms.DataGridView();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.dataGridView_AxisMove = new System.Windows.Forms.DataGridView();
            this.groupBox_TestItem = new System.Windows.Forms.GroupBox();
            this.dataGridView_TestItem = new System.Windows.Forms.DataGridView();
            this.groupBox_Operation = new System.Windows.Forms.GroupBox();
            this.button_ViewLastReport = new System.Windows.Forms.Button();
            this.button_Stop = new System.Windows.Forms.Button();
            this.button_Continue = new System.Windows.Forms.Button();
            this.button_Start = new System.Windows.Forms.Button();
            this.button_Save = new System.Windows.Forms.Button();
            this.button_open = new System.Windows.Forms.Button();
            this.button_Clear = new System.Windows.Forms.Button();
            this.button_MoveDown = new System.Windows.Forms.Button();
            this.button_MoveUp = new System.Windows.Forms.Button();
            this.button_Remove = new System.Windows.Forms.Button();
            this.button_Insert = new System.Windows.Forms.Button();
            this.button_Add = new System.Windows.Forms.Button();
            this.groupBox_Log = new System.Windows.Forms.GroupBox();
            this.listBox_Log = new ToolEx.ListBoxEx();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox_TestType.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Cylinder)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_DI)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_DO)).BeginInit();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_AxisMove)).BeginInit();
            this.groupBox_TestItem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_TestItem)).BeginInit();
            this.groupBox_Operation.SuspendLayout();
            this.groupBox_Log.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox_Log, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 79.35104F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.64897F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(920, 678);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel2.Controls.Add(this.groupBox_TestType, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.groupBox_TestItem, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.groupBox_Operation, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(914, 531);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // groupBox_TestType
            // 
            this.groupBox_TestType.Controls.Add(this.tabControl1);
            this.groupBox_TestType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_TestType.Location = new System.Drawing.Point(3, 3);
            this.groupBox_TestType.Name = "groupBox_TestType";
            this.groupBox_TestType.Size = new System.Drawing.Size(319, 525);
            this.groupBox_TestType.TabIndex = 0;
            this.groupBox_TestType.TabStop = false;
            this.groupBox_TestType.Text = "测试类型";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 18);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(313, 504);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGridView_Cylinder);
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(305, 477);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "气缸";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dataGridView_Cylinder
            // 
            this.dataGridView_Cylinder.AllowUserToAddRows = false;
            this.dataGridView_Cylinder.AllowUserToDeleteRows = false;
            this.dataGridView_Cylinder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_Cylinder.Location = new System.Drawing.Point(3, 3);
            this.dataGridView_Cylinder.Name = "dataGridView_Cylinder";
            this.dataGridView_Cylinder.ReadOnly = true;
            this.dataGridView_Cylinder.RowHeadersVisible = false;
            this.dataGridView_Cylinder.RowTemplate.Height = 23;
            this.dataGridView_Cylinder.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView_Cylinder.Size = new System.Drawing.Size(299, 471);
            this.dataGridView_Cylinder.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dataGridView_DI);
            this.tabPage2.Location = new System.Drawing.Point(4, 23);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(305, 478);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "DI";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dataGridView_DI
            // 
            this.dataGridView_DI.AllowUserToAddRows = false;
            this.dataGridView_DI.AllowUserToDeleteRows = false;
            this.dataGridView_DI.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_DI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_DI.Location = new System.Drawing.Point(3, 3);
            this.dataGridView_DI.Name = "dataGridView_DI";
            this.dataGridView_DI.ReadOnly = true;
            this.dataGridView_DI.RowHeadersVisible = false;
            this.dataGridView_DI.RowTemplate.Height = 23;
            this.dataGridView_DI.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView_DI.Size = new System.Drawing.Size(299, 472);
            this.dataGridView_DI.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.dataGridView_DO);
            this.tabPage3.Location = new System.Drawing.Point(4, 23);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(305, 478);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "DO";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // dataGridView_DO
            // 
            this.dataGridView_DO.AllowUserToAddRows = false;
            this.dataGridView_DO.AllowUserToDeleteRows = false;
            this.dataGridView_DO.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_DO.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_DO.Location = new System.Drawing.Point(3, 3);
            this.dataGridView_DO.Name = "dataGridView_DO";
            this.dataGridView_DO.ReadOnly = true;
            this.dataGridView_DO.RowHeadersVisible = false;
            this.dataGridView_DO.RowTemplate.Height = 23;
            this.dataGridView_DO.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView_DO.Size = new System.Drawing.Size(299, 472);
            this.dataGridView_DO.TabIndex = 1;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.dataGridView_AxisMove);
            this.tabPage4.Location = new System.Drawing.Point(4, 23);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(305, 478);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "轴运动";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // dataGridView_AxisMove
            // 
            this.dataGridView_AxisMove.AllowUserToAddRows = false;
            this.dataGridView_AxisMove.AllowUserToDeleteRows = false;
            this.dataGridView_AxisMove.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_AxisMove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_AxisMove.Location = new System.Drawing.Point(3, 3);
            this.dataGridView_AxisMove.Name = "dataGridView_AxisMove";
            this.dataGridView_AxisMove.ReadOnly = true;
            this.dataGridView_AxisMove.RowHeadersVisible = false;
            this.dataGridView_AxisMove.RowTemplate.Height = 23;
            this.dataGridView_AxisMove.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView_AxisMove.Size = new System.Drawing.Size(299, 472);
            this.dataGridView_AxisMove.TabIndex = 1;
            // 
            // groupBox_TestItem
            // 
            this.groupBox_TestItem.Controls.Add(this.dataGridView_TestItem);
            this.groupBox_TestItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_TestItem.Location = new System.Drawing.Point(428, 3);
            this.groupBox_TestItem.Name = "groupBox_TestItem";
            this.groupBox_TestItem.Size = new System.Drawing.Size(483, 525);
            this.groupBox_TestItem.TabIndex = 1;
            this.groupBox_TestItem.TabStop = false;
            this.groupBox_TestItem.Text = "测试项";
            // 
            // dataGridView_TestItem
            // 
            this.dataGridView_TestItem.AllowUserToAddRows = false;
            this.dataGridView_TestItem.AllowUserToDeleteRows = false;
            this.dataGridView_TestItem.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_TestItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_TestItem.Location = new System.Drawing.Point(3, 18);
            this.dataGridView_TestItem.Name = "dataGridView_TestItem";
            this.dataGridView_TestItem.RowTemplate.Height = 23;
            this.dataGridView_TestItem.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView_TestItem.Size = new System.Drawing.Size(477, 504);
            this.dataGridView_TestItem.TabIndex = 0;
            // 
            // groupBox_Operation
            // 
            this.groupBox_Operation.Controls.Add(this.button_ViewLastReport);
            this.groupBox_Operation.Controls.Add(this.button_Stop);
            this.groupBox_Operation.Controls.Add(this.button_Continue);
            this.groupBox_Operation.Controls.Add(this.button_Start);
            this.groupBox_Operation.Controls.Add(this.button_Save);
            this.groupBox_Operation.Controls.Add(this.button_open);
            this.groupBox_Operation.Controls.Add(this.button_Clear);
            this.groupBox_Operation.Controls.Add(this.button_MoveDown);
            this.groupBox_Operation.Controls.Add(this.button_MoveUp);
            this.groupBox_Operation.Controls.Add(this.button_Remove);
            this.groupBox_Operation.Controls.Add(this.button_Insert);
            this.groupBox_Operation.Controls.Add(this.button_Add);
            this.groupBox_Operation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_Operation.Location = new System.Drawing.Point(328, 3);
            this.groupBox_Operation.Name = "groupBox_Operation";
            this.groupBox_Operation.Size = new System.Drawing.Size(94, 525);
            this.groupBox_Operation.TabIndex = 2;
            this.groupBox_Operation.TabStop = false;
            this.groupBox_Operation.Text = "操作";
            // 
            // button_ViewLastReport
            // 
            this.button_ViewLastReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_ViewLastReport.Location = new System.Drawing.Point(10, 471);
            this.button_ViewLastReport.Name = "button_ViewLastReport";
            this.button_ViewLastReport.Size = new System.Drawing.Size(75, 45);
            this.button_ViewLastReport.TabIndex = 1;
            this.button_ViewLastReport.Text = "查看最后一次报告";
            this.button_ViewLastReport.UseVisualStyleBackColor = true;
            this.button_ViewLastReport.Click += new System.EventHandler(this.button_ViewLastReport_Click);
            // 
            // button_Stop
            // 
            this.button_Stop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_Stop.Location = new System.Drawing.Point(10, 437);
            this.button_Stop.Name = "button_Stop";
            this.button_Stop.Size = new System.Drawing.Size(74, 23);
            this.button_Stop.TabIndex = 0;
            this.button_Stop.Text = "停止";
            this.button_Stop.UseVisualStyleBackColor = true;
            this.button_Stop.Click += new System.EventHandler(this.button_Stop_Click);
            // 
            // button_Continue
            // 
            this.button_Continue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_Continue.Location = new System.Drawing.Point(11, 403);
            this.button_Continue.Name = "button_Continue";
            this.button_Continue.Size = new System.Drawing.Size(74, 23);
            this.button_Continue.TabIndex = 0;
            this.button_Continue.Text = "继续";
            this.button_Continue.UseVisualStyleBackColor = true;
            this.button_Continue.Click += new System.EventHandler(this.button_Continue_Click);
            // 
            // button_Start
            // 
            this.button_Start.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_Start.Location = new System.Drawing.Point(10, 369);
            this.button_Start.Name = "button_Start";
            this.button_Start.Size = new System.Drawing.Size(74, 23);
            this.button_Start.TabIndex = 0;
            this.button_Start.Text = "开始";
            this.button_Start.UseVisualStyleBackColor = true;
            this.button_Start.Click += new System.EventHandler(this.button_Start_Click);
            // 
            // button_Save
            // 
            this.button_Save.Location = new System.Drawing.Point(10, 224);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(74, 23);
            this.button_Save.TabIndex = 0;
            this.button_Save.Text = "保存";
            this.button_Save.UseVisualStyleBackColor = true;
            this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // button_open
            // 
            this.button_open.Location = new System.Drawing.Point(10, 197);
            this.button_open.Name = "button_open";
            this.button_open.Size = new System.Drawing.Size(74, 23);
            this.button_open.TabIndex = 0;
            this.button_open.Text = "打开";
            this.button_open.UseVisualStyleBackColor = true;
            this.button_open.Click += new System.EventHandler(this.button_open_Click);
            // 
            // button_Clear
            // 
            this.button_Clear.Location = new System.Drawing.Point(10, 155);
            this.button_Clear.Name = "button_Clear";
            this.button_Clear.Size = new System.Drawing.Size(74, 23);
            this.button_Clear.TabIndex = 0;
            this.button_Clear.Text = "清空";
            this.button_Clear.UseVisualStyleBackColor = true;
            this.button_Clear.Click += new System.EventHandler(this.button_Clear_Click);
            // 
            // button_MoveDown
            // 
            this.button_MoveDown.Location = new System.Drawing.Point(10, 128);
            this.button_MoveDown.Name = "button_MoveDown";
            this.button_MoveDown.Size = new System.Drawing.Size(74, 23);
            this.button_MoveDown.TabIndex = 0;
            this.button_MoveDown.Text = "下移";
            this.button_MoveDown.UseVisualStyleBackColor = true;
            this.button_MoveDown.Click += new System.EventHandler(this.button_MoveDown_Click);
            // 
            // button_MoveUp
            // 
            this.button_MoveUp.Location = new System.Drawing.Point(10, 101);
            this.button_MoveUp.Name = "button_MoveUp";
            this.button_MoveUp.Size = new System.Drawing.Size(74, 23);
            this.button_MoveUp.TabIndex = 0;
            this.button_MoveUp.Text = "上移";
            this.button_MoveUp.UseVisualStyleBackColor = true;
            this.button_MoveUp.Click += new System.EventHandler(this.button_MoveUp_Click);
            // 
            // button_Remove
            // 
            this.button_Remove.Location = new System.Drawing.Point(10, 74);
            this.button_Remove.Name = "button_Remove";
            this.button_Remove.Size = new System.Drawing.Size(74, 23);
            this.button_Remove.TabIndex = 0;
            this.button_Remove.Text = "移除";
            this.button_Remove.UseVisualStyleBackColor = true;
            this.button_Remove.Click += new System.EventHandler(this.button_Remove_Click);
            // 
            // button_Insert
            // 
            this.button_Insert.Location = new System.Drawing.Point(10, 47);
            this.button_Insert.Name = "button_Insert";
            this.button_Insert.Size = new System.Drawing.Size(74, 23);
            this.button_Insert.TabIndex = 0;
            this.button_Insert.Text = "插入";
            this.button_Insert.UseVisualStyleBackColor = true;
            this.button_Insert.Click += new System.EventHandler(this.button_Insert_Click);
            // 
            // button_Add
            // 
            this.button_Add.Location = new System.Drawing.Point(10, 20);
            this.button_Add.Name = "button_Add";
            this.button_Add.Size = new System.Drawing.Size(74, 23);
            this.button_Add.TabIndex = 0;
            this.button_Add.Text = "添加";
            this.button_Add.UseVisualStyleBackColor = true;
            this.button_Add.Click += new System.EventHandler(this.button_Add_Click);
            // 
            // groupBox_Log
            // 
            this.groupBox_Log.Controls.Add(this.listBox_Log);
            this.groupBox_Log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_Log.Location = new System.Drawing.Point(3, 540);
            this.groupBox_Log.Name = "groupBox_Log";
            this.groupBox_Log.Size = new System.Drawing.Size(914, 135);
            this.groupBox_Log.TabIndex = 1;
            this.groupBox_Log.TabStop = false;
            this.groupBox_Log.Text = "日志";
            // 
            // listBox_Log
            // 
            this.listBox_Log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox_Log.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.listBox_Log.FormattingEnabled = true;
            this.listBox_Log.HorizontalScrollbar = true;
            this.listBox_Log.Location = new System.Drawing.Point(3, 18);
            this.listBox_Log.Name = "listBox_Log";
            this.listBox_Log.Size = new System.Drawing.Size(908, 114);
            this.listBox_Log.TabIndex = 0;
            // 
            // Form_AutoTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(920, 678);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "Form_AutoTest";
            this.Text = "自动测试";
            this.Load += new System.EventHandler(this.Form_AutoTest_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.groupBox_TestType.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Cylinder)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_DI)).EndInit();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_DO)).EndInit();
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_AxisMove)).EndInit();
            this.groupBox_TestItem.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_TestItem)).EndInit();
            this.groupBox_Operation.ResumeLayout(false);
            this.groupBox_Log.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBox_TestType;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.GroupBox groupBox_TestItem;
        private System.Windows.Forms.DataGridView dataGridView_TestItem;
        private System.Windows.Forms.GroupBox groupBox_Operation;
        private System.Windows.Forms.Button button_Stop;
        private System.Windows.Forms.Button button_Start;
        private System.Windows.Forms.Button button_Save;
        private System.Windows.Forms.Button button_open;
        private System.Windows.Forms.Button button_Clear;
        private System.Windows.Forms.Button button_MoveDown;
        private System.Windows.Forms.Button button_MoveUp;
        private System.Windows.Forms.Button button_Remove;
        private System.Windows.Forms.Button button_Insert;
        private System.Windows.Forms.Button button_Add;
        private System.Windows.Forms.GroupBox groupBox_Log;
        private ToolEx.ListBoxEx listBox_Log;
        private System.Windows.Forms.DataGridView dataGridView_Cylinder;
        private System.Windows.Forms.DataGridView dataGridView_DI;
        private System.Windows.Forms.DataGridView dataGridView_DO;
        private System.Windows.Forms.DataGridView dataGridView_AxisMove;
        private System.Windows.Forms.Button button_ViewLastReport;
        private System.Windows.Forms.Button button_Continue;
    }
}