using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFrameDll;
using ToolEx;
using CommonTool;
using Communicate;
using System.Threading;
namespace AutoFrame
{
    /// <summary>
    /// 只针对欧姆龙V680S
    /// </summary>
    public class RfidCtrl
    {
        public TcpLink tcp;

        public RfidCtrl()
        {
        }

        public RfidCtrl(int nTcpIdx)
        {
            this.tcp = TcpMgr.GetInstance().GetTcpLink(nTcpIdx);
        }

        public bool Init()
        {
            return this.tcp.Open();
        }

        public void DeInit()
        {
            this.tcp.Close();
        }

        public bool ChangeMode(out string str)
        {
            byte[] byteData = new byte[15];
            byteData[0] = 0x00;
            byteData[1] = 0x00;
            byteData[2] = 0x00;
            byteData[3] = 0x00;
            byteData[4] = 0x00;
            byteData[5] = 0x09;
            byteData[6] = 0xFF;
            byteData[7] = 0x10;
            byteData[8] = 0xB0;
            byteData[9] = 0x00;
            byteData[10] = 0x00;
            byteData[11] = 0x01;
            byteData[12] = 0x02;
            byteData[13] = 0x00;
            byteData[14] = 0x00;//13,14 0000:ONCE 0001:自动 0002:FIFO
            this.tcp.WriteData(byteData, byteData.Length);
            return ReceiveFromRDID(out str);
        }
        /// <summary>
        /// 获取读取数据请求的字节数组
        /// </summary>
        /// <param name="startByte1">读取寄存器起始位的高位</param>
        /// <param name="startByte2">读取寄存器起始位的低位</param>
        /// <param name="endByte1">读取的寄存器数的高位</param>
        /// <param name="endByte2">读取寄存器数的低位,0x0001代表读取一个寄存器,两个字节,两个char</param>
        /// <returns>返回请求字节数组的string格式</returns>
        public bool Read(out string strRead, byte startByte1 = 0x00, byte startByte2 = 0x00, byte endByte1 = 0x00, byte endByte2 = 0x02)
        {
            if (!this.tcp.IsOpen())
            {
                this.tcp.Close();
                Thread.Sleep(150);
                this.tcp.Open();
            }

            this.tcp.ClearBuffer();
            byte[] byteData = new byte[15];
            byteData[0] = 0x00;
            byteData[1] = 0x00;
            byteData[2] = 0x00;
            byteData[3] = 0x00;
            byteData[4] = 0x00;
            byteData[5] = 0x06;
            byteData[6] = 0xFF;
            byteData[7] = 0x03;
            byteData[8] = startByte1;
            byteData[9] = startByte2;
            byteData[10] = endByte1;
            byteData[11] = endByte2;//一个字代表一个寄存器,存两个字节,用Ascii可以存两个char.

            strRead = "";
            this.tcp.WriteData(byteData, byteData.Length);
            Thread.Sleep(150);
            return ReceiveFromRDID(out strRead);
        }

        /// <summary>
        /// 获得写入数据请求的字节数组
        /// </summary>
        /// <param name="writeData">要写入的字符串,长度为奇数时自动补充</param>
        /// <param name="startByte1"写入寄存器地址高位</param>
        /// <param name="startByte2">写入寄存器地址低位</param>
        /// <returns>返回要写入的字节数组的字符串格式</returns>
        public bool Write(string strWrite, out string strRtn, byte startByte1 = 0x00, byte startByte2 = 0x00)
        {
            byte[] byteData = new byte[100];
            byte[] tmp = Encoding.ASCII.GetBytes(strWrite);
            byte byteLength = (byte)tmp.Length;
            byteData[0] = 0x00;
            byteData[1] = 0x00;
            byteData[2] = 0x00;
            byteData[3] = 0x00;
            byteData[4] = 0x00;
            byteData[5] = (byte)(0x03 + byteLength);
            byteData[6] = 0xFF;
            byteData[7] = 0x10;
            byteData[8] = startByte1;
            byteData[9] = startByte2;
            byteData[10] = 0x00;
            byteData[11] = 0x02;
            byteData[12] = 0x04;
            //for (int i = 0; i < tmp.Length; i++)
            //    byteData[13 + i] = tmp[i];
            byteData[13] = Convert.ToByte(strWrite.Substring(0, 2), 16);
            byteData[14] = Convert.ToByte(strWrite.Substring(2, 2), 16);
            byteData[15] = Convert.ToByte(strWrite.Substring(4, 2), 16);
            byteData[16] = Convert.ToByte(strWrite.Substring(6, 2), 16);
            byte[] request = byteData.ToList().GetRange(0, 15 + byteLength).ToArray();

            strRtn = "";
            this.tcp.WriteData(request, request.Length);
            return ReceiveFromRDID(out strRtn);
        }
        private bool ReceiveFromRDID(out string str)
        {
            str = "";
            byte[] tmp = new byte[100];
            byte[] tmp2;
            this.tcp.ReadData(tmp, 100);
            switch (tmp[7])
            {
                case 0x03: //读取数据
                    int byteLongth = tmp[8] * 2;//tmp[8]代表字长度,字节数*2
                    tmp2 = tmp.ToList().GetRange(0, 7 + byteLongth).ToArray();
                    str = ByteToString(tmp2);
                    tmp2 = tmp.ToList().GetRange(9, byteLongth).ToArray();

                    //                     byte[] tmp2 = new byte[byteLongth];
                    //                     client.ReadData(tmp2, byteLongth);
                    //str +=","/*+ *//*Encoding.ASCII.GetString(tmp2)*/;
                    return true;

                case 0x03 + 0x80:
                    tmp2 = tmp.ToList().GetRange(0, 9).ToArray();
                    str = ByteToString(tmp2);
                    // str +=","+ ErrorCode(tmp[8]);
                    return false;

                case 0x10:
                    tmp2 = tmp.ToList().GetRange(0, 12).ToArray();
                    str = ByteToString(tmp2);
                    //tmp2 = new byte[3];
                    //client.ReadData(tmp2, tmp2.Length);
                    str += "," + "写入成功";
                    return true;
                case 0x10 + 0x80:
                    tmp2 = tmp.ToList().GetRange(0, 9).ToArray();
                    str = ByteToString(tmp2);
                    //str += ","+ ErrorCode(tmp[8]);
                    return false;
                default:
                    str = "Nothing,未知反馈";
                    return false;
            }
        }
        private string ByteToString(byte[] by)
        {
            string s = "";
            foreach (var item in by)
            {
                s += Convert.ToString(item, 16).PadLeft(2, '0').ToUpper() + " ";
            }
            s = s.TrimEnd();
            return s;
        }
        private string ErrorCode(byte tmp)
        {
            switch (tmp)
            {
                case 0x00:
                    return "正常结束";
                case 0x01:
                    return "无效功能";
                case 0x02:
                    return "无效数据地址";
                case 0x03:
                    return "无效数据值";
                case 0x04:
                    return "从站设备失败";
                case 0x06:
                    return "从站设备繁忙";
                default:
                    return "未知错误";
            }
        }

    }
}
