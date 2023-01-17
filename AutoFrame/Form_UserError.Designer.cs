namespace AutoFrame
{
    partial class Form_UserError
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
            this.dataGridView_UserErrList = new System.Windows.Forms.DataGridView();
            this.button_Apply = new System.Windows.Forms.Button();
            this.button_Save = new System.Windows.Forms.Button();
            this.button_MoveUp = new System.Windows.Forms.Button();
            this.button_MoveDown = new System.Windows.Forms.Button();
            this.button_Remove = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_UserErrList)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView_UserErrList
            // 
            this.dataGridView_UserErrList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView_UserErrList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_UserErrList.Location = new System.Drawing.Point(13, 13);
            this.dataGridView_UserErrList.Name = "dataGridView_UserErrList";
            this.dataGridView_UserErrList.RowTemplate.Height = 23;
            this.dataGridView_UserErrList.Size = new System.Drawing.Size(436, 471);
            this.dataGridView_UserErrList.TabIndex = 0;
            this.dataGridView_UserErrList.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView_UserErrList_ColumnWidthChanged);
            this.dataGridView_UserErrList.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dataGridView_UserErrList_Scroll);
            // 
            // button_Apply
            // 
            this.button_Apply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Apply.Location = new System.Drawing.Point(458, 151);
            this.button_Apply.Name = "button_Apply";
            this.button_Apply.Size = new System.Drawing.Size(75, 30);
            this.button_Apply.TabIndex = 1;
            this.button_Apply.Text = "应用";
            this.button_Apply.UseVisualStyleBackColor = true;
            this.button_Apply.Click += new System.EventHandler(this.button_Apply_Click);
            // 
            // button_Save
            // 
            this.button_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Save.Location = new System.Drawing.Point(458, 197);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(75, 30);
            this.button_Save.TabIndex = 1;
            this.button_Save.Text = "保存";
            this.button_Save.UseVisualStyleBackColor = true;
            this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // button_MoveUp
            // 
            this.button_MoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_MoveUp.Location = new System.Drawing.Point(455, 13);
            this.button_MoveUp.Name = "button_MoveUp";
            this.button_MoveUp.Size = new System.Drawing.Size(75, 30);
            this.button_MoveUp.TabIndex = 1;
            this.button_MoveUp.Text = "上移";
            this.button_MoveUp.UseVisualStyleBackColor = true;
            this.button_MoveUp.Click += new System.EventHandler(this.button_MoveUp_Click);
            // 
            // button_MoveDown
            // 
            this.button_MoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_MoveDown.Location = new System.Drawing.Point(455, 59);
            this.button_MoveDown.Name = "button_MoveDown";
            this.button_MoveDown.Size = new System.Drawing.Size(75, 30);
            this.button_MoveDown.TabIndex = 1;
            this.button_MoveDown.Text = "下移";
            this.button_MoveDown.UseVisualStyleBackColor = true;
            this.button_MoveDown.Click += new System.EventHandler(this.button_MoveDown_Click);
            // 
            // button_Remove
            // 
            this.button_Remove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Remove.Location = new System.Drawing.Point(455, 105);
            this.button_Remove.Name = "button_Remove";
            this.button_Remove.Size = new System.Drawing.Size(75, 30);
            this.button_Remove.TabIndex = 1;
            this.button_Remove.Text = "移除";
            this.button_Remove.UseVisualStyleBackColor = true;
            this.button_Remove.Click += new System.EventHandler(this.button_Remove_Click);
            // 
            // Form_UserError
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(545, 496);
            this.Controls.Add(this.button_Save);
            this.Controls.Add(this.button_Remove);
            this.Controls.Add(this.button_MoveDown);
            this.Controls.Add(this.button_MoveUp);
            this.Controls.Add(this.button_Apply);
            this.Controls.Add(this.dataGridView_UserErrList);
            this.Name = "Form_UserError";
            this.Text = "自定义错误";
            this.Load += new System.EventHandler(this.Form_UserError_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_UserErrList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView_UserErrList;
        private System.Windows.Forms.Button button_Apply;
        private System.Windows.Forms.Button button_Save;
        private System.Windows.Forms.Button button_MoveUp;
        private System.Windows.Forms.Button button_MoveDown;
        private System.Windows.Forms.Button button_Remove;
    }
}