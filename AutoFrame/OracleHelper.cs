using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using System.Data.OracleClient;

namespace AutoFrame
{
    class OracleHelper
    {
        #region 判断连接是否成功  
        /// <summary>  
        ///     判断连接是否成功！  
        /// </summary>  
        /// <param name="con"> 链接字符串</param>  
        /// <returns>true 表示链接成功，false表示连接失败</returns>  
        public static bool IsConnected(string con)
        {
            bool flag;
            var conn = new OracleConnection(con);
            try
            {
                conn.Open();
                flag = true;
            }
            catch (Exception)
            {
                flag = false;
            }
            finally
            {
                conn.Close();
            }
            return flag;
        }
        #endregion
        #region 执行不带参数sql语句  
        /// <summary>  
        ///     执行不带参数sql语句  
        /// </summary>  
        /// <param name="sql">增，删，改sql语句</param>  
        /// <param name="con"></param>  
        /// <returns>返回所影响的行数</returns>  
        public static bool Execute(string sql, string con)
        {
            var cmd = new OracleCommand();
            var connection = new OracleConnection(con);
            try
            {
                using (connection)
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sql;
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
        #region 执行SQL语句返回DataTable  
        /// <summary>  
        ///     执行SQL语句返回DataTable  
        /// </summary>  
        /// <param name="sql"></param>  
        /// <param name="con"></param>  
        /// <returns></returns>  
        public static DataTable ExcuteDataTable(string sql, string con)
        {
            var cmd = new OracleCommand();
            var connection = new OracleConnection(con);
            try
            {
                using (connection)
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sql;
                    connection.Open();
                    var da = new OracleDataAdapter(cmd);
                    var ds = new DataSet();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                var dt = new DataTable();
                dt.Columns.Add("异常信息");
                DataRow row = dt.NewRow();
                row["异常信息"] = ex.Message;
                dt.Rows.Add(row);
                return dt;
            }
        }
        #endregion
        #region 执行SQL语句查询单条记录  
        /// <summary>  
        ///     执行SQL语句查询单条记录  
        /// </summary>  
        /// <param name="sql"></param>  
        /// <param name="con"></param>  
        /// <returns></returns>  
        public static object ExecuteScalar(string sql, string con)
        {
            var cmd = new OracleCommand();
            var connection = new OracleConnection(con);
            try
            {
                using (connection)
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sql;
                    connection.Open();
                    return cmd.ExecuteScalar();
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        #endregion
        #region 取得表最大Id  
        /// <summary>  
        ///     取得表最大Id  
        /// </summary>  
        /// <param name="tableName"></param>  
        /// <param name="fieldName"></param>  
        /// <param name="con"></param>  
        /// <returns></returns>  
        public static int GetMaxId(string tableName, string fieldName, string con)
        {
            string sql = "SELECT NVL(MAX({0}),0)+1 FROM {1}";
            try
            {
                sql = string.Format(sql, fieldName, tableName);
                int result;
                Int32.TryParse(ExecuteScalar(sql, con).ToString(), out result);
                return result;
            }
            catch (Exception)
            {
                return -1;
            }
        }
        #endregion
    }
}
