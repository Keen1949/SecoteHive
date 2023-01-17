namespace AutoFrame
{
    partial class Form_Robot
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Robot));
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox_out = new System.Windows.Forms.GroupBox();
            this.lampButton7 = new AutoFrameUI.LampButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.lampButton5 = new AutoFrameUI.LampButton();
            this.lampButton3 = new AutoFrameUI.LampButton();
            this.lampButton1 = new AutoFrameUI.LampButton();
            this.lampButton2 = new AutoFrameUI.LampButton();
            this.groupBox_in = new System.Windows.Forms.GroupBox();
            this.lampButton6 = new AutoFrameUI.LampButton();
            this.lampButton4 = new AutoFrameUI.LampButton();
            this.button_in_3 = new AutoFrameUI.LampButton();
            this.button_in_2 = new AutoFrameUI.LampButton();
            this.button_in_1 = new AutoFrameUI.LampButton();
            this.listBox_receive = new System.Windows.Forms.ListBox();
            this.tabControl_send = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.listBox_cmd = new System.Windows.Forms.ListBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.button_write_clear = new System.Windows.Forms.Button();
            this.checkBox_hex = new System.Windows.Forms.CheckBox();
            this.textBox_cmd = new System.Windows.Forms.TextBox();
            this.button_send = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_EthOrCom = new System.Windows.Forms.ComboBox();
            this.button_close = new System.Windows.Forms.Button();
            this.button_open = new System.Windows.Forms.Button();
            this.textBox_info = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button_read_clear = new System.Windows.Forms.Button();
            this.groupBox_out.SuspendLayout();
            this.groupBox_in.SuspendLayout();
            this.tabControl_send.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 12);
            this.label1.TabIndex = 9;
            // 
            // groupBox_out
            // 
            this.groupBox_out.Controls.Add(this.lampButton7);
            this.groupBox_out.Controls.Add(this.lampButton5);
            this.groupBox_out.Controls.Add(this.lampButton3);
            this.groupBox_out.Controls.Add(this.lampButton1);
            this.groupBox_out.Controls.Add(this.lampButton2);
            this.groupBox_out.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox_out.Location = new System.Drawing.Point(461, 1);
            this.groupBox_out.Name = "groupBox_out";
            this.groupBox_out.Size = new System.Drawing.Size(234, 219);
            this.groupBox_out.TabIndex = 11;
            this.groupBox_out.TabStop = false;
            this.groupBox_out.Text = "输出";
            // 
            // lampButton7
            // 
            this.lampButton7.AutoSize = true;
            this.lampButton7.BackColor = System.Drawing.Color.White;
            this.lampButton7.FlatAppearance.BorderSize = 0;
            this.lampButton7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lampButton7.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lampButton7.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lampButton7.ImageIndex = 0;
            this.lampButton7.ImageList = this.imageList1;
            this.lampButton7.Location = new System.Drawing.Point(4, 158);
            this.lampButton7.Margin = new System.Windows.Forms.Padding(0);
            this.lampButton7.Name = "lampButton7";
            this.lampButton7.Size = new System.Drawing.Size(165, 38);
            this.lampButton7.TabIndex = 3;
            this.lampButton7.Text = "robot reset";
            this.lampButton7.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.lampButton7.UseVisualStyleBackColor = false;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.White;
            this.imageList1.Images.SetKeyName(0, "light_gray.png");
            this.imageList1.Images.SetKeyName(1, "light_green.png");
            // 
            // lampButton5
            // 
            this.lampButton5.AutoSize = true;
            this.lampButton5.BackColor = System.Drawing.Color.White;
            this.lampButton5.FlatAppearance.BorderSize = 0;
            this.lampButton5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lampButton5.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lampButton5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lampButton5.ImageIndex = 0;
            this.lampButton5.ImageList = this.imageList1;
            this.lampButton5.Location = new System.Drawing.Point(4, 124);
            this.lampButton5.Margin = new System.Windows.Forms.Padding(0);
            this.lampButton5.Name = "lampButton5";
            this.lampButton5.Size = new System.Drawing.Size(166, 38);
            this.lampButton5.TabIndex = 3;
            this.lampButton5.Text = "robot continue";
            this.lampButton5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.lampButton5.UseVisualStyleBackColor = false;
            // 
            // lampButton3
            // 
            this.lampButton3.AutoSize = true;
            this.lampButton3.BackColor = System.Drawing.Color.White;
            this.lampButton3.FlatAppearance.BorderSize = 0;
            this.lampButton3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lampButton3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lampButton3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lampButton3.ImageIndex = 0;
            this.lampButton3.ImageList = this.imageList1;
            this.lampButton3.Location = new System.Drawing.Point(4, 90);
            this.lampButton3.Margin = new System.Windows.Forms.Padding(0);
            this.lampButton3.Name = "lampButton3";
            this.lampButton3.Size = new System.Drawing.Size(165, 38);
            this.lampButton3.TabIndex = 3;
            this.lampButton3.Text = "robot pause";
            this.lampButton3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.lampButton3.UseVisualStyleBackColor = false;
            // 
            // lampButton1
            // 
            this.lampButton1.AutoSize = true;
            this.lampButton1.BackColor = System.Drawing.Color.White;
            this.lampButton1.FlatAppearance.BorderSize = 0;
            this.lampButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lampButton1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lampButton1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lampButton1.ImageIndex = 0;
            this.lampButton1.ImageList = this.imageList1;
            this.lampButton1.Location = new System.Drawing.Point(4, 22);
            this.lampButton1.Margin = new System.Windows.Forms.Padding(0);
            this.lampButton1.Name = "lampButton1";
            this.lampButton1.Size = new System.Drawing.Size(165, 38);
            this.lampButton1.TabIndex = 3;
            this.lampButton1.Text = "robot start";
            this.lampButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.lampButton1.UseVisualStyleBackColor = false;
            // 
            // lampButton2
            // 
            this.lampButton2.AutoSize = true;
            this.lampButton2.BackColor = System.Drawing.Color.White;
            this.lampButton2.FlatAppearance.BorderSize = 0;
            this.lampButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lampButton2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lampButton2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lampButton2.ImageIndex = 0;
            this.lampButton2.ImageList = this.imageList1;
            this.lampButton2.Location = new System.Drawing.Point(4, 56);
            this.lampButton2.Margin = new System.Windows.Forms.Padding(0);
            this.lampButton2.Name = "lampButton2";
            this.lampButton2.Size = new System.Drawing.Size(165, 38);
            this.lampButton2.TabIndex = 3;
            this.lampButton2.Text = "robot stop";
            this.lampButton2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.lampButton2.UseVisualStyleBackColor = false;
            // 
            // groupBox_in
            // 
            this.groupBox_in.Controls.Add(this.lampButton6);
            this.groupBox_in.Controls.Add(this.lampButton4);
            this.groupBox_in.Controls.Add(this.button_in_3);
            this.groupBox_in.Controls.Add(this.button_in_2);
            this.groupBox_in.Controls.Add(this.button_in_1);
            this.groupBox_in.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox_in.Location = new System.Drawing.Point(221, 1);
            this.groupBox_in.Name = "groupBox_in";
            this.groupBox_in.Size = new System.Drawing.Size(234, 219);
            this.groupBox_in.TabIndex = 10;
            this.groupBox_in.TabStop = false;
            this.groupBox_in.Text = "输入";
            // 
            // lampButton6
            // 
            this.lampButton6.AutoSize = true;
            this.lampButton6.BackColor = System.Drawing.Color.White;
            this.lampButton6.FlatAppearance.BorderSize = 0;
            this.lampButton6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lampButton6.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lampButton6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lampButton6.ImageIndex = 0;
            this.lampButton6.ImageList = this.imageList1;
            this.lampButton6.Location = new System.Drawing.Point(5, 158);
            this.lampButton6.Margin = new System.Windows.Forms.Padding(0);
            this.lampButton6.Name = "lampButton6";
            this.lampButton6.Size = new System.Drawing.Size(164, 38);
            this.lampButton6.TabIndex = 3;
            this.lampButton6.Text = "robot Warning";
            this.lampButton6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.lampButton6.UseVisualStyleBackColor = false;
            // 
            // lampButton4
            // 
            this.lampButton4.AutoSize = true;
            this.lampButton4.BackColor = System.Drawing.Color.White;
            this.lampButton4.FlatAppearance.BorderSize = 0;
            this.lampButton4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lampButton4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lampButton4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lampButton4.ImageIndex = 0;
            this.lampButton4.ImageList = this.imageList1;
            this.lampButton4.Location = new System.Drawing.Point(5, 124);
            this.lampButton4.Margin = new System.Windows.Forms.Padding(0);
            this.lampButton4.Name = "lampButton4";
            this.lampButton4.Size = new System.Drawing.Size(166, 38);
            this.lampButton4.TabIndex = 3;
            this.lampButton4.Text = "robot EStopOn";
            this.lampButton4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.lampButton4.UseVisualStyleBackColor = false;
            // 
            // button_in_3
            // 
            this.button_in_3.AutoSize = true;
            this.button_in_3.BackColor = System.Drawing.Color.White;
            this.button_in_3.FlatAppearance.BorderSize = 0;
            this.button_in_3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_in_3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_in_3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_in_3.ImageIndex = 0;
            this.button_in_3.ImageList = this.imageList1;
            this.button_in_3.Location = new System.Drawing.Point(5, 90);
            this.button_in_3.Margin = new System.Windows.Forms.Padding(0);
            this.button_in_3.Name = "button_in_3";
            this.button_in_3.Size = new System.Drawing.Size(163, 38);
            this.button_in_3.TabIndex = 3;
            this.button_in_3.Text = "robot error";
            this.button_in_3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button_in_3.UseVisualStyleBackColor = false;
            // 
            // button_in_2
            // 
            this.button_in_2.AutoSize = true;
            this.button_in_2.BackColor = System.Drawing.Color.White;
            this.button_in_2.FlatAppearance.BorderSize = 0;
            this.button_in_2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_in_2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_in_2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_in_2.ImageIndex = 0;
            this.button_in_2.ImageList = this.imageList1;
            this.button_in_2.Location = new System.Drawing.Point(5, 56);
            this.button_in_2.Margin = new System.Windows.Forms.Padding(0);
            this.button_in_2.Name = "button_in_2";
            this.button_in_2.Size = new System.Drawing.Size(163, 38);
            this.button_in_2.TabIndex = 3;
            this.button_in_2.Text = "robot pause";
            this.button_in_2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button_in_2.UseVisualStyleBackColor = false;
            // 
            // button_in_1
            // 
            this.button_in_1.AutoSize = true;
            this.button_in_1.BackColor = System.Drawing.Color.White;
            this.button_in_1.FlatAppearance.BorderSize = 0;
            this.button_in_1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_in_1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_in_1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_in_1.ImageIndex = 0;
            this.button_in_1.ImageList = this.imageList1;
            this.button_in_1.Location = new System.Drawing.Point(5, 22);
            this.button_in_1.Margin = new System.Windows.Forms.Padding(0);
            this.button_in_1.Name = "button_in_1";
            this.button_in_1.Size = new System.Drawing.Size(163, 38);
            this.button_in_1.TabIndex = 3;
            this.button_in_1.Text = "robot ready";
            this.button_in_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button_in_1.UseVisualStyleBackColor = false;
            // 
            // listBox_receive
            // 
            this.listBox_receive.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listBox_receive.FormattingEnabled = true;
            this.listBox_receive.HorizontalScrollbar = true;
            this.listBox_receive.IntegralHeight = false;
            this.listBox_receive.ItemHeight = 21;
            this.listBox_receive.Location = new System.Drawing.Point(336, 250);
            this.listBox_receive.Name = "listBox_receive";
            this.listBox_receive.Size = new System.Drawing.Size(353, 134);
            this.listBox_receive.TabIndex = 12;
            // 
            // tabControl_send
            // 
            this.tabControl_send.Controls.Add(this.tabPage1);
            this.tabControl_send.Controls.Add(this.tabPage2);
            this.tabControl_send.Location = new System.Drawing.Point(8, 221);
            this.tabControl_send.Name = "tabControl_send";
            this.tabControl_send.SelectedIndex = 0;
            this.tabControl_send.Size = new System.Drawing.Size(253, 187);
            this.tabControl_send.TabIndex = 14;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.listBox_cmd);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(245, 161);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "命令发送";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // listBox_cmd
            // 
            this.listBox_cmd.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listBox_cmd.FormattingEnabled = true;
            this.listBox_cmd.ItemHeight = 21;
            this.listBox_cmd.Location = new System.Drawing.Point(6, 8);
            this.listBox_cmd.Name = "listBox_cmd";
            this.listBox_cmd.Size = new System.Drawing.Size(236, 151);
            this.listBox_cmd.TabIndex = 12;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.button_write_clear);
            this.tabPage2.Controls.Add(this.checkBox_hex);
            this.tabPage2.Controls.Add(this.textBox_cmd);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(245, 161);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "文本发送";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // button_write_clear
            // 
            this.button_write_clear.Location = new System.Drawing.Point(164, 137);
            this.button_write_clear.Name = "button_write_clear";
            this.button_write_clear.Size = new System.Drawing.Size(75, 23);
            this.button_write_clear.TabIndex = 15;
            this.button_write_clear.Text = "clear";
            this.button_write_clear.UseVisualStyleBackColor = true;
            this.button_write_clear.Click += new System.EventHandler(this.button_write_clear_Click);
            // 
            // checkBox_hex
            // 
            this.checkBox_hex.AutoSize = true;
            this.checkBox_hex.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBox_hex.Location = new System.Drawing.Point(6, 138);
            this.checkBox_hex.Name = "checkBox_hex";
            this.checkBox_hex.Size = new System.Drawing.Size(75, 21);
            this.checkBox_hex.TabIndex = 1;
            this.checkBox_hex.Text = "十六进制";
            this.checkBox_hex.UseVisualStyleBackColor = true;
            this.checkBox_hex.CheckedChanged += new System.EventHandler(this.checkBox_hex_CheckedChanged);
            // 
            // textBox_cmd
            // 
            this.textBox_cmd.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_cmd.Location = new System.Drawing.Point(7, 7);
            this.textBox_cmd.Multiline = true;
            this.textBox_cmd.Name = "textBox_cmd";
            this.textBox_cmd.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_cmd.Size = new System.Drawing.Size(235, 127);
            this.textBox_cmd.TabIndex = 0;
            this.textBox_cmd.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_cmd_KeyPress);
            // 
            // button_send
            // 
            this.button_send.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_send.Location = new System.Drawing.Point(261, 251);
            this.button_send.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_send.Name = "button_send";
            this.button_send.Size = new System.Drawing.Size(73, 43);
            this.button_send.TabIndex = 7;
            this.button_send.Text = "发送";
            this.button_send.UseVisualStyleBackColor = true;
            this.button_send.Click += new System.EventHandler(this.button_send_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.comboBox_EthOrCom);
            this.groupBox1.Controls.Add(this.button_close);
            this.groupBox1.Controls.Add(this.button_open);
            this.groupBox1.Controls.Add(this.textBox_info);
            this.groupBox1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(6, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(206, 219);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "通讯端口信息";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 20);
            this.label2.TabIndex = 16;
            this.label2.Text = "端口选择";
            // 
            // comboBox_EthOrCom
            // 
            this.comboBox_EthOrCom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_EthOrCom.FormattingEnabled = true;
            this.comboBox_EthOrCom.Location = new System.Drawing.Point(78, 18);
            this.comboBox_EthOrCom.Name = "comboBox_EthOrCom";
            this.comboBox_EthOrCom.Size = new System.Drawing.Size(121, 28);
            this.comboBox_EthOrCom.TabIndex = 16;
            this.comboBox_EthOrCom.SelectedIndexChanged += new System.EventHandler(this.SelectedIndexChanged);
            // 
            // button_close
            // 
            this.button_close.Location = new System.Drawing.Point(105, 179);
            this.button_close.Name = "button_close";
            this.button_close.Size = new System.Drawing.Size(94, 31);
            this.button_close.TabIndex = 10;
            this.button_close.Text = "Close";
            this.button_close.UseVisualStyleBackColor = true;
            this.button_close.Click += new System.EventHandler(this.button_close_Click);
            // 
            // button_open
            // 
            this.button_open.Location = new System.Drawing.Point(5, 179);
            this.button_open.Name = "button_open";
            this.button_open.Size = new System.Drawing.Size(94, 31);
            this.button_open.TabIndex = 10;
            this.button_open.Text = "Open";
            this.button_open.UseVisualStyleBackColor = true;
            this.button_open.Click += new System.EventHandler(this.button_open_Click);
            // 
            // textBox_info
            // 
            this.textBox_info.Location = new System.Drawing.Point(4, 52);
            this.textBox_info.Multiline = true;
            this.textBox_info.Name = "textBox_info";
            this.textBox_info.ReadOnly = true;
            this.textBox_info.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_info.Size = new System.Drawing.Size(196, 121);
            this.textBox_info.TabIndex = 9;
            this.textBox_info.WordWrap = false;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button_read_clear
            // 
            this.button_read_clear.Location = new System.Drawing.Point(357, 389);
            this.button_read_clear.Name = "button_read_clear";
            this.button_read_clear.Size = new System.Drawing.Size(75, 23);
            this.button_read_clear.TabIndex = 15;
            this.button_read_clear.Text = "clear";
            this.button_read_clear.UseVisualStyleBackColor = true;
            this.button_read_clear.Click += new System.EventHandler(this.button_read_clear_Click);
            // 
            // Form_Robot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(701, 414);
            this.Controls.Add(this.button_read_clear);
            this.Controls.Add(this.tabControl_send);
            this.Controls.Add(this.button_send);
            this.Controls.Add(this.listBox_receive);
            this.Controls.Add(this.groupBox_out);
            this.Controls.Add(this.groupBox_in);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_Robot";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "机器人";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form_Robot_FormClosed);
            this.Load += new System.EventHandler(this.Form_Robot_Load);
            this.groupBox_out.ResumeLayout(false);
            this.groupBox_out.PerformLayout();
            this.groupBox_in.ResumeLayout(false);
            this.groupBox_in.PerformLayout();
            this.tabControl_send.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox_out;
        private System.Windows.Forms.GroupBox groupBox_in;
        private System.Windows.Forms.ListBox listBox_receive;
        private System.Windows.Forms.TabControl tabControl_send;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ListBox listBox_cmd;
        private System.Windows.Forms.Button button_send;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox checkBox_hex;
        private System.Windows.Forms.GroupBox groupBox1;
        private AutoFrameUI.LampButton button_in_2;
        private AutoFrameUI.LampButton button_in_1;
        private AutoFrameUI.LampButton button_in_3;
        private System.Windows.Forms.ImageList imageList1;
        private AutoFrameUI.LampButton lampButton3;
        private AutoFrameUI.LampButton lampButton1;
        private AutoFrameUI.LampButton lampButton2;
        private AutoFrameUI.LampButton lampButton5;
        private AutoFrameUI.LampButton lampButton4;
        private AutoFrameUI.LampButton lampButton7;
        private AutoFrameUI.LampButton lampButton6;
        private System.Windows.Forms.TextBox textBox_info;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button_close;
        private System.Windows.Forms.Button button_open;
        private System.Windows.Forms.Button button_write_clear;
        private System.Windows.Forms.Button button_read_clear;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox_EthOrCom;
        public System.Windows.Forms.TextBox textBox_cmd;
    }
}