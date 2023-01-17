using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonTool;
using AutoFrameDll;
using AutoFrameVision;
using Communicate;
//using HalconDotNet;
using ToolEx;

namespace AutoFrame
{
    public partial class Form_Data : Form
    {
         public Form_Data()
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

        private void Form_Data_Load(object sender, EventArgs e)
        {
            double[] dForce = new double [200];
            Random rnd1 = new Random();
            for (int i=0; i<200;++i)
            {
                dForce[i] = rnd1.Next(9000, 10000) / 10.0;
            }
            chart1.Series[0].Points.DataBindY(dForce);
            for (int i = 0; i < 200; ++i)
            {
                dForce[i] = rnd1.Next(7000, 9000) / 10.0;
            }
            chart1.Series[1].Points.DataBindY(dForce);
            for (int i = 0; i < 200; ++i)
            {
                dForce[i] = rnd1.Next(6000, 9500) / 10.0;
            }
            chart1.Series[2].Points.DataBindY(dForce);



            roundPanel_force.Visible = true;
            halfRing1.Visible = false;
            halfRing1.Parent = roundPanel2;
        }

        private void button_press_Click(object sender, EventArgs e)
        {
            roundPanel_force.Visible = true;
            halfRing1.Visible = false;

        }

        private void button_data_Click(object sender, EventArgs e)
        {
            roundPanel_force.Visible = false;
            halfRing1.Visible = true;
        //    halfRing1.Show();
        }
    }
}
