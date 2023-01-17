/********************************************************************
	created:	2013/11/12
	filename: 	MOTION_8254
	file ext:	h
	author:		zjh
	purpose:	8254�˶����ƿ��ķ�װ��
*********************************************************************/

using System;
using System.Diagnostics;
using System.Threading;
using CommonTool;
using Adlink;


namespace MotionIO
{
     public class Motion_8254 : Motion
    {
        //************************************
        // Method:    Motion_8254 ���캯��
        // FullName:  Motion_8254::Motion_8254
        // Access:    public 
        // Returns:   
        // Parameter: void
        //************************************
       public Motion_8254(int nCardNo, int nMinAxisNo, int nMaxAxisNo):base(nCardNo, nMinAxisNo, nMaxAxisNo)
        {
            
        }


        public override string GetCardName()
        {
            return "8254";
        }
        //************************************
        // Method:    init �Ῠ��ʼ��
        // FullName:  Motion_8254::init
        // Access:    virtual public 
        // Returns:   bool
        //************************************
        public override bool Init()
        {
            //TRACE("init card\r\n");
            int boardID_InBits = 0;
            int mode = (Int32)APS_Define.INIT_AUTO_CARD_ID | (Int32)APS_Define.INIT_PARAM_LOAD_FLASH;

            int ret = APS168.APS_initial(ref boardID_InBits, mode);
            if ((Int32)APS_Define.ERR_NoError == ret)
            {
                ret = APS168.APS_load_param_from_file("param.xml");
                if ((Int32)APS_Define.ERR_NoError != ret)
                {
                    m_bEnable = false;
                    WarningMgr.GetInstance().Warnning("�˶����ƿ�8254��ȡ�����ļ�ʧ��");

                    return false;
                }
                return true;
            }
            else
            {
                m_bEnable = false;
                WarningMgr.GetInstance().Warnning("�˶����ƿ�8254��ʼ��ʧ��! ������ = %d", ret);
                return false;
            }
        }

        //************************************
        // Method:    Close �ر��Ῠ
        // FullName:  Motion_8254::Close
        // Access:    virtual public 
        // Returns:   bool
        //************************************
        public override bool Close()
        {
            int ret = APS168.APS_close();
            if ((Int32)APS_Define.ERR_NoError == ret)
            {
                return true;
            }
            else
            {
                WarningMgr.GetInstance().Error("8254 Card Close Error!", 0);
                return false;
            }
        }

        //************************************
        // Method:    ServoOn ����ʹ��
        // FullName:  Motion_8254::ServoOn
        // Access:    virtual public 
        // Returns:   bool
        // Parameter: int nAxisNo ���
        //************************************
        public override bool ServoOn(int nAxisNo)
        {
            int ret = APS168.APS_set_servo_on(nAxisNo, 1);
            if ((Int32)APS_Define.ERR_NoError == ret)
            {
                return true;
            }
            else
            {
                WarningMgr.GetInstance().Error("8254 Card Aixs %d servo on Error!", nAxisNo);
                return false;
            }
        }

        //************************************
        // Method:    ServoOff �Ͽ�ʹ��
        // FullName:  Motion_8254::ServoOff
        // Access:    virtual public 
        // Returns:   bool
        // Parameter: int nAxisNo ���
        //************************************
        public override bool ServoOff(int nAxisNo)
        {
            int ret = APS168.APS_set_servo_on(nAxisNo, 0);
            if ((Int32)APS_Define.ERR_NoError == ret)
            {
                return true;
            }
            else
            {
                WarningMgr.GetInstance().Error("8254 Card Axis %d servo off Error!", nAxisNo);
                return false;
            }
        }

