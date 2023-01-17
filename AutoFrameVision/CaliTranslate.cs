//2019-01-23 Binggoo 1. 把原来9点标定改为11点标定，并计算旋转中心。
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using AutoFrameDll;

namespace AutoFrameVision
{
    /// <summary>
    /// 用于九点标定的标定转换类
    /// </summary>
    public class CaliTranslate
    {
        /// <summary>
        /// 计算变化的矩阵
        /// </summary>
        HTuple m_hommatId;
        /// <summary>
        /// 九点标定的X像素座标
        /// </summary>
        HTuple m_xPixel = new HTuple();
        /// <summary>
        /// 九点标定的Y像素座标
        /// </summary>
        HTuple m_yPixel = new HTuple();
        /// <summary>
        /// 九点标定的X物理座标
        /// </summary>
        HTuple m_xPos = new HTuple();
        /// <summary>
        /// 九点标定的Y物理座标
        /// </summary>
        HTuple m_yPos = new HTuple();

        public HTuple m_PixWidth = 0;

        public HTuple m_DataCenter;
        public HTuple m_xCenter=0;
        public HTuple m_yCenter=0;

        public HTuple m_xMaxOffset=0;
        public HTuple m_yMaxOffset=0;

        HDevelopExport hde = new HDevelopExport();

        /// <summary>
        /// 构造函数,预先定义矩阵ID
        /// </summary>
        public CaliTranslate()
        {
            HOperatorSet.HomMat2dIdentity(out m_hommatId);
        }

        /// <summary>
        /// 应用九点标定的矩阵进行座标转换
        /// </summary>
        /// <param name="xIn"></param>
        /// <param name="yIn"></param>
        /// <param name="xOut"></param>
        /// <param name="yOut"></param>
        /// <returns></returns>
        public bool Translate(double xIn, double yIn, out double xOut, out double yOut)
        {
           HTuple x, y;
            try
            {
                HOperatorSet.AffineTransPoint2d(m_hommatId, xIn, yIn, out x, out y);
            }
            catch (HalconException HDevExpDefaultException1)
            {
                System.Windows.Forms.MessageBox.Show(HDevExpDefaultException1.ToString());
                xOut = yOut = 0;
                return false;
            }
            xOut = x.D;
            yOut = y.D;
            return true;
        }

        /// <summary>
        /// 清空坐标映射数据及中心数据
        /// </summary>
        public void ClearAllData()
        {
            ClearPointData();

            m_xCenter = 0;
            m_yCenter = 0;
            m_PixWidth = 0;
            m_xMaxOffset = 0;
            m_yMaxOffset = 0;

            m_DataCenter = new HTuple();
        }
        /// <summary>
        /// 清楚坐标映射数据
        /// </summary>
        public void ClearPointData()
        {
            m_xPixel = new HTuple(); 
            m_yPixel = new HTuple(); 
            m_xPos = new HTuple(); 
            m_yPos = new HTuple(); 
        }

        public void InitData()
        {
            ClearAllData();
            for (int i = 0; i < 11; i++)
            {
                m_xPixel.Append(0);
                m_yPixel.Append(0);
                m_xPos.Append(0);
                m_yPos.Append(0);
            }

            for(int i=0; i<5; ++i)
            {
                m_DataCenter.Append(0);
            }
        }

