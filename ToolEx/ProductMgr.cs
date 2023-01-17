//2019-10-24 Binggoo 1.加入序列化文件，用于保存生产数据和生产数据索引
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonTool;
using System.Reflection;
using AutoFrameDll;
using HelperTool;
using System.IO;
using System.Collections;

namespace ToolEx
{
    /// <summary>
    /// 生产数据，根据实际项目修改
    /// </summary>
    public partial class ProductData 
    {

        /// <summary>
        /// 拷贝，固定方法
        /// </summary>
        /// <returns></returns>
        public ProductData Clone()
        {
            return SerializerHelper.DeepCopy<ProductData>(this);
        }

        /// <summary>
        /// 根据名称获取字段或者属性值，固定方法
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        public object GetValue(string strName)
        {
            FieldInfo[] fields = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

            //字段
            foreach (FieldInfo field in fields)
            {
                if (strName == field.Name)
                {
                    return field.GetValue(this);
                }

                if (strName.StartsWith(field.Name + "[") && field.FieldType.IsArray)
                {
                    //数组,获取索引
                    int startIndex = strName.IndexOf('[') + 1;
                    int endIndex = strName.LastIndexOf(']');

                    try
                    {
                        string strIndex = strName.Substring(startIndex, endIndex - startIndex);

                        int index = Convert.ToInt32(strIndex);

                        Array list = (Array)field.GetValue(this);

                        return list.GetValue(index);
             
                    }
                    catch (Exception)
                    {

                    }
                    
                }
            }

            //属性
            PropertyInfo[] properties = GetType().GetProperties();
            foreach (PropertyInfo pro in properties)
            {
                if (strName == pro.Name)
                {
                    return pro.GetValue(this);
                }

                if (strName.StartsWith(pro.Name + "[") && pro.PropertyType.IsArray)
                {
                    //数组,获取索引
                    int startIndex = strName.IndexOf('[') + 1;
                    int endIndex = strName.LastIndexOf(']');

                    try
                    {
                        string strIndex = strName.Substring(startIndex, endIndex - startIndex);

                        int index = Convert.ToInt32(strIndex);

                        Array list = (Array)pro.GetValue(this);

                        return list.GetValue(index);

                    }
                    catch (Exception)
                    {

                    }

                }
            }

            return null;
        }

        /// <summary>
        /// 根据名称获取字段或者属性的类型，固定方法
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        public Type GetType(string strName)
        {
            FieldInfo[] fields = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

            //字段
            foreach (FieldInfo field in fields)
            {
                if (strName == field.Name)
                {
                    return field.FieldType;
                }

                if (strName.StartsWith(field.Name+"["))
                {
                    //数组
                    string tName = field.FieldType.FullName.Replace("[]", string.Empty);

                    Type elType = field.FieldType.Assembly.GetType(tName);

                    return elType;
                }

            }

            //属性
            PropertyInfo[] properties = GetType().GetProperties();
            foreach (PropertyInfo pro in properties)
            {
                if (strName == pro.Name)
                {
                    return pro.PropertyType;
                }

                if (strName.StartsWith(pro.Name + "["))
                {
                    //数组
                    string tName = pro.PropertyType.FullName.Replace("[]", string.Empty);

                    Type elType = pro.PropertyType.Assembly.GetType(tName);

                    return elType;
                }
            }

            return null;
        }
    }

    /// <summary>
    /// 作业模式
    /// </summary>
    public enum JobMode
    {
        /// <summary>
        /// 转盘模式
        /// </summary>
        Rotate,

        /// <summary>
        /// 普通流水线模式
        /// </summary>
        Line,

        /// <summary>
        /// 扩展流水线模式，支持中途踢料
        /// </summary>
        LineEx,
    }

    /// <summary>
    /// 作业基类
    /// </summary>
    public abstract class JobBase
    {
        /// <summary>
        /// 作业工位数量
        /// </summary>
        protected int m_nJobCount = 1;

        /// <summary>
        /// 生产数据数组
        /// </summary>
        protected ProductData[] m_arrayProductData;

        /// <summary>
        /// 互斥锁
        /// </summary>
        protected object m_lock = new object();

        /// <summary>
        /// 构造函数
        /// </summary>
        public JobBase()
        {
            LoadData();
        }

        /// <summary>
        /// 加载序列化文件
        /// </summary>
        public abstract void LoadData();

        /// <summary>
        /// 保存序列化文件
        /// </summary>
        public abstract void SaveData();


