using CommonTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows.Forms;
using System.IO;

namespace ToolEx
{
    /// <summary>
    /// 数据类型，用于PDCA
    /// </summary>
    public enum DataType
    {
        /// <summary>
        /// data数据
        /// </summary>
        pdata,

        /// <summary>
        /// 属性数据
        /// </summary>
        attr,

        /// <summary>
        /// 
        /// </summary>
        dut_pos,

        /// <summary>
        /// 开始时间，yyyy-MM-dd HH:mm:ss
        /// </summary>
        start_time,

        /// <summary>
        /// 结束时间，yyyy-MM-dd HH:mm:ss
        /// </summary>
        stop_time,

        /// <summary>
        /// 日志文件
        /// </summary>
        log_file,
    }

    /// <summary>
    /// 保存类型
    /// </summary>
    public enum SaveType
    {
        /// <summary>
        /// INI文件
        /// </summary>
        INI = 0,
        /// <summary>
        /// CSV文件
        /// </summary>
        CSV,
        /// <summary>
        /// 数据库文件
        /// </summary>
        DB,
    }

    /// <summary>
    /// 数据类
    /// </summary>
    public class Data
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string m_strName = "";

        /// <summary>
        /// 数据类型
        /// </summary>
        public string m_strDataType = "";

        /// <summary>
        /// 标准值
        /// </summary>
        public TValue m_dbStandardValue = "";

        /// <summary>
        /// 当前值
        /// </summary>
        public TValue m_dbCurValue = "";

        /// <summary>
        /// 对应ProductData中的数据字符串索引
        /// </summary>
        public string m_strDataIndex = "";

        /// <summary>
        /// 上限
        /// </summary>
        public TValue m_dbLimitU = "";

        /// <summary>
        /// 下限
        /// </summary>
        public TValue m_dbLimitL = "";

        /// <summary>
        /// 补偿值
        /// </summary>
        public TValue m_dbOffset = "";

