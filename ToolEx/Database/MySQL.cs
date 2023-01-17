using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace ToolEx
{
    /// <summary>
    /// MySQL数据库
    /// </summary>
    public class MySQL : SqlBase
    {
        #region 字段
        private MySqlConnection m_conn;
        #endregion
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public MySQL() : base()
        {

        }
        #endregion

        /// <summary>
        /// 生成连接字符串
        /// </summary>
        /// <param name="strServer"></param>
        /// <param name="nPort"></param>
        /// <param name="strUserID"></param>
        /// <param name="strPassword"></param>
        /// <param name="strDatabase"></param>
        /// <returns></returns>
        public static string GenConnectString(string strServer, int nPort, string strUserID, string strPassword, string strDatabase = null)
        {
            string strConn;
            if (string.IsNullOrEmpty(strDatabase))
            {
                strConn = string.Format("server={0};port={1};user={2};password={3};",
                strServer, nPort, strUserID, strPassword, strDatabase
                );
            }
            else
            {
                strConn = string.Format("server={0};port={1};user={2};password={3};database={4};",
                strServer, nPort, strUserID, strPassword, strDatabase
                );
            }

            return strConn;
        }

        /// <summary>
        /// 生成创建表的字符串
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static string GenerateCreateTableString(Table table)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(@"CREATE TABLE IF NOT EXISTS {0}(", table.Name);
            sb.Append(@"ID INT NOT NULL AUTO_INCREMENT, ");

            string strPrimaryKey = "";
            foreach (var item in table)
            {
                if (item.DataType == typeof(DateTime))
                {
                    sb.AppendFormat(@"{0} DATETIME,", item.Name);
                }
                else if (item.DataType == typeof(double))
                {
                    sb.AppendFormat(@"{0} DOUBLE,", item.Name);
                }
                else if (item.DataType == typeof(float))
                {
                    sb.AppendFormat(@"{0} FLOAT,", item.Name);
                }
                else if (item.DataType == typeof(int))
                {
                    sb.AppendFormat(@"{0} INT,", item.Name);
                }
                else if (item.DataType == typeof(Int64))
                {
                    sb.AppendFormat(@"{0} BIGINT,", item.Name);
                }
                else if (item.DataType == typeof(short))
                {
                    sb.AppendFormat(@"{0} SMALLINT,", item.Name);
                }
                else if (item.DataType == typeof(byte))
                {
                    sb.AppendFormat(@"{0} TINYINT,", item.Name);
                }
                else if (item.DataType == typeof(bool))
                {
                    sb.AppendFormat(@"{0} CHAR(10),", item.Name);
                }
                else if (item.DataType == typeof(string))
                {
                    sb.AppendFormat(@"{0} VARCHAR(20),", item.Name);
                }

                if (item.Name == table.PrimaryKey)
                {
                    strPrimaryKey = table.PrimaryKey;
                }
            }

            if (string.IsNullOrEmpty(strPrimaryKey))
            {
                strPrimaryKey = "ID";
            }

            sb.AppendFormat(@"PRIMARY KEY ({0}))ENGINE=InnoDB DEFAULT CHARSET=utf8; ", strPrimaryKey);

            return sb.ToString();
        }

        /// <summary>
        /// 生成创建表的字符串
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static string GenerateInsertTableString(Table table)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(@"INSERT INTO {0} SET ", table.Name);

            foreach (var item in table)
            {
                if (item.DataType == typeof(DateTime))
                {
                    sb.AppendFormat(@"{0} = '{1}',", item.Name,((DateTime)item.Value).ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else if (item.DataType == typeof(double))
                {
                    sb.AppendFormat(@"{0} = '{1}',", item.Name, ((double)item.Value).ToString("F3"));
                }
                else
                {
                    sb.AppendFormat(@"{0} = '{1}',", item.Name, item.Value.ToString());
                }
            }

            string strSql = sb.ToString().TrimEnd(',') + ";";
           

            return strSql;
        }


        #region 重载方法
        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <param name="strConn"></param>
        /// <returns></returns>
        public override bool Connect(string strConn)
        {
            try
            {
                if (m_conn == null)
                {
                    m_conn = new MySqlConnection(strConn);
                }

                m_conn.Open();

                m_bIsConnected = true;
            }
            catch (Exception ex)
            {
                m_bIsConnected = false;
                MessageBox.Show(ex.Message);
            }

            return m_bIsConnected;
        }

        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <param name="strServer"></param>
        /// <param name="nPort"></param>
        /// <param name="strUserID"></param>
        /// <param name="strPassword"></param>
        /// <param name="strDatabase"></param>
        /// <returns></returns>
        public override bool Connect(string strServer, int nPort, string strUserID, string strPassword, string strDatabase = null)
        {
            string strConn;
            if (string.IsNullOrEmpty(strDatabase))
            {
                strConn = string.Format("server={0};port={1};user={2};password={3};",
                strServer, nPort, strUserID, strPassword, strDatabase
                );
            }
            else
            {
                strConn = string.Format("server={0};port={1};user={2};password={3};database={4};",
                strServer, nPort, strUserID, strPassword, strDatabase
                );
            }
            

            return Connect(strConn);
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public override void Disconnect()
        {
            if (m_bIsConnected && m_conn != null)
            {
                m_conn.Close();

                m_conn.Dispose();

                m_conn = null;
            }
        }

        /// <summary>
        /// 执行SQL语句返回DataTable,用于查询数据库
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public override DataTable ExecuteDataTable(string sql)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(sql, m_conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

                DataTable dt = new DataTable();

                adapter.Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                return null;
            }
        }

        /// <summary>
        /// 执行SQL语句,用于插入、更新和删除数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public override int ExecuteNonQuery(string sql)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(sql, m_conn);

                int result = cmd.ExecuteNonQuery();

                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                return -1;
            }
        }

        /// <summary>
        /// 用于查询数据时，返回查询结果集中第一行第一列的值，即只返回一个值。
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public override object ExecuteScalar(string sql)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(sql, m_conn);

                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                return null;
            }
        }

        /// <summary>
        /// 判断数据表是否存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public override bool IsTableExist(string tableName)
        {
            try
            {
                string strSql = string.Format(@"select * from information_schema.tables where table_name ='{0}';", tableName);

                DataTable dt = ExecuteDataTable(strSql);

                if (dt != null && dt.Rows.Count > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return false;
        }

        /// <summary>
        /// 判断数据库是否存在
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        public override bool IsDatabaseExist(string database)
        {
            try
            {
                string strSql = string.Format(@"select COUNT(*) from information_schema.schemata where schema_name='{0}';", database);

                DataTable dt = ExecuteDataTable(strSql);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    return row[0].ToString() != "0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return false;
        }

        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        public override bool CreateDataBase(string database)
        {
            try
            {
                string strSql = string.Format("CREATE DATABASE IF NOT EXISTS {0};", database);

                return ExecuteNonQuery(strSql) != -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

                return false;
            }
        }

        /// <summary>
        /// 创建数据表
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public override bool CreateTable(Table table)
        {
            try
            {
                string strSql = GenerateCreateTableString(table);

                return ExecuteNonQuery(strSql) != -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                return false;
            }
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public override bool AddTableData(Table table)
        {
            try
            {
                string strSql = GenerateInsertTableString(table);

                return ExecuteNonQuery(strSql) != -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                return false;
            }
        }

        /// <summary>
        /// 获取所有表的名称
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        public override string[] GetTables(string database)
        {
            try
            {
                string strSql = string.Format(@"SELECT table_name FROM information_schema.TABLES WHERE table_schema = '{0}';",database);
                DataTable dt = ExecuteDataTable(strSql);

                if (dt != null)
                {
                    string[] array = new string[dt.Rows.Count];

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        array[i] = dt.Rows[i][0].ToString();
                    }

                    return array;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);             
            }

            return null;
        }

        /// <summary>
        /// 获取表的所有表项
        /// </summary>
        /// <param name="database"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public override string[] GetTableColumns(string database, string tableName)
        {
            try
            {
                string strSql = string.Format(@"SELECT COLUMN_NAME FROM information_schema.COLUMNS WHERE table_name = '{0}' AND table_schema = '{1}';", 
                    tableName, database);
                DataTable dt = ExecuteDataTable(strSql);

                if (dt != null)
                {
                    string[] array = new string[dt.Rows.Count];

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        array[i] = dt.Rows[i][0].ToString();
                    }

                    return array;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return null;
        }
        #endregion
    }
}
