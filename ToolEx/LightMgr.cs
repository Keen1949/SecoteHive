using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFrameDll;
using CommonTool;
using System.Xml;
using System.Windows.Forms;
using System.Reflection;

namespace ToolEx
{
    /// <summary>
    /// 光源管理类
    /// </summary>
    public class LightMgr : SingletonTemplate<LightMgr>
    {
        /// <summary>
        /// 光源数据
        /// </summary>
        public Dictionary<string, LightBase> m_dictLights = new Dictionary<string, LightBase>();

        /// <summary>
        /// 数据表头
        /// </summary>
        public static readonly  string[] HEADS = { "名称", "类型", "通道", "通信索引" };

        /// <summary>
        /// 从xml文件中读取定义的Data信息
        /// </summary>
        /// <param name="doc"></param>
        public void ReadCfgFromXml(XmlDocument doc)
        {
            m_dictLights.Clear();
            XmlNodeList xnl = doc.SelectNodes("/SystemCfg/" + "Light");
            if (xnl.Count > 0)
            {
                xnl = xnl.Item(0).ChildNodes;
                if (xnl.Count > 0)
                {
                    foreach (XmlNode xn in xnl)
                    {
                        XmlElement xe = (XmlElement)xn;

                        int nCol = 0;
                        //名称
                        string strName = xe.GetAttribute(HEADS[nCol++]).Trim();

                        //类型
                        string strType = xe.GetAttribute(HEADS[nCol++]).Trim();

                        //通道
                        string strChannel = xe.GetAttribute(HEADS[nCol++]).Trim();

                        //通信索引
                        string strIndex = xe.GetAttribute(HEADS[nCol++]).Trim();

                        try
                        {

                            Assembly assembly = Assembly.GetAssembly(typeof(LightBase));

                            string strClassName = "ToolEx." + strType;
                            Type type = assembly.GetType(strClassName);

                            if (type == null)
                            {
                                throw new Exception(string.Format("光源类{0}找不到可用的封装类，请确认ToolEx.dll是否正确或配置错误?" + strName, new object[0]));
                            }

                            m_dictLights.Add(strName, (LightBase)Activator.CreateInstance(type,
                                strName, Convert.ToInt32(strChannel), Convert.ToInt32(strIndex)));
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }

                    }
                }
            }
        }

        /// <summary>
        /// 跟新内存参数到表格数据
        /// </summary>
        /// <param name="grid">界面表格控件</param>
        public void UpdateGridFromParam(DataGridView grid)
        {
            grid.Rows.Clear();

            foreach (var data in m_dictLights.Values)
            {
                int nRow = grid.Rows.Add();
                int nCol = 0;
                grid.Rows[nRow].Cells[nCol++].Value = data.Name;
                grid.Rows[nRow].Cells[nCol++].Value = data.GetType().Name;
                grid.Rows[nRow].Cells[nCol++].Value = data.Channel;
                grid.Rows[nRow].Cells[nCol++].Value = data.CommIndex;
            }
        }

        /// <summary>
        /// 更新表格数据到内存参数
        /// </summary>
        /// <param name="grid">界面表格控件</param>
        public void UpdateParamFromGrid(DataGridView grid)
        {
            m_dictLights.Clear();
            foreach (DataGridViewRow row in grid.Rows)
            {
                int nCol = 0;

                try
                {
                    //名称
                    if (row.Cells[nCol].Value == null)
                    {
                        continue;

                    }
                    string strName = row.Cells[nCol++].Value.ToString();

                    //类型
                    if (row.Cells[nCol].Value == null)
                    {
                        continue;

                    }
                    string strType = row.Cells[nCol++].Value.ToString();

                    //通道
                    if (row.Cells[nCol].Value == null)
                    {
                        continue;

                    }
                    string strChannel = row.Cells[nCol++].Value.ToString();

                    //通信索引
                    if (row.Cells[nCol].Value == null)
                    {
                        continue;

                    }
                    string strIndex = row.Cells[nCol++].Value.ToString();


                    Assembly assembly = Assembly.GetAssembly(typeof(LightBase));
                    string strClassName = "ToolEx." + strType;
                    Type type = assembly.GetType(strClassName);

                    if (type == null)
                    {
                        throw new Exception(string.Format("光源类{0}找不到可用的封装类，请确认ToolEx.dll是否正确或配置错误?" + strName, new object[0]));
                    }

                    m_dictLights.Add(strName, (LightBase)Activator.CreateInstance(type,
                        strName, Convert.ToInt32(strChannel), Convert.ToInt32(strIndex)));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
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

            XmlNode root = xnl.SelectSingleNode("Light");
            if (root == null)
            {
                root = doc.CreateElement("Light");

                xnl.AppendChild(root);
            }

            root.RemoveAll();

            foreach (var data in m_dictLights.Values)
            {
                XmlElement xe = doc.CreateElement("Light");
                int nItem = 0;
                xe.SetAttribute(HEADS[nItem++], data.Name);
                xe.SetAttribute(HEADS[nItem++], data.GetType().Name);
                xe.SetAttribute(HEADS[nItem++], data.Channel.ToString());
                xe.SetAttribute(HEADS[nItem++], data.CommIndex.ToString());

                root.AppendChild(xe);
            }
        }

        /// <summary>
        /// 获取光源对象
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        public LightBase GetLight(string strName)
        {
            LightBase light;

            if (!m_dictLights.TryGetValue(strName, out light))
            {
                string text = String.Format("不存在光源名称{0}，请确认配置是否正确。", strName);

                MessageBox.Show(text);
            }

            return light;
        }

        /// <summary>
        /// 资源释放
        /// </summary>
        public void DeInit()
        {
            foreach (LightBase light in m_dictLights.Values)
            {
                if (light != null)
                {
                    light.DeInit();
                }
            }
        }

    }
}
