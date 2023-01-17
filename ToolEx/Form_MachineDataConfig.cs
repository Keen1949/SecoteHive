using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace ToolEx
{
    public partial class Form_MachineDataConfig : Form
    {
        /// <summary>
        /// 
        /// </summary>
        public Form_MachineDataConfig()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 从配置文件读取machine data 需要上传的数据类型和格式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_LoadFromFile_Click(object sender, EventArgs e)
        {
            dataGridView_Step.Rows.Clear();
            string cfg = Application.StartupPath + "\\MachineDataCfg.xml";

            try
            {
                if (File.Exists(cfg))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(cfg);
                    ReadConfigXML(doc);
                }
                else
                {
                    CreateXml();
                }
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message, "报警文件读取失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 如果不存在配置文件，就创建
        /// </summary>
        public void CreateXml()
        {
            //新建XML文件  
            XmlDocument XmlDoc = null;
            XmlDoc = new XmlDocument();

            string FilePath = Application.StartupPath;

            FilePath = FilePath + "\\MachineDataCfg.xml";

            if (File.Exists(FilePath))
            {
                XmlDoc.Load(FilePath);

                XmlNode root = XmlDoc.SelectSingleNode("Config");

                XmlNodeList xnl = XmlDoc.SelectNodes("/Config");
                if (xnl.Count > 0)
                {
                    root.RemoveAll();
                }
            }
            else
            {
                //声明版本和编码
                XmlDeclaration dec = XmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
                XmlDoc.AppendChild(dec);

                XmlElement root = XmlDoc.CreateElement("Config");
                XmlDoc.AppendChild(root);
            }

            XmlDoc.Save(FilePath);
        }

        /// <summary>
        /// 读取配置文件数据
        /// </summary>
        /// <param name="doc"></param>
        public void ReadConfigXML(XmlDocument doc)
        {
            XmlNodeList xnl = doc.SelectNodes("/Config");

            if (xnl.Count > 0)
            {
                xnl = xnl.Item(0).ChildNodes;
                if (xnl.Count > 0)
                {
                    foreach (XmlNode xn in xnl)
                    {
                        XmlElement xe = (XmlElement)xn;

                        int index = Convert.ToInt32(xe.GetAttribute("序号").Trim());
                        string strName = xe.GetAttribute("名称").Trim();
                        string strDataIndex = xe.GetAttribute("数据索引").Trim();
                        string strDataStyle = xe.GetAttribute("数据类型").Trim();


                        if (string.IsNullOrEmpty(strName) || string.IsNullOrEmpty(strDataIndex) || string.IsNullOrEmpty(strDataStyle))
                        {
                            continue;
                        }

                        dataGridView_Step.Rows.Add(strName, strDataIndex, strDataStyle);
                    }
                }
            }
        }

        /// <summary>
        /// 插入machine data 的配置信息
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="strDataIndex"></param>
        /// <param name="strDataStyle"></param>
        /// <param name="index"></param>
        public static void InsertNode(string strName, string strDataIndex, string strDataStyle, string index)
        {
            string FilePath = Application.StartupPath;
            FilePath = FilePath + "\\MachineDataCfg.xml";
            XmlDocument XmlDoc = null;
            XmlDoc = new XmlDocument();
            if (!File.Exists(FilePath))
            {
                if (MessageBox.Show($"MachineDataCfg.xml" + "文件不存在", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Question) == DialogResult.OK)//给出提示
                {
                    return;
                }
                return;
            }

            try
            {
                XmlDoc.Load(FilePath);
                XmlNode root = XmlDoc.SelectSingleNode("Config");

                XmlElement xe1 = XmlDoc.CreateElement("Config");
                xe1.SetAttribute("序号", index);
                xe1.SetAttribute("名称", strName);
                xe1.SetAttribute("数据索引", strDataIndex);
                xe1.SetAttribute("数据类型", strDataStyle);

                root.AppendChild(xe1);

                XmlDoc.Save(FilePath);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("您确认要保存该文件吗？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)//给出提示
            {
                return;
            }

            CreateXml();

            for (int i = 0; i < dataGridView_Step.Rows.Count - 1; i++)
            {
                InsertNode(dataGridView_Step.Rows[i].Cells[0].Value.ToString(), dataGridView_Step.Rows[i].Cells[1].Value.ToString(), dataGridView_Step.Rows[i].Cells[2].Value.ToString(), (i + 1).ToString());
            }
        }

        private void button_DSC_Click(object sender, EventArgs e)
        {
            int k = dataGridView_Step.SelectedRows.Count;
            if (MessageBox.Show("您确认要删除这" + Convert.ToString(k) + "项吗？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
            }
            else
            {
                if (k != dataGridView_Step.Rows.Count - 1)
                {
                    for (int i = k; i >= 1; i--)//从下往上删，避免沙漏效应
                    {
                        dataGridView_Step.Rows.RemoveAt(dataGridView_Step.SelectedRows[i - 1].Index);
                    }
                }
                else
                {
                    dataGridView_Step.Rows.Clear();
                }
            }
        }

        private void button_DA_Click(object sender, EventArgs e)
        {
            dataGridView_Step.Rows.Clear();
        }
    }
}