        /// <summary>
        /// 单位
        /// </summary>
        public string m_strUnit = "";

    }

    /// <summary>
    /// 数据组
    /// </summary>
    public class DataGroup
    {
        /// <summary>
        /// 数据组名称
        /// </summary>
        public string m_strGroupName = "";

        /// <summary>
        /// 数据版本
        /// </summary>
        public string m_strVersion = "1.0";

        /// <summary>
        /// 是否PDCA数据
        /// </summary>
        public bool m_bIsPDCA = false;

        /// <summary>
        /// 权限等级，0 - OP 1 - AE  2 - Adjustor 3 - Engineer 4 - Administator
        /// </summary>
        public int m_nLevel = 2;

        /// <summary>
        /// 数据
        /// </summary>
        public Dictionary<string, Data> m_dictData = new Dictionary<string, Data>(); 
    }

    /// <summary>
    /// 数据管理类
    /// </summary>
    public class DataMgr : SingletonTemplate<DataMgr>
    {
        /// <summary>
        /// 数据组
        /// </summary>
        public Dictionary<string, DataGroup> m_dictDataGroup = new Dictionary<string, DataGroup>();

        /// <summary>
        /// 数据项
        /// </summary>
        public static readonly string[] m_strDescribe = { "名称", "数据类型", "数据索引", "标准值", "上限", "下限","补偿值","单位"};

        /// <summary>
        /// 显示的数据
        /// </summary>
        public Dictionary<string, string> m_dictDataShow = new Dictionary<string, string>();

        /// <summary>
        /// 保存的数据
        /// </summary>
        public Dictionary<string, string> m_dictDataSave = new Dictionary<string, string>();

        private string m_strSavePath = "";

        private bool m_bDataShowSave = true;  //界面显示的内容是否保存

        private bool m_bDataSave = true;  //保存的内容是否保存

        /// <summary>
        /// 显示数据改变委托
        /// </summary>
        public delegate void DataShowChangeHander();

        /// <summary>
        /// 显示数据改变事件
        /// </summary>
        public event DataShowChangeHander DataShowChangeEvent;

        /// <summary>
        /// 保存数据改变事件
        /// </summary>
        public event DataShowChangeHander DataSaveChangeEvent;

        /// <summary>
        /// 数据改变委托
        /// </summary>
        /// <param name="strGroup"></param>
        /// <param name="strData"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        public delegate void DataChangedHandler(string strGroup, string strData, TValue oldValue, TValue newValue);

        /// <summary>
        /// 标准值改变事件
        /// </summary>
        public event DataChangedHandler StandardValueChangedEvent;

        /// <summary>
        /// 补偿值改变事件
        /// </summary>
        public event DataChangedHandler OffsetValueChangedEvent;

        /// <summary>
        /// 上限改变事件
        /// </summary>
        public event DataChangedHandler LimitUValueChangedEvent;

        /// <summary>
        /// 下限改变事件
        /// </summary>
        public event DataChangedHandler LimitLValueChangedEvent;
        /// <summary>
        /// 构造函数
        /// </summary>
        public DataMgr()
        {
        }

        /// <summary>
        /// 保存数据的路径，只针对保存的数据
        /// </summary>
        public string SavePath
        {
            get
            {
                return m_strSavePath;
            }

            set
            {
                m_strSavePath = value;
            }
        }

        /// <summary>
        /// 显示界面的数据是否需要保存
        /// </summary>
        public bool DataShowSaveEnable
        {
            get
            {
                return m_bDataShowSave;
            }

            set
            {
                m_bDataShowSave = value;
            }
        }


        /// <summary>
        /// 保存界面的数据是否需要保存
        /// </summary>
        public bool DataSaveEnable
        {
            get
            {
                return m_bDataSave;
            }

            set
            {
                m_bDataSave = value;
            }
        }

        #region 2020-03-02 Binggoo 新增数据库保存类型

        /// <summary>
        /// 保存类型
        /// </summary>
        public SaveType SaveType { get; set; }
        /// <summary>
        /// 数据库服务地址
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// 数据库端口号
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 数据库用户名
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 数据库密码，加密字符串
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// 数据表名称
        /// </summary>
        public string TableName { get; set; }
        #endregion
        /// <summary>
        /// 从xml文件中读取定义的Data信息
        /// </summary>
        /// <param name="doc"></param>
        public void ReadCfgFromXml(XmlDocument doc)
        {
            m_dictDataGroup.Clear();
            m_dictDataShow.Clear();
            m_dictDataSave.Clear();

            XmlNodeList xnl = doc.SelectNodes("/SystemCfg/" + "Data");
            if (xnl.Count > 0)
            {
                xnl = xnl.Item(0).ChildNodes;
                if (xnl.Count > 0)
                {
                    foreach (XmlNode xn in xnl)
                    {
                        if (xn.NodeType != XmlNodeType.Element)
                        {
                            continue;
                        }
                        XmlElement xe = (XmlElement)xn;
               
                        if (xn.Name == "Data")
                        {
                            #region Data
                            string strGroupName = xe.GetAttribute("名称").Trim();
                            string strVersion = xe.GetAttribute("版本").Trim();
                            string strIsPDCA = xe.GetAttribute("PDCA").Trim();
                            string strLevel = xe.GetAttribute("权限").Trim();

                            if (strGroupName.Length == 0)
                            {
                                continue;
                            }

                            if (xn.ChildNodes.Count > 0)
                            {
                                DataGroup dataGroup = new DataGroup();
                                dataGroup.m_strGroupName = strGroupName;
                                dataGroup.m_strVersion = strVersion;
                                dataGroup.m_bIsPDCA = Convert.ToBoolean(strIsPDCA);
                                dataGroup.m_nLevel = Convert.ToInt32(strLevel);

                                foreach (XmlNode item in xn.ChildNodes)
                                {
                                    if (item.NodeType != XmlNodeType.Element)
                                    {
                                        continue;
                                    }

                                    XmlElement xeItem = (XmlElement)item;

                                    int nCol = 0;
                                    //名称
                                    string strName = xeItem.GetAttribute(m_strDescribe[nCol++]).Trim();
                                    if (strName.Length == 0)
                                    {
                                        continue;
                                    }

                                    //数据类型
                                    string strDataType = xeItem.GetAttribute(m_strDescribe[nCol++]).Trim();
                                    //索引数据字符串
                                    string strIndex = xeItem.GetAttribute(m_strDescribe[nCol++]).Trim();
                                    //标准值
                                    string strValue = xeItem.GetAttribute(m_strDescribe[nCol++]).Trim();
                                    //上限
                                    string strLimitU = xeItem.GetAttribute(m_strDescribe[nCol++]).Trim();
                                    //下线
                                    string strLimtL = xeItem.GetAttribute(m_strDescribe[nCol++]).Trim();
                                    //补偿值
                                    string strOffset = xeItem.GetAttribute(m_strDescribe[nCol++]).Trim();
                                    //单位
                                    string strUnit = xeItem.GetAttribute(m_strDescribe[nCol++]).Trim();

                                    Data data = new Data();
                                    try
                                    {
                                        data.m_strName = strName;
                                        data.m_strDataType = strDataType;
                                        data.m_strDataIndex = strIndex;
                                        data.m_dbStandardValue = strValue;
                                        data.m_dbLimitU = strLimitU;
                                        data.m_dbLimitL = strLimtL;
                                        data.m_dbOffset = strOffset;
                                        data.m_strUnit = strUnit;
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.ToString());
                                    }
                                    dataGroup.m_dictData.Add(strName, data);

                                }

                                m_dictDataGroup.Add(strGroupName, dataGroup);
                            }
                            #endregion   
                        }
                        else if (xn.Name == "DataShow")
                        {
                            #region DataShow
                            if (xn.ChildNodes.Count > 0)
                            {
                                m_bDataShowSave = Convert.ToBoolean(xe.GetAttribute("是否保存").Trim());
                                foreach (XmlNode item in xn.ChildNodes)
                                {
                                    if (item.NodeType != XmlNodeType.Element)
                                    {
                                        continue;
                                    }

                                    XmlElement xeItem = (XmlElement)item;
                                    //名称
                                    string strName = xeItem.GetAttribute("名称").Trim();
                                    
                                    //索引数据字符串
                                    string strIndex = xeItem.GetAttribute("数据索引").Trim();

                                    if (strName.Length == 0)
                                    {
                                        continue;
                                    }

                                    m_dictDataShow.Add(strName, strIndex);                  
                                }
                            }
                            #endregion   
                        }
                        else if (xn.Name == "DataSave")
                        {
                            #region DataSave
                            if (xn.ChildNodes.Count > 0)
                            {
                                //2020-03-02 Binggoo 新增数据库保存
                                string strSaveType = xe.GetAttribute("保存类型").Trim();
                                SaveType type;
                                if (Enum.TryParse<SaveType>(strSaveType,out type))
                                {
                                    SaveType = type;
                                }
                                else
                                {
                                    SaveType = SaveType.CSV;
                                }
                                
                                Server = xe.GetAttribute("Server").Trim();
                                Port = Convert.ToInt32(xe.GetAttribute("Port").Trim());
                                UserID = xe.GetAttribute("UserID").Trim();
                                string strPassword = xe.GetAttribute("Password").Trim();
                                if (string.IsNullOrEmpty(strPassword))
                                {
                                    Password = "";
                                }
                                else
                                {
                                    Password = HelpTool.Decode(strPassword);
                                }
                                
                                Database = xe.GetAttribute("Database").Trim();
                                TableName = xe.GetAttribute("TableName").Trim();

                                m_strSavePath = xe.GetAttribute("保存路径").Trim();
                                m_bDataSave = Convert.ToBoolean(xe.GetAttribute("是否保存").Trim());
                                foreach (XmlNode item in xn.ChildNodes)
                                {
                                    if (item.NodeType != XmlNodeType.Element)
                                    {
                                        continue;
                                    }

                                    XmlElement xeItem = (XmlElement)item;
                                    //名称
                                    string strName = xeItem.GetAttribute("名称").Trim();

                                    //索引数据字符串
                                    string strIndex = xeItem.GetAttribute("数据索引").Trim();

                                    if (strName.Length == 0)
                                    {
                                        continue;
                                    }

                                    m_dictDataSave.Add(strName, strIndex);
                                }
                            }
                            #endregion   
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 从资源中追加数据配置
        /// </summary>
        /// <param name="strResource"></param>
        /// <returns></returns>
        public bool AppendDataCfgFromResource(string strResource)
        {
            bool bRet = false;
            XmlDocument doc = new XmlDocument();
            try
            {
                MemoryStream stream = new MemoryStream();
                StreamWriter sw = new StreamWriter(stream);
                sw.Write(strResource);
                sw.Flush();
                stream.Seek(0, SeekOrigin.Begin);

                doc.Load(stream);

                XmlNodeList xnl = doc.SelectNodes("/SystemCfg/" + "Data");
                if (xnl.Count > 0)
                {
                    xnl = xnl.Item(0).ChildNodes;
                    if (xnl.Count > 0)
                    {
                        foreach (XmlNode xn in xnl)
                        {
                            if (xn.NodeType != XmlNodeType.Element)
                            {
                                continue;
                            }
                            XmlElement xe = (XmlElement)xn;

                            if (xn.Name == "Data")
                            {
                                #region Data
                                string strGroupName = xe.GetAttribute("名称").Trim();
                                string strVersion = xe.GetAttribute("版本").Trim();
                                string strIsPDCA = xe.GetAttribute("PDCA").Trim();
                                string strLevel = xe.GetAttribute("权限").Trim();

                                if (strGroupName.Length == 0)
                                {
                                    continue;
                                }

                                if (xn.ChildNodes.Count > 0)
                                {
                                    DataGroup dataGroup = new DataGroup();

                                    if (m_dictDataGroup.ContainsKey(strGroupName))
                                    {
                                        dataGroup = m_dictDataGroup[strGroupName];
                                    }
                                    else
                                    {
                                        dataGroup.m_strGroupName = strGroupName;
                                        dataGroup.m_strVersion = strVersion;
                                        dataGroup.m_bIsPDCA = Convert.ToBoolean(strIsPDCA);
                                        dataGroup.m_nLevel = Convert.ToInt32(strLevel);
                                    }

                                    foreach (XmlNode item in xn.ChildNodes)
                                    {
                                        if (item.NodeType != XmlNodeType.Element)
                                        {
                                            continue;
                                        }

                                        XmlElement xeItem = (XmlElement)item;

                                        int nCol = 0;
                                        //名称
                                        string strName = xeItem.GetAttribute(m_strDescribe[nCol++]).Trim();
                                        if (strName.Length == 0)
                                        {
                                            continue;
                                        }

                                        if (dataGroup.m_dictData.ContainsKey(strName))
                                        {
                                            continue;
                                        }

                                        //数据类型
                                        string strDataType = xeItem.GetAttribute(m_strDescribe[nCol++]).Trim();
                                        //索引数据字符串
                                        string strIndex = xeItem.GetAttribute(m_strDescribe[nCol++]).Trim();
                                        //标准值
                                        string strValue = xeItem.GetAttribute(m_strDescribe[nCol++]).Trim();
                                        //上限
                                        string strLimitU = xeItem.GetAttribute(m_strDescribe[nCol++]).Trim();
                                        //下线
                                        string strLimtL = xeItem.GetAttribute(m_strDescribe[nCol++]).Trim();
                                        //补偿值
                                        string strOffset = xeItem.GetAttribute(m_strDescribe[nCol++]).Trim();
                                        //单位
                                        string strUnit = xeItem.GetAttribute(m_strDescribe[nCol++]).Trim();

                                        Data data = new Data();
                                        try
                                        {
                                            data.m_strName = strName;
                                            data.m_strDataType = strDataType;
                                            data.m_strDataIndex = strIndex;
                                            data.m_dbStandardValue = strValue;
                                            data.m_dbLimitU = strLimitU;
                                            data.m_dbLimitL = strLimtL;
                                            data.m_dbOffset = strOffset;
                                            data.m_strUnit = strUnit;
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show(ex.ToString());
                                        }

                                        dataGroup.m_dictData.Add(strName, data);

                                        bRet = true;

                                    }

                                    if (!m_dictDataGroup.ContainsKey(strGroupName))
                                    {
                                        m_dictDataGroup.Add(strGroupName, dataGroup);
                                        bRet = true;
                                    }
                                            
                                }
                                #endregion
                            }
                            else if (xn.Name == "DataShow")
                            {
                                #region DataShow
                                if (xn.ChildNodes.Count > 0)
                                {
                                    foreach (XmlNode item in xn.ChildNodes)
                                    {
                                        if (item.NodeType != XmlNodeType.Element)
                                        {
                                            continue;
                                        }

                                        XmlElement xeItem = (XmlElement)item;
                                        //名称
                                        string strName = xeItem.GetAttribute("名称").Trim();

                                        //索引数据字符串
                                        string strIndex = xeItem.GetAttribute("数据索引").Trim();

                                        if (strName.Length == 0)
                                        {
                                            continue;
                                        }

                                        if (m_dictDataShow.ContainsKey(strName))
                                        {
                                            continue;
                                        }

                                        m_dictDataShow.Add(strName, strIndex);

                                        bRet = true;
                                    }
                                }
                                #endregion
                            }
                            else if (xn.Name == "DataSave")
                            {
                                #region DataSave
                                if (xn.ChildNodes.Count > 0)
                                {
                                    foreach (XmlNode item in xn.ChildNodes)
                                    {
                                        if (item.NodeType != XmlNodeType.Element)
                                        {
                                            continue;
                                        }

                                        XmlElement xeItem = (XmlElement)item;
                                        //名称
                                        string strName = xeItem.GetAttribute("名称").Trim();

                                        //索引数据字符串
                                        string strIndex = xeItem.GetAttribute("数据索引").Trim();

                                        if (strName.Length == 0)
                                        {
                                            continue;
                                        }

                                        if (m_dictDataSave.ContainsKey(strName))
                                        {
                                            continue;
                                        }

                                        m_dictDataSave.Add(strName, strIndex);

                                        bRet = true;
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bRet = false;
                MessageBox.Show(ex.ToString());
            }

            return bRet;
        }

        /// <summary>
        /// 更新内存参数到表格数据
        /// </summary>
        /// <param name="strGroupName">组名</param>
        /// <param name="grid">控件</param>
        public void UpdateGridFromParam(string strGroupName,DataGridView grid)
        {
            if (!m_dictDataGroup.ContainsKey(strGroupName))
            {
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    MessageBox.Show(String.Format("Group name {0} does not exist", strGroupName));
                }
                else
                {
                    MessageBox.Show(String.Format("不存在组名{0}", strGroupName));
                }
                
                return;
            }
            grid.Rows.Clear();

            DataGroup group = m_dictDataGroup[strGroupName];

            foreach(var data in group.m_dictData)
            {
                int nRow = grid.Rows.Add();
                int nCol = 0;
                grid.Rows[nRow].Cells[nCol++].Value = data.Value.m_strName;
                grid.Rows[nRow].Cells[nCol++].Value = data.Value.m_strDataType;
                grid.Rows[nRow].Cells[nCol++].Value = data.Value.m_strDataIndex;
                grid.Rows[nRow].Cells[nCol++].Value = data.Value.m_dbStandardValue;
                grid.Rows[nRow].Cells[nCol++].Value = data.Value.m_dbLimitU;
                grid.Rows[nRow].Cells[nCol++].Value = data.Value.m_dbLimitL;
                grid.Rows[nRow].Cells[nCol++].Value = data.Value.m_dbOffset;
                grid.Rows[nRow].Cells[nCol++].Value = data.Value.m_strUnit;
            }
        }

        /// <summary>
        /// 更新内存参数到表格数据
        /// </summary>
        /// <param name="gridShow"></param>
        /// <param name="gridSave"></param>
        public void UpdateDataGridFromParam(DataGridView gridShow, DataGridView gridSave)
        {
            gridShow.Rows.Clear();

            foreach (var data in m_dictDataShow)
            {
                int nRow = gridShow.Rows.Add();
                int nCol = 0;
                gridShow.Rows[nRow].Cells[nCol++].Value = data.Key;
                gridShow.Rows[nRow].Cells[nCol++].Value = data.Value;
            }

            gridSave.Rows.Clear();

            foreach (var data in m_dictDataSave)
            {
                int nRow = gridSave.Rows.Add();
                int nCol = 0;
                gridSave.Rows[nRow].Cells[nCol++].Value = data.Key;
                gridSave.Rows[nRow].Cells[nCol++].Value = data.Value;
            }
        }

        /// <summary>
        /// 更新表格数据到内存参数
        /// </summary>
        /// <param name="strGroupName"></param>
        /// <param name="strVersion"></param>
        /// <param name="isPDCA"></param>
        /// <param name="nLevel">权限等级</param>
        /// <param name="grid"></param>
        public void UpdateParamFromGrid(string strGroupName,string strVersion,bool isPDCA,int nLevel,DataGridView grid)
        {
            if (!m_dictDataGroup.ContainsKey(strGroupName))
            {
                m_dictDataGroup.Add(strGroupName, new DataGroup());
            }

            DataGroup group = m_dictDataGroup[strGroupName];

            //备份数据
            Dictionary<string, Data> distTemp = new Dictionary<string, Data>();

            foreach (var item in group.m_dictData)
            {
                distTemp.Add(item.Key, item.Value);
            }

            group.m_dictData.Clear();
            group.m_strVersion = strVersion;
            group.m_strGroupName = strGroupName;
            group.m_bIsPDCA = isPDCA;
            group.m_nLevel = nLevel;

            foreach(DataGridViewRow row in grid.Rows)
            {
                Data data = new Data();
                int nCol = 0;

                try
                {
                    //名称
                    data.m_strName = row.Cells[nCol++].Value.ToString();

                    Data tempData;
                    if (!distTemp.TryGetValue(data.m_strName,out tempData))
                    {
                        tempData = null;
                    }

                    //数据类型
                    if (row.Cells[nCol].Value != null)
                    {
                        data.m_strDataType = row.Cells[nCol].Value.ToString(); ;
                    }
                    nCol++;

                    //数据索引
                    if (row.Cells[nCol].Value != null)
                    {
                        data.m_strDataIndex = row.Cells[nCol].Value.ToString();
                    }
                    nCol++;

                    //标准值
                    if (row.Cells[nCol].Value != null)
                    {
                        data.m_dbStandardValue = row.Cells[nCol].Value.ToString();

                        if (tempData != null)
                        {
                            if (tempData.m_dbStandardValue != data.m_dbStandardValue && StandardValueChangedEvent != null)
                            {
                                StandardValueChangedEvent(strGroupName, data.m_strName, tempData.m_dbStandardValue, data.m_dbStandardValue);
                            }
                        }
                    }
                    nCol++;

                    //上限
                    if (row.Cells[nCol].Value != null)
                    {
                        data.m_dbLimitU = row.Cells[nCol].Value.ToString();

                        if (tempData != null)
                        {
                            if (tempData.m_dbLimitU != data.m_dbLimitU && LimitUValueChangedEvent != null)
                            {
                                LimitUValueChangedEvent(strGroupName, data.m_strName, tempData.m_dbLimitU, data.m_dbLimitU);
                            }
                        }
                    }
                    nCol++;

                    //下限
                    if (row.Cells[nCol].Value != null)
                    {
                        data.m_dbLimitL = row.Cells[nCol].Value.ToString();

                        if (tempData != null)
                        {
                            if (tempData.m_dbLimitL != data.m_dbLimitL && LimitLValueChangedEvent != null)
                            {
                                LimitLValueChangedEvent(strGroupName, data.m_strName, tempData.m_dbLimitL, data.m_dbLimitL);
                            }
                        }
                    }
                    nCol++;

                    //补偿值
                    if (row.Cells[nCol].Value != null)
                    {
                        data.m_dbOffset = row.Cells[nCol].Value.ToString();

                        if (tempData != null)
                        {
                            if (tempData.m_dbOffset != data.m_dbOffset && OffsetValueChangedEvent != null)
                            {
                                OffsetValueChangedEvent(strGroupName, data.m_strName, tempData.m_dbOffset, data.m_dbOffset);
                            }
                        }
                    }
                    nCol++;

                    //单位
                    if (row.Cells[nCol].Value != null)
                    {
                        data.m_strUnit = row.Cells[nCol].Value.ToString();
                    }
                    nCol++;

                    group.m_dictData.Add(data.m_strName, data);
                    
                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// 更新表格数据到内存参数
        /// </summary>
        /// <param name="gridShow"></param>
        /// <param name="bSaveShow"></param>
        /// <param name="gridSave"></param>
        /// <param name="strSavePath"></param>
        /// <param name="bSave"></param>
        public void UpdateParamFromDataGrid(DataGridView gridShow,bool bSaveShow, DataGridView gridSave, string strSavePath, bool bSave)
        {
            string[] dataShowKeys = m_dictDataShow.Keys.ToArray();

            m_dictDataShow.Clear();
            m_bDataShowSave = bSaveShow;

            foreach (DataGridViewRow row in gridShow.Rows)
            {
                int nCol = 0;

                try
                {
                    //名称
                    string strName = row.Cells[nCol++].Value.ToString();

                    //数据索引
                    string strIndex = row.Cells[nCol++].Value.ToString();

                    m_dictDataShow.Add(strName, strIndex);
                }
                catch
                {

                }
                
            }

            //判断DataShow是否有变化
            if (DataShowChangeEvent != null)
            {
                bool bChanged = false;
                if (dataShowKeys.Length != m_dictDataShow.Keys.Count)
                {
                    bChanged = true;
                }
                else
                {
                    int i = 0;
                    foreach (var item in m_dictDataShow.Keys)
                    {
                        if (dataShowKeys[i++] != item)
                        {
                            bChanged = true;
                            break;
                        }
                    }
                }

                if (bChanged)
                {
                    DataShowChangeEvent();
                }
            }

            Dictionary<string, string> tempDataSave = new Dictionary<string, string>();

            foreach (var item in m_dictDataSave)
            {
                tempDataSave.Add(item.Key,item.Value);
            }

            m_dictDataSave.Clear();
            m_strSavePath = strSavePath;
            m_bDataSave = bSave;
            
            foreach (DataGridViewRow row in gridSave.Rows)
            {
                int nCol = 0;

                try
                {
                    //名称
                    string strName = row.Cells[nCol++].Value.ToString();

                    //数据索引
                    string strIndex = row.Cells[nCol++].Value.ToString();

                    m_dictDataSave.Add(strName, strIndex);
                }
                catch
                {

                }
                
            }

            //2020-03-03 Binggoo 判断DataSave是否有变化
            if (DataSaveChangeEvent != null)
            {
                bool bChanged = false;
                if (tempDataSave.Count != m_dictDataSave.Count)
                {
                    bChanged = true;
                }
                else
                {
                    foreach (var item in m_dictDataSave)
                    {
                        //是否包含元素
                        if (!tempDataSave.ContainsKey(item.Key))
                        {
                            bChanged = true;
                            break;
                        }

                        //值是否相等
                        if (item.Value != tempDataSave[item.Key])
                        {
                            bChanged = true;
                            break;
                        }
                    }
                }

                if (bChanged)
                {
                    DataSaveChangeEvent();
                }
            }
        }

        /// <summary>
        /// 保存内存参数到xml文件
        /// </summary>
        /// <param name="doc">已打开的xml文档</param>
        public void SaveCfgXML(XmlDocument doc)
        {
            XmlNode xnl = doc.SelectSingleNode("SystemCfg");

            XmlNode root = xnl.SelectSingleNode("Data");
            if (root == null)
            {
                root = doc.CreateElement("Data");

                xnl.AppendChild(root);
            }

            root.RemoveAll();

            #region Data
            foreach (var group in m_dictDataGroup)
            {
                
                XmlElement child = doc.CreateElement("Data");

                child.SetAttribute("名称", group.Value.m_strGroupName);
                child.SetAttribute("版本", group.Value.m_strVersion);
                child.SetAttribute("PDCA", group.Value.m_bIsPDCA.ToString());
                child.SetAttribute("权限", group.Value.m_nLevel.ToString());

                foreach (var data in group.Value.m_dictData)
                {
                    XmlElement xe = doc.CreateElement("Item");
                    int nItem = 0;
                    xe.SetAttribute(m_strDescribe[nItem++], data.Value.m_strName);
                    xe.SetAttribute(m_strDescribe[nItem++], data.Value.m_strDataType);
                    xe.SetAttribute(m_strDescribe[nItem++], data.Value.m_strDataIndex);
                    xe.SetAttribute(m_strDescribe[nItem++], data.Value.m_dbStandardValue.ToString());
                    xe.SetAttribute(m_strDescribe[nItem++], data.Value.m_dbLimitU.ToString());
                    xe.SetAttribute(m_strDescribe[nItem++], data.Value.m_dbLimitL.ToString());
                    xe.SetAttribute(m_strDescribe[nItem++], data.Value.m_dbOffset.ToString());
                    xe.SetAttribute(m_strDescribe[nItem++], data.Value.m_strUnit);

                    child.AppendChild(xe);
                }
                root.AppendChild(child);
            }
            #endregion

            #region DataShow
            XmlElement childDataShow = doc.CreateElement("DataShow");
            childDataShow.SetAttribute("是否保存", m_bDataShowSave.ToString());
            foreach (var data in m_dictDataShow)
            {
                XmlElement xe = doc.CreateElement("Item");
                xe.SetAttribute("名称", data.Key);
                xe.SetAttribute("数据索引", data.Value);

                childDataShow.AppendChild(xe);
            }
            root.AppendChild(childDataShow);
            #endregion

            #region DataSave
            XmlElement childDataSave = doc.CreateElement("DataSave");

            //2020-03-02 Binggoo 新增数据库
            childDataSave.SetAttribute("保存类型", SaveType.ToString());
            childDataSave.SetAttribute("Server", Server);
            childDataSave.SetAttribute("Port", Port.ToString());
            childDataSave.SetAttribute("UserID", UserID);
            childDataSave.SetAttribute("Password", HelpTool.Encode(Password)) ;
            childDataSave.SetAttribute("Database", Database);
            childDataSave.SetAttribute("TableName", TableName);

            childDataSave.SetAttribute("保存路径", m_strSavePath);
            childDataSave.SetAttribute("是否保存", m_bDataSave.ToString());

            foreach (var data in m_dictDataSave)
            {
                XmlElement xe = doc.CreateElement("Item");
                xe.SetAttribute("名称", data.Key);
                xe.SetAttribute("数据索引", data.Value);

                childDataSave.AppendChild(xe);
            }
            root.AppendChild(childDataSave);
            #endregion
        }

        /// <summary>
        /// 备份XML
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="strFile"></param>
        public void BackupXML(XmlDocument doc,string strFile)
        {
            XmlNode xnl = doc.SelectSingleNode("SystemCfg");

            XmlNode root = xnl.SelectSingleNode("Data");
            if (root != null)
            {
                XmlDocument backup = new XmlDocument();
                if (File.Exists(strFile))
                {
                    backup.Load(strFile);
                }
                else
                {
                    XmlDeclaration dec = backup.CreateXmlDeclaration("1.0", "utf-8", null);
                    backup.AppendChild(dec);
                    //创建一个根节点（一级）
                    xnl = backup.CreateElement("SystemCfg");

                    root = backup.ImportNode(root, true);

                    xnl.AppendChild(root);

                    backup.AppendChild(xnl);

                    backup.Save(strFile);
                }
            }
        }
    }
}
