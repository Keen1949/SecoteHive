#ifndef _DMC_LIB_H
#define _DMC_LIB_H

#ifndef TRUE
#define TRUE  1
#endif

#ifndef FALSE
#define FALSE 0
#endif

#ifndef NULL
#ifdef  __cplusplus
#define NULL    0
#else
#define NULL    ((void *)0)
#endif
#endif

typedef unsigned long       DWORD;
typedef int                 BOOL;
typedef unsigned char       BYTE;
typedef unsigned short      WORD;
typedef float               FLOAT;

typedef unsigned char  uint8;                   /* defined for unsigned 8-bits integer variable 	�޷���8λ���ͱ���  */
typedef signed   char  int8;                    /* defined for signed 8-bits integer variable		�з���8λ���ͱ���  */
typedef unsigned short uint16;                  /* defined for unsigned 16-bits integer variable 	�޷���16λ���ͱ��� */
typedef signed   short int16;                   /* defined for signed 16-bits integer variable 		�з���16λ���ͱ��� */
typedef unsigned int   uint32;                  /* defined for unsigned 32-bits integer variable 	�޷���32λ���ͱ��� */
typedef signed   int   int32;                   /* defined for signed 32-bits integer variable 		�з���32λ���ͱ��� */
typedef unsigned long long   uint64;    
typedef signed   long long   int64; 

typedef uint32 (__stdcall * DMC3K5K_OPERATE)(PVOID operator_data);

typedef struct
{
    uint32  m_Time;
	int32   m_CommandPos;
	double  m_CommandVel;
	uint32	m_CommandAcc;
	int32	m_FpgaPos;
	double	m_FpgaVel;
	int32	m_EncoderPos;
	double	m_ErrorPos;
}struct_PidAdjustData;


#define __DMC_EXPORTS

//������������
#ifdef __DMC_EXPORTS
	#define DMC_API __declspec(dllexport)
#else
	#define DMC_API __declspec(dllimport)
#endif

