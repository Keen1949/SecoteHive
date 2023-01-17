namespace AutoFrameVision
{
    partial class Form_Vision
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
            this.roundPanel1 = new AutoFrameUI.RoundPanel(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.visionControl7 = new AutoFrameVision.VisionControl();
            this.visionControl8 = new AutoFrameVision.VisionControl();
            this.visionControl9 = new AutoFrameVision.VisionControl();
            this.visionControl1 = new AutoFrameVision.VisionControl();
            this.visionControl2 = new AutoFrameVision.VisionControl();
            this.visionControl3 = new AutoFrameVision.VisionControl();
            this.visionControl4 = new AutoFrameVision.VisionControl();
            this.visionControl5 = new AutoFrameVision.VisionControl();
            this.visionControl6 = new AutoFrameVision.VisionControl();
            this.listbox_log = new ToolEx.ListBoxEx();
            this.roundPanel_button = new AutoFrameUI.RoundPanel(this.components);
            this.button_Config = new System.Windows.Forms.Button();
            this.button_Step = new System.Windows.Forms.Button();
            this.comboBox_Step = new System.Windows.Forms.ComboBox();
            this.button_debug = new System.Windows.Forms.Button();
            this.button_delete = new System.Windows.Forms.Button();
            this.button_cali = new System.Windows.Forms.Button();
            this.button_clear = new System.Windows.Forms.Button();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.roundPanel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.visionControl7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.visionControl8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.visionControl9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.visionControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.visionControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.visionControl3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.visionControl4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.visionControl5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.visionControl6)).BeginInit();
            this.roundPanel_button.SuspendLayout();
            this.SuspendLayout();
            // 
            // roundPanel1
            // 
            this.roundPanel1._setRoundRadius = 12;
            this.roundPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(235)))));
            this.roundPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.roundPanel1.Controls.Add(this.tableLayoutPanel1);
            this.roundPanel1.Controls.Add(this.listbox_log);
            this.roundPanel1.Controls.Add(this.roundPanel_button);
            this.roundPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.roundPanel1.Location = new System.Drawing.Point(0, 0);
            this.roundPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.roundPanel1.Name = "roundPanel1";
            this.roundPanel1.Size = new System.Drawing.Size(1001, 628);
            this.roundPanel1.TabIndex = 12;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.visionControl7, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.visionControl8, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.visionControl9, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.visionControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.visionControl2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.visionControl3, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.visionControl4, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.visionControl5, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.visionControl6, 2, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(740, 622);
            this.tableLayoutPanel1.TabIndex = 9;
            // 
            // visionControl7
            // 
            this.visionControl7.BackColor = System.Drawing.Color.MidnightBlue;
            this.visionControl7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.visionControl7.Location = new System.Drawing.Point(3, 417);
            this.visionControl7.Name = "visionControl7";
            this.visionControl7.Size = new System.Drawing.Size(240, 202);
            this.visionControl7.TabIndex = 11;
            this.visionControl7.TabStop = false;
            // 
            // visionControl8
            // 
            this.visionControl8.BackColor = System.Drawing.Color.MidnightBlue;
            this.visionControl8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.visionControl8.Location = new System.Drawing.Point(249, 417);
            this.visionControl8.Name = "visionControl8";
            this.visionControl8.Size = new System.Drawing.Size(240, 202);
            this.visionControl8.TabIndex = 10;
            this.visionControl8.TabStop = false;
            // 
            // visionControl9
            // 
            this.visionControl9.BackColor = System.Drawing.Color.MidnightBlue;
            this.visionControl9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.visionControl9.Location = new System.Drawing.Point(495, 417);
            this.visionControl9.Name = "visionControl9";
            this.visionControl9.Size = new System.Drawing.Size(242, 202);
            this.visionControl9.TabIndex = 7;
            this.visionControl9.TabStop = false;
            // 
            // visionControl1
            // 
            this.visionControl1.BackColor = System.Drawing.Color.MidnightBlue;
            this.visionControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.visionControl1.Location = new System.Drawing.Point(3, 3);
            this.visionControl1.Name = "visionControl1";
            this.visionControl1.Size = new System.Drawing.Size(240, 201);
            this.visionControl1.TabIndex = 6;
            this.visionControl1.TabStop = false;
            // 
            // visionControl2
            // 
            this.visionControl2.BackColor = System.Drawing.Color.MidnightBlue;
            this.visionControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.visionControl2.Location = new System.Drawing.Point(249, 3);
            this.visionControl2.Name = "visionControl2";
            this.visionControl2.Size = new System.Drawing.Size(240, 201);
            this.visionControl2.TabIndex = 6;
            this.visionControl2.TabStop = false;
            // 
            // visionControl3
            // 
            this.visionControl3.BackColor = System.Drawing.Color.MidnightBlue;
            this.visionControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.visionControl3.Location = new System.Drawing.Point(495, 3);
            this.visionControl3.Name = "visionControl3";
            this.visionControl3.Size = new System.Drawing.Size(242, 201);
            this.visionControl3.TabIndex = 6;
            this.visionControl3.TabStop = false;
            // 
            // visionControl4
            // 
            this.visionControl4.BackColor = System.Drawing.Color.MidnightBlue;
            this.visionControl4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.visionControl4.Location = new System.Drawing.Point(3, 210);
            this.visionControl4.Name = "visionControl4";
            this.visionControl4.Size = new System.Drawing.Size(240, 201);
            this.visionControl4.TabIndex = 6;
            this.visionControl4.TabStop = false;
            // 
            // visionControl5
            // 
            this.visionControl5.BackColor = System.Drawing.Color.MidnightBlue;
            this.visionControl5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.visionControl5.Location = new System.Drawing.Point(249, 210);
            this.visionControl5.Name = "visionControl5";
            this.visionControl5.Size = new System.Drawing.Size(240, 201);
            this.visionControl5.TabIndex = 6;
            this.visionControl5.TabStop = false;
            // 
            // visionControl6
            // 
            this.visionControl6.BackColor = System.Drawing.Color.MidnightBlue;
            this.visionControl6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.visionControl6.Location = new System.Drawing.Point(495, 210);
            this.visionControl6.Name = "visionControl6";
            this.visionControl6.Size = new System.Drawing.Size(242, 201);
            this.visionControl6.TabIndex = 12;
            this.visionControl6.TabStop = false;
            // 
            // listbox_log
            // 
            this.listbox_log.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listbox_log.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.listbox_log.HorizontalScrollbar = true;
            this.listbox_log.IntegralHeight = false;
            this.listbox_log.ItemHeight = 21;
            this.listbox_log.Location = new System.Drawing.Point(750, 6);
            this.listbox_log.Name = "listbox_log";
            this.listbox_log.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listbox_log.Size = new System.Drawing.Size(241, 424);
            this.listbox_log.TabIndex = 8;
            // 
            // roundPanel_button
            // 
            this.roundPanel_button._setRoundRadius = 8;
            this.roundPanel_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.roundPanel_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.roundPanel_button.Controls.Add(this.button_Config);
            this.roundPanel_button.Controls.Add(this.button_Step);
            this.roundPanel_button.Controls.Add(this.comboBox_Step);
            this.roundPanel_button.Controls.Add(this.button_debug);
            this.roundPanel_button.Controls.Add(this.button_delete);
            this.roundPanel_button.Controls.Add(this.button_cali);
            this.roundPanel_button.Controls.Add(this.button_clear);
            this.roundPanel_button.Location = new System.Drawing.Point(747, 433);
            this.roundPanel_button.Margin = new System.Windows.Forms.Padding(0);
            this.roundPanel_button.Name = "roundPanel_button";
            this.roundPanel_button.Size = new System.Drawing.Size(243, 183);
            this.roundPanel_button.TabIndex = 7;
            // 
            // button_Config
            // 
            this.button_Config.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Config.Location = new System.Drawing.Point(130, 40);
            this.button_Config.Name = "button_Config";
            this.button_Config.Size = new System.Drawing.Size(101, 44);
            this.button_Config.TabIndex = 7;
            this.button_Config.Text = "参数设置";
            this.button_Config.UseVisualStyleBackColor = true;
            this.button_Config.Click += new System.EventHandler(this.button_Config_Click);
            // 
            // button_Step
            // 
            this.button_Step.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Step.Location = new System.Drawing.Point(10, 40);
            this.button_Step.Name = "button_Step";
            this.button_Step.Size = new System.Drawing.Size(101, 44);
            this.button_Step.TabIndex = 7;
            this.button_Step.Text = "拍照处理";
            this.button_Step.UseVisualStyleBackColor = true;
            this.button_Step.Click += new System.EventHandler(this.button_Step_Click);
            // 
            // comboBox_Step
            // 
            this.comboBox_Step.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Step.FormattingEnabled = true;
            this.comboBox_Step.Location = new System.Drawing.Point(3, 7);
            this.comboBox_Step.Name = "comboBox_Step";
            this.comboBox_Step.Size = new System.Drawing.Size(235, 28);
            this.comboBox_Step.TabIndex = 6;
            this.comboBox_Step.SelectedIndexChanged += new System.EventHandler(this.comboBox_Step_SelectedIndexChanged);
            // 
            // button_debug
            // 
            this.button_debug.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_debug.Location = new System.Drawing.Point(130, 134);
            this.button_debug.Name = "button_debug";
            this.button_debug.Size = new System.Drawing.Size(101, 44);
            this.button_debug.TabIndex = 5;
            this.button_debug.Text = "调试";
            this.button_debug.UseVisualStyleBackColor = true;
            this.button_debug.Click += new System.EventHandler(this.button_debug_Click);
            // 
            // button_delete
            // 
            this.button_delete.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_delete.Location = new System.Drawing.Point(10, 86);
            this.button_delete.Name = "button_delete";
            this.button_delete.Size = new System.Drawing.Size(101, 44);
            this.button_delete.TabIndex = 4;
            this.button_delete.Text = "Delete";
            this.button_delete.UseVisualStyleBackColor = true;
            this.button_delete.Click += new System.EventHandler(this.button_delete_Click);
            // 
            // button_cali
            // 
            this.button_cali.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_cali.Location = new System.Drawing.Point(10, 134);
            this.button_cali.Name = "button_cali";
            this.button_cali.Size = new System.Drawing.Size(101, 44);
            this.button_cali.TabIndex = 4;
            this.button_cali.Text = "Cali";
            this.button_cali.UseVisualStyleBackColor = true;
            this.button_cali.Click += new System.EventHandler(this.button_cali_Click);
            // 
            // button_clear
            // 
            this.button_clear.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_clear.Location = new System.Drawing.Point(130, 86);
            this.button_clear.Name = "button_clear";
            this.button_clear.Size = new System.Drawing.Size(101, 44);
            this.button_clear.TabIndex = 4;
            this.button_clear.Text = "Clear";
            this.button_clear.UseVisualStyleBackColor = true;
            this.button_clear.Click += new System.EventHandler(this.button_clear_Click);
            // 
            // Form_Vision
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1001, 628);
            this.Controls.Add(this.roundPanel1);
            this.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "Form_Vision";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AutoFrameVision";
            this.Load += new System.EventHandler(this.Form_Vision_Load);
            this.roundPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.visionControl7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.visionControl8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.visionControl9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.visionControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.visionControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.visionControl3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.visionControl4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.visionControl5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.visionControl6)).EndInit();
            this.roundPanel_button.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AutoFrameUI.RoundPanel roundPanel1;
        private System.Windows.Forms.Button button_debug;
        private System.Windows.Forms.Button button_delete;
        private System.Windows.Forms.Button button_clear;
        private System.Windows.Forms.Button button_cali;
        private ToolEx.ListBoxEx listbox_log;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private AutoFrameUI.RoundPanel roundPanel_button;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;

        private VisionControl visionControl1;
        private VisionControl visionControl2;
        private VisionControl visionControl3;
        private VisionControl visionControl4;
        private VisionControl visionControl5;
        private VisionControl visionControl6;
        private VisionControl visionControl7;
        private VisionControl visionControl8;
        private VisionControl visionControl9;
        private System.Windows.Forms.Button button_Step;
        private System.Windows.Forms.ComboBox comboBox_Step;
        private System.Windows.Forms.Button button_Config;
    }
}