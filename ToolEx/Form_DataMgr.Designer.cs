namespace ToolEx
{
    partial class Form_DataMgr
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
            this.textBox_Version = new System.Windows.Forms.TextBox();
            this.button_Save = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button_Apply = new System.Windows.Forms.Button();
            this.comboBox_GroupName = new System.Windows.Forms.ComboBox();
            this.button_Del = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.button_MoveDown = new System.Windows.Forms.Button();
            this.button_MoveUp = new System.Windows.Forms.Button();
            this.tabControl_Data = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.comboBox_Level = new System.Windows.Forms.ComboBox();
            this.checkBox_PDCA = new System.Windows.Forms.CheckBox();
            this.dataGridView_Data = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.checkBox_ShowSave = new System.Windows.Forms.CheckBox();
            this.dataGridView_DataShow = new System.Windows.Forms.DataGridView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.comboBox_SaveType = new System.Windows.Forms.ComboBox();
            this.label_SaveType = new System.Windows.Forms.Label();
            this.checkBox_SaveData = new System.Windows.Forms.CheckBox();
            this.button_Browse = new System.Windows.Forms.Button();
            this.textBox_Port = new System.Windows.Forms.TextBox();
            this.label_Port = new System.Windows.Forms.Label();
            this.textBox_Password = new System.Windows.Forms.TextBox();
            this.label_Password = new System.Windows.Forms.Label();
            this.textBox_UserID = new System.Windows.Forms.TextBox();
            this.label_UserID = new System.Windows.Forms.Label();
            this.textBox_TableName = new System.Windows.Forms.TextBox();
            this.label_TableName = new System.Windows.Forms.Label();
            this.textBox_Database = new System.Windows.Forms.TextBox();
            this.label_Database = new System.Windows.Forms.Label();
            this.textBox_ServerIp = new System.Windows.Forms.TextBox();
            this.label_Server = new System.Windows.Forms.Label();
            this.textBox_SavePath = new System.Windows.Forms.TextBox();
            this.label_SavePath = new System.Windows.Forms.Label();
            this.dataGridView_DataSave = new System.Windows.Forms.DataGridView();
            this.button_DelGroup = new System.Windows.Forms.Button();
            this.button_SaveDefault = new System.Windows.Forms.Button();
            this.button_Restore = new System.Windows.Forms.Button();
            this.button_RecoverDefault = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_View = new System.Windows.Forms.Button();
            this.tabControl_Data.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Data)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_DataShow)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_DataSave)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox_Version
            // 
            this.textBox_Version.Location = new System.Drawing.Point(701, 10);
            this.textBox_Version.Margin = new System.Windows.Forms.Padding(8);
            this.textBox_Version.Name = "textBox_Version";
            this.textBox_Version.Size = new System.Drawing.Size(264, 35);
            this.textBox_Version.TabIndex = 9;
            // 
            // button_Save
            // 
            this.button_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Save.Location = new System.Drawing.Point(17, 353);
            this.button_Save.Margin = new System.Windows.Forms.Padding(8);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(139, 59);
            this.button_Save.TabIndex = 11;
            this.button_Save.Text = "保存";
            this.button_Save.UseVisualStyleBackColor = true;
            this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 15);
            this.label1.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 24);
            this.label1.TabIndex = 6;
            this.label1.Text = "数据组名称:";
            // 
            // button_Apply
            // 
            this.button_Apply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Apply.Location = new System.Drawing.Point(17, 284);
            this.button_Apply.Margin = new System.Windows.Forms.Padding(8);
            this.button_Apply.Name = "button_Apply";
            this.button_Apply.Size = new System.Drawing.Size(139, 59);
            this.button_Apply.TabIndex = 12;
            this.button_Apply.Text = "应用";
            this.button_Apply.UseVisualStyleBackColor = true;
            this.button_Apply.Click += new System.EventHandler(this.button_Apply_Click);
            // 
            // comboBox_GroupName
            // 
            this.comboBox_GroupName.FormattingEnabled = true;
            this.comboBox_GroupName.Location = new System.Drawing.Point(242, 11);
            this.comboBox_GroupName.Margin = new System.Windows.Forms.Padding(8);
            this.comboBox_GroupName.Name = "comboBox_GroupName";
            this.comboBox_GroupName.Size = new System.Drawing.Size(337, 32);
            this.comboBox_GroupName.TabIndex = 7;
            this.comboBox_GroupName.SelectedIndexChanged += new System.EventHandler(this.comboBox_GroupName_SelectedIndexChanged);
            // 
            // button_Del
            // 
            this.button_Del.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Del.Location = new System.Drawing.Point(17, 215);
            this.button_Del.Margin = new System.Windows.Forms.Padding(8);
            this.button_Del.Name = "button_Del";
            this.button_Del.Size = new System.Drawing.Size(139, 59);
            this.button_Del.TabIndex = 13;
            this.button_Del.Text = "移除";
            this.button_Del.UseVisualStyleBackColor = true;
            this.button_Del.Click += new System.EventHandler(this.button_Del_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(603, 15);
            this.label2.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 24);
            this.label2.TabIndex = 8;
            this.label2.Text = "版本:";
            // 
            // button_MoveDown
            // 
            this.button_MoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_MoveDown.Location = new System.Drawing.Point(17, 145);
            this.button_MoveDown.Margin = new System.Windows.Forms.Padding(8);
            this.button_MoveDown.Name = "button_MoveDown";
            this.button_MoveDown.Size = new System.Drawing.Size(139, 59);
            this.button_MoveDown.TabIndex = 14;
            this.button_MoveDown.Text = "下移";
            this.button_MoveDown.UseVisualStyleBackColor = true;
            this.button_MoveDown.Click += new System.EventHandler(this.button_MoveDown_Click);
            // 
            // button_MoveUp
            // 
            this.button_MoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_MoveUp.Location = new System.Drawing.Point(17, 77);
            this.button_MoveUp.Margin = new System.Windows.Forms.Padding(8);
            this.button_MoveUp.Name = "button_MoveUp";
            this.button_MoveUp.Size = new System.Drawing.Size(139, 59);
            this.button_MoveUp.TabIndex = 15;
            this.button_MoveUp.Text = "上移";
            this.button_MoveUp.UseVisualStyleBackColor = true;
            this.button_MoveUp.Click += new System.EventHandler(this.button_MoveUp_Click);
            // 
            // tabControl_Data
            // 
            this.tabControl_Data.Controls.Add(this.tabPage1);
            this.tabControl_Data.Controls.Add(this.tabPage2);
            this.tabControl_Data.Controls.Add(this.tabPage3);
            this.tabControl_Data.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_Data.Location = new System.Drawing.Point(8, 8);
            this.tabControl_Data.Margin = new System.Windows.Forms.Padding(8);
            this.tabControl_Data.Name = "tabControl_Data";
            this.tabControl_Data.SelectedIndex = 0;
            this.tabControl_Data.Size = new System.Drawing.Size(1382, 1034);
            this.tabControl_Data.TabIndex = 10;
            this.tabControl_Data.SelectedIndexChanged += new System.EventHandler(this.tabControl_Data_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.comboBox_Level);
            this.tabPage1.Controls.Add(this.checkBox_PDCA);
            this.tabPage1.Controls.Add(this.textBox_Version);
            this.tabPage1.Controls.Add(this.dataGridView_Data);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.comboBox_GroupName);
            this.tabPage1.Location = new System.Drawing.Point(4, 34);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(8);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(8);
            this.tabPage1.Size = new System.Drawing.Size(1374, 996);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "数据分类";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // comboBox_Level
            // 
            this.comboBox_Level.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Level.FormattingEnabled = true;
            this.comboBox_Level.Location = new System.Drawing.Point(1167, 11);
            this.comboBox_Level.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBox_Level.Name = "comboBox_Level";
            this.comboBox_Level.Size = new System.Drawing.Size(180, 32);
            this.comboBox_Level.TabIndex = 17;
            this.comboBox_Level.SelectedIndexChanged += new System.EventHandler(this.comboBox_Level_SelectedIndexChanged);
            // 
            // checkBox_PDCA
            // 
            this.checkBox_PDCA.AutoSize = true;
            this.checkBox_PDCA.Location = new System.Drawing.Point(994, 13);
            this.checkBox_PDCA.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBox_PDCA.Name = "checkBox_PDCA";
            this.checkBox_PDCA.Size = new System.Drawing.Size(132, 28);
            this.checkBox_PDCA.TabIndex = 16;
            this.checkBox_PDCA.Text = "是否PDCA";
            this.checkBox_PDCA.UseVisualStyleBackColor = true;
            this.checkBox_PDCA.CheckedChanged += new System.EventHandler(this.checkBox_PDCA_CheckedChanged);
            // 
            // dataGridView_Data
            // 
            this.dataGridView_Data.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView_Data.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Data.Location = new System.Drawing.Point(8, 69);
            this.dataGridView_Data.Margin = new System.Windows.Forms.Padding(8);
            this.dataGridView_Data.Name = "dataGridView_Data";
            this.dataGridView_Data.RowTemplate.Height = 23;
            this.dataGridView_Data.Size = new System.Drawing.Size(1363, 907);
            this.dataGridView_Data.TabIndex = 0;
            this.dataGridView_Data.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView_Data_ColumnWidthChanged);
            this.dataGridView_Data.CurrentCellChanged += new System.EventHandler(this.dataGridView_Data_CurrentCellChanged);
            this.dataGridView_Data.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dataGridView_Data_Scroll);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.checkBox_ShowSave);
            this.tabPage2.Controls.Add(this.dataGridView_DataShow);
            this.tabPage2.Location = new System.Drawing.Point(4, 34);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(1374, 996);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "数据显示";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // checkBox_ShowSave
            // 
            this.checkBox_ShowSave.AutoSize = true;
            this.checkBox_ShowSave.Checked = true;
            this.checkBox_ShowSave.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_ShowSave.Location = new System.Drawing.Point(10, 24);
            this.checkBox_ShowSave.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBox_ShowSave.Name = "checkBox_ShowSave";
            this.checkBox_ShowSave.Size = new System.Drawing.Size(132, 28);
            this.checkBox_ShowSave.TabIndex = 1;
            this.checkBox_ShowSave.Text = "是否保存";
            this.checkBox_ShowSave.UseVisualStyleBackColor = true;
            // 
            // dataGridView_DataShow
            // 
            this.dataGridView_DataShow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView_DataShow.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_DataShow.Location = new System.Drawing.Point(4, 75);
            this.dataGridView_DataShow.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dataGridView_DataShow.Name = "dataGridView_DataShow";
            this.dataGridView_DataShow.RowTemplate.Height = 23;
            this.dataGridView_DataShow.Size = new System.Drawing.Size(1214, 903);
            this.dataGridView_DataShow.TabIndex = 0;
            this.dataGridView_DataShow.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView_Data_ColumnWidthChanged);
            this.dataGridView_DataShow.CurrentCellChanged += new System.EventHandler(this.dataGridView_Data_CurrentCellChanged);
            this.dataGridView_DataShow.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dataGridView_Data_Scroll);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.comboBox_SaveType);
            this.tabPage3.Controls.Add(this.label_SaveType);
            this.tabPage3.Controls.Add(this.checkBox_SaveData);
            this.tabPage3.Controls.Add(this.button_View);
            this.tabPage3.Controls.Add(this.button_Browse);
            this.tabPage3.Controls.Add(this.textBox_Port);
            this.tabPage3.Controls.Add(this.label_Port);
            this.tabPage3.Controls.Add(this.textBox_Password);
            this.tabPage3.Controls.Add(this.label_Password);
            this.tabPage3.Controls.Add(this.textBox_UserID);
            this.tabPage3.Controls.Add(this.label_UserID);
            this.tabPage3.Controls.Add(this.textBox_TableName);
            this.tabPage3.Controls.Add(this.label_TableName);
            this.tabPage3.Controls.Add(this.textBox_Database);
            this.tabPage3.Controls.Add(this.label_Database);
            this.tabPage3.Controls.Add(this.textBox_ServerIp);
            this.tabPage3.Controls.Add(this.label_Server);
            this.tabPage3.Controls.Add(this.textBox_SavePath);
            this.tabPage3.Controls.Add(this.label_SavePath);
            this.tabPage3.Controls.Add(this.dataGridView_DataSave);
            this.tabPage3.Location = new System.Drawing.Point(4, 34);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1374, 996);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "数据保存";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // comboBox_SaveType
            // 
            this.comboBox_SaveType.FormattingEnabled = true;
            this.comboBox_SaveType.Location = new System.Drawing.Point(128, 21);
            this.comboBox_SaveType.Name = "comboBox_SaveType";
            this.comboBox_SaveType.Size = new System.Drawing.Size(149, 32);
            this.comboBox_SaveType.TabIndex = 6;
            this.comboBox_SaveType.SelectedIndexChanged += new System.EventHandler(this.comboBox_SaveType_SelectedIndexChanged);
            // 
            // label_SaveType
            // 
            this.label_SaveType.AutoSize = true;
            this.label_SaveType.Location = new System.Drawing.Point(3, 24);
            this.label_SaveType.Name = "label_SaveType";
            this.label_SaveType.Size = new System.Drawing.Size(118, 24);
            this.label_SaveType.TabIndex = 5;
            this.label_SaveType.Text = "保存类型:";
            // 
            // checkBox_SaveData
            // 
            this.checkBox_SaveData.AutoSize = true;
            this.checkBox_SaveData.Checked = true;
            this.checkBox_SaveData.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_SaveData.Location = new System.Drawing.Point(305, 22);
            this.checkBox_SaveData.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBox_SaveData.Name = "checkBox_SaveData";
            this.checkBox_SaveData.Size = new System.Drawing.Size(132, 28);
            this.checkBox_SaveData.TabIndex = 4;
            this.checkBox_SaveData.Text = "是否保存";
            this.checkBox_SaveData.UseVisualStyleBackColor = true;
            // 
            // button_Browse
            // 
            this.button_Browse.Location = new System.Drawing.Point(1103, 14);
            this.button_Browse.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_Browse.Name = "button_Browse";
            this.button_Browse.Size = new System.Drawing.Size(112, 43);
            this.button_Browse.TabIndex = 3;
            this.button_Browse.Text = "浏览";
            this.button_Browse.UseVisualStyleBackColor = true;
            this.button_Browse.Click += new System.EventHandler(this.button_Browse_Click);
            // 
            // textBox_Port
            // 
            this.textBox_Port.Location = new System.Drawing.Point(445, 62);
            this.textBox_Port.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox_Port.Name = "textBox_Port";
            this.textBox_Port.Size = new System.Drawing.Size(112, 35);
            this.textBox_Port.TabIndex = 2;
            // 
            // label_Port
            // 
            this.label_Port.AutoSize = true;
            this.label_Port.Location = new System.Drawing.Point(346, 67);
            this.label_Port.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_Port.Name = "label_Port";
            this.label_Port.Size = new System.Drawing.Size(94, 24);
            this.label_Port.TabIndex = 1;
            this.label_Port.Text = "端口号:";
            // 
            // textBox_Password
            // 
            this.textBox_Password.Location = new System.Drawing.Point(974, 62);
            this.textBox_Password.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox_Password.Name = "textBox_Password";
            this.textBox_Password.PasswordChar = '*';
            this.textBox_Password.Size = new System.Drawing.Size(164, 35);
            this.textBox_Password.TabIndex = 2;
            // 
            // label_Password
            // 
            this.label_Password.AutoSize = true;
            this.label_Password.Location = new System.Drawing.Point(854, 67);
            this.label_Password.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_Password.Name = "label_Password";
            this.label_Password.Size = new System.Drawing.Size(70, 24);
            this.label_Password.TabIndex = 1;
            this.label_Password.Text = "密码:";
            // 
            // textBox_UserID
            // 
            this.textBox_UserID.Location = new System.Drawing.Point(696, 62);
            this.textBox_UserID.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox_UserID.Name = "textBox_UserID";
            this.textBox_UserID.Size = new System.Drawing.Size(140, 35);
            this.textBox_UserID.TabIndex = 2;
            // 
            // label_UserID
            // 
            this.label_UserID.AutoSize = true;
            this.label_UserID.Location = new System.Drawing.Point(583, 67);
            this.label_UserID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_UserID.Name = "label_UserID";
            this.label_UserID.Size = new System.Drawing.Size(94, 24);
            this.label_UserID.TabIndex = 1;
            this.label_UserID.Text = "用户名:";
            // 
            // textBox_TableName
            // 
            this.textBox_TableName.Location = new System.Drawing.Point(477, 110);
            this.textBox_TableName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox_TableName.Name = "textBox_TableName";
            this.textBox_TableName.Size = new System.Drawing.Size(200, 35);
            this.textBox_TableName.TabIndex = 2;
            // 
            // label_TableName
            // 
            this.label_TableName.AutoSize = true;
            this.label_TableName.Location = new System.Drawing.Point(351, 115);
            this.label_TableName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_TableName.Name = "label_TableName";
            this.label_TableName.Size = new System.Drawing.Size(94, 24);
            this.label_TableName.TabIndex = 1;
            this.label_TableName.Text = "数据表:";
            // 
            // textBox_Database
            // 
            this.textBox_Database.Location = new System.Drawing.Point(128, 110);
            this.textBox_Database.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox_Database.Name = "textBox_Database";
            this.textBox_Database.Size = new System.Drawing.Size(200, 35);
            this.textBox_Database.TabIndex = 2;
            // 
            // label_Database
            // 
            this.label_Database.AutoSize = true;
            this.label_Database.Location = new System.Drawing.Point(2, 115);
            this.label_Database.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_Database.Name = "label_Database";
            this.label_Database.Size = new System.Drawing.Size(94, 24);
            this.label_Database.TabIndex = 1;
            this.label_Database.Text = "数据库:";
            // 
            // textBox_ServerIp
            // 
            this.textBox_ServerIp.Location = new System.Drawing.Point(128, 62);
            this.textBox_ServerIp.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox_ServerIp.Name = "textBox_ServerIp";
            this.textBox_ServerIp.Size = new System.Drawing.Size(200, 35);
            this.textBox_ServerIp.TabIndex = 2;
            // 
            // label_Server
            // 
            this.label_Server.AutoSize = true;
            this.label_Server.Location = new System.Drawing.Point(2, 67);
            this.label_Server.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_Server.Name = "label_Server";
            this.label_Server.Size = new System.Drawing.Size(118, 24);
            this.label_Server.TabIndex = 1;
            this.label_Server.Text = "服务地址:";
            // 
            // textBox_SavePath
            // 
            this.textBox_SavePath.Location = new System.Drawing.Point(710, 19);
            this.textBox_SavePath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox_SavePath.Name = "textBox_SavePath";
            this.textBox_SavePath.Size = new System.Drawing.Size(380, 35);
            this.textBox_SavePath.TabIndex = 2;
            // 
            // label_SavePath
            // 
            this.label_SavePath.AutoSize = true;
            this.label_SavePath.Location = new System.Drawing.Point(584, 24);
            this.label_SavePath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_SavePath.Name = "label_SavePath";
            this.label_SavePath.Size = new System.Drawing.Size(118, 24);
            this.label_SavePath.TabIndex = 1;
            this.label_SavePath.Text = "保存路径:";
            // 
            // dataGridView_DataSave
            // 
            this.dataGridView_DataSave.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView_DataSave.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_DataSave.Location = new System.Drawing.Point(4, 157);
            this.dataGridView_DataSave.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dataGridView_DataSave.Name = "dataGridView_DataSave";
            this.dataGridView_DataSave.RowTemplate.Height = 23;
            this.dataGridView_DataSave.Size = new System.Drawing.Size(1214, 824);
            this.dataGridView_DataSave.TabIndex = 0;
            this.dataGridView_DataSave.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView_Data_ColumnWidthChanged);
            this.dataGridView_DataSave.CurrentCellChanged += new System.EventHandler(this.dataGridView_Data_CurrentCellChanged);
            this.dataGridView_DataSave.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dataGridView_Data_Scroll);
            // 
            // button_DelGroup
            // 
            this.button_DelGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_DelGroup.Location = new System.Drawing.Point(17, 8);
            this.button_DelGroup.Margin = new System.Windows.Forms.Padding(8);
            this.button_DelGroup.Name = "button_DelGroup";
            this.button_DelGroup.Size = new System.Drawing.Size(139, 59);
            this.button_DelGroup.TabIndex = 15;
            this.button_DelGroup.Text = "删除组";
            this.button_DelGroup.UseVisualStyleBackColor = true;
            this.button_DelGroup.Click += new System.EventHandler(this.button_DelGroup_Click);
            // 
            // button_SaveDefault
            // 
            this.button_SaveDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_SaveDefault.Location = new System.Drawing.Point(17, 422);
            this.button_SaveDefault.Margin = new System.Windows.Forms.Padding(8);
            this.button_SaveDefault.Name = "button_SaveDefault";
            this.button_SaveDefault.Size = new System.Drawing.Size(139, 59);
            this.button_SaveDefault.TabIndex = 11;
            this.button_SaveDefault.Text = "保存默认";
            this.button_SaveDefault.UseVisualStyleBackColor = true;
            this.button_SaveDefault.Click += new System.EventHandler(this.button_SaveDefault_Click);
            // 
            // button_Restore
            // 
            this.button_Restore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Restore.Location = new System.Drawing.Point(17, 491);
            this.button_Restore.Margin = new System.Windows.Forms.Padding(8);
            this.button_Restore.Name = "button_Restore";
            this.button_Restore.Size = new System.Drawing.Size(139, 59);
            this.button_Restore.TabIndex = 11;
            this.button_Restore.Text = "还原";
            this.button_Restore.UseVisualStyleBackColor = true;
            this.button_Restore.Click += new System.EventHandler(this.button_Restore_Click);
            // 
            // button_RecoverDefault
            // 
            this.button_RecoverDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_RecoverDefault.Location = new System.Drawing.Point(17, 560);
            this.button_RecoverDefault.Margin = new System.Windows.Forms.Padding(8);
            this.button_RecoverDefault.Name = "button_RecoverDefault";
            this.button_RecoverDefault.Size = new System.Drawing.Size(139, 59);
            this.button_RecoverDefault.TabIndex = 11;
            this.button_RecoverDefault.Text = "恢复默认";
            this.button_RecoverDefault.UseVisualStyleBackColor = true;
            this.button_RecoverDefault.Click += new System.EventHandler(this.button_RecoverDefault_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tableLayoutPanel1.Controls.Add(this.tabControl_Data, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1578, 1050);
            this.tableLayoutPanel1.TabIndex = 16;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button_DelGroup);
            this.panel1.Controls.Add(this.button_RecoverDefault);
            this.panel1.Controls.Add(this.button_MoveUp);
            this.panel1.Controls.Add(this.button_Restore);
            this.panel1.Controls.Add(this.button_MoveDown);
            this.panel1.Controls.Add(this.button_SaveDefault);
            this.panel1.Controls.Add(this.button_Del);
            this.panel1.Controls.Add(this.button_Save);
            this.panel1.Controls.Add(this.button_Apply);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(1401, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(174, 1044);
            this.panel1.TabIndex = 11;
            // 
            // button_View
            // 
            this.button_View.Location = new System.Drawing.Point(710, 106);
            this.button_View.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_View.Name = "button_View";
            this.button_View.Size = new System.Drawing.Size(112, 43);
            this.button_View.TabIndex = 3;
            this.button_View.Text = "查看";
            this.button_View.UseVisualStyleBackColor = true;
            this.button_View.Click += new System.EventHandler(this.button_View_Click);
            // 
            // Form_DataMgr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1578, 1050);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(8);
            this.Name = "Form_DataMgr";
            this.Text = "数据管理";
            this.Load += new System.EventHandler(this.Form_DataMgr_Load);
            this.tabControl_Data.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Data)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_DataShow)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_DataSave)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_Version;
        private System.Windows.Forms.Button button_Save;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_Apply;
        private System.Windows.Forms.ComboBox comboBox_GroupName;
        private System.Windows.Forms.Button button_Del;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_MoveDown;
        private System.Windows.Forms.Button button_MoveUp;
        private System.Windows.Forms.TabControl tabControl_Data;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dataGridView_Data;
        private System.Windows.Forms.Button button_DelGroup;
        private System.Windows.Forms.CheckBox checkBox_PDCA;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DataGridView dataGridView_DataShow;
        private System.Windows.Forms.DataGridView dataGridView_DataSave;
        private System.Windows.Forms.Button button_Browse;
        private System.Windows.Forms.TextBox textBox_SavePath;
        private System.Windows.Forms.Label label_SavePath;
        private System.Windows.Forms.CheckBox checkBox_ShowSave;
        private System.Windows.Forms.CheckBox checkBox_SaveData;
        private System.Windows.Forms.ComboBox comboBox_Level;
        private System.Windows.Forms.Button button_SaveDefault;
        private System.Windows.Forms.Button button_Restore;
        private System.Windows.Forms.Button button_RecoverDefault;
        private System.Windows.Forms.ComboBox comboBox_SaveType;
        private System.Windows.Forms.Label label_SaveType;
        private System.Windows.Forms.TextBox textBox_Port;
        private System.Windows.Forms.Label label_Port;
        private System.Windows.Forms.TextBox textBox_ServerIp;
        private System.Windows.Forms.Label label_Server;
        private System.Windows.Forms.TextBox textBox_Password;
        private System.Windows.Forms.Label label_Password;
        private System.Windows.Forms.TextBox textBox_UserID;
        private System.Windows.Forms.Label label_UserID;
        private System.Windows.Forms.TextBox textBox_TableName;
        private System.Windows.Forms.Label label_TableName;
        private System.Windows.Forms.TextBox textBox_Database;
        private System.Windows.Forms.Label label_Database;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button_View;
    }
}