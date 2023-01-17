using System;
using System.Text;
using System.Runtime.InteropServices;
#pragma warning disable 1591

namespace PCI_DMC_ERR
{
    public class CPCI_DMC_ERR
    {
        //General
        public const short ERR_SameCardNumber = 901;		//�d������(�Ҧ��b�d�d������ۦP)
        public const short ERR_CardType = 902;	//���b�d���䴩��API
        public const short ERR_CantFindDLL = 903;	//��API�P�������DLL�s������(�S��DLL�άODLL�禡�w���S�����禡)
        public const short ERR_SerchErrorCode = 904;	//��API�����~�ݨϥ�_DMC_01_get_cardtype_errorcode�Ө��o
        public const short ERR_NoCardFound = 905;	//�䤣�즹�b�d
        public const short ERR_DLLinUse = 910;	//�X�֪�DLL���Q�ϥΤ�
        public const short ERR_CantFindAB01 = 911;		//�䤣��A01DLL
        public const short ERR_CantFindF01 = 912;	//�䤣��F01DLL
        public const short ERR_CantFindDMCDLL = 913;	//���A01�BF01DLL

        //PCI_DMC_A01 / PCI_DMC_B01  
        public const short ERR_NoError = 0;	//�S���D
        public const short ERR_FeedRate_Entry_Dec = 1;	//FeedRate overwrite �i�J��t�q,�����O���A�ܤƳt��,���U�ӫ��O�N�|�s���t���ܤ�
        public const short ERR_CardNoError = 3;	//�d�����~�нT�{�d���W��DIP Switch�վ㪺���X
        public const short ERR_bootmodeErr = 5;	//�L�k�Ұ�DSP�Ʀ�{��
        public const short ERR_downloadcode = 6;//DSP Program Memory R/W Error
        public const short ERR_downloadinit = 7;//DSP Data Memory R/W Error
        public const short ERR_PCI_boot_first = 8;	//�ϥΦ�API�ݥ��Ұ�DSP�Ʀ�{��  I16 PASCAL _DMC_01_pci_initial(U16 CardNo)
        public const short ERR_FeedRate_updata = 9;	//FeedRate overwrite ���ȬۦP 
        public const short ERR_DSP_inside_calcu = 10;//DSP memory float error
        public const short ERR_AxisNoError = 11;	//�b�ƥعL�j
        public const short ERR_IPO_First = 12;	//�ݭn��IPO�Ҧ�
        public const short ERR_Target_reach = 13;//Mode 1 �B���,��Target��F
        public const short ERR_Servo_on_first = 14;	//�ݭn��servo on 
        public const short ERR_MPG_Mode = 15;//�b����Ҧ��U�L�k�M����m
        public const short ERR_PDO_TG = 16;	//�ϥ�PDO�Ҧ��U���O���Ҳ�,�L�k�^�Ǧ���
        public const short ERR_ConfigFileOpenError = 17;	//�إ�Debug Information�ɮ׿��~
        public const short ERR_Ctrl_value = 18;	//�ϥα�����O���~
        public const short ERR_Security_Fifo = 19;	//�ϥ�Security Fpga write Error
        public const short ERR_Security_Fifo_busy = 20;	//�ϥ�Security Fpga busy
        public const short ERR_SpeedLimitError = 21;	//�]�w�t�פj��̤j�t��
        public const short ERR_Security_Page = 22;	//�ϥ�Security page �ݭn�p��16
        public const short ERR_Slave_Security_op = 23;	//�ϥ�Security slave_operate ���O�L��
        public const short ERR_channel_no = 24;	//channel no ���~
        public const short ERR_start_ring_first = 25;//�ϥΦ�API�ݥ��Ұ�DMCNet  I16 PASCAL _DMC_01_start_ring(U16 CardNo, U8 RingNo)
        public const short ERR_NodeIDError = 26;	//�L��NodeID
        public const short ERR_MailBoxErr = 27;	//���O�L�k�U�F, DSP Busy
        public const short ERR_SdoData = 28;	//SDO Data�e�X���L�^��,SDO Data Send Request ,But Without ACK
        public const short ERR_IOCTL = 29;	//�@�~�t�εL�k�B�z��IRP
        public const short ERR_SdoSvonFirst = 30;//�ϥ�SDO�ާ@�b����,�ݥ�Servo On
        public const short ERR_SlotIDError = 31;	//GA�L��Slot���X
        public const short ERR_PDO_First = 32;	//�ϥ�PDO���O�ݥ��N�b�নPDO�Ҧ�
        public const short ERR_Protocal_build = 33;	//Protocal,�q��|���إ�
        public const short ERR_Maching_TimeOut = 34;	//�Ҳհt��time Out
        public const short ERR_Maching_NG = 35;	//�Ҳհt��NG
        public const short ERR_Group_Num = 36;	//�]�w�s�ճ̤j�Ȭ�6��
        public const short ERR_Master_Alarm = 37;//�G�ٵo��(�q�T���},Driver Alm)
        public const short ERR_Alarm_reset = 38;
        public const short ERR_Master_Security_Wr = 40;	//�ϥ�Security Master Write���O�L��
        public const short ERR_Master_Security_Rd = 41;	//�ϥ�Security Master Read���O�L��
        public const short ERR_Master_Security_Pw = 42;	//�ݭn����J���T��password
        public const short ERR_NonSupport_CardVer = 50;	//�ϥΥD���d���������~,�гsô�N�z��,�ʶR���T�D���d
        public const short ERR_Compare_Source = 51;	//Ver Type : B Compare Source ��ܿ��~
        public const short ERR_Compare_Direction = 52;	//Compare����V���~  dir�ݬ�1��0 1:ccw,0:cw
        public const short ERR_GetDLLPath = 60;
        public const short ERR_GetDLLVersion = 61;
        public const short ERR_GA_Port = 62;
        public const short ERR_04PISTOP_Timeout = 70;	//04PI Stop Fifo time out 
        public const short ERR_ServoSTOP_Timeout = 71;	//Servo Stop Fifo time out
        public const short ERR_04PISTOP_status = 72;	//04PI Stop MC_done not to Error
        public const short ERR_04PIHoming_err = 73;	//04PI Home status error
        public const short ERR_04PISdo_trans = 74;	//04PI sdo send but get data error
        public const short ERR_QEP_INDEX = 75;		// QEP
        public const short ERR_GPIO_OUTPUT_SIZE = 76;	// GPIO
        public const short ERR_IPCMP_INDEX = 77;		// IPCMP
        public const short ERR_IPCMP_MPC_DATA_TYPE = 78;     	// It will never happened (internal use)

