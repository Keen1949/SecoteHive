using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Threading.Tasks;

namespace HelperTool
{
    /// <summary>
    /// 序列化类
    /// </summary>
    public class SerializerHelper
    {
        /// <summary>
        /// 把对象序列化为二进制进行保存
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="path"></param>
        public static void SaveObjectToFile(object obj, string path)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            using (FileStream fileStream = File.Create(path))
            {
                BinaryFormatter binFormat = new BinaryFormatter();
                binFormat.Serialize(fileStream, obj);
            }
        }

        /// <summary>
        /// 从二进制序列化文件加载对象
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static object LoadObjectFromFile(string path)
        {
            object obj;
            using (FileStream fileStream = File.OpenRead(path))
            {
                BinaryFormatter binFormat = new BinaryFormatter();
                obj = binFormat.Deserialize(fileStream);
            }

            return obj;
        }

        /// <summary>
        /// 深度拷贝
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepCopy<T>(T obj)
        {
            object retval;
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                //序列化成流
                bf.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                //反序列化成对象
                retval = bf.Deserialize(ms);
                ms.Close();
            }
            return (T)retval;
        }

        /// <summary>
        /// 二进制序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file"></param>
        /// <param name="obj"></param>
        public static void SerializeBinary<T>(string file, T obj) where T : class
        {
            Stream s = File.Open(file, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(s, obj);

            s.Close();
        }

        /// <summary>
        /// 二进制反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file"></param>
        /// <returns></returns>
        public static T DeserializeBinary<T>(string file) where T : class
        {
            Stream s = File.Open(file, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();

            object o = bf.Deserialize(s);

            s.Close();

            return o as T;
        }

        /// <summary>
        /// SOAP序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file"></param>
        /// <param name="obj"></param>
        public static void SerializeSoap<T>(string file, T obj) where T : class
        {
            Stream s = File.Open(file, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            SoapFormatter sf = new SoapFormatter();
            sf.Serialize(s, obj);

            s.Close();
        }

        /// <summary>
        /// SOAP反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file"></param>
        /// <returns></returns>
        public static T DeserializeSoap<T>(string file) where T : class
        {
            using (Stream s = File.Open(file, FileMode.Open))
            {
                SoapFormatter sf = new SoapFormatter();

                object o = sf.Deserialize(s);

                s.Close();

                return o as T;
            }
        }
       
    }
}
