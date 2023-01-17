using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTool;
using ToolEx;
using AutoFrameDll;

namespace AutoFrame
{
    /// <summary>
    /// 手动安全操作
    /// </summary>
    public class ManulSafeMgr : SingletonTemplate<ManulSafeMgr>
    {

        public ManulSafeMgr()
        {
            ManaulTool.IsManulAxisMotionSafeEvent += IsManulMotionSafe;
            ManaulTool.IsManulIoSafeEvent += IsManulIoSafe;
            ManaulTool.IsManulStaMotionSafeEvent += IsManulStaMotionSafe;
            ManaulTool.IsCylinderSafeEvent += IsCylinderSafe;
        }


        /// <summary>
        /// 单轴运动判断是否安全
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public bool IsManulMotionSafe(int axis)
        {
            //单轴运动判断是否安全
            return true;
        }

        /// <summary>
        /// 点位运动判断是否安全
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        public bool IsManulStaMotionSafe(StationBase station)
        {
            return true;
        }

        /// <summary>
        /// IO控制是否安全
        /// </summary>
        /// <param name="strIoName"></param>
        /// <returns></returns>
        public bool IsManulIoSafe(string strIoName)
        {
            //安全操作
            //if (strIoName.Contains("压料顶升气缸升"))
            //{
            //    int nAxis = StationMgr.GetInstance().GetStation("转盘站").AxisU;

            //    //获取轴的状态
            //    if (MotionMgr.GetInstance().IsAxisNormalStop(nAxis) == -1)
            //    {
            //        //正在运行中，不能操作气缸
            //        MessageBox.Show("DD马达正在运行中，不能操作压料顶升气缸");
            //        return false;

            //    }
            //}

            return true;
        }

        /// <summary>
        /// 气缸操作是否安全
        /// </summary>
        /// <param name="cyl"></param>
        /// <returns></returns>
        public static bool IsCylinderSafe(Cylinder cyl)
        {
            //判断是否满足气缸动作条件，根据实际情况编写
            return true;
        }
    }
}
