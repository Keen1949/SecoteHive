using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

#pragma warning disable 1591
namespace M2400
{
    // 轴序号
    public enum AxisNo : int
    {
        X = 0,
        Y,
        Z,
        U
    };

    // 返回的错误码
    public enum ErrorCode : int
    {
        Success = 0,
        Failed = -1,
        OpenFailed = -2,
        TimeOut = -3,
        AxisNoError = -4,
        ParameterError = -5,
        WriteFailed = -6,
        ReadFailed = -7,
        Moving = -8,
        IoCardNoError = -9
    };

    // 轴IO状态
    public enum AxisStateBit : int
    {
        SoftLimitPosBit = 0,
        SoftLimitNegBit,
        HardLimitPosBit,
        HardLimitNegBit,
        AlarmBit,
        EmergencyAlarmBit,

        ServoReadyBit = 8,
        InposSignalBit,
        EncoderZeroBit,
        OriginSignalBit,  
                           
        ServoOnBit = 16,
        AlarmClearBit
    }

    // 连续运动时，触发停止的原因
    public enum StopReason : int
    {
        Normal = 0,
        OnHardLimitPos,
        OnHardLimitNeg,
        OnAlarm,
        OnEmergencyAlarm,
        OnSoftLimitPos,
        OnSoftLimitNeg,
        OnServoOff
    };

    [Serializable]
    public class AxisConfig
    {
        public int MotorType;
        public int SpeedLevel;

        public bool OriginValidLevel;
        public bool ReadyValidLevel;
        public bool EncoderZValidLevel;

        public bool LimitPosEnable;
        public bool LimitNegEnable;
        public bool LimitPosValidLevel;
        public bool LimitNegValidLevel;

        public bool PulseOutMode;
        public bool PulseLogical;
        public bool DirLogical;
        public bool EncoderInMode;
        public int EncoderDivMode;
        public bool AlarmValidLevel;
        public bool AlarmEnable;
        public bool InposValidLevel;
        public bool InposEnable;
        public bool In0ValidLevel;

        public bool ServoOnValidLevel;
        public bool AlarmClearValidLevel;

        public bool SoftLimitPosEnable;
        public bool SoftLimitNegEnable;
        public int SoftLimitPos;
        public int SoftLimitNeg;

        public int Acceleration;
        public int Deceleration;
        public int StartSpeed;
        public int DriverSpeed;

        public int HomeMode;
        public bool HomeDir;
        public bool HomeUseEz;
        public int HomeSpeedLow;
        public int HomeSpeedHigh;
        public int HomeOffset;

        public AxisConfig()
        {
            MotorType = 0;
            SpeedLevel = 2;

            OriginValidLevel = false;
            ReadyValidLevel = false;
            EncoderZValidLevel = false;

            LimitPosEnable = true;
            LimitNegEnable = true;
            LimitPosValidLevel = false;
            LimitNegValidLevel = false;

            PulseOutMode = false;
            PulseLogical = false;
            DirLogical = false;
            EncoderInMode = false;
            EncoderDivMode = 0;
            AlarmValidLevel = false;
            AlarmEnable = true;
            InposValidLevel = false;
            InposEnable = false;
            In0ValidLevel = false;

            ServoOnValidLevel = false;
            AlarmClearValidLevel = false;

            SoftLimitPosEnable = false;
            SoftLimitNegEnable = false;
            SoftLimitPos = 50000;
            SoftLimitNeg = -50000;

            Acceleration = 20000;
            Deceleration = 20000;
            StartSpeed = 100;
            DriverSpeed = 10000;

            HomeMode = 0 ;
            HomeDir = false;
            HomeUseEz = false;
            HomeSpeedLow = 100;
            HomeSpeedHigh = 500;
            HomeOffset = 0;
        }

        public AxisConfig Copy()
        {
            return this.MemberwiseClone() as AxisConfig;
        }
    }  

    [StructLayout(LayoutKind.Sequential)]
    public struct AxisConfigBase
    {
        public int MotorType;
        public int SpeedLevel;
       
        public int OriginValidLevel;
        public int ReadyValidLevel;
        public int EncoderZValidLevel;

        public int LimitPosEnable;
        public int LimitNegEnable;
        public int LimitPosValidLevel;
        public int LimitNegValidLevel;

        public int PulseOutMode;
        public int PulseLogical;
        public int DirLogical;
        public int EncoderInMode;
        public int EncoderDivMode;
        public int AlarmValidLevel;
        public int AlarmEnable;
        public int InposValidLevel;
        public int InposEnable;

        public int ServoOnValidLevel;
        public int AlarmClearValidLevel;

        public int SoftLimitPosEnable;
        public int SoftLimitNegEnable;
        public int SoftLimitPos;
        public int SoftLimitNeg;

        public int Acceleration;
        public int Deceleration;
        public int StartSpeed;
        public int DriverSpeed;