        /// <summary>
        /// 存储的座标的个数
        /// </summary>
        /// <returns></returns>
        public int GetDataLength()
        {
            return m_xPixel.Length;
        }
        /// <summary>
        /// 获取指定行列的座标
        /// </summary>
        /// <param name="Row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public string GetData(int Row, int col)
        {
            if(Row < m_xPixel.Length && col < 4)
            {
                if (col == 0)
                    return m_xPixel[Row].D.ToString();
                else if (col == 1)
                    return m_yPixel[Row].D.ToString();
                else if (col == 2)
                    return m_xPos[Row].D.ToString();
                else if (col == 3)
                    return m_yPos[Row].D.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// 向座标列表中加入座标映射数据
        /// </summary>
        /// <param name="xPixel"></param>
        /// <param name="yPixel"></param>
        /// <param name="xPos"></param>
        /// <param name="yPos"></param>
        public void AppendPointData(double xPixel, double yPixel, double xPos, double yPos)
        {
            m_xPixel.Append(xPixel);
            m_yPixel.Append(yPixel);
            m_xPos.Append(xPos);
            m_yPos.Append(yPos);
        }

        public void AppendRotateData(double x1, double y1, double x2, double y2, double angle)
        {
            m_DataCenter.Append(x1);
            m_DataCenter.Append(y1);
            m_DataCenter.Append(x2);
            m_DataCenter.Append(y2);
            m_DataCenter.Append(angle);
        }


        /// <summary>
        /// 根据九点标定的数据计算矩阵,像素大小，同时计算旋转中心，并计算标定的评估数据
        /// </summary>
        public bool CalcCalib()
        {
            m_hommatId = 0;
            try
            {
                BuildPointMap();
                double detlaX = (m_xPixel[8] - m_xPixel[0]);
                double detlaY = (m_yPixel[8] - m_yPixel[0]);
                double lengthPixel = Math.Sqrt(detlaX * detlaX + detlaY * detlaY);

                detlaX = (m_xPos[8] - m_xPos[0]);
                detlaY = (m_yPos[8] - m_yPos[0]);

                double lengthPos = Math.Sqrt(detlaX * detlaX + detlaY * detlaY);
                m_PixWidth = lengthPos / lengthPixel;
                CalcRotateCenter();

                GetMaxOffset(out m_xMaxOffset, out m_yMaxOffset);
            }
            catch (HalconException HDevExpDefaultException1)
            {
                System.Windows.Forms.MessageBox.Show(HDevExpDefaultException1.ToString());
                return false;
            }
            return true;
        }

        private void CalcRotateCenter()
        {            
             //hde.CalcRotateCenter(m_DataCenter[0], m_DataCenter[1], m_DataCenter[2], m_DataCenter[3], m_DataCenter[4], 
             //    out m_xCenter, out m_yCenter);

            if (m_xPixel.Length >= 11 && m_yPixel.Length >= 11)
            {

                HTuple Rows = new HTuple();
                HTuple Cols = new HTuple();

                for(int i = 9;i < 11;i++)
                {
                    Rows.Append(m_xPixel[i]);
                    Cols.Append(m_yPixel[i]);
                }

                HTuple CenterRow, CenterCol, Radius;

                CalcRotateCenter(Rows, Cols, out CenterRow, out CenterCol, out Radius);

                m_xCenter = CenterRow;
                m_yCenter = CenterCol;
            }
        }

        private void CalcRotateCenter(HTuple Rows,HTuple Cols,out HTuple CenterRow,out HTuple CenterCol,out HTuple Radius)
        {
            //根据垂直平分线求旋转中心
            CenterRow = 999;
            CenterCol = 999;
            Radius = 999;

            try
            {
                //求2个线段的中点
                HTuple CC1, CC2, RC1, RC2,Len1,Len2,Phi1,Phi2;
                HOperatorSet.LinePosition(Rows[0], Cols[0], Rows[1], Cols[1], out RC1, out CC1, out Len1, out Phi1);
                HOperatorSet.LinePosition(Rows[0], Cols[0], Rows[2], Cols[2], out RC2, out CC2, out Len2, out Phi2);

                //以线段中点为圆心，任意端点旋转90度
                HTuple hom2D1, hom2D2;
                HTuple R1, C1, R2, C2;
                HTuple RotPhi = 90;

                HOperatorSet.HomMat2dIdentity(out hom2D1);
                HOperatorSet.HomMat2dRotate(hom2D1, RotPhi.TupleRad(), RC1, CC1, out hom2D1);
                HOperatorSet.AffineTransPoint2d(hom2D1, Rows[0], Cols[0], out R1, out C1);

                HOperatorSet.HomMat2dIdentity(out hom2D2);
                HOperatorSet.HomMat2dRotate(hom2D2, RotPhi.TupleRad(), RC2, CC2, out hom2D2);
                HOperatorSet.AffineTransPoint2d(hom2D2, Rows[0], Cols[0], out R2, out C2);

                //求垂直平分线的交点
                HTuple IsOverlap;
                HOperatorSet.IntersectionLines(R1, C1, RC1, CC1, R2, C2, RC2, CC2, out CenterRow, out CenterCol, out IsOverlap);

                //求旋转半径
                HOperatorSet.DistancePp(CenterRow, CenterCol, Rows[0], Cols[0], out Radius);

            }
            catch (Exception)
            {
                CenterRow = 999;
                CenterCol = 999;
                Radius = 999;
            }
        }

        /// <summary>
        /// 已知旋转中心点，将要被旋转的点和需要旋转的角度，计算旋转后的点的位置
        /// </summary>
        /// <param name="rowCenter"></param>
        /// <param name="colCenter"></param>
        /// <param name="rowOld"></param>
        /// <param name="colOld"></param>
        /// <param name="angle"></param>
        /// <param name="rowNew"></param>
        /// <param name="colNew"></param>
        public void CalcRotatePoint(double rowOld, double colOld, double angle,out HTuple rowNew,out HTuple colNew)
        {
            hde.CalcRotatePoint(m_xCenter, m_yCenter, rowOld, colOld, angle, out rowNew, out colNew);
        }

        /// <summary>
        /// 计算标定转换后的最大偏差值
        /// </summary>
        /// <param name="xOffset"></param>
        /// <param name="yOffset"></param>
        /// <returns></returns>
        private bool GetMaxOffset(out HTuple xOffset, out HTuple yOffset)
        {
            HTuple hommatId = 0;
            HTuple xPix, yPix;
            try
            {
                HOperatorSet.HomMat2dIdentity(out hommatId);

                HTuple Px = new HTuple();
                HTuple Py = new HTuple();
                HTuple Qx = new HTuple();
                HTuple Qy = new HTuple();

                for (int i = 0; i < 9; i++)
                {
                    Qx.Append(m_xPixel[i]);
                    Qy.Append(m_yPixel[i]);

                    Px.Append(m_xPos[i]);
                    Py.Append(m_yPos[i]);
                }

                //HOperatorSet.VectorToHomMat2d(m_xPos, m_yPos, m_xPixel, m_yPixel, out hommatId);
                //HOperatorSet.AffineTransPoint2d(hommatId, m_xPos, m_yPos, out xPix , out yPix);
                HOperatorSet.VectorToHomMat2d(Px, Px, Qx, Qy, out hommatId);

                HOperatorSet.AffineTransPoint2d(hommatId, Px, Py, out xPix , out yPix);

                xPix =  xPix.TupleSub(Qx);
                yPix = yPix.TupleSub(Qy);

                xOffset = xPix.TupleMax().D;
                yOffset = yPix.TupleMax().D;
            }
            catch (HalconException HDevExpDefaultException1)
            {
                System.Windows.Forms.MessageBox.Show(HDevExpDefaultException1.ToString());
                xOffset = 9999;
                yOffset = 9999;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 保存标定的数据
        /// </summary>
        /// <param name="strPath"></param>
        public void SaveCaliData(string strPath)
        {
            if (m_xPixel.Length == 11 && m_xCenter.Length == 1)
            {
                string strFile = strPath;
                //for (int i=0; i<9; i++)
                //    IniOperation.WriteValue(strFile, "PixelX", "xPixel_"+string.Format("{0}",i), m_xPixel[i].D.ToString("F6"));
                //for (int i = 0; i < 9; i++)
                //    IniOperation.WriteValue(strFile, "PixelY", "yPixel_" + string.Format("{0}", i), m_yPixel[i].D.ToString("F6"));
                //for (int i = 0; i < 9; i++)
                //    IniOperation.WriteValue(strFile, "PosX", "xPos_" + string.Format("{0}", i), m_xPos[i].D.ToString("F6"));
                //for (int i = 0; i < 9; i++)
                //    IniOperation.WriteValue(strFile, "PosY", "yPos_" + string.Format("{0}", i), m_yPos[i].D.ToString("F6"));

                for (int i = 0; i < 11;i++)
                {
                    IniOperation.WriteValue(strFile, "PixelX", "xPixel_" + string.Format("{0}", i), m_xPixel[i].D.ToString("F6"));
                    IniOperation.WriteValue(strFile, "PixelY", "yPixel_" + string.Format("{0}", i), m_yPixel[i].D.ToString("F6"));
                    IniOperation.WriteValue(strFile, "PosX", "xPos_" + string.Format("{0}", i), m_xPos[i].D.ToString("F6"));
                    IniOperation.WriteValue(strFile, "PosY", "yPos_" + string.Format("{0}", i), m_yPos[i].D.ToString("F6"));
                }

                //IniOperation.WriteValue(strFile, "RoateCenter", "x1", m_DataCenter[0].D.ToString("F6"));
                //IniOperation.WriteValue(strFile, "RoateCenter", "y1", m_DataCenter[1].D.ToString("F6"));
                //IniOperation.WriteValue(strFile, "RoateCenter", "x2", m_DataCenter[2].D.ToString("F6"));
                //IniOperation.WriteValue(strFile, "RoateCenter", "y2", m_DataCenter[3].D.ToString("F6"));
                //IniOperation.WriteValue(strFile, "RoateCenter", "angle", m_DataCenter[4].D.ToString("F6"));

                IniOperation.WriteValue(strFile, "Calib", "xCenter", m_xCenter.D.ToString("F6"));
                IniOperation.WriteValue(strFile, "Calib", "yCenter", m_yCenter.D.ToString("F6"));
                IniOperation.WriteValue(strFile, "Calib", "xMaxOffset", m_xMaxOffset.D.ToString("F10"));
                IniOperation.WriteValue(strFile, "Calib", "yMaxOffset", m_yMaxOffset.D.ToString("F10"));
                IniOperation.WriteValue(strFile, "Calib", "PixWidth", m_PixWidth.D.ToString("F10"));
            }
        }

        private void   BuildPointMap()
        {
            HTuple Px = new HTuple();
            HTuple Py = new HTuple();
            HTuple Qx = new HTuple();
            HTuple Qy = new HTuple();

            for (int i = 0; i < 9;i++)
            {
                Px.Append(m_xPixel[i]);
                Py.Append(m_yPixel[i]);

                Qx.Append(m_xPos[i]);
                Qy.Append(m_yPos[i]);
            }

            //HOperatorSet.VectorToHomMat2d(m_xPixel, m_yPixel, m_xPos, m_yPos, out m_hommatId);
            HOperatorSet.VectorToHomMat2d(Px, Py, Qx, Qy, out m_hommatId);
        }


        /// <summary>
        /// 读取标定的数据
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        public bool LoadCaliData(string strPath)
        {
            try
            {
                string strFile = strPath;
                InitData();
                for (int i = 0; i < 11; i++)
                {
                    string strTemp = IniOperation.GetStringValue(strFile, "PixelX", "xPixel_" + string.Format("{0}", i), null);
                    m_xPixel[i] = Convert.ToDouble(strTemp);

                    strTemp = IniOperation.GetStringValue(strFile, "PixelY", "yPixel_" + string.Format("{0}", i), null);
                    m_yPixel[i] = Convert.ToDouble(strTemp);

                    strTemp = IniOperation.GetStringValue(strFile, "PosX", "xPos_" + string.Format("{0}", i), null);
                    m_xPos[i] = Convert.ToDouble(strTemp);

                    strTemp = IniOperation.GetStringValue(strFile, "PosY", "yPos_" + string.Format("{0}", i), null);
                    m_yPos[i] = Convert.ToDouble(strTemp);
                }

                string strCxy = IniOperation.GetStringValue(strFile, "RoateCenter", "x1", null);
                m_DataCenter[0] = Convert.ToDouble(strCxy);
                strCxy = IniOperation.GetStringValue(strFile, "RoateCenter", "y1", null);
                m_DataCenter[1] = Convert.ToDouble(strCxy);
                strCxy = IniOperation.GetStringValue(strFile, "RoateCenter", "x2", null);
                m_DataCenter[2] = Convert.ToDouble(strCxy);
                strCxy = IniOperation.GetStringValue(strFile, "RoateCenter", "y2", null);
                m_DataCenter[3] = Convert.ToDouble(strCxy);
                strCxy = IniOperation.GetStringValue(strFile, "RoateCenter", "angle", null);
                m_DataCenter[4] = Convert.ToDouble(strCxy);

                strCxy = IniOperation.GetStringValue(strFile, "Calib", "xCenter", null);
                m_xCenter = Convert.ToDouble(strCxy);
                strCxy = IniOperation.GetStringValue(strFile, "Calib", "yCenter", null);
                m_yCenter = Convert.ToDouble(strCxy);

                strCxy = IniOperation.GetStringValue(strFile, "Calib", "xMaxOffset", null);
                m_xMaxOffset = Convert.ToDouble(strCxy);

                strCxy = IniOperation.GetStringValue(strFile, "Calib", "yMaxOffset", null);
                m_yMaxOffset = Convert.ToDouble(strCxy);

                strCxy = IniOperation.GetStringValue(strFile, "Calib", "PixWidth", null);
                m_PixWidth = Convert.ToDouble(strCxy);

                BuildPointMap();
            }
            catch (HalconException HDevExpDefaultException1)
            {
                System.Windows.Forms.MessageBox.Show(HDevExpDefaultException1.ToString());
                return false;
            }
           return true;
        }
    }
}
