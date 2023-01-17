
/**********************************************************************************************
1、PLC通信程序介绍
	PLC通信程序用于Autoframe软件对PLC元件的读写，通过串口或者网口通信。
	Plc.cs文件主要定义了 PlcDevice 类，PlcDevice类是所有Plc设备的基类，定义了PLC设备的公用接口。
	Plc设备类的名称格式为“Plc_*”,比如汇川Plc对应的类为“Plc_Inovance”，基恩士的Plc对应为“Plc_Keyence”。
	Plc_Inovance类继承PlcDevice 类，实现了对汇川Plc的读写。
	Plc_Keyence类继承PlcDevice 类，实现了对基恩士Plc的读写。

    目前没有GUI可修改参数，通过修改SystemCfg.xml来配置， 需要根据实际的PLC配置来进行元件数量的配置
      <Plc>
     <汇川PLC 序号="1" 名称="Inovance" 通讯方式="串口" 索引号="1" 站号="5">
		<AddrMap>
			<AddrMap 元件="X" 起始地址="0" 元件数量="32" />
			<AddrMap 元件="Y" 起始地址="0" 元件数量="32" />
			<AddrMap 元件="M" 起始地址="0" 元件数量="1024" />
			<AddrMap 元件="D" 起始地址="0" 元件数量="512" />
		</AddrMap>
    </汇川PLC>
    <三菱PLC 序号="1" 名称="Mitsubishi" 通讯方式="网口" 索引号="5" 站号="2">
		<AddrMap>
			<AddrMap 元件="X" 起始地址="0" 元件数量="32" />
			<AddrMap 元件="Y" 起始地址="0" 元件数量="32" />
			<AddrMap 元件="M" 起始地址="0" 元件数量="1024" />
			<AddrMap 元件="D" 起始地址="0" 元件数量="512" />
		</AddrMap>
    </三菱PLC>
  </Plc> 


2、通过PlcDevice类直接读写Plc元件。读写操作都是同步执行，连续读写多个元件时，可能导致程序的长时间阻塞。
	下面以对“D”寄存器的读写为例。
	
	// 获取系统的首个PLC设备
	PlcDevice plcDev = PlcMgr.GetInstance().Getplc(0);
	
	// 准备向“D”寄存器写入的数值，写入10个字，数值为0~10 
	UInt16[] regVals = new UInt16[10];
	for (int i = 0; i < regVals.Length; i++)
		regVals[i] = (UInt16)(i);

	// 向“D”寄存器写入数据
	plcDev.WriteMultiWord(SoftElement.D, 0, regVals);
	
	// 从“D”寄存器读取数据
	plcDev.ReadMultiWord(SoftElement.D, 0, regVals);
	
	PlcDevice类提供了多个读写函数，详细的函数接口说明请看 PlcDevice类的定义。

	
3、通过PlcMonitor类，在后台读取PLC的元件。通信程序在后台执行，将读取到的PLC元件值保存在数组中，
    获取元件值时直接从数组中获取，不会阻塞程序的运行。

    在监控过程中，如果要读写指定PLC地址，可以按第二点的方式操作，函数内部有多线程互斥，不会冲突。
	
    PLC后台读取的步骤如下：
	1)添加需要读取的元件；
	2)启动后台线程，读取元件值；
	3)获取元件值。
	
	// 监视Plc0的D1寄存器
	PlcMgr.GetInstance().Monitor.Add(0, SoftElement.D, 1);

	// 监视Plc0的D5~D14寄存器
	PlcMgr.GetInstance().Monitor.Add(0, SoftElement.D, 5, 10);
	
	// 启动后台线程
	PlcMgr.GetInstance().StartMonitor();
	
	// 获取Plc0的D5~D14寄存器值
	UInt16[] regVals = PlcMgr.GetInstance().Monitor.ReadWords(0, SoftElement.D, 5, 10);
	
	在启动后台线程以后，获取的PLC元件值才是有效的。
	启动后台线程的函数 “PlcMgr.GetInstance().StartMonitor()” 一般在AutoTool.cs文件的 InitSystem()函数内执行。

**********************************************************************************************/
