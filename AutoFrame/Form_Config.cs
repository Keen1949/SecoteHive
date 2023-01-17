using AutoFrameDll;
using CommonTool;
using MotionIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ToolEx;

namespace AutoFrame
{
    public partial class Form_Config : Form
    {
        private ComboBox comboBox_HomeMode = new ComboBox();
        private ComboBox comboBox_Enable = new ComboBox();

        public Form_Config()
        {
            InitializeComponent();
        }

        enum Heads
        {
            卡类型,
            名称,
            轴号,
            单位脉冲数,
            回零方式,
            回零最小速度,
            回零最大速度,
            回零加速时间,
            回零减速时间,
            最大运行速度,
            加速时间,
            减速时间,
            平滑系数,
            到位误差,
            软正限位启用,
            软正限位,
            软负限位启用,
            软负限位,
        }

        private void Form_Config_Load(object sender, EventArgs e)
        {
            InitAxisCfg();

            //初始化回原点模式
            comboBox_HomeMode.Items.Clear();

            foreach (var item in Enum.GetValues(typeof(HomeMode)))
            {
                comboBox_HomeMode.Items.Add(string.Format("{0} - {1}", (int)item, item));
            }

            comboBox_HomeMode.Visible = false;

            comboBox_HomeMode.SelectedIndexChanged += OnSelectedIndexChanged;
            comboBox_HomeMode.LostFocus += OnSelectedIndexChanged;

            comboBox_Enable.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_Enable.Items.Clear();
            comboBox_Enable.Items.Add(true);
            comboBox_Enable.Items.Add(false);

            comboBox_Enable.Visible = false;

            comboBox_Enable.SelectedIndexChanged += OnSelectedIndexChanged;
            comboBox_Enable.LostFocus += OnSelectedIndexChanged;

            dataGridView_Axis.Controls.Add(comboBox_HomeMode);
            dataGridView_Axis.Controls.Add(comboBox_Enable);

            //增加权限等级变更通知
            OnModeChanged();
            Security.ModeChangedEvent += OnModeChanged;

            OnLanguageChangeEvent(LanguageMgr.GetInstance().Language, true);

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

        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;

            if (comboBox == comboBox_HomeMode)
            {
                Regex rg = new Regex(@"[0-9]+");  //正则表达式

                Match m = rg.Match(comboBox_HomeMode.Text);

                if (m.Length == 0)
                {
                    return;
                }

                int nHomeMode = Convert.ToInt32(m.Value);
                dataGridView_Axis.CurrentCell.Value = m.Value;

                string strFile = Path.Combine(System.Environment.CurrentDirectory, "res", nHomeMode + ".png");

                try
                {
                    FileInfo fi = new FileInfo(strFile);
                    if (fi.Exists)
                    {
                        pictureBox_HomeMode.Image = Image.FromFile(strFile);
                    }
                    else
                    {
                        pictureBox_HomeMode.Image = null;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                label_Tips.Text = LanguageMgr.GetInstance().GetString(this.GetType().Namespace, this.GetType().Name, "Home" + nHomeMode, "");

                comboBox_HomeMode.Visible = false;
            }
            else if (comboBox == comboBox_Enable)
            {
                dataGridView_Axis.CurrentCell.Value = comboBox_Enable.Text;
            }
        }

        /// <summary>
        /// 权限变更响应
        /// </summary>
        private void OnModeChanged()
        {
            switch (Security.GetUserMode())
            {
                case UserMode.Administrator:
                case UserMode.Engineer:
                    for(int i = (int)Heads.单位脉冲数; i < dataGridView_Axis.Columns.Count;i++)
                    {
                        dataGridView_Axis.Columns[i].ReadOnly = false;
                    }

                    break;

                case UserMode.Adjustor:
                    for (int i = (int)Heads.单位脉冲数; i <=  (int)Heads.回零减速时间; i++)
                    {
                        dataGridView_Axis.Columns[i].ReadOnly = true;
                    }

                    for (int i = (int)Heads.最大运行速度; i <= (int)Heads.到位误差; i++)
                    {
                        dataGridView_Axis.Columns[i].ReadOnly = false;
                    }

                    for (int i = (int)Heads.软正限位启用; i <= (int)Heads.软负限位; i++)
                    {
                        dataGridView_Axis.Columns[i].ReadOnly = true;
                    }

                    break;

                default:
                    for (int i = 0; i < dataGridView_Axis.Columns.Count; i++)
                    {
                        dataGridView_Axis.Columns[i].ReadOnly = true;
                    }
                    break;
            }
        }

        /// <summary>
        /// 初始化轴配置
        /// </summary>
        private void InitAxisCfg()
        {
            dataGridView_Axis.Rows.Clear();
            dataGridView_Axis.Columns.Clear();
            dataGridView_Axis.RowsDefaultCellStyle.BackColor = Color.Ivory;
            dataGridView_Axis.AlternatingRowsDefaultCellStyle.BackColor = Color.PaleTurquoise;

            foreach (string str in Enum.GetNames(typeof(Heads)))
            {
                int col = dataGridView_Axis.Columns.Add(str, str);
                dataGridView_Axis.Columns[col].SortMode = DataGridViewColumnSortMode.NotSortable;

                if (col == (int)Heads.回零方式)
                {
                    dataGridView_Axis.Columns[col].Width = 100;
                }
                else
                {
                    dataGridView_Axis.Columns[col].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }

                //软限位暂时不用，隐藏起来
                //if (col >= (int)Heads.软正限位启用)
                //{
                //    dataGridView_Axis.Columns[col].Visible = false;
                //}
                
            }  

            dataGridView_Axis.Columns[0].ReadOnly = true;
            dataGridView_Axis.Columns[1].ReadOnly = true;
            dataGridView_Axis.Columns[2].ReadOnly = true;

            foreach (var item in MotionMgr.GetInstance().m_listCard)
            {
                for (int i = item.GetMinAxisNo(); i <= item.GetMaxAxisNo(); i++)
                {
                    int row = dataGridView_Axis.Rows.Add();
                    int col = 0;

                    string strAxisName, strDesc;
                    StationBase sta = StationMgr.GetInstance().GetStation(i, out strAxisName);
                    if (sta != null)
                    {
                        strDesc = sta.Name + strAxisName;
                    }
                    else
                    {
                        strDesc = string.Format("{0} axis no assignment", i);
                    }

                    dataGridView_Axis.Rows[row].Cells[col++].Value = item.GetCardName();
                    dataGridView_Axis.Rows[row].Cells[col++].Value = strDesc;
                    dataGridView_Axis.Rows[row].Cells[col++].Value = i;

                    AxisCfg cfg;
                    if (!MotionMgr.GetInstance().GetAxisCfg(i, out cfg))
                    {
                        cfg.dbGearRatio = 1.0;
                        cfg.nHomeMode = (int)HomeMode.ORG_N;
                        cfg.dbHomeSpeedMin = 1000;
                        cfg.dbHomeSpeedMax = 5000;
                        cfg.dbHomeAcc = 0.1;
                        cfg.dbHomeDec = 0.1;
                        cfg.dbSpeedMax = 20000;
                        cfg.dbAcc = 0.1;
                        cfg.dbDec = 0.1;
                        cfg.dbSFac = 0;
                        cfg.nInPosError = 1000;
                        cfg.bEnableSPEL = false;
                        cfg.dbSPELPos = 1.0;
                        cfg.bEnableSMEL = false;
                        cfg.dbSMELPos = 1.0;

                        MotionMgr.GetInstance().AppendAxisCfg(i, cfg);
                    }

                    dataGridView_Axis.Rows[row].Cells[col++].Value = cfg.dbGearRatio;
                    dataGridView_Axis.Rows[row].Cells[col++].Value = cfg.nHomeMode;
                    dataGridView_Axis.Rows[row].Cells[col++].Value = cfg.dbHomeSpeedMin;
                    dataGridView_Axis.Rows[row].Cells[col++].Value = cfg.dbHomeSpeedMax;
                    dataGridView_Axis.Rows[row].Cells[col++].Value = cfg.dbHomeAcc;
                    dataGridView_Axis.Rows[row].Cells[col++].Value = cfg.dbHomeDec;
                    dataGridView_Axis.Rows[row].Cells[col++].Value = cfg.dbSpeedMax;
                    dataGridView_Axis.Rows[row].Cells[col++].Value = cfg.dbAcc;
                    dataGridView_Axis.Rows[row].Cells[col++].Value = cfg.dbDec;
                    dataGridView_Axis.Rows[row].Cells[col++].Value = cfg.dbSFac;
                    dataGridView_Axis.Rows[row].Cells[col++].Value = cfg.nInPosError;
                    dataGridView_Axis.Rows[row].Cells[col++].Value = cfg.bEnableSPEL;
                    dataGridView_Axis.Rows[row].Cells[col++].Value = cfg.dbSPELPos;
                    dataGridView_Axis.Rows[row].Cells[col++].Value = cfg.bEnableSMEL;
                    dataGridView_Axis.Rows[row].Cells[col++].Value = cfg.dbSMELPos;
                }
            }
        }

        private void button_Apply_Click(object sender, EventArgs e)
        {
            string str1 = "是否应用此配置?";
            string str2 = "提示";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "Do you want to apply this config?";
                str2 = "Tips";
            }
            if (MessageBox.Show(str1,str2,MessageBoxButtons.YesNo,MessageBoxIcon.Information,MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                foreach (DataGridViewRow row in dataGridView_Axis.Rows)
                {
                    int col = (int)Heads.轴号;
                    int nAxisNo = Convert.ToInt32(row.Cells[col++].Value);

                    AxisCfg cfg;
                    MotionMgr.GetInstance().GetAxisCfg(nAxisNo, out cfg);

                    cfg.dbGearRatio = Convert.ToDouble(row.Cells[col++].Value);
                    cfg.nHomeMode = Convert.ToInt32(row.Cells[col++].Value);
                    cfg.dbHomeSpeedMin = Convert.ToDouble(row.Cells[col++].Value);
                    cfg.dbHomeSpeedMax = Convert.ToDouble(row.Cells[col++].Value);
                    cfg.dbHomeAcc = Convert.ToDouble(row.Cells[col++].Value);
                    cfg.dbHomeDec = Convert.ToDouble(row.Cells[col++].Value);
                    cfg.dbSpeedMax = Convert.ToDouble(row.Cells[col++].Value);
                    cfg.dbAcc = Convert.ToDouble(row.Cells[col++].Value);
                    cfg.dbDec = Convert.ToDouble(row.Cells[col++].Value);
                    cfg.dbSFac = Convert.ToDouble(row.Cells[col++].Value);
                    cfg.nInPosError = Convert.ToInt32(row.Cells[col++].Value);
                    cfg.bEnableSPEL = Convert.ToBoolean(row.Cells[col++].Value);
                    cfg.dbSPELPos = Convert.ToDouble(row.Cells[col++].Value);
                    cfg.bEnableSMEL = Convert.ToBoolean(row.Cells[col++].Value);
                    cfg.dbSMELPos = Convert.ToDouble(row.Cells[col++].Value);

                    MotionMgr.GetInstance().AppendAxisCfg(nAxisNo, cfg);
                }
            }
            
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            string str1 = "是否保存此配置?";
            string str2 = "提示";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "Do you want to save this config?";
                str2 = "Tips";
            }
            if (MessageBox.Show(str1, str2, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                foreach (DataGridViewRow row in dataGridView_Axis.Rows)
                {
                    int col = (int)Heads.轴号;
                    int nAxisNo = Convert.ToInt32(row.Cells[col++].Value);

                    AxisCfg cfg;
                    MotionMgr.GetInstance().GetAxisCfg(nAxisNo, out cfg);

                    cfg.dbGearRatio = Convert.ToDouble(row.Cells[col++].Value);
                    cfg.nHomeMode = Convert.ToInt32(row.Cells[col++].Value);
                    cfg.dbHomeSpeedMin = Convert.ToDouble(row.Cells[col++].Value);
                    cfg.dbHomeSpeedMax = Convert.ToDouble(row.Cells[col++].Value);
                    cfg.dbHomeAcc = Convert.ToDouble(row.Cells[col++].Value);
                    cfg.dbHomeDec = Convert.ToDouble(row.Cells[col++].Value);
                    cfg.dbSpeedMax = Convert.ToDouble(row.Cells[col++].Value);
                    cfg.dbAcc = Convert.ToDouble(row.Cells[col++].Value);
                    cfg.dbDec = Convert.ToDouble(row.Cells[col++].Value);
                    cfg.dbSFac = Convert.ToDouble(row.Cells[col++].Value);
                    cfg.nInPosError= Convert.ToInt32(row.Cells[col++].Value);
                    cfg.bEnableSPEL = Convert.ToBoolean(row.Cells[col++].Value);
                    cfg.dbSPELPos = Convert.ToDouble(row.Cells[col++].Value);
                    cfg.bEnableSMEL = Convert.ToBoolean(row.Cells[col++].Value);
                    cfg.dbSMELPos = Convert.ToDouble(row.Cells[col++].Value);

                    MotionMgr.GetInstance().AppendAxisCfg(nAxisNo, cfg);
                }

                MotionMgr.GetInstance().SaveAxisCfg();
            }
            
        }

        private void dataGridView_Axis_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dataGridView_Axis.CurrentCell == null)
            {
                comboBox_HomeMode.Visible = false;
                comboBox_Enable.Visible = false;
                return;
            }

            int row = dataGridView_Axis.CurrentCell.RowIndex;
            int col = dataGridView_Axis.CurrentCell.ColumnIndex;

            if (col == (int)Heads.回零方式)
            {
                Rectangle rect = dataGridView_Axis.GetCellDisplayRectangle(col,
                    row, true);

                comboBox_HomeMode.Top = rect.Top;
                comboBox_HomeMode.Left = rect.Left;
                comboBox_HomeMode.Width = rect.Width;
                comboBox_HomeMode.Height = rect.Height;

                if (dataGridView_Axis.CurrentCell.Value != null)
                {
                    comboBox_HomeMode.Text = dataGridView_Axis.CurrentCell.Value.ToString();

                    OnSelectedIndexChanged(null, null);
                }
                else
                {
                    comboBox_HomeMode.SelectedIndex = -1;
                }

                if (!dataGridView_Axis.Columns[col].ReadOnly)
                {
                    comboBox_HomeMode.Visible = true;
                }
                else
                {
                    comboBox_HomeMode.Visible = false;
                }
                
            }
            else if (col == (int)Heads.软正限位启用 ||
                col == (int)Heads.软负限位启用)
            {
                Rectangle rect = dataGridView_Axis.GetCellDisplayRectangle(col,
                    row, true);

                comboBox_Enable.Top = rect.Top;
                comboBox_Enable.Left = rect.Left;
                comboBox_Enable.Width = rect.Width;
                comboBox_Enable.Height = rect.Height;

                if (dataGridView_Axis.CurrentCell.Value != null)
                {
                    comboBox_Enable.Text = dataGridView_Axis.CurrentCell.Value.ToString();
                }
                else
                {
                    comboBox_Enable.SelectedIndex = -1;
                }

                if (!dataGridView_Axis.Columns[col].ReadOnly)
                {
                    comboBox_Enable.Visible = true;
                }
                else
                {
                    comboBox_Enable.Visible = false;
                }
            }
            else
            {
                comboBox_HomeMode.Visible = false;
                comboBox_Enable.Visible = false;
            }
        }

