using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTool;
using gts;


namespace MotionIO
{
    /// <summary>
    /// 固高卡的IO类
    /// </summary>
    public class IoCtrl_GTS : IoCtrl
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nIndex"></param>
        /// <param name="nCardNo"></param>
        public IoCtrl_GTS(int nIndex, int nCardNo)
            : base(nIndex, nCardNo)
        {
            m_bEnable = false;
            m_strCardName = "GTS";
            m_strArrayIn = new string[16];
            m_strArrayOut = new string[16];
        }

        /// <summary>
        /// 初始化函数
        /// </summary>
        /// <returns></returns>
        public override bool Init()
        {
            short nRtn = 0;          
            if (0 == mc.GT_Open((short)m_nCardNo, 0, 1))
            {
                m_bEnable = true;
                return true;
            }
           else
            {
                string str1 = "第{0}张运动控制卡GTS初始化失败! result = {1}";
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    str1 = "GTS initialization of the {0} motion control card failed! Result = {1}";
                }
                m_bEnable = false;
                //WarningMgr.GetInstance().Error(string.Format("30100,ERR-XYT, 第{0}张运动控制卡GTS初始化失败! result = {1}", m_nCardNo, nRtn));
                WarningMgr.GetInstance().Error(ErrorType.Err_IO_Init,m_nCardNo.ToString(),
                    string.Format(str1, m_nCardNo, nRtn));

                return false;
            }
        }

        /// <summary>
        /// 反初始化
        /// </summary>
        public override void DeInit()
        {
            mc.GT_Close((short)m_nCardNo);
        }

        /// <summary>
        /// 读取一组IO输入数据
        /// </summary>
        /// <param name="nData"></param>
        /// <returns></returns>
        public override bool ReadIOIn(ref int nData)
        {
            short nRtn = 0;
            nRtn = mc.GT_GetDi((short)m_nCardNo, mc.MC_GPI, out nData);
            if (0 == nRtn)
            {
                nData = ~nData;
                if (m_nInData != nData)
                {
                    m_bDataChange = true;

                    m_nInData = nData;
                }
                else
                {
                    m_bDataChange = false;
                }

                return true;
            }
            else
            {
                if (m_bEnable)
                {
                    string str1 = "第{0}张IO卡GTS ReadIOIn Error,ErrorCode = {1}";
                    if (LanguageMgr.GetInstance().LanguageID == 1)
                    {
                        str1 = "The {0} IO card GTS ReadIOIn Error,ErrorCode = {1}";
                    }
                    //WarningMgr.GetInstance().Error(string.Format("20101,ERR-XYT,第{0}张IO卡GTS ReadIOIn Error,ErrorCode = {1}", m_nCardNo, nRtn));
                    WarningMgr.GetInstance().Error(ErrorType.Err_IO_Read_In,m_nCardNo.ToString(),
                        string.Format(str1, m_nCardNo, nRtn));

                }
                return false;
            }
        }

        /// <summary>
        /// 读取一个IO输入点
        /// </summary>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        public override bool ReadIoInBit(int nIndex)
        {
            int nData = 0;
            if (0 == mc.GT_GetDi((short)m_nCardNo, mc.MC_GPI, out nData))
            {
                nData = ~nData;
                return 0 != (nData & (1 << (nIndex - 1)));
            }
            return false;
        }

        /// <summary>
        /// 读取一组IO输出数据
        /// </summary>
        /// <param name="nData"></param>
        /// <returns></returns>
        public override bool ReadIOOut(ref int nData)
        {
            short nRtn = 0;
            nRtn = mc.GT_GetDo((short)m_nCardNo, mc.MC_GPO, out nData);
            if (0 == nRtn)
            {
                nData = ~nData;
                m_nOutData = nData;
                return true;
            }
            else
            {
                if (m_bEnable)
                {
                    string str1 = "第{0}张IO卡GTS ReadIOOut Error,result is {1}";
                    if (LanguageMgr.GetInstance().LanguageID == 1)
                    {
                        str1 = "The {0} IO card GTS ReadIOOut Error,result is {1}";
                    }
                    //WarningMgr.GetInstance().Error(string.Format("20102,ERR-XYT,第{0}张IO卡GTS ReadIOOut Error,result is {1}", m_nCardNo, nRtn));
                    WarningMgr.GetInstance().Error(ErrorType.Err_IO_Read_Out,m_nCardNo.ToString(),
                        string.Format(str1, m_nCardNo, nRtn));

                }
                return false;
            }
        }

        /// <summary>
        /// 读取一个IO输出点
        /// </summary>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        public override bool ReadIoOutBit(int nIndex)
        {
            int nData = 0;
            if (0 == mc.GT_GetDo((short)m_nCardNo, mc.MC_GPO, out nData))
            {
                nData = ~nData;
                return 0 != (nData & (1 << (nIndex - 1)));
            }
            return false;
        }

        /// <summary>
        /// 写入一个IO输出点
        /// </summary>
        /// <param name="nIndex"></param>
        /// <param name="bBit"></param>
        /// <returns></returns>
        public override bool WriteIoBit(int nIndex, bool bBit)
        {
            return 0 == mc.GT_SetDoBit((short)m_nCardNo, mc.MC_GPO, (short)nIndex, (short)(bBit ? 0 : 1));
        }

        /// <summary>
        /// 写入一组IO输出数据
        /// </summary>
        /// <param name="nData"></param>
        /// <returns></returns>
        public override bool WriteIo(int nData)
        {
            return 0 == mc.GT_SetDo((short)m_nCardNo, mc.MC_GPO, ~nData);
        }
    }
}
