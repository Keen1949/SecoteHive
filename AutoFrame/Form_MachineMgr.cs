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
using System.IO;
using ToolEx;

namespace AutoFrame
{
    public partial class Form_MachineMgr : Form
    {
        public Form_MachineMgr()
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

        private void Form_MachineMgr_Load(object sender, EventArgs e)
        {
            TopMost = true;
            UpdateProductInfo();

            Security.ModeChangedEvent += OnChangeMode;
        }

        private void OnChangeMode()
        {
            if (Security.GetUserMode() >= UserMode.Engineer)
            {
                button_Del.Enabled = true;
                button_Reset.Enabled = true;
                button_Save.Enabled = true;

                textBox_DeviceName.Enabled = true;
                textBox_DeviceID.Enabled = true;
                textBox_Air.Enabled = true;
                textBox_Voltage.Enabled = true;
                textBox_Current.Enabled = true;
                textBox_Power.Enabled = true;

                comboBox_JobCount.Enabled = true;
                comboBox_JobMode.Enabled = true;

                dataGridView1.ReadOnly = false;
            }
            else
            { 
                button_Del.Enabled = false;
                button_Reset.Enabled = false;
                button_Save.Enabled = false;

                textBox_DeviceName.Enabled = false;
                textBox_DeviceID.Enabled = false;
                textBox_Air.Enabled = false;
                textBox_Voltage.Enabled = false;
                textBox_Current.Enabled = false;
                textBox_Power.Enabled = false;

                comboBox_JobCount.Enabled = false;
                comboBox_JobMode.Enabled = false;

                dataGridView1.ReadOnly = true;
            }
        }

        private void UpdateProductInfo()
        {
            int nMachineTime = ProductMgr.GetInstance().MachineTime;
            int nSoftwareTime = ProductMgr.GetInstance().SoftwareTime;

            TimeSpan ts = new TimeSpan(0, 0, nMachineTime);
            textBox_MachineTime.Text = string.Format("{0:00} {1:00}:{2:00}:{3:00}", ts.Days, ts.Hours, ts.Minutes, ts.Seconds);

            ts = new TimeSpan(0, 0, nSoftwareTime);
            textBox_SoftwareTime.Text = string.Format("{0:00} {1:00}:{2:00}:{3:00}", ts.Days, ts.Hours, ts.Minutes, ts.Seconds);

            textBox_DeviceName.Text = ProductMgr.GetInstance().DeviceName;
            textBox_DeviceID.Text = ProductMgr.GetInstance().DeviceID;
            textBox_Air.Text = ProductMgr.GetInstance().AirPressure;
            textBox_Voltage.Text = ProductMgr.GetInstance().Voltage;
            textBox_Current.Text = ProductMgr.GetInstance().Current;
            textBox_Power.Text = ProductMgr.GetInstance().Power;

            comboBox_JobCount.Items.Clear();
            for (int i = 1; i <= 20; i++)
            {
                comboBox_JobCount.Items.Add(i);
            }
            comboBox_JobCount.Text = ProductMgr.GetInstance().JobCount.ToString();

            comboBox_JobMode.Items.Clear();
            foreach (var item in Enum.GetNames(typeof(JobMode)))
            {
                comboBox_JobMode.Items.Add(item);
            }

            comboBox_JobMode.SelectedIndex = (int)ProductMgr.GetInstance().Mode;


            dataGridView1.Rows.Clear();
            dataGridView1.RowsDefaultCellStyle.BackColor = Color.Ivory;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.PaleTurquoise;
            //Motion
            foreach (string str in ProductMgr.GetInstance().MotionArray)
            {
                int row = dataGridView1.Rows.Add();
                dataGridView1.Rows[row].Cells[0].Value = str;
            }


            //Robot
            int len = dataGridView1.Rows.Count - 1;

            if (ProductMgr.GetInstance().RobotArray.Length < len)
            {
                len = ProductMgr.GetInstance().RobotArray.Length;
            }

            string[] array = ProductMgr.GetInstance().RobotArray;

            for (int i = 0; i < len; i++)
            {
                dataGridView1.Rows[i].Cells[1].Value = array[i];
            }

            for (int i = len; i < ProductMgr.GetInstance().RobotArray.Length; i++)
            {
                int row = dataGridView1.Rows.Add();
                dataGridView1.Rows[row].Cells[1].Value = array[i];
            }

            //CCD
            len = dataGridView1.Rows.Count - 1;

            if (ProductMgr.GetInstance().CCDArray.Length < len)
            {
                len = ProductMgr.GetInstance().CCDArray.Length;
            }

            array = ProductMgr.GetInstance().CCDArray;

            for (int i = 0; i < len; i++)
            {
                dataGridView1.Rows[i].Cells[2].Value = array[i];
            }

            for (int i = len; i < ProductMgr.GetInstance().CCDArray.Length; i++)
            {
                int row = dataGridView1.Rows.Add();
                dataGridView1.Rows[row].Cells[2].Value = array[i];
            }

            //LENS
            len = dataGridView1.Rows.Count - 1;

            if (ProductMgr.GetInstance().LensArray.Length < len)
            {
                len = ProductMgr.GetInstance().LensArray.Length;
            }

            array = ProductMgr.GetInstance().LensArray;

            for (int i = 0; i < len; i++)
            {
                dataGridView1.Rows[i].Cells[3].Value = array[i];
            }

            for (int i = len; i < ProductMgr.GetInstance().LensArray.Length; i++)
            {
                int row = dataGridView1.Rows.Add();
                dataGridView1.Rows[row].Cells[3].Value = array[i];
            }

            //Light
            len = dataGridView1.Rows.Count - 1;

            if (ProductMgr.GetInstance().LightArray.Length < len)
            {
                len = ProductMgr.GetInstance().LightArray.Length;
            }

            array = ProductMgr.GetInstance().LightArray;

            for (int i = 0; i < len; i++)
            {
                dataGridView1.Rows[i].Cells[4].Value = array[i];
            }

            for (int i = len; i < ProductMgr.GetInstance().LightArray.Length; i++)
            {
                int row = dataGridView1.Rows.Add();
                dataGridView1.Rows[row].Cells[4].Value = array[i];
            }
        }

