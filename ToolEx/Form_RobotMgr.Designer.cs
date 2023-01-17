namespace ToolEx
{
    partial class Form_RobotMgr
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_RobotMgr));
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_RobotName = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_Mode = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox_Remote = new System.Windows.Forms.ComboBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox_mgr = new System.Windows.Forms.GroupBox();
            this.button_DelRobot = new System.Windows.Forms.Button();
            this.button_Save = new System.Windows.Forms.Button();
            this.button_DelSelect = new System.Windows.Forms.Button();
            this.button_Apply = new System.Windows.Forms.Button();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.dataGridView_sysIoIn = new System.Windows.Forms.DataGridView();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.dataGridView_sysIoOut = new System.Windows.Forms.DataGridView();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.dataGridView_IoIn = new System.Windows.Forms.DataGridView();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.dataGridView_IoOut = new System.Windows.Forms.DataGridView();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.dataGridView_Point = new System.Windows.Forms.DataGridView();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.dataGridView_Cmd = new System.Windows.Forms.DataGridView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this.comboBox_Vendor = new System.Windows.Forms.ComboBox();
            this.comboBox_Manul = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.comboBox_Monitor = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox_MotionCtrol = new System.Windows.Forms.GroupBox();
            this.comboBox_MoveMode = new System.Windows.Forms.ComboBox();
            this.label19 = new System.Windows.Forms.Label();
            this.comboBox_Point = new System.Windows.Forms.ComboBox();
            this.label18 = new System.Windows.Forms.Label();
            this.button_Move = new System.Windows.Forms.Button();
            this.button_PointMove = new System.Windows.Forms.Button();
            this.button_Teach = new System.Windows.Forms.Button();
            this.button_Set = new System.Windows.Forms.Button();
            this.radioButton_Abs = new System.Windows.Forms.RadioButton();
            this.comboBox_Speed = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.radioButton_Relative = new System.Windows.Forms.RadioButton();
            this.comboBox_Tool = new System.Windows.Forms.ComboBox();
            this.radioButton_Continue = new System.Windows.Forms.RadioButton();
            this.label16 = new System.Windows.Forms.Label();
            this.comboBox_Local = new System.Windows.Forms.ComboBox();
            this.button_W_P = new System.Windows.Forms.Button();
            this.button_V_P = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.button_U_P = new System.Windows.Forms.Button();
            this.button_PowerHigh = new System.Windows.Forms.Button();
            this.button_MotorOff = new System.Windows.Forms.Button();
            this.button_Z_P = new System.Windows.Forms.Button();
            this.button_PowerLow = new System.Windows.Forms.Button();
            this.button_Y_P = new System.Windows.Forms.Button();
            this.button_MotorOn = new System.Windows.Forms.Button();
            this.button_Reset = new System.Windows.Forms.Button();
            this.textBox_StepW = new System.Windows.Forms.TextBox();
            this.textBox_StepV = new System.Windows.Forms.TextBox();
            this.textBox_StepU = new System.Windows.Forms.TextBox();
            this.textBox_StepZ = new System.Windows.Forms.TextBox();
            this.textBox_StepY = new System.Windows.Forms.TextBox();
            this.textBox_StepX = new System.Windows.Forms.TextBox();
            this.button_X_N = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.button_W_N = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.button_V_N = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.button_U_N = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.button_Z_N = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.button_X_P = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.button_Y_N = new System.Windows.Forms.Button();
            this.groupBox_CurPos = new System.Windows.Forms.GroupBox();
            this.textBox_W = new System.Windows.Forms.TextBox();
            this.textBox_Z = new System.Windows.Forms.TextBox();
            this.textBox_V = new System.Windows.Forms.TextBox();
            this.textBox_Y = new System.Windows.Forms.TextBox();
            this.textBox_U = new System.Windows.Forms.TextBox();
            this.textBox_X = new System.Windows.Forms.TextBox();
            this.label_W = new System.Windows.Forms.Label();
            this.label_Z = new System.Windows.Forms.Label();
            this.label_V = new System.Windows.Forms.Label();
            this.label_Y = new System.Windows.Forms.Label();
            this.label_U = new System.Windows.Forms.Label();
            this.label_X = new System.Windows.Forms.Label();
            this.groupBox_Cmd = new System.Windows.Forms.GroupBox();
            this.textBox_Receive = new System.Windows.Forms.TextBox();
            this.button_Clear = new System.Windows.Forms.Button();
            this.button_Send = new System.Windows.Forms.Button();
            this.textBox_params = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox_Cmd = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox_CommInfo = new System.Windows.Forms.GroupBox();
            this.button_CloseMonitor = new System.Windows.Forms.Button();
            this.button_OpenMonitor = new System.Windows.Forms.Button();
            this.button_close = new System.Windows.Forms.Button();
            this.button_open = new System.Windows.Forms.Button();
            this.textBox_info = new System.Windows.Forms.TextBox();
            this.groupBox_sysout = new System.Windows.Forms.GroupBox();
            this.panel_sysIoOut = new System.Windows.Forms.Panel();
            this.groupBox_ioOut = new System.Windows.Forms.GroupBox();
            this.panel_IoOut = new System.Windows.Forms.Panel();
            this.groupBox_ioIn = new System.Windows.Forms.GroupBox();
            this.panel_IoIn = new System.Windows.Forms.Panel();
            this.groupBox_sysin = new System.Windows.Forms.GroupBox();
            this.panel_sysIoIn = new System.Windows.Forms.Panel();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox_mgr.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_sysIoIn)).BeginInit();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_sysIoOut)).BeginInit();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_IoIn)).BeginInit();
            this.tabPage6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_IoOut)).BeginInit();
            this.tabPage7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Point)).BeginInit();
            this.tabPage8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Cmd)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox_MotionCtrol.SuspendLayout();
            this.groupBox_CurPos.SuspendLayout();
            this.groupBox_Cmd.SuspendLayout();
            this.groupBox_CommInfo.SuspendLayout();
            this.groupBox_sysout.SuspendLayout();
            this.groupBox_ioOut.SuspendLayout();
            this.groupBox_ioIn.SuspendLayout();
            this.groupBox_sysin.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "机器人名称:";
            // 
            // comboBox_RobotName
            // 
            this.comboBox_RobotName.FormattingEnabled = true;
            this.comboBox_RobotName.Location = new System.Drawing.Point(83, 9);
            this.comboBox_RobotName.Name = "comboBox_RobotName";
            this.comboBox_RobotName.Size = new System.Drawing.Size(160, 20);
            this.comboBox_RobotName.TabIndex = 1;
            this.comboBox_RobotName.SelectedIndexChanged += new System.EventHandler(this.comboBox_RobotName_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(385, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "通信方式:";
            // 
            // comboBox_Mode
            // 
            this.comboBox_Mode.FormattingEnabled = true;
            this.comboBox_Mode.Location = new System.Drawing.Point(446, 9);
            this.comboBox_Mode.Name = "comboBox_Mode";
            this.comboBox_Mode.Size = new System.Drawing.Size(92, 20);
            this.comboBox_Mode.TabIndex = 1;
            this.comboBox_Mode.SelectedIndexChanged += new System.EventHandler(this.comboBox_Mode_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(545, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "远程:";
            // 
            // comboBox_Remote
            // 
            this.comboBox_Remote.FormattingEnabled = true;
            this.comboBox_Remote.Location = new System.Drawing.Point(589, 9);
            this.comboBox_Remote.Name = "comboBox_Remote";
            this.comboBox_Remote.Size = new System.Drawing.Size(68, 20);
            this.comboBox_Remote.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(13, 35);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(966, 741);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(958, 715);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "调试界面";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.White;
            this.imageList1.Images.SetKeyName(0, "light_gray.png");
            this.imageList1.Images.SetKeyName(1, "light_green.png");
            this.imageList1.Images.SetKeyName(2, "servo_Left.png");
            this.imageList1.Images.SetKeyName(3, "servo_right.png");
            this.imageList1.Images.SetKeyName(4, "servo_up.png");
            this.imageList1.Images.SetKeyName(5, "servo_down.png");
            this.imageList1.Images.SetKeyName(6, "servo_turn_right.png");
            this.imageList1.Images.SetKeyName(7, "servo_turn_left.png");
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox_mgr);
            this.tabPage2.Controls.Add(this.tabControl2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(958, 715);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "管理界面";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox_mgr
            // 
            this.groupBox_mgr.Controls.Add(this.button_DelRobot);
            this.groupBox_mgr.Controls.Add(this.button_Save);
            this.groupBox_mgr.Controls.Add(this.button_DelSelect);
            this.groupBox_mgr.Controls.Add(this.button_Apply);
            this.groupBox_mgr.Location = new System.Drawing.Point(828, 25);
            this.groupBox_mgr.Name = "groupBox_mgr";
            this.groupBox_mgr.Size = new System.Drawing.Size(131, 234);
            this.groupBox_mgr.TabIndex = 9;
            this.groupBox_mgr.TabStop = false;
            // 
            // button_DelRobot
            // 
            this.button_DelRobot.Location = new System.Drawing.Point(14, 20);
            this.button_DelRobot.Name = "button_DelRobot";
            this.button_DelRobot.Size = new System.Drawing.Size(106, 48);
            this.button_DelRobot.TabIndex = 0;
            this.button_DelRobot.Text = "删除机器人";
            this.button_DelRobot.UseVisualStyleBackColor = true;
            this.button_DelRobot.Click += new System.EventHandler(this.button_DelRobot_Click);
            // 
            // button_Save
            // 
            this.button_Save.Location = new System.Drawing.Point(14, 170);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(106, 48);
            this.button_Save.TabIndex = 2;
            this.button_Save.Text = "保存";
            this.button_Save.UseVisualStyleBackColor = true;
            this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // button_DelSelect
            // 
            this.button_DelSelect.Location = new System.Drawing.Point(14, 70);
            this.button_DelSelect.Name = "button_DelSelect";
            this.button_DelSelect.Size = new System.Drawing.Size(106, 48);
            this.button_DelSelect.TabIndex = 0;
            this.button_DelSelect.Text = "删除选中项";
            this.button_DelSelect.UseVisualStyleBackColor = true;
            this.button_DelSelect.Click += new System.EventHandler(this.button_DelSelect_Click);
            // 
            // button_Apply
            // 
            this.button_Apply.Location = new System.Drawing.Point(14, 120);
            this.button_Apply.Name = "button_Apply";
            this.button_Apply.Size = new System.Drawing.Size(106, 48);
            this.button_Apply.TabIndex = 1;
            this.button_Apply.Text = "应用";
            this.button_Apply.UseVisualStyleBackColor = true;
            this.button_Apply.Click += new System.EventHandler(this.button_Apply_Click);
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Controls.Add(this.tabPage5);
            this.tabControl2.Controls.Add(this.tabPage6);
            this.tabControl2.Controls.Add(this.tabPage7);
            this.tabControl2.Controls.Add(this.tabPage8);
            this.tabControl2.Location = new System.Drawing.Point(3, 3);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(823, 712);
            this.tabControl2.TabIndex = 8;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.dataGridView_sysIoIn);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(815, 686);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "远程输入";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // dataGridView_sysIoIn
            // 
            this.dataGridView_sysIoIn.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_sysIoIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_sysIoIn.Location = new System.Drawing.Point(3, 3);
            this.dataGridView_sysIoIn.Name = "dataGridView_sysIoIn";
            this.dataGridView_sysIoIn.RowTemplate.Height = 23;
            this.dataGridView_sysIoIn.Size = new System.Drawing.Size(809, 680);
            this.dataGridView_sysIoIn.TabIndex = 0;
            this.dataGridView_sysIoIn.CurrentCellChanged += new System.EventHandler(this.dataGridView_IoIn_CurrentCellChanged);
            this.dataGridView_sysIoIn.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dataGridView_Scroll);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.dataGridView_sysIoOut);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(815, 686);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "远程输出";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // dataGridView_sysIoOut
            // 
            this.dataGridView_sysIoOut.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_sysIoOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_sysIoOut.Location = new System.Drawing.Point(3, 3);
            this.dataGridView_sysIoOut.Name = "dataGridView_sysIoOut";
            this.dataGridView_sysIoOut.RowTemplate.Height = 23;
            this.dataGridView_sysIoOut.Size = new System.Drawing.Size(809, 680);
            this.dataGridView_sysIoOut.TabIndex = 1;
            this.dataGridView_sysIoOut.CurrentCellChanged += new System.EventHandler(this.dataGridView_IoOut_CurrentCellChanged);
            this.dataGridView_sysIoOut.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dataGridView_Scroll);
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.dataGridView_IoIn);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(815, 686);
            this.tabPage5.TabIndex = 2;
            this.tabPage5.Text = "机器人输入";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // dataGridView_IoIn
            // 
            this.dataGridView_IoIn.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_IoIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_IoIn.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_IoIn.Name = "dataGridView_IoIn";
            this.dataGridView_IoIn.RowTemplate.Height = 23;
            this.dataGridView_IoIn.Size = new System.Drawing.Size(815, 686);
            this.dataGridView_IoIn.TabIndex = 0;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.dataGridView_IoOut);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(815, 686);
            this.tabPage6.TabIndex = 3;
            this.tabPage6.Text = "机器人输出";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // dataGridView_IoOut
            // 
            this.dataGridView_IoOut.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_IoOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_IoOut.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_IoOut.Name = "dataGridView_IoOut";
            this.dataGridView_IoOut.RowTemplate.Height = 23;
            this.dataGridView_IoOut.Size = new System.Drawing.Size(815, 686);
            this.dataGridView_IoOut.TabIndex = 0;
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.dataGridView_Point);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Size = new System.Drawing.Size(815, 686);
            this.tabPage7.TabIndex = 4;
            this.tabPage7.Text = "机器点位";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // dataGridView_Point
            // 
            this.dataGridView_Point.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Point.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_Point.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_Point.Name = "dataGridView_Point";
            this.dataGridView_Point.RowTemplate.Height = 23;
            this.dataGridView_Point.Size = new System.Drawing.Size(815, 686);
            this.dataGridView_Point.TabIndex = 0;
            // 
            // tabPage8
            // 
            this.tabPage8.Controls.Add(this.dataGridView_Cmd);
            this.tabPage8.Location = new System.Drawing.Point(4, 22);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Size = new System.Drawing.Size(815, 686);
            this.tabPage8.TabIndex = 5;
            this.tabPage8.Text = "机器人命令";
            this.tabPage8.UseVisualStyleBackColor = true;
            // 
            // dataGridView_Cmd
            // 
            this.dataGridView_Cmd.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Cmd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_Cmd.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_Cmd.Name = "dataGridView_Cmd";
            this.dataGridView_Cmd.RowTemplate.Height = 23;
            this.dataGridView_Cmd.Size = new System.Drawing.Size(815, 686);
            this.dataGridView_Cmd.TabIndex = 1;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(247, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "品牌:";
            // 
            // comboBox_Vendor
            // 
            this.comboBox_Vendor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Vendor.FormattingEnabled = true;
            this.comboBox_Vendor.Location = new System.Drawing.Point(285, 9);
            this.comboBox_Vendor.Name = "comboBox_Vendor";
            this.comboBox_Vendor.Size = new System.Drawing.Size(94, 20);
            this.comboBox_Vendor.TabIndex = 1;
            // 
            // comboBox_Manul
            // 
            this.comboBox_Manul.FormattingEnabled = true;
            this.comboBox_Manul.Location = new System.Drawing.Point(735, 9);
            this.comboBox_Manul.Name = "comboBox_Manul";
            this.comboBox_Manul.Size = new System.Drawing.Size(68, 20);
            this.comboBox_Manul.TabIndex = 4;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(664, 13);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 12);
            this.label7.TabIndex = 3;
            this.label7.Text = "手控:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(808, 13);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 12);
            this.label8.TabIndex = 3;
            this.label8.Text = "监视:";
            // 
            // comboBox_Monitor
            // 
            this.comboBox_Monitor.FormattingEnabled = true;
            this.comboBox_Monitor.Location = new System.Drawing.Point(857, 9);
            this.comboBox_Monitor.Name = "comboBox_Monitor";
            this.comboBox_Monitor.Size = new System.Drawing.Size(68, 20);
            this.comboBox_Monitor.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.groupBox_MotionCtrol);
            this.panel1.Controls.Add(this.groupBox_CurPos);
            this.panel1.Controls.Add(this.groupBox_Cmd);
            this.panel1.Controls.Add(this.groupBox_CommInfo);
            this.panel1.Controls.Add(this.groupBox_sysout);
            this.panel1.Controls.Add(this.groupBox_ioOut);
            this.panel1.Controls.Add(this.groupBox_ioIn);
            this.panel1.Controls.Add(this.groupBox_sysin);
            this.panel1.Location = new System.Drawing.Point(3, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(952, 685);
            this.panel1.TabIndex = 18;
            // 
            // groupBox_MotionCtrol
            // 
            this.groupBox_MotionCtrol.Controls.Add(this.comboBox_MoveMode);
            this.groupBox_MotionCtrol.Controls.Add(this.label19);
            this.groupBox_MotionCtrol.Controls.Add(this.comboBox_Point);
            this.groupBox_MotionCtrol.Controls.Add(this.label18);
            this.groupBox_MotionCtrol.Controls.Add(this.button_Move);
            this.groupBox_MotionCtrol.Controls.Add(this.button_PointMove);
            this.groupBox_MotionCtrol.Controls.Add(this.button_Teach);
            this.groupBox_MotionCtrol.Controls.Add(this.button_Set);
            this.groupBox_MotionCtrol.Controls.Add(this.radioButton_Abs);
            this.groupBox_MotionCtrol.Controls.Add(this.comboBox_Speed);
            this.groupBox_MotionCtrol.Controls.Add(this.label17);
            this.groupBox_MotionCtrol.Controls.Add(this.radioButton_Relative);
            this.groupBox_MotionCtrol.Controls.Add(this.comboBox_Tool);
            this.groupBox_MotionCtrol.Controls.Add(this.radioButton_Continue);
            this.groupBox_MotionCtrol.Controls.Add(this.label16);
            this.groupBox_MotionCtrol.Controls.Add(this.comboBox_Local);
            this.groupBox_MotionCtrol.Controls.Add(this.button_W_P);
            this.groupBox_MotionCtrol.Controls.Add(this.button_V_P);
            this.groupBox_MotionCtrol.Controls.Add(this.label15);
            this.groupBox_MotionCtrol.Controls.Add(this.button_U_P);
            this.groupBox_MotionCtrol.Controls.Add(this.button_PowerHigh);
            this.groupBox_MotionCtrol.Controls.Add(this.button_MotorOff);
            this.groupBox_MotionCtrol.Controls.Add(this.button_Z_P);
            this.groupBox_MotionCtrol.Controls.Add(this.button_PowerLow);
            this.groupBox_MotionCtrol.Controls.Add(this.button_Y_P);
            this.groupBox_MotionCtrol.Controls.Add(this.button_MotorOn);
            this.groupBox_MotionCtrol.Controls.Add(this.button_Reset);
            this.groupBox_MotionCtrol.Controls.Add(this.textBox_StepW);
            this.groupBox_MotionCtrol.Controls.Add(this.textBox_StepV);
            this.groupBox_MotionCtrol.Controls.Add(this.textBox_StepU);
            this.groupBox_MotionCtrol.Controls.Add(this.textBox_StepZ);
            this.groupBox_MotionCtrol.Controls.Add(this.textBox_StepY);
            this.groupBox_MotionCtrol.Controls.Add(this.textBox_StepX);
            this.groupBox_MotionCtrol.Controls.Add(this.button_X_N);
            this.groupBox_MotionCtrol.Controls.Add(this.label14);
            this.groupBox_MotionCtrol.Controls.Add(this.button_W_N);
            this.groupBox_MotionCtrol.Controls.Add(this.label13);
            this.groupBox_MotionCtrol.Controls.Add(this.button_V_N);
            this.groupBox_MotionCtrol.Controls.Add(this.label12);
            this.groupBox_MotionCtrol.Controls.Add(this.button_U_N);
            this.groupBox_MotionCtrol.Controls.Add(this.label11);
            this.groupBox_MotionCtrol.Controls.Add(this.button_Z_N);
            this.groupBox_MotionCtrol.Controls.Add(this.label10);
            this.groupBox_MotionCtrol.Controls.Add(this.button_X_P);
            this.groupBox_MotionCtrol.Controls.Add(this.label9);
            this.groupBox_MotionCtrol.Controls.Add(this.button_Y_N);
            this.groupBox_MotionCtrol.Location = new System.Drawing.Point(250, 103);
            this.groupBox_MotionCtrol.Name = "groupBox_MotionCtrol";
            this.groupBox_MotionCtrol.Size = new System.Drawing.Size(437, 350);
            this.groupBox_MotionCtrol.TabIndex = 25;
            this.groupBox_MotionCtrol.TabStop = false;
            this.groupBox_MotionCtrol.Text = "运动控制";
            // 
            // comboBox_MoveMode
            // 
            this.comboBox_MoveMode.FormattingEnabled = true;
            this.comboBox_MoveMode.Location = new System.Drawing.Point(81, 294);
            this.comboBox_MoveMode.Name = "comboBox_MoveMode";
            this.comboBox_MoveMode.Size = new System.Drawing.Size(186, 20);
            this.comboBox_MoveMode.TabIndex = 5;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(10, 298);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(53, 12);
            this.label19.TabIndex = 4;
            this.label19.Text = "运动模式";
            // 
            // comboBox_Point
            // 
            this.comboBox_Point.FormattingEnabled = true;
            this.comboBox_Point.Location = new System.Drawing.Point(50, 320);
            this.comboBox_Point.Name = "comboBox_Point";
            this.comboBox_Point.Size = new System.Drawing.Size(217, 20);
            this.comboBox_Point.TabIndex = 5;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(10, 324);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(29, 12);
            this.label18.TabIndex = 4;
            this.label18.Text = "点位";
            // 
            // button_Move
            // 
            this.button_Move.Location = new System.Drawing.Point(273, 242);
            this.button_Move.Name = "button_Move";
            this.button_Move.Size = new System.Drawing.Size(75, 36);
            this.button_Move.TabIndex = 3;
            this.button_Move.Text = "执行运动";
            this.button_Move.UseVisualStyleBackColor = true;
            this.button_Move.Click += new System.EventHandler(this.button_Move_Click);
            // 
            // button_PointMove
            // 
            this.button_PointMove.Location = new System.Drawing.Point(351, 296);
            this.button_PointMove.Name = "button_PointMove";
            this.button_PointMove.Size = new System.Drawing.Size(75, 36);
            this.button_PointMove.TabIndex = 3;
            this.button_PointMove.Text = "点位运动";
            this.button_PointMove.UseVisualStyleBackColor = true;
            this.button_PointMove.Click += new System.EventHandler(this.button_PointMove_Click);
            // 
            // button_Teach
            // 
            this.button_Teach.Location = new System.Drawing.Point(273, 296);
            this.button_Teach.Name = "button_Teach";
            this.button_Teach.Size = new System.Drawing.Size(75, 36);
            this.button_Teach.TabIndex = 3;
            this.button_Teach.Text = "示教";
            this.button_Teach.UseVisualStyleBackColor = true;
            this.button_Teach.Click += new System.EventHandler(this.button_Teach_Click);
            // 
            // button_Set
            // 
            this.button_Set.Location = new System.Drawing.Point(351, 18);
            this.button_Set.Name = "button_Set";
            this.button_Set.Size = new System.Drawing.Size(75, 23);
            this.button_Set.TabIndex = 3;
            this.button_Set.Text = "设置";
            this.button_Set.UseVisualStyleBackColor = true;
            this.button_Set.Click += new System.EventHandler(this.button_Set_Click);
            // 
            // radioButton_Abs
            // 
            this.radioButton_Abs.AutoSize = true;
            this.radioButton_Abs.Location = new System.Drawing.Point(173, 54);
            this.radioButton_Abs.Name = "radioButton_Abs";
            this.radioButton_Abs.Size = new System.Drawing.Size(71, 16);
            this.radioButton_Abs.TabIndex = 2;
            this.radioButton_Abs.TabStop = true;
            this.radioButton_Abs.Text = "绝对运动";
            this.radioButton_Abs.UseVisualStyleBackColor = true;
            this.radioButton_Abs.CheckedChanged += new System.EventHandler(this.radioButton_Abs_CheckedChanged);
            // 
            // comboBox_Speed
            // 
            this.comboBox_Speed.FormattingEnabled = true;
            this.comboBox_Speed.Location = new System.Drawing.Point(271, 19);
            this.comboBox_Speed.Name = "comboBox_Speed";
            this.comboBox_Speed.Size = new System.Drawing.Size(71, 20);
            this.comboBox_Speed.TabIndex = 2;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(214, 23);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(53, 12);
            this.label17.TabIndex = 0;
            this.label17.Text = "Speed(%)";
            // 
            // radioButton_Relative
            // 
            this.radioButton_Relative.AutoSize = true;
            this.radioButton_Relative.Location = new System.Drawing.Point(91, 54);
            this.radioButton_Relative.Name = "radioButton_Relative";
            this.radioButton_Relative.Size = new System.Drawing.Size(71, 16);
            this.radioButton_Relative.TabIndex = 2;
            this.radioButton_Relative.TabStop = true;
            this.radioButton_Relative.Text = "相对运动";
            this.radioButton_Relative.UseVisualStyleBackColor = true;
            this.radioButton_Relative.CheckedChanged += new System.EventHandler(this.radioButton_Relative_CheckedChanged);
            // 
            // comboBox_Tool
            // 
            this.comboBox_Tool.FormattingEnabled = true;
            this.comboBox_Tool.Location = new System.Drawing.Point(152, 19);
            this.comboBox_Tool.Name = "comboBox_Tool";
            this.comboBox_Tool.Size = new System.Drawing.Size(51, 20);
            this.comboBox_Tool.TabIndex = 2;
            // 
            // radioButton_Continue
            // 
            this.radioButton_Continue.AutoSize = true;
            this.radioButton_Continue.Location = new System.Drawing.Point(12, 54);
            this.radioButton_Continue.Name = "radioButton_Continue";
            this.radioButton_Continue.Size = new System.Drawing.Size(71, 16);
            this.radioButton_Continue.TabIndex = 2;
            this.radioButton_Continue.TabStop = true;
            this.radioButton_Continue.Text = "连续运动";
            this.radioButton_Continue.UseVisualStyleBackColor = true;
            this.radioButton_Continue.CheckedChanged += new System.EventHandler(this.radioButton_Continue_CheckedChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(113, 23);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(29, 12);
            this.label16.TabIndex = 0;
            this.label16.Text = "Tool";
            // 
            // comboBox_Local
            // 
            this.comboBox_Local.FormattingEnabled = true;
            this.comboBox_Local.Location = new System.Drawing.Point(50, 19);
            this.comboBox_Local.Name = "comboBox_Local";
            this.comboBox_Local.Size = new System.Drawing.Size(51, 20);
            this.comboBox_Local.TabIndex = 2;
            // 
            // button_W_P
            // 
            this.button_W_P.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_W_P.ImageIndex = 7;
            this.button_W_P.ImageList = this.imageList1;
            this.button_W_P.Location = new System.Drawing.Point(169, 241);
            this.button_W_P.Name = "button_W_P";
            this.button_W_P.Size = new System.Drawing.Size(48, 46);
            this.button_W_P.TabIndex = 0;
            this.button_W_P.Text = "W+";
            this.button_W_P.UseVisualStyleBackColor = true;
            this.button_W_P.Click += new System.EventHandler(this.buttonMove_Click);
            // 
            // button_V_P
            // 
            this.button_V_P.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_V_P.ImageIndex = 7;
            this.button_V_P.ImageList = this.imageList1;
            this.button_V_P.Location = new System.Drawing.Point(88, 241);
            this.button_V_P.Name = "button_V_P";
            this.button_V_P.Size = new System.Drawing.Size(48, 46);
            this.button_V_P.TabIndex = 0;
            this.button_V_P.Text = "V+";
            this.button_V_P.UseVisualStyleBackColor = true;
            this.button_V_P.Click += new System.EventHandler(this.buttonMove_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(10, 23);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(35, 12);
            this.label15.TabIndex = 0;
            this.label15.Text = "Local";
            // 
            // button_U_P
            // 
            this.button_U_P.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_U_P.ImageIndex = 7;
            this.button_U_P.ImageList = this.imageList1;
            this.button_U_P.Location = new System.Drawing.Point(7, 241);
            this.button_U_P.Name = "button_U_P";
            this.button_U_P.Size = new System.Drawing.Size(48, 46);
            this.button_U_P.TabIndex = 0;
            this.button_U_P.Text = "U+";
            this.button_U_P.UseVisualStyleBackColor = true;
            this.button_U_P.Click += new System.EventHandler(this.buttonMove_Click);
            // 
            // button_PowerHigh
            // 
            this.button_PowerHigh.Location = new System.Drawing.Point(351, 202);
            this.button_PowerHigh.Name = "button_PowerHigh";
            this.button_PowerHigh.Size = new System.Drawing.Size(75, 36);
            this.button_PowerHigh.TabIndex = 0;
            this.button_PowerHigh.Text = "Power High";
            this.button_PowerHigh.UseVisualStyleBackColor = true;
            this.button_PowerHigh.Click += new System.EventHandler(this.button_PowerHigh_Click);
            // 
            // button_MotorOff
            // 
            this.button_MotorOff.Location = new System.Drawing.Point(351, 122);
            this.button_MotorOff.Name = "button_MotorOff";
            this.button_MotorOff.Size = new System.Drawing.Size(75, 36);
            this.button_MotorOff.TabIndex = 0;
            this.button_MotorOff.Text = "Motor Off";
            this.button_MotorOff.UseVisualStyleBackColor = true;
            this.button_MotorOff.Click += new System.EventHandler(this.button_MotorOff_Click);
            // 
            // button_Z_P
            // 
            this.button_Z_P.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Z_P.ImageIndex = 4;
            this.button_Z_P.ImageList = this.imageList1;
            this.button_Z_P.Location = new System.Drawing.Point(169, 85);
            this.button_Z_P.Name = "button_Z_P";
            this.button_Z_P.Size = new System.Drawing.Size(48, 46);
            this.button_Z_P.TabIndex = 0;
            this.button_Z_P.Text = "Z+";
            this.button_Z_P.UseVisualStyleBackColor = true;
            this.button_Z_P.Click += new System.EventHandler(this.buttonMove_Click);
            // 
            // button_PowerLow
            // 
            this.button_PowerLow.Location = new System.Drawing.Point(351, 162);
            this.button_PowerLow.Name = "button_PowerLow";
            this.button_PowerLow.Size = new System.Drawing.Size(75, 36);
            this.button_PowerLow.TabIndex = 0;
            this.button_PowerLow.Text = "Power Low";
            this.button_PowerLow.UseVisualStyleBackColor = true;
            this.button_PowerLow.Click += new System.EventHandler(this.button_PowerLow_Click);
            // 
            // button_Y_P
            // 
            this.button_Y_P.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Y_P.ImageIndex = 5;
            this.button_Y_P.ImageList = this.imageList1;
            this.button_Y_P.Location = new System.Drawing.Point(61, 136);
            this.button_Y_P.Name = "button_Y_P";
            this.button_Y_P.Size = new System.Drawing.Size(48, 46);
            this.button_Y_P.TabIndex = 0;
            this.button_Y_P.Text = "Y+";
            this.button_Y_P.UseVisualStyleBackColor = true;
            this.button_Y_P.Click += new System.EventHandler(this.buttonMove_Click);
            // 
            // button_MotorOn
            // 
            this.button_MotorOn.BackColor = System.Drawing.Color.Transparent;
            this.button_MotorOn.Location = new System.Drawing.Point(351, 82);
            this.button_MotorOn.Name = "button_MotorOn";
            this.button_MotorOn.Size = new System.Drawing.Size(75, 36);
            this.button_MotorOn.TabIndex = 0;
            this.button_MotorOn.Text = "Motor On";
            this.button_MotorOn.UseVisualStyleBackColor = false;
            this.button_MotorOn.Click += new System.EventHandler(this.button_MotorOn_Click);
            // 
            // button_Reset
            // 
            this.button_Reset.Location = new System.Drawing.Point(351, 242);
            this.button_Reset.Name = "button_Reset";
            this.button_Reset.Size = new System.Drawing.Size(75, 36);
            this.button_Reset.TabIndex = 0;
            this.button_Reset.Text = "Reset";
            this.button_Reset.UseVisualStyleBackColor = true;
            this.button_Reset.Click += new System.EventHandler(this.button_Reset_Click);
            // 
            // textBox_StepW
            // 
            this.textBox_StepW.Location = new System.Drawing.Point(268, 217);
            this.textBox_StepW.Name = "textBox_StepW";
            this.textBox_StepW.Size = new System.Drawing.Size(70, 21);
            this.textBox_StepW.TabIndex = 1;
            // 
            // textBox_StepV
            // 
            this.textBox_StepV.Location = new System.Drawing.Point(268, 190);
            this.textBox_StepV.Name = "textBox_StepV";
            this.textBox_StepV.Size = new System.Drawing.Size(70, 21);
            this.textBox_StepV.TabIndex = 1;
            // 
            // textBox_StepU
            // 
            this.textBox_StepU.Location = new System.Drawing.Point(268, 163);
            this.textBox_StepU.Name = "textBox_StepU";
            this.textBox_StepU.Size = new System.Drawing.Size(70, 21);
            this.textBox_StepU.TabIndex = 1;
            // 
            // textBox_StepZ
            // 
            this.textBox_StepZ.Location = new System.Drawing.Point(268, 136);
            this.textBox_StepZ.Name = "textBox_StepZ";
            this.textBox_StepZ.Size = new System.Drawing.Size(70, 21);
            this.textBox_StepZ.TabIndex = 1;
            // 
            // textBox_StepY
            // 
            this.textBox_StepY.Location = new System.Drawing.Point(268, 109);
            this.textBox_StepY.Name = "textBox_StepY";
            this.textBox_StepY.Size = new System.Drawing.Size(70, 21);
            this.textBox_StepY.TabIndex = 1;
            // 
            // textBox_StepX
            // 
            this.textBox_StepX.Location = new System.Drawing.Point(268, 82);
            this.textBox_StepX.Name = "textBox_StepX";
            this.textBox_StepX.Size = new System.Drawing.Size(70, 21);
            this.textBox_StepX.TabIndex = 1;
            // 
            // button_X_N
            // 
            this.button_X_N.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_X_N.ImageIndex = 3;
            this.button_X_N.ImageList = this.imageList1;
            this.button_X_N.Location = new System.Drawing.Point(115, 113);
            this.button_X_N.Name = "button_X_N";
            this.button_X_N.Size = new System.Drawing.Size(48, 46);
            this.button_X_N.TabIndex = 0;
            this.button_X_N.Text = "X-";
            this.button_X_N.UseVisualStyleBackColor = true;
            this.button_X_N.Click += new System.EventHandler(this.buttonMove_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(225, 221);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(41, 12);
            this.label14.TabIndex = 0;
            this.label14.Text = "W(deg)";
            // 
            // button_W_N
            // 
            this.button_W_N.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_W_N.ImageIndex = 6;
            this.button_W_N.ImageList = this.imageList1;
            this.button_W_N.Location = new System.Drawing.Point(169, 190);
            this.button_W_N.Name = "button_W_N";
            this.button_W_N.Size = new System.Drawing.Size(48, 46);
            this.button_W_N.TabIndex = 0;
            this.button_W_N.Text = "W-";
            this.button_W_N.UseVisualStyleBackColor = true;
            this.button_W_N.Click += new System.EventHandler(this.buttonMove_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(225, 194);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(41, 12);
            this.label13.TabIndex = 0;
            this.label13.Text = "V(deg)";
            // 
            // button_V_N
            // 
            this.button_V_N.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_V_N.ImageIndex = 6;
            this.button_V_N.ImageList = this.imageList1;
            this.button_V_N.Location = new System.Drawing.Point(88, 190);
            this.button_V_N.Name = "button_V_N";
            this.button_V_N.Size = new System.Drawing.Size(48, 46);
            this.button_V_N.TabIndex = 0;
            this.button_V_N.Text = "V-";
            this.button_V_N.UseVisualStyleBackColor = true;
            this.button_V_N.Click += new System.EventHandler(this.buttonMove_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(225, 167);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 12);
            this.label12.TabIndex = 0;
            this.label12.Text = "U(deg)";
            // 
            // button_U_N
            // 
            this.button_U_N.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_U_N.ImageIndex = 6;
            this.button_U_N.ImageList = this.imageList1;
            this.button_U_N.Location = new System.Drawing.Point(7, 190);
            this.button_U_N.Name = "button_U_N";
            this.button_U_N.Size = new System.Drawing.Size(48, 46);
            this.button_U_N.TabIndex = 0;
            this.button_U_N.Text = "U-";
            this.button_U_N.UseVisualStyleBackColor = true;
            this.button_U_N.Click += new System.EventHandler(this.buttonMove_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(225, 140);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(35, 12);
            this.label11.TabIndex = 0;
            this.label11.Text = "Z(mm)";
            // 
            // button_Z_N
            // 
            this.button_Z_N.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Z_N.ImageIndex = 5;
            this.button_Z_N.ImageList = this.imageList1;
            this.button_Z_N.Location = new System.Drawing.Point(169, 136);
            this.button_Z_N.Name = "button_Z_N";
            this.button_Z_N.Size = new System.Drawing.Size(48, 46);
            this.button_Z_N.TabIndex = 0;
            this.button_Z_N.Text = "Z-";
            this.button_Z_N.UseVisualStyleBackColor = true;
            this.button_Z_N.Click += new System.EventHandler(this.buttonMove_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(225, 113);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 12);
            this.label10.TabIndex = 0;
            this.label10.Text = "Y(mm)";
            // 
            // button_X_P
            // 
            this.button_X_P.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_X_P.ImageIndex = 2;
            this.button_X_P.ImageList = this.imageList1;
            this.button_X_P.Location = new System.Drawing.Point(7, 113);
            this.button_X_P.Name = "button_X_P";
            this.button_X_P.Size = new System.Drawing.Size(48, 46);
            this.button_X_P.TabIndex = 0;
            this.button_X_P.Text = "X+";
            this.button_X_P.UseVisualStyleBackColor = true;
            this.button_X_P.Click += new System.EventHandler(this.buttonMove_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(225, 86);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(35, 12);
            this.label9.TabIndex = 0;
            this.label9.Text = "X(mm)";
            // 
            // button_Y_N
            // 
            this.button_Y_N.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Y_N.ImageIndex = 4;
            this.button_Y_N.ImageList = this.imageList1;
            this.button_Y_N.Location = new System.Drawing.Point(61, 85);
            this.button_Y_N.Name = "button_Y_N";
            this.button_Y_N.Size = new System.Drawing.Size(48, 46);
            this.button_Y_N.TabIndex = 0;
            this.button_Y_N.Text = "Y-";
            this.button_Y_N.UseVisualStyleBackColor = true;
            this.button_Y_N.Click += new System.EventHandler(this.buttonMove_Click);
            // 
            // groupBox_CurPos
            // 
            this.groupBox_CurPos.Controls.Add(this.textBox_W);
            this.groupBox_CurPos.Controls.Add(this.textBox_Z);
            this.groupBox_CurPos.Controls.Add(this.textBox_V);
            this.groupBox_CurPos.Controls.Add(this.textBox_Y);
            this.groupBox_CurPos.Controls.Add(this.textBox_U);
            this.groupBox_CurPos.Controls.Add(this.textBox_X);
            this.groupBox_CurPos.Controls.Add(this.label_W);
            this.groupBox_CurPos.Controls.Add(this.label_Z);
            this.groupBox_CurPos.Controls.Add(this.label_V);
            this.groupBox_CurPos.Controls.Add(this.label_Y);
            this.groupBox_CurPos.Controls.Add(this.label_U);
            this.groupBox_CurPos.Controls.Add(this.label_X);
            this.groupBox_CurPos.Location = new System.Drawing.Point(250, 0);
            this.groupBox_CurPos.Name = "groupBox_CurPos";
            this.groupBox_CurPos.Size = new System.Drawing.Size(437, 97);
            this.groupBox_CurPos.TabIndex = 24;
            this.groupBox_CurPos.TabStop = false;
            this.groupBox_CurPos.Text = "当前位置";
            // 
            // textBox_W
            // 
            this.textBox_W.Location = new System.Drawing.Point(338, 59);
            this.textBox_W.Name = "textBox_W";
            this.textBox_W.ReadOnly = true;
            this.textBox_W.Size = new System.Drawing.Size(82, 21);
            this.textBox_W.TabIndex = 1;
            // 
            // textBox_Z
            // 
            this.textBox_Z.Location = new System.Drawing.Point(338, 24);
            this.textBox_Z.Name = "textBox_Z";
            this.textBox_Z.ReadOnly = true;
            this.textBox_Z.Size = new System.Drawing.Size(82, 21);
            this.textBox_Z.TabIndex = 1;
            // 
            // textBox_V
            // 
            this.textBox_V.Location = new System.Drawing.Point(191, 59);
            this.textBox_V.Name = "textBox_V";
            this.textBox_V.ReadOnly = true;
            this.textBox_V.Size = new System.Drawing.Size(82, 21);
            this.textBox_V.TabIndex = 1;
            // 
            // textBox_Y
            // 
            this.textBox_Y.Location = new System.Drawing.Point(191, 24);
            this.textBox_Y.Name = "textBox_Y";
            this.textBox_Y.ReadOnly = true;
            this.textBox_Y.Size = new System.Drawing.Size(82, 21);
            this.textBox_Y.TabIndex = 1;
            // 
            // textBox_U
            // 
            this.textBox_U.Location = new System.Drawing.Point(50, 59);
            this.textBox_U.Name = "textBox_U";
            this.textBox_U.ReadOnly = true;
            this.textBox_U.Size = new System.Drawing.Size(82, 21);
            this.textBox_U.TabIndex = 1;
            // 
            // textBox_X
            // 
            this.textBox_X.Location = new System.Drawing.Point(50, 24);
            this.textBox_X.Name = "textBox_X";
            this.textBox_X.ReadOnly = true;
            this.textBox_X.Size = new System.Drawing.Size(82, 21);
            this.textBox_X.TabIndex = 1;
            // 
            // label_W
            // 
            this.label_W.AutoSize = true;
            this.label_W.Location = new System.Drawing.Point(294, 63);
            this.label_W.Name = "label_W";
            this.label_W.Size = new System.Drawing.Size(41, 12);
            this.label_W.TabIndex = 0;
            this.label_W.Text = "W(deg)";
            // 
            // label_Z
            // 
            this.label_Z.AutoSize = true;
            this.label_Z.Location = new System.Drawing.Point(294, 28);
            this.label_Z.Name = "label_Z";
            this.label_Z.Size = new System.Drawing.Size(35, 12);
            this.label_Z.TabIndex = 0;
            this.label_Z.Text = "Z(mm)";
            // 
            // label_V
            // 
            this.label_V.AutoSize = true;
            this.label_V.Location = new System.Drawing.Point(147, 63);
            this.label_V.Name = "label_V";
            this.label_V.Size = new System.Drawing.Size(41, 12);
            this.label_V.TabIndex = 0;
            this.label_V.Text = "V(deg)";
            // 
            // label_Y
            // 
            this.label_Y.AutoSize = true;
            this.label_Y.Location = new System.Drawing.Point(153, 28);
            this.label_Y.Name = "label_Y";
            this.label_Y.Size = new System.Drawing.Size(35, 12);
            this.label_Y.TabIndex = 0;
            this.label_Y.Text = "Y(mm)";
            // 
            // label_U
            // 
            this.label_U.AutoSize = true;
            this.label_U.Location = new System.Drawing.Point(6, 63);
            this.label_U.Name = "label_U";
            this.label_U.Size = new System.Drawing.Size(41, 12);
            this.label_U.TabIndex = 0;
            this.label_U.Text = "U(deg)";
            // 
            // label_X
            // 
            this.label_X.AutoSize = true;
            this.label_X.Location = new System.Drawing.Point(12, 28);
            this.label_X.Name = "label_X";
            this.label_X.Size = new System.Drawing.Size(35, 12);
            this.label_X.TabIndex = 0;
            this.label_X.Text = "X(mm)";
            // 
            // groupBox_Cmd
            // 
            this.groupBox_Cmd.Controls.Add(this.textBox_Receive);
            this.groupBox_Cmd.Controls.Add(this.button_Clear);
            this.groupBox_Cmd.Controls.Add(this.button_Send);
            this.groupBox_Cmd.Controls.Add(this.textBox_params);
            this.groupBox_Cmd.Controls.Add(this.label5);
            this.groupBox_Cmd.Controls.Add(this.comboBox_Cmd);
            this.groupBox_Cmd.Controls.Add(this.label4);
            this.groupBox_Cmd.Location = new System.Drawing.Point(250, 468);
            this.groupBox_Cmd.Name = "groupBox_Cmd";
            this.groupBox_Cmd.Size = new System.Drawing.Size(437, 238);
            this.groupBox_Cmd.TabIndex = 23;
            this.groupBox_Cmd.TabStop = false;
            this.groupBox_Cmd.Text = "命令";
            // 
            // textBox_Receive
            // 
            this.textBox_Receive.Location = new System.Drawing.Point(9, 55);
            this.textBox_Receive.Multiline = true;
            this.textBox_Receive.Name = "textBox_Receive";
            this.textBox_Receive.ReadOnly = true;
            this.textBox_Receive.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_Receive.Size = new System.Drawing.Size(331, 165);
            this.textBox_Receive.TabIndex = 5;
            // 
            // button_Clear
            // 
            this.button_Clear.Location = new System.Drawing.Point(346, 197);
            this.button_Clear.Name = "button_Clear";
            this.button_Clear.Size = new System.Drawing.Size(75, 23);
            this.button_Clear.TabIndex = 4;
            this.button_Clear.Text = "清空";
            this.button_Clear.UseVisualStyleBackColor = true;
            this.button_Clear.Click += new System.EventHandler(this.button_Clear_Click);
            // 
            // button_Send
            // 
            this.button_Send.Location = new System.Drawing.Point(346, 16);
            this.button_Send.Name = "button_Send";
            this.button_Send.Size = new System.Drawing.Size(75, 23);
            this.button_Send.TabIndex = 4;
            this.button_Send.Text = "发送";
            this.button_Send.UseVisualStyleBackColor = true;
            this.button_Send.Click += new System.EventHandler(this.button_Send_Click);
            // 
            // textBox_params
            // 
            this.textBox_params.Location = new System.Drawing.Point(192, 18);
            this.textBox_params.Name = "textBox_params";
            this.textBox_params.Size = new System.Drawing.Size(148, 21);
            this.textBox_params.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(150, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 12);
            this.label5.TabIndex = 2;
            this.label5.Text = "参数:";
            // 
            // comboBox_Cmd
            // 
            this.comboBox_Cmd.FormattingEnabled = true;
            this.comboBox_Cmd.Location = new System.Drawing.Point(44, 18);
            this.comboBox_Cmd.Name = "comboBox_Cmd";
            this.comboBox_Cmd.Size = new System.Drawing.Size(96, 20);
            this.comboBox_Cmd.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "命令:";
            // 
            // groupBox_CommInfo
            // 
            this.groupBox_CommInfo.Controls.Add(this.button_CloseMonitor);
            this.groupBox_CommInfo.Controls.Add(this.button_OpenMonitor);
            this.groupBox_CommInfo.Controls.Add(this.button_close);
            this.groupBox_CommInfo.Controls.Add(this.button_open);
            this.groupBox_CommInfo.Controls.Add(this.textBox_info);
            this.groupBox_CommInfo.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox_CommInfo.Location = new System.Drawing.Point(3, -3);
            this.groupBox_CommInfo.Name = "groupBox_CommInfo";
            this.groupBox_CommInfo.Size = new System.Drawing.Size(244, 231);
            this.groupBox_CommInfo.TabIndex = 22;
            this.groupBox_CommInfo.TabStop = false;
            this.groupBox_CommInfo.Text = "通讯端口信息";
            // 
            // button_CloseMonitor
            // 
            this.button_CloseMonitor.Location = new System.Drawing.Point(136, 192);
            this.button_CloseMonitor.Name = "button_CloseMonitor";
            this.button_CloseMonitor.Size = new System.Drawing.Size(102, 31);
            this.button_CloseMonitor.TabIndex = 10;
            this.button_CloseMonitor.Text = "Close";
            this.button_CloseMonitor.UseVisualStyleBackColor = true;
            this.button_CloseMonitor.Click += new System.EventHandler(this.button_CloseMonitor_Click);
            // 
            // button_OpenMonitor
            // 
            this.button_OpenMonitor.Location = new System.Drawing.Point(6, 192);
            this.button_OpenMonitor.Name = "button_OpenMonitor";
            this.button_OpenMonitor.Size = new System.Drawing.Size(114, 31);
            this.button_OpenMonitor.TabIndex = 10;
            this.button_OpenMonitor.Text = "打开监控";
            this.button_OpenMonitor.UseVisualStyleBackColor = true;
            this.button_OpenMonitor.Click += new System.EventHandler(this.button_OpenMonitor_Click);
            // 
            // button_close
            // 
            this.button_close.Location = new System.Drawing.Point(136, 155);
            this.button_close.Name = "button_close";
            this.button_close.Size = new System.Drawing.Size(102, 31);
            this.button_close.TabIndex = 10;
            this.button_close.Text = "Close";
            this.button_close.UseVisualStyleBackColor = true;
            this.button_close.Click += new System.EventHandler(this.button_close_Click);
            // 
            // button_open
            // 
            this.button_open.Location = new System.Drawing.Point(6, 155);
            this.button_open.Name = "button_open";
            this.button_open.Size = new System.Drawing.Size(114, 31);
            this.button_open.TabIndex = 10;
            this.button_open.Text = "打开远程";
            this.button_open.UseVisualStyleBackColor = true;
            this.button_open.Click += new System.EventHandler(this.button_open_Click);
            // 
            // textBox_info
            // 
            this.textBox_info.Location = new System.Drawing.Point(4, 25);
            this.textBox_info.Multiline = true;
            this.textBox_info.Name = "textBox_info";
            this.textBox_info.ReadOnly = true;
            this.textBox_info.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_info.Size = new System.Drawing.Size(234, 124);
            this.textBox_info.TabIndex = 9;
            this.textBox_info.WordWrap = false;
            // 
            // groupBox_sysout
            // 
            this.groupBox_sysout.Controls.Add(this.panel_sysIoOut);
            this.groupBox_sysout.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox_sysout.Location = new System.Drawing.Point(3, 466);
            this.groupBox_sysout.Name = "groupBox_sysout";
            this.groupBox_sysout.Size = new System.Drawing.Size(245, 231);
            this.groupBox_sysout.TabIndex = 21;
            this.groupBox_sysout.TabStop = false;
            this.groupBox_sysout.Text = "远程输出";
            // 
            // panel_sysIoOut
            // 
            this.panel_sysIoOut.AutoScroll = true;
            this.panel_sysIoOut.BackColor = System.Drawing.Color.LightGray;
            this.panel_sysIoOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_sysIoOut.Location = new System.Drawing.Point(3, 22);
            this.panel_sysIoOut.Name = "panel_sysIoOut";
            this.panel_sysIoOut.Size = new System.Drawing.Size(239, 206);
            this.panel_sysIoOut.TabIndex = 1;
            // 
            // groupBox_ioOut
            // 
            this.groupBox_ioOut.Controls.Add(this.panel_IoOut);
            this.groupBox_ioOut.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox_ioOut.Location = new System.Drawing.Point(693, 289);
            this.groupBox_ioOut.Name = "groupBox_ioOut";
            this.groupBox_ioOut.Size = new System.Drawing.Size(247, 285);
            this.groupBox_ioOut.TabIndex = 18;
            this.groupBox_ioOut.TabStop = false;
            this.groupBox_ioOut.Text = "机器人输出";
            // 
            // panel_IoOut
            // 
            this.panel_IoOut.AutoScroll = true;
            this.panel_IoOut.BackColor = System.Drawing.Color.LightGray;
            this.panel_IoOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_IoOut.Location = new System.Drawing.Point(3, 22);
            this.panel_IoOut.Name = "panel_IoOut";
            this.panel_IoOut.Size = new System.Drawing.Size(241, 260);
            this.panel_IoOut.TabIndex = 0;
            // 
            // groupBox_ioIn
            // 
            this.groupBox_ioIn.Controls.Add(this.panel_IoIn);
            this.groupBox_ioIn.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox_ioIn.Location = new System.Drawing.Point(693, -2);
            this.groupBox_ioIn.Name = "groupBox_ioIn";
            this.groupBox_ioIn.Size = new System.Drawing.Size(247, 285);
            this.groupBox_ioIn.TabIndex = 19;
            this.groupBox_ioIn.TabStop = false;
            this.groupBox_ioIn.Text = "机器人输入";
            // 
            // panel_IoIn
            // 
            this.panel_IoIn.AutoScroll = true;
            this.panel_IoIn.BackColor = System.Drawing.Color.LightGray;
            this.panel_IoIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_IoIn.Location = new System.Drawing.Point(3, 22);
            this.panel_IoIn.Name = "panel_IoIn";
            this.panel_IoIn.Size = new System.Drawing.Size(241, 260);
            this.panel_IoIn.TabIndex = 0;
            // 
            // groupBox_sysin
            // 
            this.groupBox_sysin.Controls.Add(this.panel_sysIoIn);
            this.groupBox_sysin.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox_sysin.Location = new System.Drawing.Point(3, 232);
            this.groupBox_sysin.Name = "groupBox_sysin";
            this.groupBox_sysin.Size = new System.Drawing.Size(245, 231);
            this.groupBox_sysin.TabIndex = 20;
            this.groupBox_sysin.TabStop = false;
            this.groupBox_sysin.Text = "远程输入";
            // 
            // panel_sysIoIn
            // 
            this.panel_sysIoIn.AutoScroll = true;
            this.panel_sysIoIn.BackColor = System.Drawing.Color.LightGray;
            this.panel_sysIoIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_sysIoIn.Location = new System.Drawing.Point(3, 22);
            this.panel_sysIoIn.Name = "panel_sysIoIn";
            this.panel_sysIoIn.Size = new System.Drawing.Size(239, 206);
            this.panel_sysIoIn.TabIndex = 0;
            // 
            // Form_RobotMgr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 750);
            this.Controls.Add(this.comboBox_Monitor);
            this.Controls.Add(this.comboBox_Manul);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.comboBox_Vendor);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.comboBox_Remote);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBox_Mode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox_RobotName);
            this.Controls.Add(this.label1);
            this.Name = "Form_RobotMgr";
            this.Text = "机器人管理";
            this.Load += new System.EventHandler(this.Form_RobotMgr_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox_mgr.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_sysIoIn)).EndInit();
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_sysIoOut)).EndInit();
            this.tabPage5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_IoIn)).EndInit();
            this.tabPage6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_IoOut)).EndInit();
            this.tabPage7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Point)).EndInit();
            this.tabPage8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Cmd)).EndInit();
            this.panel1.ResumeLayout(false);
            this.groupBox_MotionCtrol.ResumeLayout(false);
            this.groupBox_MotionCtrol.PerformLayout();
            this.groupBox_CurPos.ResumeLayout(false);
            this.groupBox_CurPos.PerformLayout();
            this.groupBox_Cmd.ResumeLayout(false);
            this.groupBox_Cmd.PerformLayout();
            this.groupBox_CommInfo.ResumeLayout(false);
            this.groupBox_CommInfo.PerformLayout();
            this.groupBox_sysout.ResumeLayout(false);
            this.groupBox_ioOut.ResumeLayout(false);
            this.groupBox_ioIn.ResumeLayout(false);
            this.groupBox_sysin.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox_RobotName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox_Mode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox_Remote;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dataGridView_sysIoIn;
        private System.Windows.Forms.DataGridView dataGridView_Cmd;
        private System.Windows.Forms.DataGridView dataGridView_sysIoOut;
        private System.Windows.Forms.Button button_Save;
        private System.Windows.Forms.Button button_Apply;
        private System.Windows.Forms.Button button_DelRobot;
        private System.Windows.Forms.Button button_DelSelect;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBox_Vendor;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ComboBox comboBox_Manul;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboBox_Monitor;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.TabPage tabPage8;
        private System.Windows.Forms.DataGridView dataGridView_IoIn;
        private System.Windows.Forms.DataGridView dataGridView_IoOut;
        private System.Windows.Forms.DataGridView dataGridView_Point;
        private System.Windows.Forms.GroupBox groupBox_mgr;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox_MotionCtrol;
        private System.Windows.Forms.ComboBox comboBox_MoveMode;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.ComboBox comboBox_Point;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Button button_Move;
        private System.Windows.Forms.Button button_PointMove;
        private System.Windows.Forms.Button button_Teach;
        private System.Windows.Forms.Button button_Set;
        private System.Windows.Forms.RadioButton radioButton_Abs;
        private System.Windows.Forms.ComboBox comboBox_Speed;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.RadioButton radioButton_Relative;
        private System.Windows.Forms.ComboBox comboBox_Tool;
        private System.Windows.Forms.RadioButton radioButton_Continue;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox comboBox_Local;
        private System.Windows.Forms.Button button_W_P;
        private System.Windows.Forms.Button button_V_P;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button button_U_P;
        private System.Windows.Forms.Button button_PowerHigh;
        private System.Windows.Forms.Button button_MotorOff;
        private System.Windows.Forms.Button button_Z_P;
        private System.Windows.Forms.Button button_PowerLow;
        private System.Windows.Forms.Button button_Y_P;
        private System.Windows.Forms.Button button_MotorOn;
        private System.Windows.Forms.Button button_Reset;
        private System.Windows.Forms.TextBox textBox_StepW;
        private System.Windows.Forms.TextBox textBox_StepV;
        private System.Windows.Forms.TextBox textBox_StepU;
        private System.Windows.Forms.TextBox textBox_StepZ;
        private System.Windows.Forms.TextBox textBox_StepY;
        private System.Windows.Forms.TextBox textBox_StepX;
        private System.Windows.Forms.Button button_X_N;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button button_W_N;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button button_V_N;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button button_U_N;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button button_Z_N;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button button_X_P;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button button_Y_N;
        private System.Windows.Forms.GroupBox groupBox_CurPos;
        private System.Windows.Forms.TextBox textBox_W;
        private System.Windows.Forms.TextBox textBox_Z;
        private System.Windows.Forms.TextBox textBox_V;
        private System.Windows.Forms.TextBox textBox_Y;
        private System.Windows.Forms.TextBox textBox_U;
        private System.Windows.Forms.TextBox textBox_X;
        private System.Windows.Forms.Label label_W;
        private System.Windows.Forms.Label label_Z;
        private System.Windows.Forms.Label label_V;
        private System.Windows.Forms.Label label_Y;
        private System.Windows.Forms.Label label_U;
        private System.Windows.Forms.Label label_X;
        private System.Windows.Forms.GroupBox groupBox_Cmd;
        private System.Windows.Forms.TextBox textBox_Receive;
        private System.Windows.Forms.Button button_Clear;
        private System.Windows.Forms.Button button_Send;
        private System.Windows.Forms.TextBox textBox_params;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBox_Cmd;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox_CommInfo;
        private System.Windows.Forms.Button button_CloseMonitor;
        private System.Windows.Forms.Button button_OpenMonitor;
        private System.Windows.Forms.Button button_close;
        private System.Windows.Forms.Button button_open;
        private System.Windows.Forms.TextBox textBox_info;
        private System.Windows.Forms.GroupBox groupBox_sysout;
        private System.Windows.Forms.Panel panel_sysIoOut;
        private System.Windows.Forms.GroupBox groupBox_ioOut;
        private System.Windows.Forms.Panel panel_IoOut;
        private System.Windows.Forms.GroupBox groupBox_ioIn;
        private System.Windows.Forms.Panel panel_IoIn;
        private System.Windows.Forms.GroupBox groupBox_sysin;
        private System.Windows.Forms.Panel panel_sysIoIn;
    }
}