        private void dataGridView_Axis_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            comboBox_HomeMode.Visible = false;
            comboBox_Enable.Visible = false;
        }

        private void dataGridView_Axis_Scroll(object sender, ScrollEventArgs e)
        {
            comboBox_HomeMode.Visible = false;
            comboBox_Enable.Visible = false;
        }

        private void dataGridView_Axis_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            string str1 = "取值超出范围(0~1)";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "Value out of range (0 ~ 1)";
            }

            if (e.ColumnIndex == 12)
            {
                try
                {
                    //平滑系数
                    double dbCurValue = Convert.ToDouble(dataGridView_Axis.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);

                    if (dbCurValue < 0 || dbCurValue > 1)
                    {
                        MessageBox.Show(str1);

                        //还原成原值
                        int nAxis = Convert.ToInt32(dataGridView_Axis.Rows[e.RowIndex].Cells[2].Value);
                        AxisCfg cfg;
                        if (MotionMgr.GetInstance().GetAxisCfg(nAxis, out cfg))
                        {
                            dataGridView_Axis.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = cfg.dbSFac.ToString();
                        }
                    }
                }
                catch
                {
                    //还原成原值
                    int nAxis = Convert.ToInt32(dataGridView_Axis.Rows[e.RowIndex].Cells[2].Value);
                    AxisCfg cfg;
                    if (MotionMgr.GetInstance().GetAxisCfg(nAxis, out cfg))
                    {
                        dataGridView_Axis.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = cfg.dbSFac.ToString();
                    }
                }
            }
        }
    }
}
