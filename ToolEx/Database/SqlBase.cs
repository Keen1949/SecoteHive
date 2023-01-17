using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolEx
{
    /// <summary>
    /// 数据表
    /// </summary>
    public class TableColumn
    {
        /// <summary>
        /// 列名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 数据类型
        /// </summary>
        public Type DataType;

        /// <summary>
        /// 数值
        /// </summary>
        public object Value;
    }

    /// <summary>
    /// 数据表
    /// </summary>
    public class Table : IList<TableColumn>
    {
        private List<TableColumn> m_list = new List<TableColumn>();

        #region 重载方法
        public TableColumn this[int index]
        {
            get
            {
                return m_list[index];
            }

            set
            {
                m_list[index] = value;
            }
        }

        public int Count
        {
            get
            {
                return m_list.Count; ;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public void Add(TableColumn item)
        {
            m_list.Add(item);
        }

        public void Clear()
        {
            m_list.Clear();
        }

        public bool Contains(TableColumn item)
        {
            return m_list.Contains(item);
        }

        public void CopyTo(TableColumn[] array, int arrayIndex)
        {
            m_list.CopyTo(array,arrayIndex);
        }

        public IEnumerator<TableColumn> GetEnumerator()
        {
            return m_list.GetEnumerator();
        }

        public int IndexOf(TableColumn item)
        {
            return m_list.IndexOf(item);
        }

        public void Insert(int index, TableColumn item)
        {
            m_list.Insert(index,item);
        }

        public bool Remove(TableColumn item)
        {
            return m_list.Remove(item);
        }

        public void RemoveAt(int index)
        {
            m_list.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_list.GetEnumerator();
        }
        #endregion

        /// <summary>
        /// 数据表名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        public string PrimaryKey { get; set; }

        /// <summary>
        /// 添加列
        /// </summary>
        /// <param name="strColumn"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        public void Add(string strColumn,Type type,object value)
        {
            TableColumn col = new TableColumn();
            col.Name = strColumn;
            col.DataType = type;
            col.Value = value;

            Add(col);
        }
    }

    /// <summary>
    /// 数据库基类
    /// </summary>
    public abstract class SqlBase:IDisposable
    {
        #region 字段
        /// <summary>
        /// 数据库是否连接
        /// </summary>
        protected bool m_bIsConnected = false;

        #endregion
        #region 属性
        /// <summary>
        /// 数据库是否连接
        /// </summary>
        public bool IsConnected
        {
            get
            {
                return m_bIsConnected;
            }
        }

        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public SqlBase()
        {

        }
        #endregion

        #region 方法
        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public abstract bool Connect(string strConn);

        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <returns></returns>
        public abstract bool Connect(string strServer,int nPort,string strUserID,string strPassword,string strDatabase);

        /// <summary>
        /// 断开连接
        /// </summary>
        public abstract void Disconnect();

        /// <summary>
        /// 执行SQL语句,用于插入、更新和删除数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public abstract int ExecuteNonQuery(string sql);

        /// <summary>
        /// 执行SQL语句返回DataTable,用于查询数据库
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public abstract DataTable ExecuteDataTable(string sql);

        /// <summary>
        /// 用于查询数据时，返回查询结果集中第一行第一列的值，即只返回一个值。
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public abstract object ExecuteScalar(string sql);

        /// <summary>
        /// 判断数据表是否存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public abstract bool IsTableExist(string tableName);

        /// <summary>
        /// 判断数据库是否存在
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        public abstract bool IsDatabaseExist(string database);

        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        public abstract bool CreateDataBase(string database);

        /// <summary>
        /// 创建数据表
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public abstract bool CreateTable(Table table);

        /// <summary>
        /// 删除表
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public virtual bool  DropTable(string tableName)
        {
            string sql = string.Format(@"DROP TABLE {0};", tableName);
            int ret = ExecuteNonQuery(sql);

            return ret != -1;
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public abstract bool AddTableData(Table table);

        /// <summary>
        /// 获取所有表的名称
        /// </summary>
        /// <param name="database"
        /// <returns></returns>
        public abstract string[] GetTables(string database);

        /// <summary>
        /// 获取表的所有表项
        /// </summary>
        /// <param name="database"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public abstract string[] GetTableColumns(string database,string tableName);

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Disconnect();
        }

        #endregion
    }
}