        //************************************
        // Method:    Home ��ԭ��
        // FullName:  Motion_8254::Home
        // Access:    virtual public 
        // Returns:   bool
        // Parameter: int nAxisNo ���
        // Parameter: int nDir ��ԭ�㷽ʽ
        //************************************
        public override bool Home(int nAxisNo, int nDir)
        {
            APS168.APS_set_axis_param(nAxisNo, (Int32)APS_Define.PRA_HOME_DIR, nDir);
            Thread.Sleep(50);
            int ret = APS168.APS_home_move(nAxisNo);
            if ((Int32)APS_Define.ERR_NoError == ret)
            {
                return true;
            }
            else
            {
                WarningMgr.GetInstance().Error("8254 Axis %d Home Error, code is %d", nAxisNo, ret);
                return false;
            }
        }

        //************************************
        // Method:    AbsMove �Ծ���λ���ƶ�
        // FullName:  Motion_8254::AbsMove
        // Access:    virtual public 
        // Returns:   bool
        // Parameter: int nAxisNo ���
        // Parameter: long nPos λ��
        // Parameter: int nSpeed �ٶ�
        //************************************
        public override bool AbsMove(int nAxisNo, int nPos, int nSpeed)
        {
            int ret = APS168.APS_absolute_move(nAxisNo, nPos, nSpeed);
            if ((Int32)APS_Define.ERR_NoError == ret)
            {
                return true;
            }
            else
            {
                WarningMgr.GetInstance().Error("8254 Axis %d  abs move Error, code is %d", nAxisNo, ret);
                return false;
            }
        }

        //************************************
        // Method:    RelativeMove ���λ���ƶ�
        // FullName:  Motion_8254::RelativeMove
        // Access:    virtual public 
        // Returns:   bool
        // Parameter: int nAxisNo ���
        // Parameter: long nPos λ��
        // Parameter: int nSpeed �ٶ�
        //************************************
        public override bool RelativeMove(int nAxisNo, int nPos, int nSpeed)
        {
            int ret = APS168.APS_relative_move(nAxisNo, nPos, nSpeed);
            if ((Int32)APS_Define.ERR_NoError == ret)
            {
                return true;
            }
            else
            {
                WarningMgr.GetInstance().Error("8254 Axis %d  relative move Error, code is %d", nAxisNo, ret);
                return false;
            }
        }

        public override bool JogMove(int axis, int nDirection, int bStrat, int nSpeed)
        {
            APS168.APS_set_axis_param(axis, (Int32)APS_Define.PRA_JG_MODE, 0);
            APS168.APS_set_axis_param(axis, (Int32)APS_Define.PRA_JG_DIR, nDirection);
            APS168.APS_set_axis_param_f(axis, (Int32)APS_Define.PRA_JG_VM, nSpeed);
            int ret = APS168.APS_jog_start(axis, bStrat);
            if ((Int32)APS_Define.ERR_NoError == ret)
            {
                return true;
            }
            else
            {
                WarningMgr.GetInstance().Error("8254 Axis %d  relative move Error, code is %d", axis, ret);
                return false;
            }
        }
        //************************************
        // Method:    StopAxis ������ֹͣ
        // FullName:  Motion_8254::StopAxis
        // Access:    virtual public 
        // Returns:   bool
        // Parameter: int nAxisNo ���
        //************************************
        public override bool StopAxis(int nAxisNo)
        {
            int ret = APS168.APS_stop_move(nAxisNo);
            if ((Int32)APS_Define.ERR_NoError != ret)
            {
                WarningMgr.GetInstance().Error("8254 Card normal stop axis %d Error!", nAxisNo);
                return false;
            }
            else
                return true;
        }

        //************************************
        // Method:    StopEmg ��ͣ
        // FullName:  Motion_8254::StopEmg
        // Access:    virtual public 
        // Returns:   bool
        // Parameter: int nAxisNo ���
        //************************************
        public override bool StopEmg(int nAxisNo)
        {
            int Result = APS168.APS_emg_stop(nAxisNo);
            if ((Int32)APS_Define.ERR_NoError != Result)
            {
                WarningMgr.GetInstance().Error("8254 Card Emergency stop axis %d Error!", nAxisNo);
                return false;
            }
            else
                return true;
        }

