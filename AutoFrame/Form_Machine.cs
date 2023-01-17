using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonTool;
using System.IO;
using ToolEx;

namespace AutoFrame
{
    public partial class Form_Machine : Form
    {
        public Form_Machine()
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

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
        //    for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
        //    {
        //        if (i % 2 == 0)
        //        {
        //            this.dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Gray;
        ////            this.dataGridViewBase.Rows[i].DefaultCellStyle.Font = this.splitContainer1.Font;
        //        }
        //        else
        //        {
        //            this.dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.White;
        ////            this.dataGridView1.Rows[i].DefaultCellStyle.Font = this.splitContainer1.Font;
        //        }
        //    }
        }

        private void Form_Machine_Load(object sender, EventArgs e)
        {
            //更新设备信息
            //dataGridView1.Rows.Add(3);
            //dataGridView1.Rows[0].Cells[0].Value = "Denso V132";
            //dataGridView1.Rows[0].Cells[1].Value = "OPT-RING 32mm";
            //dataGridView1.Rows[0].Cells[2].Value = "Cognex M34X3";
            //dataGridView1.Rows[0].Cells[3].Value = "Cognex - CCD TU833";
            //dataGridView1.Rows[1].Cells[1].Value = "OPT-RING 32mm";
            //dataGridView1.Rows[1].Cells[3].Value = "Cognex - CCD TU833";

            //dataGridView1.Rows[2].Cells[1].Value = "OPT-RING 32mm";
            //dataGridView1.Rows[2].Cells[3].Value = "Cognex - CCD TU833";

            dataGridView1.RowsDefaultCellStyle.BackColor = Color.Ivory;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.PaleTurquoise;


            OnProductInfoChanged();
            ProductMgr.GetInstance().ProductInfoChangedEvent += OnProductInfoChanged;


            DateTime dtExe = System.IO.File.GetLastWriteTime(this.GetType().Assembly.Location);
            label_build_date.Text = dtExe.ToString();
            label_version.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            FileInfo fi = new FileInfo(SystemMgr.GetInstance().m_strSystemParamName);
            if (fi.Exists)
            {
                if (dtExe > fi.LastWriteTime)
                {
                    label_last_date.Text = dtExe.ToString();
                }
                else
                {
                    label_last_date.Text = fi.LastWriteTime.ToString();
                }
                
            }

            
        }

        private void OnProductInfoChanged()
        {

            int nMachineTime = ProductMgr.GetInstance().MachineTime;
            int nSoftwareTime = ProductMgr.GetInstance().SoftwareTime;

            TimeSpan ts = new TimeSpan(0, 0, nMachineTime);
            label_machine_life.Text = string.Format("{0}天{1}小时{2}分{3}秒", ts.Days, ts.Hours, ts.Minutes, ts.Seconds);

            ts = new TimeSpan(0, 0, nSoftwareTime);
            label_software_life.Text = string.Format("{0}天{1}小时{2}分{3}秒", ts.Days, ts.Hours, ts.Minutes, ts.Seconds);

            label_DeviceName.Text = ProductMgr.GetInstance().DeviceName;
            label_DeviceID.Text = ProductMgr.GetInstance().DeviceID;
            label_Air.Text = ProductMgr.GetInstance().AirPressure;
            label_Voltage.Text = ProductMgr.GetInstance().Voltage;
            label_Current.Text = ProductMgr.GetInstance().Current;
            label_Power.Text = ProductMgr.GetInstance().Power;

            //Motion
            dataGridView1.Rows.Clear();
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
    }
}

