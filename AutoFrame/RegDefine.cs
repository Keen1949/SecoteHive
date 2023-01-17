using System;
namespace AutoFrame
{
    /// <summary>
    /// 
    /// </summary>
    public enum SysBitReg
    {
        xxx, //xxx位寄存器，可用汉字描述
        转盘站初始化完成,
        上料站开始工作,
        上料站工作完成,
        穴位一涂锡站开始工作,
        穴位一涂锡站工作完成,
        穴位一内锡焊开始工作,
        穴位一内锡焊工作完成,
        穴位一外锡焊开始工作,
        穴位一外锡焊工作完成,
        穴位二涂锡站开始工作,
        穴位二涂锡站工作完成,
        穴位二内锡焊开始工作,
        穴位二内锡焊工作完成,
        穴位二外锡焊开始工作,
        穴位二外锡焊工作完成,
        下料站开始工作,
        下料站工作完成,

        bit_Hive连接失败,
        bit_PLC蜂鸣响,

    }

    /// <summary>
    /// 系统整型寄存器索引枚举声明
    /// </summary>
    public enum SysIntReg
    {
        Int_Process_Step,
    };

    /// <summary>
    /// 浮点型整型寄存器索引枚举声明
    /// </summary>
    public enum SysDoubleReg
    {
    };

    /// <summary>
    /// 系统字符串寄存器索引枚举声明
    /// </summary>
    public enum SysStrReg
    {
    };

}