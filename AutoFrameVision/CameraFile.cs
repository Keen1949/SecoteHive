using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
namespace AutoFrameVision
{
    /// <summary>
    /// 相机采集类,由路径生成的文件采集相机
    /// </summary>
    public class CameraFile : CameraBase
    {
        /// <summary>
        /// 图像文件名称表
        /// </summary>
        HTuple m_ImageFiles = new HTuple();
        /// <summary>
        /// 当前的图像文件索引
        /// </summary>
        int m_nIndex = 0;
        /// <summary>
        /// 是否已经打开过相机
        /// </summary>
        bool m_bOpen = false;
        /// <summary>
        /// 以目录路径来构造本相机采集类
        /// </summary>
        /// <param name="strName"></param>
        public CameraFile(string strName):base(strName)
        {
            //  Init();
        }

        /// <summary>
        /// 打开相机
        /// </summary>
        /// <returns></returns>
        public override bool Open()
        {
            try
            {
                m_nIndex = 0;
                m_bOpen = true;
                HOperatorSet.ListFiles(Name, (new HTuple("files")).TupleConcat("recursive").TupleConcat("max_files 1000"), out m_ImageFiles);
                HOperatorSet.TupleRegexpSelect(m_ImageFiles, (new HTuple("\\.(tif|tiff|gif|bmp|jpg|jpeg|jp2|png|pcx)$")).TupleConcat(
                   "ignore_case"), out m_ImageFiles);
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                return false;
            }
            return m_ImageFiles.TupleLength() > 0;
        }

        /// <summary>
        /// 判断相机是否已经打开
        /// </summary>
        /// <returns></returns>
        public override bool isOpen()
        {
            return m_bOpen ;
        }
        /// <summary>
        /// 关闭相机,同时释放相机缓存图像
        /// </summary>
        /// <returns></returns>
        public override bool Close()
        {
            DeInit();
              m_bOpen = false;
            m_nIndex = 0;
            return true;
        }

        /// <summary>
        /// 同步采集一张图像
        /// </summary>
        /// <returns>0:采集失败 1:采集成功 -1:采集成功,但路径已经全部循环一遍</returns>
        public override int Snap()
        {
            if (!m_bOpen)
            {
                Open();
            }

            if (m_ImageFiles.TupleLength() > 0)
            {
                if (m_nIndex < m_ImageFiles.TupleLength())
                {
                    try
                    {
                        m_nCurrentIndex++;
                        m_image[m_nCurrentIndex % m_nBufferCount].Dispose();
                        HOperatorSet.ReadImage(out m_image[m_nCurrentIndex % m_nBufferCount], m_ImageFiles.TupleSelect(m_nIndex++));
                       
                    }
                    catch(Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.ToString());
                        return 0;
                    }
                }
                if(m_nIndex >= m_ImageFiles.TupleLength())
                {
                    m_nIndex = 0;
                    return -1;   //一次循环完成,需要停止grab
                }
                else
                {
                    return 1;
                }
            }
            return -1;
        }
        /// <summary>
        /// 异步采集一张图像
        /// </summary>
        /// <returns></returns>
        public override int Grab()
        {
            return Snap();
            //return false;
        }

        /// <summary>
        /// 停止异步采集
        /// </summary>
        /// <returns></returns>
        public override bool StopGrab()
        {            
            return true;
        }

        /// <summary>
        /// 不需要实现此功能
        /// </summary>
        public override void SetGrabParam(string strParam ,int nValue)
        {

        }
    }
}
