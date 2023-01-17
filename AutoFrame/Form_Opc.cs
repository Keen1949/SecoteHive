using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Communicate;
using System.IO;
using CommonTool;

namespace AutoFrame
{
    public partial class Form_Opc : Form
    {
        public Form_Opc()
        {
            InitializeComponent();

            OnLanguageChangeEvent(LanguageMgr.GetInstance().Language,true);

            LanguageMgr.GetInstance().LanguageChangeEvent += OnLanguageChangeEvent;

        }

        private void OnLanguageChangeEvent(string strLanguage, bool bChange)
        {
            IniHelper ini = new IniHelper();

            ini.IniFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "language", strLanguage, this.GetType().Namespace + ".ini");

            if (bChange)
            {
                LanguageMgr.GetInstance().ChangeUIText(this.GetType().Name, this, ini);
            }
            else
            {
                LanguageMgr.GetInstance().SaveUIText(this.GetType().Name, this, ini);
            }
        }

        private void Form_Opc_Load(object sender, EventArgs e)
        {
            checkBoxOpc.Checked = OpcMgr.GetInstance().OpcEnable;

            UpdateCtrls();             
            
        }

        void UpdateCtrls()
        {
            if (OpcMgr.GetInstance().GetOpcLink()  != null)
            {
                object opcServers = OpcMgr.GetInstance().GetOpcLink().ServerNames;

                this.comboBox_ServerName.Items.Clear();
                this.dataGridView_opc.Rows.Clear();

                foreach (string str in (Array)opcServers)
                {
                    this.comboBox_ServerName.Items.Add(str);
                }

                if (OpcMgr.GetInstance().GetOpcLink().OpcConnected)
                {
                    //注册消息
                    OpcMgr.GetInstance().RegistorMsg(KepGroup_DataChange);
                }
            }

            this.textBox_ServerIP.Text = OpcMgr.GetInstance().ServerIP;
            this.textBox_GroupName.Text = OpcMgr.GetInstance().GroupName;
            this.comboBox_ServerName.Text = OpcMgr.GetInstance().ServerName;
            this.textBox_UpdateRate.Text = OpcMgr.GetInstance().UpdateRate.ToString();

            OpcMgr.GetInstance().UpdateGridFromParam(this.dataGridView_opc);

        }

        void KepGroup_DataChange(int TransactionID, int NumItems, ref Array ClientHandles, ref Array ItemValues, ref Array Qualities, ref Array TimeStamps)
        {
            for (int i = 1; i <= NumItems; i++)
            {
                if (ItemValues.GetValue(i) == null)
                    continue;

                int nIdx = (int)ClientHandles.GetValue(i) - OpcMgr.OPC_Hander_Start;
                dataGridView_opc.Rows[nIdx].Cells[2].Value = ItemValues.GetValue(i).ToString();
                dataGridView_opc.Rows[nIdx].Cells[3].Value = Qualities.GetValue(i).ToString();
                dataGridView_opc.Rows[nIdx].Cells[4].Value = TimeStamps.GetValue(i).ToString();
            }
        }

        private void button_Load_Click(object sender, EventArgs e)
        {
            AutoTool.LoadSystemCfgEx();

            UpdateCtrls();
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            OpcMgr.GetInstance().UpdateParamFromGrid(dataGridView_opc);

            OpcMgr.GetInstance().OpcEnable = checkBoxOpc.Checked;

            OpcMgr.GetInstance().ServerName = this.comboBox_ServerName.Text;
            OpcMgr.GetInstance().ServerIP = this.textBox_ServerIP.Text;
            OpcMgr.GetInstance().GroupName = this.textBox_GroupName.Text;
            OpcMgr.GetInstance().UpdateRate = Convert.ToInt32(this.textBox_UpdateRate.Text);
            
            AutoTool.SaveSystemCfgEx();

            AutoTool.LoadSystemCfgEx();
            UpdateCtrls();
        }

        private void button_UpdateSelect_Click(object sender, EventArgs e)
        {
            foreach(DataGridViewCell item in this.dataGridView_opc.SelectedCells)
            {
                string strTag = item.OwningRow.Cells[0].Value.ToString();
                string val = item.OwningRow.Cells[2].Value.ToString();
                OpcMgr.GetInstance().WriteDataByTag(strTag, val);
            }
        }

        private void button_UpdateAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in this.dataGridView_opc.Rows)
            {
                if (item.Cells[0].Value != null && item.Cells[2].Value != null)
                {
                    string strTag = item.Cells[0].Value.ToString();
                    string val = item.Cells[2].Value.ToString();
                    OpcMgr.GetInstance().WriteDataByTag(strTag, val);
                }
                
            }
        }
    }
}
