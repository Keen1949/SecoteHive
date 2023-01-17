using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolEx
{
    /// <summary>
    /// 通用数据类型
    /// </summary>
    public class TValue : ICloneable
    {
        private object m_oValue = "";

        /// <summary>
        /// 构造函数
        /// </summary>
        public TValue()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="v"></param>
        public TValue(int v)
        {
            m_oValue = v;
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="v"></param>
        public TValue(double v)
        {
            m_oValue = v;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="v"></param>
        public TValue(bool v)
        {
            m_oValue = v;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="v"></param>
        public TValue(string v)
        {
            m_oValue = v;
        }


        /// <summary>
        /// 转换为int型数据
        /// </summary>
        public int I
        {
            get
            {
                int v = 0;
                if (int.TryParse(m_oValue.ToString(),out v))
                {
                    return v;
                }

                return 0;
            }
        }

        /// <summary>
        /// 转换为double型数据
        /// </summary>
        public double D
        {
            get
            {
                double v = 0;
                if (double.TryParse(m_oValue.ToString(), out v))
                {
                    return v;
                }

                return 0;
            }
        }

        /// <summary>
        /// 转换为bool型数据
        /// </summary>
        public bool B
        {
            get
            {
                bool v = false;
                if (bool.TryParse(m_oValue.ToString(), out v))
                {
                    return v;
                }

                return false;
            }
        }

        /// <summary>
        /// 转换为string型数据
        /// </summary>
        public string S
        {
            get
            {
                return m_oValue.ToString();
            }
        }

        /// <summary>
        /// 是否是数字
        /// </summary>
        public bool IsNumber
        {
            get
            {
                double v = 0;
                return double.TryParse(m_oValue.ToString(), out v);
            }
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        public bool IsNullOrEmpty
        {
            get
            {
                if (m_oValue == null)
                {
                    return true;
                }
                else
                {
                    return string.IsNullOrEmpty(m_oValue.ToString());
                }
            }
        }

        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return m_oValue.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 拷贝
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        /// <summary>
        /// int型数据隐式转换为TValue类型
        /// </summary>
        /// <param name="v"></param>
        public static implicit operator TValue(int v)
        {
            return new TValue(v);
        }

        /// <summary>
        /// double型数据隐式转换为TValue类型
        /// </summary>
        /// <param name="v"></param>
        public static implicit operator TValue(double v)
        {
            return new TValue(v);
        }

        /// <summary>
        /// bool型数据隐式转换为TValue类型
        /// </summary>
        /// <param name="v"></param>
        public static implicit operator TValue(bool v)
        {
            return new TValue(v);
        }

        /// <summary>
        /// string型数据隐式转换为TValue类型
        /// </summary>
        /// <param name="v"></param>
        public static implicit operator TValue(string v)
        {
            return new TValue(v);
        }
 
        /// <summary>
        /// 重载+运算
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static TValue operator + (TValue v1,int v2)
        {
            if (v1.IsNumber)
            {
                return new TValue(v1.I + v2);
            }
            else
            {
                return new TValue(v2);
            }
        }

        /// <summary>
        /// 重载+运算
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static TValue operator +(TValue v1, double v2)
        {
            if (v1.IsNumber)
            {
                return new TValue(v1.D + v2);
            }
            else
            {
                return new TValue(v2);
            }
        }

        /// <summary>
        /// 重载+运算
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static TValue operator +(TValue v1, string v2)
        {
            if (v1.IsNumber)
            {
                return new TValue(v2);
            }
            {
                return new TValue(v1.S + v2);
            }
        }

        /// <summary>
        /// 重载>运算
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator >(TValue v1, int v2)
        {
            if (v1.IsNumber)
            {
                return v1.I > v2;
            }
            {
                return false;
            }
        }

        /// <summary>
        /// 重载>运算
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator >(TValue v1, double v2)
        {
            if (v1.IsNumber)
            {
                return v1.D > v2;
            }
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator >=(TValue v1, int v2)
        {
            if (v1.IsNumber)
            {
                return v1.I > v2;
            }
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator >=(TValue v1, double v2)
        {
            if (v1.IsNumber)
            {
                return v1.D > v2;
            }
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator <(TValue v1, int v2)
        {
            if (v1.IsNumber)
            {
                return v1.I < v2;
            }
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator <(TValue v1, double v2)
        {
            if (v1.IsNumber)
            {
                return v1.D < v2;
            }
            {
                return false;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator <=(TValue v1, int v2)
        {
            if (v1.IsNumber)
            {
                return v1.I <= v2;
            }
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator <=(TValue v1, double v2)
        {
            if (v1.IsNumber)
            {
                return v1.D <= v2;
            }
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator ==(TValue v1, TValue v2)
        {
            return v1.ToString() == v2.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator !=(TValue v1, TValue v2)
        {
            return v1.ToString() != v2.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator ==(TValue v1, int v2)
        {
            if (v1.IsNumber)
            {
                return v1.I == v2;
            }
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator ==(TValue v1, double v2)
        {
            if (v1.IsNumber)
            {
                return v1.D == v2;
            }
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator !=(TValue v1, int v2)
        {
            if (v1.IsNumber)
            {
                return v1.I != v2;
            }
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator !=(TValue v1, double v2)
        {
            if (v1.IsNumber)
            {
                return v1.D != v2;
            }
            {
                return false;
            }
        }
    }
}