        //************************************
        // Method:    GetMotionState ��ȡ�Ῠ�˶�״̬
        // FullName:  Motion_8254::GetMotionState
        // Access:    virtual public 
        // Returns:   long
        // Parameter: int nAxisNo ���
        //************************************
        public override long GetMotionState(int nAxisNo)
        {
            return APS168.APS_motion_status(nAxisNo);
        }

        //************************************
        // Method:    GetMotionIoState ��ȡ�Ῠ�˶�IO�ź�
        // FullName:  Motion_8254::GetMotionIoState
        // Access:    virtual public 
        // Returns:   long
        // Parameter: int nAxisNo ���
        //************************************
        public override long GetMotionIoState(int nAxisNo)
        {
            return APS168.APS_motion_io_status(nAxisNo);
        }

        //************************************
        // Method:    GetAixsPos ��ȡ��ĵ�ǰλ��
        // FullName:  Motion_8254::GetAixsPos
        // Access:    virtual public 
        // Returns:   long
        // Parameter: int nAxisNo ���
        //************************************
        public override long GetAixsPos(int nAxisNo)
        {

            int nAxisPos = 0;
            APS168.APS_get_position(nAxisNo, ref nAxisPos);
            return nAxisPos;
        }

        //************************************
        // Method:    IsAxisNormalStop ���Ƿ�����ֹͣ
        // FullName:  Motion_8254::IsAxisNormalStop
        // Access:    virtual public 
        // Returns:   int
        // Parameter: int nAxisNo ���
        //************************************
        public override int IsAxisNormalStop(int nAxisNo)
        {
            int nRet = APS168.APS_motion_status(nAxisNo);
            if (((nRet & (Int32)(APS_Define.MTS_NSTP_V)) & 1) == 1)
            {
                if (((nRet & (Int32)(APS_Define.MTS_ASTP_V)) & 1) == 1)
                {
                    int stop_code = 0;
                    APS168.APS_get_stop_code(nAxisNo, ref stop_code);
                    if (1 == stop_code)  //��ͣ
                    {
                        Debug.WriteLine("Axis %d have Emg single \r\n", nAxisNo);

                        return stop_code;
                    }
                    else if (2 == stop_code)  //����
                    {
                        Debug.WriteLine("Axis %d have Alm single \r\n", nAxisNo);
                        return stop_code;
                    }
                    else if (3 == stop_code)  //δservo on
                    {
                        Debug.WriteLine("Axis %d have servo single \r\n", nAxisNo);
                        return stop_code;
                    }
                    else if (4 == stop_code) //����λ   
                    {
                        Debug.WriteLine("Axis %d have PEL single \r\n", nAxisNo);
                        return stop_code;
                    }
                    else if (5 == stop_code) //����λ
                    {
                        Debug.WriteLine("Axis %d have MEL single \r\n", nAxisNo);
                        return stop_code;
                    }
                }
                return 0;
            }
            else
                return -1;
        }

        //************************************
        // Method:    SetPosZero λ������
        // FullName:  Motion_8254::SetPosZero
        // Access:    virtual public 
        // Returns:   bool
        // Parameter: int nAxisNo ���
        //************************************
        public override bool SetPosZero(int nAxisNo)
        {
            return APS168.APS_set_position(nAxisNo, 0) == (Int32)APS_Define.ERR_NoError;
        }

        //************************************
        // Method:    �ٶ�ģʽ��ת��
        // FullName:  Motion_8254::VelocityMove
        // Access:    virtual public 
        // Returns:   bool
        // Parameter: int nAxisNo
        // Parameter: int nSpeed
        //************************************
        public override bool VelocityMove(int nAxisNo, int nSpeed)
        {
            return APS168.APS_velocity_move(nAxisNo, nSpeed) == (Int32)APS_Define.ERR_NoError;
        }


        public override bool IsHomeNormalStop(int nAxisNo)
        {

            return false;
        }

    }

}
