namespace AutoFrameVision
{
    partial class Form_Vision_config
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
            this.button_ok = new System.Windows.Forms.Button();
            this.label31 = new System.Windows.Forms.Label();
            this.groupBox_Step = new System.Windows.Forms.GroupBox();
            this.checkBox_Gain = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDown_Y = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_ExposeTime = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_X = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_DigitalShift = new System.Windows.Forms.NumericUpDown();
            this.label19 = new System.Windows.Forms.Label();
            this.numericUpDown_Gain = new System.Windows.Forms.NumericUpDown();
            this.label30 = new System.Windows.Forms.Label();
            this.numericUpDown_U = new System.Windows.Forms.NumericUpDown();
            this.groupBox_Step.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_ExposeTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_X)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_DigitalShift)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Gain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_U)).BeginInit();
            this.SuspendLayout();
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(472, 12);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(98, 56);
            this.button_ok.TabIndex = 1;
            this.button_ok.Text = "确定";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(631, 240);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(0, 12);
            this.label31.TabIndex = 2;
            // 
            // groupBox_Step
            // 
            this.groupBox_Step.Controls.Add(this.checkBox_Gain);
            this.groupBox_Step.Controls.Add(this.label2);
            this.groupBox_Step.Controls.Add(this.label33);
            this.groupBox_Step.Controls.Add(this.label35);
            this.groupBox_Step.Controls.Add(this.label29);
            this.groupBox_Step.Controls.Add(this.label17);
            this.groupBox_Step.Controls.Add(this.label1);
            this.groupBox_Step.Controls.Add(this.label15);
            this.groupBox_Step.Controls.Add(this.label3);
            this.groupBox_Step.Controls.Add(this.numericUpDown_Y);
            this.groupBox_Step.Controls.Add(this.numericUpDown_ExposeTime);
            this.groupBox_Step.Controls.Add(this.numericUpDown_X);
            this.groupBox_Step.Controls.Add(this.numericUpDown_DigitalShift);
            this.groupBox_Step.Controls.Add(this.label19);
            this.groupBox_Step.Controls.Add(this.numericUpDown_Gain);
            this.groupBox_Step.Controls.Add(this.label30);
            this.groupBox_Step.Controls.Add(this.numericUpDown_U);
            this.groupBox_Step.Location = new System.Drawing.Point(13, 7);
            this.groupBox_Step.Name = "groupBox_Step";
            this.groupBox_Step.Size = new System.Drawing.Size(453, 223);
            this.groupBox_Step.TabIndex = 3;
            this.groupBox_Step.TabStop = false;
            this.groupBox_Step.Text = "Step";
            // 
            // checkBox_Gain
            // 
            this.checkBox_Gain.AutoSize = true;
            this.checkBox_Gain.Location = new System.Drawing.Point(8, 69);
            this.checkBox_Gain.Name = "checkBox_Gain";
            this.checkBox_Gain.Size = new System.Drawing.Size(72, 16);
            this.checkBox_Gain.TabIndex = 22;
            this.checkBox_Gain.Text = "启用增益";
            this.checkBox_Gain.UseVisualStyleBackColor = true;
            this.checkBox_Gain.CheckedChanged += new System.EventHandler(this.checkBox_Gain_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 136);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 21;
            this.label2.Text = "增益倍率:";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(390, 102);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(29, 12);
            this.label33.TabIndex = 20;
            this.label33.Text = "像素";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(392, 33);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(17, 12);
            this.label35.TabIndex = 18;
            this.label35.Text = "度";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(390, 68);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(29, 12);
            this.label29.TabIndex = 15;
            this.label29.Text = "像素";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(237, 102);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(65, 12);
            this.label17.TabIndex = 13;
            this.label17.Text = "组装补偿Y:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 14;
            this.label1.Text = "曝光时间:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(237, 68);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(65, 12);
            this.label15.TabIndex = 10;
            this.label15.Text = "组装补偿X:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(238, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 11;
            this.label3.Text = "角度范围:";
            // 
            // numericUpDown_Y
            // 
            this.numericUpDown_Y.DecimalPlaces = 3;
            this.numericUpDown_Y.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown_Y.Location = new System.Drawing.Point(315, 98);
            this.numericUpDown_Y.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numericUpDown_Y.Name = "numericUpDown_Y";
            this.numericUpDown_Y.Size = new System.Drawing.Size(71, 21);
            this.numericUpDown_Y.TabIndex = 4;
            // 
            // numericUpDown_ExposeTime
            // 
            this.numericUpDown_ExposeTime.Location = new System.Drawing.Point(90, 29);
            this.numericUpDown_ExposeTime.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDown_ExposeTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_ExposeTime.Name = "numericUpDown_ExposeTime";
            this.numericUpDown_ExposeTime.Size = new System.Drawing.Size(78, 21);
            this.numericUpDown_ExposeTime.TabIndex = 8;
            this.numericUpDown_ExposeTime.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numericUpDown_X
            // 
            this.numericUpDown_X.DecimalPlaces = 3;
            this.numericUpDown_X.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown_X.Location = new System.Drawing.Point(315, 64);
            this.numericUpDown_X.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numericUpDown_X.Name = "numericUpDown_X";
            this.numericUpDown_X.Size = new System.Drawing.Size(71, 21);
            this.numericUpDown_X.TabIndex = 7;
            // 
            // numericUpDown_DigitalShift
            // 
            this.numericUpDown_DigitalShift.Location = new System.Drawing.Point(90, 132);
            this.numericUpDown_DigitalShift.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDown_DigitalShift.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_DigitalShift.Name = "numericUpDown_DigitalShift";
            this.numericUpDown_DigitalShift.Size = new System.Drawing.Size(78, 21);
            this.numericUpDown_DigitalShift.TabIndex = 6;
            this.numericUpDown_DigitalShift.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(171, 34);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(17, 12);
            this.label19.TabIndex = 19;
            this.label19.Text = "us";
            // 
            // numericUpDown_Gain
            // 
            this.numericUpDown_Gain.Location = new System.Drawing.Point(90, 98);
            this.numericUpDown_Gain.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDown_Gain.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_Gain.Name = "numericUpDown_Gain";
            this.numericUpDown_Gain.Size = new System.Drawing.Size(78, 21);
            this.numericUpDown_Gain.TabIndex = 5;
            this.numericUpDown_Gain.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(8, 102);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(59, 12);
            this.label30.TabIndex = 12;
            this.label30.Text = "曝光增益:";
            // 
            // numericUpDown_U
            // 
            this.numericUpDown_U.DecimalPlaces = 3;
            this.numericUpDown_U.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDown_U.Location = new System.Drawing.Point(315, 29);
            this.numericUpDown_U.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.numericUpDown_U.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.numericUpDown_U.Name = "numericUpDown_U";
            this.numericUpDown_U.Size = new System.Drawing.Size(71, 21);
            this.numericUpDown_U.TabIndex = 9;
            // 
            // Form_Vision_config
            // 
            this.AcceptButton = this.button_ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(581, 242);
            this.Controls.Add(this.groupBox_Step);
            this.Controls.Add(this.label31);
            this.Controls.Add(this.button_ok);
            this.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_Vision_config";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "视觉参数配置";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Vision_config_FormClosing);
            this.Load += new System.EventHandler(this.Form_Vision_config_Load);
            this.groupBox_Step.ResumeLayout(false);
            this.groupBox_Step.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_ExposeTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_X)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_DigitalShift)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Gain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_U)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.GroupBox groupBox_Step;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDown_Y;
        private System.Windows.Forms.NumericUpDown numericUpDown_ExposeTime;
        private System.Windows.Forms.NumericUpDown numericUpDown_X;
        private System.Windows.Forms.NumericUpDown numericUpDown_DigitalShift;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.NumericUpDown numericUpDown_Gain;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.NumericUpDown numericUpDown_U;
        private System.Windows.Forms.CheckBox checkBox_Gain;
    }
}