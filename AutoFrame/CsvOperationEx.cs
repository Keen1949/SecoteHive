using CommonTool;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
namespace AutoFrame
{
	/// <summary>
	///
	/// </summary>
	public class CsvOperationEx
	{
		private ArrayList rowAL;
		private string fileName;
		private Encoding encoding;
        private bool m_bInsertQuota = true; //是否插入引号
		/// <summary>
		/// 文件名,包括文件路径
		/// </summary>
		public string FileName
		{
			set
			{
				this.fileName = value;
			}

            get
            {
                return fileName;
            }
		}
		/// <summary>
		/// 文件编码
		/// </summary>
		public Encoding FileEncoding
		{
			set
			{
				this.encoding = value;
			}
		}
		/// <summary>
		/// 获取行数
		/// </summary>
		public int RowCount
		{
			get
			{
				return this.rowAL.Count;
			}
		}
		/// <summary>
		/// 获取列数
		/// </summary>
		public int ColCount
		{
			get
			{
				int num = 0;
				int num2;
				for (int i = 0; i < this.rowAL.Count; i = num2 + 1)
				{
					ArrayList arrayList = (ArrayList)this.rowAL[i];
					num = ((num > arrayList.Count) ? num : arrayList.Count);
					num2 = i;
				}
				return num;
			}
		}

        /// <summary>
        /// 是否用分号包括数据项
        /// </summary>
        public bool BQuota
        {
            get
            {
                return m_bInsertQuota;
            }

            set
            {
                m_bInsertQuota = value;
            }
        }
		/// <summary>
		/// 获取某行某列的数据
		/// row:行,row = 1代表第一行
		/// col:列,col = 1代表第一列  
		/// </summary>
		public string this[int row, int col]
		{
			get
			{
				this.CheckRowValid(row);
				this.CheckColValid(col);
				ArrayList arrayList = (ArrayList)this.rowAL[row];
				bool flag = arrayList.Count <= col;
				string result;
				if (flag)
				{
					result = "";
				}
				else
				{
					result = arrayList[col].ToString();
				}
				return result;
			}
			set
			{
				bool flag = row < 0;
				if (flag)
				{
					throw new Exception("行数不能小于0");
				}
				bool flag2 = row >= this.rowAL.Count;
				if (flag2)
				{
					int num;
					for (int i = this.rowAL.Count; i <= row; i = num + 1)
					{
						this.rowAL.Add(new ArrayList());
						num = i;
					}
				}
				bool flag3 = col < 0;
				if (flag3)
				{
					throw new Exception("列数不能小于0");
				}
				ArrayList arrayList = (ArrayList)this.rowAL[row];
				bool flag4 = col >= arrayList.Count;
				if (flag4)
				{
					int num;
					for (int j = arrayList.Count; j <= col; j = num + 1)
					{
						arrayList.Add("");
						num = j;
					}
				}
				this.rowAL[row] = arrayList;
				ArrayList arrayList2 = (ArrayList)this.rowAL[row];
				arrayList2[col] = value;
				this.rowAL[row] = arrayList2;
			}
		}
		/// <summary>
		/// 根据最小行，最大行，最小列，最大列，来生成一个DataTable类型的数据
		/// 行等于1代表第一行
		/// 列等于1代表第一列
		/// maxrow: -1代表最大行
		/// maxcol: -1代表最大列
		/// </summary>
		public DataTable this[int minRow, int maxRow, int minCol, int maxCol]
		{
			get
			{
				this.CheckRowValid(minRow);
				this.CheckMaxRowValid(maxRow);
				this.CheckColValid(minCol);
				this.CheckMaxColValid(maxCol);
				bool flag = maxRow == -1;
				if (flag)
				{
					maxRow = this.RowCount;
				}
				bool flag2 = maxCol == -1;
				if (flag2)
				{
					maxCol = this.ColCount;
				}
				bool flag3 = maxRow < minRow;
				if (flag3)
				{
					throw new Exception("最大行数不能小于最小行数");
				}
				bool flag4 = maxCol < minCol;
				if (flag4)
				{
					throw new Exception("最大列数不能小于最小列数");
				}
				DataTable dataTable = new DataTable();
				int num;
				for (int i = minCol; i <= maxCol; i = num + 1)
				{
					dataTable.Columns.Add(i.ToString());
					num = i;
				}
				for (int j = minRow; j <= maxRow; j = num + 1)
				{
					DataRow dataRow = dataTable.NewRow();
					int i = 0;
					for (int k = minCol; k <= maxCol; k = num + 1)
					{
						dataRow[i] = this[j, k];
						num = i;
						i = num + 1;
						num = k;
					}
					dataTable.Rows.Add(dataRow);
					num = j;
				}
				return dataTable;
			}
		}
		/// <summary>
		/// 默认以系统参数路径，当前日期生成文件名
		/// </summary>
		public CsvOperationEx()
		{
			this.rowAL = new ArrayList();
			this.fileName = SingletonTemplate<SystemMgr>.GetInstance().GetDataPath("") + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".csv";
			this.encoding = Encoding.Default;
            m_bInsertQuota = true;
		}
		/// <summary>
		///             以系统参数路径，文件名称及当前日期组合生成文件名
		/// </summary>
		/// <param name="fileName">文件名,包括文件路径</param>
		public CsvOperationEx(string fileName)
		{
			this.rowAL = new ArrayList();
			this.fileName = string.Concat(new string[]
			{
				SingletonTemplate<SystemMgr>.GetInstance().GetDataPath(""),
				"\\",
				fileName,
				"_",
				DateTime.Now.ToString("yyyyMMdd"),
				".csv"
			});
			this.encoding = Encoding.Default;
		}
        public CsvOperationEx(string fileName,bool IsZhao)
        {
            this.rowAL = new ArrayList();
            this.fileName = string.Concat(new string[]
            {               
                fileName,
                "_",
                DateTime.Now.ToString("yyyyMMdd"),
                ".csv"
            });
            this.encoding = Encoding.Default;
        }
		/// <summary>
		///             指定文件名和编码方式
		/// </summary>
		/// <param name="fileName">文件名,包括文件路径</param>
		/// <param name="encoding">文件编码</param>
		public CsvOperationEx(string fileName, Encoding encoding)
		{
			this.rowAL = new ArrayList();
			this.fileName = string.Concat(new string[]
			{
				SingletonTemplate<SystemMgr>.GetInstance().GetDataPath(""),
				"\\",
				fileName,
				"_",
				DateTime.Now.ToString("yyyyMMdd"),
				".csv"
			});
			this.encoding = encoding;
		}