        /// <summary>
        /// 作业工位数量
        /// </summary>
        public int JobCount
        {
            get
            {
                return m_nJobCount;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public abstract void Init();

        /// <summary>
        /// 更新生产数据列表
        /// </summary>
        /// <param name="nStationIndex">作业工位索引。转盘作业此参数无效</param>
        public abstract void Update(int nStationIndex = 0);

        /// <summary>
        /// 获取生产数据
        /// </summary>
        /// <param name="nStationIndex">作业工位索引，从1开始</param>
        /// <returns>生产数据</returns>
        public abstract ProductData Get(int nStationIndex);

        /// <summary>
        /// 设置生产数据
        /// </summary>
        /// <param name="nStationIndex">作业工位索引，从1开始</param>
        /// <param name="data">生产数据</param>
        /// <param name="bRef">是否传入引用，此参数用于决定是否重新创建一个生产数据对象</param>
        public abstract void Set(int nStationIndex,ProductData data,bool bRef = false);

    }

    /// <summary>
    /// 转盘作业
    /// </summary>
    public class RotateJob:JobBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nCount">作业工位数量</param>
        public RotateJob(int nCount) : base()
        {
            m_nJobCount = nCount;

            if (m_arrayProductData == null || m_arrayProductData.Length != m_nJobCount)
            {
                Init();
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init()
        {
            m_arrayProductData = new ProductData[m_nJobCount];
            for (int i = 0; i < m_nJobCount;i++)
            {
                ProductData data = new ProductData();

                m_arrayProductData[i] = data;
            }
        }

        /// <summary>
        /// 获取生产数据
        /// </summary>
        /// <param name="nStationIndex">作业工位索引，从1开始</param>
        /// <returns>生产数据</returns>
        public override ProductData Get(int nStationIndex)
        {
            if (nStationIndex > m_nJobCount || nStationIndex < 1)
            {
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    MessageBox.Show("Wrong station index");
                }
                else
                {
                    MessageBox.Show("错误的工位索引");
                }

                return null;
            }

            return m_arrayProductData[nStationIndex - 1];

        }

        /// <summary>
        /// 设置生产数据
        /// </summary>
        /// <param name="nStationIndex">作业工位索引，从1开始</param>
        /// <param name="data">生产数据</param>
        /// <param name="bRef">是否传入引用，此参数用于决定是否重新创建一个生产数据对象</param>
        public override void Set(int nStationIndex, ProductData data, bool bRef = false)
        {
            if (nStationIndex > m_nJobCount || nStationIndex < 1)
            {
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    MessageBox.Show("Wrong station index");
                }
                else
                {
                    MessageBox.Show("错误的工位索引");
                }

                return;
            }

            if (bRef)
            {
                //此处需要用到浅拷贝，传值而非引用
                m_arrayProductData[nStationIndex - 1] = data;
            }
            else
            {
                //此处需要用到浅拷贝，传值而非引用
                m_arrayProductData[nStationIndex - 1] = data.Clone();
            }
    
        }

        /// <summary>
        /// 更新生产数据列表，在转盘转动时执行
        /// </summary>
        /// <param name="nStationIndex">转盘作业此参数无效</param>
        public override void Update(int nStationIndex = 0)
        {
            //转盘转动
            ProductData temp = m_arrayProductData[m_nJobCount - 1];

            for (int i = m_nJobCount - 1; i  > 0;i--)
            {
                m_arrayProductData[i] = m_arrayProductData[i - 1];
            }

            m_arrayProductData[0] = temp;
        }

        /// <summary>
        /// 加载序列化文件
        /// </summary>
        public override void LoadData()
        {
            string strProductDataFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "productdata.xml");

            if (File.Exists(strProductDataFile))
            {
                try
                {
                    ArrayList array = SerializerHelper.DeserializeSoap<ArrayList>(strProductDataFile);

                    m_arrayProductData = array[0] as ProductData[];

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

            }
        }

        /// <summary>
        /// 保存序列化文件
        /// </summary>
        public override void SaveData()
        {
            string strProductDataFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "productdata.xml");

            try
            {
                ArrayList array = new ArrayList();
                array.Add(m_arrayProductData);
                array.Add(new int[m_nJobCount]);

                SerializerHelper.SerializeSoap(strProductDataFile, array);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }


    /// <summary>
    /// 流水线作业
    /// </summary>
    public class LineJob: JobBase
    {
        private int[] m_LineIndex;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nCount">作业站位数量</param>
        public LineJob(int nCount) : base()
        {
            m_nJobCount = nCount;

            if (m_arrayProductData == null || m_arrayProductData.Length != m_nJobCount)
            {
                Init();
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init()
        {
            m_LineIndex = new int[m_nJobCount];
            m_arrayProductData = new ProductData[m_nJobCount];

            for (int i = 0; i < m_nJobCount;i++)
            {
                m_LineIndex[i] = 0;

                m_arrayProductData[i] = new ProductData();
            }
            
        }

        /// <summary>
        /// 获取生产数据
        /// </summary>
        /// <param name="nStationIndex">作业站位索引，从1开始</param>
        /// <returns></returns>
        public override ProductData Get(int nStationIndex)
        {
            if (nStationIndex > m_nJobCount || nStationIndex < 1)
            {
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    MessageBox.Show("Wrong station index");
                }
                else
                {
                    MessageBox.Show("错误的工位索引");
                }

                return null;
            }

            lock(m_lock)
            { 
                ProductData data = m_arrayProductData[m_LineIndex[nStationIndex - 1]];

                return data;
            }
        }

        /// <summary>
        /// 设置生产数据，一般只在第一站调用
        /// </summary>
        /// <param name="nStationIndex">作业工位索引，从1开始</param>
        /// <param name="data">生产数据</param>
        /// <param name="bRef">是否传入引用，此参数用于决定是否重新创建一个生产数据对象</param>
        public override void Set(int nStationIndex, ProductData data, bool bRef = false)
        {
            if (nStationIndex > m_nJobCount || nStationIndex < 1)
            {
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    MessageBox.Show("Wrong station index");
                }
                else
                {
                    MessageBox.Show("错误的工位索引");
                }

                return;
            }

            lock (m_lock)
            {
                if (bRef)
                {
                    m_arrayProductData[m_LineIndex[nStationIndex - 1]] = data;
                }
                else
                {
                    //此处需要用到浅拷贝，传值而非引用
                    m_arrayProductData[m_LineIndex[nStationIndex - 1]] = data.Clone();
                }
                
            }

        }

        /// <summary>
        /// 更新生产数据列表，在当前作业站位数据更新完后，执行
        /// </summary>
        /// <param name="nStationIndex">作业工位索引，从1开始</param>
        public override void Update(int nStationIndex = 0)
        {
            if (nStationIndex > m_nJobCount || nStationIndex < 1)
            {
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    MessageBox.Show("Wrong station index");
                }
                else
                {
                    MessageBox.Show("错误的工位索引");
                }

                return;
            }

            lock(m_lock)
            {
                //计数加1
                int index = m_LineIndex[nStationIndex - 1];
                index++;

                index %= m_nJobCount;

                m_LineIndex[nStationIndex - 1] = index;
            }
            
        }

        /// <summary>
        /// 加载序列化文件
        /// </summary>
        public override void LoadData()
        {
            string strProductDataFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "productdata.xml");

            if (File.Exists(strProductDataFile))
            {
                try
                {
                    ArrayList array = SerializerHelper.DeserializeSoap<ArrayList>(strProductDataFile);

                    m_arrayProductData = array[0] as ProductData[];
                    m_LineIndex = array[1] as int[];

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

            }
        }

        /// <summary>
        /// 保存序列化文件
        /// </summary>
        public override void SaveData()
        {
            string strProductDataFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "productdata.xml");

            try
            {
                ArrayList array = new ArrayList();
                array.Add(m_arrayProductData);
                array.Add(m_LineIndex);

                SerializerHelper.SerializeSoap(strProductDataFile, array);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }

    /// <summary>
    /// 扩展流水线作业，支持中途拿料
    /// </summary>
    public class LineExJob: RotateJob
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nCount"></param>
        public LineExJob(int nCount) : base(nCount)
        {

        }

        /// <summary>
        /// 更新数据，把本站的数据传递给下一站,必须在下站是空闲状态才能传递
        /// </summary>
        /// <param name="nStationIndex"></param>
        public override void Update(int nStationIndex)
        {
            if (nStationIndex > m_nJobCount || nStationIndex < 1)
            {
                if (LanguageMgr.GetInstance().LanguageID == 1)
                {
                    MessageBox.Show("Wrong station index");
                }
                else
                {
                    MessageBox.Show("错误的工位索引");
                }
                return;
            }

            //不是最后一站
            if (nStationIndex < m_nJobCount)
            {
                ProductData data = m_arrayProductData[nStationIndex - 1].Clone();

                m_arrayProductData[nStationIndex] = data;
            }
            
        }

        /// <summary>
        /// 传递数据，传递值
        /// </summary>
        /// <param name="nStationIndex"></param>
        /// <param name="data"></param>
        /// <param name="bRef"></param>
        public override void Set(int nStationIndex, ProductData data, bool bRef = false)
        {
            base.Set(nStationIndex, data, false);
        }
    }

