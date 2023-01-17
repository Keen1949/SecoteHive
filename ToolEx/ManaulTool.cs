using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFrameDll;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using CommonTool;

namespace ToolEx
{
    /// <summary>
    /// 手动控制类
    /// </summary>
    public static class ManaulTool
    {
        /// <summary>
        /// 控件排序
        /// </summary>
        public class ControlSort : IComparer<Control>
        {

            /// <summary>
            /// 定义类型为比较两个对象而实现的方法。  
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public int Compare(Control x, Control y) //为界面的按钮排序
            {
                if (x.Location.X > y.Location.X)
                {
                    return 1;
                }
                else if(x.Location.X == y.Location.X)
                {
                    return x.Location.Y > y.Location.Y ? 1:-1;
                }
                else
                {
                    return -1;
                }
            }
        }
        
        /// <summary>
        /// 通过窗体对象得到相应工站对象
        /// </summary>
        /// <param name="frm">窗体对象</param>
        /// <returns></returns>
        public static StationBase GetStation(Form frm)
        {
            StationBase s =  StationMgr.GetInstance().GetStation(frm);
            if (s == null)
            {
                string str = string.Format("手动调试时{0}找不到对应的站位类可用，配置不匹配",frm.Text);
                MessageBox.Show(str);
                return null;
            }
            return s;
        }

        /// <summary>
        /// 更新内存中的点位参数到站位表格
        /// </summary>
        /// <param name="s">站位对象</param>
        /// <param name="grid">表格控件</param>
        public static void UpdateGrid(StationBase s, DataGridView grid)
        {
            grid.Rows.Clear();

            if (s.m_dicPoint.Count > 0)
            {
                foreach (KeyValuePair<int, PointInfo> kvp in s.m_dicPoint)
                {
                    int k = 0;
                    grid.Rows.Add();
                    int j = grid.Rows.Count - 2;

                    grid.Rows[j].Cells[k++].Value = kvp.Key.ToString();
                    grid.Rows[j].Cells[k++].Value = kvp.Value.strName;

                    for (int i = 0; i < 8;i++)
                    {
                        if (kvp.Value.Pos[i] != -1)
                            grid.Rows[j].Cells[k++].Value = kvp.Value.Pos[i];
                        else
                            k++;
                    }
                }
            }
        }

        /// <summary>
        /// 单轴运动安全委托
        /// </summary>
        /// <param name="nAxis"></param>
        /// <returns></returns>
        public delegate bool IsManulAxisMotionSafeHandler(int nAxis);
        /// <summary>
        /// 单轴运动安全委托事件
        /// </summary>
        public static event IsManulAxisMotionSafeHandler IsManulAxisMotionSafeEvent;
        /// <summary>
        /// 绝对运动
        /// </summary>
        /// <param name="nAxis">轴号</param>
        /// <param name="textBox_tar">目标位置</param>
        /// <param name="textBox_speed">速度</param>
        /// <param name="bPositive">方向 true正  false负</param>
        public static void absMove(int nAxis, TextBox textBox_tar, TextBox textBox_speed, bool bPositive)
        {
            if (IsManulAxisMotionSafeEvent != null)
            {
                if (!IsManulAxisMotionSafeEvent(nAxis))
                {
                    return;
                }   
            };

            string str1 = "轴{0}未配置";
            string str2 = "绝对移动-轴{0},位置{1},速度{2}";
            string str3 = "轴速度设置为0";
            string str4 = "目标位置设置为空";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "Axis {0} is not configured";
                str2 = "Absolute move - axis {0}, position {1}, speed {2}";
                str3 = "Axis speed set to 0";
                str4 = "Target location set to empty";
            }

            AxisCfg cfg = new AxisCfg();

            if (!MotionMgr.GetInstance().GetAxisCfg(nAxis,out cfg))
            {
                MessageBox.Show(string.Format(str1,nAxis));

                return;
            }

