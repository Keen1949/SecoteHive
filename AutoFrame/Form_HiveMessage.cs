using AutoFrameDll;
using CommonTool;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoFrame
{
    public partial class Form_HiveMessage : Form
    {
        public Form_HiveMessage()
        {
            InitializeComponent();
        }

        public Form_HiveMessage(string Message)
        {
            InitializeComponent();
            
            Label_HiveMessage.Text = Message.Trim();
            try
            {
                SystemMgr.GetInstance().WriteRegBit((int)SysBitReg.bit_PLC蜂鸣响, true);
                //IoMgr.GetInstance().WriteIoBit("黄灯", true);
                //IoMgr.GetInstance().WriteIoBit("蜂鸣器", true);
            }
            catch
            {                
            }
            
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            try
            {
                SystemMgr.GetInstance().WriteRegBit((int)SysBitReg.bit_PLC蜂鸣响, false);
                //IoMgr.GetInstance().WriteIoBit("黄灯", false);
                //IoMgr.GetInstance().WriteIoBit("蜂鸣器", false);
            }
            catch 
            {                
            }
            
            this.Close();
        }

        private void Form_HiveMessage_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                SystemMgr.GetInstance().WriteRegBit((int)SysBitReg.bit_PLC蜂鸣响, false);
                //IoMgr.GetInstance().WriteIoBit("黄灯", false);
                //IoMgr.GetInstance().WriteIoBit("蜂鸣器", false);
            }
            catch
            {
            }
           
        }
    }
}
