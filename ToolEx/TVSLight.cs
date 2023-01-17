using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFrameDll;
using Communicate;

namespace ToolEx
{
    class TVSLight : LightBase
    {
        //命令定义：
        //读EEPROM配置             0x02
        //设置单通道亮度           0x31 
        //设置单通道工作模式       0x32 
        //设置4通道亮度            0x33  
        //设置4通道工作模式        0x34 
        //数据格式：起始位高字节(0xab)+起始位低字节(0xba)+数据长度(从命令开始到最后的数组字节数)+命令+数据

        private ComLink m_comLink;

        public TVSLight(string strName,int nChannel,int nCommIndex) : base(strName,nChannel,nCommIndex)
        {
            LightOff();
        }

        public override void LightOff()
        {
            //通道0，亮度为0
            byte[] cmd = new byte[] { 0xAB, 0xBA, 0x03, 0x31, 0x00, 0x00 };

            cmd[4] = (byte)m_nChannel;
            cmd[5] = (byte)0;

            if (m_bInit)
            {
                m_comLink.Lock();

                
                if (m_comLink.WriteData(cmd, cmd.Length))
                {
                    byte[] data = new byte[2];
                    m_comLink.ReadData(data, data.Length);
                }

                m_comLink.UnLock();
            }
        }

        public override bool LightOn(int nLight = 255)
        {
            //通道0，亮度为0
            byte[] cmd = new byte[] { 0xAB, 0xBA, 0x03, 0x31, 0x00, 0x00 };

            cmd[4] = (byte)m_nChannel;
            cmd[5] = (byte)nLight;

            if (m_bInit)
            {
                m_comLink.Lock();
                bool bRet = m_comLink.WriteData(cmd, cmd.Length);
                if (bRet)
                {
                    byte[] data = new byte[2];
                    m_comLink.ReadData(data, data.Length);
                }

                m_comLink.UnLock();

                return bRet;
            }

            return false;
        }

        public override void DeInit()
        {
            LightOff();
        }

        public override bool Init()
        {
            bool bRet = false;
            m_comLink = ComMgr.GetInstance().GetComLink(m_nCommIndex);
            if (m_comLink != null)
            {
                m_comLink.Lock();
                if (!m_comLink.IsOpen())
                {
                    m_comLink.Open();

                    //建立通信
                    byte[] cmd = { 0xAB, 0xBA, 0x02, 0x01, 0x00 };
                    byte[] SuccessRet = { 0xCD, 0xDC, 0x02, 0x50, 0x01 };

                    if (m_comLink.WriteData(cmd, cmd.Length))
                    {
                        byte[] data = new byte[SuccessRet.Length];
                        m_comLink.ReadData(data, data.Length);

                        //比较接收数据

                        for (int i = 0; i < SuccessRet.Length;i++)
                        {
                            if (data[i] != SuccessRet[i])
                            {
                                return false;
                            }
                        }

                        m_bInit = bRet = true;
                    }
                }
                else
                {
                    m_bInit = bRet = true;
                }
                m_comLink.UnLock();
            }

            return bRet;
        }
    }
}