            if (textBox_tar.Text != null)
            {
                double fPos = Convert.ToDouble(textBox_tar.Text);
                double fSpeed = Convert.ToDouble(textBox_speed.Text);
                if (fSpeed != 0)
                {
                    //MotionMgr.GetInstance().AbsMove(nAxis, nPos/*bPositive ?  nPos : -nPos*/, nSpeed);

                    MotionMgr.GetInstance().AbsMove(nAxis, fPos * cfg.dbGearRatio, fSpeed, cfg.dbAcc, cfg.dbDec);

                    WarningMgr.GetInstance().Info(string.Format(str2 , nAxis , textBox_tar.Text, textBox_speed.Text));
                }
                else
                {
                    MessageBox.Show(str3);
                }
            }
            else
            {
                MessageBox.Show(str4);
            }
        }

        
        /// <summary>
        /// 相对运动
        /// </summary>
        /// <param name="nAxis">轴号</param>
        /// <param name="textBox_tar">目标位置距离</param>
        /// <param name="textBox_speed">速度</param>
        /// <param name="bPositive">方向 true正  false负</param>
        public static void relMove(int nAxis, TextBox textBox_tar, TextBox textBox_speed, bool bPositive)
        {
            if (IsManulAxisMotionSafeEvent != null)
            {
                if (!IsManulAxisMotionSafeEvent(nAxis))
                {
                    return;
                }
            };

            string str1 = "轴{0}未配置";
            string str2 = "相对移动-轴{0},位置{1},速度{2}，方向{3}";
            string str3 = "轴速度设置为0";
            string str4 = "目标距离设置为空";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "Axis {0} is not configured";
                str2 = "Relative move - axis {0}, position {1}, speed {2}";
                str3 = "Axis speed set to 0";
                str4 = "Target location set to empty";
            }

            AxisCfg cfg = new AxisCfg();

            if (!MotionMgr.GetInstance().GetAxisCfg(nAxis, out cfg))
            {
                MessageBox.Show(string.Format(str1, nAxis));

                return;
            }