#ifdef __cplusplus
extern "C" {
#endif

//���úͶ�ȡ��ӡģʽ
DMC_API short __stdcall dmc_set_debug_mode(WORD mode,const char* pFileName);
DMC_API short __stdcall dmc_get_debug_mode(WORD* mode,char* pFileName);

DMC_API short __stdcall dmc_board_init(void); 	//��ʼ�����ƿ�
DMC_API short __stdcall dmc_board_init_onecard(WORD CardNo); 	////�ͻ����ƶ������� ��ʼ��һ�ſ��ƿ�
DMC_API short __stdcall dmc_board_close_onecard(WORD CardNo); 	////�ͻ����ƶ����ؿ� �ر�һ�����ƿ�
DMC_API short __stdcall	dmc_get_CardInfList(WORD* CardNum,DWORD* CardTypeList,WORD* CardIdList);//��ȡ���ƿ���Ϣ�б�
DMC_API short __stdcall dmc_board_close(void);	//�رտ��ƿ�
DMC_API short __stdcall dmc_board_reset(void);   //Ӳ����λ
DMC_API short __stdcall dmc_board_reset_onecard(WORD CardNo);//�ͻ����Ƶ���Ӳ����λ
DMC_API short __stdcall dmc_soft_reset(WORD CardNo);//���ƿ���λ��pci���� �ȸ�λ�����߿���
DMC_API short __stdcall dmc_cool_reset(WORD CardNo);//���ƿ��临λ
DMC_API short __stdcall dmc_original_reset(WORD CardNo);//���ƿ���ʼ��λ
DMC_API short __stdcall dmc_get_card_ID (WORD CardNo,DWORD *CardID);	//��ȡ���ƿ�����
DMC_API short __stdcall dmc_get_release_version(WORD CardNo,char *ReleaseVersion);//��ȡ�����汾��
DMC_API short __stdcall dmc_get_card_version(WORD CardNo,DWORD *CardVersion);	//��ȡ���ƿ�Ӳ���汾
DMC_API short __stdcall dmc_get_card_soft_version(WORD CardNo,DWORD *FirmID,DWORD *SubFirmID);	//��ȡ���ƿ�Ӳ���Ĺ̼��汾
DMC_API short __stdcall dmc_get_card_lib_version(DWORD *LibVer);	//��ȡ���ƿ���̬��汾
DMC_API short __stdcall dmc_get_total_axes(WORD CardNo,DWORD *TotalAxis); 	//��ȡָ��������
DMC_API short __stdcall dmc_get_total_liners(WORD CardNo,DWORD *TotalLiner); //��ȡָ�����岹����ϵ��

DMC_API short __stdcall dmc_get_total_ionum(WORD CardNo,WORD *TotalIn,WORD *TotalOut);//��ȡ����IO����
DMC_API short __stdcall dmc_get_total_adcnum(WORD CardNo,WORD* TotalIn,WORD* TotalOut);//��ȡ����ADDA���������

//���뺯��
DMC_API short __stdcall dmc_check_sn(WORD CardNo, const char * str_sn);
DMC_API short __stdcall dmc_write_sn(WORD CardNo, const char * str_sn);

/***********�����*************/
//����ģʽ		
DMC_API short __stdcall dmc_set_pulse_outmode(WORD CardNo,WORD axis,WORD outmode);	
DMC_API short __stdcall dmc_get_pulse_outmode(WORD CardNo,WORD axis,WORD* outmode);	
//���嵱��
DMC_API short __stdcall dmc_set_equiv(WORD CardNo,WORD axis, double equiv);
DMC_API short __stdcall dmc_get_equiv(WORD CardNo,WORD axis, double *equiv);
//�����϶(����)
DMC_API short __stdcall dmc_set_backlash_unit(WORD CardNo,WORD axis,double backlash);
DMC_API short __stdcall dmc_get_backlash_unit(WORD CardNo,WORD axis,double *backlash);
//�����϶(����)
DMC_API short __stdcall dmc_set_backlash(WORD CardNo,WORD axis,long backlash);
DMC_API short __stdcall dmc_get_backlash(WORD CardNo,WORD axis,long *backlash);
 
/***********************************�ļ�����**************************************/
/*********************************************************************************************************
�ļ�������� ���߿�
filetype
0-basic
1-gcode
2-setting
3-firewave
4-CAN configfile
100-trace data
*********************************************************************************************************/
DMC_API short __stdcall dmc_download_file(WORD CardNo, const char* pfilename, const char* pfilenameinControl,WORD filetype);
//�����ڴ��ļ� ���߿�
DMC_API short __stdcall dmc_download_memfile(WORD CardNo, const char* pbuffer, uint32 buffsize, const char* pfilenameinControl,WORD filetype);
//�ϴ��ļ�
DMC_API short __stdcall dmc_upload_file(WORD CardNo, const char* pfilename, const char* pfilenameinControl, WORD filetype);
//�ϴ��ڴ��ļ�
DMC_API short __stdcall dmc_upload_memfile(WORD CardNo, char* pbuffer, uint32 buffsize, const char* pfilenameinControl, uint32* puifilesize,WORD filetype);
//���ز����ļ�
DMC_API short __stdcall dmc_download_configfile(WORD CardNo,const char *FileName);
//���ع̼��ļ�
DMC_API short __stdcall dmc_download_firmware(WORD CardNo,const char *FileName);
//�ļ�����
DMC_API short __stdcall dmc_get_progress(WORD CardNo,float* process);

//��ȫ����
DMC_API short __stdcall dmc_set_softlimit(WORD CardNo,WORD axis,WORD enable, WORD source_sel,WORD SL_action, long N_limit,long P_limit);//��������λ����
DMC_API short __stdcall dmc_get_softlimit(WORD CardNo,WORD axis,WORD *enable, WORD *source_sel,WORD *SL_action,long *N_limit,long *P_limit);//��ȡ����λ����
DMC_API short __stdcall dmc_set_el_mode(WORD CardNo,WORD axis,WORD enable,WORD el_logic,WORD el_mode);//����EL�ź�
DMC_API short __stdcall dmc_get_el_mode(WORD CardNo,WORD axis,WORD *enable,WORD *el_logic,WORD *el_mode);//��ȡ����EL�ź�
DMC_API short __stdcall dmc_set_emg_mode(WORD CardNo,WORD axis,WORD enable,WORD emg_logic);//����EMG�ź�
DMC_API short __stdcall dmc_get_emg_mode(WORD CardNo,WORD axis,WORD *enable,WORD *emg_logic);//��ȡ����EMG�ź�

/*************************************�����˶�*****************************************/
//�ٶ�����		
DMC_API short __stdcall dmc_set_profile(WORD CardNo,WORD axis,double Min_Vel,double Max_Vel,double Tacc,double Tdec,double stop_vel);//�趨�ٶ����߲���
DMC_API short __stdcall dmc_get_profile(WORD CardNo,WORD axis,double *Min_Vel,double *Max_Vel,double *Tacc,double *Tdec,double *stop_vel);//��ȡ�ٶ����߲���
//�ٶ�����(���嵱��)
DMC_API short __stdcall dmc_set_profile_unit(WORD CardNo,WORD axis,double Min_Vel,double Max_Vel,double Tacc,double Tdec,double Stop_Vel);
DMC_API short __stdcall dmc_get_profile_unit(WORD CardNo,WORD axis,double* Min_Vel,double* Max_Vel,double* Tacc,double* Tdec,double* Stop_Vel);

//20160105�������ٶ������Լ��ٶ� ���ٶ� �����ٶ�����ʾ(����)
DMC_API short __stdcall dmc_set_profile_extern(WORD CardNo,WORD axis,double Min_Vel,double Max_Vel,double Tacc,double Tdec,double Ajerk,double Djerk,double stop_vel);
DMC_API short __stdcall dmc_get_profile_extern(WORD CardNo,WORD axis,double *Min_Vel,double *Max_Vel,double *Tacc,double *Tdec,double *Ajerk,double *Djerk,double *stop_vel);
//�ٶ��������ã����ٶ�ֵ��ʾ(����)
DMC_API short __stdcall dmc_set_acc_profile(WORD CardNo,WORD axis,double Min_Vel,double Max_Vel,double Acc,double Dec,double stop_vel);//�趨�ٶ����߲���
DMC_API short __stdcall dmc_get_acc_profile(WORD CardNo,WORD axis,double *Min_Vel,double *Max_Vel,double *Acc,double *Dec,double *stop_vel);//��ȡ�ٶ����߲���
//�ٶ��������ã����ٶ�ֵ��ʾ(����)
DMC_API short __stdcall dmc_set_profile_unit_acc(WORD CardNo,WORD axis,double Min_Vel,double Max_Vel,double Tacc,double Tdec,double Stop_Vel);
DMC_API short __stdcall dmc_get_profile_unit_acc(WORD CardNo,WORD axis,double* Min_Vel,double* Max_Vel,double* Tacc,double* Tdec,double* Stop_Vel);
DMC_API short __stdcall dmc_set_s_profile(WORD CardNo,WORD axis,WORD s_mode,double s_para);//����ƽ���ٶ����߲���
DMC_API short __stdcall dmc_get_s_profile(WORD CardNo,WORD axis,WORD s_mode,double *s_para);//��ȡƽ���ٶ����߲��� ����DMC5800 s_mode����ָ�뷵�ز���ֵ

//��λ�˶�(����)		
DMC_API short __stdcall dmc_pmove(WORD CardNo,WORD axis,long dist,WORD posi_mode);//ָ����������λ���˶�
//��λ�˶�(����)
DMC_API short __stdcall dmc_pmove_unit(WORD CardNo,WORD axis,double Dist,WORD posi_mode);
//���������ٶ��˶�		
DMC_API short __stdcall dmc_vmove(WORD CardNo,WORD axis,WORD dir);
//ָ����������λ���˶� ͬʱ�����ٶȺ�Sʱ��(����)
DMC_API short __stdcall dmc_pmove_extern(WORD CardNo, WORD axis, double dist,double Min_Vel, double Max_Vel, double Tacc, double Tdec, double stop_Vel, double s_para, WORD posi_mode);
//���߱�λ/����(����)
DMC_API short __stdcall dmc_reset_target_position(WORD CardNo,WORD axis,long dist,WORD posi_mode);//�˶��иı�Ŀ��λ��
DMC_API short __stdcall dmc_change_speed(WORD CardNo,WORD axis,double Curr_Vel,double Taccdec);//���߸ı�ָ����ĵ�ǰ�˶��ٶ�
DMC_API short __stdcall dmc_update_target_position(WORD CardNo,WORD axis,long dist,WORD posi_mode);//�����˶����ǿ�иı�Ŀ��λ��
//���ٱ�λ(����)
DMC_API short __stdcall dmc_reset_target_position_unit(WORD CardNo,WORD axis, double New_Pos);
DMC_API short __stdcall dmc_change_speed_unit(WORD CardNo,WORD axis, double New_Vel,double Taccdec);
DMC_API short __stdcall dmc_update_target_position_unit(WORD CardNo,WORD axis, double New_Pos);

/******************************�岹�˶�**********************************/
//3000ϵ���ٶ�����(����)
DMC_API short __stdcall dmc_set_vector_profile_multicoor(WORD CardNo,WORD Crd, double Min_Vel,double Max_Vel,double Tacc,double Tdec,double Stop_Vel);
DMC_API short __stdcall dmc_get_vector_profile_multicoor(WORD CardNo,WORD Crd, double* Min_Vel,double* Max_Vel,double* Tacc,double* Tdec,double* Stop_Vel);
DMC_API short __stdcall dmc_set_vector_s_profile_multicoor(WORD CardNo,WORD Crd,WORD s_mode,double s_para);//����ƽ���ٶ����߲���
DMC_API short __stdcall dmc_get_vector_s_profile_multicoor(WORD CardNo,WORD Crd,WORD s_mode,double *s_para);//��ȡƽ���ٶ����߲���

//�岹�ٶȲ���(����)
DMC_API short __stdcall dmc_set_vector_profile_unit(WORD CardNo,WORD Crd,double Min_Vel,double Max_Vel,double Tacc,double Tdec,double Stop_Vel);
DMC_API short __stdcall dmc_get_vector_profile_unit(WORD CardNo,WORD Crd,double* Min_Vel,double* Max_Vel,double* Tacc,double* Tdec,double* Stop_Vel);
DMC_API short __stdcall dmc_set_vector_s_profile(WORD CardNo,WORD Crd,WORD s_mode,double s_para);//����ƽ���ٶ����߲���
DMC_API short __stdcall dmc_get_vector_s_profile(WORD CardNo,WORD Crd,WORD s_mode,double *s_para);

//3000ϵ�в岹����(����)
DMC_API short __stdcall dmc_line_multicoor(WORD CardNo,WORD Crd,WORD axisNum,WORD *axisList,long *DistList,WORD posi_mode);	//ָ����ֱ�߲岹�˶�
DMC_API short __stdcall dmc_arc_move_multicoor(WORD CardNo,WORD Crd,WORD *AxisList,long *Target_Pos,long *Cen_Pos,WORD Arc_Dir,WORD posi_mode);//Բ���岹�˶�
//���β岹(����)
DMC_API short __stdcall dmc_line_unit(WORD CardNo,WORD Crd,WORD AxisNum,WORD* AxisList,double* Dist,WORD posi_mode);
DMC_API short __stdcall dmc_arc_move_center_unit(WORD CardNo,WORD Crd,WORD AxisNum,WORD* AxisList,double *Target_Pos,double *Cen_Pos,WORD Arc_Dir,long Circle,WORD posi_mode);
DMC_API short __stdcall dmc_arc_move_radius_unit(WORD CardNo,WORD Crd,WORD AxisNum,WORD* AxisList,double *Target_Pos,double Arc_Radius,WORD Arc_Dir,long Circle,WORD posi_mode);
DMC_API short __stdcall dmc_arc_move_3points_unit(WORD CardNo,WORD Crd,WORD AxisNum,WORD* AxisList,double *Target_Pos,double *Mid_Pos,long Circle,WORD posi_mode);
DMC_API short __stdcall dmc_rectangle_move_unit(WORD CardNo,WORD Crd,WORD AxisNum,WORD* AxisList,double* Target_Pos,double* Mark_Pos,long num,WORD rect_mode,WORD posi_mode);

/********************PVT�˶�****************************/
//PVT�˶� �ɰ�
DMC_API short __stdcall dmc_PvtTable(WORD CardNo,WORD iaxis,DWORD count,double *pTime,long *pPos,double *pVel);
DMC_API short __stdcall dmc_PtsTable(WORD CardNo,WORD iaxis,DWORD count,double *pTime,long *pPos,double *pPercent);
DMC_API short __stdcall dmc_PvtsTable(WORD CardNo,WORD iaxis,DWORD count,double *pTime,long *pPos,double velBegin,double velEnd);
DMC_API short __stdcall dmc_PttTable(WORD CardNo,WORD iaxis,DWORD count,double *pTime,long *pPos);
DMC_API short __stdcall dmc_PvtMove(WORD CardNo,WORD AxisNum,WORD* AxisList);
//PVT���������
DMC_API short __stdcall dmc_PttTable_add(WORD CardNo,WORD iaxis,DWORD count,double *pTime,long *pPos);
DMC_API short __stdcall dmc_PtsTable_add(WORD CardNo,WORD iaxis,DWORD count,double *pTime,long *pPos,double *pPercent);
DMC_API short __stdcall dmc_pvt_get_remain_space(WORD CardNo,WORD iaxis);//��ȡpvtʣ��ռ�
/*****************************************************************************
PVT�˶� ���߿��¹滮
******************************************************************************/
DMC_API short __stdcall dmc_pvt_table_unit(WORD CardNo,WORD iaxis,DWORD count,double *pTime,double *pPos,double *pVel);
DMC_API short __stdcall dmc_pts_table_unit(WORD CardNo,WORD iaxis,DWORD count,double *pTime,double *pPos,double *pPercent);
DMC_API short __stdcall dmc_pvts_table_unit(WORD CardNo,WORD iaxis,DWORD count,double *pTime,double *pPos,double velBegin,double velEnd);
DMC_API short __stdcall dmc_ptt_table_unit(WORD CardNo,WORD iaxis,DWORD count,double *pTime,double *pPos);
DMC_API short __stdcall dmc_pvt_move(WORD CardNo,WORD AxisNum,WORD* AxisList);

DMC_API short __stdcall dmc_SetGearProfile(WORD CardNo,WORD axis,WORD MasterType, WORD MasterIndex,long MasterEven,long SlaveEven,DWORD MasterSlope);
DMC_API short __stdcall dmc_GetGearProfile(WORD CardNo,WORD axis,WORD* MasterType, WORD* MasterIndex,long* MasterEven,long* SlaveEven,DWORD* MasterSlope);
DMC_API short __stdcall dmc_GearMove(WORD CardNo,WORD AxisNum,WORD* AxisList);

/************************�����˶�*************************/	
DMC_API short __stdcall dmc_set_home_pin_logic(WORD CardNo,WORD axis,WORD org_logic,double filter);//����HOME�ź�
DMC_API short __stdcall dmc_get_home_pin_logic(WORD CardNo,WORD axis,WORD *org_logic,double *filter);//��ȡ����HOME�ź�
DMC_API short __stdcall dmc_set_homemode(WORD CardNo,WORD axis,WORD home_dir,double vel,WORD mode,WORD EZ_count);//�趨ָ����Ļ�ԭ��ģʽ
DMC_API short __stdcall dmc_get_homemode(WORD CardNo,WORD axis,WORD *home_dir, double *vel_mode,WORD *home_mode,WORD *EZ_count);//��ȡָ����Ļ�ԭ��ģʽ
DMC_API short __stdcall dmc_home_move(WORD CardNo,WORD axis);//��������
DMC_API short __stdcall dmc_set_home_profile_unit(WORD CardNo,WORD axis,double Low_Vel,double High_Vel,double Tacc,double Tdec);//���û����ٶȲ���
DMC_API short __stdcall dmc_get_home_profile_unit(WORD CardNo,WORD axis,double* Low_Vel,double* High_Vel,double* Tacc,double* Tdec);//��ȡ�����ٶȲ���
DMC_API short __stdcall dmc_get_home_result(WORD CardNo,WORD axis,WORD* state);//��ȡ����ִ��״̬
DMC_API short __stdcall dmc_set_home_position_unit(WORD CardNo,WORD axis,WORD enable,double position);
DMC_API short __stdcall dmc_get_home_position_unit(WORD CardNo,WORD axis,WORD *enable,double *position);
DMC_API short __stdcall dmc_set_el_home(WORD CardNo,WORD axis,WORD mode);

/***************************ԭ������******************************/
DMC_API short __stdcall dmc_set_homelatch_mode(WORD CardNo,WORD axis,WORD enable,WORD logic,WORD source);
DMC_API short __stdcall dmc_get_homelatch_mode(WORD CardNo,WORD axis,WORD* enable,WORD* logic,WORD* source);
DMC_API long __stdcall dmc_get_homelatch_flag(WORD CardNo,WORD axis);
DMC_API short __stdcall dmc_reset_homelatch_flag(WORD CardNo,WORD axis);
DMC_API long __stdcall dmc_get_homelatch_value(WORD CardNo,WORD axis);
/*****************************EZ����********************************/
DMC_API short __stdcall dmc_set_ezlatch_mode(WORD CardNo,WORD axis,WORD enable,WORD logic,WORD source);
DMC_API short __stdcall dmc_get_ezlatch_mode(WORD CardNo,WORD axis,WORD* enable,WORD* logic,WORD* source);
DMC_API long __stdcall dmc_get_ezlatch_flag(WORD CardNo,WORD axis);
DMC_API short __stdcall dmc_reset_ezlatch_flag(WORD CardNo,WORD axis);
DMC_API long __stdcall dmc_get_ezlatch_value(WORD CardNo,WORD axis);

/************************�����˶�����*********************************/	
//һ��һ
DMC_API short __stdcall dmc_set_handwheel_inmode(WORD CardNo,WORD axis,WORD inmode,long multi,double vh);//�����������������źŵĹ�����ʽ
DMC_API short __stdcall dmc_get_handwheel_inmode(WORD CardNo,WORD axis,WORD *inmode,long *multi,double *vh);//��ȡ�������������źŵĹ�����ʽ
//һ��һ�����ͱ���
DMC_API short __stdcall dmc_set_handwheel_inmode_decimals(WORD CardNo,WORD axis,WORD inmode,double multi,double vh);
DMC_API short __stdcall dmc_get_handwheel_inmode_decimals(WORD CardNo,WORD axis,WORD *inmode,double *multi,double *vh);
DMC_API short __stdcall dmc_handwheel_move(WORD CardNo,WORD axis);
//��������ͨ��
DMC_API short __stdcall dmc_set_handwheel_channel(WORD CardNo,WORD index);
DMC_API short __stdcall dmc_get_handwheel_channel(WORD CardNo,WORD* index);
//һ�Զ�
DMC_API short __stdcall dmc_set_handwheel_inmode_extern(WORD CardNo,WORD inmode,WORD AxisNum,WORD* AxisList,long* multi);
DMC_API short __stdcall dmc_get_handwheel_inmode_extern(WORD CardNo,WORD* inmode,WORD* AxisNum,WORD* AxisList,long *multi);	
//һ�Զม���ͱ���
DMC_API short __stdcall dmc_set_handwheel_inmode_extern_decimals(WORD CardNo,WORD inmode,WORD AxisNum,WORD* AxisList,double* multi);	
DMC_API short __stdcall dmc_get_handwheel_inmode_extern_decimals(WORD CardNo,WORD* inmode,WORD* AxisNum,WORD* AxisList,double *multi);

/*********************************************************************************************************
�����˶� �������ߵ�����ģʽ  ����
*********************************************************************************************************/
DMC_API short __stdcall dmc_handwheel_set_axislist( WORD CardNo, WORD AxisSelIndex,WORD AxisNum,WORD* AxisList);
DMC_API short __stdcall dmc_handwheel_get_axislist( WORD CardNo,WORD AxisSelIndex, WORD* AxisNum, WORD* AxisList);
DMC_API short __stdcall dmc_handwheel_set_ratiolist( WORD CardNo, WORD AxisSelIndex, WORD StartRatioIndex, WORD RatioSelNum, double* RatioList);
DMC_API short __stdcall dmc_handwheel_get_ratiolist( WORD CardNo,WORD AxisSelIndex, WORD StartRatioIndex, WORD RatioSelNum,double* RatioList );
DMC_API short __stdcall dmc_handwheel_set_mode( WORD CardNo, WORD InMode, WORD IfHardEnable );
DMC_API short __stdcall dmc_handwheel_get_mode ( WORD CardNo, WORD* InMode, WORD*  IfHardEnable );
DMC_API short __stdcall dmc_handwheel_set_index( WORD CardNo, WORD AxisSelIndex,WORD RatioSelIndex );
DMC_API short __stdcall dmc_handwheel_get_index( WORD CardNo, WORD* AxisSelIndex,WORD* RatioSelIndex );
DMC_API short __stdcall dmc_handwheel_move( WORD CardNo, WORD ForceMove );
DMC_API short __stdcall dmc_handwheel_stop ( WORD CardNo );

/**************************�������溯��***************************/
/*************************************
LTC1	AXIS0	AXIS1	AXIS2	AXIS3
LTC2	AXIS4	AXIS5	AXIS6	AXIS7
***************************************/
DMC_API short __stdcall dmc_set_ltc_mode(WORD CardNo,WORD axis,WORD ltc_logic,WORD ltc_mode,double filter);//����LTC�ź�
DMC_API short __stdcall dmc_get_ltc_mode(WORD CardNo,WORD axis,WORD*ltc_logic,WORD*ltc_mode,double *filter);//��ȡ����LTC�ź�
DMC_API short __stdcall dmc_set_latch_mode(WORD CardNo,WORD axis,WORD all_enable,WORD latch_source,WORD triger_chunnel);//�������淽ʽ
DMC_API short __stdcall dmc_get_latch_mode(WORD CardNo,WORD axis,WORD *all_enable,WORD* latch_source,WORD* triger_chunnel);
DMC_API short __stdcall dmc_SetLtcOutMode(WORD CardNo,WORD axis,WORD enable,WORD bitno);//�������
DMC_API short __stdcall dmc_GetLtcOutMode(WORD CardNo,WORD axis,WORD *enable,WORD* bitno);
DMC_API short __stdcall dmc_get_latch_flag(WORD CardNo,WORD axis);//��ȡ��������־
DMC_API short __stdcall dmc_reset_latch_flag(WORD CardNo,WORD axis);//��λ��������־
DMC_API long __stdcall  dmc_get_latch_value(WORD CardNo,WORD axis);//��ȡ��������������ֵ
DMC_API short __stdcall dmc_get_latch_value_unit(WORD CardNo,WORD axis,double* pos_by_mm);
DMC_API short __stdcall dmc_get_latch_flag_extern(WORD CardNo,WORD axis);//��ȡ��������־
DMC_API long __stdcall dmc_get_latch_value_extern(WORD CardNo,WORD axis,WORD index);//������ȡֵ
DMC_API short __stdcall dmc_set_latch_stop_time(WORD CardNo,WORD axis,long time);//������ͣʱ��
DMC_API short __stdcall dmc_get_latch_stop_time(WORD CardNo,WORD axis,long* time); 

/*********************************************************************************************************
�������� �¹滮20170308 ����
*********************************************************************************************************/
//����������������ģʽ0-�������棬1-�������棻�������0-�½��أ�1-�����أ�2-˫���أ��˲�ʱ�䣬��λus
DMC_API short __stdcall dmc_ltc_set_mode(WORD CardNo,WORD latch,WORD ltc_mode,WORD ltc_logic,double filter);
DMC_API short __stdcall dmc_ltc_get_mode(WORD CardNo,WORD latch,WORD *ltc_mode,WORD *ltc_logic,double *filter);
//��������Դ��0-ָ��λ�ã�1-����������λ��
DMC_API short __stdcall dmc_ltc_set_source(WORD CardNo,WORD latch,WORD axis,WORD ltc_source);
DMC_API short __stdcall dmc_ltc_get_source(WORD CardNo,WORD latch,WORD axis,WORD *ltc_source);
//��λ������
DMC_API short __stdcall dmc_ltc_reset(WORD CardNo,WORD latch);
//��ȡ�������
DMC_API short __stdcall dmc_ltc_get_number(WORD CardNo,WORD latch,WORD axis,int *number);
//��ȡ����ֵ
DMC_API short __stdcall dmc_ltc_get_value_unit(WORD CardNo,WORD latch,WORD axis,double *value);

/*****************************λ�ñȽϺ���****************************/
//����λ�ñȽ�	
DMC_API short __stdcall dmc_compare_set_config(WORD CardNo,WORD axis,WORD enable, WORD cmp_source);//���ñȽ���
DMC_API short __stdcall dmc_compare_get_config(WORD CardNo,WORD axis,WORD *enable, WORD *cmp_source);//��ȡ���ñȽ���
DMC_API short __stdcall dmc_compare_clear_points(WORD CardNo,WORD cmp);//������бȽϵ�
DMC_API short __stdcall dmc_compare_add_point(WORD CardNo,WORD cmp,long pos,WORD dir, WORD action,DWORD actpara);//��ӱȽϵ�
DMC_API short __stdcall dmc_compare_get_current_point(WORD CardNo,WORD cmp,long *pos);//��ȡ��ǰ�Ƚϵ�
DMC_API short __stdcall dmc_compare_get_points_runned(WORD CardNo,WORD cmp,long *pointNum);//��ѯ�Ѿ��ȽϹ��ĵ�
DMC_API short __stdcall dmc_compare_get_points_remained(WORD CardNo,WORD cmp,long *pointNum);//��ѯ���Լ���ıȽϵ�����

//��άλ�ñȽ�
DMC_API short __stdcall dmc_compare_set_config_extern(WORD CardNo,WORD enable, WORD cmp_source);//���ñȽ���
DMC_API short __stdcall dmc_compare_get_config_extern(WORD CardNo,WORD *enable, WORD *cmp_source);//��ȡ���ñȽ���
DMC_API short __stdcall dmc_compare_clear_points_extern(WORD CardNo);//������бȽϵ�
DMC_API short __stdcall dmc_compare_add_point_extern(WORD CardNo,WORD* axis,long* pos,WORD* dir, WORD action,DWORD actpara);//�������λ�ñȽϵ�
DMC_API short __stdcall dmc_compare_get_current_point_extern(WORD CardNo,long *pos);//��ȡ��ǰ�Ƚϵ�
DMC_API short __stdcall dmc_compare_add_point_extern_unit(WORD CardNo,WORD* axis,double* pos,WORD* dir, WORD action,DWORD actpara);//�������λ�ñȽϵ�
DMC_API short __stdcall dmc_compare_get_current_point_extern_unit(WORD CardNo,double *pos);//��ȡ��ǰ�Ƚϵ�
DMC_API short __stdcall dmc_compare_get_points_runned_extern(WORD CardNo,long *pointNum);//��ѯ�Ѿ��ȽϹ��ĵ�
DMC_API short __stdcall dmc_compare_get_points_remained_extern(WORD CardNo,long *pointNum);//��ѯ���Լ���ıȽϵ�����

//����λ�ñȽ�
DMC_API short __stdcall dmc_compare_set_config_multi(WORD CardNo,WORD queue,WORD enable, WORD axis, WORD cmp_source);//���ñȽ���
DMC_API short __stdcall dmc_compare_get_config_multi(WORD CardNo, WORD queue,WORD* enable, WORD* axis, WORD* cmp_source);//��ȡ���ñȽ���
DMC_API short __stdcall dmc_compare_add_point_multi(WORD CardNo, WORD cmp,int32 pos, WORD dir,  WORD action, DWORD actpara,double times);//��ӱȽϵ� ��ǿ

//����λ�ñȽ�
DMC_API short __stdcall dmc_hcmp_set_mode(WORD CardNo,WORD hcmp, WORD cmp_mode);//���ø��ٱȽ�ģʽ
DMC_API short __stdcall dmc_hcmp_get_mode(WORD CardNo,WORD hcmp, WORD* cmp_mode);
//���ٱȽ�ģʽ��չ
DMC_API short __stdcall dmc_hcmp_set_config_extern(WORD CardNo,WORD hcmp,WORD axis, WORD cmp_source, WORD cmp_logic,WORD cmp_mode,long dist,long time);
DMC_API short __stdcall dmc_hcmp_get_config_extern(WORD CardNo,WORD hcmp,WORD* axis, WORD* cmp_source, WORD* cmp_logic,WORD* cmp_mode,long* dist,long* time);

DMC_API short __stdcall dmc_hcmp_set_config(WORD CardNo,WORD hcmp,WORD axis, WORD cmp_source, WORD cmp_logic,long time);//���ø��ٱȽϲ���
DMC_API short __stdcall dmc_hcmp_get_config(WORD CardNo,WORD hcmp,WORD* axis, WORD* cmp_source, WORD* cmp_logic,long* time);
DMC_API short __stdcall dmc_hcmp_add_point(WORD CardNo,WORD hcmp, long cmp_pos);
DMC_API short __stdcall dmc_hcmp_set_liner(WORD CardNo,WORD hcmp, long Increment,long Count);//��������ģʽ����
DMC_API short __stdcall dmc_hcmp_get_liner(WORD CardNo,WORD hcmp, long* Increment,long* Count);
DMC_API short __stdcall dmc_hcmp_get_current_state(WORD CardNo,WORD hcmp,long *remained_points,long *current_point,long *runned_points); //��ȡ���ٱȽ�״̬
DMC_API short __stdcall dmc_hcmp_clear_points(WORD CardNo,WORD hcmp);
DMC_API short __stdcall dmc_read_cmp_pin(WORD CardNo,WORD axis);
DMC_API short __stdcall dmc_write_cmp_pin(WORD CardNo,WORD axis, WORD on_off);//����cmp�˿����

//��ά����λ�ñȽϹ���
DMC_API short __stdcall dmc_hcmp_2d_set_enable(WORD CardNo,WORD hcmp, WORD cmp_enable);
DMC_API short __stdcall dmc_hcmp_2d_get_enable(WORD CardNo,WORD hcmp, WORD *cmp_enable);
DMC_API short __stdcall dmc_hcmp_2d_set_config(WORD CardNo,WORD hcmp,WORD cmp_mode,WORD x_axis, WORD x_cmp_source, WORD y_axis, WORD y_cmp_source, long error,WORD cmp_logic,long time,WORD pwm_enable,double duty,long freq,WORD port_sel,WORD pwm_number);
DMC_API short __stdcall dmc_hcmp_2d_get_config(WORD CardNo,WORD hcmp,WORD *cmp_mode,WORD *x_axis, WORD *x_cmp_source, WORD *y_axis, WORD *y_cmp_source, long *error,WORD *cmp_logic,long *time,WORD *pwm_enable,double *duty,long *freq,WORD *port_sel,WORD *pwm_number);
DMC_API short __stdcall dmc_hcmp_2d_add_point(WORD CardNo,WORD hcmp, long x_cmp_pos, long y_cmp_pos);
DMC_API short __stdcall dmc_hcmp_2d_get_current_state(WORD CardNo,WORD hcmp,long *remained_points,long *x_current_point,long *y_current_point,long *runned_points,WORD *current_state); 
DMC_API short __stdcall dmc_hcmp_2d_clear_points(WORD CardNo,WORD hcmp);
DMC_API short __stdcall dmc_hcmp_2d_force_output(WORD CardNo,WORD hcmp,WORD enable);

/********************ͨ��IO����**************************/
//ͨ��IO
DMC_API short __stdcall dmc_read_inbit(WORD CardNo,WORD bitno);//��ȡ����ڵ�״̬
DMC_API short __stdcall dmc_write_outbit(WORD CardNo,WORD bitno,WORD on_off);//��������ڵ�״̬
DMC_API short __stdcall dmc_read_outbit(WORD CardNo,WORD bitno);//��ȡ����ڵ�״̬
DMC_API DWORD __stdcall dmc_read_inport(WORD CardNo,WORD portno);//��ȡ����˿ڵ�ֵ
DMC_API DWORD __stdcall dmc_read_outport(WORD CardNo,WORD portno);//��ȡ����˿ڵ�ֵ
DMC_API short __stdcall dmc_write_outport(WORD CardNo,WORD portno,DWORD outport_val);//������������˿ڵ�ֵ

DMC_API short __stdcall dmc_write_outport_16X(WORD CardNo,WORD portno,DWORD outport_val);//����ͨ������˿ڵ�ֵ

//����IOӳ��
DMC_API short __stdcall dmc_set_io_map_virtual(WORD CardNo,WORD bitno,WORD MapIoType,WORD MapIoIndex,double Filter);
DMC_API short __stdcall dmc_get_io_map_virtual(WORD CardNo,WORD bitno,WORD* MapIoType,WORD* MapIoIndex,double* Filter);
DMC_API short __stdcall dmc_read_inbit_virtual(WORD CardNo,WORD bitno); //��ȡ����ڵ�״̬

DMC_API short __stdcall dmc_reverse_outbit(WORD CardNo,WORD bitno,double reverse_time);//IO��ʱ��ת
DMC_API short __stdcall dmc_set_io_count_mode(WORD CardNo,WORD bitno,WORD mode,double filter);//����IO����ģʽ
DMC_API short __stdcall dmc_get_io_count_mode(WORD CardNo,WORD bitno,WORD *mode,double* filter);
DMC_API short __stdcall dmc_set_io_count_value(WORD CardNo,WORD bitno,DWORD CountValue);//����IO����ֵ
DMC_API short __stdcall dmc_get_io_count_value(WORD CardNo,WORD bitno,DWORD* CountValue);

/*********************ר��IO�ź�************************/
DMC_API short __stdcall dmc_set_axis_io_map(WORD CardNo,WORD Axis,WORD IoType,WORD MapIoType,WORD MapIoIndex,double Filter);
DMC_API short __stdcall dmc_get_axis_io_map(WORD CardNo,WORD Axis,WORD IoType,WORD* MapIoType,WORD* MapIoIndex,double* Filter);
DMC_API short __stdcall dmc_set_special_input_filter(WORD CardNo,double Filter);//��������ר��IO�˲�ʱ��

//3410ר�� ��ԭ������ź�����
DMC_API short __stdcall dmc_set_sd_mode(WORD CardNo,WORD axis,WORD enable,WORD sd_logic,WORD sd_mode);//����SD�ź�
DMC_API short __stdcall dmc_get_sd_mode(WORD CardNo,WORD axis,WORD* enable,WORD *sd_logic,WORD *sd_mode);//��ȡ����SD�ź�
//ר��IO
DMC_API short __stdcall dmc_set_inp_mode(WORD CardNo,WORD axis,WORD enable,WORD inp_logic);//����INP�ź�
DMC_API short __stdcall dmc_get_inp_mode(WORD CardNo,WORD axis,WORD *enable,WORD *inp_logic);//��ȡ����INP�ź�
DMC_API short __stdcall dmc_set_rdy_mode(WORD CardNo,WORD axis,WORD enable,WORD rdy_logic);//����RDY�ź�
DMC_API short __stdcall dmc_get_rdy_mode(WORD CardNo,WORD axis,WORD* enable,WORD* rdy_logic);//��ȡ����RDY�ź�
DMC_API short __stdcall dmc_set_erc_mode(WORD CardNo,WORD axis,WORD enable,WORD erc_logic,WORD erc_width,WORD erc_off_time);//����ERC�ź�
DMC_API short __stdcall dmc_get_erc_mode(WORD CardNo,WORD axis,WORD *enable,WORD *erc_logic, WORD *erc_width,WORD *erc_off_time);//��ȡ����ERC�ź�
DMC_API short __stdcall dmc_set_alm_mode(WORD CardNo,WORD axis,WORD enable,WORD alm_logic,WORD alm_action);//����ALM�ź�
DMC_API short __stdcall dmc_get_alm_mode(WORD CardNo,WORD axis,WORD *enable,WORD *alm_logic,WORD *alm_action);//��ȡ����ALM�ź�
DMC_API short __stdcall dmc_set_ez_mode(WORD CardNo,WORD axis,WORD ez_logic,WORD ez_mode,double filter);//����EZ�ź�
DMC_API short __stdcall dmc_get_ez_mode(WORD CardNo,WORD axis,WORD *ez_logic,WORD *ez_mode,double *filter);//��ȡ����EZ�ź�

DMC_API short __stdcall dmc_write_sevon_pin(WORD CardNo,WORD axis,WORD on_off);//���SEVON�ź�
DMC_API short __stdcall dmc_read_sevon_pin(WORD CardNo,WORD axis);//��ȡSEVON�ź�
DMC_API short __stdcall dmc_read_rdy_pin(WORD CardNo,WORD axis);//��ȡRDY״̬
DMC_API short __stdcall dmc_write_erc_pin(WORD CardNo,WORD axis,WORD on_off);//����ERC�ź����
DMC_API short __stdcall dmc_read_erc_pin(WORD CardNo,WORD axis); 	
DMC_API short __stdcall dmc_write_sevrst_pin(WORD CardNo,WORD axis,WORD on_off);//����ŷ���λ�ź�
DMC_API short __stdcall dmc_read_sevrst_pin(WORD CardNo,WORD axis);//���ŷ���λ�ź�

//�ⲿ����ֹͣ�źż�����ֹͣʱ������
DMC_API short __stdcall dmc_set_io_dstp_mode(WORD CardNo,WORD axis,WORD enable,WORD logic);//enable:0-���ã�1-��ʱ�����ֹͣ��2-���������ֹͣ
DMC_API short __stdcall dmc_get_io_dstp_mode(WORD CardNo,WORD axis,WORD *enable,WORD *logic); 
//����ֹͣʱ��
DMC_API short __stdcall dmc_set_dec_stop_time(WORD CardNo,WORD axis,double time);
DMC_API short __stdcall dmc_get_dec_stop_time(WORD CardNo,WORD axis,double *time);
//�岹����ֹͣ�źźͼ���ʱ������
DMC_API short __stdcall dmc_set_vector_dec_stop_time(WORD CardNo,WORD Crd,double time);
DMC_API short __stdcall dmc_get_vector_dec_stop_time(WORD CardNo,WORD Crd,double *time);
//����ֹͣ����
DMC_API short __stdcall dmc_set_dec_stop_dist(WORD CardNo,WORD axis,long dist);
DMC_API short __stdcall dmc_get_dec_stop_dist(WORD CardNo,WORD axis,long *dist);
DMC_API short __stdcall dmc_set_io_dstp_bitno(WORD CardNo,WORD axis,WORD bitno,double filter);//����ͨ������ڵ�һλ����ֹͣIO��
DMC_API short __stdcall dmc_get_io_dstp_bitno(WORD CardNo,WORD axis,WORD *bitno,double* filter);

/************************����������**********************/	
DMC_API short __stdcall dmc_set_counter_inmode(WORD CardNo,WORD axis,WORD mode);//�趨�������ļ�����ʽ
DMC_API short __stdcall dmc_get_counter_inmode(WORD CardNo,WORD axis,WORD *mode);//��ȡ�������ļ�����ʽ
//������ֵ(����)
DMC_API long __stdcall dmc_get_encoder(WORD CardNo,WORD axis);
DMC_API short __stdcall dmc_set_encoder(WORD CardNo,WORD axis,long encoder_value);
//������ֵ(����)
DMC_API short __stdcall dmc_set_encoder_unit(WORD CardNo,WORD axis, double pos);
DMC_API short __stdcall dmc_get_encoder_unit(WORD CardNo,WORD axis, double * pos);
//���ֱ����� ���߿�
DMC_API short __stdcall dmc_set_handwheel_encoder(WORD CardNo,WORD channel, long pos);
DMC_API short __stdcall dmc_get_handwheel_encoder(WORD CardNo,WORD channel, long * pos);
//���ø�������ģʽ
DMC_API short __stdcall dmc_set_extra_encoder_mode(WORD CardNo,WORD channel,WORD inmode,WORD multi);
DMC_API short __stdcall dmc_get_extra_encoder_mode(WORD CardNo,WORD channel,WORD* inmode,WORD* multi);
//���ø���������ֵ
DMC_API short __stdcall dmc_set_extra_encoder(WORD CardNo,WORD channel, int pos);
DMC_API short __stdcall dmc_get_extra_encoder(WORD CardNo,WORD channel, int * pos);

/*********************λ�ü�������***************************/
//��ǰλ��(����)
DMC_API short __stdcall dmc_set_position(WORD CardNo,WORD axis,long current_position);
DMC_API long __stdcall dmc_get_position(WORD CardNo,WORD axis);	
//��ǰλ��(����)
DMC_API short __stdcall dmc_set_position_unit(WORD CardNo,WORD axis, double pos);
DMC_API short __stdcall dmc_get_position_unit(WORD CardNo,WORD axis, double * pos);

/**************************�˶�״̬********************************/
//��״̬
DMC_API double __stdcall dmc_read_current_speed(WORD CardNo,WORD axis);	//��ȡָ����ĵ�ǰ�ٶ�(����)
DMC_API short __stdcall dmc_read_current_speed_unit(WORD CardNo,WORD axis, double *current_speed);//��ȡ��ǰ�ٶ�(����)
DMC_API double __stdcall dmc_read_vector_speed(WORD CardNo);	//��ȡ��ǰ���Ĳ岹�ٶ�
DMC_API long __stdcall dmc_get_target_position(WORD CardNo,WORD axis);	//��ȡָ�����Ŀ��λ��
DMC_API short __stdcall dmc_get_target_position_unit(WORD CardNo,WORD axis, double * pos);//��ȡָ�����Ŀ��λ��(����)
DMC_API short __stdcall dmc_check_done(WORD CardNo,WORD axis);	//��ȡָ������˶�״̬

DMC_API DWORD __stdcall dmc_axis_io_status(WORD CardNo,WORD axis);//��ȡָ�����й��˶��źŵ�״̬
DMC_API short __stdcall dmc_stop(WORD CardNo,WORD axis,WORD stop_mode);//����ֹͣ
DMC_API short __stdcall dmc_check_done_multicoor(WORD CardNo,WORD Crd);//�岹�˶�״̬
DMC_API short __stdcall dmc_stop_multicoor(WORD CardNo,WORD Crd,WORD stop_mode);//ֹͣ�岹��
DMC_API short __stdcall dmc_emg_stop(WORD CardNo);//����ֹͣ������
DMC_API short __stdcall dmc_LinkState(WORD CardNo,WORD* State);//����״̬
DMC_API short __stdcall dmc_get_axis_run_mode(WORD CardNo, WORD axis,WORD* run_mode);//��ȡָ������˶�ģʽ
DMC_API short __stdcall dmc_get_stop_reason(WORD CardNo,WORD axis,long* StopReason);//��ȡֹͣԭ��
DMC_API short __stdcall dmc_clear_stop_reason(WORD CardNo,WORD axis);//���ֹͣԭ��

//trace����
DMC_API short __stdcall dmc_set_trace(WORD CardNo,WORD axis,WORD enable);
DMC_API short __stdcall dmc_get_trace(WORD CardNo,WORD axis,WORD* enable);
DMC_API short __stdcall dmc_read_trace_data(WORD CardNo,WORD axis,WORD data_option,long* ReceiveSize,double* time,double* data,long* remain_num);
DMC_API short __stdcall dmc_trace_start(WORD CardNo,WORD AxisNum,WORD *AxisList);
DMC_API short __stdcall dmc_trace_stop(WORD CardNo);

//��������
DMC_API short __stdcall dmc_calculate_arclength_center(double* start_pos,double *target_pos,double *cen_pos, WORD arc_dir,double circle,double* ArcLength);

/*********************************�����岹����**************************************
�����岹���ٶȺ����͵��β岹��ͬ
************************************************************************************/
DMC_API short __stdcall dmc_conti_open_list (WORD CardNo,WORD Crd,WORD AxisNum,WORD *AxisList);//������������
DMC_API short __stdcall dmc_conti_close_list(WORD CardNo,WORD Crd);//�ر�����������
DMC_API short __stdcall dmc_conti_reset_list(WORD CardNo,WORD Crd);//��λ����������
DMC_API short __stdcall dmc_conti_stop_list (WORD CardNo,WORD Crd,WORD stop_mode);//�����岹��ֹͣ
DMC_API short __stdcall dmc_conti_pause_list(WORD CardNo,WORD Crd);//�����岹����ͣ
DMC_API short __stdcall dmc_conti_start_list(WORD CardNo,WORD Crd);//��ʼ�����岹
DMC_API short __stdcall dmc_conti_get_run_state(WORD CardNo,WORD Crd);//0-���У�1-��ͣ��2-����ֹͣ��3-δ������4-����
DMC_API long __stdcall dmc_conti_remain_space (WORD CardNo,WORD Crd);//�������岹ʣ�໺����
DMC_API long __stdcall dmc_conti_read_current_mark (WORD CardNo,WORD Crd);//��ȡ��ǰ�����岹�εı��

//blendģʽ
DMC_API short __stdcall dmc_conti_set_blend(WORD CardNo,WORD Crd,WORD enable);
DMC_API short __stdcall dmc_conti_get_blend(WORD CardNo,WORD Crd,WORD* enable);
DMC_API short __stdcall dmc_conti_set_override(WORD CardNo,WORD Crd,double Percent);//���ò岹���ٶȱ���
DMC_API short __stdcall dmc_conti_change_speed_ratio (WORD CardNo,WORD Crd,double percent);//���ò岹�ж�̬����
//С�߶�ǰհ
DMC_API short __stdcall dmc_conti_set_lookahead_mode(WORD CardNo,WORD Crd,WORD enable,long LookaheadSegments,double PathError,double LookaheadAcc);
DMC_API short __stdcall dmc_conti_get_lookahead_mode(WORD CardNo,WORD Crd,WORD* enable,long* LookaheadSegments,double* PathError,double* LookaheadAcc);

//�����岹IO����
DMC_API short __stdcall dmc_conti_wait_input(WORD CardNo,WORD Crd,WORD bitno,WORD on_off,double TimeOut,long mark);
DMC_API short __stdcall dmc_conti_delay_outbit_to_start(WORD CardNo, WORD Crd, WORD bitno,WORD on_off,double delay_value,WORD delay_mode,double ReverseTime);
DMC_API short __stdcall dmc_conti_delay_outbit_to_stop(WORD CardNo, WORD Crd, WORD bitno,WORD on_off,double delay_time,double ReverseTime);
DMC_API short __stdcall dmc_conti_ahead_outbit_to_stop(WORD CardNo, WORD Crd, WORD bitno,WORD on_off,double ahead_value,WORD ahead_mode,double ReverseTime);
DMC_API short __stdcall dmc_conti_accurate_outbit_unit(WORD CardNo, WORD Crd, WORD cmp_no,WORD on_off,WORD axis,double abs_pos,WORD pos_source,double ReverseTime);
DMC_API short __stdcall dmc_conti_write_outbit(WORD CardNo, WORD Crd, WORD bitno,WORD on_off,double ReverseTime);
DMC_API short __stdcall dmc_conti_clear_io_action(WORD CardNo, WORD Crd, DWORD Io_Mask);
DMC_API short __stdcall dmc_conti_set_pause_output(WORD CardNo,WORD Crd,WORD action,long mask,long state);
DMC_API short __stdcall dmc_conti_get_pause_output(WORD CardNo,WORD Crd,WORD* action,long* mask,long* state);
DMC_API short __stdcall dmc_conti_delay(WORD CardNo, WORD Crd,double delay_time,long mark);//��ʱָ��

DMC_API short __stdcall  dmc_conti_reverse_outbit(WORD CardNo, WORD Crd, WORD bitno,double reverse_time);//IO�����ʱ��ת
DMC_API short __stdcall  dmc_conti_delay_outbit(WORD CardNo, WORD Crd, WORD bitno,WORD on_off,double delay_time);//IO��ʱ���

//�����岹�켣��
DMC_API short __stdcall dmc_conti_line_unit(WORD CardNo,WORD Crd,WORD AxisNum,WORD* AxisList,double* pPosList,WORD posi_mode,long mark);
DMC_API short __stdcall dmc_conti_arc_move_center_unit(WORD CardNo,WORD Crd,WORD AxisNum,WORD* AxisList,double *Target_Pos,double *Cen_Pos,WORD Arc_Dir,long Circle,WORD posi_mode,long mark);
DMC_API short __stdcall dmc_conti_arc_move_radius_unit(WORD CardNo,WORD Crd,WORD AxisNum,WORD* AxisList,double *Target_Pos,double Arc_Radius,WORD Arc_Dir,long Circle,WORD posi_mode,long mark);
DMC_API short __stdcall dmc_conti_arc_move_3points_unit(WORD CardNo,WORD Crd,WORD AxisNum,WORD* AxisList,double *Target_Pos,double *Mid_Pos,long Circle,WORD posi_mode,long mark);
DMC_API short __stdcall dmc_conti_rectangle_move_unit(WORD CardNo,WORD Crd,WORD AxisNum,WORD* AxisList,double* Target_Pos,double* Mark_Pos,long num,WORD rect_mode,WORD posi_mode,long mark);
DMC_API short __stdcall dmc_conti_pmove_unit(WORD CardNo,WORD Crd,WORD axis,double dist,WORD posi_mode,WORD mode,long imark);
DMC_API short __stdcall dmc_conti_set_involute_mode(WORD CardNo,WORD Crd,WORD mode);//���������߲岹�˶�ģʽ
DMC_API short __stdcall dmc_conti_get_involute_mode(WORD CardNo,WORD Crd,WORD* mode);
DMC_API short __stdcall dmc_set_gear_follow_profile(WORD CardNo,WORD axis,WORD enable,WORD master_axis,double ratio);//˫Z��
DMC_API short __stdcall dmc_get_gear_follow_profile(WORD CardNo,WORD axis,WORD* enable,WORD* master_axis,double* ratio);

DMC_API short __stdcall dmc_conti_line_unit_extern(WORD CardNo,WORD Crd,WORD AxisNum,WORD* AxisList,double* Target_Pos,double* Cen_Pos,WORD posi_mode,long mark);
DMC_API short __stdcall dmc_conti_arc_move_center_unit_extern(WORD CardNo,WORD Crd,WORD AxisNum,WORD* AxisList,double* Target_Pos,double* Cen_Pos,double Arc_Radius,WORD posi_mode,long mark);

/*********************************PWM���ܺ���*******************************/
//PWM����
DMC_API short __stdcall dmc_set_pwm_pin(WORD CardNo,WORD portno,WORD ON_OFF, double dfreqency,double dduty);	
DMC_API short __stdcall dmc_get_pwm_pin(WORD CardNo,WORD portno,WORD *ON_OFF, double *dfreqency,double *dduty);
//PWM����
DMC_API short __stdcall dmc_set_pwm_enable(WORD CardNo,WORD enable);
DMC_API short __stdcall dmc_get_pwm_enable(WORD CardNo,WORD* enable);
DMC_API short __stdcall dmc_set_pwm_output(WORD CardNo, WORD PwmNo,double fDuty, double fFre);
DMC_API short __stdcall dmc_get_pwm_output(WORD CardNo,WORD PwmNo,double* fDuty, double* fFre);
DMC_API short __stdcall dmc_conti_set_pwm_output(WORD CardNo,WORD Crd, WORD PwmNo,double fDuty, double fFre);

//����PWM����
DMC_API short __stdcall dmc_set_pwm_enable_extern(WORD CardNo,WORD channel, WORD enable);
DMC_API short __stdcall dmc_get_pwm_enable_extern(WORD CardNo,WORD channel, WORD* enable);

/**********PWM�ٶȸ���**************
mode:����ģʽ0-������ ����״̬ 1-������ ����͵�ƽ2-������ ����ߵ�ƽ3-���� ռ�ձ��Զ�����4-���� Ƶ���Զ�����
MaxVel:��������ٶȣ���λunit
MaxValue:������ռ�ձȻ���Ƶ��
OutValue���������Ƶ�ʻ�ռ�ձ�
*************************************/
DMC_API short __stdcall dmc_conti_set_pwm_follow_speed(WORD CardNo,WORD Crd,WORD pwm_no,WORD mode,double MaxVel,double MaxValue,double OutValue);
DMC_API short __stdcall dmc_conti_get_pwm_follow_speed(WORD CardNo,WORD Crd,WORD pwm_no,WORD* mode,double* MaxVel,double* MaxValue,double* OutValue);
//����PWM���ض�Ӧ��ռ�ձ�
DMC_API short __stdcall dmc_set_pwm_onoff_duty(WORD CardNo, WORD PwmNo,double fOnDuty, double fOffDuty);
DMC_API short __stdcall dmc_get_pwm_onoff_duty(WORD CardNo, WORD PwmNo,double* fOnDuty, double* fOffDuty);
DMC_API short __stdcall dmc_conti_delay_pwm_to_start(WORD CardNo, WORD Crd, WORD pwmno,WORD on_off,double delay_value,WORD delay_mode,double ReverseTime);
DMC_API short __stdcall dmc_conti_delay_pwm_to_stop(WORD CardNo, WORD Crd, WORD pwmno,WORD on_off,double delay_time,double ReverseTime);
DMC_API short __stdcall dmc_conti_ahead_pwm_to_stop(WORD CardNo, WORD Crd, WORD bitno,WORD on_off,double ahead_value,WORD ahead_mode,double ReverseTime);
DMC_API short __stdcall dmc_conti_write_pwm(WORD CardNo, WORD Crd, WORD pwmno,WORD on_off,double ReverseTime);

/*********************ADDA���******************************/
//PWMתDA���
DMC_API short __stdcall dmc_set_da_enable(WORD CardNo,WORD enable);
DMC_API short __stdcall dmc_get_da_enable(WORD CardNo,WORD* enable);
DMC_API short __stdcall dmc_set_da_output(WORD CardNo, WORD channel,double Vout);
DMC_API short __stdcall dmc_get_da_output(WORD CardNo,WORD channel,double* Vout);
//��ȡAD����
DMC_API short __stdcall dmc_get_ad_input(WORD CardNo,WORD channel,double* Vout);
//���������岹DA���
DMC_API short __stdcall dmc_conti_set_da_output(WORD CardNo, WORD Crd, WORD channel,double Vout);
//��������DAʹ��
DMC_API short __stdcall dmc_conti_set_da_enable(WORD CardNo, WORD Crd, WORD enable,WORD channel,long mark);
/**********DA�ٶȸ���**************
da_no:ͨ����
MaxVel:��������ٶȣ���λunit
MaxValue:����ѹ
*************************************/
DMC_API short __stdcall dmc_conti_set_da_follow_speed(WORD CardNo,WORD Crd,WORD da_no,double MaxVel,double MaxValue,double acc_offset,double dec_offset,double acc_dist,double dec_dist);
DMC_API short __stdcall dmc_conti_get_da_follow_speed(WORD CardNo,WORD Crd,WORD da_no,double* MaxVel,double* MaxValue,double* acc_offset,double* dec_offset,double* acc_dist,double* dec_dist);

/******************************CAN IO***********************************/
//baud:0-1M 1-800 2-500 3-250 4-125 5-100
DMC_API short __stdcall dmc_set_can_state(WORD CardNo,WORD NodeNum,WORD state,WORD baud);//0-�Ͽ���1-���ӣ�2-��λ���Զ�����
DMC_API short __stdcall dmc_get_can_state(WORD CardNo,WORD* NodeNum,WORD* state);////0-�Ͽ���1-���ӣ�2-�쳣
DMC_API short __stdcall dmc_get_can_errcode(WORD CardNo,WORD *Errcode);
DMC_API short __stdcall dmc_write_can_outbit(WORD CardNo,WORD Node,WORD bitno,WORD on_off);
DMC_API short __stdcall dmc_read_can_outbit(WORD CardNo,WORD Node,WORD bitno);
DMC_API short __stdcall dmc_read_can_inbit(WORD CardNo,WORD Node,WORD bitno);
DMC_API short __stdcall dmc_write_can_outport(WORD CardNo,WORD Node,WORD PortNo,DWORD outport_val);
DMC_API DWORD __stdcall dmc_read_can_outport(WORD CardNo,WORD Node,WORD PortNo);
DMC_API DWORD __stdcall dmc_read_can_inport(WORD CardNo,WORD Node,WORD PortNo);
//��ȡCANͨѶ����
DMC_API short __stdcall dmc_get_can_errcode_extern(WORD CardNo,WORD *Errcode,WORD *msg_losed, WORD *emg_msg_num, WORD *lostHeartB, WORD *EmgMsg);

DMC_API long __stdcall  dmc_set_profile_limit(WORD CardNo,WORD axis,double Max_Vel,double Max_Acc,double EvenTime);
DMC_API long __stdcall  dmc_get_profile_limit(WORD CardNo,WORD axis,double* Max_Vel,double* Max_Acc,double* EvenTime);
DMC_API long __stdcall  dmc_set_vector_profile_limit(WORD CardNo,WORD Crd,double Max_Vel,double Max_Acc,double EvenTime);
DMC_API long __stdcall  dmc_get_vector_profile_limit(WORD CardNo,WORD Crd,double* Max_Vel,double* Max_Acc,double* EvenTime);
//СԲ����ʹ��
DMC_API short __stdcall dmc_set_arc_limit(WORD CardNo,WORD Crd,WORD Enable,double MaxCenAcc,double MaxArcError);
DMC_API short __stdcall dmc_get_arc_limit(WORD CardNo,WORD Crd,WORD* Enable,double* MaxCenAcc,double* MaxArcError);

//DMC_API short __stdcall dmc_get_axis_debug_state(WORD CardNo,WORD axis,struct_DebugPara* pack);

//�����湦��
//����������������ģʽ0-�������棬1-�������棻�������0-�½��أ�1-�����أ�2-˫���أ��˲�ʱ�䣬��λus
DMC_API short __stdcall dmc_softltc_set_mode(WORD CardNo,WORD latch,WORD ltc_enable,WORD ltc_mode,WORD ltc_inbit,WORD ltc_logic,double filter);
DMC_API short __stdcall dmc_softltc_get_mode(WORD CardNo,WORD latch,WORD *ltc_enable,WORD *ltc_mode,WORD *ltc_inbit,WORD *ltc_logic,double *filter);
//��������Դ��0-ָ��λ�ã�1-����������λ��
DMC_API short __stdcall dmc_softltc_set_source(WORD CardNo,WORD latch,WORD axis,WORD ltc_source);
DMC_API short __stdcall dmc_softltc_get_source(WORD CardNo,WORD latch,WORD axis,WORD *ltc_source);
//��λ������
DMC_API short __stdcall dmc_softltc_reset(WORD CardNo,WORD latch);
//��ȡ�������
DMC_API short __stdcall dmc_softltc_get_number(WORD CardNo,WORD latch,WORD axis,int *number);
//��ȡ����ֵ
DMC_API short __stdcall dmc_softltc_get_value_unit(WORD CardNo,WORD latch,WORD axis,double *value);

DMC_API short __stdcall dmc_set_IoFilter(WORD CardNo,WORD bitno, double filter);
DMC_API short __stdcall dmc_get_IoFilter(WORD CardNo,WORD bitno, double *filter);

//�ݾಹ��
DMC_API short __stdcall dmc_set_lsc_index_value (WORD CardNo, WORD axis,WORD IndexID, long IndexValue);
DMC_API short __stdcall dmc_get_lsc_index_value(WORD CardNo, WORD axis,WORD IndexID, long *IndexValue);

DMC_API short __stdcall dmc_set_lsc_config(WORD CardNo, WORD axis,WORD Origin, DWORD Interal,DWORD NegIndex,DWORD PosIndex,double Ratio);
DMC_API short __stdcall dmc_get_lsc_config(WORD CardNo, WORD axis,WORD *Origin, DWORD *Interal,DWORD *NegIndex,DWORD *PosIndex,double *Ratio);

//���Ź�
DMC_API short __stdcall dmc_set_watchdog(WORD CardNo,WORD enable,DWORD time);
DMC_API short __stdcall dmc_call_watchdog(WORD CardNo);

DMC_API short __stdcall dmc_read_diagnoseData(WORD CardNo);	
DMC_API short __stdcall dmc_conti_set_cmd_end(WORD CardNo,WORD Crd,WORD enable);

//��������λ
DMC_API short __stdcall dmc_set_zone_limit_config(WORD CardNo, WORD *axis, WORD *Source, long x_pos_p, long x_pos_n, long y_pos_p, long y_pos_n, WORD action_para);
DMC_API short __stdcall dmc_get_zone_limit_config(WORD CardNo, WORD* axis, WORD* Source, long* x_pos_p, long* x_pos_n, long* y_pos_p, long* y_pos_n, WORD* action_para);
DMC_API short __stdcall dmc_set_zone_limit_enable(WORD CardNo, WORD enable);

//�ụ������
DMC_API short __stdcall dmc_set_interlock_config(WORD CardNo, WORD* axis, WORD* Source, long delta_pos, WORD action_para);
DMC_API short __stdcall dmc_get_interlock_config(WORD CardNo, WORD* axis, WORD* Source, long* delta_pos, WORD* action_para);
DMC_API short __stdcall dmc_set_interlock_enable(WORD CardNo, WORD enable);

//����ģʽ������
DMC_API short __stdcall dmc_set_grant_error_protect(WORD CardNo, WORD axis,WORD enable,DWORD dstp_error, DWORD emg_error);
DMC_API short __stdcall dmc_get_grant_error_protect(WORD CardNo, WORD axis,WORD* enable,DWORD* dstp_error, DWORD* emg_error);

DMC_API short __stdcall dmc_set_safety_param(WORD CardNo,WORD axis,WORD enable,long safety_pos);	
DMC_API short __stdcall dmc_get_safety_param(WORD CardNo,WORD axis,WORD* enable,long* safety_pos);	
DMC_API short __stdcall dmc_get_diagnose_param(WORD CardNo,WORD axis,long* tartet_pos,int* mode,long* pulse_pos,long* endcoder_pos);

//����ּ���
DMC_API short __stdcall dmc_set_camerablow_config(WORD CardNo,WORD camerablow_en,long cameraPos,WORD piece_num,long piece_distance,WORD axis_sel,long latch_distance_min);	
DMC_API short __stdcall dmc_get_camerablow_config(WORD CardNo,WORD* camerablow_en,long* cameraPos,WORD* piece_num,long* piece_distance,WORD* axis_sel,long* latch_distance_min);	
DMC_API short __stdcall dmc_clear_camerablow_errorcode(WORD CardNo);	
DMC_API short __stdcall dmc_get_camerablow_errorcode(WORD CardNo,WORD* errorcode);

//����ͨ�����루0~15����Ϊ�����λ�ź�
DMC_API short __stdcall dmc_set_io_limit_config(WORD CardNo,WORD portno,WORD enable,WORD axis_sel,WORD el_mode,WORD el_logic);	
DMC_API short __stdcall dmc_get_io_limit_config(WORD CardNo,WORD portno,WORD* enable,WORD* axis_sel,WORD* el_mode,WORD* el_logic);	

//�����˲�����
DMC_API short __stdcall dmc_set_handwheel_filter(WORD CardNo,WORD axis,double filter_factor);
DMC_API short __stdcall dmc_get_handwheel_filter(WORD CardNo,WORD axis,double* filter_factor);

//��ȡ����ϵ����ĵ�ǰ�滮����
DMC_API short __stdcall dmc_conti_get_interp_map(WORD CardNo,WORD Crd,WORD* AxisNum ,WORD* AxisList,double *pPosList);
//����ϵ������� 
DMC_API short __stdcall dmc_conti_get_crd_errcode(WORD CardNo,WORD Crd,WORD* errcode);


DMC_API short __stdcall dmc_line_unit_follow(WORD CardNo,WORD Crd,WORD AxisNum,WORD* AxisList,double* Dist,WORD posi_mode);
DMC_API short __stdcall dmc_conti_line_unit_follow(WORD CardNo,WORD Crd,WORD AxisNum,WORD* AxisList,double* pPosList,WORD posi_mode,long mark);

//�����岹������DA����  
DMC_API short __stdcall dmc_conti_set_da_action(WORD CardNo,WORD Crd,WORD mode,WORD portno,double dvalue);

//���������ٶ�
DMC_API short __stdcall dmc_read_encoder_speed(WORD CardNo,WORD Axis,double *current_speed);

DMC_API short __stdcall dmc_axis_follow_line_enable(WORD CardNo,WORD Crd,WORD enable_flag);

//�岹�����岹��
DMC_API short __stdcall dmc_set_interp_compensation(WORD CardNo,WORD axis, double dvalue,double time);
DMC_API short __stdcall dmc_get_interp_compensation(WORD CardNo,WORD axis, double *dvalue,double *time); 

//IO��ȷֹͣ
DMC_API short __stdcall dmc_set_io_exactstop(WORD CardNo,WORD axis, WORD ioNum,WORD *ioList,WORD enable,WORD valid_logic,WORD action,WORD move_dir); 

//��ȡ��������ľ���
DMC_API short __stdcall dmc_get_distance_to_start(WORD CardNo,WORD Crd, double* distance_x, double* distance_y,long imark); 
//���ñ�־λ ��ʾ�Ƿ�ʼ����������
DMC_API short __stdcall dmc_set_start_distance_flag(WORD CardNo,WORD Crd,WORD flag);

/******************���û�����λ����**********************
	��  ����
	CardNo:����
	Axis�����
	N_limit:����λ������
	P_limit:����λ������
	����ֵ���������
*******************************************************************/
DMC_API short __stdcall dmc_set_home_soft_limit(WORD CardNo,WORD Axis,int N_limit,int P_limit); 
DMC_API short __stdcall dmc_get_home_soft_limit(WORD CardNo,WORD Axis,int* N_limit,int* P_limit); 

/*********************
	ָ��˵����ʵ�ֵ�����湦�ܣ�����ĳ��������˶������ô�ָ��ʱ��������axis������Ŵ�ָ����滺�����Ĳ岹ָ��Ϲ켣�˶���ֱ�����õ�����������ϡ�
	ָ�����ͣ��岹������ָ��
	���������CardNo ����ϵ��
	axis ������
	dist �������(���嵱����λ)
        follow_mode //�˶�ģʽ��0-��ͣ��������1-�˶�������
	imark �κ�
	�����������
*******************************/
DMC_API short __stdcall dmc_conti_gear_unit(WORD CardNo,WORD Crd,WORD axis,double dist, WORD follow_mode,long imark); 

//�켣���ʹ������
DMC_API short __stdcall dmc_set_path_fitting_enable(WORD CardNo,WORD Crd,WORD enable);
//�ݾಹ������(��)
DMC_API short __stdcall dmc_enable_leadscrew_comp(WORD CardNo, WORD axis,WORD enable);
DMC_API short __stdcall dmc_set_leadscrew_comp_config(WORD CardNo, WORD axis,WORD n, int startpos,int lenpos,int *pCompPos,int *pCompNeg);
//ָ����������λ���˶� ���̶������˶�
DMC_API short __stdcall dmc_t_pmove_extern(WORD CardNo, WORD axis, double MidPos,double TargetPos, double Min_Vel,double Max_Vel, double stop_Vel, double acc,double dec,WORD posi_mode);

/*
���ܣ������������ֵ�ͱ���������ֵ֮���ֵ�ı�����ֵ
���������CardNo ����
axis ���
error ��ֵ����������ֵ
�����������
*/
DMC_API short __stdcall dmc_set_pulse_encoder_count_error(WORD CardNo,WORD axis,WORD error);
DMC_API short __stdcall dmc_get_pulse_encoder_count_error(WORD CardNo,WORD axis,WORD *error);
/*
���ܣ�����������ֵ�ͱ���������ֵ֮���ֵ�Ƿ񳬹�������ֵ
	���������CardNo ����
	axis ���
	�������;��
	���ز�����0����ֵС�ڱ�����ֵ
	1����ֵ���ڵ��ڱ�����ֵ
*/
DMC_API short __stdcall dmc_check_pulse_encoder_count_error(WORD CardNo,WORD axis,int* pulse_position, int* enc_position);
/*
ǿ�б�λ��չ
mid_pos: �м�λ��
aim_pos:Ŀ��λ��
posi_mode: ����������Ĭ��Ϊ����ֵ
*/
DMC_API short __stdcall dmc_update_target_position_extern(WORD CardNo, WORD axis, double mid_pos, double aim_pos, double vel,WORD posi_mode);

//������ּ���
//�̶�
DMC_API short __stdcall dmc_sorting_close(WORD CardNo);
DMC_API short __stdcall dmc_sorting_start(WORD CardNo);
DMC_API short __stdcall dmc_sorting_set_init_config(WORD CardNo ,WORD cameraCount, int *pCameraPos, WORD *pCamIONo, DWORD cameraTime, WORD cameraTrigLevel, WORD blowCount, int*pBlowPos, WORD*pBlowIONo, DWORD blowTime, WORD blowTrigLevel, WORD axis, WORD dir, WORD checkMode);
DMC_API short __stdcall dmc_sorting_set_camera_trig_count(WORD CardNo ,WORD cameraNum, DWORD cameraTrigCnt);
DMC_API short __stdcall dmc_sorting_get_camera_trig_count(WORD CardNo ,WORD cameraNum, DWORD* pCameraTrigCnt, WORD count);
DMC_API short __stdcall dmc_sorting_set_blow_trig_count(WORD CardNo ,WORD blowNum, DWORD blowTrigCnt);
DMC_API short __stdcall dmc_sorting_get_blow_trig_count(WORD CardNo ,WORD blowNum, DWORD* pBlowTrigCnt, WORD count);
DMC_API short __stdcall dmc_sorting_get_camera_config(WORD CardNo ,WORD index,int* pos,DWORD* trigTime, WORD* ioNo, WORD* trigLevel);
DMC_API short __stdcall dmc_sorting_get_blow_config(WORD CardNo ,WORD index, int* pos,DWORD* trigTime, WORD* ioNo, WORD* trigLevel);
DMC_API short __stdcall dmc_sorting_get_blow_status(WORD CardNo ,DWORD* trigCntAll, WORD* trigMore,WORD* trigLess);
DMC_API short __stdcall dmc_sorting_trig_blow(WORD CardNo ,WORD blowNum);
DMC_API short __stdcall dmc_sorting_set_blow_enable(WORD CardNo ,WORD blowNum,WORD enable);
DMC_API short __stdcall dmc_sorting_set_piece_config(WORD CardNo ,DWORD maxWidth,DWORD minWidth,DWORD minDistance, DWORD minTimeIntervel);
DMC_API short __stdcall dmc_sorting_get_piece_status(WORD CardNo ,DWORD* pieceFind,DWORD* piecePassCam, DWORD* dist2next, DWORD*pieceWidth);
DMC_API short __stdcall dmc_sorting_set_cam_trig_phase(WORD CardNo,WORD blowNo,double coef);
DMC_API short __stdcall dmc_sorting_set_blow_trig_phase(WORD CardNo,WORD blowNo,double coef);

DMC_API short __stdcall dmc_set_sevon_enable(WORD CardNo,WORD axis,WORD on_off);
DMC_API short __stdcall dmc_get_sevon_enable(WORD CardNo,WORD axis);
DMC_API short __stdcall dmc_compare_add_point_cycle(WORD CardNo,WORD cmp,long pos,WORD dir, DWORD bitno,DWORD cycle,WORD level);//��ӱȽϵ�

//ʹ�ܺ����ø��ٱ��������ڷ�Χ��ʱ���ֹͣģʽ
DMC_API short __stdcall dmc_set_encoder_count_error_action_config(WORD CardNo,WORD enable,WORD stopmode);
DMC_API short __stdcall dmc_get_encoder_count_error_action_config(WORD CardNo,WORD* enable,WORD* stopmode);

DMC_API short __stdcall dmc_set_home_el_return(WORD CardNo,WORD axis,WORD enable);

//����������da����
DMC_API short __stdcall dmc_conti_set_encoder_da_follow_enable(WORD CardNo, WORD Crd,WORD axis,WORD enable);	
DMC_API short __stdcall dmc_conti_get_encoder_da_follow_enable(WORD CardNo, WORD Crd,WORD* axis,WORD* enable);

DMC_API short __stdcall dmc_check_done_pos(WORD CardNo,WORD axis,WORD posi_mode);
DMC_API short __stdcall dmc_set_factor_error(WORD CardNo,WORD axis,double factor,long error);
DMC_API short __stdcall dmc_set_factor(WORD CardNo,WORD axis,double factor);
DMC_API short __stdcall dmc_set_error(WORD CardNo,WORD axis,long error);
DMC_API short __stdcall dmc_get_factor_error(WORD CardNo,WORD axis,double* factor,long* error);
DMC_API short __stdcall dmc_check_success_pulse(WORD CardNo,WORD axis);
DMC_API short __stdcall dmc_check_success_encoder(WORD CardNo,WORD axis);

//IO����������������
DMC_API short __stdcall dmc_set_io_count_profile(WORD CardNo, WORD chan, WORD bitno,WORD mode,double filter, double count_value, WORD* axis_list, WORD axis_num, WORD stop_mode );
DMC_API short __stdcall dmc_get_io_count_profile(WORD CardNo, WORD chan, WORD* bitno,WORD* mode,double* filter, double* count_value, WORD* axis_list, WORD* axis_num, WORD* stop_mode );
DMC_API short __stdcall dmc_set_io_count_enable(WORD CardNo, WORD chan, WORD ifenable);
DMC_API short __stdcall dmc_clear_io_count(WORD CardNo, WORD chan);
DMC_API short __stdcall dmc_get_io_count_value_extern(WORD CardNo, WORD chan, long* current_value);

//�ݾಹ��ǰ������λ�ã�������λ��//20191025
DMC_API short __stdcall dmc_get_position_ex(WORD CardNo,WORD axis, double * pos);
DMC_API short __stdcall dmc_get_encoder_ex(WORD CardNo,WORD axis, double * pos);
//�ݾಹ��ǰ������λ�ã�������λ�� ����
DMC_API short __stdcall dmc_get_position_ex_unit(WORD CardNo,WORD axis, double * pos);
DMC_API short __stdcall dmc_get_encoder_ex_unit(WORD CardNo,WORD axis, double * pos);

//����ƫ��ģʽ����
DMC_API short __stdcall dmc_set_home_shift_param(WORD CardNo, WORD axis, WORD pos_clear_mode, double ShiftValue);
DMC_API short __stdcall dmc_get_home_shift_param(WORD CardNo, WORD axis, WORD *pos_clear_mode, double* ShiftValue);

DMC_API short __stdcall dmc_change_speed_extend(WORD CardNo,WORD axis,double Curr_Vel, double Taccdec, WORD pin_num, WORD trig_mode);

DMC_API short __stdcall dmc_follow_vector_speed_move(WORD CardNo,WORD axis,WORD Follow_AxisNum,WORD* Follow_AxisList,double ratio);
DMC_API short __stdcall dmc_conti_line_unit_extend(WORD CardNo, WORD Crd, WORD AxisNum, WORD* AxisList, double* pPosList, WORD posi_mode, double Extend_Len, WORD enable,long mark); //�����岹ֱ��
DMC_API short __stdcall dmc_hcmp_2d_set_config_unit(WORD CardNo,WORD hcmp,WORD cmp_mode,WORD x_axis, WORD x_cmp_source, double x_cmp_error, WORD y_axis, WORD y_cmp_source, double y_cmp_error,WORD cmp_logic,int time);
DMC_API short __stdcall dmc_hcmp_2d_get_config_unit(WORD CardNo,WORD hcmp,WORD *cmp_mode,WORD *x_axis, WORD *x_cmp_source,double *x_cmp_error,  WORD *y_axis, WORD *y_cmp_source, double *y_cmp_error, WORD *cmp_logic,int *time);

DMC_API short __stdcall dmc_hcmp_2d_set_pwmoutput(WORD CardNo,WORD hcmp,WORD pwm_enable,double duty,double freq,WORD pwm_number);
DMC_API short __stdcall dmc_hcmp_2d_get_pwmoutput(WORD CardNo,WORD hcmp,WORD *pwm_enable,double *duty,double *freq,WORD *pwm_number);
DMC_API short __stdcall dmc_hcmp_2d_add_point_unit(WORD ConnectNo,WORD hcmp, double x_cmp_pos, double y_cmp_pos,WORD cmp_outbit);
DMC_API short __stdcall dmc_hcmp_2d_get_current_state_unit(WORD CardNo,WORD hcmp,int *remained_points,double *x_current_point,double *y_current_point,int *runned_points,WORD *current_state,WORD *current_outbit); 

DMC_API short __stdcall dmc_set_home_position(WORD CardNo,WORD axis,WORD enable,double position);
DMC_API short __stdcall dmc_get_home_position(WORD CardNo,WORD axis,WORD *enable,double *position);
DMC_API short __stdcall dmc_conti_line_io_union(WORD CardNo,WORD Crd,WORD AxisNum,WORD* AxisList,double* pPosList,WORD posi_mode,WORD bitno,WORD on_off,double io_value,WORD io_mode,WORD MapAxis,WORD pos_source,double ReverseTime,long mark);
//���ñ���������
DMC_API short __stdcall dmc_set_encoder_dir(WORD CardNo, WORD axis,WORD dir);

//Բ����������λ
DMC_API short __stdcall dmc_set_arc_zone_limit_config(WORD CardNo, WORD* AxisList, WORD AxisNum, double *Center, double Radius, WORD Source,WORD StopMode);
DMC_API short __stdcall dmc_get_arc_zone_limit_config(WORD CardNo, WORD* AxisList, WORD* AxisNum, double *Center, double* Radius, WORD* Source,WORD* StopMode);
DMC_API short __stdcall dmc_get_arc_zone_limit_axis_status(WORD CardNo, WORD axis);
DMC_API short __stdcall dmc_set_arc_zone_limit_enable(WORD CardNo, WORD enable);
DMC_API short __stdcall dmc_get_arc_zone_limit_enable(WORD CardNo, WORD* enable);

//1��	���û��淽ʽ��ӱȽ�λ�ã�
DMC_API short __stdcall dmc_hcmp_fifo_set_mode(WORD CardNo,WORD hcmp, WORD fifo_mode);
DMC_API short __stdcall dmc_hcmp_fifo_get_mode(WORD CardNo,WORD hcmp, WORD* fifo_mode);
//2��	��ȡʣ�໺��״̬����λ��ͨ���˺����ж��Ƿ������ӱȽ�λ��
DMC_API short __stdcall dmc_hcmp_fifo_get_state(WORD CardNo,WORD hcmp,long *remained_points); 
//3��	������ķ�ʽ������ӱȽ�λ��
DMC_API short __stdcall dmc_hcmp_fifo_add_point_unit(WORD CardNo,WORD hcmp, WORD num,double *cmp_pos);
//4��	����Ƚ�λ��,Ҳ���FPGA��λ��ͬ�������
DMC_API short __stdcall dmc_hcmp_fifo_clear_points(WORD CardNo,WORD hcmp);
//��Ӵ����ݣ������һ��ʱ�䣬ָ������������
DMC_API short __stdcall dmc_hcmp_fifo_add_table(WORD CardNo,WORD hcmp, WORD num,double *cmp_pos);

//��ά����λ�ñȽϻ���
//1��	���û��淽ʽ��ӱȽ�λ�ã�
DMC_API short __stdcall dmc_hcmp_2d_fifo_set_mode(WORD CardNo,WORD hcmp, WORD fifo_mode);
DMC_API short __stdcall dmc_hcmp_2d_fifo_get_mode(WORD CardNo,WORD hcmp, WORD* fifo_mode);
//2��	��ȡʣ�໺��״̬����λ��ͨ���˺����ж��Ƿ������ӱȽ�λ��
DMC_API short __stdcall dmc_hcmp_2d_fifo_get_state(WORD CardNo,WORD hcmp,long *remained_points); 
//3��	������ķ�ʽ������ӱȽ�λ��
DMC_API short __stdcall dmc_hcmp_2d_fifo_add_point_unit(WORD CardNo,WORD hcmp, WORD num,double *x_cmp_pos,double *y_cmp_pos,WORD *cmp_outbit);
//4��	����Ƚ�λ��,Ҳ���FPGA��λ��ͬ�������
DMC_API short __stdcall dmc_hcmp_2d_fifo_clear_points(WORD CardNo,WORD hcmp);
//��Ӵ����ݣ������һ��ʱ�䣬ָ������������
DMC_API short __stdcall dmc_hcmp_2d_fifo_add_table(WORD CardNo,WORD hcmp, WORD num,double *x_cmp_pos,double *y_cmp_pos);

//���ƿ����ߺж��ߺ��Ƿ��ʼ�������ƽ
DMC_API short __stdcall dmc_set_output_status_repower(WORD CardNo, WORD enable);

DMC_API short __stdcall dmc_t_pmove_extern_softlanding(WORD CardNo, WORD axis, double MidPos, double TargetPos, double start_Vel, double Max_Vel, double stop_Vel, DWORD delay_ms, double Max_Vel2,double stop_vel2, double acc_time, double dec_time, WORD posi_mode);
DMC_API short __stdcall dmc_compare_add_point_XD(WORD CardNo,WORD cmp,long pos,WORD dir, WORD action,DWORD actpara, long startPos);//���綨�ƱȽϺ���

DMC_API short __stdcall dmc_pmove_change_pos_speed_config(WORD CardNo,WORD axis,double tar_vel, double tar_rel_pos, WORD trig_mode, WORD source);
DMC_API short __stdcall dmc_get_pmove_change_pos_speed_config(WORD CardNo,WORD axis,double* tar_vel, double* tar_rel_pos, WORD* trig_mode, WORD* source);
DMC_API short __stdcall dmc_pmove_change_pos_speed_enable(WORD CardNo,WORD axis, WORD enable);
DMC_API short __stdcall dmc_get_pmove_change_pos_speed_enable(WORD CardNo,WORD axis, WORD* enable);
DMC_API short __stdcall dmc_compare_add_point_extend(WORD CardNo,WORD axis, long pos, WORD dir, WORD action, WORD para_num, DWORD* actpara,DWORD compare_time);
DMC_API short __stdcall dmc_get_cmd_position(WORD CardNo,WORD axis, double * pos);
//�߼���������
DMC_API short __stdcall dmc_set_logic_analyzer_config(WORD CardNo,WORD channel, DWORD SampleFre, DWORD SampleDepth, WORD SampleMode);
DMC_API short __stdcall dmc_start_logic_analyzer(WORD CardNo,WORD channel, WORD enable);
DMC_API short __stdcall dmc_get_logic_analyzer_counter(WORD CardNo, WORD channel, DWORD *counter);
//������	 20190923�޸Ŀ��趨�ƺ����ӿ�
DMC_API short __stdcall dmc_read_inbit_append(WORD CardNo,WORD bitno);//��ȡ����ڵ�״̬
DMC_API short __stdcall dmc_write_outbit_append(WORD CardNo,WORD bitno,WORD on_off);//��������ڵ�״̬
DMC_API short __stdcall dmc_read_outbit_append(WORD CardNo,WORD bitno);//��ȡ����ڵ�״̬
DMC_API DWORD __stdcall dmc_read_inport_append(WORD CardNo,WORD portno);//��ȡ����˿ڵ�ֵ
DMC_API DWORD __stdcall dmc_read_outport_append(WORD CardNo,WORD portno);//��ȡ����˿ڵ�ֵ
DMC_API short __stdcall dmc_write_outport_append(WORD CardNo,WORD portno,DWORD port_value);//������������˿ڵ�ֵ
//���˶�
DMC_API short __stdcall	dmc_m_move_unit(WORD CardNo,WORD Crd, WORD axis_num, WORD* axis_list,double* mid_pos, double* target_pos, double* saftpos, WORD pos_mode);
DMC_API short __stdcall	dmc_get_m_move_config(WORD CardNo,WORD Crd, WORD *axis_num, WORD* axis_list,double* mid_pos, double* target_pos, double* saftpos,WORD* pos_mode);
// ��������ϵ�������
DMC_API short __stdcall dmc_set_tangent_follow(WORD CardNo, WORD Crd, WORD axis, WORD follow_curve, WORD rotate_dir, double degree_equivalent);
// ��ȡָ������ϵ����������
DMC_API	short __stdcall dmc_get_tangent_follow_param(WORD CardNo, WORD Crd, WORD* axis, WORD* follow_curve, WORD* rotate_dir, double* degree_equivalent);
// ȡ������ϵ����
DMC_API short __stdcall dmc_disable_follow_move(WORD CardNo, WORD Crd);
// ��Բ�岹
DMC_API short __stdcall dmc_ellipse_move(WORD CardNo, WORD Crd,WORD axisNum, WORD* Axis_List, double* Target_Pos, double* Cen_Pos, double A_Axis_Len, double B_Axis_Len, WORD Dir, WORD Pos_Mode);
DMC_API short __stdcall dmc_read_vector_speed_unit(WORD CardNo,WORD Crd,double *current_speed);	//��ȡ��ǰ���Ĳ岹�ٶ�
//��ȡ��������λ����ʹ��
DMC_API short __stdcall dmc_get_home_el_return(WORD CardNo,WORD axis,WORD *enable);

//�¿��Ź�����
DMC_API short __stdcall dmc_set_watchdog_action_event(WORD CardNo, WORD event_mask);
DMC_API short __stdcall dmc_get_watchdog_action_event(WORD CardNo, WORD* event_mask);
DMC_API short __stdcall dmc_set_watchdog_enable (WORD CardNo, double timer_period, WORD enable);
DMC_API short __stdcall dmc_get_watchdog_enable (WORD CardNo, double * timer_period, WORD* enable);
DMC_API short __stdcall dmc_reset_watchdog_timer (WORD CardNo);
//io���ƹ���
DMC_API short __stdcall dmc_set_io_check_control(WORD CardNo, WORD sensor_in_no, WORD check_mode, WORD A_out_no, WORD B_out_no, WORD C_out_no, WORD output_mode);
DMC_API short __stdcall dmc_get_io_check_control(WORD CardNo, WORD* sensor_in_no, WORD* check_mode, WORD* A_out_no, WORD* B_out_no, WORD* C_out_no, WORD* output_mode);
DMC_API short __stdcall dmc_stop_io_check_control(WORD CardNo);

//������λ����ƫ�ƾ���
DMC_API short __stdcall dmc_set_el_ret_deviation(WORD CardNo, WORD axis, WORD enable,double deviation);
DMC_API short __stdcall  dmc_get_el_ret_deviation(WORD CardNo, WORD axis, WORD* enable, double* deviation);

/*****************************������غ���**********************************/
//�����˶�
DMC_API short __stdcall nmc_set_home_profile(WORD CardNo ,WORD axis,WORD home_mode,double Low_Vel, double High_Vel,double Tacc,double Tdec ,double offsetpos);//���û���������ϲ�����
DMC_API short __stdcall nmc_get_home_profile(WORD CardNo ,WORD axis,WORD* home_mode,double* Low_Vel, double* High_Vel,double* Tacc,double* Tdec ,double* offsetpos);
DMC_API short __stdcall nmc_home_move(WORD CardNo,WORD axis);

//-------------------------��������-----------------------
/*******************************************************
portnum��ʾ�˿ںţ���������
0: ��ʾcanopen��0�Ŷ˿�
1: ��ʾcanopen��1�Ŷ˿�
10:��ʾEtherCAT��0�Ŷ˿�
11:��ʾEtherCAT��1�Ŷ˿�
********************************************************/
DMC_API short __stdcall nmc_set_manager_para(WORD CardNo,WORD PortNum,DWORD Baudrate,WORD ManagerID);
DMC_API short __stdcall nmc_get_manager_para(WORD CardNo,WORD PortNum,DWORD *Baudrate,WORD *ManagerID);
DMC_API short __stdcall nmc_set_manager_od(WORD CardNo,WORD PortNum, WORD Index,WORD SubIndex,WORD ValLength,DWORD Value);
DMC_API short __stdcall nmc_get_manager_od(WORD CardNo,WORD PortNum, WORD Index,WORD SubIndex,WORD ValLength,DWORD *Value);

DMC_API short __stdcall nmc_set_node_od(WORD CardNo,WORD PortNum,WORD NodeNum, WORD Index,WORD SubIndex,WORD ValLength,long Value);
DMC_API short __stdcall nmc_get_node_od(WORD CardNo,WORD PortNum,WORD NodeNum, WORD Index,WORD SubIndex,WORD ValLength,long* Value);

DMC_API short __stdcall nmc_upload_configfile(WORD CardNo,WORD PortNum, const char *FileName);
DMC_API short __stdcall nmc_reset_to_factory(WORD CardNo,WORD PortNum,WORD NodeNum);
DMC_API short __stdcall nmc_write_to_pci(WORD CardNo,WORD PortNum,WORD NodeNum);
DMC_API short __stdcall nmc_download_configfile(WORD CardNo,WORD PortNum,const char *FileName);//����ENI�����ļ�
DMC_API short __stdcall nmc_download_mapfile(WORD CardNo,const char *FileName);//����ӳ���ļ�

//��ӵ���ʹ�ܺ��� 255��ʾȫʹ��
DMC_API short __stdcall nmc_set_axis_enable(WORD CardNo,WORD axis);
DMC_API short __stdcall nmc_set_axis_disable(WORD CardNo,WORD axis);

//��������ź�
DMC_API short __stdcall nmc_set_alarm_clear(WORD CardNo,WORD PortNum,WORD NodeNum);

DMC_API short __stdcall nmc_get_slave_nodes(WORD CardNo,WORD PortNum,WORD BaudRate,WORD* NodeId,WORD* NodeNum);
//��ȡ��������
DMC_API short __stdcall nmc_get_total_axes(WORD CardNo,DWORD* TotalAxis);
//��ȡ����ADDA�����������
DMC_API short __stdcall nmc_get_total_adcnum(WORD CardNo,WORD* TotalIn,WORD* TotalOut);
//��ȡ����IO����
DMC_API short __stdcall nmc_get_total_ionum(WORD CardNo,WORD *TotalIn,WORD *TotalOut);
//����˿ڱ���
DMC_API short __stdcall nmc_clear_alarm_fieldbus(WORD CardNo,WORD PortNum);
//��ȡ����������ģʽ  1��ʾethercatģʽ��0��ʾ����ģʽ
DMC_API short __stdcall nmc_get_controller_workmode(WORD CardNo,WORD* controller_mode);
//���ÿ���������ģʽ  1��ʾethercatģʽ��0��ʾ����ģʽ
DMC_API short __stdcall nmc_set_controller_workmode(WORD CardNo,WORD controller_mode);
//����ethercat����ѭ������(us)
DMC_API short __stdcall nmc_set_cycletime(WORD CardNo,WORD PortNum,DWORD CycleTime);
//��ȡethercat����ѭ������(us)
DMC_API short __stdcall nmc_get_cycletime(WORD CardNo,WORD PortNum,DWORD* CycleTime);

//��ȡ�����߳�ʱ�����Ĳ���
DMC_API short __stdcall dmc_get_perline_time(WORD CardNo,WORD TypeIndex,DWORD *Averagetime,DWORD *Maxtime,uint64 *Cycles ); //TypeIndex:0~6  m_Averagetime ; ƽ��ʱ�� m_Maxtime;���ʱ�� uint64  m_Cycles;��ǰʱ��
DMC_API short __stdcall nmc_set_axis_run_mode(WORD CardNo,WORD axis,WORD run_mode);//�����������ģʽ 1Ϊppģʽ��6Ϊ����ģʽ��8Ϊcspģʽ


//��ȡ������
/*
enum fieldbus_type
{
	virtual_type=0,
	ect_type=1,
	can_type=2,
	pulse_type=3, // or local IO 
	unknown=4
};*/
DMC_API short __stdcall nmc_get_axis_type(WORD CardNo,WORD axis, WORD* Axis_Type);
//��ȡ����ʱ������ƽ��ʱ�䣬���ʱ�䣬ִ��������
DMC_API short __stdcall nmc_get_consume_time_fieldbus(WORD CardNo,WORD PortNum,DWORD* Average_time, DWORD* Max_time,uint64* Cycles);
//���ʱ����
DMC_API short __stdcall nmc_clear_consume_time_fieldbus(WORD CardNo,WORD PortNum);
//ֹͣethercat����,����0��ʾ�ɹ�������������ʾ���ɹ�
DMC_API short __stdcall nmc_stop_etc(WORD CardNo,WORD* ETCState);

//---------------------------������ģʽ---------------
//��ȡ��״̬��
DMC_API short __stdcall nmc_get_axis_statusword(WORD CardNo,WORD axis,long* statusword);
//��ȡ�����������
DMC_API short __stdcall nmc_set_axis_contrlword(WORD CardNo,WORD Axis,long Contrlword);
//���������������
DMC_API short __stdcall nmc_get_axis_contrlword(WORD CardNo,WORD Axis,long *Contrlword);
DMC_API short __stdcall nmc_set_axis_contrlmode(WORD CardNo,WORD Axis,long Contrlmode);
DMC_API short __stdcall nmc_get_axis_contrlmode(WORD CardNo,WORD Axis,long *Contrlmode);

// ��ȡ���߶˿ڴ�����
DMC_API short __stdcall nmc_get_errcode(WORD CardNo,WORD channel,WORD *Errcode);
// ��ȡ���ƿ�������
DMC_API short __stdcall nmc_get_card_errcode(WORD CardNo,WORD *Errcode);
// ��ȡ�����������
DMC_API short __stdcall nmc_get_axis_errcode(WORD CardNo,WORD axis,WORD *Errcode);
// ������߶˿ڴ�����
DMC_API short __stdcall nmc_clear_errcode(WORD CardNo,WORD channel);
// ������ƿ�������
DMC_API short __stdcall nmc_clear_card_errcode(WORD CardNo);
// ��������������
DMC_API short __stdcall nmc_clear_axis_errcode(WORD CardNo,WORD iaxis);

DMC_API short __stdcall nmc_get_LostHeartbeat_Nodes(WORD CardNo,WORD PortNum,WORD* NodeID,WORD* NodeNum);
DMC_API short __stdcall nmc_get_EmergeneyMessege_Nodes(WORD CardNo,WORD PortNum,DWORD* NodeMsg,WORD* MsgNum);
DMC_API short __stdcall nmc_SendNmtCommand(WORD CardNo,WORD PortNum,WORD NodeID,WORD NmtCommand);
DMC_API short __stdcall nmc_syn_move(WORD CardNo,WORD AxisNum,WORD* AxisList,long* Position,WORD* PosiMode);
DMC_API short __stdcall nmc_syn_move_unit(WORD CardNo,WORD AxisNum,WORD* AxisList,double* Position,WORD* PosiMode);
//���߶���ͬ���˶�
DMC_API short __stdcall nmc_sync_pmove_unit(WORD CardNo,WORD AxisNum,WORD* AxisList,double* Dist,WORD* PosiMode);
DMC_API short __stdcall nmc_sync_vmove_unit(WORD CardNo,WORD AxisNum,WORD* AxisList,WORD* Dir);

//������վ����
DMC_API short __stdcall nmc_set_master_para(WORD CardNo,WORD PortNum,WORD Baudrate,DWORD NodeCnt,WORD MasterId);
//��ȡ��վ����
DMC_API short __stdcall nmc_get_master_para(WORD CardNo,WORD PortNum,WORD *Baudrate,DWORD *NodeCnt,WORD *MasterId);
//����io���
DMC_API short __stdcall nmc_write_outbit(WORD CardNo,WORD NoteID,WORD IoBit,WORD IoValue);
//��ȡio���
DMC_API short __stdcall nmc_read_outbit(WORD CardNo,WORD NoteID,WORD IoBit,WORD *IoValue);
//��ȡio����
DMC_API short __stdcall nmc_read_inbit(WORD CardNo,WORD NoteID,WORD IoBit,WORD *IoValue);
//����DA����	
DMC_API short __stdcall nmc_set_da_output(WORD CardNo,WORD NoteID,WORD channel,double Value);
//��ȡDA����
DMC_API short __stdcall nmc_get_da_output(WORD CardNo,WORD NoteID,WORD channel,double *Value);
//��ȡAD����
DMC_API short __stdcall nmc_get_ad_input(WORD CardNo,WORD NoteID,WORD channel,double *Value);
//����ADģʽ
DMC_API short __stdcall nmc_set_ad_mode(WORD CardNo,WORD NoteID,WORD channel,WORD mode,DWORD buffer_nums);
DMC_API short __stdcall nmc_get_ad_mode(WORD CardNo,WORD NoteID,WORD channel,WORD* mode,DWORD buffer_nums);
//����DAģʽ
DMC_API short __stdcall nmc_set_da_mode(WORD CardNo,WORD NoteID,WORD channel,WORD mode,DWORD buffer_nums);
DMC_API short __stdcall nmc_get_da_mode(WORD CardNo,WORD NoteID,WORD channel,WORD* mode,DWORD buffer_nums);

//����д��flash
DMC_API short __stdcall nmc_write_to_flash(WORD CardNo,WORD PortNum,WORD NodeNum);

//��������
DMC_API short __stdcall nmc_set_connect_state(WORD CardNo,WORD NodeNum,WORD state,WORD baud);//0-�Ͽ���1-���ӣ�2-��λ���Զ�����
DMC_API short __stdcall nmc_get_connect_state(WORD CardNo,WORD* NodeNum,WORD* state);//0-�Ͽ���1-���ӣ�2-�쳣

//����io���32λ
DMC_API short __stdcall nmc_write_outport(WORD CardNo,WORD NoteID,WORD portno,DWORD outport_val);
//��ȡio���32λ
DMC_API short __stdcall nmc_read_outport(WORD CardNo,WORD NoteID,WORD portno,DWORD *outport_val);
//��ȡio����32λ
DMC_API short __stdcall nmc_read_inport(WORD CardNo,WORD NoteID,WORD portno,DWORD *inport_val);

//��״̬��
DMC_API short __stdcall nmc_get_axis_state_machine(WORD CardNo,WORD axis, WORD* Axis_StateMachine);
//��ȡ�����ÿ���ģʽ������ֵ��6����ģʽ��8cspģʽ��
DMC_API short __stdcall nmc_get_axis_setting_contrlmode(WORD CardNo,WORD axis,long* contrlmode);
// ��ȡ��վ����
DMC_API short __stdcall nmc_get_total_slaves(WORD CardNo,WORD PortNum,WORD* TotalSlaves);
// ��ȡ��Ĵ�վ��Ϣ
DMC_API short __stdcall nmc_get_axis_node_address(WORD CardNo,WORD axis, WORD* SlaveAddr,WORD* Sub_SlaveAddr);
DMC_API short __stdcall nmc_set_axis_io_out(WORD CardNo,WORD axis,DWORD  iostate);
DMC_API DWORD __stdcall nmc_get_axis_io_out(WORD CardNo,WORD axis);
DMC_API DWORD __stdcall nmc_get_axis_io_in(WORD CardNo,WORD axis);


/************************************************************
*
*RTEX����Ӻ���
*
*
************************************************************/
DMC_API short __stdcall nmc_start_connect(WORD CardNo,WORD chan,WORD*info,WORD* len);
//DMC_API short __stdcall nmc_get_node_info(WORD CardNo,WORD*info,WORD* len);
DMC_API short __stdcall nmc_get_vendor_info(WORD CardNo,WORD axis,char* info,WORD* len);
DMC_API short __stdcall nmc_get_slave_type_info(WORD CardNo,WORD axis,char* info,WORD* len);
DMC_API short __stdcall nmc_get_slave_name_info(WORD CardNo,WORD axis,char* info,WORD* len);
DMC_API short __stdcall nmc_get_slave_version_info(WORD CardNo,WORD axis,char* info,WORD* len);

DMC_API short __stdcall nmc_write_parameter(WORD CardNo,WORD axis,WORD index, WORD subindex,DWORD para_data);
/**************************************************************
*����˵����RTEX������дEEPROM����
*
*
**************************************************************/
DMC_API short __stdcall nmc_write_slave_eeprom(WORD CardNo,WORD axis);
/**************************************************************
*index:rtex�������Ĳ�������
*subindex:rtex��������index����µĲ������
*para_data:�����Ĳ���ֵ
**************************************************************/
DMC_API short __stdcall nmc_read_parameter(WORD CardNo,WORD axis,WORD index, WORD subindex,DWORD* para_data);
/**************************************************************
*index:rtex�������Ĳ�������
*subindex:rtex��������index����µĲ������
*para_data:�����Ĳ���ֵ
**************************************************************/
DMC_API short __stdcall nmc_read_parameter_attributes(WORD CardNo,WORD axis,WORD index, WORD subindex,DWORD* para_data);
DMC_API short __stdcall nmc_set_cmdcycletime(WORD CardNo,WORD PortNum,DWORD cmdtime);
//����RTEX�������ڱ�(us)
DMC_API short __stdcall nmc_get_cmdcycletime(WORD CardNo,WORD PortNum,DWORD* cmdtime);
DMC_API short __stdcall nmc_start_log(WORD CardNo,WORD chan,WORD node, WORD Ifenable);
DMC_API short __stdcall nmc_get_log(WORD CardNo,WORD chan,WORD node, DWORD* data);
DMC_API short __stdcall nmc_config_atuo_log(WORD CardNo,WORD ifenable,WORD dir,WORD byte_index,WORD mask,WORD condition,DWORD counter);

//��չPDO
DMC_API short __stdcall nmc_write_rxpdo_extra(WORD CardNo,WORD PortNum,WORD address,WORD DataLen,int Value);
DMC_API short __stdcall nmc_read_rxpdo_extra(WORD CardNo,WORD PortNum,WORD address,WORD DataLen,int* Value);
DMC_API short __stdcall nmc_read_txpdo_extra(WORD CardNo,WORD PortNum,WORD address,WORD DataLen,int* Value);
DMC_API short __stdcall nmc_write_rxpdo_extra_uint(WORD CardNo,WORD PortNum,WORD address,WORD DataLen,DWORD Value);
DMC_API short __stdcall nmc_read_rxpdo_extra_uint(WORD CardNo,WORD PortNum,WORD address,WORD DataLen,DWORD* Value);
DMC_API short __stdcall nmc_read_txpdo_extra_uint(WORD CardNo,WORD PortNum,WORD address,WORD DataLen,DWORD* Value);
DMC_API short __stdcall nmc_get_log_state(WORD CardNo,WORD chan, DWORD* state);
DMC_API short __stdcall nmc_driver_reset(WORD CardNo,WORD axis);
DMC_API short __stdcall nmc_set_offset_pos(WORD CardNo,WORD axis, double offset_pos);
DMC_API short __stdcall nmc_get_offset_pos(WORD CardNo,WORD axis, double* offset_pos);
//���rtex����ֵ�������Ķ�Ȧֵ
DMC_API short __stdcall nmc_clear_abs_driver_multi_cycle(WORD CardNo,WORD axis);
//����io���32λ������չ
DMC_API short __stdcall nmc_write_outport_extern(WORD CardNo,WORD Channel,WORD NoteID,WORD portno,DWORD outport_val);
//��ȡio���32λ������չ
DMC_API short __stdcall nmc_read_outport_extern(WORD CardNo,WORD Channel,WORD NoteID,WORD portno,DWORD *outport_val);
//��ȡio����32λ������չ
DMC_API short __stdcall nmc_read_inport_extern(WORD CardNo,WORD Channel,WORD NoteID,WORD portno,DWORD *inport_val);
//����io���
DMC_API short __stdcall nmc_write_outbit_extern(WORD CardNo,WORD Channel,WORD NoteID,WORD IoBit,WORD IoValue);
//��ȡio���
DMC_API short __stdcall nmc_read_outbit_extern(WORD CardNo,WORD Channel,WORD NoteID,WORD IoBit,WORD *IoValue);
//��ȡio����
DMC_API short __stdcall nmc_read_inbit_extern(WORD CardNo,WORD Channel,WORD NoteID,WORD IoBit,WORD *IoValue);
//�������������
DMC_API short __stdcall nmc_get_current_fieldbus_state_info(WORD CardNo,WORD Channel,WORD *Axis,WORD *ErrorType,WORD *SlaveAddr,DWORD *ErrorFieldbusCode);
// ������ʷ������
DMC_API short __stdcall nmc_get_detail_fieldbus_state_info(WORD CardNo,WORD Channel,DWORD ReadErrorNum,DWORD *TotalNum, DWORD *ActualNum, WORD *Axis,WORD *ErrorType,WORD *SlaveAddr,DWORD *ErrorFieldbusCode);

//�����ɼ�
DMC_API short __stdcall nmc_start_pdo_trace(WORD CardNo,WORD Channel,WORD SlaveAddr,WORD Index_Num,DWORD Trace_Len,WORD *Index,WORD *Sub_Index);
//��ȡ�ɼ�����
DMC_API short __stdcall nmc_get_pdo_trace(WORD CardNo,WORD Channel,WORD SlaveAddr,WORD *Index_Num,DWORD *Trace_Len,WORD *Index,WORD *Sub_Index);
//���ô����ɼ�����
DMC_API short __stdcall nmc_set_pdo_trace_trig_para(WORD CardNo,WORD Channel,WORD SlaveAddr,WORD Trig_Index,WORD Trig_Sub_Index,int Trig_Value,WORD Trig_Mode);
//��ȡ�����ɼ�����
DMC_API short __stdcall nmc_get_pdo_trace_trig_para(WORD CardNo,WORD Channel,WORD SlaveAddr,WORD *Trig_Index,WORD *Trig_Sub_Index,int *Trig_Value,WORD *Trig_Mode);
//�ɼ����
DMC_API short __stdcall nmc_clear_pdo_trace_data(WORD CardNo,WORD Channel,WORD SlaveAddr);
//�ɼ�ֹͣ
DMC_API short __stdcall nmc_stop_pdo_trace(WORD CardNo,WORD Channel,WORD SlaveAddr);
//�ɼ����ݶ�ȡ
DMC_API short __stdcall nmc_read_pdo_trace_data(WORD CardNo,WORD Channel,WORD SlaveAddr,DWORD StartAddr,DWORD Readlen,DWORD *ActReadlen,BYTE *Data);
//�Ѳɼ�����
DMC_API short __stdcall nmc_get_pdo_trace_num(WORD CardNo,WORD Channel,WORD SlaveAddr,DWORD *Data_num, DWORD *Size_of_each_bag);
//�ɼ�״̬
DMC_API short __stdcall nmc_get_pdo_trace_state(WORD CardNo,WORD Channel,WORD SlaveAddr,WORD *Trace_state);
//����ר��
DMC_API short __stdcall nmc_reset_canopen(WORD CardNo);
DMC_API short __stdcall nmc_reset_rtex(WORD CardNo);
DMC_API short __stdcall nmc_reset_etc(WORD CardNo);
//���ߴ���������
DMC_API short __stdcall nmc_set_fieldbus_error_switch(WORD CardNo, WORD channel,WORD data);
DMC_API short __stdcall nmc_get_fieldbus_error_switch(WORD CardNo, WORD channel,WORD* data);

DMC_API short __stdcall nmc_torque_move(WORD CardNo,WORD axis,int Torque,WORD PosLimitValid,double PosLimitValue,WORD PosMode);
DMC_API short __stdcall nmc_change_torque(WORD CardNo,WORD axis,int Torque);
//��ȡת�ش�С
DMC_API short __stdcall nmc_get_torque(WORD CardNo,WORD axis,int* Torque);
//modbus����
DMC_API short __stdcall dmc_modbus_active_COM1(WORD id,const char* COMID,int speed, int bits, int check, int stop);
DMC_API short __stdcall dmc_modbus_active_COM2(WORD id,const char* COMID,int speed, int bits, int check, int stop);
DMC_API short __stdcall dmc_modbus_active_ETH(WORD id, WORD port);

DMC_API short __stdcall dmc_set_modbus_0x(WORD CardNo, WORD start, WORD inum, const char* pdata);
DMC_API short __stdcall dmc_get_modbus_0x(WORD CardNo, WORD start, WORD inum, char* pdata);
DMC_API short __stdcall dmc_set_modbus_4x(WORD CardNo, WORD start, WORD inum, const WORD* pdata);
DMC_API short __stdcall dmc_get_modbus_4x(WORD CardNo, WORD start, WORD inum, WORD* pdata);

DMC_API short __stdcall dmc_set_modbus_4x_float(WORD CardNo, WORD start, WORD inum, const float* pdata);
DMC_API short __stdcall dmc_get_modbus_4x_float(WORD CardNo, WORD start, WORD inum, float* pdata);
DMC_API short __stdcall dmc_set_modbus_4x_int(WORD CardNo, WORD start, WORD inum, const int* pdata);
DMC_API short __stdcall dmc_get_modbus_4x_int(WORD CardNo, WORD start, WORD inum, int* pdata);

DMC_API short __stdcall dmc_set_grant_error_protect_unit(WORD CardNo, WORD axis,WORD enable,double dstp_error, double emg_error);
DMC_API short __stdcall dmc_get_grant_error_protect_unit(WORD CardNo, WORD axis,WORD* enable,double* dstp_error, double* emg_error);
//�ݾಹ�����
DMC_API short __stdcall dmc_get_leadscrew_comp_config(WORD CardNo, WORD axis,WORD *n, int *startpos,int *lenpos,int *pCompPos,int *pCompNeg);
DMC_API short __stdcall dmc_set_leadscrew_comp_config_unit(WORD CardNo, WORD axis,WORD n, double startpos,double lenpos,double *pCompPos,double *pCompNeg);
DMC_API short __stdcall dmc_get_leadscrew_comp_config_unit(WORD CardNo, WORD axis,WORD *n, double *startpos,double *lenpos,double *pCompPos,double *pCompNeg);
//EZ���� ԭ�����棬���������
DMC_API short __stdcall dmc_get_homelatch_value_unit(WORD CardNo,WORD axis, double* pos);
DMC_API short __stdcall dmc_get_ezlatch_value_unit(WORD CardNo,WORD axis, double* pos);
//��������
DMC_API short __stdcall dmc_get_latch_value_extern_unit(WORD CardNo,WORD axis,WORD index,double* pos_by_mm);//������ȡֵ��ȡ δ���
//һά�Ƚ�
DMC_API short __stdcall dmc_compare_add_point_unit(WORD CardNo,WORD cmp,double pos,WORD dir, WORD action,DWORD actpara);//��ӱȽϵ�
DMC_API short __stdcall dmc_compare_get_current_point_unit(WORD CardNo,WORD cmp,double *pos);//��ȡ��ǰ�Ƚϵ�
//����λ�ñȽ�
DMC_API short __stdcall dmc_compare_add_point_multi_unit(WORD CardNo, WORD cmp,double pos, WORD dir,  WORD action, DWORD actpara,double times);//��ӱȽϵ� ��ǿ
//����λ�ñȽ�
DMC_API short __stdcall dmc_hcmp_add_point_unit(WORD CardNo,WORD hcmp, double cmp_pos);
DMC_API short __stdcall dmc_hcmp_set_liner_unit(WORD CardNo,WORD hcmp, double Increment,long Count);//��������ģʽ����
DMC_API short __stdcall dmc_hcmp_get_liner_unit(WORD CardNo,WORD hcmp, double* Increment,long* Count);
DMC_API short __stdcall dmc_hcmp_get_current_state_unit(WORD CardNo,WORD hcmp,long *remained_points,double *current_point,long *runned_points); //��ȡ���ٱȽ�״̬
DMC_API short __stdcall dmc_set_softlimit_unit(WORD CardNo,WORD axis,WORD enable, WORD source_sel,WORD SL_action, double N_limit,double P_limit);//��������λ����
DMC_API short __stdcall dmc_get_softlimit_unit(WORD CardNo,WORD axis,WORD *enable, WORD *source_sel,WORD *SL_action,double *N_limit,double *P_limit);//��ȡ����λ����

//����λ�õ��ӣ����ٱȽϹ���
DMC_API short __stdcall dmc_hcmp_set_config_overlap(WORD CardNo, WORD hcmp, WORD axis, WORD cmp_source, WORD cmp_logic, long time, WORD axis_num, WORD aux_axis, WORD aux_source);
DMC_API short __stdcall dmc_hcmp_get_config_overlap(WORD CardNo, WORD hcmp, WORD* axis, WORD* cmp_source, WORD* cmp_logic, long* time, WORD* axis_num, WORD* aux_axis, WORD* aux_source);

//�°����˶�
DMC_API short __stdcall	dmc_m_move_set_coodinate(WORD card,WORD liner, WORD axis_num, WORD* axis_list,uint32 in_io_num, WORD trig_flag, WORD pos_mode);
DMC_API short __stdcall	dmc_m_move_get_coodinate_para(WORD card,WORD liner, WORD* axis_num, WORD* axis_list,uint32* in_io_num, WORD* trig_flag, WORD* pos_mode);
DMC_API short __stdcall	dmc_m_move_set_z_para(WORD card,WORD liner, double z_up_safe_pos, double z_up_target_pos,double z_down_safe_pos, double z_down_target_pos);
DMC_API short __stdcall	dmc_m_move_get_z_para(WORD card,WORD liner, double* z_up_safe_pos, double* z_up_target_pos,double* z_down_safe_pos, double* z_down_target_pos);
DMC_API short __stdcall	dmc_m_move_set_xy_para(WORD card,WORD liner, double x_first_safe_pos,double x_second_safe_pos, double x_target_pos, WORD y_num ,double* y_target_pos);
DMC_API short __stdcall	dmc_m_move_get_xy_para(WORD card,WORD liner, double* x_first_safe_pos,double* x_second_safe_pos, double* x_target_pos, WORD y_num ,double* y_target_pos);
DMC_API short __stdcall	dmc_m_move_axes(WORD card,WORD liner);
DMC_API short __stdcall	dmc_m_move_get_run_phase(WORD card,WORD liner, WORD* run_phase);
DMC_API short __stdcall	dmc_m_move_stop(WORD card,WORD liner, WORD stop_mode);
DMC_API short __stdcall dmc_t_pmove_extern_unit(WORD CardNo, WORD axis, double MidPos,double TargetPos, double Min_Vel,double Max_Vel, double stop_Vel, double acc,double dec,WORD posi_mode);

DMC_API short __stdcall dmc_rtcp_set_kinematic_type(WORD CardNo,WORD Crd, WORD machine_type);
DMC_API short __stdcall dmc_rtcp_get_kinematic_type(WORD CardNo,WORD Crd, WORD* machine_type);
//�������߹ر�RTCP����
DMC_API short __stdcall dmc_rtcp_set_enable(WORD CardNo,WORD Crd, WORD enable);
DMC_API short __stdcall dmc_rtcp_get_enable(WORD CardNo,WORD Crd, WORD* enable);
//λ�������������ͣ�0-��������ϵλ�ã�1-��е����ϵλ��
DMC_API short __stdcall dmc_rtcp_set_position_type(WORD CardNo,WORD Crd, WORD position_type);
DMC_API short __stdcall dmc_rtcp_get_position_type(WORD CardNo,WORD Crd, WORD* position_type);
//����A�������ԭ�������ǰһ������ϵ��ƫ��, xyz��ƫ��a_offset[3]
DMC_API short __stdcall dmc_rtcp_set_vector_a(WORD CardNo,WORD Crd, double* a_offset);
DMC_API short __stdcall dmc_rtcp_get_vector_a(WORD CardNo,WORD Crd, double* a_offset);
//����B�������ԭ�������ǰһ������ϵ��ƫ��, xyz��ƫ��b_offset[3]
DMC_API short __stdcall dmc_rtcp_set_vector_b(WORD CardNo,WORD Crd, double* b_offset);
DMC_API short __stdcall dmc_rtcp_get_vector_b(WORD CardNo,WORD Crd, double* b_offset);
//����C�������ԭ�������ǰһ������ϵ��ƫ��, xyz��ƫ��c_offset[3]
DMC_API short __stdcall dmc_rtcp_set_vector_c(WORD CardNo,WORD Crd, double* c_offset);
DMC_API short __stdcall dmc_rtcp_get_vector_c(WORD CardNo,WORD Crd, double* c_offset);
//����A��B��C���ƫ��λ��,
//A,B,C�ڻ�0�����ƶ�����ʼ��̬��λ�ã����ʱ���A/B/C��ƫ�ƽǶȣ�
//������˳�ʼ��̬��λ�ò�������0�����ʱ���Ҫ����ƫ�ƽǶ�
DMC_API short __stdcall dmc_rtcp_set_rotate_joint_offset(WORD CardNo,WORD Crd, double A, double B, double C);
DMC_API short __stdcall dmc_rtcp_get_rotate_joint_offset(WORD CardNo,WORD Crd, double* A, double* B, double* C);
//���ø���ķ����빤������ϵ�Ĺ�ϵ
//������ڹ����ķ�������������˶���ʱ��������������ڹ���Ҳ�������˶��ģ��趨Ϊ1
//������Ϊ-1����ʼ����Ϊ1
//dir[5],��Ӧ����X,Y,Z,��ת��1����ת��2
DMC_API short __stdcall dmc_rtcp_set_joints_direction(WORD CardNo,WORD Crd, int* dir);
DMC_API short __stdcall dmc_rtcp_get_joints_direction(WORD CardNo,WORD Crd, int* dir);
//���õ��߳��ȣ���˫��ͷ��������
DMC_API short __stdcall dmc_rtcp_set_tool_length(WORD CardNo,WORD Crd, double tool);
DMC_API short __stdcall dmc_rtcp_get_tool_length(WORD CardNo,WORD Crd, double* tool);

DMC_API short __stdcall dmc_rtcp_get_actual_pos(WORD CardNo,WORD Crd, WORD AxisNum,WORD *AxisList,double* actual_pos);
DMC_API short __stdcall dmc_rtcp_get_command_pos(WORD CardNo,WORD Crd, WORD AxisNum,WORD *AxisList,double* command_pos);

DMC_API short __stdcall dmc_rtcp_kinematics_forward(WORD CardNo,WORD Crd, double* joint_pos, double* axis_pos);
DMC_API short __stdcall dmc_rtcp_kinematics_inverse(WORD CardNo,WORD Crd, double* axis_pos, double* joint_pos);
//���ô���ת����ٶ�
DMC_API short __stdcall dmc_rtcp_set_max_rotate_param(WORD CardNo,WORD Crd, double rotate_speed, double rotate_acc);
DMC_API short __stdcall dmc_rtcp_get_max_rotate_param(WORD CardNo,WORD Crd, double* rotate_speed, double* rotate_acc);
//���ù���ԭ��ƫ��ֵ
DMC_API short __stdcall dmc_rtcp_set_workpiece_offset(WORD CardNo,WORD Crd, WORD workpiece_index, double* offset);
DMC_API short __stdcall dmc_rtcp_get_workpiece_offset(WORD CardNo,WORD Crd, WORD workpiece_index, double* offset);
//���ù���ԭ��ƫ�÷�ʽ
DMC_API short __stdcall dmc_rtcp_set_workpiece_mode(WORD CardNo,WORD Crd, WORD enable,WORD workpiece_index);
DMC_API short __stdcall dmc_rtcp_get_workpiece_mode(WORD CardNo,WORD Crd, WORD* enable,WORD* workpiece_index);

//�����岹
DMC_API short __stdcall dmc_conti_helix_move_unit(WORD CardNo,WORD Crd,WORD AxisNum,WORD * AixsList,double * StartPos,double * TargetPos,WORD Arc_Dir,int Circle,WORD mode,int mark);
DMC_API short __stdcall dmc_helix_move_unit(WORD CardNo,WORD Crd,WORD AxisNum,WORD* AxisList,double * StartPos,double *TargetPos,WORD Arc_Dir,int Circle,WORD mode);

DMC_API short __stdcall dmc_compare_add_point_cycle_unit(WORD CardNo,WORD cmp,double pos,WORD dir, DWORD bitno,DWORD cycle,WORD level);//��ӱȽϵ�
//PDO����20190715
DMC_API short __stdcall dmc_pdo_buffer_enter(WORD CardNo,WORD axis);
DMC_API short __stdcall dmc_pdo_buffer_stop(WORD CardNo,WORD axis);
DMC_API short __stdcall dmc_pdo_buffer_clear(WORD CardNo,WORD axis);

DMC_API short __stdcall dmc_pdo_buffer_run_state(WORD CardNo,WORD axis,int * RunState,int * Remain,int * NotRunned,int* Runned);
DMC_API short __stdcall dmc_pdo_buffer_add_data(WORD CardNo,WORD axis, int size, int* data_table);

DMC_API short __stdcall dmc_pdo_buffer_start_multi(WORD CardNo,WORD AxisNum,WORD* AxisList,WORD* ResultList);
DMC_API short __stdcall dmc_pdo_buffer_pause_multi(WORD CardNo,WORD AxisNum,WORD* AxisList,WORD* ResultList);
DMC_API short __stdcall dmc_pdo_buffer_stop_multi(WORD CardNo,WORD AxisNum,WORD* AxisList,WORD* ResultList);
DMC_API short __stdcall dmc_pdo_buffer_add_data_multi(WORD CardNo, WORD AxisNum,WORD* AxisList, int size, int** data_table);
DMC_API short __stdcall dmc_calculate_arccenter_3point(double *start_pos,double *mid_pos, double *target_pos,double* cen_pos);

DMC_API short __stdcall	dmc_cmd_buf_open(WORD card,WORD group, WORD axis_num, WORD* axis_list);
DMC_API short __stdcall	dmc_cmd_buf_close(WORD card,WORD group);
DMC_API short __stdcall	dmc_cmd_buf_start(WORD card,WORD group);
DMC_API short __stdcall	dmc_cmd_buf_stop(WORD card,WORD group, WORD stop_mode);
DMC_API short __stdcall	dmc_cmd_buf_get_group_config(WORD card,WORD group, WORD* enable ,WORD* axis_num, WORD* axis_list);
DMC_API short __stdcall	dmc_cmd_buf_get_group_run_info(WORD card,WORD group, WORD* enable ,DWORD* array_id, DWORD* stop_reason,WORD* trig_phase ,WORD* phase);
DMC_API short __stdcall	dmc_cmd_buf_add_cmd(WORD card, WORD group, DWORD index, WORD  cmd_type, WORD  ProcessMode, WORD Dim,
	WORD* AxisList, double* TargetPositionFirst, double* m_TargetPositionSecond,WORD*  m_SoftlandFlag,double* SoftLandVel,double* SoftLandEndVel, WORD*  m_PosMode,double* m_TrigAheadPos,
	WORD  m_TrigFlag,  WORD m_TrigAxisNum,WORD* m_TrigAxislist,WORD*  m_TrigPosType,WORD*  m_TrigAxisPosRelationgship,double* m_TrigAxisPos,
	WORD m_TrigIONum, WORD*  m_TrigIOState,WORD* m_TrigINIOList,
	DWORD m_DelayCmdTime, WORD m_IOList, WORD  m_IOState,WORD m_TrigError
	);
DMC_API short __stdcall	dmc_cmd_buf_set_axis_profile(WORD card,WORD group, WORD axis_num, WORD* axis_list, double* start_vel, double* max_vel, double* stop_vel, double* tacc, double* tdec);

DMC_API short __stdcall dmc_m_set_muti_profile_unit(WORD CardNo, WORD group, WORD axis_num, WORD* axis_list, double* start_vel, double* max_vel, double* tacc, double* tdec, double* stop_vel);
DMC_API short __stdcall dmc_m_set_profile_unit(WORD CardNo, WORD group, WORD axis, double start_vel, double max_vel, double tacc, double tdec, double stop_vel);
DMC_API short __stdcall dmc_m_add_sigaxis_moveseg_data(WORD CardNo, WORD group, WORD axis, double Target_pos, WORD process_mode, DWORD mark);
DMC_API short __stdcall dmc_m_add_sigaxis_move_twoseg_data(WORD CardNo, WORD group, WORD axis, double Target_pos, double softland_pos, double softland_vel, double softland_endvel, WORD process_mode, DWORD mark);
DMC_API short __stdcall dmc_m_add_mutiaxis_moveseg_data(WORD CardNo, WORD group, WORD axisnum, WORD* axis_list, double* Target_pos, WORD process_mode, DWORD mark);
DMC_API short __stdcall dmc_m_add_mutiaxis_move_twoseg_data(WORD CardNo, WORD group, WORD axisnum,  WORD* axis_list, double* Target_pos, double* softland_pos, double* softland_vel, double* softland_endvel, WORD process_mode, DWORD mark);
DMC_API short __stdcall dmc_m_add_ioTrig_movseg_data(WORD CardNo, WORD group, WORD axisnum, WORD* axisList, double* Target_pos, WORD process_mode, WORD trigINbit, WORD trigINstate, DWORD mark);//io�����ƶ�
DMC_API short __stdcall dmc_m_add_mutiposTrig_movseg_data(WORD CardNo, WORD group, WORD axis, double Target_pos, WORD process_mode, WORD trigaxisNum, WORD* trigAxisList, double* trigPos, WORD* trigPosType, WORD* trigPosMode, DWORD mark);//λ�ô����ƶ�
DMC_API short __stdcall dmc_m_add_mutiposTrig_mov_twoseg_data(WORD CardNo, WORD group, WORD axis, double Target_pos, double softland_pos, double softland_vel, double softland_endvel, WORD process_mode, WORD trigAxisNum, WORD* trigAxisList, double* trigPos, WORD* trigPosType, WORD* trigPosMode, DWORD mark);//����λ�ô����ƶ�
DMC_API short __stdcall dmc_m_add_upseg_data(WORD CardNo, WORD group, WORD axis, double Target_pos, DWORD mark);
DMC_API short __stdcall dmc_m_add_up_twoseg_data(WORD CardNo, WORD group, WORD axis, double Target_pos, double softland_pos, double softland_vel, double softland_endvel, DWORD mark);
DMC_API short __stdcall dmc_m_add_ioPosTrig_movseg_data(WORD CardNo, WORD group, WORD axisnum, WORD* axisList, double* Target_pos, WORD process_mode, WORD trigAxis,double trigPos, WORD trigPosType, WORD trigMode, WORD TrigINNum, WORD* trigINList, WORD* trigINstate, DWORD mark);//λ��+io�����ƶ�
DMC_API short __stdcall dmc_m_add_ioPosTrig_mov_twoseg_data(WORD CardNo, WORD group, WORD axisnum, WORD* axisList, double* Target_pos, double* softland_pos, double* softland_vel, double* softland_endvel,WORD process_mode, WORD trigAxis, double trigPos, WORD trigPosType, WORD trigMode, WORD TrigINNum, WORD* trigINList, WORD* trigINstate, DWORD mark);//λ��+io�����ƶ�
DMC_API short __stdcall dmc_m_add_posTrig_movseg_data(WORD CardNo, WORD group, WORD axisnum, WORD* axisList, double* Target_pos, WORD process_mode, WORD trigAxis, double trigPos, WORD trigPosType, WORD trigMode, DWORD mark);//λ�ô����ƶ�
DMC_API short __stdcall dmc_m_add_posTrig_mov_twoseg_data(WORD CardNo, WORD group, WORD axisnum, WORD* axisList, double* Target_pos, double* softland_pos, double* softland_vel, double* softland_endvel, WORD process_mode, WORD trigAxis, double trigPos, WORD trigPosType, WORD trigMode, DWORD mark);//λ�ô����ƶ�
DMC_API short __stdcall dmc_m_add_ioPosTrig_down_seg_data(WORD CardNo, WORD group, WORD axis, double safePos, double Target_pos, WORD trigAxisNum, WORD* trigAxisList, double* trigPos, WORD* trigPosType, WORD* trigMode, WORD trigIN, WORD trigINstate, DWORD mark);
DMC_API short __stdcall dmc_m_add_ioPosTrig_down_twoseg_data(WORD CardNo, WORD group, WORD axis, double safePos, double Target_pos, double softland_pos, double softland_vel, double softland_endvel, WORD trigAxisNum, WORD* trigAxisList, double* trigPos, WORD* trigPosType, WORD* trigMode, WORD trigIN, WORD trigINstate, DWORD mark);
DMC_API short __stdcall dmc_m_add_posTrig_down_seg_data(WORD CardNo, WORD group, WORD axis, double safePos, double Target_pos, WORD trigAxisNum, WORD* trigAxisList, double* trigPos, WORD* trigPosType, WORD* trigMode, DWORD mark);
DMC_API short __stdcall dmc_m_add_posTrig_down_twoseg_data(WORD CardNo, WORD group, WORD axis, double safePos, double Target_pos, double softland_pos, double softland_vel, double softland_endvel, WORD trigAxisNum, WORD* trigAxisList, double* trigPos, WORD* trigPosType, WORD* trigMode, DWORD mark);

DMC_API short __stdcall dmc_m_posTrig_outbit(WORD CardNo, WORD group, WORD bitno, WORD on_off, WORD ahead_axis, double ahead_value, WORD ahead_PosType, WORD ahead_Mode, DWORD mark);
DMC_API short __stdcall dmc_m_immediate_write_outbit(WORD CardNo, WORD group, WORD bitno, WORD on_off, DWORD mark);
DMC_API short __stdcall dmc_m_wait_input(WORD CardNo, WORD group, WORD bitno, WORD on_off, double time_out, DWORD mark);
DMC_API short __stdcall dmc_m_delay_time(WORD CardNo, WORD group, double delay_time, DWORD mark);
DMC_API short __stdcall dmc_m_get_run_state(WORD CardNo, WORD group, WORD* state, WORD* enable, DWORD* stop_reason, WORD* trig_phase, DWORD* mark);
DMC_API short __stdcall dmc_m_open_list(WORD CardNo, WORD group, WORD axis_num, WORD* axis_list);
DMC_API short __stdcall dmc_m_close_list(WORD CardNo, WORD group);
DMC_API short __stdcall dmc_m_start_list(WORD CardNo, WORD group);
DMC_API short __stdcall dmc_m_stop_list(WORD CardNo, WORD group,WORD stopMode);
DMC_API short __stdcall dmc_m_add_posTrig_down_seg_cmd_data(WORD CardNo, WORD group, WORD axis, double safePos, double Target_pos, WORD trigAxisNum, WORD* trigAxisList, DWORD mark);
DMC_API short __stdcall dmc_m_add_posTrig_down_twoseg_cmd_data(WORD CardNo, WORD group, WORD axis, double safePos, double Target_pos, double softland_pos, double softland_vel, double softland_endvel, WORD trigAxisNum, WORD* trigAxisList, DWORD mark);
DMC_API short __stdcall dmc_m_pause_list(WORD CardNo, WORD group, WORD stop_mode);

DMC_API short __stdcall dmc_get_ad_input_all(WORD CardNo, double* Vout);
DMC_API short __stdcall dmc_conti_pmove_unit_pausemode(WORD CardNo, WORD axis,double TargetPos, double Min_Vel,double Max_Vel, double stop_Vel, double acc,double dec,double smooth_time,WORD posi_mode);
DMC_API short __stdcall dmc_conti_return_pausemode(WORD CardNo, WORD Crd, WORD axis);
DMC_API short __stdcall	dmc_cmd_buf_temp_stop(WORD CardNo,WORD group,WORD stop_mode);
DMC_API short __stdcall dmc_check_if_crc_support(WORD CardNo);
//������da����
DMC_API short __stdcall dmc_set_encoder_da_follow_enable(WORD CardNo,WORD axis,WORD enable);	
DMC_API short __stdcall dmc_get_encoder_da_follow_enable(WORD CardNo,WORD axis,WORD* enable);

//����ײ��⹦�ܽӿ� 
DMC_API short __stdcall dmc_set_axis_conflict_config(WORD CardNo, WORD*  axis_list, WORD* axis_depart_dir, double home_dist, double conflict_dist, WORD stop_mode); 
DMC_API short __stdcall dmc_get_axis_conflict_config (WORD CardNo, WORD*  axis_list, WORD* axis_depart_dir, double* home_dist, double* conflict_dist, WORD* stop_mode);
DMC_API short __stdcall dmc_axis_conflict_config_en(WORD CardNo, WORD enable);
DMC_API short __stdcall dmc_get_axis_conflict_config_en(WORD CardNo, WORD* enable);

//trig_num ����������trig_pos ����λ��
DMC_API short __stdcall dmc_get_pmove_change_pos_speed_state(WORD CardNo,WORD axis, WORD * trig_num, double * trig_pos);

//������������Ӵ�����ֵ�Ľӿ�
DMC_API short __stdcall dmc_read_inbit_ex(WORD CardNo,WORD bitno,WORD *state);//��ȡ����ڵ�״̬
DMC_API short __stdcall dmc_read_outbit_ex(WORD CardNo,WORD bitno,WORD *state);//��ȡ����ڵ�״̬
DMC_API short __stdcall dmc_read_inport_ex(WORD CardNo,WORD portno,DWORD *state);//��ȡ����˿ڵ�ֵ
DMC_API short __stdcall dmc_read_outport_ex(WORD CardNo,WORD portno,DWORD *state);//��ȡ����˿ڵ�ֵ
//ģ�����Ӷ�ȡ״̬
//����io���
DMC_API short __stdcall nmc_write_outbit_ex(WORD CardNo,WORD NoteID,WORD IoBit,WORD IoValue,WORD* state);
//��ȡio���
DMC_API short __stdcall nmc_read_outbit_ex(WORD CardNo,WORD NoteID,WORD IoBit,WORD *IoValue,WORD* state);
//��ȡio����
DMC_API short __stdcall nmc_read_inbit_ex(WORD CardNo,WORD NoteID,WORD IoBit,WORD *IoValue,WORD* state);
//����io���32λ
DMC_API short __stdcall nmc_write_outport_ex(WORD CardNo,WORD NoteID,WORD portno,DWORD outport_val,WORD* state);
//��ȡio���32λ
DMC_API short __stdcall nmc_read_outport_ex(WORD CardNo,WORD NoteID,WORD portno,DWORD *outport_val,WORD* state);
//��ȡio����32λ
DMC_API short __stdcall nmc_read_inport_ex(WORD CardNo,WORD NoteID,WORD portno,DWORD *inport_val,WORD* state);

//����DA����	
DMC_API short __stdcall nmc_set_da_output_ex(WORD CardNo,WORD NoteID,WORD channel,double Value,WORD* state);
//��ȡDA����
DMC_API short __stdcall nmc_get_da_output_ex(WORD CardNo,WORD NoteID,WORD channel,double *Value,WORD* state);
//��ȡAD����
DMC_API short __stdcall nmc_get_ad_input_ex(WORD CardNo,WORD NoteID,WORD channel,double *Value,WORD* state);
//����ADģʽ
DMC_API short __stdcall nmc_set_ad_mode_ex(WORD CardNo,WORD NoteID,WORD channel,WORD mode,DWORD buffer_nums,WORD* state);
DMC_API short __stdcall nmc_get_ad_mode_ex(WORD CardNo,WORD NoteID,WORD channel,WORD* mode,DWORD buffer_nums,WORD* state);
//����DAģʽ
DMC_API short __stdcall nmc_set_da_mode_ex(WORD CardNo,WORD NoteID,WORD channel,WORD mode,DWORD buffer_nums,WORD* state);
DMC_API short __stdcall nmc_get_da_mode_ex(WORD CardNo,WORD NoteID,WORD channel,WORD* mode,DWORD buffer_nums,WORD* state);
//����д��flash
DMC_API short __stdcall nmc_write_to_flash_ex(WORD CardNo,WORD PortNum,WORD NodeNum,WORD* state);

//����ּ��ͨ��
DMC_API short __stdcall dmc_sorting_close_ex(WORD CardNo,WORD sortModuleNo);
DMC_API short __stdcall dmc_sorting_start_ex(WORD CardNo,WORD sortModuleNo);
DMC_API short __stdcall dmc_sorting_set_init_config_ex(WORD CardNo ,WORD cameraCount, int *pCameraPos, WORD *pCamIONo, DWORD cameraTime, WORD cameraTrigLevel, WORD blowCount, int*pBlowPos, WORD*pBlowIONo, DWORD blowTime, WORD blowTrigLevel, WORD axis, WORD dir, WORD checkMode,WORD sortModuleNo);
DMC_API short __stdcall dmc_sorting_set_camera_trig_count_ex(WORD CardNo ,WORD cameraNum, DWORD cameraTrigCnt,WORD sortModuleNo);
DMC_API short __stdcall dmc_sorting_get_camera_trig_count_ex(WORD CardNo ,WORD cameraNum, DWORD* pCameraTrigCnt, WORD count,WORD sortModuleNo);
DMC_API short __stdcall dmc_sorting_set_blow_trig_count_ex(WORD CardNo ,WORD blowNum, DWORD blowTrigCnt,WORD sortModuleNo);
DMC_API short __stdcall dmc_sorting_get_blow_trig_count_ex(WORD CardNo ,WORD blowNum, DWORD* pBlowTrigCnt, WORD count,WORD sortModuleNo);
DMC_API short __stdcall dmc_sorting_get_camera_config_ex(WORD CardNo ,WORD index,int* pos,DWORD* trigTime, WORD* ioNo, WORD* trigLevel,WORD sortModuleNo);
DMC_API short __stdcall dmc_sorting_get_blow_config_ex(WORD CardNo ,WORD index, int* pos,DWORD* trigTime, WORD* ioNo, WORD* trigLevel,WORD sortModuleNo);
DMC_API short __stdcall dmc_sorting_get_blow_status_ex(WORD CardNo ,DWORD* trigCntAll, WORD* trigMore,WORD* trigLess,WORD sortModuleNo);
DMC_API short __stdcall dmc_sorting_trig_blow_ex(WORD CardNo ,WORD blowNum,WORD sortModuleNo);
DMC_API short __stdcall dmc_sorting_set_blow_enable_ex(WORD CardNo ,WORD blowNum,WORD enable,WORD sortModuleNo);
DMC_API short __stdcall dmc_sorting_set_piece_config_ex(WORD CardNo ,DWORD maxWidth,DWORD minWidth,DWORD minDistance, DWORD minTimeIntervel,WORD sortModuleNo);
DMC_API short __stdcall dmc_sorting_get_piece_status_ex(WORD CardNo ,DWORD* pieceFind,DWORD* piecePassCam, DWORD* dist2next, DWORD*pieceWidth,WORD sortModuleNo);
DMC_API short __stdcall dmc_sorting_set_cam_trig_phase_ex(WORD CardNo,WORD blowNo,double coef,WORD sortModuleNo);
DMC_API short __stdcall dmc_sorting_set_blow_trig_phase_ex(WORD CardNo,WORD blowNo,double coef,WORD sortModuleNo);
//��ȡ�ּ�ָ����������
DMC_API short __stdcall dmc_get_sortdev_blow_cmd_cnt(WORD CardNo, WORD blowDevNum, long* cnt);
//��ȡδ����ָ��������������
DMC_API short __stdcall dmc_get_sortdev_blow_cmderr_cnt(WORD CardNo, WORD blowDevNum, long* errCnt);
//�ּ����״̬
DMC_API short __stdcall dmc_get_sortqueue_status(WORD CardNo, long * curSorQueueLen, long* passCamWithNoCmd);

// ��Բ�����岹
DMC_API short __stdcall dmc_conti_ellipse_move_unit(WORD CardNo, WORD Crd,WORD AxisNum, WORD* AxisList, double* Target_Pos, double* Cen_Pos, double A_Axis_Len, double B_Axis_Len, WORD Dir, WORD Pos_Mode,long mark);
//��ȡ��״̬����
DMC_API	short dmc_get_axis_status_advance(WORD CardNo, WORD axis_no, WORD motion_no, WORD* axis_plan_state, DWORD* ErrPlulseCnt, WORD* fpga_busy);
//�����岹vmove
DMC_API	short __stdcall dmc_conti_vmove_unit(WORD CardNo, WORD Crd, WORD axis, double vel, double acc ,WORD dir, long imark);
DMC_API	short __stdcall dmc_conti_vmove_stop(WORD CardNo, WORD Crd, WORD axis, double dec, long imark);

//����sn20191101
DMC_API short __stdcall dmc_enter_password_ex(WORD CardNo, const char * str_pass);

//���ӳ��ֹ���
DMC_API	short __stdcall dmc_gear_in(WORD CardNo, WORD master_axis, WORD slave_axis, WORD follow_source, double ratio_numerator, double ratio_denominator, double acc, double dec, double s_time, WORD buffer_mode);
DMC_API	short __stdcall dmc_get_gear_in(WORD CardNo, WORD* master_axis, WORD slave_axis, WORD* follow_source, double* ratio_numerator, double* ratio_denominator, double* acc, double* dec, double* s_time, WORD* buffer_mode);
DMC_API	short __stdcall dmc_update_gear_scale(WORD CardNo, WORD slave_axis, double ratio_numerator, double ratio_denominator, double acc, double dec,double s_time);
DMC_API	short __stdcall dmc_gear_in_pos(WORD CardNo, WORD master_axis,WORD slave_axis,WORD follow_source,double ratio_numerator,double ratio_denominator,double master_sync_pos,double slave_sync_pos,double master_start_dist,double velocity,double acc,double dec,double s_time, WORD buffer_mode);
DMC_API	short __stdcall dmc_get_gear_in_pos(WORD CardNo, WORD* master_axis,WORD slave_axis,WORD* follow_source,double* ratio_numerator,double* ratio_denominator,double* master_sync_pos,double* slave_sync_pos,double* master_start_dist,double* velocity,double* acc,double* dec,double* s_time, WORD* buffer_mode);
DMC_API	short __stdcall dmc_get_in_gear_state(WORD CardNo, WORD slave_axis,WORD* in_gear);
DMC_API	short __stdcall dmc_get_gear_aborted_state(WORD CardNo, WORD slave_axis,WORD* aborted_state);
DMC_API	short __stdcall dmc_gear_out(WORD CardNo, WORD slave_axis);
DMC_API short __stdcall dmc_trace_set_config(WORD CardNo, short trace_cycle, short lost_handle, short trace_type, short trigger_object_index, short trigger_type, int mask, long long condition);
DMC_API short __stdcall dmc_trace_get_config(WORD CardNo, short * trace_cycle, short * lost_handle, short * trace_type, short * trigger_object_index, short * trigger_type, int * mask, long long * condition);

/***********************************************************
 * ���òɼ�����һ�ο������500���ɼ�����
 * data_type 	���ݵ����ͣ����ɼ�����˵����
 * data_index 	���ݵ�������������Ǹ�����أ���������ţ������IO������IO��ţ��������
 * data_sub_index ���ݵ�������������ǰ���ɼ�IO�����ʾIO��������š�
 * data_bytes 	�����ֽ��������вɼ����ͻ��Զ�ƥ�䣬�̶�Ϊ0��Ԥ��������չ����
 **********************************************************/
DMC_API short __stdcall dmc_trace_reset_config_object(WORD CardNo);
DMC_API short __stdcall dmc_trace_add_config_object(WORD CardNo, short data_type, short data_index, short data_sub_index, short slave_id, short data_bytes);
DMC_API short __stdcall dmc_trace_get_config_object(WORD CardNo,short object_index, short * data_type, short * data_index, short * data_sub_index, short * slave_id, short * data_bytes);

//����trace
DMC_API short __stdcall dmc_trace_data_start(WORD CardNo);

//ֹͣtrace
DMC_API short __stdcall dmc_trace_data_stop(WORD CardNo);

//��λtrace��ֹͣ�ɼ���ʱ����ܵ��ã�������Ѳɼ����������Լ������־λ
DMC_API short __stdcall dmc_trace_data_reset(WORD CardNo);


//trace�Ƿ��Ѿ�����
DMC_API short __stdcall dmc_trace_get_flag(WORD CardNo,short * start_flag,short * triggered_flag,short * lost_flag);

/***********************************************************
 *��ȡ�ɼ�״̬
 * valid_num 	�Ѳɼ���δ����ȡ�����ݸ���
 * free_num 	ʣ������ڱ���ɼ����ݵĸ���
 * object_total_bytes   �ɼ��������ֽ���
 * object_total_num 	�ɼ������ܸ���
 **********************************************************/
DMC_API short __stdcall dmc_trace_get_state(WORD CardNo,int* valid_num,int* free_num,int* object_total_bytes,int* object_total_num);

/***********************************************************
 *��ȡ�ɼ�����
 * bufsize 	���ݻ������ֽ���
 * data[1024] 	���ݻ�������
 * byte_size   ��ȡ�����ݵ��ֽ���
 **********************************************************/
DMC_API short __stdcall dmc_trace_get_data(WORD CardNo,int bufsize,unsigned char* data, int* byte_size);

//trace��λ����źţ�ֻ�Ǹ�λ��־λ
DMC_API short __stdcall dmc_trace_reset_lost_flag(WORD CardNo);
DMC_API short __stdcall dmc_message_buffer_enable(WORD CardNo,WORD index, DWORD buffer_size,  BYTE console_enable);
DMC_API short __stdcall dmc_message_buffer_disable(WORD CardNo,WORD index);
DMC_API short __stdcall dmc_message_buffer_get_data (WORD CardNo,WORD index, long bufsize, BYTE* data,DWORD *pbufsize);

DMC_API short __stdcall dmc_t_pmove_extern_softstart(WORD CardNo, WORD axis, double MidPos, double TargetPos, double start_Vel, double Max_Vel, double stop_Vel, DWORD delay_ms, double Max_Vel2,double stop_vel2, double acc_time, double dec_time, WORD posi_mode);

DMC_API short __stdcall dmc_t_pmove_extern_softstart_unit(WORD CardNo, WORD axis, double MidPos, double TargetPos, double start_Vel, double Max_Vel, double stop_Vel, DWORD delay_ms, double Max_Vel2,double stop_vel2, double acc_time, double dec_time, WORD posi_mode);


//PVT_continuous�ӿ�.
//�·�PVT continuous���ڵ�����
DMC_API short __stdcall dmc_pvt_table_continuous(WORD CardNo, WORD axis, DWORD count, double* pos, double* vel, double* percent, double* vel_max, double* acc, double* dec);
//���ݴ���Ĳ�������ȡ����λ�ýڵ��ʱ��
DMC_API short __stdcall dmc_pvt_continuous_calculate(WORD CardNo, WORD axis, double* time);
//����PVT continuous �˶�
DMC_API short __stdcall dmc_pvt_continuous_start(WORD CardNo, WORD axis_num, WORD* axis_list,double* start_delay_time);

//���߸�λ������ֿ�������
DMC_API short __stdcall nmc_set_slave_output_retain(WORD CardNo,WORD Enable);
DMC_API short __stdcall nmc_get_slave_output_retain(WORD CardNo,WORD * Enable);

DMC_API short __stdcall dmc_m_add_mutiposTrig_singledown_seg_data(WORD CardNo, WORD group, WORD axis, double safePos, double Target_pos, WORD process_mode, WORD trigAxisNum, WORD* trigAxisList, double* trigPos, WORD* trigPosType, WORD* trigMode, DWORD mark);
DMC_API short __stdcall dmc_m_add_mutiposTrig_mutidown_seg_data(WORD CardNo, WORD group, WORD axisnum, WORD* axis_list, double* safePos, double* Target_pos, WORD process_mode, WORD trigAxisNum, WORD* trigAxisList, double* trigPos, WORD* trigPosType, WORD* trigMode, DWORD mark);

//����λ���������Χ����
DMC_API short __stdcall dmc_cmd_buf_set_allow_error(WORD CardNo, WORD group, double allow_error);
DMC_API short __stdcall dmc_cmd_buf_get_allow_error(WORD CardNo, WORD group, double* allow_error);
//�뾶�յ�Բ���岹
DMC_API short __stdcall dmc_arc_move_radius_multicoor(WORD CardNo, WORD Crd, WORD* AxisList, double *Target_Pos, double Arc_Radius, WORD Arc_Dir, long Circle, WORD posi_mode);
//����Բ��
DMC_API short __stdcall dmc_arc_move_3points_multicoor(WORD CardNo,WORD Crd,WORD* AxisList, double *Target_Pos, double *Mid_Pos, long Circle, WORD posi_mode);

//����λ���������Χ����
DMC_API short __stdcall dmc_m_set_encoder_error_allow(WORD CardNo, WORD group, double allow_error);
DMC_API short __stdcall dmc_m_get_encoder_error_allow(WORD CardNo, WORD group, double* allow_error);

//���籣��
DMC_API short __stdcall dmc_set_persistent_reg_byte(WORD CardNo, DWORD start, DWORD inum, const char* pdata);
DMC_API short __stdcall dmc_get_persistent_reg_byte(WORD CardNo, DWORD start, DWORD inum, char* pdata);
DMC_API short __stdcall dmc_set_persistent_reg_float(WORD CardNo, DWORD start, DWORD inum, const float* pdata);
DMC_API short __stdcall dmc_get_persistent_reg_float(WORD CardNo, DWORD start, DWORD inum, float* pdata);
DMC_API short __stdcall dmc_set_persistent_reg_int(WORD CardNo, DWORD start, DWORD inum, const int* pdata);
DMC_API short __stdcall dmc_get_persistent_reg_int(WORD CardNo, DWORD start, DWORD inum, int* pdata);

DMC_API short __stdcall nmc_torque_set_delay_cycle(WORD CardNo,WORD axis,int delay_cycle);

DMC_API short __stdcall dmc_conti_delay_outbit_to_start_path(WORD CardNo, WORD Crd, WORD bitno,WORD on_off,double delay_value,WORD delay_mode,double ReverseTime);
DMC_API short __stdcall dmc_conti_delay_outbit_to_stop_path(WORD CardNo, WORD Crd, WORD bitno,WORD on_off,double delay_time,double ReverseTime);
DMC_API short __stdcall dmc_conti_ahead_outbit_to_stop_path(WORD CardNo, WORD Crd, WORD bitno,WORD on_off,double ahead_value,WORD ahead_mode,double ReverseTime);

//���������дflash�ӿ�
DMC_API short __stdcall dmc_set_persistent_param_config(WORD CardNo, WORD axis, DWORD item);
DMC_API short __stdcall dmc_get_persistent_param_config(WORD CardNo, WORD axis, DWORD* item);

DMC_API short __stdcall dmc_hcmp_fifo_add_point_dir_unit(WORD CardNo,WORD hcmp, WORD num,double *cmp_pos,DWORD dir);
DMC_API short __stdcall dmc_hcmp_fifo_add_table_dir(WORD CardNo,WORD hcmp, WORD num,double *cmp_pos,DWORD dir);

DMC_API short __stdcall dmc_axis_io_status_ex(WORD CardNo,WORD axis,DWORD *state);//��ȡָ�����й��˶��źŵ�״̬
DMC_API short __stdcall dmc_check_done_ex(WORD CardNo,WORD axis,WORD * state);//��ȡָ������˶�״̬

DMC_API short __stdcall dmc_conti_line_section_unit(WORD CardNo,WORD Crd,WORD AxisNum,WORD* AxisList,double* pPosList,double Section_Dist,WORD Bitno,WORD On_Off,WORD Io_Mode,double Time_Dist_Value,double ReverseTime,WORD posi_mode,WORD mark);

DMC_API short __stdcall dmc_conti_arc_move_center_section_unit(WORD CardNo,WORD Crd,WORD AxisNum,WORD* AxisList,double* Target_Pos,double* Cen_Pos,WORD Arc_Dir,WORD Circle,double Section_Dist,WORD Bitno,WORD On_Off,WORD Io_Mode,double Time_Dist_Value,double ReverseTime,WORD posi_mode,WORD mark);

DMC_API short __stdcall dmc_conti_arc_move_radius_section_unit(WORD CardNo,WORD Crd,WORD AxisNum,WORD* AxisList,double* Target_Pos,double Arc_Radius,WORD Arc_Dir,WORD Circle,double Section_Dist,WORD Bitno,WORD On_Off,WORD Io_Mode,double Time_Dist_Value,double ReverseTime,WORD posi_mode,WORD mark);

DMC_API short __stdcall dmc_conti_arc_move_3points_section_unit(WORD CardNo,WORD Crd,WORD AxisNum,WORD* AxisList,double* Target_Pos,double* Mid_Pos,WORD Circle,double Section_Dist,WORD Bitno,WORD On_Off,WORD Io_Mode,double Time_Dist_Value,double ReverseTime,WORD posi_mode,WORD mark);

DMC_API short __stdcall dmc_get_firmware_boot_type(WORD CardNo,WORD* boot_type);



//�жϹ��� 
DMC_API DWORD __stdcall dmc_int_enable(WORD CardNo, DMC3K5K_OPERATE funcIntHandler, PVOID operate_data);
DMC_API DWORD __stdcall dmc_int_disable(WORD CardNo);
DMC_API short __stdcall dmc_set_intmode_enable(WORD Cardno,WORD Intno,WORD Enable);
DMC_API short __stdcall dmc_get_intmode_enable(WORD Cardno,WORD Intno,WORD *Status);
DMC_API short __stdcall dmc_set_intmode_config(WORD Cardno,WORD Intno,WORD IntItem,WORD IntIndex,WORD IntSubIndex,WORD Logic);
DMC_API	short __stdcall dmc_get_intmode_config(WORD Cardno,WORD Intno,WORD *IntItem,WORD *IntIndex,WORD *IntSubIndex,WORD*Logic);
DMC_API short __stdcall dmc_get_int_status(WORD Cardno,DWORD *IntStatus);
DMC_API short __stdcall dmc_reset_int_status(WORD Cardno, WORD Intno);

DMC_API short __stdcall dmc_set_pci_int(WORD CardNo);
DMC_API short __stdcall dmc_pmove_change_pos_speed_inbit(WORD CardNo,WORD axis, WORD inbit, WORD enable);
DMC_API short __stdcall dmc_get_pmove_change_pos_speed_inbit(WORD CardNo,WORD axis, WORD * inbit, WORD* enable);

DMC_API short __stdcall dmc_set_arc_zone_limit_config_unit(WORD CardNo, WORD* AxisList, WORD AxisNum, double *Center, double Radius,WORD Source, WORD StopMode);
DMC_API short __stdcall dmc_get_arc_zone_limit_config_unit(WORD CardNo, WORD* AxisList, WORD* AxisNum, double *Center, double* Radius,WORD* Source, WORD* StopMode);

DMC_API short __stdcall dmc_set_latch_stop_axis(WORD CardNo, WORD latch, WORD  num,  WORD * axislist);
DMC_API short __stdcall dmc_get_latch_stop_axis(WORD CardNo, WORD latch, WORD * num,  WORD * axislist);
DMC_API short __stdcall dmc_compare_add_point_cycle_2d(WORD ConnectNo,WORD* axis,double* pos,WORD* dir, DWORD bitno,DWORD cycle,WORD level);

DMC_API short __stdcall dmc_set_factor_error_unit(WORD CardNo,WORD axis,double factor,double error);
DMC_API short __stdcall dmc_get_factor_error_unit(WORD CardNo,WORD axis,double* factor,double* error);
DMC_API short __stdcall dmc_set_pulse_encoder_count_error_unit(WORD CardNo,WORD axis,double error);
DMC_API short __stdcall dmc_get_pulse_encoder_count_error_unit(WORD CardNo,WORD axis,double *error);
DMC_API short __stdcall dmc_check_pulse_encoder_count_error_unit(WORD CardNo,WORD axis,double* pulse_position, double* enc_position);

DMC_API short __stdcall dmc_set_ad_monitor_config(WORD CardNo, WORD Crd, WORD CANid, WORD channel, WORD ADEn, double ADValDown, double ADValUp);
DMC_API short __stdcall dmc_get_ad_monitor_config(WORD CardNo, WORD Crd, WORD* CANid, WORD* channel, WORD* ADEn, double* ADValDown, double* ADValUp);
DMC_API short __stdcall dmc_get_ad_monitor_result(WORD CardNo, WORD Crd, WORD *ch, double* ADRet, WORD* num,double *pos);
DMC_API short __stdcall dmc_clear_ad_monitor_result(WORD CardNo,WORD Crd);


DMC_API short __stdcall dmc_update_target_position_extern_unit(WORD CardNo, WORD axis, double mid_pos, double aim_pos, double vel,WORD posi_mode);
//ָ����������λ���˶� ͬʱ�����ٶȺ�Sʱ��(����)
DMC_API short __stdcall dmc_pmove_extern_unit(WORD CardNo, WORD axis, double dist,double Min_Vel, double Max_Vel, double Tacc, double Tdec, double stop_Vel, double s_para, WORD posi_mode);
DMC_API short __stdcall dmc_pmove_extern_acc_unit(WORD CardNo, WORD axis, double dist,double Min_Vel, double Max_Vel, double Tacc, double Tdec, double stop_Vel, double s_para, WORD posi_mode);
DMC_API short __stdcall dmc_m_mutiposTrig_outbit(WORD CardNo, WORD group, WORD bitno, WORD on_off, WORD process_mode, WORD trigAxisNum, WORD* trigAxisList, double* trigPos, WORD* trigPosType, WORD* trigMode, DWORD mark);//λ�ô���IO���

DMC_API short __stdcall dmc_cmp_fifo_set_enable(WORD CardNo, WORD Crd, WORD enable);
DMC_API short __stdcall dmc_cmp_fifo_get_enable(WORD CardNo, WORD Crd, WORD* enable);
DMC_API short __stdcall dmc_cmp_fifo_get_state(WORD CardNo, WORD Crd, long* remained_space); 
DMC_API short __stdcall dmc_cmp_fifo_clear_points(WORD CardNo, WORD Crd);
DMC_API short __stdcall dmc_cmp_fifo_set_config_params(WORD CardNo, WORD Crd, WORD Bitno, WORD On_Off, WORD Io_Mode, double Time_Dist_Value, double ReverseTime);
DMC_API short __stdcall dmc_cmp_fifo_get_config_params(WORD CardNo, WORD Crd, WORD* Bitno, WORD* On_Off, WORD* Io_Mode, double* Time_Dist_Value, double* ReverseTime);
DMC_API short __stdcall dmc_conti_line_add_cmp_fifo_unit(WORD CardNo, WORD Crd, WORD AxisNum,WORD* AxisList,double* Target_Pos, double* cmp_pos, WORD num, WORD posi_mode, long mark);
DMC_API short __stdcall dmc_conti_arc_move_3points_add_cmp_fifo_unit(WORD CardNo, WORD Crd, WORD AxisNum, WORD* AxisList,double* Target_Pos, double* Mid_Pos,WORD Circle, double *cmp_pos,WORD num, WORD posi_mode,long mark);
DMC_API short __stdcall dmc_conti_arc_move_center_add_cmp_fifo_unit(WORD CardNo, WORD Crd, WORD AxisNum, WORD* AxisList, double* Target_Pos, double* Cen_Pos,WORD Arc_Dir,WORD Circle, double *cmp_pos,WORD num, WORD posi_mode,long mark);
DMC_API short __stdcall dmc_conti_arc_move_radius_add_cmp_fifo_unit(WORD CardNo, WORD Crd, WORD AxisNum, WORD* AxisList, double* Target_Pos, double Arc_Radius,WORD Arc_Dir,WORD Circle, double *cmp_pos,WORD num, WORD posi_mode,long mark);
DMC_API short __stdcall dmc_cmp_fifo_get_total_point(WORD CardNo,WORD  Crd,long *total_point); 
DMC_API short __stdcall dmc_cmp_fifo_get_remain_point(WORD CardNo,WORD  Crd,long *remain_point); 
DMC_API short __stdcall dmc_cmp_fifo_get_trig_point(WORD CardNo,WORD  Crd,long *trig_point); 
DMC_API short __stdcall dmc_cmp_fifo_get_force_trig_point(WORD CardNo,WORD  Crd,long *force_trig_point); 

DMC_API short __stdcall dmc_conti_wait_node_input_delay_to_start(WORD CardNo,WORD Crd,WORD node_ID, WORD bitno,WORD on_off, double delay_value,WORD delay_mode ,double TimeOut);
DMC_API short __stdcall dmc_conti_wait_node_input_ahead_to_stop(WORD CardNo,WORD Crd,WORD node_ID, WORD bitno,WORD on_off, double ahead_value,WORD ahead_mode ,double TimeOut);
DMC_API short __stdcall dmc_conti_delay_node_outbit_to_start(WORD CardNo, WORD Crd,WORD node_ID, WORD bitno,WORD on_off,double delay_value,WORD delay_mode,double ReverseTime);
DMC_API short __stdcall dmc_conti_delay_node_outbit_to_stop(WORD CardNo, WORD Crd,WORD node_ID, WORD bitno,WORD on_off,double delay_time,double ReverseTime);
DMC_API short __stdcall dmc_conti_ahead_node_outbit_to_stop(WORD CardNo, WORD Crd,WORD node_ID, WORD bitno,WORD on_off,double ahead_value,WORD ahead_mode,double ReverseTime);
DMC_API short __stdcall dmc_conti_write_node_outbit(WORD CardNo, WORD Crd,WORD node_ID, WORD bitno,WORD on_off,double ReverseTime);
DMC_API short __stdcall dmc_conti_clear_node_io_action(WORD CardNo, WORD Crd, WORD node_ID,DWORD Io_Mask);


DMC_API short __stdcall dmc_get_connect_type(WORD CardNo, WORD * ConnectType);
DMC_API short __stdcall dmc_board_init_eth(const char* IpAddr);
//����ֹͣ����
DMC_API short __stdcall dmc_set_dec_stop_dist_unit(WORD CardNo,WORD axis,double dist);
DMC_API short __stdcall dmc_get_dec_stop_dist_unit(WORD CardNo,WORD axis,double *dist);
//����ٶ���������(���嵱��)
DMC_API short __stdcall dmc_set_profile_limit_unit(WORD CardNo,WORD axis,double Limit_Max_Vel,double Limit_Max_Acc,double EvenTime);
DMC_API short __stdcall dmc_get_profile_limit_unit(WORD CardNo,WORD axis,double* Limit_Max_Vel,double* Limit_MAX_Acc,double* EvenTime);
DMC_API short __stdcall dmc_set_vector_profile_limit_unit(WORD CardNo,WORD axis,double Limit_Max_Vel,double Limit_Max_Acc,double EvenTime);
DMC_API short __stdcall dmc_get_vector_profile_limit_unit(WORD CardNo,WORD axis,double* Limit_Max_Vel,double* Limit_MAX_Acc,double* EvenTime);
//���õ������ٹ���
DMC_API short __stdcall dmc_set_vector_profile_limit_by_axis(WORD CardNo,WORD Crd,WORD Enable);	
DMC_API short __stdcall dmc_get_vector_profile_limit_by_axis(WORD CardNo,WORD Crd,WORD* Enable);
DMC_API short __stdcall dmc_get_axis_follow_line_enable(WORD CardNo,WORD Crd,WORD * enable_flag);

DMC_API short __stdcall dmc_set_counter_reverse(WORD CardNo,WORD axis,WORD reverse);
DMC_API short __stdcall dmc_get_counter_reverse(WORD CardNo,WORD axis,WORD *reverse);
DMC_API short __stdcall dmc_set_extra_counter_reverse(WORD CardNo,WORD axis,WORD reverse);
DMC_API short __stdcall dmc_get_extra_counter_reverse(WORD CardNo,WORD axis,WORD *reverse);

DMC_API	short __stdcall dmc_conti_stop_axis(WORD CardNo, WORD Crd, WORD axis, double dec, int imark);

//��ȡ�岹����
DMC_API short __stdcall dmc_read_vector_length_unit(WORD CardNo,WORD Crd, double* total_length, double* left_length);
/*********************************************************************************************************
���׵���͹���˶�
*********************************************************************************************************/
DMC_API short __stdcall dmc_cam_table_unit(WORD CardNo,WORD MasterAxisNo,WORD SlaveAxisNo,DWORD Count,double *pMasterPos,double *pSlavePos,WORD SrcMode);
DMC_API short __stdcall dmc_cam_move(WORD CardNo,WORD axis);
DMC_API short __stdcall dmc_cam_move_cycle(WORD CardNo,WORD axis);

DMC_API short __stdcall dmc_conti_set_fairing_enable(WORD CardNo,WORD Crd, WORD enable, double fairing_length);
DMC_API short __stdcall dmc_conti_get_fairing_enable(WORD CardNo,WORD Crd, WORD * enable, double * fairing_length);

DMC_API short __stdcall dmc_set_eth_timeout(int timems);

DMC_API short __stdcall dmc_set_extra_encoder_extern(WORD CardNo,WORD channel, int pos);
DMC_API short __stdcall dmc_get_extra_encoder_extern(WORD CardNo,WORD channel, int * pos);

#ifdef __cplusplus
}
#endif

#endif