        /// <summary>
		/// 指定文件路径和文件名
		/// </summary>
		/// <param name="fileName">文件名,包括文件路径</param>
		/// <param name="encoding">文件编码</param>
		public CsvOperationEx(string filePath, string fileName)
        {
            this.rowAL = new ArrayList();
          // if() filePath
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            this.fileName = string.Concat(new string[]
            {
                filePath,
                "\\",
                fileName
            });


            this.encoding = Encoding.Default;
        }
        /// <summary>
        /// 检查行数是否是有效的
        /// </summary>
        /// <param name="row"></param>  
        private void CheckRowValid(int row)
		{
			bool flag = row <= 0;
			if (flag)
			{
				throw new Exception("行数不能小于0");
			}
			bool flag2 = row > this.RowCount;
			if (flag2)
			{
				throw new Exception("没有当前行的数据");
			}
		}
		/// <summary>
		/// 检查最大行数是否是有效的
		/// </summary>
		/// <param name="maxRow"></param>  
		private void CheckMaxRowValid(int maxRow)
		{
			bool flag = maxRow <= 0 && maxRow != -1;
			if (flag)
			{
				throw new Exception("行数不能等于0或小于-1");
			}
			bool flag2 = maxRow > this.RowCount;
			if (flag2)
			{
				throw new Exception("没有当前行的数据");
			}
		}
		/// <summary>
		/// 检查列数是否是有效的
		/// </summary>
		/// <param name="col"></param>  
		private void CheckColValid(int col)
		{
			bool flag = col <= 0;
			if (flag)
			{
				throw new Exception("列数不能小于0");
			}
			bool flag2 = col > this.ColCount;
			if (flag2)
			{
				throw new Exception("没有当前列的数据");
			}
		}
		/// <summary>
		/// 检查检查最大列数是否是有效的
		/// </summary>
		/// <param name="maxCol"></param>  
		private void CheckMaxColValid(int maxCol)
		{
			bool flag = maxCol <= 0 && maxCol != -1;
			if (flag)
			{
				throw new Exception("列数不能等于0或小于-1");
			}
			bool flag2 = maxCol > this.ColCount;
			if (flag2)
			{
				throw new Exception("没有当前列的数据");
			}
		}
		/// <summary>
		/// 载入CSV文件
		/// </summary>
		private void LoadCsvFile()
		{
			bool flag = this.fileName == null;
			if (flag)
			{
				throw new Exception("请指定要载入的CSV文件名");
			}
			bool flag2 = !File.Exists(this.fileName);
			if (flag2)
			{
				throw new Exception("指定的CSV文件不存在");
			}
			bool flag3 = this.encoding == null;
			if (flag3)
			{
				this.encoding = Encoding.Default;
			}
			string text = "";
			using (FileStream fileStream = new FileStream(this.fileName, FileMode.Append, FileAccess.Write, FileShare.Write))
			{
				using (StreamReader streamReader = new StreamReader(fileStream, this.encoding))
				{
					while (true)
					{
						string text2 = streamReader.ReadLine();
						bool flag4 = text2 == null;
						if (flag4)
						{
							break;
						}
						bool flag5 = text == "";
						if (flag5)
						{
							text = text2;
						}
						else
						{
							text = text + "/r/n" + text2;
						}
						bool flag6 = !this.IfOddQuota(text);
						if (flag6)
						{
							this.AddNewDataLine(text);
							text = "";
						}
					}
				}
			}
			bool flag7 = text.Length > 0;
			if (flag7)
			{
				throw new Exception("CSV文件的格式有错误");
			}
		}
		/// <summary>
		/// 获取两个连续引号变成单个引号的数据行
		/// </summary>
		/// <param name="fileDataLine">文件数据行</param>
		/// <returns></returns>
		private string GetDeleteQuotaDataLine(string fileDataLine)
		{
			return fileDataLine.Replace("\"\"", "\"");
		}
		/// <summary>
		/// 判断字符串是否包含奇数个引号
		/// </summary>
		/// <param name="dataLine">数据行</param>
		/// <returns>为奇数时，返回为真；否则返回为假</returns>
		private bool IfOddQuota(string dataLine)
		{
			int num = 0;
			int num2;
			for (int i = 0; i < dataLine.Length; i = num2 + 1)
			{
				bool flag = dataLine[i] == '"';
				if (flag)
				{
					num2 = num;
					num = num2 + 1;
				}
				num2 = i;
			}
			bool result = false;
			bool flag2 = num % 2 == 1;
			if (flag2)
			{
				result = true;
			}
			return result;
		}
		/// <summary>
		/// 判断是否以奇数个引号开始
		/// </summary>
		/// <param name="dataCell"></param>
		/// <returns></returns>
		private bool IfOddStartQuota(string dataCell)
		{
			int num = 0;
			int num2;
			for (int i = 0; i < dataCell.Length; i = num2 + 1)
			{
				bool flag = dataCell[i] == '"';
				if (!flag)
				{
					break;
				}
				num2 = num;
				num = num2 + 1;
				num2 = i;
			}
			bool result = false;
			bool flag2 = num % 2 == 1;
			if (flag2)
			{
				result = true;
			}
			return result;
		}
		/// <summary>
		/// 判断是否以奇数个引号结尾
		/// </summary>
		/// <param name="dataCell"></param>
		/// <returns></returns>
		private bool IfOddEndQuota(string dataCell)
		{
			int num = 0;
			int num2;
			for (int i = dataCell.Length - 1; i >= 0; i = num2 - 1)
			{
				bool flag = dataCell[i] == '"';
				if (!flag)
				{
					break;
				}
				num2 = num;
				num = num2 + 1;
				num2 = i;
			}
			bool result = false;
			bool flag2 = num % 2 == 1;
			if (flag2)
			{
				result = true;
			}
			return result;
		}
		/// <summary>
		/// 加入新的数据行
		/// </summary>
		/// <param name="newDataLine">新的数据行</param>
		private void AddNewDataLine(string newDataLine)
		{
			Debug.WriteLine("NewLine:" + newDataLine);
			ArrayList arrayList = new ArrayList();
			string[] array = newDataLine.Split(new char[]
			{
				','
			});
			bool flag = false;
			string text = "";
			int i = 0;
			while (i < array.Length)
			{
				bool flag2 = flag;
				if (flag2)
				{
					text = text + "," + array[i];
					bool flag3 = this.IfOddEndQuota(array[i]);
					if (flag3)
					{
						arrayList.Add(this.GetHandleData(text));
						flag = false;
					}
				}
				else
				{
					bool flag4 = this.IfOddStartQuota(array[i]);
					if (flag4)
					{
						bool flag5 = this.IfOddEndQuota(array[i]) && array[i].Length > 2 && !this.IfOddQuota(array[i]);
						if (flag5)
						{
							arrayList.Add(this.GetHandleData(array[i]));
							flag = false;
						}
						else
						{
							flag = true;
							text = array[i];
						}
					}
					else
					{
						arrayList.Add(this.GetHandleData(array[i]));
					}
				}
				IL_EE:
				int num = i;
				i = num + 1;
				continue;
				goto IL_EE;
			}
			bool flag6 = flag;
			if (flag6)
			{
				throw new Exception("数据格式有问题");
			}
			this.rowAL.Add(arrayList);
		}
		/// <summary>
		/// 去掉格子的首尾引号，把双引号变成单引号
		/// </summary>
		/// <param name="fileCellData"></param>
		/// <returns></returns>
		private string GetHandleData(string fileCellData)
		{
			bool flag = fileCellData == "";
			string result;
			if (flag)
			{
				result = "";
			}
			else
			{
				bool flag2 = this.IfOddStartQuota(fileCellData);
				if (flag2)
				{
					bool flag3 = this.IfOddEndQuota(fileCellData);
					if (!flag3)
					{
						throw new Exception("数据引号无法匹配" + fileCellData);
					}
					result = fileCellData.Substring(1, fileCellData.Length - 2).Replace("\"\"", "\"");
				}
				else
				{
					bool flag4 = fileCellData.Length > 2 && fileCellData[0] == '"';
					if (flag4)
					{
						fileCellData = fileCellData.Substring(1, fileCellData.Length - 2).Replace("\"\"", "\"");
					}
					result = fileCellData;
				}
			}
			return result;
		}
		/// <summary>
		/// 添加表数据到CSV文件中
		/// </summary>
		/// <param name="dataDT">表数据</param>
		/// <param name="beginCol">从第几列开始,beginCol = 1代表第一列</param>
		public void AddData(DataTable dataDT, int beginCol)
		{
			bool flag = dataDT == null;
			if (flag)
			{
				throw new Exception("需要添加的表数据为空");
			}
			int count = this.rowAL.Count;
			int num;
			for (int i = 0; i < dataDT.Rows.Count; i = num + 1)
			{
				for (int j = 0; j < dataDT.Columns.Count; j = num + 1)
				{
					this[count + i + 1, beginCol + j] = dataDT.Rows[i][j].ToString();
					num = j;
				}
				num = i;
			}
		}
		/// <summary>
		/// 保存数据,如果当前硬盘中已经存在文件名一样的文件，将会覆盖
		/// </summary>
		public void Save()
		{
			bool flag = this.fileName == null;
			if (flag)
			{
				throw new Exception("缺少文件名");
			}
			bool flag2 = this.encoding == null;
			if (flag2)
			{
				this.encoding = Encoding.Default;
			}
			try
			{
				using (FileStream fileStream = new FileStream(this.fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
				{
					using (StreamWriter streamWriter = new StreamWriter(fileStream, this.encoding))
					{
						int num;
						for (int i = 0; i < this.rowAL.Count; i = num + 1)
						{
							streamWriter.WriteLine(this.ConvertToSaveLine((ArrayList)this.rowAL[i]));
							num = i;
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString(), "文件写入失败");
			}
		}
		/// <summary>
		/// 保存数据,如果当前硬盘中已经存在文件名一样的文件，将会覆盖
		/// </summary>
		/// <param name="fileName">文件名,包括文件路径</param>
		public void Save(string fileName)
		{
			this.fileName = fileName;
			this.Save();
		}
		/// <summary>
		/// 保存数据,如果当前硬盘中已经存在文件名一样的文件，将会覆盖
		/// </summary>
		/// <param name="fileName">文件名,包括文件路径</param>
		/// <param name="encoding">文件编码</param>
		public void Save(string fileName, Encoding encoding)
		{
			this.fileName = fileName;
			this.encoding = encoding;
			this.Save();
		}
		/// <summary>
		/// 转换成保存行
		/// </summary>
		/// <param name="colAL">一行</param>
		/// <returns></returns>
		private string ConvertToSaveLine(ArrayList colAL)
		{
			string text = "";
			int num;
			for (int i = 0; i < colAL.Count; i = num + 1)
			{
                if (m_bInsertQuota)
                {
                    text += this.ConvertToSaveCell(colAL[i].ToString());
                }
                else
                {
                    text += colAL[i].ToString();
                }
				
				bool flag = i < colAL.Count - 1;
				if (flag)
				{
					text += ",";
				}
				num = i;
			}
			return text;
		}
		/// <summary>
		/// 字符串转换成CSV中的格子
		/// 双引号转换成两个双引号，然后首尾各加一个双引号
		/// 这样就不需要考虑逗号及换行的问题
		/// </summary>
		/// <param name="cell">格子内容</param>
		/// <returns></returns>
		private string ConvertToSaveCell(string cell)
		{
			cell = cell.Replace("\"", "\"\"");
			return "\"" + cell + "\"";
		}
	}
}