            if (textBox_tar.Text != null)
            {
                double fPos = Convert.ToDouble(textBox_tar.Text);
                double fSpeed = Convert.ToDouble(textBox_speed.Text);
                if (fSpeed != 0)
                {
                    //MotionMgr.GetInstance().RelativeMove(nAxis, bPositive ? nPos : -nPos, nSpeed);

                    MotionMgr.GetInstance().RelativeMove(nAxis, bPositive ? fPos * cfg.dbGearRatio : -fPos * cfg.dbGearRatio, fSpeed, cfg.dbAcc, cfg.dbDec);

                    WarningMgr.GetInstance().Info(string.Format(str2, nAxis, textBox_tar.Text, textBox_speed.Text, bPositive));

                }
                else
                {
                    MessageBox.Show(str3);
                }
            }
            else
            {
                MessageBox.Show(str4);
            }
        }
        
        /// <summary>
        /// jog运动
        /// </summary>
        /// <param name="nAxis">轴号</param>
        /// <param name="textBox_tar">目标位置</param>
        /// <param name="textBox_speed">速度</param>
        /// <param name="bPositive">方向 true正  false负</param>
        public static void jogMove(int nAxis, TextBox textBox_tar, TextBox textBox_speed, bool bPositive)
        {
            if (IsManulAxisMotionSafeEvent != null)
            {
                if (!IsManulAxisMotionSafeEvent(nAxis))
                {
                    return;
                }
            };

            string str1 = "JOG移动-轴{0},位置{1},速度{2}";
            string str2 = "轴速度设置为0";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "JOG move - axis {0}, position {1}, speed {2}";
                str2 = "Axis speed set to 0";
            }

            if (textBox_tar.Text != null)
            {
                int nSpeed = Convert.ToInt32(textBox_speed.Text);
                if (nSpeed != 0)
                {
                    MotionMgr.GetInstance().JogMove(nAxis, bPositive, 1, nSpeed);
                    WarningMgr.GetInstance().Info(string.Format(str1, nAxis, textBox_tar.Text, textBox_speed.Text));

                }
                else
                {
                    MessageBox.Show(str2);
                }
            }
        }
        /// <summary>
        /// 单站运动安全委托
        /// </summary>
        /// <param name="staion"></param>
        /// <returns></returns>
        public delegate bool IsManulStaMotionSafeHandler(StationBase staion);
        /// <summary>
        /// 单站运动安全事件
        /// </summary>
        public static event IsManulStaMotionSafeHandler IsManulStaMotionSafeEvent;

        /// <summary>
        /// 根据选中的点位表格单元单轴移动
        /// </summary>
        /// <param name="dataGridView_point">点位表格</param>
        /// <param name="sta">工站对象</param>
        /// <param name="tb">速度</param>
        public static void singleMove(DataGridView dataGridView_point, StationBase sta, TextBox[] tb)
        {
            if (IsManulStaMotionSafeEvent != null)
            {
                if (!IsManulStaMotionSafeEvent(sta))
                {
                    return;
                } 
            }

            int m = dataGridView_point.CurrentCell.ColumnIndex - 2;
            if (m < 0 || m >= sta.AxisCount) return;
            if (dataGridView_point.CurrentCell.Value != null)
            {
                double fPos = Convert.ToDouble(dataGridView_point.CurrentCell.Value.ToString());
                int nAxis = sta.GetAxisNo(m);

                if(tb[m % 4] != null)
                {
                    string str1 = "轴{0}未配置";
                    string str2 = "单轴移动-轴{0},位置{1},速度{2}";
                    string str3 = "当前轴速度设置为0";
                    if (LanguageMgr.GetInstance().LanguageID == 1)
                    {
                        str1 = "Axis {0} is not configured";
                        str2 = "Single move - axis {0}, position {1}, speed {2}";
                        str3 = "Axis speed set to 0";
                    }

                    AxisCfg cfg = new AxisCfg();
                    if (!MotionMgr.GetInstance().GetAxisCfg(nAxis,out cfg))
                    {
                        MessageBox.Show(string.Format(str1, nAxis));

                        return;
                    }

                    double fSpeed = Convert.ToDouble(tb[m % 4].Text);
                    if (fSpeed > 0)
                    {
                        MotionMgr.GetInstance().AbsMove(nAxis, fPos * cfg.dbGearRatio, fSpeed,cfg.dbAcc,cfg.dbDec);
                        WarningMgr.GetInstance().Info(string.Format(str2, nAxis, fPos, fSpeed));

                    }
                    else
                        MessageBox.Show(str3);
                }
               
            }
        }

        /// <summary>
        /// 根据选中的点位表格行全轴移动
        /// </summary>
        /// <param name="dataGridView_point">点位表格</param>
        /// <param name="sta">工站对象</param>
        /// <param name="tb">速度</param>
        public static void allMove(DataGridView dataGridView_point, StationBase sta, TextBox[] tb)
        {
            if (IsManulStaMotionSafeEvent != null)
            {
                if (!IsManulStaMotionSafeEvent(sta))
                {
                    return;
                }
            }

            string str1 = "轴{0}未配置";
            string str2 = "当前轴速度设置为0";
            string str3 = "全轴移动-站位-{0},点位{1}";
            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                str1 = "Axis {0} is not configured";
                str2 = "Axis speed set to 0";
                str3 = "All axis movement - station - {0}, point {1}";
            }

            int n = dataGridView_point.CurrentCell.RowIndex;

            for(int i=0; i<sta.AxisCount; ++i)
            {
                if(dataGridView_point.Rows[n].Cells[i+2].Value != null)
                {
                    double fPos = Convert.ToDouble(dataGridView_point.Rows[n].Cells[i + 2].Value.ToString());
                    int nAxis = sta.GetAxisNo(i);
                    

                    if (tb[i % 4] != null && nAxis > 0)
                    {
                        AxisCfg cfg = new AxisCfg();
                        if (!MotionMgr.GetInstance().GetAxisCfg(nAxis, out cfg))
                        {
                            MessageBox.Show(string.Format(str1, nAxis));

                            return;
                        }

                        double fSpeed = Convert.ToDouble(tb[i % 4].Text);
                        if (fSpeed > 0)
                            MotionMgr.GetInstance().AbsMove(nAxis, fPos * cfg.dbGearRatio, fSpeed,cfg.dbAcc,cfg.dbDec);
                        else
                            MessageBox.Show(str2);
                    }
                }
            }
            WarningMgr.GetInstance().Info(string.Format(str3, sta.Name, n));

        }

        /// <summary>
        /// 根据选中的点位表格更新此轴的位置
        /// </summary>
        /// <param name="dataGridView_point">点位表格</param>
        /// <param name="sta">工站对象</param>
        public static void updateAxisPos(DataGridView dataGridView_point, StationBase sta)
        {
            int m = dataGridView_point.CurrentCell.ColumnIndex - 2;
            if (m >= 0 && m < sta.AxisCount)//zd
            {
                //if (dataGridView_point.CurrentCell.Value != null)
                {
                    int nAxis = sta.GetAxisNo(m);

                    if (nAxis > 0)
                    {
                        double fPos = MotionMgr.GetInstance().GetAixsPos(nAxis);
                        dataGridView_point.CurrentCell.Value = fPos.ToString();
                    }
                    
                }
            }
        }
        
        /// <summary>
        /// 根据选中的点位表格更新此点所有轴的位置
        /// </summary>
        /// <param name="dataGridView_point">点位表格</param>
        /// <param name="sta">工站对象</param>
        public static void updatePointPos(DataGridView dataGridView_point, StationBase sta)
        {
            int m = dataGridView_point.CurrentCell.RowIndex;
            for(int i=0; i<sta.AxisCount; ++i)
            {
                //if(dataGridView_point.Rows[m].Cells[i+2].Value != null)
                {
                    int nAxis = sta.GetAxisNo(i);//m修改为i 20200716 by arvin

                    if (nAxis > 0)
                    {
                        dataGridView_point.Rows[m].Cells[i + 2].Value = MotionMgr.GetInstance().GetAixsPos(nAxis).ToString();
                    }
                        
                }
            }
        }

        /// <summary>
        /// 保存当前工站所有点位到文件
        /// </summary>
        /// <param name="dataGridView_point">点位表格</param>
        /// <param name="sta">工站对象</param>
        public static void SavePoint(DataGridView dataGridView_point, StationBase sta)
        {

            for (int i = 0; i < dataGridView_point.RowCount; ++i)
            {               
                if (dataGridView_point.Rows[i].Cells[0].Value != null)
                {
                    //index
                    int n = Convert.ToInt32(dataGridView_point.Rows[i].Cells[0].Value.ToString());

                    if (!sta.m_dicPoint.ContainsKey(n))
                    {
                        sta.m_dicPoint.Add(n, new PointInfo());
                    }

                    PointInfo point = sta.m_dicPoint[n];

                    if (dataGridView_point.Rows[i].Cells[1].Value != null)
                        point.strName = dataGridView_point.Rows[i].Cells[1].Value.ToString();
                    else
                        point.strName = string.Empty;

                    for (int k = 0; k < 8;k++)
                    {
                        if (dataGridView_point.Rows[i].Cells[2+k].Value != null)
                            point.Pos[k] = Convert.ToDouble(dataGridView_point.Rows[i].Cells[2+k].Value.ToString());
                        else
                            point.Pos[k] = -1;
                    }
                                   
                }
                
            }

            StationMgr.GetInstance().SavePointFile();
        }

        /// <summary>
        /// 通过窗体对象(工站对话框)更新轴状态
        /// </summary>
        /// <param name="frm">窗体对象</param>
        /// <param name="textBox_pos"></param>
        /// <param name="label_state"></param>
        /// <param name="nIndex">页面索引：0/1</param>
        public static void UpdateMotionState(Form frm, TextBox[]textBox_pos, Label[,]label_state,int nIndex = 0)
        {
            StationBase sta = StationMgr.GetInstance().GetStation(frm);
            if (sta != null)
            {
                for (int i = 0; i < 4; ++i)
                {
                    int nStartIndex = i + nIndex * 4;
                    int nAxis = sta.GetAxisNo(nStartIndex);
                    if (nAxis > 0)
                    {
                        textBox_pos[i].Text = MotionMgr.GetInstance().GetAixsPos(nAxis).ToString();
                        int nstate = (int)MotionMgr.GetInstance().GetMotionIoState(nAxis);
               //         if(nstate > 0)
                        {
                            for (int j = 0; j < 8; ++j)
                            {
                                int n = (nstate & (0x1 << j)) > 0 ? 1 : 0;
                                if (label_state[i, j + 1].ImageIndex != n)
                                {
                                    label_state[i, j + 1].ImageIndex = n;
                                }
                            }
                        }

                    }

                }
            }
        }

        /// <summary>
        /// 通过轴号更新相应轴状态
        /// </summary>
        /// <param name="nAxisNo">轴号</param>
        /// <param name="textBox_pos"></param>
        /// <param name="label_state"></param>
        public static void UpdateMotionState(int nAxisNo, TextBox textBox_pos, Label[] label_state)
        {

                textBox_pos.Text = MotionMgr.GetInstance().GetAixsPos(nAxisNo).ToString();
                int nstate = (int)MotionMgr.GetInstance().GetMotionIoState(nAxisNo);
                for (int i = 0; i < 8; ++i)
                {
                    if (label_state[i].ImageIndex != (nstate & (0x1 << i)))
                    {
                        label_state[i].ImageIndex = (nstate & (0x1 << i));
                    }
                }
           
        }

        /// <summary>
        /// 显示系统IO输入输出点名字
        /// </summary>
        /// <param name="btn">按钮对象数组</param>
        /// <param name="strIO">要显示IO点名字索引数组</param>
        /// <param name="bIn">ture为输入,false为输出</param>
        public static void updateIoText(Button[] btn, string[] strIO, bool bIn = true)
        {
            for (int i = 0; i < btn.Length; ++i)
            {
                if (i > strIO.Length-1)
                {
                    btn[i].Visible = false;
                }
                else
                {
                    /*
                    string[] str = strIO[i].Split('.');
                    if (str.Length == 2)
                    {
                        int nCardNo = Convert.ToInt32(str[0]) - 1;
                        int nIndex = Convert.ToInt32(str[1]) - 1;
                        if (nCardNo < IoMgr.GetInstance().CountCard)
                        {
                            if (bIn)
                                btn[i].Text = string.Format("{0}.{1,2} {2}", str[0], str[1],
                                            IoMgr.GetInstance().m_listCard.ElementAt(nCardNo).m_strArrayIn[nIndex]);
                            else
                                btn[i].Text = string.Format("{0}.{1,2} {2}", str[0], str[1],
                                        IoMgr.GetInstance().m_listCard.ElementAt(nCardNo).m_strArrayOut[nIndex]);
                            btn[i].Visible = true;
                        }
                    }
                    else
                        btn[i].Visible = false;
                    */

                    long num;
                    int nCardNo, nIndex;
                    if (bIn)
                    {
                        if (IoMgr.GetInstance().m_dicIn.TryGetValue(strIO[i], out num))
                        {
                            nCardNo = (int)(num >> 8);
                            nIndex = (int)(num & 0xFF);
                        }
                        else
                        {
                            btn[i].Visible = false;

                            continue;
                        }

                    }
                    else
                    {
                        if (IoMgr.GetInstance().m_dicOut.TryGetValue(strIO[i], out num))
                        {
                            nCardNo = (int)(num >> 8);
                            nIndex = (int)(num & 0xFF);
                        }
                        else
                        {
                            btn[i].Visible = false;

                            continue;
                        }
                    }
                    
                    if (nCardNo-1 < IoMgr.GetInstance().CountCard)
                    {
                        string strIoName = strIO[i];
                        if (LanguageMgr.GetInstance().LanguageID == 1)
                        {
                            if (bIn)
                            {
                                strIoName = IoMgr.GetInstance().GetIoInTranslate(strIoName);
                            }
                            else
                            {
                                strIoName = IoMgr.GetInstance().GetIoOutTranslate(strIoName);
                            }
                        }
                        btn[i].Text = string.Format("{0}.{1,2} {2}", nCardNo, nIndex,strIoName);
                        btn[i].Visible = true;
                    }

                }
            }
        }

        /// <summary>
        /// 更新IO的状态
        /// </summary>
        /// <param name="btn">按钮对象数组</param>
        /// <param name="strIO">字符串数组,包含卡号和位数</param>
        /// <param name="bIn">ture为输入,false为输出</param>
        public static void updateIoState(Button[] btn, string[] strIO, bool bIn = true)
        {
            for (int i = 0; i < strIO.Length; ++i)
            {
                if (i > btn.Length - 1)
                    break;

                /*
                string[] str = strIO[i].Split('.');
                if (str.Length == 2)
                {
                    int ncard = Convert.ToInt32(str[0]);
                    int nIndex = Convert.ToInt32(str[1]);
                    if (ncard < IoMgr.GetInstance().CountCard + 1)
                    {
                        bool bBit = bIn ? IoMgr.GetInstance().ReadIoInBit(ncard, nIndex)
                                : IoMgr.GetInstance().ReadIoOutBit(ncard, nIndex);
                        if (btn[i].ImageIndex != Convert.ToInt32(bBit))
                        {
                            btn[i].ImageIndex = Convert.ToInt32(bBit);
                        }
                    }
                }
                */

                int ncard, nIndex;
                long num;

                if(bIn)
                {
                    IoMgr.GetInstance().m_dicIn.TryGetValue(strIO[i], out num);

                    ncard = (int)(num >> 8);
                    nIndex = (int)(num & 0xFF);
                }
                else
                {
                    IoMgr.GetInstance().m_dicOut.TryGetValue(strIO[i], out num);

                    ncard = (int)(num >> 8);
                    nIndex = (int)(num & 0xFF);
                }

                if (ncard < IoMgr.GetInstance().CountCard + 1 && ncard > 0)
                {
                    bool bBit = bIn ? IoMgr.GetInstance().ReadIoInBit(ncard, nIndex)
                            : IoMgr.GetInstance().ReadIoOutBit(ncard, nIndex);
                    if (btn[i].ImageIndex != Convert.ToInt32(bBit))
                    {
                        btn[i].ImageIndex = Convert.ToInt32(bBit);
                    }
                }

            }
        }
        /// <summary>
        /// IO操作安全委托
        /// </summary>
        /// <param name="strIoName"></param>
        /// <returns></returns>
        public delegate bool IsManulIoSafeHandler(string strIoName);
        /// <summary>
        /// IO操作安全事件
        /// </summary>
        public static event IsManulIoSafeHandler IsManulIoSafeEvent;

        /// <summary>
        /// 单击输出按钮事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">附带数据的对象</param>
        public static void Form_IO_Out_Click(object sender, EventArgs e)
        {
            string str = ((Button)sender).Text;
            Regex rg = new Regex(@"[0-9]+");  //正则表达式

            Match m = rg.Match(str);

            if (m.Length == 0)
                return;

            int nCard = Convert.ToInt32(m.Value);
            int nIndex = Convert.ToInt32(m.NextMatch().Value);
            if (nCard > 0 && (nCard - 1 < IoMgr.GetInstance().CountCard))
            {

                if (IsManulIoSafeEvent != null)
                {
                    string strIoName = IoMgr.GetInstance().GetIoOutName(nCard, nIndex);
                    if (!IsManulIoSafeEvent(strIoName))
                    {
                        return;
                    }
                    
                }

                if (nIndex > 0 && (nIndex - 1 < IoMgr.GetInstance().m_listCard.ElementAt(nCard-1).m_strArrayOut.Length))
                {

                    IoMgr.GetInstance().WriteIoBit(nCard, nIndex, !IoMgr.GetInstance().ReadIoOutBit(nCard, nIndex));
                }
            }

            if (LanguageMgr.GetInstance().LanguageID == 1)
            {
                WarningMgr.GetInstance().Info("IO manual trigger-" + str);
            }
            else
            {
                WarningMgr.GetInstance().Info("IO手动触发-" + str);
            }

        }

        /// <summary>
        /// 气缸安全委托
        /// </summary>
        /// <param name="cyl"></param>
        /// <returns></returns>
        public delegate bool IsCylinderSafeHandler(Cylinder cyl);
        /// <summary>
        /// 气缸安全事件
        /// </summary>
        public static event IsCylinderSafeHandler IsCylinderSafeEvent;
        /// <summary>
        /// 判断是否满足气缸动作条件
        /// </summary>
        /// <param name="cyl">气缸</param>
        /// <returns></returns>
        public static bool IsCylinderSafe(Cylinder cyl)
        {
            //判断是否满足气缸动作条件，根据实际情况编写
            if (IsCylinderSafeEvent != null)
            {
                return IsCylinderSafeEvent(cyl);
            }
            return true;
        }

    }
}
