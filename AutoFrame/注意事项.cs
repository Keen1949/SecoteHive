﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFrame
{
    //塔灯使用
    // 红灯：安全门回路断开，工作人员已在自动模式下打开门或按下紧急停止按钮。 “危险状态”
    //黄灯恒定：机器不在自动模式状态，例如设备启动，自检。
    //黄灯闪烁：自动供料系统中的设备在等待物料的状态
    //绿灯恒定：机器/设备处在自动模式，且停留在初始位置，没有执行自动过程。
    //绿灯闪烁：机器处于“自动模式”处理状态;正在执行自动过程

    //程序基本要求
    //紧急停止功能：切断执行部件的控制电源，气缸必须保持在当时的位置，所有输入状态和指示灯等正常显示。
    //当解除紧急停止状态时，必须复位设备才可以重新开始下一个自动循环，同样适用于断电重启的情况。
    //机器/设备上必须设置有“复位”按钮，通过它，设备可以恢复到定义好的原点或初始位置。当设备复位完成后需要有明显的信号通知操作人员。
    //在传感器信号缺失的情况下，相应的“超时”消息必须显示对应的缺失的传感器信号；同样适用于自动循环中的超时情况



    class 程序一致性要求
    {
        //安全控制
        //涉及人身或机器安全的外部元件均以常闭点的形式接入系统（包括设备自身状态等）；
        //通常使用的安全产品包括：紧急按钮、安全光栅、区域传感器、安全门、拉绳开关、限位开关、压力开关以及外部智能设备的异常状态开关等；
        //一般情况下，当涉及安全的产品被触发时系统即刻停止，程序内部对应位状态被保持，此时即使安全元件的物理触发状态被解除，
        //内部对应位状态不会自动复位，需要压按【复位】按钮后才解除内部的状态锁定，在此情况下系统才能进入新的自动工作循环；
        //系统自动运行时应加入所有的内部安全状态连锁条件，为了方便调试和设备维护手动运行一般仅受紧急停止等一些严格条件约束
        //（因为它是在相对明确的条件下的单一动作）。原点复位可能涉及到一系列的连锁动作，人的预知性不明确，所以它的执行条件应
        //比手动时要严格，具体则要根据实际情况来区别对待；

        //手动操作
        //在电气程序里通常都包括手动控制程序，它主要是为了方便设备调试和维护；
        //手动的操作可能是外部物理上的按钮或选择开关等，也可能是HMI或其他一些通讯设备直接映射写入的内部寄存器；
        //手动程序主要是针对外部动作执行元件（如电磁阀、继电器等）的操作，它的回路中包括：手动模式，执行条件，
        //位置连锁，动作互锁，触发信号以及最终的中间输出位寄存器（为了程序的可读性和修改方便，忌直接将手动和自动
        //执行条件以复杂的方式进行串并联后直接去驱动输出位）；
        //手动动作应注意实际生产现场的位置连锁条件（如干涉等）和涉及电气安全的互锁

        // 原点复位
        //原点复位的功能是将系统回归到初始状态，它既包括外部执行机构的物理位置状态，也包括内部的保持状态等；
        //原点复位应注意外部执行机构的位置连锁以及动作次序；
        //原点复位应考虑到人身和设备的安全条件；
        //原点复位应在系统停止的状态下进行；

        //启动信息
        //因为设备自动启动时有很多的状态和位置作为约束条件（设备安全、物理位置、工件检测等），
        //为了操作者或维护者快速识别并快速排除“故障”，需要在程序中实现这一模块功能并在外部
        //实时显示出来（在硬件条件满足时应尽可能将所有“不能启动”原因一次性全部显示出来，在硬件条件
        //限制时则将重要的原因全部显示或按重要等级依次显示）；

        //自动循环
        //在自动运行过程中，循环任务结束时系统从自动循环中正常结束退出，除指示灯外的所有外部执行输出位关闭，内部过程状态位复位；
        //系统自动运行状态位ON时，在一个扫描周期内将数据寄存器中保存的上次的结果数据清除，准备下个自动循环任务的数据记录；
        //废品的丢弃确认：在测试工作站中当不合格品被检测到时，控制系统要求作业者必须有往指定位置丢弃废品的动作才能开始下个自动工作循环；
        //前置动作的确认：前置动作通常指在进行自动工作循环前必须的辅助动作（比如：取料、涂油等）。
        //控制系统要求在这些动作被执行后方可进行后续工作循环（注意状态保持位的复位方法）；
        //重复过程的确认：重复过程是指对同一工件的反复测试动作。控制系统要求系统避免进行同一工件的多次反复测试，
        //当然在必要时，作业者必须从测试位置取下工件重新放置后以“新”工件的方式进行测试；
        //顺序动作的表达：在组装工作站中，工艺上往往要求部件有严格的装配顺序。系统需要明确提示作业者当前的作业内容以及装配的过程信息等；
        //自动控制程序中使用的内部寄存器位的使用应尽可能连续、规则和容易识别；

        //数据处理
        //大多数控制系统除传统的逻辑关系处理外还需要进行数据的输入、输出和过程运算等，它往往是产品判定的直接依据；
        //我们需要对内部数据存储器的物理构成、数据结构以及存储方式有清晰的认识，对参与运算的数据类型以及预期结果的判断，以便使用合理的数据类型和分配合理的数据空间；
        //准确理解内部“位”“整型”、“浮点数”的基本概念和能表达的数据范围、使用方法及存储表达方式；
        //使用产品计数功能时，要注意数据越界等异常的处理，及数值清零调整等的权限设置
        //对于关键的信息数据应采用文件记录的方式进行存储便于后续跟踪，界面采用表格显示便于查看。

        //过程判定
        //通常指在对产品的性能测试时根据事先确定的工艺参数设定来对产品的优劣判断，内部的实际
        //操作上既有“位”判断（位置、物检等）也有“字”判断（位移、拉力、扭矩、电压、电流等）；
        //通常情况下指定的功能测试都是不可修复性错误（不可逆转的本质错误），因此当测试过程发生异常时即可
        //判定产品的合格与否，此时控制系统会立即停止（有时是做物理上的回位动作后停止），异常的内部状态保
        //持并实时显示，后续测试过程将不再进行；
        //测试的最终结果信息除以明确的方式提示作业者外，有时会辅以声音报警，作业者处理好既定动作后按【复位】按钮以解除警示信息；
        //测试的最终结果包括OK/NG的结果和过程信息，也包括“字”结果数据，我们需要把它以直接的方式通知作业者

        //通讯处理
        //用于处理接收外部串口或以太网设备的数据或向外部串口设备发送数据以及对这些数据的处理；
        //通讯异常的处理过程（提示、报警等），数据重发等等；
        //通讯参数的设置，比如IP地址，RS232端口、波特率等等必须明确标明


        //输入输出

        //输入方面：对反应位置的输入做到位保持延时，对物检稳定或动作确认的输入做延时等；
        //输出方面：将设备的IO输出集中在一个连续的程序段内，输出通常包括手动条件下的输出、原点复位情况下的输出、自动模式下的输出等，将这些输出并联后驱动外部输出寄存器；
        //除非特殊情况需要，禁止使用SET和RESET指令操作位寄存器或物理输出点，应使用自保持方式。


        //异常处理
        //设备异常大多指：安全设施的异常，动作位置的异常，物料检测的异常，测试结果的异常等
        //安全异常报警，<不能启动>报警，动作超时报警，测试异常报警，循环结束报警（提示）等；
        //报警以指示灯和蜂鸣器的方式，不同触发条件下的报警以不同的声音间隔和总时间加以区别

    }
}
