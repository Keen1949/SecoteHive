using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonTool;

namespace ToolEx
{
    /// <summary>
    /// 气缸控制控件
    /// </summary>
    public partial class CylinderCtrl : UserControl
    {
        private Cylinder m_cylinder;

        /// <summary>
        /// 委托
        /// </summary>
        /// <param name="cyl"></param>
        /// <returns></returns>
        public delegate bool IsCylinderSafeHandler(Cylinder cyl);

        /// <summary>
        /// 事件
        /// </summary>
        public event IsCylinderSafeHandler IsCylinderSafeEvent;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CylinderCtrl()
        {
            InitializeComponent();

            InitAnchor();
        }

        private void InitAnchor()
        {
            button_out_o.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            button_back_o.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;

            button_out_i.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button_back_i.Anchor = AnchorStyles.Top | AnchorStyles.Right;

        }

        /// <summary>
        /// 气缸对象
        /// </summary>
        public Cylinder CylinderObject
        {
            get
            {
                return m_cylinder;
            }

            set
            {
                m_cylinder = value;
            }
        }

        private void CylinderCtrl_Load(object sender, EventArgs e)
        {
            OnLanguageChangeEvent(LanguageMgr.GetInstance().Language, true);
            LanguageMgr.GetInstance().LanguageChangeEvent += OnLanguageChangeEvent;
        }

        private void OnLanguageChangeEvent(string strLanguage, bool bChange = true)
        {
            if (m_cylinder != null)
            {
                string strCylName = m_cylinder.m_strName;
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    strCylName = CylinderMgr.GetInstance().GetCylTranslate(strCylName);
                }

                groupBox_Name.Text = strCylName;

                ManaulTool.updateIoText(new Button[] { button_out_o, button_back_o },
                    m_cylinder.m_strIoOuts, false);

                ManaulTool.updateIoText(new Button[] { button_out_i, button_back_i },
                    m_cylinder.m_strIoIns);

                timer1.Enabled = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ManaulTool.updateIoState(new Button[] { button_out_o, button_back_o },
                m_cylinder.m_strIoOuts, false);

            ManaulTool.updateIoState(new Button[] { button_out_i, button_back_i },
                m_cylinder.m_strIoIns);
        }

        private void button_o_Click(object sender, EventArgs e)
        {
            if (m_cylinder != null)
            {
                if (IsCylinderSafeEvent != null)
                {
                    if (IsCylinderSafeEvent(m_cylinder))
                    {
                        if (m_cylinder.IsOut)
                        {
                            m_cylinder.CylBack(null, null);
                        }
                        else
                        {
                            m_cylinder.CylOut(null, null);
                        }
                        
                    }
                }
                
            }
        }
    }
}