        private void button_Reset_Click(object sender, EventArgs e)
        {
            ProductMgr.GetInstance().SoftwareTime = 0;
            ProductMgr.GetInstance().MachineTime = 0;

            TimeSpan ts = new TimeSpan(0, 0, 0);
            textBox_MachineTime.Text = string.Format("{0:00} {1:00}:{2:00}:{3:00}", ts.Days, ts.Hours, ts.Minutes, ts.Seconds);

            ts = new TimeSpan(0, 0, 0);
            textBox_SoftwareTime.Text = string.Format("{0:00} {1:00}:{2:00}:{3:00}", ts.Days, ts.Hours, ts.Minutes, ts.Seconds);

            ProductMgr.GetInstance().UpdateProductInfo();
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            
            ProductMgr.GetInstance().DeviceName = textBox_DeviceName.Text;
            ProductMgr.GetInstance().DeviceID = textBox_DeviceID.Text;
            ProductMgr.GetInstance().AirPressure = textBox_Air.Text;
            ProductMgr.GetInstance().Voltage = textBox_Voltage.Text;
            ProductMgr.GetInstance().Current = textBox_Current.Text;
            ProductMgr.GetInstance().Power = textBox_Power.Text;

            
            ProductMgr.GetInstance().JobCount = Convert.ToInt32(comboBox_JobCount.Text);

            ProductMgr.GetInstance().Mode = (JobMode)comboBox_JobMode.SelectedIndex;

            List<string> listMotion = new List<string>();
            List<string> listRobot = new List<string>();
            List<string> listCCD = new List<string>();
            List<string> listLight = new List<string>();
            List<string> listLens = new List<string>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value != null)
                {
                    string strValue = row.Cells[0].Value.ToString();
                    if (strValue.Length > 0)
                    {
                        listMotion.Add(strValue);
                    }
                }

                if (row.Cells[1].Value != null)
                {
                    string strValue = row.Cells[1].Value.ToString();
                    if (strValue.Length > 0)
                    {
                        listRobot.Add(strValue);
                    }
                }

                if (row.Cells[2].Value != null)
                {
                    string strValue = row.Cells[2].Value.ToString();
                    if (strValue.Length > 0)
                    {
                        listCCD.Add(strValue);
                    }
                }

                if (row.Cells[3].Value != null)
                {
                    string strValue = row.Cells[3].Value.ToString();
                    if (strValue.Length > 0)
                    {
                        listLight.Add(strValue);
                    }
                }

                if (row.Cells[4].Value != null)
                {
                    string strValue = row.Cells[4].Value.ToString();
                    if (strValue.Length > 0)
                    {
                        listLens.Add(strValue);
                    }

                }
            }

            ProductMgr.GetInstance().MotionArray = listMotion.ToArray();
            ProductMgr.GetInstance().RobotArray = listRobot.ToArray();
            ProductMgr.GetInstance().CCDArray = listCCD.ToArray();
            ProductMgr.GetInstance().LightArray = listLight.ToArray();
            ProductMgr.GetInstance().LensArray = listLens.ToArray();

            ProductMgr.GetInstance().UpdateProductInfo();
            ProductMgr.GetInstance().WriteCfg();

            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                MessageBox.Show("Save configuration complete");
            }
            else
            {
                MessageBox.Show("保存配置完成");
            }
            
        }

        private void button_Del_Click(object sender, EventArgs e)
        {
            int nSelectRow = dataGridView1.CurrentCell.RowIndex;
            int nSelectCol = dataGridView1.CurrentCell.ColumnIndex;

            int i = nSelectRow;
            for (; i < dataGridView1.Rows.Count - 2;i++)
            {
                dataGridView1.Rows[i].Cells[nSelectCol].Value = dataGridView1.Rows[i + 1].Cells[nSelectCol].Value;
            }

            dataGridView1.Rows[i].Cells[nSelectCol].Value = null;

        }
    }
}
