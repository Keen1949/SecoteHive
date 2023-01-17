namespace ToolEx
{
    partial class CylinderCtrl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CylinderCtrl));
            this.groupBox_Name = new System.Windows.Forms.GroupBox();
            this.button_back_i = new System.Windows.Forms.Button();
            this.imageList_In = new System.Windows.Forms.ImageList(this.components);
            this.button_out_i = new System.Windows.Forms.Button();
            this.button_back_o = new System.Windows.Forms.Button();
            this.imageList_Out = new System.Windows.Forms.ImageList(this.components);
            this.button_out_o = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox_Name.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox_Name
            // 
            this.groupBox_Name.Controls.Add(this.tableLayoutPanel1);
            this.groupBox_Name.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_Name.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox_Name.Location = new System.Drawing.Point(0, 0);
            this.groupBox_Name.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_Name.Name = "groupBox_Name";
            this.groupBox_Name.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_Name.Size = new System.Drawing.Size(628, 133);
            this.groupBox_Name.TabIndex = 0;
            this.groupBox_Name.TabStop = false;
            this.groupBox_Name.Text = "气缸名称";
            // 
            // button_back_i
            // 
            this.button_back_i.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_back_i.FlatAppearance.BorderSize = 0;
            this.button_back_i.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_back_i.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_back_i.ImageIndex = 1;
            this.button_back_i.ImageList = this.imageList_In;
            this.button_back_i.Location = new System.Drawing.Point(314, 57);
            this.button_back_i.Margin = new System.Windows.Forms.Padding(4);
            this.button_back_i.Name = "button_back_i";
            this.button_back_i.Size = new System.Drawing.Size(302, 45);
            this.button_back_i.TabIndex = 1;
            this.button_back_i.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button_back_i.UseVisualStyleBackColor = true;
            // 
            // imageList_In
            // 
            this.imageList_In.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList_In.ImageStream")));
            this.imageList_In.TransparentColor = System.Drawing.Color.White;
            this.imageList_In.Images.SetKeyName(0, "light_gray.png");
            this.imageList_In.Images.SetKeyName(1, "light_green.png");
            // 
            // button_out_i
            // 
            this.button_out_i.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_out_i.FlatAppearance.BorderSize = 0;
            this.button_out_i.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_out_i.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_out_i.ImageIndex = 0;
            this.button_out_i.ImageList = this.imageList_In;
            this.button_out_i.Location = new System.Drawing.Point(314, 4);
            this.button_out_i.Margin = new System.Windows.Forms.Padding(4);
            this.button_out_i.Name = "button_out_i";
            this.button_out_i.Size = new System.Drawing.Size(302, 45);
            this.button_out_i.TabIndex = 2;
            this.button_out_i.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button_out_i.UseVisualStyleBackColor = true;
            // 
            // button_back_o
            // 
            this.button_back_o.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_back_o.FlatAppearance.BorderSize = 0;
            this.button_back_o.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_back_o.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_back_o.ImageIndex = 1;
            this.button_back_o.ImageList = this.imageList_Out;
            this.button_back_o.Location = new System.Drawing.Point(4, 57);
            this.button_back_o.Margin = new System.Windows.Forms.Padding(4);
            this.button_back_o.Name = "button_back_o";
            this.button_back_o.Size = new System.Drawing.Size(302, 45);
            this.button_back_o.TabIndex = 0;
            this.button_back_o.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button_back_o.UseVisualStyleBackColor = true;
            this.button_back_o.Click += new System.EventHandler(this.button_o_Click);
            // 
            // imageList_Out
            // 
            this.imageList_Out.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList_Out.ImageStream")));
            this.imageList_Out.TransparentColor = System.Drawing.Color.White;
            this.imageList_Out.Images.SetKeyName(0, "btn_off_switch.png");
            this.imageList_Out.Images.SetKeyName(1, "btn_on_switch.png");
            // 
            // button_out_o
            // 
            this.button_out_o.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_out_o.FlatAppearance.BorderSize = 0;
            this.button_out_o.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_out_o.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_out_o.ImageIndex = 0;
            this.button_out_o.ImageList = this.imageList_Out;
            this.button_out_o.Location = new System.Drawing.Point(4, 4);
            this.button_out_o.Margin = new System.Windows.Forms.Padding(4);
            this.button_out_o.Name = "button_out_o";
            this.button_out_o.Size = new System.Drawing.Size(302, 45);
            this.button_out_o.TabIndex = 0;
            this.button_out_o.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button_out_o.UseVisualStyleBackColor = true;
            this.button_out_o.Click += new System.EventHandler(this.button_o_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.button_out_o, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.button_back_i, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.button_back_o, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.button_out_i, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 23);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(620, 106);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // CylinderCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox_Name);
            this.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "CylinderCtrl";
            this.Size = new System.Drawing.Size(628, 133);
            this.Load += new System.EventHandler(this.CylinderCtrl_Load);
            this.groupBox_Name.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox_Name;
        private System.Windows.Forms.Button button_out_o;
        private System.Windows.Forms.Button button_back_i;
        private System.Windows.Forms.Button button_out_i;
        private System.Windows.Forms.Button button_back_o;
        private System.Windows.Forms.ImageList imageList_Out;
        private System.Windows.Forms.ImageList imageList_In;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