    /// <summary>
    /// 生产数据管理类
    /// </summary>
    public partial class ProductMgr : SingletonTemplate<ProductMgr>
    {
        private string m_strDeviceName = "Secote";

        private string m_strDeviceID = "ST-0001";

        private int m_nSoftwareTime = 0;

        private int m_nMachineTime = 0;

        private int m_nJobCount = 1;

        private JobMode m_nJobMode = JobMode.Rotate;

        private string m_strVoltage = "220V";

        private string m_strCurrent = "20A";

        private string m_strPower = "4000W";

        private string m_strAirPressure = "0.6kPa";

        private string[] m_strArrayMotion;

        private string[] m_strArrayRobot;

        private string[] m_strArrayLight;

        private string[] m_strArrayCCD;

        private string[] m_strArrayLens;

        private JobBase m_job;

        private string m_strIniFile = AppDomain.CurrentDomain.BaseDirectory + "Autoframe.ini";//获取当前路径

        private IniHelper m_ini;

        /// <summary>
        /// 发送生产数据委托
        /// </summary>
        /// <param name="data"></param>
        public delegate void SendProductDataHandler(ProductData data);

        /// <summary>
        /// 发送生产数据事件
        /// </summary>
        public event SendProductDataHandler SendProductDataEvent;

        /// <summary>
        /// 产品信息改变委托
        /// </summary>
        public delegate void ProductInfoChangedHandler();

