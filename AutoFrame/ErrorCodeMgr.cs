using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTool;
using System.IO;
using AutoFrameDll;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;

namespace AutoFrame
{
    /// <summary>
    /// 用户自定义错误
    /// </summary>
    public class UserDefineError
    {
        public string strSysErrType;
        public string strSysObject;
        public string strSysErrCode;

        public string strUserErrType;
        public string strUserObject;
        public string strUserErrCode;
        public string strUserDesc;
    }

    public class ErrorCodeMgr : SingletonTemplate<ErrorCodeMgr>
    {
        private Dictionary<string, UserDefineError> m_dictUserDefineErrors = new Dictionary<string, UserDefineError>();

        public string[] Heads;

        public ErrorCodeMgr()
        {
            LoadUserError();

            WarningMgr.GetInstance().TransformEvent += GetUserWarningData;
        }

        public void LoadUserError()
        {
            CsvOperation csv = new CsvOperation(AppDomain.CurrentDomain.BaseDirectory, "UserErrorList.csv");

            csv.LoadCsvFile();

            ArrayList head = (ArrayList)csv.Rows[0];

            Heads = new string[head.Count];

            head.CopyTo(Heads);

            m_dictUserDefineErrors.Clear();

            //SystemErrType,SystemObject,SystemErrCode,UserErrType,UserObject,UserErrCode,UserDesc
            for (int row = 0; row < csv.RowCount;row++)
            {
                if (row == 0)
                {
                    if (csv.ColCount < 7)
                    {
                        MessageBox.Show("UserErrorList format error");

                        return;
                    }
                    else if (csv[0,0] != "SystemErrType"
                        || csv[0, 1] != "SystemObject"
                        || csv[0, 2] != "SystemErrCode"
                        || csv[0, 3] != "UserErrType"
                        || csv[0, 4] != "UserObject"
                        || csv[0, 5] != "UserErrCode"
                        || csv[0, 6] != "UserDesc")
                    {
                        MessageBox.Show("UserErrorList format error");

                        return;
                    }
                }
                else
                {
                    string strKey = csv[row, 0] + "+" + csv[row, 1];

                    UserDefineError userError = new UserDefineError();

                    int col = 0;
                    userError.strSysErrType = csv[row, col++];
                    userError.strSysObject = csv[row, col++];
                    userError.strSysErrCode = csv[row, col++];
                    userError.strUserErrType = csv[row, col++];
                    userError.strUserObject = csv[row, col++];
                    userError.strUserErrCode = csv[row, col++];
                    userError.strUserDesc = csv[row, col++];

                    m_dictUserDefineErrors.Add(strKey,userError);
                }

            }
        }

        public void SaveUserError()
        {
            string strFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserErrorList.csv");

            if (File.Exists(strFile))
            {
                File.Delete(strFile);
            }

            CsvOperation csv = new CsvOperation(AppDomain.CurrentDomain.BaseDirectory, "UserErrorList.csv");
            csv.BInsertQuota = false;

            int row = 0, col = 0;
            foreach (string head in Heads)
            {
                csv[row, col++] = head;
            }

            foreach(var item in m_dictUserDefineErrors.Values)
            {
                col = 0;
                row++;

                csv[row, col++] = item.strSysErrType;
                csv[row, col++] = item.strSysObject;
                csv[row, col++] = item.strSysErrCode;
                csv[row, col++] = item.strUserErrType;
                csv[row, col++] = item.strUserObject;
                csv[row, col++] = item.strUserErrCode;
                csv[row, col++] = item.strUserDesc;
            }

            csv.Save();
        }