        public const short ERR_TPCMP_FIFO_INDEX = 79;	// TPCMP
        public const short ERR_TPCMP_QEP_SOURCE = 80;
        public const short ERR_TPCMP_TABLE_NO_DATA = 81;    	// Input table empty
        public const short ERR_TPCMP_TABLE_OUT_OF_RANGE = 82;		// Input table size is large than TPCMP max. size
        public const short ERR_TPCMP_TABLE_OVERFLOW = 83;    	// Input table size is large than TPCMP FIFO remainder size

        public const short ERR_EXTIO_RANGE = 84;		// EXT. IO
        public const short ERR_EXTIO_LATCH_TYPE = 85;  	// It will never happened (internal use)

        public const short ERR_LATCH_FIFO_EMPTY = 86;		// Latch FIFO
        public const short ERR_LATCH_FIFO_INVALID_DATA = 87;
        public const short ERR_RangeError = 112;	//�]�w�b�����X���~  ���Υ[,�]���b���P�O�[�bNodeID
        public const short ERR_MotionBusy = 114;	//Motion ���O���|
        public const short ERR_SpeedError = 116;	//�̤j�t�׳]�m��0 
        public const short ERR_AccTimeError = 117;	//�[��t�ɤj��1000��
        public const short ERR_PitchZero = 124;	//Helix pitch�ѼƵ���0,�L�k�B��
        public const short ERR_BufferFull = 127;	//�B�ʫ��OBuffer�v��
        public const short ERR_PathError = 128;	//�B�ʫ��O���~
        public const short ERR_NoSupportMode = 130;	//���䴩�t���ܤ�
        public const short ERR_FeedHold_support = 132;	//Feedhold Stop �Ұ�,�L�k�����s���O 
        public const short ERR_SDStop_On = 133;	//�����t���O, �L�k�����s���O
        public const short ERR_VelChange_supper = 134;	//�Ҧ�1.Feedhold,2.�P�ʫ��O3.��t���O,�L�k����t���ܤƥ\��
        public const short ERR_Command_set = 135;	//�L�k�s�����FeedHold���\��
        public const short ERR_sdo_message_choke = 136;	//Sdo���O�^�Ǧ��~,���ˬd�����u���u�O�_OK
        public const short ERR_VelChange_buff_feedhold = 137;	//Feed Hold  �\�ॲ�����P�� ,�L�k�t���ܤ�
        public const short ERR_VelChange_sync_move = 138;	//�ثe�b�d���b���ݦP�ʫ��O,�L�k�t���ܤ�
        public const short ERR_VelChange_SD_On = 139;	//�ثe�b�d���b�����t���O,�L�k�t���ܤ�
        public const short ERR_P_Change_Mode = 140;	//��b�I���I�Ҧ� �[�t�q,�t�׵���0,�D��b�I���I�Ҧ�
        public const short ERR_BufferLength = 141;	//��Ҧ��b _Path_p_change,_Path_velocity_change_onfly, _Path_Start_Move_2seg�� Buffer Length �ݭn��0
        public const short ERR_2segMove_Dist = 142;	//�Z���ݭn�P�V
        public const short ERR_CenterMatch = 143;	// �g�L�ϦV�B���߭n�@�P
        public const short ERR_EndMatch = 144;	//�g�L�ϦV�B���߭n�@�P
        public const short ERR_AngleCalcu = 145;	//�g�L�p�⨤�׿��~
        public const short ERR_RedCalcu = 146;	//�g�L�b�|���~
        public const short ERR_GearSetting = 147;	//Gear�����l�Τ�����0
        public const short ERR_CamTable = 148;	// Table Setting First Arrary Point Error�]�wtable�����t��table[-1]�S���o�س]�w
        public const short ERR_AxesNum = 149;	// �h�b�]�w�ȥ����n��b�H�W
        public const short ERR_SpiralPos = 150;	// �̲צ�m�|��F���۶��
        public const short ERR_SpeedMode_Slave = 151;	// �b�t�׳s��ɨϥΪ�Slave�b,�L�k����Motion���O
        public const short ERR_SpeedMode_SlaveSet = 152;	// �]�w�b�����������b�b���e�b��
        public const short ERR_VelChange_high = 153;	// �]�w�ȳt�ק��ܹL�j�άO����sec�L��
        public const short ERR_Backlash_step = 154;	// accstep+contstep+decstep�ݤp��100
        public const short ERR_Backlash_status = 155;	//�]�w��motion done���ݬ�0�Bbuffer length���ݬ�0	
        public const short ERR_DistOver = 156;	//��JDist�W�LTotalDist