        /// <summary>
        /// 产品系统改变事件
        /// </summary>
        public event ProductInfoChangedHandler ProductInfoChangedEvent;


        /// <summary>
        /// 构造函数
        /// </summary>
        public ProductMgr()
        {
            m_ini = new IniHelper(m_strIniFile);
            ReadCfg();
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~ProductMgr()
        {
            Job.SaveData();
            WriteCfg();
        }

        /// <summary>
        /// 读配置文件
        /// </summary>
        public void ReadCfg()
        {
            try
            {
                string strValue = IniOperation.GetStringValue(m_strIniFile, "Time", "MachineTime", "0");
                m_nMachineTime = Convert.ToInt32(strValue);


                strValue = IniOperation.GetStringValue(m_strIniFile, "Time", "SoftwareTime", "0");
                m_nSoftwareTime = Convert.ToInt32(strValue);

                m_strDeviceName = IniOperation.GetStringValue(m_strIniFile, "DeviceInfo", "DeviceName", "Secote");

                m_strDeviceID = IniOperation.GetStringValue(m_strIniFile, "DeviceInfo", "DeviceID", "ST-0001");

                strValue = IniOperation.GetStringValue(m_strIniFile, "DeviceInfo", "JobCount", "5");
                m_nJobCount = Convert.ToInt32(strValue);

                strValue = IniOperation.GetStringValue(m_strIniFile, "DeviceInfo", "JobMode", "0");
                m_nJobMode = (JobMode)Convert.ToInt32(strValue);

                switch (m_nJobMode)
                {
                    case JobMode.Rotate:
                        m_job = new RotateJob(m_nJobCount);
                        break;

                    case JobMode.Line:
                        m_job = new LineJob(m_nJobCount);
                        break;

                    case JobMode.LineEx:
                        m_job = new LineExJob(m_nJobCount);
                        break;
                }

                m_strVoltage = IniOperation.GetStringValue(m_strIniFile, "DeviceInfo", "Voltage", "220V");
                m_strCurrent = IniOperation.GetStringValue(m_strIniFile, "DeviceInfo", "Current", "20A");
                m_strPower = IniOperation.GetStringValue(m_strIniFile, "DeviceInfo", "Power", "4000W");
                m_strAirPressure = IniOperation.GetStringValue(m_strIniFile, "DeviceInfo", "AirPressure", "0.6KPA");

                strValue = IniOperation.GetStringValue(m_strIniFile, "DeviceInfo", "Motion", "");
                m_strArrayMotion = strValue.Split(',');

                strValue = IniOperation.GetStringValue(m_strIniFile, "DeviceInfo", "Robot", "");
                m_strArrayRobot = strValue.Split(',');

                strValue = IniOperation.GetStringValue(m_strIniFile, "DeviceInfo", "Light", "");
                m_strArrayLight = strValue.Split(',');

                strValue = IniOperation.GetStringValue(m_strIniFile, "DeviceInfo", "CCD", "");
                m_strArrayCCD = strValue.Split(',');

                strValue = IniOperation.GetStringValue(m_strIniFile, "DeviceInfo", "Lens", "");
                m_strArrayLens = strValue.Split(',');

            }
            catch (Exception)
            {

                throw;
            }
            
        }

        /// <summary>
        /// 写配置文件
        /// </summary>
        public void WriteCfg()
        {
            IniOperation.WriteValue(m_strIniFile, "Time", "MachineTime", m_nMachineTime.ToString());

            IniOperation.WriteValue(m_strIniFile, "Time", "SoftwareTime", m_nSoftwareTime.ToString());

            IniOperation.WriteValue(m_strIniFile, "DeviceInfo", "DeviceName", m_strDeviceName);

            IniOperation.WriteValue(m_strIniFile, "DeviceInfo", "DeviceID", m_strDeviceID);

            IniOperation.WriteValue(m_strIniFile, "DeviceInfo", "JobCount", m_nJobCount.ToString());

            IniOperation.WriteValue(m_strIniFile, "DeviceInfo", "JobMode", ((int)m_nJobMode).ToString());

            IniOperation.WriteValue(m_strIniFile, "DeviceInfo", "Voltage", m_strVoltage);

            IniOperation.WriteValue(m_strIniFile, "DeviceInfo", "Current", m_strCurrent);

            IniOperation.WriteValue(m_strIniFile, "DeviceInfo", "Power", m_strPower);

            IniOperation.WriteValue(m_strIniFile, "DeviceInfo", "AirPressure", m_strAirPressure);

            string strValue = "";
            foreach(string str in m_strArrayMotion)
            {
                strValue += str + ",";
            }
            strValue = strValue.TrimEnd(',');
            IniOperation.WriteValue(m_strIniFile, "DeviceInfo", "Motion", strValue);

            strValue = "";
            foreach (string str in m_strArrayRobot)
            {
                strValue += str + ",";
            }
            strValue = strValue.TrimEnd(',');
            IniOperation.WriteValue(m_strIniFile, "DeviceInfo", "Robot", strValue);

            strValue = "";
            foreach (string str in m_strArrayLight)
            {
                strValue += str + ",";
            }
            strValue = strValue.TrimEnd(',');
            IniOperation.WriteValue(m_strIniFile, "DeviceInfo", "Light", strValue);

            strValue = "";
            foreach (string str in m_strArrayCCD)
            {
                strValue += str + ",";
            }
            strValue = strValue.TrimEnd(',');
            IniOperation.WriteValue(m_strIniFile, "DeviceInfo", "CCD", strValue);

            strValue = "";
            foreach (string str in m_strArrayLens)
            {
                strValue += str + ",";
            }
            strValue = strValue.TrimEnd(',');
            IniOperation.WriteValue(m_strIniFile, "DeviceInfo", "Lens", strValue);
        }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName
        {
            get
            {
                return m_strDeviceName;
            }

            set
            {
                m_strDeviceName = value;
            }
        }

        /// <summary>
        /// 设备ID
        /// </summary>
        public string DeviceID
        {
            get
            {
                return m_strDeviceID;
            }

            set
            {
                m_strDeviceID = value;
            }
        }

        /// <summary>
        /// 设备运行时间
        /// </summary>
        public int MachineTime
        {
            get
            {
                return m_nMachineTime;
            }

            set
            {
                m_nMachineTime = value;
            }
        }

        /// <summary>
        /// 软件运行时间
        /// </summary>
        public int SoftwareTime
        {
            get
            {
                return m_nSoftwareTime;
            }

            set
            {
                m_nSoftwareTime = value;
            }
        }

        /// <summary>
        /// 作业工位数量
        /// </summary>
        public int JobCount
        {
            get
            {
                return m_nJobCount;
            }

            set
            {
                m_nJobCount = value;
            }
        }

        /// <summary>
        /// 作业模式
        /// </summary>
        public JobMode Mode
        {
            get
            {
                return m_nJobMode;
            }

            set
            {
                m_nJobMode = value;
            }
        }

        /// <summary>
        /// 设备电压
        /// </summary>
        public string Voltage
        {
            get
            {
                return m_strVoltage;
            }

            set
            {
                m_strVoltage = value;
            }
        }

        /// <summary>
        /// 设备电流
        /// </summary>
        public string Current
        {
            get
            {
                return m_strCurrent;
            }

            set
            {
                m_strCurrent = value;
            }
        }

        /// <summary>
        /// 设备功率
        /// </summary>
        public string Power
        {
            get
            {
                return m_strPower;
            }

            set
            {
                m_strPower = value;
            }
        }

        /// <summary>
        /// 设备气压
        /// </summary>
        public string AirPressure
        {
            get
            {
                return m_strAirPressure;
            }

            set
            {
                m_strAirPressure = value;
            }
        }

        /// <summary>
        /// 设备使用运动板卡
        /// </summary>
        public string[] MotionArray
        {
            get
            {
                return m_strArrayMotion;
            }

            set
            {
                m_strArrayMotion = value;
            }
        }


        /// <summary>
        /// 设备使用机器人
        /// </summary>
        public string[] RobotArray
        {
            get
            {
                return m_strArrayRobot;
            }

            set
            {
                m_strArrayRobot = value;
            }
        }

        /// <summary>
        /// 设备使用光源
        /// </summary>
        public string[] LightArray
        {
            get
            {
                return m_strArrayLight;
            }

            set
            {
                m_strArrayLight = value;
            }
        }

        /// <summary>
        /// 设备使用相机
        /// </summary>
        public string[] CCDArray
        {
            get
            {
                return m_strArrayCCD;
            }

            set
            {
                m_strArrayCCD = value;
            }
        }

        /// <summary>
        /// 设备使用镜头
        /// </summary>
        public string[] LensArray
        {
            get
            {
                return m_strArrayLens;
            }

            set
            {
                m_strArrayLens = value;
            }
        }

        /// <summary>
        /// 作业类
        /// </summary>
        public JobBase Job
        {
            get
            {
                return m_job;
            }

            set
            {
                m_job = value;
            }
        }

        /// <summary>
        /// 当前操作者改变事件
        /// </summary>
        public event Action<string, string> CurrentWorkerChangedEvent;
        /// <summary>
        /// 当前操作者
        /// </summary>
        public string CurrentWorker
        {
            get
            {
                return m_ini.GetString("Worker", "CurrentWorker");
            }

            set
            {
                if (CurrentWorker != value && CurrentWorkerChangedEvent != null)
                {
                    CurrentWorkerChangedEvent(CurrentWorker, value);
                }

                m_ini.WriteString("Worker", "CurrentWorker", value);
            }
        }

        /// <summary>
        /// 操作者工号长度
        /// </summary>
        public int WorkerLength
        {
            get
            {
                return m_ini.GetInt("Worker", "WorkerLength", 12);
            }

            set
            {
                m_ini.WriteInt("Worker", "WorkerLength", value);
            }
        }

        /// <summary>
        /// 发送生产数据
        /// </summary>
        /// <param name="data"></param>
        public void SendProductData(ProductData data)
        {
            if (SendProductDataEvent != null)
            {
                SendProductDataEvent(data.Clone());
            }

            Task.Run(() =>
            {
                Job.SaveData();
            });
        }

        /// <summary>
        /// 获取PDCA上传格式字符串
        /// </summary>
        /// <param name="pdca">PDCA数据组</param>
        /// <param name="strSN">PDCA绑定SN</param>
        /// <param name="data">生产数据</param>
        /// <returns></returns>
        public string GetPDCAString(DataGroup pdca,string strSN,ProductData data)
        {
            //下面开始添加数据，根据实际项目来定
            //数据格式：序列号@pdata@数据名称@数据值@最小值@最大值@单位\n
            //此处的数据名称、最小值、最大值和单位可以用DataMgr中的DataGroup中的一个数据项
            //例如，可以创建一个PDCA的数据组，把各个需要上传的数据名称、最小值、最大值以及单位维护到这个数据组中
            //DataGroup pdca = DataMgr.GetInstance().m_dictDataGroup["PDCA数据"];
            try
            {
                StringBuilder strPDCA = new StringBuilder("{\n");

                strPDCA.Append(strSN + "@start\n");

                //数据的先后顺序可以在数据管理的界面中调整
                foreach (var item in pdca.m_dictData.Values)
                {
                    DataType type;
                    if (Enum.TryParse(item.m_strDataType, out type))
                    {
                        string strCurValue = "";
                        //如果数据索引为空，则用标准值作为数据
                        if (string.IsNullOrEmpty(item.m_strDataIndex))
                        {
                            strCurValue = item.m_dbStandardValue.ToString();
                        }
                        else
                        {
                            Type valType = data.GetType(item.m_strDataIndex);
                            if (valType == typeof(DateTime))
                            {
                                strCurValue = ((DateTime)data.GetValue(item.m_strDataIndex)).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            else if (valType == typeof(double))
                            {
                                strCurValue = ((double)data.GetValue(item.m_strDataIndex)).ToString("f3");
                            }
                            else
                            {
                                strCurValue = data.GetValue(item.m_strDataIndex).ToString();
                            }
                        }

                        switch (type)
                        {
                            case DataType.pdata:
                                {
                                    #region pdata
                                    //判断有无Limit和单位
                                    if (item.m_dbLimitL.IsNullOrEmpty && item.m_dbLimitU.IsNullOrEmpty && string.IsNullOrEmpty(item.m_strUnit))
                                    {
                                        strPDCA.AppendFormat("{0}@pdata@{1}@{2}\n"
                                            , strSN
                                            , item.m_strName
                                            , strCurValue);
                                    }
                                    else if (!item.m_dbLimitL.IsNullOrEmpty && !item.m_dbLimitU.IsNullOrEmpty && string.IsNullOrEmpty(item.m_strUnit))
                                    {
                                        strPDCA.AppendFormat("{0}@pdata@{1}@{2}@{3}@{4}\n"
                                            , strSN
                                            , item.m_strName
                                            , strCurValue
                                            , item.m_dbLimitL.S
                                            , item.m_dbLimitU.S);
                                    }
                                    else if (!item.m_dbLimitL.IsNullOrEmpty && !item.m_dbLimitU.IsNullOrEmpty && !string.IsNullOrEmpty(item.m_strUnit))
                                    {
                                        strPDCA.AppendFormat("{0}@pdata@{1}@{2}@{3}@{4}@{5}\n"
                                            , strSN
                                            , item.m_strName
                                            , strCurValue
                                            , item.m_dbLimitL.S
                                            , item.m_dbLimitU.S
                                            , item.m_strUnit);
                                    }
                                    else if (item.m_dbLimitL.IsNullOrEmpty && item.m_dbLimitU.IsNullOrEmpty && !string.IsNullOrEmpty(item.m_strUnit))
                                    {
                                        strPDCA.AppendFormat("{0}@pdata@{1}@{2}@{3}@{4}@{5}\n"
                                            , strSN
                                            , item.m_strName
                                            , strCurValue
                                            , ""
                                            , ""
                                            , item.m_strUnit);
                                    }
                                    else
                                    {
                                        throw (new Exception("PDCA pdata数据格式错误"));
                                    }
                                    #endregion
                                }
                                break;

                            case DataType.attr:
                                {
                                    strPDCA.AppendFormat("{0}@attr@{1}@{2}\n"
                                        , strSN
                                        , item.m_strName
                                        , strCurValue);
                                }
                                break;

                            case DataType.log_file:
                                {

                                    //CCQMTESTFY1K@log_ﬁle@smb://127.0.0.1/Downloads/gitbox-1.6.2-ml.zip@uid@pwd
                                    strPDCA.AppendFormat("{0}@log_file@{1}@{2}@{3}\n"
                                        , strSN
                                        , strCurValue
                                        , item.m_dbLimitL.S    //用户名
                                        , item.m_dbLimitU.S);  //密码
                                }
                                break;

                            case DataType.dut_pos:
                                {
                                    //数据名称作为FixtureID，标准值作为HeadID
                                    if (!string.IsNullOrEmpty(item.m_strName) && item.m_dbStandardValue.IsNullOrEmpty)
                                    {
                                        strPDCA.AppendFormat("{0}@dut_pos@{1}\n"
                                       , strSN
                                       , item.m_strName);
                                    }
                                    else if (!string.IsNullOrEmpty(item.m_strName) && !item.m_dbStandardValue.IsNullOrEmpty)
                                    {
                                        strPDCA.AppendFormat("{0}@dut_pos@{1}@{2}\n"
                                       , strSN
                                       , item.m_strName
                                       , item.m_dbStandardValue.S);
                                    }
                                    else
                                    {
                                        throw (new Exception("PDCA dut_pos格式错误"));
                                    }

                                }
                                break;

                            default:
                                strPDCA.AppendFormat("{0}@{1}@{2}\n"
                                       , strSN
                                       , type.ToString()
                                       , strCurValue);
                                break;
                        }
                    }
                }

                strPDCA.Append(strSN + "@submit@" + pdca.m_strVersion + "\n");

                strPDCA.Append("}\n");

                return strPDCA.ToString();
            }
            catch (Exception )
            {
                return "";
            }
            
        }

        /// <summary>
        /// 测试PDCA
        /// </summary>
        public void Test()
        {
            ProductData data = new ProductData();
            data.m_strBarCode = "12345ABCDE";
            SendProductData(data);
        }

        /// <summary>
        /// 更新设备信息
        /// </summary>
        public void UpdateProductInfo()
        {
            if (ProductInfoChangedEvent != null)
            {
                ProductInfoChangedEvent();
            }
        }


        /// <summary>
        /// 保存INI文件
        /// </summary>
        /// <param name="data"></param>
        /// <param name="strSavePath"></param>
        /// <param name="fileName"></param>
        public bool SaveIniData(ProductData data,string strSavePath, string fileName)
        {
            if (!Directory.Exists(strSavePath))
            {
                Directory.CreateDirectory(strSavePath);
            }

            IniHelper ini = new IniHelper(Path.Combine(strSavePath, fileName));

            foreach (var item in DataMgr.GetInstance().m_dictDataSave)
            {
                Type t = data.GetType(item.Value);
                if (t == typeof(DateTime))
                {
                    ini.WriteString("Data",item.Key, ((DateTime)data.GetValue(item.Value)).ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else if (t == typeof(double))
                {
                    ini.WriteString("Data", item.Key, ((double)data.GetValue(item.Value)).ToString("f3"));
                }
                else if (t == typeof(bool))
                {
                    ini.WriteString("Data", item.Key, ((bool)data.GetValue(item.Value)) ? "OK" : "NG");
                }
                else
                {
                    ini.WriteString("Data", item.Key, data.GetValue(item.Value).ToString());
                }
            }

            return true;
        }

        /// <summary>
        /// 保存CSV文件
        /// </summary>
        /// <param name="data"></param>
        /// <param name="strSavePath"></param>
        /// <param name="fileName"></param>
        public bool SaveCSVData(ProductData data, string strSavePath, string fileName)
        {
            //记录文件
            CsvOperation csv = new CsvOperation(strSavePath, fileName);

            int row = 0, col = 0;
            if (!File.Exists(csv.FileName))
            {
                foreach (var item in DataMgr.GetInstance().m_dictDataSave.Keys)
                {
                    csv[0, col++] = item;
                }

                row = 1;
            }

            col = 0;

            foreach (var item in DataMgr.GetInstance().m_dictDataSave)
            {
                Type t = data.GetType(item.Value);
                if (t == typeof(DateTime))
                {
                    csv[row, col++] = ((DateTime)data.GetValue(item.Value)).ToString("yyyy-MM-dd HH:mm:ss");
                }
                else if (t == typeof(double))
                {
                    csv[row, col++] = ((double)data.GetValue(item.Value)).ToString("f3");
                }
                else if (t == typeof(bool))
                {
                    csv[row, col++] = ((bool)data.GetValue(item.Value)) ? "OK" : "NG";
                }
                else
                {
                    csv[row, col++] = data.GetValue(item.Value).ToString();
                }
            }

            csv.Save();

            return true;
        }

        /// <summary>
        /// 保存数据库文件
        /// </summary>
        /// <param name="data"></param>
        /// <param name="strConn"></param>
        /// <param name="strTableName"></param>
        /// <returns></returns>
        public bool SaveDBData(ProductData data,string strConn,string strTableName)
        {
            SqlBase sql = new MySQL();

            if (!sql.Connect(strConn))
            {
                return false;
            }

            Table table = new Table();
            table.Name = strTableName;

            foreach (var item in DataMgr.GetInstance().m_dictDataSave)
            {
                Type t = data.GetType(item.Value);
                string name = item.Key;
                table.Add(name, t, data.GetValue(item.Value));
            }

            //判断数据表是否创建
            if (!sql.IsTableExist(strTableName))
            {
                //数据表不存在，创建数据表
                if (!sql.CreateTable(table))
                {
                    return false;
                }
            }

            if (!sql.AddTableData(table))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool SaveData(ProductData data)
        {
            if (DataMgr.GetInstance().DataSaveEnable)
            {
                switch (DataMgr.GetInstance().SaveType)
                {
                    case SaveType.CSV:
                        {
                            string strSavePath = Path.Combine(DataMgr.GetInstance().SavePath, DateTime.Now.ToString("yyyy-MM-dd"));
                            string strFileName = ProductMgr.GetInstance().DeviceName;

                            return SaveCSVData(data, strSavePath, strFileName);
                        }
                        break;
                    case SaveType.INI:
                        {
                            string strSavePath = Path.Combine(DataMgr.GetInstance().SavePath, DateTime.Now.ToString("yyyy-MM-dd"));
                            string strFileName = ProductMgr.GetInstance().DeviceName;

                            return SaveIniData(data, strSavePath, strFileName);
                        }
                        break;
                    case SaveType.DB:
                        {
                            //连接数据库判断数据库是否存在
                            SqlBase sql = new MySQL();

                            if (!sql.Connect(DataMgr.GetInstance().Server, DataMgr.GetInstance().Port, DataMgr.GetInstance().UserID, DataMgr.GetInstance().Password, null))
                            {
                                return false;
                            }

                            if (!sql.IsDatabaseExist(DataMgr.GetInstance().Database))
                            {
                                if (!sql.CreateDataBase(DataMgr.GetInstance().Database))
                                {
                                    return false;
                                }
                            }

                            sql.Disconnect();

                            string strConn = MySQL.GenConnectString(DataMgr.GetInstance().Server, DataMgr.GetInstance().Port, DataMgr.GetInstance().UserID, DataMgr.GetInstance().Password, DataMgr.GetInstance().Database);
                            string strTable = DataMgr.GetInstance().TableName;

                            return SaveDBData(data, strConn, strTable);
                        }
                        break;
                }
            }

            return true;
        }

        /// <summary>
        /// 记录数据变更
        /// </summary>
        /// <param name="name"></param>
        /// <param name="oldValue"></param>
        /// <param name="curValue"></param>
        public static void LogChanged(string name, string oldValue, string curValue)
        {
            try
            {
                string strLogFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log");

                if (!Directory.Exists(strLogFile))
                {
                    Directory.CreateDirectory(strLogFile);
                }

                strLogFile = Path.Combine(strLogFile, string.Format("{0}.csv", DateTime.Now.ToString("yyyy-MM-dd")));

                StringBuilder sb = new StringBuilder();
                if (!File.Exists(strLogFile))
                {
                    sb.AppendLine("Time,Worker,Name,OldValue,CurValue");
                }

                sb.AppendLine(string.Format("{0},{1},{2},{3},{4}",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    , ProductMgr.GetInstance().CurrentWorker
                    , name
                    , oldValue
                    , curValue
                    ));

                using (StreamWriter sw = new StreamWriter(strLogFile, true))
                {
                    sw.Write(sb.ToString());
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
    }
}
