using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolEx;

#pragma warning disable 1591

namespace ToolEx
{
    [Serializable]
    public partial class ProductData
    {
        #region 原本框架自带的变量
        public string m_strBarCode = "";   //物料码
        public string m_strJigCode = "";   //载具码
        public int m_nGlueIndex = 1;      //点胶位，1：左点胶位 2：右点胶位
        public DateTime m_dtBeginTime = DateTime.Now;     //开始时间
        public DateTime m_dtEndTime = DateTime.Now;       //结束时间
        public DateTime m_dtStartGlue = DateTime.Now;     //开始点胶时间
        public DateTime m_dtEndGlue = DateTime.Now;       //结束点胶时间
        public DateTime m_dtStartAssem = DateTime.Now;    //开始组装时间
        public double m_dbRecheckX = 0.0;  //复检X
        public double m_dbRecheckY = 0.0;  //复检Y
        public double m_dbRecheckU = 0.0;  //复检U
        public bool m_bResult = false;      //最终结果 
        #endregion


        public int StationNum;
        public int CaveNum;

        public string str穴一吸气名称;
        public string str穴一吹气名称;
        public string str穴二吸气名称;
        public string str穴二吹气名称;

        #region 穴位一信息
        /// <summary>
        /// 一穴是否能做
        /// </summary>
        public bool bCaveOk1 = false;
        public string strCoilCode1 = "";
        public DateTime dt涂锡开始时间1 = DateTime.Now;
        public DateTime dt涂锡结束时间1 = DateTime.Now;
        public DateTime dt焊锡_内_开始时间1 = DateTime.Now;
        public DateTime dt焊锡_内_结束时间1 = DateTime.Now;
        public DateTime dt焊锡_外_开始时间1 = DateTime.Now;
        public DateTime dt焊锡_外_结束时间1 = DateTime.Now;
        #endregion


        #region 穴位二信息
        /// <summary>
        /// 二穴是否能做
        /// </summary>
        public bool bCaveOk2 = false;
        public string strCoilCode2 = "";
        public DateTime dt涂锡开始时间2 = DateTime.Now;
        public DateTime dt涂锡结束时间2 = DateTime.Now;
        public DateTime dt焊锡_内_开始时间2 = DateTime.Now;
        public DateTime dt焊锡_内_结束时间2 = DateTime.Now;
        public DateTime dt焊锡_外_开始时间2 = DateTime.Now;
        public DateTime dt焊锡_外_结束时间2 = DateTime.Now;
        #endregion

        #region 需要PDCA上传的信息
        public string strCoilCode = "";
        public int iStationNum;
        public int iCaveNum;
        public DateTime dt涂锡开始时间 = DateTime.Now;
        public DateTime dt涂锡结束时间 = DateTime.Now;
        public DateTime dt焊锡_内_开始时间 = DateTime.Now;
        public DateTime dt焊锡_内_结束时间 = DateTime.Now;
        public DateTime dt焊锡_外_开始时间 = DateTime.Now;
        public DateTime dt焊锡_外_结束时间 = DateTime.Now;
        #endregion
        public double ExposureGlueTimeS
        {
            get
            {
                return (m_dtStartAssem - m_dtStartGlue).TotalSeconds;
            }
        }


        public double GlueTimeS
        {
            get
            {
                return (m_dtEndGlue - m_dtStartGlue).TotalSeconds;
            }
        }
    }

    public partial class ProductMgr
    {
        /// <summary>
        /// 点胶数据
        /// </summary>
        public Queue<ProductData> m_queGlueData = new Queue<ProductData>();

        /// <summary>
        /// 前置二维码信息
        /// </summary>
        public Queue<string> m_queScanFrontCode = new Queue<string>();


    }
}
