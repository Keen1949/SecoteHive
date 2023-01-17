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
using AutoFrameDll;
using AutoFrameVision;
using System.IO;
using ToolEx;

namespace AutoFrame
{
    public partial class Form_debug : Form
    {
        public Form_debug()
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

        private void Form_debug_Load(object sender, EventArgs e)
        {
            dataGridView_station.RowsDefaultCellStyle.BackColor = Color.Ivory;
            dataGridView_station.AlternatingRowsDefaultCellStyle.BackColor = Color.PaleTurquoise;
            dataGridView_station.Rows.Add(StationMgr.GetInstance().m_lsStation.Count);
            UpdateStation();


            Array ar = Enum.GetValues(typeof(SysBitReg));
            dataGridView_reg_bit.RowsDefaultCellStyle.BackColor = Color.Ivory;
            dataGridView_reg_bit.AlternatingRowsDefaultCellStyle.BackColor = Color.PaleTurquoise;
            dataGridView_reg_bit.Rows.Add(ar.Length);
            UpdateRegBit();

            ar = Enum.GetValues(typeof(SysFloatReg));
            dataGridView_reg_float.RowsDefaultCellStyle.BackColor = Color.Ivory;
            dataGridView_reg_float.AlternatingRowsDefaultCellStyle.BackColor = Color.PaleTurquoise;
            dataGridView_reg_float.Rows.Add(ar.Length);
            UpdateRegFloat();


        }

        private void UpdateStation()
        {
            int i = 0;
            foreach (StationBase sb in StationMgr.GetInstance().m_lsStation)
            {
                dataGridView_station.Rows[i].Cells[0].Value = sb.Name;
                dataGridView_station.Rows[i].Cells[1].Value = sb.StationEnable;
                dataGridView_station.Rows[i].Cells[2].Value = !sb.StationEnable;
                i++;
            }
        }
        private void UpdateRegBit()
        {
            int i = 0;
            Array ar = Enum.GetValues(typeof(SysBitReg));
       //     int i = 0; 
            foreach (SysBitReg sbr in ar)
            {
                dataGridView_reg_bit.Rows[i].Cells[0].Value = ((int)(sbr)).ToString();
                dataGridView_reg_bit.Rows[i].Cells[1].Value = sbr.ToString();
                dataGridView_reg_bit.Rows[i].Cells[2].Value = SystemMgr.GetInstance().GetRegBit((int)sbr);
                dataGridView_reg_bit.Rows[i].Cells[3].Value = !SystemMgr.GetInstance().GetRegBit((int)sbr);

                i++;
            }
        }

        private void UpdateRegFloat()
        {
            int i = 0;
            Array ar = Enum.GetValues(typeof(SysFloatReg));
            foreach (SysFloatReg sbr in ar)
            {
                dataGridView_reg_float.Rows[i].Cells[0].Value = sbr.ToString();
                dataGridView_reg_float.Rows[i].Cells[1].Value = string.Format("{0:0000.000}", SystemMgr.GetInstance().GetRegDouble((int)sbr));
                //        dataGridView_reg_float.Rows[i].Cells[2].Value = !SystemMgr.GetInstance().GetRegBit((int)sbr);

                i++;
            }
        }

        private void button_sta_true_Click(object sender, EventArgs e)
        {
            foreach (StationBase sb in StationMgr.GetInstance().m_lsStation)
            {
                sb.StationEnable = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (StationBase sb in StationMgr.GetInstance().m_lsStation)
            {
                sb.StationEnable = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateStation();
            UpdateRegBit();
            UpdateRegFloat();
        }

        private void button_bit_true_Click(object sender, EventArgs e)
        {
            Array ar = Enum.GetValues(typeof(SysBitReg));
            foreach (SysBitReg sbr in ar)
            {
                SystemMgr.GetInstance().WriteRegBit((int)sbr, true);
            }
        }


        private void button_bit_false_Click(object sender, EventArgs e)
        {
            Array ar = Enum.GetValues(typeof(SysBitReg));
            foreach (SysBitReg sbr in ar)
            {
                SystemMgr.GetInstance().WriteRegBit((int)sbr, false);
            }
        }


        private void button_float_zero_Click(object sender, EventArgs e)
        {
            Array ar = Enum.GetValues(typeof(SysFloatReg));
            foreach (SysFloatReg sbr in ar)
            {
                SystemMgr.GetInstance().WriteRegDouble((int)sbr, 0);
            }
        }

        private void dataGridView_station_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1 || e.ColumnIndex == 2)
            {
                string strName = dataGridView_station.Rows[e.RowIndex].Cells[0].Value.ToString();
                StationBase sb = StationMgr.GetInstance().GetStation(strName);
              
                sb.StationEnable = !sb.StationEnable;

                dataGridView_station.Rows[e.RowIndex].Cells[1].Value = sb.StationEnable;
                dataGridView_station.Rows[e.RowIndex].Cells[2].Value = !sb.StationEnable;
            }
        }

        private void dataGridView_reg_bit_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2 || e.ColumnIndex == 3)
            {
                Array ar = Enum.GetValues(typeof(SysBitReg));

                bool bTrue = SystemMgr.GetInstance().GetRegBit((int)ar.GetValue(e.RowIndex));
                bTrue = !bTrue;
                SystemMgr.GetInstance().WriteRegBit((int)ar.GetValue(e.RowIndex), bTrue);

                dataGridView_reg_bit.Rows[e.RowIndex].Cells[2].Value = bTrue;
                dataGridView_reg_bit.Rows[e.RowIndex].Cells[3].Value = !bTrue;
            }


        }

        private void dataGridView_reg_float_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 2)
            {
                object value = dataGridView_reg_float[e.ColumnIndex, e.RowIndex].Value;
                if ( value != null)
                {
                    double data = 0;
                    if(double.TryParse(value.ToString(), out data))
                    {
                        Array ar = Enum.GetValues(typeof(SysFloatReg));
                        SystemMgr.GetInstance().WriteRegDouble((int)ar.GetValue(e.RowIndex), data);
                        dataGridView_reg_float[e.ColumnIndex - 1, e.RowIndex].Value = data.ToString();
                    }
                    else
                    {
                        dataGridView_reg_float[e.ColumnIndex , e.RowIndex].Value = null;
                    }
                }  
            }

        }
    }
}