        public int HomeMode;
        public int HomeDir;
        public int HomeUseEz;
        public int HomeSpeedLow;
        public int HomeSpeedHigh;
        public int HomeOffset;
    };
    
    [StructLayout(LayoutKind.Sequential)]
    public struct AxisSpeedRange
    {
        public int AccMin;
        public int AccMax;

        public int DecMin;
        public int DecMax;

        public int StartSpeedMin;
        public int StartSpeedMax;

        public int DriverSpeedMin;
        public int DriverSpeedMax;
    };


    public class Function
    {
        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_GetLibVer();

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_GetFirmwareVer(int CardNo, ref int FirmVersion);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_GetCardType(int CardNo, ref int CardType);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_Initial(int CardNo);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_GetIoCardId(int CardNo, int[] IoCardId);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_WriteOutput(int CardNo, int NodeId, UInt32 OutputState);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]        
        public static extern int SCT_WriteOutBit(int nCardNo, int NodeId, int nIndex, int nValue);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_ReadInput(int CardNo, int NodeId, ref UInt32 InputState);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_ReadInBit(int nCardNo, int NodeId, int nIndex, ref UInt32 InBit);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_ReadOutput(int CardNo, int NodeId, ref UInt32 OutputState);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_ReadOutBit(int nCardNo, int NodeId, int nIndex, ref UInt32 pOutBit);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_SetServoOn(int CardNo, int Axis, int Enable);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_SetAlarmClear(int CardNo, int Axis, int Enable);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_GetAxisIo(int CardNo, int Axis, ref int IoData);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_SetPulseMode(int CardNo, int Axis, int PulseMode, int PulseLogical, int DirLogical);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_SetEncoderMode(int CardNo, int Axis, int Mode, int Div);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_SetLimitMode(int CardNo, int Axis, int PosValidLevel, int NegValidLevel);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_SetSoftLimitMode(int CardNo, int Axis, int EnablePos, int PosP, int EnableNeg, int PosN);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_AlarmEnable(int CardNo, int Axis, int AlarmInEnable);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_InposEnable(int CardNo, int Axis, int InposEnable);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_SetSpeedLevel(int CardNo, int Axis, int Level);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_SetAccOffset(int CardNo, int Axis, int Offset);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_SetAccMode(int CardNo, int Mode);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_SetJcc(int CardNo, int Axis, int Jcc);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_SetAcc(int CardNo, int Axis, int Acc);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_SetDec(int CardNo, int Axis, int Dec);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_SetStartSpeed(int CardNo, int Axis, int Speed);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_SetSpeed(int CardNo, int Axis, int Speed);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_SetCommandPos(int CardNo, int Axis, int Position);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_SetRealPos(int CardNo, int Axis, int Position);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_GetTargetPos(int CardNo, int Axis, ref int Position);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_GetCommandPos(int CardNo, int Axis, ref int Position);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_GetRealPos(int CardNo, int Axis, ref int Position);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_HasError(int CardNo, int Axis, ref int SingleAxisError);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_IsMoving(int CardNo, int Axis, ref int IsMoving);    

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_IsInterpolating(int CardNo, ref int IsInpolating);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_GetStopData(int CardNo, int Axis, ref int StopData);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_DecEnable(int CardNo);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_DecDisable(int CardNo);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_DecStop(int CardNo, int nAxis);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_SuddenStop(int CardNo, int nAxis);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_JogMove(int CardNo, int Axis, int Dir);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_RelativeMove(int CardNo, int Axis, int Position);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_AbsoluteMove(int CardNo, int Axis, int Position);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_RelativeMove2D(int CardNo, int[] AxisId, int[] Distance);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_AbsoluteMove2D(int CardNo, int[] AxisId, int[] Distance);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_RelativeMove3D(int CardNo, int[] AxisId, int[] Distance);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_AbsoluteMove3D(int CardNo, int[] AxisId, int[] Distance);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_RelativeArc2D(int CardNo, int[] AxisId, int[] EndPos, int[] Center, int Dir);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_AbsoluteArc2D(int CardNo, int[] AxisId, int[] EndPos, int[] Center, int Dir);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_HomeConfig(int nCardNo, int nAxis, int nDir);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_HomeStart(int CardNo, int nAxis);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_HomeStop(int CardNo, int nAxis);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_GetHomeState(int CardNo, int Axis, ref int Running, ref int HomeState);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_WriteConfig(int CardNo, int Axis, ref AxisConfigBase Cfg);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_ReadConfig(int CardNo, int Axis, ref AxisConfigBase Cfg);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_GetSpeedRange(int CardNo, int Axis, ref AxisSpeedRange SpeedRange);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_LoadConfigFile(string Path);

        [DllImport("M2400.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SCT_SaveConfigFile(string Path);
    }
}
