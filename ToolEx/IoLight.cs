using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFrameDll;
using CommonTool;

namespace ToolEx
{
    class IoLight : LightBase
    {
        public IoLight(string strName,int nChannel,int nCommIndex) : base(strName,-1,-1)
        {
            LightOff();
        }

        public override bool Init()
        {
            LightOff();

            return true;
        }

        public override void DeInit()
        {
            LightOff();
        }

        public override bool LightOn(int nLight = 0)
        {
            IoMgr.GetInstance().WriteIoBit(m_strName, true);
            return true;
        }

        public override void LightOff()
        {
            IoMgr.GetInstance().WriteIoBit(m_strName, false);
        }

        
    }
}