        public void UpdateGridFromParam(DataGridView grid)
        {
            grid.Rows.Clear();

            foreach (var data in m_dictUserDefineErrors.Values)
            {
                int nRow = grid.Rows.Add();
                int nCol = 0;
                grid.Rows[nRow].Cells[nCol++].Value = data.strSysErrType;
                grid.Rows[nRow].Cells[nCol++].Value = data.strSysObject;
                grid.Rows[nRow].Cells[nCol++].Value = data.strSysErrCode;

                grid.Rows[nRow].Cells[nCol++].Value = data.strUserErrType;
                grid.Rows[nRow].Cells[nCol++].Value = data.strUserObject;
                grid.Rows[nRow].Cells[nCol++].Value = data.strUserErrCode;
                grid.Rows[nRow].Cells[nCol++].Value = data.strUserDesc;
            }
        }

        /// <summary>
        /// 更新表格数据到内存参数
        /// </summary>
        /// <param name="grid">界面表格控件</param>
        public void UpdateParamFromGrid(DataGridView grid)
        {
            m_dictUserDefineErrors.Clear();
            foreach (DataGridViewRow row in grid.Rows)
            {
                int nCol = 0;

                try
                {
                    UserDefineError userError = new UserDefineError();

                    //SystemErrType
                    if (row.Cells[nCol].Value == null)
                    {
                        continue;
                    }
                    userError.strSysErrType = row.Cells[nCol++].Value.ToString();

                    //SystemObject
                    if (row.Cells[nCol].Value == null)
                    {
                        row.Cells[nCol].Value = "";
                    }
                    userError.strSysObject = row.Cells[nCol++].Value.ToString();

                    //SystemErrCode
                    if (row.Cells[nCol].Value == null)
                    {
                        continue;
                    }
                    userError.strSysErrCode = row.Cells[nCol++].Value.ToString();

                    //UserErrType
                    if (row.Cells[nCol].Value == null)
                    {
                        continue;
                    }
                    userError.strUserErrType = row.Cells[nCol++].Value.ToString();

                    //UserObject
                    if (row.Cells[nCol].Value == null)
                    {
                        row.Cells[nCol].Value = "";
                    }
                    userError.strUserObject = row.Cells[nCol++].Value.ToString();

                    //UserErrCode
                    if (row.Cells[nCol].Value == null)
                    {
                        row.Cells[nCol].Value = "";
                    }
                    userError.strUserErrCode= row.Cells[nCol++].Value.ToString();

                    //UserDesc
                    if (row.Cells[nCol].Value == null)
                    {
                        row.Cells[nCol].Value = "";
                    }
                    userError.strUserDesc = row.Cells[nCol++].Value.ToString();

                    m_dictUserDefineErrors.Add(userError.strSysErrType + "+" + userError.strSysObject, userError);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private WARNING_DATA GetUserWarningData(WARNING_DATA wd)
        {

            string strKey = wd.strCategory + "+" + wd.strObject;

            //完全自定义
            if (m_dictUserDefineErrors.ContainsKey(strKey))
            {
                UserDefineError userError = m_dictUserDefineErrors[strKey];

                if (!string.IsNullOrEmpty(userError.strUserErrType))
                {
                    wd.strCategory = userError.strUserErrType;
                }

                if (!string.IsNullOrEmpty(userError.strUserObject))
                {
                    wd.strObject = userError.strUserObject;
                }

                if (!string.IsNullOrEmpty(userError.strUserErrCode))
                {
                    wd.strCode = userError.strUserErrCode;
                }

                if (!string.IsNullOrEmpty(userError.strUserDesc))
                {
                    wd.strMsg = userError.strUserDesc;
                }

            }
            else
            {
                //自定义某一类
                foreach(var item in m_dictUserDefineErrors.Values)
                {
                    if (wd.strCategory == item.strSysErrType 
                        && string.IsNullOrEmpty(item.strSysObject)
                        && !string.IsNullOrEmpty(item.strUserErrType))
                    {
                        wd.strCategory = item.strUserErrType;

                        if (!string.IsNullOrEmpty(item.strUserErrCode))
                        {
                            wd.strCode = item.strUserErrCode;
                        }

                        if (!string.IsNullOrEmpty(item.strUserDesc))
                        {
                            wd.strMsg = item.strUserDesc;
                        }

                        break;
                    }
                }
            }

            return wd;
        }
    }
}
