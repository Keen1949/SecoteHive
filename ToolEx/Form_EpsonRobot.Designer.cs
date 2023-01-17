namespace ToolEx
{
    partial class Form_EpsonRobot
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_EpsonRobot));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.button_W_P = new System.Windows.Forms.Button();
            this.button_V_P = new System.Windows.Forms.Button();
            this.button_U_P = new System.Windows.Forms.Button();
            this.button_Z_P = new System.Windows.Forms.Button();
            this.button_Y_P = new System.Windows.Forms.Button();
            this.button_X_N = new System.Windows.Forms.Button();
            this.button_W_N = new System.Windows.Forms.Button();
            this.button_V_N = new System.Windows.Forms.Button();
            this.button_U_N = new System.Windows.Forms.Button();
            this.button_Z_N = new System.Windows.Forms.Button();
            this.button_X_P = new System.Windows.Forms.Button();
            this.button_Y_N = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_ProjectPath = new System.Windows.Forms.TextBox();
            this.button_ProjectPath = new System.Windows.Forms.Button();
            this.tabPage1.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
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
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox7);
            this.tabPage1.Controls.Add(this.groupBox6);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(733, 499);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "手动调试界面";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.button_W_P);
            this.groupBox7.Controls.Add(this.button_V_P);
            this.groupBox7.Controls.Add(this.button_U_P);
            this.groupBox7.Controls.Add(this.button_Z_P);
            this.groupBox7.Controls.Add(this.button_Y_P);
            this.groupBox7.Controls.Add(this.button_X_N);
            this.groupBox7.Controls.Add(this.button_W_N);
            this.groupBox7.Controls.Add(this.button_V_N);
            this.groupBox7.Controls.Add(this.button_U_N);
            this.groupBox7.Controls.Add(this.button_Z_N);
            this.groupBox7.Controls.Add(this.button_X_P);
            this.groupBox7.Controls.Add(this.button_Y_N);
            this.groupBox7.Location = new System.Drawing.Point(7, 141);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(374, 234);
            this.groupBox7.TabIndex = 1;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "步进";
            // 
            // button_W_P
            // 
            this.button_W_P.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_W_P.ImageIndex = 7;
            this.button_W_P.ImageList = this.imageList1;
            this.button_W_P.Location = new System.Drawing.Point(167, 176);
            this.button_W_P.Name = "button_W_P";
            this.button_W_P.Size = new System.Drawing.Size(48, 46);
            this.button_W_P.TabIndex = 0;
            this.button_W_P.Text = "W+";
            this.button_W_P.UseVisualStyleBackColor = true;
            // 
            // button_V_P
            // 
            this.button_V_P.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_V_P.ImageIndex = 7;
            this.button_V_P.ImageList = this.imageList1;
            this.button_V_P.Location = new System.Drawing.Point(86, 176);
            this.button_V_P.Name = "button_V_P";
            this.button_V_P.Size = new System.Drawing.Size(48, 46);
            this.button_V_P.TabIndex = 0;
            this.button_V_P.Text = "V+";
            this.button_V_P.UseVisualStyleBackColor = true;
            // 
            // button_U_P
            // 
            this.button_U_P.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_U_P.ImageIndex = 7;
            this.button_U_P.ImageList = this.imageList1;
            this.button_U_P.Location = new System.Drawing.Point(5, 176);
            this.button_U_P.Name = "button_U_P";
            this.button_U_P.Size = new System.Drawing.Size(48, 46);
            this.button_U_P.TabIndex = 0;
            this.button_U_P.Text = "U+";
            this.button_U_P.UseVisualStyleBackColor = true;
            // 
            // button_Z_P
            // 
            this.button_Z_P.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Z_P.ImageIndex = 5;
            this.button_Z_P.Location = new System.Drawing.Point(167, 71);
            this.button_Z_P.Name = "button_Z_P";
            this.button_Z_P.Size = new System.Drawing.Size(48, 46);
            this.button_Z_P.TabIndex = 0;
            this.button_Z_P.Text = "Z+";
            this.button_Z_P.UseVisualStyleBackColor = true;
            // 
            // button_Y_P
            // 
            this.button_Y_P.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Y_P.ImageIndex = 5;
            this.button_Y_P.ImageList = this.imageList1;
            this.button_Y_P.Location = new System.Drawing.Point(59, 71);
            this.button_Y_P.Name = "button_Y_P";
            this.button_Y_P.Size = new System.Drawing.Size(48, 46);
            this.button_Y_P.TabIndex = 0;
            this.button_Y_P.Text = "Y+";
            this.button_Y_P.UseVisualStyleBackColor = true;
            // 
            // button_X_N
            // 
            this.button_X_N.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_X_N.ImageIndex = 3;
            this.button_X_N.ImageList = this.imageList1;
            this.button_X_N.Location = new System.Drawing.Point(113, 48);
            this.button_X_N.Name = "button_X_N";
            this.button_X_N.Size = new System.Drawing.Size(48, 46);
            this.button_X_N.TabIndex = 0;
            this.button_X_N.Text = "X-";
            this.button_X_N.UseVisualStyleBackColor = true;
            // 
            // button_W_N
            // 
            this.button_W_N.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_W_N.ImageIndex = 6;
            this.button_W_N.ImageList = this.imageList1;
            this.button_W_N.Location = new System.Drawing.Point(167, 125);
            this.button_W_N.Name = "button_W_N";
            this.button_W_N.Size = new System.Drawing.Size(48, 46);
            this.button_W_N.TabIndex = 0;
            this.button_W_N.Text = "W-";
            this.button_W_N.UseVisualStyleBackColor = true;
            // 
            // button_V_N
            // 
            this.button_V_N.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_V_N.ImageIndex = 6;
            this.button_V_N.ImageList = this.imageList1;
            this.button_V_N.Location = new System.Drawing.Point(86, 125);
            this.button_V_N.Name = "button_V_N";
            this.button_V_N.Size = new System.Drawing.Size(48, 46);
            this.button_V_N.TabIndex = 0;
            this.button_V_N.Text = "V-";
            this.button_V_N.UseVisualStyleBackColor = true;
            // 
            // button_U_N
            // 
            this.button_U_N.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_U_N.ImageIndex = 6;
            this.button_U_N.ImageList = this.imageList1;
            this.button_U_N.Location = new System.Drawing.Point(5, 125);
            this.button_U_N.Name = "button_U_N";
            this.button_U_N.Size = new System.Drawing.Size(48, 46);
            this.button_U_N.TabIndex = 0;
            this.button_U_N.Text = "U-";
            this.button_U_N.UseVisualStyleBackColor = true;
            // 
            // button_Z_N
            // 
            this.button_Z_N.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Z_N.ImageIndex = 4;
            this.button_Z_N.ImageList = this.imageList1;
            this.button_Z_N.Location = new System.Drawing.Point(167, 20);
            this.button_Z_N.Name = "button_Z_N";
            this.button_Z_N.Size = new System.Drawing.Size(48, 46);
            this.button_Z_N.TabIndex = 0;
            this.button_Z_N.Text = "Z-";
            this.button_Z_N.UseVisualStyleBackColor = true;
            // 
            // button_X_P
            // 
            this.button_X_P.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_X_P.ImageIndex = 2;
            this.button_X_P.ImageList = this.imageList1;
            this.button_X_P.Location = new System.Drawing.Point(5, 48);
            this.button_X_P.Name = "button_X_P";
            this.button_X_P.Size = new System.Drawing.Size(48, 46);
            this.button_X_P.TabIndex = 0;
            this.button_X_P.Text = "X+";
            this.button_X_P.UseVisualStyleBackColor = true;
            // 
            // button_Y_N
            // 
            this.button_Y_N.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Y_N.ImageIndex = 4;
            this.button_Y_N.ImageList = this.imageList1;
            this.button_Y_N.Location = new System.Drawing.Point(59, 20);
            this.button_Y_N.Name = "button_Y_N";
            this.button_Y_N.Size = new System.Drawing.Size(48, 46);
            this.button_Y_N.TabIndex = 0;
            this.button_Y_N.Text = "Y-";
            this.button_Y_N.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.radioButton3);
            this.groupBox6.Controls.Add(this.radioButton2);
            this.groupBox6.Controls.Add(this.radioButton1);
            this.groupBox6.Controls.Add(this.textBox_W);
            this.groupBox6.Controls.Add(this.textBox_Z);
            this.groupBox6.Controls.Add(this.textBox_V);
            this.groupBox6.Controls.Add(this.textBox_Y);
            this.groupBox6.Controls.Add(this.textBox_U);
            this.groupBox6.Controls.Add(this.textBox_X);
            this.groupBox6.Controls.Add(this.label_W);
            this.groupBox6.Controls.Add(this.label_Z);
            this.groupBox6.Controls.Add(this.label_V);
            this.groupBox6.Controls.Add(this.label_Y);
            this.groupBox6.Controls.Add(this.label_U);
            this.groupBox6.Controls.Add(this.label_X);
            this.groupBox6.Location = new System.Drawing.Point(7, 7);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(374, 127);
            this.groupBox6.TabIndex = 0;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "当前位置";
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(305, 87);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(47, 16);
            this.radioButton3.TabIndex = 4;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "脉冲";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(305, 54);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(47, 16);
            this.radioButton2.TabIndex = 3;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "关节";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(305, 21);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(47, 16);
            this.radioButton1.TabIndex = 2;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "世界";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // textBox_W
            // 
            this.textBox_W.Location = new System.Drawing.Point(207, 86);
            this.textBox_W.Name = "textBox_W";
            this.textBox_W.Size = new System.Drawing.Size(82, 21);
            this.textBox_W.TabIndex = 1;
            // 
            // textBox_Z
            // 
            this.textBox_Z.Location = new System.Drawing.Point(207, 37);
            this.textBox_Z.Name = "textBox_Z";
            this.textBox_Z.Size = new System.Drawing.Size(82, 21);
            this.textBox_Z.TabIndex = 1;
            // 
            // textBox_V
            // 
            this.textBox_V.Location = new System.Drawing.Point(107, 86);
            this.textBox_V.Name = "textBox_V";
            this.textBox_V.Size = new System.Drawing.Size(82, 21);
            this.textBox_V.TabIndex = 1;
            // 
            // textBox_Y
            // 
            this.textBox_Y.Location = new System.Drawing.Point(107, 37);
            this.textBox_Y.Name = "textBox_Y";
            this.textBox_Y.Size = new System.Drawing.Size(82, 21);
            this.textBox_Y.TabIndex = 1;
            // 
            // textBox_U
            // 
            this.textBox_U.Location = new System.Drawing.Point(7, 86);
            this.textBox_U.Name = "textBox_U";
            this.textBox_U.Size = new System.Drawing.Size(82, 21);
            this.textBox_U.TabIndex = 1;
            // 
            // textBox_X
            // 
            this.textBox_X.Location = new System.Drawing.Point(7, 37);
            this.textBox_X.Name = "textBox_X";
            this.textBox_X.Size = new System.Drawing.Size(82, 21);
            this.textBox_X.TabIndex = 1;
            // 
            // label_W
            // 
            this.label_W.AutoSize = true;
            this.label_W.Location = new System.Drawing.Point(233, 70);
            this.label_W.Name = "label_W";
            this.label_W.Size = new System.Drawing.Size(41, 12);
            this.label_W.TabIndex = 0;
            this.label_W.Text = "W(deg)";
            // 
            // label_Z
            // 
            this.label_Z.AutoSize = true;
            this.label_Z.Location = new System.Drawing.Point(233, 21);
            this.label_Z.Name = "label_Z";
            this.label_Z.Size = new System.Drawing.Size(35, 12);
            this.label_Z.TabIndex = 0;
            this.label_Z.Text = "Z(mm)";
            // 
            // label_V
            // 
            this.label_V.AutoSize = true;
            this.label_V.Location = new System.Drawing.Point(133, 70);
            this.label_V.Name = "label_V";
            this.label_V.Size = new System.Drawing.Size(41, 12);
            this.label_V.TabIndex = 0;
            this.label_V.Text = "V(deg)";
            // 
            // label_Y
            // 
            this.label_Y.AutoSize = true;
            this.label_Y.Location = new System.Drawing.Point(133, 21);
            this.label_Y.Name = "label_Y";
            this.label_Y.Size = new System.Drawing.Size(35, 12);
            this.label_Y.TabIndex = 0;
            this.label_Y.Text = "Y(mm)";
            // 
            // label_U
            // 
            this.label_U.AutoSize = true;
            this.label_U.Location = new System.Drawing.Point(33, 70);
            this.label_U.Name = "label_U";
            this.label_U.Size = new System.Drawing.Size(41, 12);
            this.label_U.TabIndex = 0;
            this.label_U.Text = "U(deg)";
            // 
            // label_X
            // 
            this.label_X.AutoSize = true;
            this.label_X.Location = new System.Drawing.Point(33, 21);
            this.label_X.Name = "label_X";
            this.label_X.Size = new System.Drawing.Size(35, 12);
            this.label_X.TabIndex = 0;
            this.label_X.Text = "X(mm)";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(13, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(741, 525);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.button_ProjectPath);
            this.tabPage2.Controls.Add(this.textBox_ProjectPath);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.tableLayoutPanel1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(733, 499);
            this.tabPage2.TabIndex = 3;
            this.tabPage2.Text = "手动管理界面";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(733, 448);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 464);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "工程文件:";
            // 
            // textBox_ProjectPath
            // 
            this.textBox_ProjectPath.Location = new System.Drawing.Point(65, 460);
            this.textBox_ProjectPath.Name = "textBox_ProjectPath";
            this.textBox_ProjectPath.Size = new System.Drawing.Size(343, 21);
            this.textBox_ProjectPath.TabIndex = 2;
            // 
            // button_ProjectPath
            // 
            this.button_ProjectPath.Location = new System.Drawing.Point(413, 459);
            this.button_ProjectPath.Name = "button_ProjectPath";
            this.button_ProjectPath.Size = new System.Drawing.Size(53, 23);
            this.button_ProjectPath.TabIndex = 3;
            this.button_ProjectPath.Text = "浏览";
            this.button_ProjectPath.UseVisualStyleBackColor = true;
            // 
            // Form_EpsonRobot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(774, 549);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form_EpsonRobot";
            this.Text = "Form_EpsonRobot";
            this.Load += new System.EventHandler(this.Form_EpsonRobot_Load);
            this.tabPage1.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Button button_W_P;
        private System.Windows.Forms.Button button_V_P;
        private System.Windows.Forms.Button button_U_P;
        private System.Windows.Forms.Button button_Z_P;
        private System.Windows.Forms.Button button_Y_P;
        private System.Windows.Forms.Button button_X_N;
        private System.Windows.Forms.Button button_W_N;
        private System.Windows.Forms.Button button_V_N;
        private System.Windows.Forms.Button button_U_N;
        private System.Windows.Forms.Button button_Z_N;
        private System.Windows.Forms.Button button_X_P;
        private System.Windows.Forms.Button button_Y_N;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
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
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button button_ProjectPath;
        private System.Windows.Forms.TextBox textBox_ProjectPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}