        public const short Compare_Cards_Not_Equal = 201;	//��ﵲ�G�w�b�d��T(�d���B�ƶq)����
        public const short Compare_Nodes_Not_Equal = 202;	//��ﵲ�G�w������T(�ƶq)����
        public const short Compare_Node_ID_Not_Equal = 203;	//��ﵲ�G�w������T(����)����
        public const short Compare_Node_Device_Type_Not_Equal = 204;	//��ﵲ�G�w�Ҳո�T(�ҲղĤG����)����
        public const short Compare_Node_Identity_Object_Not_Equal = 205;	//��ﵲ�G�w�Ҳո�T(�ҲղĤ@����)����
        public const short Compare_File_Path_NULL = 206;	//�ɮ׸��|���~
        public const short Compare_File_Open_Fail = 207;	//�ɮ׶}�ҥ���(�нT�w���|��J���T)
        public const short Compare_File_Not_Exist = 208;	//�ɮפ��s�b

        //PCI_DMC_F01

        //main
        public const short ERR_NotCardFound = 301;//�L���d���Ω|��Initial
        public const short ERR_CardInitial = 302;	//Initial����
        public const short ERR_MemoryAccess_Failed = 303;	//�O����Ū�g����
        public const short ERR_MemoryOutOfRange = 304;//�O����ϥζW�LRange
        public const short ERR_UartTxIsBusy = 305;	//Uart Tx is busy
        public const short ERR_UartRxError = 306;	//Uart Rx Ū�����~
        public const short ERR_UartRxIsNotReady = 307;	//Uart Rx �|���ǳƧ���
        public const short ERR_NotSupportFunc = 308;	//���䴩��Function
        public const short ERR_NoNodeFound = 309;	//�����]�m���~
        public const short ERR_APIInputError = 310;	//API�Ѽƿ�J���~(�W�X���w��)
        public const short ERR_SDOFailed = 311;	//SDO�ǰe����
        public const short ERR_SDOBusy = 312;	//SDO���L�� / SDO�P�ɦ���ӫ��O�Q�g�J
        public const short ERR_APITypeErr = 313;	//���Ҳդ��䴩��API
        public const short ERR_ScaleFailed = 314;	//AD�ե�����
        public const short ERR_F_BufferFull = 315;//MailBox_Buffer�w��
        public const short ERR_ConnectErr = 316;	//�q�T�s�u���`
        public const short ERR_MBWordChFailed = 317;	//MailBox�P�ɦ���ӫ��O�g�J
        public const short ERR_MailBoxFailed = 318;	//MailBox�ǰe����
        public const short ERR_CantResetCard = 319;	//�L�kResetCard
        public const short ERR_PDOFailed = 320;	//PDO���^��
        public const short ERR_MBCmding = 321;//MailBox���b�B�z���O
        public const short ERR_SVOFF = 322;	//���ʧ@��Servo On��i����
        public const short ERR_DriverError = 323;	//���ʧ@�]DriverErr�L�k����A�Х�����Ralm
        public const short ERR_ConnReset_Failed = 324;//��l�ƭ��m����
        public const short ERR_F01SlotIDError = 325;	//���˸m���䴩Slot�ο�J��Slot�s���W�X�d��
        public const short ERR_UartData_NoMatch = 326;//Download CodeŪ��Uart����ƮɡA�^�ǵL�k�ŦX���T��
        public const short ERR_SVON = 327;//���ʧ@��Servo Off��i����
        public const short ERR_Mpg_Already_On = 328;	//���b�w�g�P�����BJog��DDA�\��A��l�\��ȮɵL�k�ϥ�
        public const short ERR_MpgNumber_Over = 329;	//�����Jog�P��ƶq�w�F�W��(�̤j3��)
        public const short ERR_Mpg_Data_Failed = 330;	//�����Jog��ƶǻ�����
        public const short ERR_DDA_Buffer_Full = 331;	//DDA Buffer�w��
        public const short ERR_F_Slave_Security_op = 332;	//Slave Secutiry�g�J����		
        public const short ERR_F_Security_Page = 333;	//Page�W�L�]�m
        public const short ERR_F_GetDLLPath = 334;//�䤣��DLL���|
        public const short ERR_F_GetDLLVersion = 335;	//�䤣��DLL�����T��
        public const short F_Compare_File_Open_Fail = 336;//�ɮ׶}�ҥ���(�нT�w���|��J���T)	
        public const short F_Compare_File_Not_Exist = 337;//�ɮפ��s�b
        public const short F_Compare_Cards_Not_Equal = 338;	//��ﵲ�G�w�b�d��T(�d���B�ƶq)����
        public const short F_Compare_File_Path_NULL = 339;//�ɮ׸��|���~
        public const short F_Compare_Node_ID_Not_Equal = 340;	//��ﵲ�G�w������T(����)����
        public const short F_Compare_Node_Device_Type_Not_Equal = 341;	//��ﵲ�G�w�Ҳո�T(�ҲղĤG����)����
        public const short F_Compare_Node_Identity_Object_Not_Equal = 342;	//��ﵲ�G�w�Ҳո�T(�ҲղĤ@����)����
        public const short F_Compare_Nodes_Not_Equal = 343;	//��ﵲ�G�w������T(�ƶq)����
        public const short ERR_SecurityNoRet = 344;	//Security�ǰe���G���^��
        public const short ERR_SDORetTimeOut = 345;	//SDO�^�Ǯɶ��L��
        public const short ERR_Uart_Connect_Fail = 346;	//Uart�q�T����
        public const short ERR_CardNum_SetError = 347;//�d���]�m���~
        public const short ERR_Target_Reached = 348;//�L���T����
        public const short ERR_NoF02Found = 349;//�䤣��F02
        public const short ERR_MCHSecurity = 350;//F02 Security Error


        public const short ERR_UseGetError = 399;	//�t�Ϋ����~, �ݨϥ�_DMC_01_master_alm_codeŪ�����~�X
        //sub if main = 99
        public const short ERR_sub_NoError = 0;	//���d���ҹ������b�d�S���o�Ϳ��~
        public const short ERR_sub_CantConnect = 1;		//DMC�q�T�s���L�k�ͦ�
        public const short ERR_sub_SDOFailed = 2;		//SDO�ǰe����
        public const short ERR_sub_CantReset = 3;		//�L�k���m�q�T

    }
}