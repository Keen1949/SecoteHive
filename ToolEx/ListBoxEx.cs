using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToolEx
{
    /// <summary>
    /// 扩展ListBox的颜色信息
    /// </summary>
    class ColorInfo
    {
        public Color BackColor { get; set; }
        public Color TextColor { get; set; }
        public Color FlashColor { get; set; }
        public bool EnableFlash { get; set; }

        public ColorInfo(Color bkColor,Color txtColor,bool bFlash = false)
        {
            BackColor = bkColor;
            TextColor = txtColor;
            EnableFlash = bFlash;
            FlashColor = SystemColors.Window;
        }
    }
    /// <summary>
    /// 扩展ListBox类
    /// </summary>
    public class ListBoxEx : ListBox
    {
        private List<ColorInfo> m_listColorInfos = new List<ColorInfo>();
        private object m_lock = new object();
        private Timer m_timer = new Timer();

        /// <summary>
        /// 构造函数
        /// </summary>
        public ListBoxEx() 
        {
            this.DrawMode = DrawMode.OwnerDrawVariable;
            this.IntegralHeight = true;
            this.HorizontalScrollbar = true;

            m_timer.Interval = 500;
            m_timer.Tick += OnTimerTick;
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            m_timer.Enabled = false;
            bool bFlash = false;
            lock(m_lock)
            {
                for (int i = 0; i < m_listColorInfos.Count; i++)
                {
                    if (m_listColorInfos[i].EnableFlash)
                    {
                        bFlash = true;
                        Color temp = m_listColorInfos[i].BackColor;

                        m_listColorInfos[i].BackColor = m_listColorInfos[i].FlashColor;

                        m_listColorInfos[i].FlashColor = temp;
                    }
                }
            } 

            if (bFlash)
            {
                m_timer.Enabled = true;

                this.Invalidate();
            }
        }

        /// <summary>
        /// 数量
        /// </summary>
        public int Count
        {
            get
            {
                return this.Items.Count;
            }
        }

        /// <summary>
        /// 追加记录
        /// </summary>
        /// <param name="strText">文本</param>
        /// <param name="bkColor">背景颜色</param>
        /// <param name="textColor">文本颜色</param>
        /// <param name="bFlash">是否闪烁</param>
        public int Append(string strText, Color bkColor, Color textColor, bool bFlash = false)
        {
            int nItem;
            lock (m_lock)
            {
                nItem = this.Items.Add(strText);

                //防止用ListBox的方法加入了Item而没有颜色信息
                while (nItem > m_listColorInfos.Count)
                {
                    m_listColorInfos.Add(new ColorInfo(this.BackColor, this.ForeColor, false));
                }

                m_listColorInfos.Add(new ColorInfo(bkColor, textColor, bFlash));

                this.Invalidate();

                if (bFlash)
                {
                    m_timer.Enabled = true;
                }
            }
            return nItem;
        }

        /// <summary>
        /// 重绘
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            int nItem = e.Index;
            if (nItem >= 0 && this.Items.Count > 0)
            {
                //绘制背景
                Color colorBk = GetBkColor(nItem);
                Brush brushBk = new SolidBrush(colorBk);
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    e.DrawBackground();
                }
                else
                {
                    e.Graphics.FillRectangle(brushBk, e.Bounds);
                }

                //焦点框
                e.DrawFocusRectangle();

                //绘制文本
                Color textColor = GetTextColor(nItem);
                e.Graphics.DrawString(this.Items[nItem].ToString(), e.Font, new SolidBrush(textColor), new Rectangle(e.Bounds.X,e.Bounds.Y,e.Bounds.Width + 5,e.Bounds.Height + 5));
            }

            base.OnDrawItem(e);

        }

        /// <summary>
        /// 重绘
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMeasureItem(MeasureItemEventArgs e)
        {
            if (this.Items.Count > 0 && e.Index >= 0)
            {
                string strText = this.Items[e.Index].ToString();

                SizeF size = e.Graphics.MeasureString(strText, this.Font);

                if (size.Width == 0 || size.Height == 0)
                {
                    e.ItemHeight = 50;
                    e.ItemWidth = 100;
                    this.ItemHeight = 50;

                    return;
                }

                if (this.HorizontalExtent < size.Width)
                {
                    this.HorizontalExtent = (int)size.Width;
                }

                e.ItemHeight = (int)size.Height;
                e.ItemWidth = (int)size.Width;
                this.ItemHeight = (int)size.Height;
            }
            
        }

        /// <summary>
        /// 设置背景颜色
        /// </summary>
        /// <param name="nItem">索引号</param>
        /// <param name="color">颜色</param>
        public void SetBkColor(int nItem, Color color)
        {
            if(nItem < m_listColorInfos.Count)
            {
                m_listColorInfos[nItem].BackColor = color;

                this.Invalidate();
            } 
        }

        /// <summary>
        /// 获取背景颜色
        /// </summary>
        /// <param name="nItem">索引号</param>
        /// <returns>颜色</returns>
        public Color GetBkColor(int nItem)
        {
            Color color = this.BackColor;

            if (nItem < m_listColorInfos.Count)
            {
                color = m_listColorInfos[nItem].BackColor;
            }

            return color;
        }

        /// <summary>
        /// 设置文本颜色
        /// </summary>
        /// <param name="nItem">索引号</param>
        /// <param name="color">颜色</param>
        public void SetTextColor(int nItem,Color color)
        {
            if (nItem < m_listColorInfos.Count)
            {
                m_listColorInfos[nItem].TextColor = color;

                this.Invalidate();
            }
        }

        /// <summary>
        /// 获取文本颜色
        /// </summary>
        /// <param name="nItem">索引号</param>
        /// <returns></returns>
        public Color GetTextColor(int nItem)
        {
            Color color = this.ForeColor;

            if (nItem < m_listColorInfos.Count)
            {
                color = m_listColorInfos[nItem].TextColor;
            }

            return color;
        }

        /// <summary>
        /// 重置背景颜色和文本颜色
        /// </summary>
        public void Clear()
        {
            lock(m_lock)
            {
                this.Items.Clear();
                m_listColorInfos.Clear();

                m_timer.Enabled = false;
            }
            
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            lock(m_lock)
            {
                if (index < m_listColorInfos.Count)
                {
                    this.Items.RemoveAt(index);
                    m_listColorInfos.RemoveAt(index);
                }
            } 
        }
    }
}
