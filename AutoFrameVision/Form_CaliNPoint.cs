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
using CommonTool;


namespace AutoFrameVision
{
    /// <summary>
    /// 标定配置对话框内
    /// </summary>
    public partial class Form_CaliNPoint : Form
    {
        CaliTranslate m_trans = new CaliTranslate();
        /// <summary>
        /// 
        /// </summary>
        public Form_CaliNPoint()
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


        /// <summary>
        /// 窗口初始化时,可改写为自动加载某一标定文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CaliNPoint_Load(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// 加载一个标定文件的座标数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_load_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox_path.Text = openFileDialog1.FileName;

                listBox_point_info.Items.Clear();
                if (m_trans.LoadCaliData(openFileDialog1.FileName))
                {
                    dataGridView_point.Rows.Clear();
                    int n = m_trans.GetDataLength();
                    for(int i=0; i< n; ++i)
                    {
                        dataGridView_point.Rows.Add();
                        dataGridView_point.Rows[i].Cells[0].Value = (i+1).ToString();
                        for (int j = 0; j < 4; ++j)
                            dataGridView_point.Rows[i].Cells[j + 1].Value = m_trans.GetData(i, j);
                    }
                    listBox_point_info.Items.Add(string.Format("像素大小：{0}mm", m_trans.m_PixWidth));
                    listBox_point_info.Items.Add(string.Format("X方向最大偏差值：{0}像素", m_trans.m_xMaxOffset));
                    listBox_point_info.Items.Add(string.Format("Y方向最大偏差值：{0}像素", m_trans.m_yMaxOffset));


                    dataGridView_center.Rows.Clear();
                    string[] strItem = { "第一旋转点X座标", "第一旋转点Y座标", "第二旋转点X座标", "第二旋转点Y座标", "旋转角度" };
                    n = m_trans.m_DataCenter.Length;
                    for (int i = 0; i < n; ++i)
                    {
                        dataGridView_center.Rows.Add();
                        dataGridView_center.Rows[i].Cells[0].Value = (i + 1).ToString();

                        dataGridView_center.Rows[i].Cells[1].Value = strItem[i];
                        dataGridView_center.Rows[i].Cells[2].Value = m_trans.m_DataCenter[i].D.ToString();
                    }


                    listBox_center_info.Items.Add(string.Format("旋转中心X坐标：{0}像素", m_trans.m_xCenter));
                    listBox_center_info.Items.Add(string.Format("旋转中心Y坐标：{0}像素", m_trans.m_yCenter));
                   
                }   
            }
        }

        /// <summary>
        /// 保存当前的标定结果到文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_save_Click(object sender, EventArgs e)
        {
            SaveData(textBox_path.Text);
        }

        /// <summary>
        /// 使用当前的座标进行标定并评估转换误差
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_calc_Click(object sender, EventArgs e)
        {
            m_trans.ClearPointData();
            int i = dataGridView_point.Rows.Count - 1;
            for (int k = 0; k < i; ++k)
            {
                if (dataGridView_point.Rows[k].Cells[1].Value != null
                    && dataGridView_point.Rows[k].Cells[2].Value != null
                    && dataGridView_point.Rows[k].Cells[3].Value != null
                    && dataGridView_point.Rows[k].Cells[4].Value != null
                    )
                {

                    double x0 = Convert.ToDouble(dataGridView_point.Rows[k].Cells[1].Value);
                    double y0 = Convert.ToDouble(dataGridView_point.Rows[k].Cells[2].Value);
                    double x1 = Convert.ToDouble(dataGridView_point.Rows[k].Cells[3].Value);
                    double y1 = Convert.ToDouble(dataGridView_point.Rows[k].Cells[4].Value);

                    m_trans.AppendPointData(x0, y0, x1, y1);
                }
            }

            double xx0 = Convert.ToDouble(dataGridView_center.Rows[0].Cells[2].Value);
            double yy0 = Convert.ToDouble(dataGridView_center.Rows[1].Cells[2].Value);
            double xx1 = Convert.ToDouble(dataGridView_center.Rows[2].Cells[2].Value);
            double yy1 = Convert.ToDouble(dataGridView_center.Rows[3].Cells[2].Value);
            double u = Convert.ToDouble(dataGridView_center.Rows[4].Cells[2].Value);

            m_trans.AppendRotateData(xx0, yy0, xx1, yy1, u);

            listBox_point_info.Items.Clear();
            listBox_center_info.Items.Clear();
            if (m_trans.CalcCalib())
            {
                listBox_point_info.Items.Add(string.Format("X方向最大偏差：{0}像素", m_trans.m_xMaxOffset));
                listBox_point_info.Items.Add(string.Format("Y方向最大偏差：{0}像素", m_trans.m_yMaxOffset));
                listBox_point_info.Items.Add(string.Format("像素大小：{0}mm", m_trans.m_PixWidth));

                listBox_center_info.Items.Add(string.Format("旋转中心X坐标：{0}像素", m_trans.m_xCenter));
                listBox_center_info.Items.Add(string.Format("旋转中心Y坐标：{0}像素", m_trans.m_yCenter));
                return;

            }
            listBox_point_info.Items.Add("标定失败, 请检查标定过程及数据是否正常？");
            listBox_center_info.Items.Add("标定失败, 请检查标定过程及数据是否正常？");

        }

        /// <summary>
        /// 按指定文件名保存标定结果
        /// </summary>
        /// <param name="strFile"></param>
        /// <returns></returns>
        private bool SaveData(string strFile)
        {
            if(m_trans.CalcCalib())
                m_trans.SaveCaliData(strFile);
            else
            {
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    System.Windows.Forms.MessageBox.Show("Calibration matrix conversion failed!");
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("标定矩阵转换失败!");
                }
                
                return false;
            }

            return true;
        }

        /// <summary>
        /// 显示另存为对话框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_save_as_Click(object sender, EventArgs e)
        {
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                SaveData(openFileDialog1.FileName);
            }
        }


        /// <summary>
        /// 利用标定转换对某一点进行转换并显示结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_test_Click(object sender, EventArgs e)
        {
            textBox_xPos.Text = string.Empty;
            textBox_yPos.Text = string.Empty;


            double x = Convert.ToDouble(textBox_xPix.Text);
            double y = Convert.ToDouble(textBox_yPix.Text);

            double xOut, yOut;
            if (m_trans.Translate(x, y, out xOut, out yOut))
            {
                textBox_xPos.Text = xOut.ToString();
                textBox_yPos.Text = yOut.ToString();
            }
        }
    }
}
