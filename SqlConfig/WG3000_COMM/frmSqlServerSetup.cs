using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WG3000_COMM.CoreAA;
using WG3000_COMM.Properties;
using WG3000_COMM.ResStrings;
using System.Diagnostics;

namespace WG3000_COMM
{
    public partial class frmSqlServerSetup : Form
    {
        private bool bSqlExress;
        private static float dbVersionNewest = 75f;
        private const string defaultDBFileName = "SqlDB.sql";
		private string defaultDBFileNameSqlString = "\r\nexec sp_dboption N'AccessData', N'autoclose', N'false'\r\nGO\r\n\r\nexec sp_dboption N'AccessData', N'bulkcopy', N'false'\r\nGO\r\n\r\nexec sp_dboption N'AccessData', N'trunc. log', N'true'\r\nGO\r\n\r\nexec sp_dboption N'AccessData', N'torn page detection', N'true'\r\nGO\r\n\r\nexec sp_dboption N'AccessData', N'read only', N'false'\r\nGO\r\n\r\nexec sp_dboption N'AccessData', N'dbo use', N'false'\r\nGO\r\n\r\nexec sp_dboption N'AccessData', N'single', N'false'\r\nGO\r\n\r\nexec sp_dboption N'AccessData', N'autoshrink', N'true'\r\nGO\r\n\r\nexec sp_dboption N'AccessData', N'ANSI null default', N'false'\r\nGO\r\n\r\nexec sp_dboption N'AccessData', N'recursive triggers', N'false'\r\nGO\r\n\r\nexec sp_dboption N'AccessData', N'ANSI nulls', N'false'\r\nGO\r\n\r\nexec sp_dboption N'AccessData', N'concat null yields null', N'false'\r\nGO\r\n\r\nexec sp_dboption N'AccessData', N'cursor close on commit', N'false'\r\nGO\r\n\r\nexec sp_dboption N'AccessData', N'default to local cursor', N'false'\r\nGO\r\n\r\nexec sp_dboption N'AccessData', N'quoted identifier', N'false'\r\nGO\r\n\r\nexec sp_dboption N'AccessData', N'ANSI warnings', N'false'\r\nGO\r\n\r\nexec sp_dboption N'AccessData', N'auto create statistics', N'true'\r\nGO\r\n\r\nexec sp_dboption N'AccessData', N'auto update statistics', N'true'\r\nGO\r\n\r\nif( (@@microsoftversion / power(2, 24) = 8) and (@@microsoftversion & 0xffff >= 724) )\r\n\texec sp_dboption N'AccessData', N'db chaining', N'false'\r\nGO\r\n\r\nuse [AccessData]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_a_Attendence]    脚本日期: 2010-06-21 14:28:12 ******/\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_a_Attendence]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_a_Attendence]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_a_Holiday]    脚本日期: 2010-06-21 14:28:12 ******/\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_a_Holiday]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_a_Holiday]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_a_HolidayType]    脚本日期: 2010-06-21 14:28:12 ******/\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_a_HolidayType]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_a_HolidayType]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_a_Shift_Attendence]    脚本日期: 2010-06-21 14:28:12 ******/\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_a_Shift_Attendence]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_a_Shift_Attendence]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_a_SystemParam]    脚本日期: 2010-06-21 14:28:12 ******/\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_a_SystemParam]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_a_SystemParam]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_b_Consumer]    脚本日期: 2010-06-21 14:28:12 ******/\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_b_Consumer]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_b_Consumer]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_b_Consumer_Other]    脚本日期: 2010-06-21 14:28:12 ******/\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_b_Consumer_Other]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_b_Consumer_Other]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_b_ControlSeg]    脚本日期: 2010-06-21 14:28:12 ******/\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_b_ControlSeg]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_b_ControlSeg]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_b_Controller]    脚本日期: 2010-06-21 14:28:12 ******/\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_b_Controller]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_b_Controller]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_b_ControllerTaskList]    脚本日期: 2010-06-21 14:28:12 ******/\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_b_ControllerTaskList]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_b_ControllerTaskList]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_b_Controller_Zone]    脚本日期: 2010-06-21 14:28:12 ******/\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_b_Controller_Zone]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_b_Controller_Zone]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_b_Door]    脚本日期: 2010-06-21 14:28:12 ******/\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_b_Door]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_b_Door]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_b_Group]    脚本日期: 2010-06-21 14:28:12 ******/\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_b_Group]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_b_Group]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_b_IDCard_Lost]    脚本日期: 2010-06-21 14:28:12 ******/\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_b_IDCard_Lost]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_b_IDCard_Lost]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_b_Reader]    脚本日期: 2010-06-21 14:28:12 ******/\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_b_Reader]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_b_Reader]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_b_ReaderPassword]    脚本日期: 2010-06-21 14:28:12 ******/\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_b_ReaderPassword]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_b_ReaderPassword]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_b_ShiftSet]    脚本日期: 2010-06-21 14:28:12 ******/\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_b_ShiftSet]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_b_ShiftSet]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_d_AttStatistic]    脚本日期: 2010-06-21 14:28:12 ******/\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_d_AttStatistic]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_d_AttStatistic]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_d_AttendenceData]    脚本日期: 2010-06-21 14:28:12 ******/\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_d_AttendenceData]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_d_AttendenceData]\r\nGO\r\n\r\n/****** Object:  Table [dbo].[t_d_AuthModes]    Script Date: 2013-11-21 16:50:24 ******/\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_d_AuthModes]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_d_AuthModes]\r\nGO\r\n\r\n/****** Object:  Table [dbo].[t_d_FaceTempl]    Script Date: 2014-4-18 15:24:25 ******/\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_d_FaceTempl]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_d_FaceTempl]\r\nGO\r\n\r\n/****** Object:  Table [dbo].[t_d_FaceTemplChanged]    Script Date: 2014-4-18 15:24:25 ******/\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_d_FaceTemplChanged]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_d_FaceTemplChanged]\r\nGO\r\n\r\n/****** Object:  Table [dbo].[t_d_FpTempl]    Script Date: 2013-11-21 16:50:24 ******/\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_d_FpTempl]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_d_FpTempl]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_d_Leave]    脚本日期: 2010-06-21 14:28:12 ******/\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_d_Leave]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_d_Leave]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_d_ManualCardRecord]    脚本日期: 2010-06-21 14:28:12 ******/\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_d_ManualCardRecord]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_d_ManualCardRecord]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_d_Privilege]    脚本日期: 2010-06-21 14:28:12 ******/\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_d_Privilege]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_d_Privilege]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_d_ShiftData]    脚本日期: 2010-06-21 14:28:12 ******/\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_d_ShiftData]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_d_ShiftData]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_d_Shift_Work_Schedule]    脚本日期: 2010-06-21 14:28:12 ******/\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_d_Shift_Work_Schedule]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_d_Shift_Work_Schedule]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_d_SwipeRecord]    脚本日期: 2010-06-21 14:28:12 ******/\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_d_SwipeRecord]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_d_SwipeRecord]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_d_doorFirstCardUsers]    脚本日期: 2010-06-21 14:28:12 ******/\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_d_doorFirstCardUsers]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_d_doorFirstCardUsers]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_d_doorMoreCardsUsers]    脚本日期: 2010-06-21 14:28:12 ******/\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_d_doorMoreCardsUsers]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_d_doorMoreCardsUsers]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_d_shift_AttReport]    脚本日期: 2010-06-21 14:28:12 ******/\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_d_shift_AttReport]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_d_shift_AttReport]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_d_shift_AttStatistic]    脚本日期: 2010-06-21 14:28:12 ******/\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_d_shift_AttStatistic]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_d_shift_AttStatistic]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_s_Operator]    脚本日期: 2010-06-21 14:28:12 ******/\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_s_Operator]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_s_Operator]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_s_OperatorPrivilege]    脚本日期: 2010-06-21 14:28:12 ******/\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_s_OperatorPrivilege]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_s_OperatorPrivilege]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_s_wglog]    脚本日期: 2010-06-21 14:28:12 ******/\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_s_wglog]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_s_wglog]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_d_Privilege]    脚本日期: 2010-05-31 13:39:40 ******/\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_d_Privilege]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_d_Privilege]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_a_Attendence]    脚本日期: 2010-06-21 14:28:14 ******/\r\nCREATE TABLE [dbo].[t_a_Attendence] (\r\n\t[f_No] [int] NOT NULL ,\r\n\t[f_Name] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_EName] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_Value] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_Notes] [ntext] COLLATE Chinese_PRC_CI_AS NULL \r\n) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_a_Holiday]    脚本日期: 2010-06-21 14:28:14 ******/\r\nCREATE TABLE [dbo].[t_a_Holiday] (\r\n\t[f_NO] [int] IDENTITY (1, 1) NOT NULL ,\r\n\t[f_Name] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_EName] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_Value] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_Value1] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_Value2] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_Value3] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_Type] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_Note] [ntext] COLLATE Chinese_PRC_CI_AS NULL \r\n) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_a_HolidayType]    脚本日期: 2010-06-21 14:28:14 ******/\r\nCREATE TABLE [dbo].[t_a_HolidayType] (\r\n\t[f_No] [int] IDENTITY (1, 1) NOT NULL ,\r\n\t[f_HolidayType] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL \r\n) ON [PRIMARY]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_a_Shift_Attendence]    脚本日期: 2010-06-21 14:28:14 ******/\r\nCREATE TABLE [dbo].[t_a_Shift_Attendence] (\r\n\t[f_NO] [int] NOT NULL ,\r\n\t[f_EName] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_Value] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_Notes] [ntext] COLLATE Chinese_PRC_CI_AS NULL \r\n) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_a_SystemParam]    脚本日期: 2010-06-21 14:28:14 ******/\r\nCREATE TABLE [dbo].[t_a_SystemParam] (\r\n\t[f_NO] [int] NOT NULL ,\r\n\t[f_Name] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_EName] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_Value] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_Notes] [ntext] COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_Modified] [datetime] NOT NULL \r\n) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_b_Consumer]    脚本日期: 2010-06-21 14:28:15 ******/\r\nCREATE TABLE [dbo].[t_b_Consumer] (\r\n\t[f_ConsumerID] [int] IDENTITY (1, 1) NOT FOR REPLICATION  NOT NULL ,\r\n\t[f_ConsumerNO] [nvarchar] (20) COLLATE Chinese_PRC_CI_AS NOT NULL ,\r\n\t[f_ConsumerName] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,\r\n\t[f_CardNO] [bigint] NULL ,\r\n\t[f_GroupID] [int] NOT NULL ,\r\n\t[f_AttendEnabled] [tinyint] NOT NULL ,\r\n\t[f_ShiftEnabled] [tinyint] NOT NULL ,\r\n\t[f_DoorEnabled] [tinyint] NOT NULL ,\r\n\t[f_BeginYMD] [datetime] NOT NULL ,\r\n\t[f_EndYMD] [datetime] NOT NULL ,\r\n\t[f_PIN] [int] NULL \r\n) ON [PRIMARY]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_b_Consumer_Other]    脚本日期: 2010-06-21 14:28:15 ******/\r\nCREATE TABLE [dbo].[t_b_Consumer_Other] (\r\n\t[f_ConsumerID] [int] NOT NULL ,\r\n\t[f_Note] [ntext] COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_WorkNo] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_Title] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_Culture] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_Hometown] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_Birthday] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_Marriage] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_JoinDate] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_LeaveDate] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_CertificateType] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_CertificateID] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SocialInsuranceNo] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_Addr] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_Postcode] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_Sex] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_Nationality] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_Religion] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_EnglishName] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_Mobile] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_HomePhone] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_Telephone] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_Email] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_Political] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_CorporationName] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_TechGrade] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL \r\n) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_b_ControlSeg]    脚本日期: 2010-06-21 14:28:15 ******/\r\nCREATE TABLE [dbo].[t_b_ControlSeg] (\r\n\t[f_ControlSegID] [int] NOT NULL ,\r\n\t[f_ControlSegName] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_DateControl] [tinyint] NULL ,\r\n\t[f_Monday] [tinyint] NOT NULL ,\r\n\t[f_Tuesday] [tinyint] NOT NULL ,\r\n\t[f_Wednesday] [tinyint] NOT NULL ,\r\n\t[f_Thursday] [tinyint] NOT NULL ,\r\n\t[f_Friday] [tinyint] NOT NULL ,\r\n\t[f_Saturday] [tinyint] NOT NULL ,\r\n\t[f_Sunday] [tinyint] NOT NULL ,\r\n\t[f_BeginHMS1] [datetime] NOT NULL ,\r\n\t[f_EndHMS1] [datetime] NOT NULL ,\r\n\t[f_BeginHMS2] [datetime] NOT NULL ,\r\n\t[f_EndHMS2] [datetime] NOT NULL ,\r\n\t[f_BeginHMS3] [datetime] NOT NULL ,\r\n\t[f_EndHMS3] [datetime] NOT NULL ,\r\n\t[f_BeginYMD] [datetime] NOT NULL ,\r\n\t[f_EndYMD] [datetime] NOT NULL ,\r\n\t[f_ControlSegIDLinked] [int] NOT NULL ,\r\n\t[f_ReaderCount] [tinyint] NOT NULL ,\r\n\t[f_LimitedTimesOfDay] [int] NOT NULL ,\r\n\t[f_LimitedTimesOfHMS1] [int] NOT NULL ,\r\n\t[f_LimitedTimesOfHMS2] [int] NOT NULL ,\r\n\t[f_LimitedTimesOfHMS3] [int] NOT NULL,\r\n    [f_ControlByHoliday] INT  NOT NULL DEFAULT 1 \r\n) ON [PRIMARY]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_b_Controller]    脚本日期: 2010-06-21 14:28:15 ******/\r\nCREATE TABLE [dbo].[t_b_Controller] (\r\n\t[f_ControllerID] [int] IDENTITY (1, 1) NOT FOR REPLICATION  NOT NULL ,\r\n\t[f_ControllerNO] [int] NOT NULL ,\r\n\t[f_ControllerSN] [int] NOT NULL ,\r\n\t[f_Enabled] [tinyint] NOT NULL ,\r\n\t[f_IP] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_PORT] [int] NOT NULL ,\r\n\t[f_Note] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_DoorNames] [nvarchar] (100) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_ZoneID] [int] NOT NULL ,\r\n\t[f_ControllerWiFiSSID] [nvarchar] (32) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_ControllerWiFiKey] [nvarchar] (64) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_ControllerWiFiEnable] [int] NULL ,\r\n\t[f_ControllerWiFiIP] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_ControllerWiFiMask] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_ControllerWiFiGateway] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_ControllerWiFiPort] [int] NULL ,\r\n\t[f_RtCamera] [int] NULL ,\r\n\t[f_TouchSensor] [int] NULL ,\r\n\t[f_M1Card] [int] NULL ,\r\n\t[f_Volume] [int] NULL ,\r\n\t[f_FpIdent] [int] NULL ,\r\n\t[f_FaceIdent] [int] NULL ,\r\n\t[f_AntiBack] [int] NOT NULL ,\r\n\t[f_InterLock] [int] NOT NULL ,\r\n\t[f_MoreCards_GoInOut] [int] NOT NULL ,\r\n\t[f_DoorInvalidOpen] [int] NOT NULL ,\r\n\t[f_DoorOpenTooLong] [int] NOT NULL ,\r\n\t[f_ForceWarn] [int] NOT NULL ,\r\n\t[f_InvalidCardWarn] [int] NOT NULL ,\r\n\t[f_DoorTamperWarn] [int] NOT NULL ,\r\n\t[f_PeripheralControl] [nvarchar] (500) NULL, \r\n\t[f_lastDelAddDateTime] [nvarchar](50) NULL,\r\n\t[f_lastDelAddConsuemrsTotal] [int] NOT NULL,\r\n\t[f_lastDelAddAndUploadDateTime] [nvarchar](50) NULL,\r\n\t[f_lastDelAddAndUploadConsuemrsTotal] [int] NOT NULL,\r\n\t[f_lastConsoleUploadDateTime] [nvarchar](50) NULL,\r\n\t[f_lastConsoleUploadConsuemrsTotal] [int] NOT NULL,\r\n\t[f_lastConsoleUploadPrivilege] [int] NOT NULL,\r\n\t[f_lastConsoleUploadValidPrivilege] [int] NOT NULL\r\n) ON [PRIMARY]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_b_ControllerTaskList]    脚本日期: 2010-06-21 14:28:15 ******/\r\nCREATE TABLE [dbo].[t_b_ControllerTaskList] (\r\n\t[f_Id] [int] IDENTITY (1, 1) NOT NULL ,\r\n\t[f_BeginYMD] [datetime] NOT NULL ,\r\n\t[f_EndYMD] [datetime] NOT NULL ,\r\n\t[f_OperateTime] [datetime] NOT NULL ,\r\n\t[f_Monday] [tinyint] NOT NULL ,\r\n\t[f_Tuesday] [tinyint] NOT NULL ,\r\n\t[f_Wednesday] [tinyint] NOT NULL ,\r\n\t[f_Thursday] [tinyint] NOT NULL ,\r\n\t[f_Friday] [tinyint] NOT NULL ,\r\n\t[f_Saturday] [tinyint] NOT NULL ,\r\n\t[f_Sunday] [tinyint] NOT NULL ,\r\n\t[f_DoorID] [int] NOT NULL ,\r\n\t[f_DoorControl] [int] NOT NULL ,\r\n\t[f_Notes] [ntext] COLLATE Chinese_PRC_CI_AS NULL \r\n) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_b_Controller_Zone]    脚本日期: 2010-06-21 14:28:15 ******/\r\nCREATE TABLE [dbo].[t_b_Controller_Zone] (\r\n\t[f_ZoneID] [int] IDENTITY (1, 1) NOT NULL ,\r\n\t[f_ZoneName] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL, \r\n    [f_ZoneNO] [int]  NOT NULL DEFAULT 0 \r\n) ON [PRIMARY]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_b_Door]    脚本日期: 2010-06-21 14:28:15 ******/\r\nCREATE TABLE [dbo].[t_b_Door] (\r\n\t[f_DoorID] [int] IDENTITY (1, 1) NOT FOR REPLICATION  NOT NULL ,\r\n\t[f_ControllerID] [int] NOT NULL ,\r\n\t[f_DoorNO] [tinyint] NOT NULL ,\r\n\t[f_DoorName] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,\r\n\t[f_DoorControl] [int] NOT NULL ,\r\n\t[f_DoorDelay] [int] NOT NULL ,\r\n\t[f_DoorEnabled] [int] NOT NULL ,\r\n\t[f_MoreCards_Total] [int] NOT NULL ,\r\n\t[f_MoreCards_Grp1] [int] NOT NULL ,\r\n\t[f_MoreCards_Grp2] [int] NOT NULL ,\r\n\t[f_MoreCards_Grp3] [int] NOT NULL ,\r\n\t[f_MoreCards_Grp4] [int] NOT NULL ,\r\n\t[f_MoreCards_Grp5] [int] NOT NULL ,\r\n\t[f_MoreCards_Grp6] [int] NOT NULL ,\r\n\t[f_MoreCards_Grp7] [int] NOT NULL ,\r\n\t[f_MoreCards_Grp8] [int] NOT NULL ,\r\n\t[f_MoreCards_Option] [int] NOT NULL ,\r\n\t[f_FirstCard_Enabled] [int] NOT NULL ,\r\n\t[f_FirstCard_Weekday] [int] NOT NULL ,\r\n\t[f_FirstCard_BeginHMS] [nvarchar] (100) COLLATE Chinese_PRC_CI_AS NOT NULL ,\r\n\t[f_FirstCard_BeginControl] [int] NOT NULL ,\r\n\t[f_FirstCard_EndHMS] [nvarchar] (100) COLLATE Chinese_PRC_CI_AS NOT NULL ,\r\n\t[f_FirstCard_EndControl] [int] NOT NULL \r\n) ON [PRIMARY]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_b_Group]    脚本日期: 2010-06-21 14:28:15 ******/\r\nCREATE TABLE [dbo].[t_b_Group] (\r\n\t[f_GroupID] [int] IDENTITY (1, 1) NOT FOR REPLICATION  NOT NULL ,\r\n\t[f_GroupName] [nvarchar] (255) COLLATE Chinese_PRC_CI_AS NOT NULL ,\r\n\t[f_GroupNO] [int] NOT NULL \r\n) ON [PRIMARY]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_b_IDCard_Lost]    脚本日期: 2010-06-21 14:28:15 ******/\r\nCREATE TABLE [dbo].[t_b_IDCard_Lost] (\r\n\t[f_CardNO] [bigint] NOT NULL ,\r\n\t[f_ConsumerID] [int] NOT NULL ,\r\n\t[f_Modified] [datetime] NOT NULL \r\n) ON [PRIMARY]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_b_Reader]    脚本日期: 2010-06-21 14:28:16 ******/\r\nCREATE TABLE [dbo].[t_b_Reader] (\r\n\t[f_ReaderID] [int] IDENTITY (1, 1) NOT FOR REPLICATION  NOT NULL ,\r\n\t[f_ControllerID] [int] NOT NULL ,\r\n\t[f_ReaderNO] [int] NOT NULL ,\r\n\t[f_ReaderName] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_AuthenticationMode] [int] NOT NULL ,\r\n\t[f_Attend] [int] NOT NULL ,\r\n\t[f_Patrol] [int] NOT NULL ,\r\n\t[f_DutyOnOff] [int] NOT NULL ,\r\n\t[f_InputCardno_Enabled] [int] NOT NULL \r\n) ON [PRIMARY]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_b_ReaderPassword]    脚本日期: 2010-06-21 14:28:16 ******/\r\nCREATE TABLE [dbo].[t_b_ReaderPassword] (\r\n\t[f_Id] [int] IDENTITY (1, 1) NOT NULL ,\r\n\t[f_Password] [int] NULL ,\r\n\t[f_BAll] [int] NULL ,\r\n\t[f_ReaderID] [int] NULL ,\r\n\t[f_Notes] [ntext] COLLATE Chinese_PRC_CI_AS NULL \r\n) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_b_ShiftSet]    脚本日期: 2010-06-21 14:28:16 ******/\r\nCREATE TABLE [dbo].[t_b_ShiftSet] (\r\n\t[f_ShiftID] [int] NOT NULL ,\r\n\t[f_ShiftName] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_ReadTimes] [int] NULL ,\r\n\t[f_OnDuty1] [datetime] NULL ,\r\n\t[f_OffDuty1] [datetime] NULL ,\r\n\t[f_OnDuty2] [datetime] NULL ,\r\n\t[f_OffDuty2] [datetime] NULL ,\r\n\t[f_OnDuty3] [datetime] NULL ,\r\n\t[f_OffDuty3] [datetime] NULL ,\r\n\t[f_OnDuty4] [datetime] NULL ,\r\n\t[f_OffDuty4] [datetime] NULL ,\r\n\t[f_bOvertimeShift] [int] NULL ,\r\n\t[f_Notes] [ntext] COLLATE Chinese_PRC_CI_AS NULL \r\n) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_d_AttStatistic]    脚本日期: 2010-06-21 14:28:16 ******/\r\nCREATE TABLE [dbo].[t_d_AttStatistic] (\r\n\t[f_RecID] [int] IDENTITY (1, 1) NOT NULL ,\r\n\t[f_ConsumerID] [int] NULL ,\r\n\t[f_AttDateStart] [datetime] NULL ,\r\n\t[f_AttDateEnd] [datetime] NULL ,\r\n\t[f_DayShouldWork] [int] NOT NULL ,\r\n\t[f_DayRealWork] [int] NULL ,\r\n\t[f_TotalLate] [int] NOT NULL ,\r\n\t[f_TotalLeaveEarly] [int] NOT NULL ,\r\n\t[f_TotalOvertime] [numeric](18, 1) NOT NULL ,\r\n\t[f_TotalAbsenceDay] [numeric](8, 1) NOT NULL ,\r\n\t[f_TotalNotReadCard] [int] NULL ,\r\n\t[f_SpecialType1] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType2] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType3] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType4] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType5] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType6] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType7] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType8] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType9] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType10] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType11] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType12] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType13] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType14] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType15] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType16] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType17] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType18] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType19] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType20] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType21] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType22] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType23] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType24] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType25] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType26] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType27] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType28] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType29] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType30] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType31] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType32] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_LateMinutes] [numeric](10, 0) NOT NULL ,\r\n\t[f_LeaveEarlyMinutes] [numeric](10, 0) NOT NULL ,\r\n\t[f_ManualReadTimesCount] [int] NOT NULL \r\n) ON [PRIMARY]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_d_AttendenceData]    脚本日期: 2010-06-21 14:28:16 ******/\r\nCREATE TABLE [dbo].[t_d_AttendenceData] (\r\n\t[f_RecID] [int] IDENTITY (1, 1) NOT NULL ,\r\n\t[f_ConsumerID] [int] NULL ,\r\n\t[f_AttDate] [datetime] NULL ,\r\n\t[f_Onduty1] [datetime] NULL ,\r\n\t[f_Onduty1Desc] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_Offduty1] [datetime] NULL ,\r\n\t[f_Offduty1Desc] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_Onduty2] [datetime] NULL ,\r\n\t[f_Onduty2Desc] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_Offduty2] [datetime] NULL ,\r\n\t[f_Offduty2Desc] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_LateTime] [int] NOT NULL ,\r\n\t[f_LeaveEarlyTime] [int] NOT NULL ,\r\n\t[f_OvertimeTime] [numeric](18, 1) NOT NULL ,\r\n\t[f_AbsenceDay] [numeric](8, 1) NOT NULL \r\n) ON [PRIMARY]\r\nGO\r\n\r\n/****** Object:  Table [dbo].[t_d_AuthModes]    Script Date: 2013-11-21 16:50:26 ******/\r\nCREATE TABLE [dbo].[t_d_AuthModes] (\r\n\t[f_AuthMode] [int] NOT NULL ,\r\n\t[f_AuthDesc] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_AuthDescCh] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_AuthDevType] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL \r\n) ON [PRIMARY]\r\nGO\r\n\r\n/****** Object:  Table [dbo].[t_d_FaceTempl]    Script Date: 2014-4-18 15:24:28 ******/\r\nCREATE TABLE [dbo].[t_d_FaceTempl] (\r\n\t[f_ConsumerID] [int] NOT NULL ,\r\n\t[f_Templ] [image] NULL \r\n) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]\r\nGO\r\n\r\n/****** Object:  Table [dbo].[t_d_FaceTemplChanged]    Script Date: 2014-4-18 15:24:28 ******/\r\nCREATE TABLE [dbo].[t_d_FaceTemplChanged] (\r\n\t[f_Changed] [int] NOT NULL DEFAULT(0)\r\n)\r\nGO\r\n\r\n/****** Object:  Table [dbo].[t_d_FpTempl]    Script Date: 2013-11-21 16:50:27 ******/\r\nCREATE TABLE [dbo].[t_d_FpTempl] (\r\n\t[f_ConsumerID] [int] NOT NULL ,\r\n\t[f_Finger] [int] NOT NULL ,\r\n\t[f_Templ] [varbinary] (1404) NOT NULL ,\r\n\t[f_Duress] [int] NOT NULL \r\n) ON [PRIMARY]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_d_Leave]    脚本日期: 2010-06-21 14:28:16 ******/\r\nCREATE TABLE [dbo].[t_d_Leave] (\r\n\t[f_NO] [int] IDENTITY (1, 1) NOT NULL ,\r\n\t[f_ConsumerID] [int] NULL ,\r\n\t[f_Value] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_Value1] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_Value2] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_Value3] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_HolidayType] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_Notes] [ntext] COLLATE Chinese_PRC_CI_AS NULL \r\n) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_d_ManualCardRecord]    脚本日期: 2010-06-21 14:28:16 ******/\r\nCREATE TABLE [dbo].[t_d_ManualCardRecord] (\r\n\t[f_ManualCardRecordID] [int] IDENTITY (1, 1) NOT NULL ,\r\n\t[f_CardNO] [bigint] NULL ,\r\n\t[f_ReadDate] [datetime] NULL ,\r\n\t[f_Character] [smallint] NULL ,\r\n\t[f_ControllerSN] [nvarchar] (8) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_ReaderID] [int] NULL ,\r\n\t[f_ConsumerID] [int] NULL ,\r\n\t[f_Note] [ntext] COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_DutyOnOff] [int] NOT NULL \r\n) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]\r\nGO\r\n\r\n\r\n\r\n/****** 对象: 表 [dbo].[t_d_ShiftData]    脚本日期: 2010-06-21 14:28:16 ******/\r\nCREATE TABLE [dbo].[t_d_ShiftData] (\r\n\t[f_RecID] [int] IDENTITY (1, 1) NOT NULL ,\r\n\t[f_ConsumerID] [int] NULL ,\r\n\t[f_DateYM] [nvarchar] (10) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_ShiftID_01] [int] NULL ,\r\n\t[f_ShiftID_02] [int] NULL ,\r\n\t[f_ShiftID_03] [int] NULL ,\r\n\t[f_ShiftID_04] [int] NULL ,\r\n\t[f_ShiftID_05] [int] NULL ,\r\n\t[f_ShiftID_06] [int] NULL ,\r\n\t[f_ShiftID_07] [int] NULL ,\r\n\t[f_ShiftID_08] [int] NULL ,\r\n\t[f_ShiftID_09] [int] NULL ,\r\n\t[f_ShiftID_10] [int] NULL ,\r\n\t[f_ShiftID_11] [int] NULL ,\r\n\t[f_ShiftID_12] [int] NULL ,\r\n\t[f_ShiftID_13] [int] NULL ,\r\n\t[f_ShiftID_14] [int] NULL ,\r\n\t[f_ShiftID_15] [int] NULL ,\r\n\t[f_ShiftID_16] [int] NULL ,\r\n\t[f_ShiftID_17] [int] NULL ,\r\n\t[f_ShiftID_18] [int] NULL ,\r\n\t[f_ShiftID_19] [int] NULL ,\r\n\t[f_ShiftID_20] [int] NULL ,\r\n\t[f_ShiftID_21] [int] NULL ,\r\n\t[f_ShiftID_22] [int] NULL ,\r\n\t[f_ShiftID_23] [int] NULL ,\r\n\t[f_ShiftID_24] [int] NULL ,\r\n\t[f_ShiftID_25] [int] NULL ,\r\n\t[f_ShiftID_26] [int] NULL ,\r\n\t[f_ShiftID_27] [int] NULL ,\r\n\t[f_ShiftID_28] [int] NULL ,\r\n\t[f_ShiftID_29] [int] NULL ,\r\n\t[f_ShiftID_30] [int] NULL ,\r\n\t[f_ShiftID_31] [int] NULL ,\r\n\t[f_LogDate] [datetime] NULL ,\r\n\t[f_Notes] [ntext] COLLATE Chinese_PRC_CI_AS NULL \r\n) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_d_Shift_Work_Schedule]    脚本日期: 2010-06-21 14:28:16 ******/\r\nCREATE TABLE [dbo].[t_d_Shift_Work_Schedule] (\r\n\t[f_RecID] [int] IDENTITY (1, 1) NOT NULL ,\r\n\t[f_ConsumerID] [int] NULL ,\r\n\t[f_ShiftDate] [datetime] NULL ,\r\n\t[f_ShiftID] [int] NULL ,\r\n\t[f_ReadTimes] [int] NULL ,\r\n\t[f_PlanTime] [datetime] NULL ,\r\n\t[f_TimeSeg] [int] NULL ,\r\n\t[f_WorkTime] [datetime] NULL ,\r\n\t[f_AttDesc] [nvarchar] (20) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_CardRecordDesc] [nvarchar] (20) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_Duration] [int] NULL ,\r\n\t[f_bOvertimeShift] [int] NULL \r\n) ON [PRIMARY]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_d_SwipeRecord]    脚本日期: 2010-06-21 14:28:16 ******/\r\nCREATE TABLE [dbo].[t_d_SwipeRecord] (\r\n\t[f_RecID] [int] IDENTITY (1, 1) NOT FOR REPLICATION  NOT NULL ,\r\n\t[f_ReadDate] [datetime] NOT NULL ,\r\n\t[f_ConsumerNO] [nvarchar] (20) COLLATE Chinese_PRC_CI_AS NOT NULL ,\r\n\t[f_ConsumerID] [int] NOT NULL ,\r\n\t[f_Character] [tinyint] NOT NULL ,\r\n\t[f_InOut] [tinyint] NOT NULL ,\r\n\t[f_VerifMode] [tinyint] NOT NULL ,\r\n\t[f_Status] [tinyint] NOT NULL ,\r\n\t[f_RecOption] [tinyint] NOT NULL ,\r\n\t[f_ControllerSN] [int] NOT NULL ,\r\n\t[f_ReaderID] [int] NOT NULL ,\r\n\t[f_ReaderNO] [tinyint] NOT NULL ,\r\n\t[f_RecordFlashLoc] [int] NOT NULL ,\r\n\t[f_Photo] [image] NULL ,\r\n\t[f_RecordAll] [nvarchar] (48) COLLATE Chinese_PRC_CI_AS NOT NULL ,\r\n\t[f_Modified] [datetime] NOT NULL \r\n) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_d_doorFirstCardUsers]    脚本日期: 2010-06-21 14:28:16 ******/\r\nCREATE TABLE [dbo].[t_d_doorFirstCardUsers] (\r\n\t[f_doorFirstCardUsersId] [int] IDENTITY (1, 1) NOT NULL ,\r\n\t[f_DoorID] [int] NOT NULL ,\r\n\t[f_ConsumerID] [int] NOT NULL \r\n) ON [PRIMARY]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_d_doorMoreCardsUsers]    脚本日期: 2010-06-21 14:28:17 ******/\r\nCREATE TABLE [dbo].[t_d_doorMoreCardsUsers] (\r\n\t[f_doorMoreCardsUsersId] [int] IDENTITY (1, 1) NOT NULL ,\r\n\t[f_DoorID] [int] NOT NULL ,\r\n\t[f_MoreCards_GrpID] [int] NOT NULL ,\r\n\t[f_ConsumerID] [int] NOT NULL \r\n) ON [PRIMARY]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_d_shift_AttReport]    脚本日期: 2010-06-21 14:28:17 ******/\r\nCREATE TABLE [dbo].[t_d_shift_AttReport] (\r\n\t[f_RecID] [int] IDENTITY (1, 1) NOT NULL ,\r\n\t[f_ConsumerID] [int] NULL ,\r\n\t[f_ShiftDate] [datetime] NULL ,\r\n\t[f_ShiftID] [int] NULL ,\r\n\t[f_ReadTimes] [int] NULL ,\r\n\t[f_OnDuty1] [datetime] NULL ,\r\n\t[f_OnDuty1AttDesc] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_OnDuty1CardRecordDesc] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_OffDuty1] [datetime] NULL ,\r\n\t[f_OffDuty1AttDesc] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_OffDuty1CardRecordDesc] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_OnDuty2] [datetime] NULL ,\r\n\t[f_OnDuty2AttDesc] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_OnDuty2CardRecordDesc] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_OffDuty2] [datetime] NULL ,\r\n\t[f_OffDuty2AttDesc] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_OffDuty2CardRecordDesc] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_OnDuty3] [datetime] NULL ,\r\n\t[f_OnDuty3AttDesc] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_OnDuty3CardRecordDesc] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_OffDuty3] [datetime] NULL ,\r\n\t[f_OffDuty3AttDesc] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_OffDuty3CardRecordDesc] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_OnDuty4] [datetime] NULL ,\r\n\t[f_OnDuty4AttDesc] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_OnDuty4CardRecordDesc] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_OffDuty4] [datetime] NULL ,\r\n\t[f_OffDuty4AttDesc] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_OffDuty4CardRecordDesc] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_LateMinutes] [numeric](10, 0) NULL ,\r\n\t[f_LeaveEarlyMinutes] [numeric](10, 0) NULL ,\r\n\t[f_OvertimeHours] [numeric](10, 1) NULL ,\r\n\t[f_AbsenceDays] [numeric](10, 1) NULL ,\r\n\t[f_NotReadCardCount] [int] NULL ,\r\n\t[f_bOvertimeShift] [int] NULL \r\n) ON [PRIMARY]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_d_shift_AttStatistic]    脚本日期: 2010-06-21 14:28:17 ******/\r\nCREATE TABLE [dbo].[t_d_shift_AttStatistic] (\r\n\t[f_RecID] [int] IDENTITY (1, 1) NOT NULL ,\r\n\t[f_ConsumerID] [int] NULL ,\r\n\t[f_AttDateStart] [datetime] NULL ,\r\n\t[f_AttDateEnd] [datetime] NULL ,\r\n\t[f_DayShouldWork] [int] NULL ,\r\n\t[f_LateMinutes] [numeric](10, 0) NULL ,\r\n\t[f_LateCount] [int] NULL ,\r\n\t[f_LeaveEarlyMinutes] [numeric](10, 0) NULL ,\r\n\t[f_LeaveEarlyCount] [int] NULL ,\r\n\t[f_OvertimeHours] [numeric](10, 1) NULL ,\r\n\t[f_AbsenceDays] [numeric](10, 1) NULL ,\r\n\t[f_NotReadCardCount] [int] NULL ,\r\n\t[f_ManualReadTimesCount] [int] NULL ,\r\n\t[f_SpecialType1] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType2] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType3] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType4] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType5] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType6] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType7] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType8] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType9] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType10] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType11] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType12] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType13] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType14] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType15] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType16] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType17] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType18] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType19] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType20] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType21] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType22] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType23] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType24] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType25] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType26] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType27] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType28] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType29] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType30] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType31] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_SpecialType32] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_DayRealWork] [int] NOT NULL \r\n) ON [PRIMARY]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_s_Operator]    脚本日期: 2010-06-21 14:28:17 ******/\r\nCREATE TABLE [dbo].[t_s_Operator] (\r\n\t[f_OperatorID] [int] IDENTITY (1, 1) NOT NULL ,\r\n\t[f_OperatorName] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,\r\n\t[f_Password] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_OperatePrivilege] [ntext] COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_Notes] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL \r\n) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_s_OperatorPrivilege]    脚本日期: 2010-06-21 14:28:17 ******/\r\nCREATE TABLE [dbo].[t_s_OperatorPrivilege] (\r\n\t[f_OperatorID] [int] NOT NULL ,\r\n\t[f_FunctionID] [int] NOT NULL ,\r\n\t[f_FunctionName] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,\r\n\t[f_FunctionDisplayName] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,\r\n\t[f_ReadOnly] [int] NOT NULL ,\r\n\t[f_FullControl] [int] NOT NULL \r\n) ON [PRIMARY]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_s_wglog]    脚本日期: 2010-06-21 14:28:17 ******/\r\nCREATE TABLE [dbo].[t_s_wglog] (\r\n\t[f_RecID] [int] IDENTITY (1, 1) NOT NULL ,\r\n\t[f_EventType] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_EventDesc] [ntext] COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_UserID] [int] NULL ,\r\n\t[f_UserName] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,\r\n\t[f_LogDateTime] [datetime] NULL \r\n) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]\r\nGO\r\n\r\n/****** 对象: 表 [dbo].[t_d_Privilege]    脚本日期: 2010-05-31 13:39:43 2012-5-16_07:47:53******/\r\nCREATE TABLE [dbo].[t_d_Privilege](\r\n\t[f_PrivilegeRecID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,\r\n\t[f_DoorID] [int] NOT NULL,\r\n\t[f_ControlSegID] [int] NOT NULL,\r\n\t[f_ConsumerID] [int] NOT NULL,\r\n\t[f_ControllerID] [int] NOT NULL,\r\n\t[f_DoorNO] [tinyint] NOT NULL\r\n)ON [PRIMARY] \r\nGO\r\n\r\n/****** 2012-5-16_07:47:53******/\r\nALTER TABLE [dbo].[t_d_Privilege] WITH NOCHECK ADD \r\n\tCONSTRAINT [PK_t_d_Privilege] PRIMARY KEY  CLUSTERED \r\n\t(\r\n\t[f_ControllerID] ASC,\r\n\t[f_PrivilegeRecID] ASC\r\n\t)  ON [PRIMARY] \r\nGO\r\n\r\n/****** 2012-5-16_07:47:53******/\r\nCREATE NONCLUSTERED INDEX [_dta_index_t_d_Privilege_12_1810105489__K4_1_2_3_5] ON [dbo].[t_d_Privilege] \r\n(\r\n\t[f_ConsumerID] ASC\r\n) ON [PRIMARY]\r\nGO\r\n\r\nALTER TABLE [dbo].[t_a_Holiday] WITH NOCHECK ADD \r\n\tCONSTRAINT [PK_t_a_Holiday] PRIMARY KEY  CLUSTERED \r\n\t(\r\n\t\t[f_NO]\r\n\t)  ON [PRIMARY] \r\nGO\r\n\r\nALTER TABLE [dbo].[t_a_HolidayType] WITH NOCHECK ADD \r\n\tCONSTRAINT [PK_t_a_HolidayType] PRIMARY KEY  CLUSTERED \r\n\t(\r\n\t\t[f_No]\r\n\t)  ON [PRIMARY] \r\nGO\r\n\r\nALTER TABLE [dbo].[t_a_SystemParam] WITH NOCHECK ADD \r\n\tCONSTRAINT [PK_t_a_SystemParam] PRIMARY KEY  CLUSTERED \r\n\t(\r\n\t\t[f_NO]\r\n\t)  ON [PRIMARY] \r\nGO\r\n\r\nALTER TABLE [dbo].[t_b_ControlSeg] WITH NOCHECK ADD \r\n\tCONSTRAINT [PK_t_b_ControlSeg] PRIMARY KEY  CLUSTERED \r\n\t(\r\n\t\t[f_ControlSegID]\r\n\t)  ON [PRIMARY] \r\nGO\r\n\r\nALTER TABLE [dbo].[t_b_Controller] WITH NOCHECK ADD \r\n\tCONSTRAINT [PK_t_b_Controller] PRIMARY KEY  CLUSTERED \r\n\t(\r\n\t\t[f_ControllerID]\r\n\t)  ON [PRIMARY] \r\nGO\r\n\r\nALTER TABLE [dbo].[t_b_ControllerTaskList] WITH NOCHECK ADD \r\n\tCONSTRAINT [PK_t_b_TaskList] PRIMARY KEY  CLUSTERED \r\n\t(\r\n\t\t[f_Id]\r\n\t)  ON [PRIMARY] \r\nGO\r\n\r\nALTER TABLE [dbo].[t_b_Controller_Zone] WITH NOCHECK ADD \r\n\tCONSTRAINT [PK_t_b_Controller_Zone] PRIMARY KEY  CLUSTERED \r\n\t(\r\n\t\t[f_ZoneID]\r\n\t)  ON [PRIMARY] \r\nGO\r\n\r\nALTER TABLE [dbo].[t_b_Door] WITH NOCHECK ADD \r\n\tCONSTRAINT [PK_t_b_Door] PRIMARY KEY  CLUSTERED \r\n\t(\r\n\t\t[f_DoorID]\r\n\t)  ON [PRIMARY] \r\nGO\r\n\r\nALTER TABLE [dbo].[t_b_Group] WITH NOCHECK ADD \r\n\tCONSTRAINT [PK_t_b_Group] PRIMARY KEY  CLUSTERED \r\n\t(\r\n\t\t[f_GroupID]\r\n\t)  ON [PRIMARY] \r\nGO\r\n\r\nALTER TABLE [dbo].[t_b_IDCard_Lost] WITH NOCHECK ADD \r\n\tCONSTRAINT [PK_t_b_IDCard_Lost] PRIMARY KEY  CLUSTERED \r\n\t(\r\n\t\t[f_CardNO]\r\n\t)  ON [PRIMARY] \r\nGO\r\n\r\nALTER TABLE [dbo].[t_b_Reader] WITH NOCHECK ADD \r\n\tCONSTRAINT [PK_t_b_Reader] PRIMARY KEY  CLUSTERED \r\n\t(\r\n\t\t[f_ReaderID]\r\n\t)  ON [PRIMARY] \r\nGO\r\n\r\nALTER TABLE [dbo].[t_b_ReaderPassword] WITH NOCHECK ADD \r\n\tCONSTRAINT [PK_t_b_ReaderPassword] PRIMARY KEY  CLUSTERED \r\n\t(\r\n\t\t[f_Id]\r\n\t)  ON [PRIMARY] \r\nGO\r\n\r\nALTER TABLE [dbo].[t_d_AttStatistic] WITH NOCHECK ADD \r\n\tCONSTRAINT [PK_t_d_AttStatistic] PRIMARY KEY  CLUSTERED \r\n\t(\r\n\t\t[f_RecID]\r\n\t)  ON [PRIMARY] \r\nGO\r\n\r\nALTER TABLE [dbo].[t_d_AttendenceData] WITH NOCHECK ADD \r\n\tCONSTRAINT [PK_t_d_AttendenceData] PRIMARY KEY  CLUSTERED \r\n\t(\r\n\t\t[f_RecID]\r\n\t)  ON [PRIMARY] \r\nGO\r\n\r\nALTER TABLE [dbo].[t_d_FaceTempl] WITH NOCHECK ADD \r\n\tCONSTRAINT [PK_t_d_FaceTempl] PRIMARY KEY  CLUSTERED \r\n\t(\r\n\t\t[f_ConsumerID]\r\n\t)  ON [PRIMARY] \r\nGO\r\n\r\nALTER TABLE [dbo].[t_d_FpTempl] WITH NOCHECK ADD \r\n\tCONSTRAINT [PK_t_d_FpTempl] PRIMARY KEY  CLUSTERED \r\n\t(\r\n\t\t[f_ConsumerID],\r\n\t\t[f_Finger]\r\n\t)  ON [PRIMARY] \r\nGO\r\n\r\nALTER TABLE [dbo].[t_d_Leave] WITH NOCHECK ADD \r\n\tCONSTRAINT [PK_t_d_Leave] PRIMARY KEY  CLUSTERED \r\n\t(\r\n\t\t[f_NO]\r\n\t)  ON [PRIMARY] \r\nGO\r\n\r\nALTER TABLE [dbo].[t_d_ManualCardRecord] WITH NOCHECK ADD \r\n\tCONSTRAINT [PK_t_d_ManualCardRecord] PRIMARY KEY  CLUSTERED \r\n\t(\r\n\t\t[f_ManualCardRecordID]\r\n\t)  ON [PRIMARY] \r\nGO\r\n\r\n\r\n\r\nALTER TABLE [dbo].[t_d_ShiftData] WITH NOCHECK ADD \r\n\tCONSTRAINT [PK_t_d_ShiftData] PRIMARY KEY  CLUSTERED \r\n\t(\r\n\t\t[f_RecID]\r\n\t)  ON [PRIMARY] \r\nGO\r\n\r\nALTER TABLE [dbo].[t_d_Shift_Work_Schedule] WITH NOCHECK ADD \r\n\tCONSTRAINT [PK_t_d_Shift_Work_Schedule] PRIMARY KEY  CLUSTERED \r\n\t(\r\n\t\t[f_RecID]\r\n\t)  ON [PRIMARY] \r\nGO\r\n\r\nALTER TABLE [dbo].[t_d_SwipeRecord] WITH NOCHECK ADD \r\n\tCONSTRAINT [PK_t_d_SwipeRecord] PRIMARY KEY  CLUSTERED \r\n\t(\r\n\t\t[f_RecID] DESC \r\n\t)  ON [PRIMARY] \r\nGO\r\n\r\nALTER TABLE [dbo].[t_d_doorFirstCardUsers] WITH NOCHECK ADD \r\n\tCONSTRAINT [PK_t_d_doorFirstCardUsers] PRIMARY KEY  CLUSTERED \r\n\t(\r\n\t\t[f_doorFirstCardUsersId]\r\n\t)  ON [PRIMARY] \r\nGO\r\n\r\nALTER TABLE [dbo].[t_d_doorMoreCardsUsers] WITH NOCHECK ADD \r\n\tCONSTRAINT [PK_t_d_doorMoreCardsUsers] PRIMARY KEY  CLUSTERED \r\n\t(\r\n\t\t[f_doorMoreCardsUsersId]\r\n\t)  ON [PRIMARY] \r\nGO\r\n\r\nALTER TABLE [dbo].[t_d_shift_AttReport] WITH NOCHECK ADD \r\n\tCONSTRAINT [PK_t_d_shift_AttReport] PRIMARY KEY  CLUSTERED \r\n\t(\r\n\t\t[f_RecID]\r\n\t)  ON [PRIMARY] \r\nGO\r\n\r\nALTER TABLE [dbo].[t_d_shift_AttStatistic] WITH NOCHECK ADD \r\n\tCONSTRAINT [PK_t_d_shift_AttStatistic] PRIMARY KEY  CLUSTERED \r\n\t(\r\n\t\t[f_RecID]\r\n\t)  ON [PRIMARY] \r\nGO\r\n\r\nALTER TABLE [dbo].[t_s_Operator] WITH NOCHECK ADD \r\n\tCONSTRAINT [PK_t_s_Operator] PRIMARY KEY  CLUSTERED \r\n\t(\r\n\t\t[f_OperatorID]\r\n\t)  ON [PRIMARY] \r\nGO\r\n\r\nALTER TABLE [dbo].[t_s_wglog] WITH NOCHECK ADD \r\n\tCONSTRAINT [PK_t_s_wglog] PRIMARY KEY  CLUSTERED \r\n\t(\r\n\t\t[f_RecID] DESC \r\n\t)  ON [PRIMARY] \r\nGO\r\n\r\n CREATE  UNIQUE  CLUSTERED  INDEX [IX_t_b_Consumer_ConsumerNO] ON [dbo].[t_b_Consumer]([f_ConsumerNO]) ON [PRIMARY]\r\nGO\r\n\r\nALTER TABLE [dbo].[t_a_SystemParam] ADD \r\n\tCONSTRAINT [DF_t_a_SystemParam_f_Modified] DEFAULT (getdate()) FOR [f_Modified]\r\nGO\r\n\r\nALTER TABLE [dbo].[t_b_Consumer] ADD \r\n\tCONSTRAINT [DF_t_b_Consumer_Common_f_GroupID] DEFAULT (0) FOR [f_GroupID],\r\n\tCONSTRAINT [DF_t_b_Consumer_Common_f_AttendEnabled] DEFAULT (1) FOR [f_AttendEnabled],\r\n\tCONSTRAINT [DF_t_b_Consumer_Common_f_ShiftEnabled] DEFAULT (0) FOR [f_ShiftEnabled],\r\n\tCONSTRAINT [DF_t_b_Consumer_Common_f_DoorEnabled] DEFAULT (1) FOR [f_DoorEnabled],\r\n\tCONSTRAINT [DF_t_b_Consumer_Common_f_BeginYMD] DEFAULT (convert(datetime,'2010-1-1 00:00:00',120)) FOR [f_BeginYMD],\r\n\tCONSTRAINT [DF_t_b_Consumer_Common_f_EndYMD] DEFAULT (convert(datetime,'2029-12-31 00:00:00',120)) FOR [f_EndYMD],\r\n\tCONSTRAINT [PK_t_b_Consumer] PRIMARY KEY  NONCLUSTERED \r\n\t(\r\n\t\t[f_ConsumerID]\r\n\t)  ON [PRIMARY] \r\nGO\r\n\r\n CREATE  INDEX [IX_t_b_Consumer_CardNO] ON [dbo].[t_b_Consumer]([f_CardNO]) ON [PRIMARY]\r\nGO\r\n\r\n CREATE  INDEX [IX_t_b_Consumer_ConsumerName] ON [dbo].[t_b_Consumer]([f_ConsumerName]) ON [PRIMARY]\r\nGO\r\n\r\nALTER TABLE [dbo].[t_b_ControlSeg] ADD \r\n\tCONSTRAINT [DF_t_b_ControlSeg_f_DateControl] DEFAULT (0) FOR [f_DateControl],\r\n\tCONSTRAINT [DF_t_b_ControlSeg_f_Monday] DEFAULT (1) FOR [f_Monday],\r\n\tCONSTRAINT [DF_t_b_ControlSeg_f_Tuesday] DEFAULT (1) FOR [f_Tuesday],\r\n\tCONSTRAINT [DF_t_b_ControlSeg_f_Wednesday] DEFAULT (1) FOR [f_Wednesday],\r\n\tCONSTRAINT [DF_t_b_ControlSeg_f_Thursday] DEFAULT (1) FOR [f_Thursday],\r\n\tCONSTRAINT [DF_t_b_ControlSeg_f_Friday] DEFAULT (1) FOR [f_Friday],\r\n\tCONSTRAINT [DF_t_b_ControlSeg_f_Saturday] DEFAULT (1) FOR [f_Saturday],\r\n\tCONSTRAINT [DF_t_b_ControlSeg_f_Sunday] DEFAULT (1) FOR [f_Sunday],\r\n\tCONSTRAINT [DF_t_b_ControlSeg_f_BeginHMS1] DEFAULT (convert(datetime,'2010-01-01 00:00:00',120)) FOR [f_BeginHMS1],\r\n\tCONSTRAINT [DF_t_b_ControlSeg_f_EndHMS1] DEFAULT (convert(datetime,'2010-01-01 00:00:00',120)) FOR [f_EndHMS1],\r\n\tCONSTRAINT [DF_t_b_ControlSeg_f_BeginHMS2] DEFAULT (convert(datetime,'2010-01-01 00:00:00',120)) FOR [f_BeginHMS2],\r\n\tCONSTRAINT [DF_t_b_ControlSeg_f_EndHMS2] DEFAULT (convert(datetime,'2010-01-01 00:00:00',120)) FOR [f_EndHMS2],\r\n\tCONSTRAINT [DF_t_b_ControlSeg_f_BeginHMS3] DEFAULT (convert(datetime,'2010-01-01 00:00:00',120)) FOR [f_BeginHMS3],\r\n\tCONSTRAINT [DF_t_b_ControlSeg_f_EndHMS3] DEFAULT (convert(datetime,'2010-01-01 00:00:00',120)) FOR [f_EndHMS3],\r\n\tCONSTRAINT [DF_t_b_ControlSeg_f_BeginYMD] DEFAULT (convert(datetime,'2010-01-01 00:00:00',120)) FOR [f_BeginYMD],\r\n\tCONSTRAINT [DF_t_b_ControlSeg_f_EndYMD] DEFAULT (convert(datetime,'2029-12-31 00:00:00',120)) FOR [f_EndYMD],\r\n\tCONSTRAINT [DF_t_b_ControlSeg_f_ControlSegIDLinked] DEFAULT (0) FOR [f_ControlSegIDLinked],\r\n\tCONSTRAINT [DF_t_b_ControlSeg_f_LimitedForReader] DEFAULT (0) FOR [f_ReaderCount],\r\n\tCONSTRAINT [DF_t_b_ControlSeg_f_LimitedTimesOfDay] DEFAULT (0) FOR [f_LimitedTimesOfDay],\r\n\tCONSTRAINT [DF_t_b_ControlSeg_f_LimitedTimesOfHMS1] DEFAULT (0) FOR [f_LimitedTimesOfHMS1],\r\n\tCONSTRAINT [DF_t_b_ControlSeg_f_LimitedTimesOfHMS2] DEFAULT (0) FOR [f_LimitedTimesOfHMS2],\r\n\tCONSTRAINT [DF_t_b_ControlSeg_f_LimitedTimesOfHMS21] DEFAULT (0) FOR [f_LimitedTimesOfHMS3]\r\nGO\r\n\r\nALTER TABLE [dbo].[t_b_Controller] ADD \r\n\tCONSTRAINT [DF_t_b_Controller_f_Enabled] DEFAULT (1) FOR [f_Enabled],\r\n\tCONSTRAINT [DF_t_b_Controller_f_PORT] DEFAULT (60000) FOR [f_PORT],\r\n\tCONSTRAINT [DF_t_b_Controller_f_ZoneID] DEFAULT (0) FOR [f_ZoneID],\r\n\tCONSTRAINT [DF_t_b_Controller_f_ControllerWiFiPort] DEFAULT (60000) FOR [f_ControllerWiFiPort],\r\n\tCONSTRAINT [DF_t_b_Controller_f_RtCamera] DEFAULT (0) FOR [f_RtCamera],\r\n\tCONSTRAINT [DF_t_b_Controller_f_TouchSensor] DEFAULT (0) FOR [f_TouchSensor],\r\n\tCONSTRAINT [DF_t_b_Controller_f_M1Card] DEFAULT (0) FOR [f_M1Card],\r\n\tCONSTRAINT [DF_t_b_Controller_f_Volume] DEFAULT (5) FOR [f_Volume],\r\n\tCONSTRAINT [DF_t_b_Controller_f_FpIdent] DEFAULT (0) FOR [f_FpIdent],\r\n\tCONSTRAINT [DF_t_b_Controller_f_FaceIdent] DEFAULT (0) FOR [f_FaceIdent],\r\n\tCONSTRAINT [DF_t_b_Controller_f_AntiBack] DEFAULT (0) FOR [f_AntiBack],\r\n\tCONSTRAINT [DF_t_b_Controller_f_InterLock] DEFAULT (0) FOR [f_InterLock],\r\n\tCONSTRAINT [DF_t_b_Controller_f_MoreCards_GoInOut_1] DEFAULT (85) FOR [f_MoreCards_GoInOut],\r\n\tCONSTRAINT [DF_t_b_Controller_f_DoorInvalidOpen_1] DEFAULT (0) FOR [f_DoorInvalidOpen],\r\n\tCONSTRAINT [DF_t_b_Controller_f_DoorOpenTooLong_1] DEFAULT (0) FOR [f_DoorOpenTooLong],\r\n\tCONSTRAINT [DF_t_b_Controller_f_ForceWarn_1] DEFAULT (0) FOR [f_ForceWarn],\r\n\tCONSTRAINT [DF_t_b_Controller_f_InvalidCardWarn_1] DEFAULT (0) FOR [f_InvalidCardWarn],\r\n\tCONSTRAINT [DF_t_b_Controller_f_f_DoorTamperWarn_1] DEFAULT (0) FOR [f_DoorTamperWarn],\r\n  CONSTRAINT [DF_t_b_Controller_f_lastDelAddConsuemrsTotal]  DEFAULT ((0)) FOR [f_lastDelAddConsuemrsTotal],\r\n  CONSTRAINT [DF_t_b_Controller_f_lastDelAddAndUploadConsuemrsTotal]  DEFAULT ((0)) FOR [f_lastDelAddAndUploadConsuemrsTotal],\r\n\tCONSTRAINT [DF_t_b_Controller_f_lastConsoleUploadConsuemrsTotal]  DEFAULT ((0)) FOR [f_lastConsoleUploadConsuemrsTotal],\r\n  CONSTRAINT [DF_t_b_Controller_f_lastConsoleUploadPrivilege]  DEFAULT ((0)) FOR [f_lastConsoleUploadPrivilege],\r\n  CONSTRAINT [DF_t_b_Controller_f_lastConsoleUploadValidPrivilege]  DEFAULT ((0)) FOR [f_lastConsoleUploadValidPrivilege]\r\nGO\r\n\r\n CREATE  UNIQUE  INDEX [IX_t_b_Controller_ControllerNO] ON [dbo].[t_b_Controller]([f_ControllerNO]) WITH  IGNORE_DUP_KEY  ON [PRIMARY]\r\nGO\r\n\r\n CREATE  UNIQUE  INDEX [IX_t_b_Controller_ControllerSN] ON [dbo].[t_b_Controller]([f_ControllerSN]) WITH  IGNORE_DUP_KEY  ON [PRIMARY]\r\nGO\r\n\r\nALTER TABLE [dbo].[t_b_ControllerTaskList] ADD \r\n\tCONSTRAINT [DF_t_b_ControllerTaskList_f_BeginYMD] DEFAULT (convert(datetime,'2010-01-01 00:00:00',120)) FOR [f_BeginYMD],\r\n\tCONSTRAINT [DF_t_b_ControllerTaskList_f_EndYMD] DEFAULT (convert(datetime,'2029-12-31 00:00:00',120)) FOR [f_EndYMD],\r\n\tCONSTRAINT [DF_t_b_ControllerTaskList_f_OperateTime] DEFAULT (convert(datetime,'2010-01-01 00:00:00',120)) FOR [f_OperateTime],\r\n\tCONSTRAINT [DF_t_b_ControllerTaskList_f_Monday] DEFAULT (1) FOR [f_Monday],\r\n\tCONSTRAINT [DF_t_b_ControllerTaskList_f_Tuesday] DEFAULT (1) FOR [f_Tuesday],\r\n\tCONSTRAINT [DF_t_b_ControllerTaskList_f_Wednesday] DEFAULT (1) FOR [f_Wednesday],\r\n\tCONSTRAINT [DF_t_b_ControllerTaskList_f_Thursday] DEFAULT (1) FOR [f_Thursday],\r\n\tCONSTRAINT [DF_t_b_ControllerTaskList_f_Friday] DEFAULT (1) FOR [f_Friday],\r\n\tCONSTRAINT [DF_t_b_ControllerTaskList_f_Saturday] DEFAULT (1) FOR [f_Saturday],\r\n\tCONSTRAINT [DF_t_b_ControllerTaskList_f_Sunday] DEFAULT (1) FOR [f_Sunday],\r\n\tCONSTRAINT [DF_t_b_TaskList_f_DoorID] DEFAULT (0) FOR [f_DoorID],\r\n\tCONSTRAINT [DF_t_b_ControllerTaskList_f_DoorControl] DEFAULT (0) FOR [f_DoorControl]\r\nGO\r\n\r\nALTER TABLE [dbo].[t_b_Controller_Zone] ADD \r\n\tCONSTRAINT [IX_t_b_Controller_Zone] UNIQUE  NONCLUSTERED \r\n\t(\r\n\t\t[f_ZoneName]\r\n\t)  ON [PRIMARY] \r\nGO\r\n\r\nALTER TABLE [dbo].[t_b_Door] ADD \r\n\tCONSTRAINT [DF_t_b_Door_f_DoorNO] DEFAULT (1) FOR [f_DoorNO],\r\n\tCONSTRAINT [DF_t_b_Door_f_DoorControl] DEFAULT (3) FOR [f_DoorControl],\r\n\tCONSTRAINT [DF_t_b_Door_f_DoorDelay] DEFAULT (3) FOR [f_DoorDelay],\r\n\tCONSTRAINT [DF_t_b_Door_f_DoorEnabled] DEFAULT (1) FOR [f_DoorEnabled],\r\n\tCONSTRAINT [DF_t_b_Door_f_MoreCards_Total_1] DEFAULT (0) FOR [f_MoreCards_Total],\r\n\tCONSTRAINT [DF_t_b_Door_f_MoreCards_Grp1_1] DEFAULT (0) FOR [f_MoreCards_Grp1],\r\n\tCONSTRAINT [DF_t_b_Door_f_MoreCards_Grp2_1] DEFAULT (0) FOR [f_MoreCards_Grp2],\r\n\tCONSTRAINT [DF_t_b_Door_f_MoreCards_Grp3_1] DEFAULT (0) FOR [f_MoreCards_Grp3],\r\n\tCONSTRAINT [DF_t_b_Door_f_MoreCards_Grp4_1] DEFAULT (0) FOR [f_MoreCards_Grp4],\r\n\tCONSTRAINT [DF_t_b_Door_f_MoreCards_Grp5_1] DEFAULT (0) FOR [f_MoreCards_Grp5],\r\n\tCONSTRAINT [DF_t_b_Door_f_MoreCards_Grp6_1] DEFAULT (0) FOR [f_MoreCards_Grp6],\r\n\tCONSTRAINT [DF_t_b_Door_f_MoreCards_Grp7_1] DEFAULT (0) FOR [f_MoreCards_Grp7],\r\n\tCONSTRAINT [DF_t_b_Door_f_MoreCards_Grp8_1] DEFAULT (0) FOR [f_MoreCards_Grp8],\r\n\tCONSTRAINT [DF_t_b_Door_f_MoreCards_Option_1] DEFAULT (0) FOR [f_MoreCards_Option],\r\n\tCONSTRAINT [DF_t_b_Door_f_FirstCard_Enabled_1] DEFAULT (0) FOR [f_FirstCard_Enabled],\r\n\tCONSTRAINT [DF_t_b_Door_f_FirstCard_Weekday_1] DEFAULT (127) FOR [f_FirstCard_Weekday],\r\n\tCONSTRAINT [DF_t_b_Door_f_FirstCard_BeginHMS_1] DEFAULT (N'2010-4-8 00:00:00') FOR [f_FirstCard_BeginHMS],\r\n\tCONSTRAINT [DF_t_b_Door_f_FirstCard_BeginControl_1] DEFAULT (0) FOR [f_FirstCard_BeginControl],\r\n\tCONSTRAINT [DF_t_b_Door_f_FirstCard_EndHMS_1] DEFAULT (N'2010-4-8 00:00:00') FOR [f_FirstCard_EndHMS],\r\n\tCONSTRAINT [DF_t_b_Door_f_FirstCard_EndControl_1] DEFAULT (0) FOR [f_FirstCard_EndControl]\r\nGO\r\n\r\n CREATE  UNIQUE  INDEX [IX_t_b_Door_DoorNO] ON [dbo].[t_b_Door]([f_ControllerID], [f_DoorNO]) WITH  IGNORE_DUP_KEY  ON [PRIMARY]\r\nGO\r\n\r\n CREATE  UNIQUE  INDEX [IX_t_b_DoorName] ON [dbo].[t_b_Door]([f_DoorName]) WITH  IGNORE_DUP_KEY  ON [PRIMARY]\r\nGO\r\n\r\nALTER TABLE [dbo].[t_b_Group] ADD \r\n\tCONSTRAINT [DF_t_b_Group_f_GroupNO] DEFAULT (0) FOR [f_GroupNO]\r\nGO\r\n\r\n CREATE  UNIQUE  INDEX [IX_t_b_GroupName] ON [dbo].[t_b_Group]([f_GroupName]) WITH  IGNORE_DUP_KEY  ON [PRIMARY]\r\nGO\r\n\r\nALTER TABLE [dbo].[t_b_IDCard_Lost] ADD \r\n\tCONSTRAINT [DF_t_b_IDCard_Lost_f_Modified] DEFAULT (getdate()) FOR [f_Modified]\r\nGO\r\n\r\nALTER TABLE [dbo].[t_b_Reader] ADD \r\n\tCONSTRAINT [DF_t_b_Reader_f_AuthenticationMode] DEFAULT (0) FOR [f_AuthenticationMode],\r\n\tCONSTRAINT [DF_t_b_Reader_f_Attend] DEFAULT (1) FOR [f_Attend],\r\n\tCONSTRAINT [DF_t_b_Reader_f_Patrol] DEFAULT (0) FOR [f_Patrol],\r\n\tCONSTRAINT [DF_t_b_Reader_f_InputCardno_Enabled] DEFAULT (0) FOR [f_InputCardno_Enabled]\r\nGO\r\n\r\n CREATE  UNIQUE  INDEX [IX_t_b_ReaderNO] ON [dbo].[t_b_Reader]([f_ControllerID], [f_ReaderNO]) WITH  IGNORE_DUP_KEY  ON [PRIMARY]\r\nGO\r\n\r\nALTER TABLE [dbo].[t_b_ReaderPassword] ADD \r\n\tCONSTRAINT [DF_t_b_ReaderPassword_f_Password] DEFAULT (0) FOR [f_Password],\r\n\tCONSTRAINT [DF_t_b_ReaderPassword_f_BAll] DEFAULT (1) FOR [f_BAll],\r\n\tCONSTRAINT [DF_t_b_ReaderPassword_f_ReaderID] DEFAULT (0) FOR [f_ReaderID]\r\nGO\r\n\r\nALTER TABLE [dbo].[t_d_AttStatistic] ADD \r\n\tCONSTRAINT [DF_t_d_AttStatistic_f_ConsumerID] DEFAULT (0) FOR [f_ConsumerID],\r\n\tCONSTRAINT [DF_t_d_AttStatistic_f_DayShouldWork] DEFAULT (0) FOR [f_DayShouldWork],\r\n\tCONSTRAINT [DF_t_d_AttStatistic_f_DayRealWork] DEFAULT (0) FOR [f_DayRealWork],\r\n\tCONSTRAINT [DF_t_d_AttStatistic_f_TotalLate] DEFAULT (0) FOR [f_TotalLate],\r\n\tCONSTRAINT [DF_t_d_AttStatistic_f_TotalLeaveEarly] DEFAULT (0) FOR [f_TotalLeaveEarly],\r\n\tCONSTRAINT [DF_t_d_AttStatistic_f_TotalOvertime] DEFAULT (0) FOR [f_TotalOvertime],\r\n\tCONSTRAINT [DF_t_d_AttStatistic_f_TotalAbsenceDay] DEFAULT (0) FOR [f_TotalAbsenceDay],\r\n\tCONSTRAINT [DF_t_d_AttStatistic_f_TotalNotReadCard] DEFAULT (0) FOR [f_TotalNotReadCard],\r\n\tCONSTRAINT [DF_t_d_AttStatistic_f_LateMinutes] DEFAULT (0) FOR [f_LateMinutes],\r\n\tCONSTRAINT [DF_t_d_AttStatistic_f_LeaveEarlyMinutes] DEFAULT (0) FOR [f_LeaveEarlyMinutes],\r\n\tCONSTRAINT [DF_t_d_AttStatistic_f_ManualReadTimesCount] DEFAULT (0) FOR [f_ManualReadTimesCount]\r\nGO\r\n\r\nALTER TABLE [dbo].[t_d_AttendenceData] ADD \r\n\tCONSTRAINT [DF_t_d_AttendenceData_f_ConsumerID] DEFAULT (0) FOR [f_ConsumerID],\r\n\tCONSTRAINT [DF_t_d_AttendenceData_f_LateTime] DEFAULT (0) FOR [f_LateTime],\r\n\tCONSTRAINT [DF_t_d_AttendenceData_f_LeaveEarlyTime] DEFAULT (0) FOR [f_LeaveEarlyTime],\r\n\tCONSTRAINT [DF_t_d_AttendenceData_f_OvertimeTime] DEFAULT (0) FOR [f_OvertimeTime],\r\n\tCONSTRAINT [DF_t_d_AttendenceData_f_AbsenceDay] DEFAULT (0) FOR [f_AbsenceDay]\r\nGO\r\n\r\nALTER TABLE [dbo].[t_d_Leave] ADD \r\n\tCONSTRAINT [DF_t_d_Leave_f_ConsumerID] DEFAULT (0) FOR [f_ConsumerID]\r\nGO\r\n\r\nALTER TABLE [dbo].[t_d_ManualCardRecord] ADD \r\n\tCONSTRAINT [DF_t_d_ManualCardRecord_f_CardNO] DEFAULT (0) FOR [f_CardNO],\r\n\tCONSTRAINT [DF_t_d_ManualCardRecord_f_Character] DEFAULT (0) FOR [f_Character],\r\n\tCONSTRAINT [DF_t_d_ManualCardRecord_f_ReaderID] DEFAULT (0) FOR [f_ReaderID],\r\n\tCONSTRAINT [DF_t_d_ManualCardRecord_f_ConsumerID] DEFAULT (0) FOR [f_ConsumerID],\r\n\tCONSTRAINT [DF_t_d_ManualCardRecord_f_DutyOnOff] DEFAULT (3) FOR [f_DutyOnOff]\r\nGO\r\n\r\nALTER TABLE [dbo].[t_d_ShiftData] ADD \r\n\tCONSTRAINT [DF_t_d_ShiftData_f_ConsumerID] DEFAULT (0) FOR [f_ConsumerID],\r\n\tCONSTRAINT [DF_t_d_ShiftData_f_ShiftID_01] DEFAULT ((-1)) FOR [f_ShiftID_01],\r\n\tCONSTRAINT [DF_t_d_ShiftData_f_ShiftID_02] DEFAULT ((-1)) FOR [f_ShiftID_02],\r\n\tCONSTRAINT [DF_t_d_ShiftData_f_ShiftID_03] DEFAULT ((-1)) FOR [f_ShiftID_03],\r\n\tCONSTRAINT [DF_t_d_ShiftData_f_ShiftID_04] DEFAULT ((-1)) FOR [f_ShiftID_04],\r\n\tCONSTRAINT [DF_t_d_ShiftData_f_ShiftID_05] DEFAULT ((-1)) FOR [f_ShiftID_05],\r\n\tCONSTRAINT [DF_t_d_ShiftData_f_ShiftID_06] DEFAULT ((-1)) FOR [f_ShiftID_06],\r\n\tCONSTRAINT [DF_t_d_ShiftData_f_ShiftID_07] DEFAULT ((-1)) FOR [f_ShiftID_07],\r\n\tCONSTRAINT [DF_t_d_ShiftData_f_ShiftID_08] DEFAULT ((-1)) FOR [f_ShiftID_08],\r\n\tCONSTRAINT [DF_t_d_ShiftData_f_ShiftID_09] DEFAULT ((-1)) FOR [f_ShiftID_09],\r\n\tCONSTRAINT [DF_t_d_ShiftData_f_ShiftID_10] DEFAULT ((-1)) FOR [f_ShiftID_10],\r\n\tCONSTRAINT [DF_t_d_ShiftData_f_ShiftID_11] DEFAULT ((-1)) FOR [f_ShiftID_11],\r\n\tCONSTRAINT [DF_t_d_ShiftData_f_ShiftID_12] DEFAULT ((-1)) FOR [f_ShiftID_12],\r\n\tCONSTRAINT [DF_t_d_ShiftData_f_ShiftID_13] DEFAULT ((-1)) FOR [f_ShiftID_13],\r\n\tCONSTRAINT [DF_t_d_ShiftData_f_ShiftID_14] DEFAULT ((-1)) FOR [f_ShiftID_14],\r\n\tCONSTRAINT [DF_t_d_ShiftData_f_ShiftID_15] DEFAULT ((-1)) FOR [f_ShiftID_15],\r\n\tCONSTRAINT [DF_t_d_ShiftData_f_ShiftID_16] DEFAULT ((-1)) FOR [f_ShiftID_16],\r\n\tCONSTRAINT [DF_t_d_ShiftData_f_ShiftID_17] DEFAULT ((-1)) FOR [f_ShiftID_17],\r\n\tCONSTRAINT [DF_t_d_ShiftData_f_ShiftID_18] DEFAULT ((-1)) FOR [f_ShiftID_18],\r\n\tCONSTRAINT [DF_t_d_ShiftData_f_ShiftID_19] DEFAULT ((-1)) FOR [f_ShiftID_19],\r\n\tCONSTRAINT [DF_t_d_ShiftData_f_ShiftID_20] DEFAULT ((-1)) FOR [f_ShiftID_20],\r\n\tCONSTRAINT [DF_t_d_ShiftData_f_ShiftID_21] DEFAULT ((-1)) FOR [f_ShiftID_21],\r\n\tCONSTRAINT [DF_t_d_ShiftData_f_ShiftID_22] DEFAULT ((-1)) FOR [f_ShiftID_22],\r\n\tCONSTRAINT [DF_t_d_ShiftData_f_ShiftID_23] DEFAULT ((-1)) FOR [f_ShiftID_23],\r\n\tCONSTRAINT [DF_t_d_ShiftData_f_ShiftID_24] DEFAULT ((-1)) FOR [f_ShiftID_24],\r\n\tCONSTRAINT [DF_t_d_ShiftData_f_ShiftID_25] DEFAULT ((-1)) FOR [f_ShiftID_25],\r\n\tCONSTRAINT [DF_t_d_ShiftData_f_ShiftID_26] DEFAULT ((-1)) FOR [f_ShiftID_26],\r\n\tCONSTRAINT [DF_t_d_ShiftData_f_ShiftID_27] DEFAULT ((-1)) FOR [f_ShiftID_27],\r\n\tCONSTRAINT [DF_t_d_ShiftData_f_ShiftID_28] DEFAULT ((-1)) FOR [f_ShiftID_28],\r\n\tCONSTRAINT [DF_t_d_ShiftData_f_ShiftID_29] DEFAULT ((-1)) FOR [f_ShiftID_29],\r\n\tCONSTRAINT [DF_t_d_ShiftData_f_ShiftID_30] DEFAULT ((-1)) FOR [f_ShiftID_30],\r\n\tCONSTRAINT [DF_t_d_ShiftData_f_ShiftID_31] DEFAULT ((-1)) FOR [f_ShiftID_31],\r\n\tCONSTRAINT [DF_t_d_ShiftData_f_LogDate] DEFAULT (getdate()) FOR [f_LogDate]\r\nGO\r\n\r\n CREATE  INDEX [IX_t_d_ShiftData] ON [dbo].[t_d_ShiftData]([f_ConsumerID]) ON [PRIMARY]\r\nGO\r\n\r\nALTER TABLE [dbo].[t_d_Shift_Work_Schedule] ADD \r\n\tCONSTRAINT [DF_t_d_Shift_Work_Schedule_f_ConsumerID] DEFAULT (0) FOR [f_ConsumerID],\r\n\tCONSTRAINT [DF_t_d_Shift_Work_Schedule_f_TimeSeg] DEFAULT (0) FOR [f_TimeSeg],\r\n\tCONSTRAINT [DF_t_d_Shift_Work_Schedule_f_Duration] DEFAULT (0) FOR [f_Duration],\r\n\tCONSTRAINT [DF_t_d_Shift_Work_Schedule_f_bOvertimeShift] DEFAULT (0) FOR [f_bOvertimeShift]\r\nGO\r\n\r\n CREATE  INDEX [IX_t_d_Shift_Work_Schedule] ON [dbo].[t_d_Shift_Work_Schedule]([f_ConsumerID]) ON [PRIMARY]\r\nGO\r\n\r\nALTER TABLE [dbo].[t_d_SwipeRecord] ADD \r\n\tCONSTRAINT [DF_t_d_SwipeRecord_f_ConsumerID] DEFAULT (0) FOR [f_ConsumerID],\r\n\tCONSTRAINT [DF_t_d_SwipeRecord_f_Modified] DEFAULT (getdate()) FOR [f_Modified]\r\nGO\r\n\r\n CREATE  INDEX [IX_t_d_SwipeRecord] ON [dbo].[t_d_SwipeRecord]([f_ReadDate], [f_ConsumerID], [f_ReaderID]) ON [PRIMARY]\r\nGO\r\n\r\nALTER TABLE [dbo].[t_d_doorFirstCardUsers] ADD \r\n\tCONSTRAINT [DF_t_d_doorFirstCardUsers_f_DoorID] DEFAULT (0) FOR [f_DoorID],\r\n\tCONSTRAINT [DF_t_d_doorFirstCardUsers_f_ConsumerID] DEFAULT (0) FOR [f_ConsumerID]\r\nGO\r\n\r\nALTER TABLE [dbo].[t_d_doorMoreCardsUsers] ADD \r\n\tCONSTRAINT [DF_t_d_doorMoreCardsUsers_f_DoorID] DEFAULT (0) FOR [f_DoorID],\r\n\tCONSTRAINT [DF_t_d_doorMoreCardsUsers_f_MoreCards_GrpID] DEFAULT (0) FOR [f_MoreCards_GrpID],\r\n\tCONSTRAINT [DF_t_d_doorMoreCardsUsers_f_ConsumerID] DEFAULT (0) FOR [f_ConsumerID]\r\nGO\r\n\r\nALTER TABLE [dbo].[t_d_shift_AttReport] ADD \r\n\tCONSTRAINT [DF_t_d_shift_AttReport_f_ConsumerID] DEFAULT (0) FOR [f_ConsumerID],\r\n\tCONSTRAINT [DF_t_d_shift_AttReport_f_ShiftID] DEFAULT ((-1)) FOR [f_ShiftID],\r\n\tCONSTRAINT [DF_t_d_shift_AttReport_f_ReadTimes] DEFAULT (0) FOR [f_ReadTimes],\r\n\tCONSTRAINT [DF_t_d_shift_AttReport_f_LateMinutes] DEFAULT (0) FOR [f_LateMinutes],\r\n\tCONSTRAINT [DF_t_d_shift_AttReport_f_LeaveEarlyMinutes] DEFAULT (0) FOR [f_LeaveEarlyMinutes],\r\n\tCONSTRAINT [DF_t_d_shift_AttReport_f_OvertimeHours] DEFAULT (0) FOR [f_OvertimeHours],\r\n\tCONSTRAINT [DF_t_d_shift_AttReport_f_AbsenceDays] DEFAULT (0) FOR [f_AbsenceDays],\r\n\tCONSTRAINT [DF_t_d_shift_AttReport_f_NotReadCardCount] DEFAULT (0) FOR [f_NotReadCardCount],\r\n\tCONSTRAINT [DF_t_d_shift_AttReport_f_bOvertimeShift] DEFAULT (0) FOR [f_bOvertimeShift]\r\nGO\r\n\r\n CREATE  INDEX [IX_t_d_shift_AttReport] ON [dbo].[t_d_shift_AttReport]([f_ConsumerID]) ON [PRIMARY]\r\nGO\r\n\r\nALTER TABLE [dbo].[t_d_shift_AttStatistic] ADD \r\n\tCONSTRAINT [DF_t_d_shift_AttStatistic_f_ConsumerID] DEFAULT (0) FOR [f_ConsumerID],\r\n\tCONSTRAINT [DF_t_d_shift_AttStatistic_f_DayShouldWork] DEFAULT (0) FOR [f_DayShouldWork],\r\n\tCONSTRAINT [DF_t_d_shift_AttStatistic_f_LateMinutes] DEFAULT (0) FOR [f_LateMinutes],\r\n\tCONSTRAINT [DF_t_d_shift_AttStatistic_f_LateCount] DEFAULT (0) FOR [f_LateCount],\r\n\tCONSTRAINT [DF_t_d_shift_AttStatistic_f_LeaveEarlyMinutes] DEFAULT (0) FOR [f_LeaveEarlyMinutes],\r\n\tCONSTRAINT [DF_t_d_shift_AttStatistic_f_LeaveEarlyCount] DEFAULT (0) FOR [f_LeaveEarlyCount],\r\n\tCONSTRAINT [DF_t_d_shift_AttStatistic_f_OvertimeHours] DEFAULT (0) FOR [f_OvertimeHours],\r\n\tCONSTRAINT [DF_t_d_shift_AttStatistic_f_AbsenceDays] DEFAULT (0) FOR [f_AbsenceDays],\r\n\tCONSTRAINT [DF_t_d_shift_AttStatistic_f_NotReadCardCount] DEFAULT (0) FOR [f_NotReadCardCount],\r\n\tCONSTRAINT [DF_t_d_shift_AttStatistic_f_ManualReadTimesCount] DEFAULT (0) FOR [f_ManualReadTimesCount],\r\n\tCONSTRAINT [DF__t_d_shift__f_Day__12C8C788] DEFAULT (0) FOR [f_DayRealWork]\r\nGO\r\n\r\n CREATE  INDEX [IX_t_d_shift_AttStatistic] ON [dbo].[t_d_shift_AttStatistic]([f_ConsumerID]) ON [PRIMARY]\r\nGO\r\n\r\nALTER TABLE [dbo].[t_s_OperatorPrivilege] ADD \r\n\tCONSTRAINT [DF_t_s_OperatorPrivilege_f_ReadOnly] DEFAULT (1) FOR [f_ReadOnly],\r\n\tCONSTRAINT [DF_t_s_OperatorPrivilege_f_FullControl] DEFAULT (1) FOR [f_FullControl]\r\nGO\r\n\r\nALTER TABLE [dbo].[t_s_wglog] ADD \r\n\tCONSTRAINT [DF_t_s_wglog_f_UserID] DEFAULT (0) FOR [f_UserID],\r\n\tCONSTRAINT [DF_t_s_wglog_f_LogDateTime] DEFAULT (getdate()) FOR [f_LogDateTime]\r\nGO\r\n\r\n\r\nexec sp_addextendedproperty N'MS_Description', N'考勤', N'user', N'dbo', N'table', N't_b_Consumer', N'column', N'f_AttendEnabled'\r\nGO\r\nexec sp_addextendedproperty N'MS_Description', N'起始日期', N'user', N'dbo', N'table', N't_b_Consumer', N'column', N'f_BeginYMD'\r\nGO\r\nexec sp_addextendedproperty N'MS_Description', N'卡号 可以为空 不重复', N'user', N'dbo', N'table', N't_b_Consumer', N'column', N'f_CardNO'\r\nGO\r\nexec sp_addextendedproperty N'MS_Description', N'用户唯一ID号', N'user', N'dbo', N'table', N't_b_Consumer', N'column', N'f_ConsumerID'\r\nGO\r\nexec sp_addextendedproperty N'MS_Description', N'姓名[可重复]', N'user', N'dbo', N'table', N't_b_Consumer', N'column', N'f_ConsumerName'\r\nGO\r\nexec sp_addextendedproperty N'MS_Description', N'编号[不重复]', N'user', N'dbo', N'table', N't_b_Consumer', N'column', N'f_ConsumerNO'\r\nGO\r\nexec sp_addextendedproperty N'MS_Description', N'门禁 ', N'user', N'dbo', N'table', N't_b_Consumer', N'column', N'f_DoorEnabled'\r\nGO\r\nexec sp_addextendedproperty N'MS_Description', N'截止日期', N'user', N'dbo', N'table', N't_b_Consumer', N'column', N'f_EndYMD'\r\nGO\r\nexec sp_addextendedproperty N'MS_Description', N'部门ID号', N'user', N'dbo', N'table', N't_b_Consumer', N'column', N'f_GroupID'\r\nGO\r\nexec sp_addextendedproperty N'MS_Description', N'密码 [=0 表示不用密码]', N'user', N'dbo', N'table', N't_b_Consumer', N'column', N'f_PIN'\r\nGO\r\nexec sp_addextendedproperty N'MS_Description', N'倒班', N'user', N'dbo', N'table', N't_b_Consumer', N'column', N'f_ShiftEnabled'\r\n\r\n\r\nGO\r\n\r\n\r\nexec sp_addextendedproperty N'MS_Description', N'时段ID', N'user', N'dbo', N'table', N't_b_ControlSeg', N'column', N'f_ControlSegID'\r\nGO\r\nexec sp_addextendedproperty N'MS_Description', N'此时段当天限次', N'user', N'dbo', N'table', N't_b_ControlSeg', N'column', N'f_LimitedTimesOfDay'\r\nGO\r\nexec sp_addextendedproperty N'MS_Description', N'时区1限次', N'user', N'dbo', N'table', N't_b_ControlSeg', N'column', N'f_LimitedTimesOfHMS1'\r\nGO\r\nexec sp_addextendedproperty N'MS_Description', N'时区2限次', N'user', N'dbo', N'table', N't_b_ControlSeg', N'column', N'f_LimitedTimesOfHMS2'\r\nGO\r\nexec sp_addextendedproperty N'MS_Description', N'时区3限次', N'user', N'dbo', N'table', N't_b_ControlSeg', N'column', N'f_LimitedTimesOfHMS3'\r\nGO\r\nexec sp_addextendedproperty N'MS_Description', N'读头独立计数', N'user', N'dbo', N'table', N't_b_ControlSeg', N'column', N'f_ReaderCount'\r\n\r\n\r\nGO\r\n\r\n\r\nexec sp_addextendedproperty N'MS_Description', N'反潜[0 不启用]', N'user', N'dbo', N'table', N't_b_Controller', N'column', N'f_AntiBack'\r\nGO\r\nexec sp_addextendedproperty N'MS_Description', N'控制器ID', N'user', N'dbo', N'table', N't_b_Controller', N'column', N'f_ControllerID'\r\nGO\r\nexec sp_addextendedproperty N'MS_Description', N'控制器编号', N'user', N'dbo', N'table', N't_b_Controller', N'column', N'f_ControllerNO'\r\nGO\r\nexec sp_addextendedproperty N'MS_Description', N'门名称', N'user', N'dbo', N'table', N't_b_Controller', N'column', N'f_DoorNames'\r\nGO\r\nexec sp_addextendedproperty N'MS_Description', N'互锁', N'user', N'dbo', N'table', N't_b_Controller', N'column', N'f_InterLock'\r\nGO\r\nexec sp_addextendedproperty N'MS_Description', N'默认进门多卡, 出门单卡', N'user', N'dbo', N'table', N't_b_Controller', N'column', N'f_MoreCards_GoInOut'\r\nGO\r\nexec sp_addextendedproperty N'MS_Description', N'说明', N'user', N'dbo', N'table', N't_b_Controller', N'column', N'f_Note'\r\nGO\r\nexec sp_addextendedproperty N'MS_Description', N'通信端口', N'user', N'dbo', N'table', N't_b_Controller', N'column', N'f_PORT'\r\n\r\n\r\nGO\r\n\r\n\r\nexec sp_addextendedproperty N'MS_Description', N'0表示所有', N'user', N'dbo', N'table', N't_b_ControllerTaskList', N'column', N'f_DoorID'\r\n\r\n\r\nGO\r\n\r\n\r\nexec sp_addextendedproperty N'MS_Description', N'门控制方式 [3在线,2常闭,1常开]', N'user', N'dbo', N'table', N't_b_Door', N'column', N'f_DoorControl'\r\nGO\r\nexec sp_addextendedproperty N'MS_Description', N'开门延时', N'user', N'dbo', N'table', N't_b_Door', N'column', N'f_DoorDelay'\r\nGO\r\nexec sp_addextendedproperty N'MS_Description', N'门ID', N'user', N'dbo', N'table', N't_b_Door', N'column', N'f_DoorID'\r\nGO\r\nexec sp_addextendedproperty N'MS_Description', N'门名称 唯一', N'user', N'dbo', N'table', N't_b_Door', N'column', N'f_DoorName'\r\nGO\r\nexec sp_addextendedproperty N'MS_Description', N'门编号1-4', N'user', N'dbo', N'table', N't_b_Door', N'column', N'f_DoorNO'\r\n\r\n\r\nGO\r\n\r\n\r\nexec sp_addextendedproperty N'MS_Description', N'卡号', N'user', N'dbo', N'table', N't_b_IDCard_Lost', N'column', N'f_CardNO'\r\nGO\r\nexec sp_addextendedproperty N'MS_Description', N'之前拥有此卡的用户ID', N'user', N'dbo', N'table', N't_b_IDCard_Lost', N'column', N'f_ConsumerID'\r\nGO\r\nexec sp_addextendedproperty N'MS_Description', N'修改的时间', N'user', N'dbo', N'table', N't_b_IDCard_Lost', N'column', N'f_Modified'\r\n\r\n\r\nGO\r\n\r\n\r\nexec sp_addextendedproperty N'MS_Description', N'修改时间 [也就是提取时间]', N'user', N'dbo', N'table', N't_d_SwipeRecord', N'column', N'f_Modified'\r\nGO\r\nexec sp_addextendedproperty N'MS_Description', N'刷卡时间', N'user', N'dbo', N'table', N't_d_SwipeRecord', N'column', N'f_ReadDate'\r\nGO\r\n\r\n/*****************************************2011-12-19_10:38:34 增加 到7.3部分 ********************/\r\n/**新增一个表: 电子地图表**/\r\n                        /****** Object:  Table [dbo].[t_d_maps]    Script Date: 2006-4-29 12:23:47 ******/\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_d_maps]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_d_maps]\r\nGO\r\n\r\nCREATE TABLE [t_d_maps] (\r\n\t[f_MapId] [int] IDENTITY (1, 1) NOT NULL ,\r\n\t[f_MapName] [nvarchar] (255)  NOT NULL ,\r\n\t[f_MapPageIndex] [int] NOT NULL ,\r\n\t[f_MapFile] [nvarchar] (255)  NOT NULL ,\r\n\t[f_Notes] [ntext]  NULL \r\n)   ON [PRIMARY]\r\nGO\r\nALTER TABLE [t_d_maps] WITH NOCHECK ADD \r\n\tCONSTRAINT [PK_t_d_maps] PRIMARY KEY  CLUSTERED \r\n\t(\r\n\t\t[f_MapId]\r\n\t)   ON [PRIMARY]\r\nGO\r\nALTER TABLE [t_d_maps] WITH NOCHECK ADD \r\n\tCONSTRAINT [DF_t_d_maps_f_MapName] DEFAULT (N'Map') FOR [f_MapName],\r\n\tCONSTRAINT [DF_t_d_maps_f_MapPageIndex] DEFAULT (0) FOR [f_MapPageIndex],\r\n\tCONSTRAINT [DF_t_d_maps_f_MapFile] DEFAULT (N'.\\PHOTO') FOR [f_MapFile]\r\nGO\r\n\r\n/**新增一个表: 各电子地图表包含的门表**/\r\n                        /****** Object:  Table [dbo].[t_d_mapdoors]    Script Date: 2006-4-29 12:23:47 ******/\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_d_mapdoors]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_d_mapdoors]\r\nGO\r\nCREATE TABLE [t_d_mapdoors] (\r\n\t[f_MapDoorId] [int] IDENTITY (1, 1) NOT NULL ,\r\n\t[f_DoorID] [int] NOT NULL ,\r\n\t[f_MapId] [int] NOT NULL ,\r\n\t[f_DoorLocationX] [int] NOT NULL ,\r\n\t[f_DoorLocationY] [int] NOT NULL ,\r\n\t[f_Notes] [ntext]  NULL \r\n) \r\nGO\r\nALTER TABLE [t_d_mapdoors] WITH NOCHECK ADD \r\n\tCONSTRAINT [PK_t_d_mapdoors] PRIMARY KEY  CLUSTERED \r\n\t(\r\n\t\t[f_MapDoorId]\r\n\t)\r\nGO\r\nALTER TABLE [t_d_mapdoors] WITH NOCHECK ADD \r\n\tCONSTRAINT [DF_t_d_mapdoors_f_DoorID] DEFAULT (0) FOR [f_DoorID],\r\n\tCONSTRAINT [DF_t_d_mapdoors_f_MapId] DEFAULT (0) FOR [f_MapId],\r\n\tCONSTRAINT [DF_t_d_mapdoors_f_DoorLocationX] DEFAULT (0) FOR [f_DoorLocationX],\r\n\tCONSTRAINT [DF_t_d_mapdoors_f_DoorLocationY] DEFAULT (0) FOR [f_DoorLocationY]\r\nGO\r\n\r\n/*2010-11-6 14:37:29 新增一个表: t_b_Group4Operator 用于操作员只能管指定的部门或班组, 缺省为空时表示所有的部门*/\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_b_Group4Operator]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_b_Group4Operator]\r\nGO\r\nCREATE TABLE t_b_Group4Operator ( \r\n    f_OperatorGroupId         [int] IDENTITY (1, 1) NOT NULL  ,\r\n    f_GroupID   [int]  NOT NULL Default(0) ,\r\n    f_OperatorID   [int]  NOT NULL Default(0) ) \r\nGO\r\n\r\nALTER TABLE [t_b_Group4Operator] WITH NOCHECK ADD \r\n     \tCONSTRAINT [PK_t_b_Group4Operator] PRIMARY KEY  CLUSTERED \r\n         ([f_OperatorGroupId])  \r\nGO\r\n\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_b_Controller_Zone4Operator]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_b_Controller_Zone4Operator]\r\nGO\r\nCREATE TABLE t_b_Controller_Zone4Operator ( \r\n f_OperatorZoneId         [int] IDENTITY (1, 1) NOT NULL  ,\r\n f_ZoneID   [int]  NOT NULL Default(0) ,\r\n f_OperatorID   [int]  NOT NULL Default(0) ) \r\nGO\r\n\r\nALTER TABLE [t_b_Controller_Zone4Operator] WITH NOCHECK ADD \r\n\tCONSTRAINT [PK_t_b_Controller_Zone4Operator] PRIMARY KEY  CLUSTERED \r\n ([f_OperatorZoneId]) \r\nGO\r\n\r\nALTER TABLE [t_s_OperatorPrivilege] WITH NOCHECK ADD \r\n\tCONSTRAINT [PK_t_s_OperatorPrivilege] PRIMARY KEY  CLUSTERED \r\n\t(\r\n\t\t[f_OperatorID],\r\n\t\t[f_FunctionID]\r\n\t)  ON [PRIMARY] \r\nGO\r\n\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_b_group4PCCheckAccess]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_b_group4PCCheckAccess]\r\nGO\r\nCREATE TABLE t_b_group4PCCheckAccess (\r\nf_Id        [int] IDENTITY (1, 1) NOT NULL  ,\r\nf_GroupID   [int]  NOT NULL Default(0) ,\r\nf_GroupType   [int]  NOT NULL Default(0) ,\r\nf_CheckAccessActive   [int]  NOT NULL Default(0) ,\r\nf_MoreCards   [int]  NOT NULL Default(1) ,\r\nf_SoundFileName   [nvarchar] (255)  NULL,\r\nf_Notes      [ntext]  NULL  )\r\nGO\r\n\r\nALTER TABLE [t_b_group4PCCheckAccess] WITH NOCHECK ADD \r\n\tCONSTRAINT [PK_t_b_group4PCCheckAccess] PRIMARY KEY  CLUSTERED \r\n ([f_Id]) \r\nGO\r\n        \r\n\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_b_ControlHolidays]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_b_ControlHolidays]\r\nGO\r\nCREATE TABLE t_b_ControlHolidays ( \r\n[f_Id]        [int] IDENTITY (1, 1) NOT NULL  ,\r\n[f_BeginYMDHMS] [datetime] NOT NULL  DEFAULT (convert(datetime,'2010-01-01 00:00:00',120)),\r\n[f_EndYMDHMS] [datetime] NOT NULL  DEFAULT (convert(datetime,'2029-12-31 23:59:00',120)),\r\n[f_forceWork] INT  NOT NULL DEFAULT 0 ,\r\n[f_Notes]      [ntext]  NULL  )\r\n\r\nGO\r\n\r\nALTER TABLE [t_b_ControlHolidays] WITH NOCHECK ADD \r\n\tCONSTRAINT [PK_t_b_ControlHolidays] PRIMARY KEY  CLUSTERED \r\n ([f_Id]) \r\nGO\r\n\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_b_Floor]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_b_Floor]\r\nGO\r\nCREATE TABLE t_b_Floor ( \r\nf_FloorId        [int] IDENTITY (1, 1) NOT NULL  ,\r\nf_DoorID         [int]  NOT NULL Default(0) ,\r\nf_ControllerID   [int]  NOT NULL Default(0) ,\r\nf_FloorNO     [int]  NOT NULL Default(0) ,\r\n[f_FloorName] [nvarchar] (255)  NOT NULL ,\r\nf_Notes      [ntext]  NULL  )\r\nGO\r\n\r\nALTER TABLE [t_b_Floor] WITH NOCHECK ADD \r\n\tCONSTRAINT [PK_t_b_Floor] PRIMARY KEY  CLUSTERED \r\n\t(\r\n\t\t[f_FloorId])\r\nGO\r\n\r\nALTER TABLE [t_b_Floor] WITH NOCHECK ADD \r\n\tCONSTRAINT [DF_t_b_Floor_f_FloorName] DEFAULT (N'Floor') FOR [f_FloorName]\r\nGO\r\n\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_b_ElevatorGroup]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_b_ElevatorGroup]\r\nGO\r\nCREATE TABLE t_b_ElevatorGroup ( \r\nf_DoorID         [int]  NOT NULL Default(0) ,\r\nf_ControllerID   [int]  NOT NULL Default(0) ,\r\nf_ElevatorGroupNO     [int]  NOT NULL Default(0) ,\r\nf_Notes      [ntext]  NULL  )\r\nGO\r\n\r\nALTER TABLE [t_b_ElevatorGroup] WITH NOCHECK ADD \r\n\tCONSTRAINT [PK_t_b_ElevatorGroup] PRIMARY KEY  CLUSTERED \r\n\t(\r\n\t\t[f_DoorId]\r\n\t) \r\nGO\r\n\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_b_UserFloor]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_b_UserFloor]\r\nGO\r\nCREATE TABLE t_b_UserFloor ( \r\nf_RecId          [int] IDENTITY (1, 1) NOT NULL  ,\r\nf_FloorId        [int]  NOT NULL Default(0) ,  \r\nf_ConsumerID     [int]  NOT NULL Default(0) ,   \r\nf_ControlSegID   [int]  NOT NULL Default(1) ,   \r\nf_MoreFloorNum   [int]  NOT NULL Default(1) ,   \r\nf_Notes      [ntext]  NULL  )\r\nGO\r\n\r\nALTER TABLE [t_b_UserFloor] WITH NOCHECK ADD \r\n\tCONSTRAINT [PK_t_b_UserFloor] PRIMARY KEY  CLUSTERED \r\n\t(\r\n\t\t[f_RecId]\r\n\t) \r\nGO\r\n\r\n/*****************************************默认数据 ********************/\r\n\r\nInsert Into t_a_Attendence(f_No,f_Name,f_EName,f_Value,f_Notes) Values(1,'迟到多少分钟以内不算迟到','LateTimeout','5','单位是分钟 缺省是5分钟')\r\nInsert Into t_a_Attendence(f_No,f_Name,f_EName,f_Value,f_Notes) Values(2,'迟到多少分钟作旷工','LateAbsenceTimeout','90','单位是分钟 缺省是120分钟')\r\nInsert Into t_a_Attendence(f_No,f_Name,f_EName,f_Value,f_Notes) Values(3,'迟到作旷工的天数','LateAbsenceDay','0.5','单位是天, 缺省是半天')\r\nInsert Into t_a_Attendence(f_No,f_Name,f_EName,f_Value,f_Notes) Values(4,'提前多少分钟离开不算迟到','LeaveTimeout','5','单位是分钟 缺省是5分钟')\r\nInsert Into t_a_Attendence(f_No,f_Name,f_EName,f_Value,f_Notes) Values(5,'迟到多少分钟作旷工','LeaveAbsenceTimeout','90','单位是分钟 缺省是120分钟')\r\nInsert Into t_a_Attendence(f_No,f_Name,f_EName,f_Value,f_Notes) Values(6,'迟到作旷工的天数','LeaveAbsenceDay','0.5','单位是天, 缺省是半天[只有0,0.5,1] 0天，半天，1天')\r\nInsert Into t_a_Attendence(f_No,f_Name,f_EName,f_Value,f_Notes) Values(7,'下午下班多少分钟后打卡算加班','OvertimeTimeout','60','单位是分钟 缺省是60分钟')\r\nInsert Into t_a_Attendence(f_No,f_Name,f_EName,f_Value,f_Notes) Values(8,'两次刷卡上班时间0','OnDuty0','2010-05-31 08:30:00','上班时间0 缺省是8:30:00==只取用时间部分, 而日期只是为了整个操作方便')\r\nInsert Into t_a_Attendence(f_No,f_Name,f_EName,f_Value,f_Notes) Values(9,'两次刷卡下班时间0','OffDuty0','2010-05-31 17:30:00','下班时间0 缺省是17:30:00')\r\nInsert Into t_a_Attendence(f_No,f_Name,f_EName,f_Value,f_Notes) Values(10,'四次刷卡上班时间1','OnDuty1','2010-05-31 08:30:00','上班时间1 缺省是08:30:00')\r\nInsert Into t_a_Attendence(f_No,f_Name,f_EName,f_Value,f_Notes) Values(11,'四次刷卡下班时间1','OffDuty1','2010-05-31 12:00:00','下班时间1 缺省是12:00:00')\r\nInsert Into t_a_Attendence(f_No,f_Name,f_EName,f_Value,f_Notes) Values(12,'四次刷卡上班时间2','OnDuty2','2010-05-31 13:30:00','上班时间2 缺省是13:30:00')\r\nInsert Into t_a_Attendence(f_No,f_Name,f_EName,f_Value,f_Notes) Values(13,'四次刷卡下班时间2','OffDuty2','2010-05-31 17:30:00','下班时间2 缺省是17:30:00')\r\nInsert Into t_a_Attendence(f_No,f_Name,f_EName,f_Value,f_Notes) Values(14,'每天刷卡次数','EveryDayReadCardTimes','2','缺省是2次，采用两次刷卡上班时间,采用Duty0设置。另一个是四次，采用Duty1,Duty2设置')\r\nInsert Into t_a_Attendence(f_No,f_Name,f_EName,f_Value,f_Notes) Values(15,'考勤报表生成日志','LogCreateReport','','')\r\nInsert Into t_a_Attendence(f_No,f_Name,f_EName,f_Value,f_Notes) Values(16,'两次刷卡最小间隔','twoReadMintime','60','缺省是60秒, V8')\r\n\r\nInsert Into t_a_Holiday(f_Name,f_EName,f_Value,f_Value1,f_Value2,f_Value3,f_Type,f_Note) Values('Sat.','Saturday','0','','','','1','缺省休息， 0=表示休息，1=表示上午上班，2=表示下午上班，3=表示上正常班')\r\nInsert Into t_a_Holiday(f_Name,f_EName,f_Value,f_Value1,f_Value2,f_Value3,f_Type,f_Note) Values('Sun.','Sunday','0','','','','1','缺省休息， 0=表示休息，1=表示上午上班，2=表示下午上班，3=表示上正常班')\r\n\r\nInsert Into t_a_HolidayType(f_HolidayType) Values('Business Trip')\r\nInsert Into t_a_HolidayType(f_HolidayType) Values('Sick Leave')\r\nInsert Into t_a_HolidayType(f_HolidayType) Values('Private Leave')\r\n\r\nInsert Into t_a_Shift_Attendence(f_NO,f_EName,f_Value,f_Notes) Values(1,'LateTimeout','5','V13-OK 迟到多少分钟以内不算迟到 单位是分钟 缺省是5分钟')\r\nInsert Into t_a_Shift_Attendence(f_NO,f_EName,f_Value,f_Notes) Values(4,'LeaveTimeout','5','V13 提前多少分钟离开不算迟到 单位是分钟 缺省是5分钟')\r\nInsert Into t_a_Shift_Attendence(f_NO,f_EName,f_Value,f_Notes) Values(7,'OvertimeTimeout','60','V13 下午下班多少分钟后打卡算加班 单位是分钟 缺省是60分钟')\r\nInsert Into t_a_Shift_Attendence(f_NO,f_EName,f_Value,f_Notes) Values(17,'AheadMinutesOnDutyFirst','60','V13-OK 17最早上班可提早的分钟, 缺省60分钟')\r\nInsert Into t_a_Shift_Attendence(f_NO,f_EName,f_Value,f_Notes) Values(18,'AheadMinutes','60','V13-OK 18普通可提早的分钟, 缺省60分钟')\r\nInsert Into t_a_Shift_Attendence(f_NO,f_EName,f_Value,f_Notes) Values(19,'DelayMinutes','60','V13-OK 19可推迟的分钟, 缺省60分钟')\r\nInsert Into t_a_Shift_Attendence(f_NO,f_EName,f_Value,f_Notes) Values(20,'OvertimeMinutes','360','V13-OK 20可加班的最大时间长度[分钟], 缺省360分钟[相当于6小时]')\r\n\r\n/***Insert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(9,'Database Version','dbVersion','70','')***/\r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(9,'Database Version','dbVersion','73','')\r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(12,'PD','2005-01-06','0','produce')\r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(17,'Custom Title','CustomTitle','','个性化的公司标题[不能超过25个汉字]')\r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(22,'Watch Picture Size Enlarge','WatchPictureSizeEnlarge','1','V6 缺省倍数调为1 V5 ''监控时的显示图标--缺省是倍数为1')\r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(24,'Threat Password','ForcePassword','889988','V5--可以修改, 但不能为空, 缺省为889988')\r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(30,'Application Version','Application Version','','V9 当前使用的应用软件版本')\r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(36,'Company Name','Company Name','','')\r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(38,'System Information','System Information','V13','V13')\r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(40,'Open Door Overtime','Open Door Overtime','15','V15 默认是15秒, 不能大于6553秒')\r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(41,'Path of Photo','Path of Photo','','缺省=() 表示 (.\\PHOTO)')                    \r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(42,'Path of Captured JPGs','Path of Captured JPGs','','缺省=() 表示 (.\\AVI_JPG)')  \r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(43,'Path of Captured AVIes','Path of Captured AVIes','','缺省=() 表示 (.\\AVI_JPG)')\r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(44,'Time of Captured AVI','Time of Captured AVI','3000','单位:毫秒. 缺省3000表示3秒')   \r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(48,'ConInfo','ConInfo','','------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------')                                 \r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(49,'Install Time','Install Time','','开始使用时间')                                  \r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(50,'Time of User Update','2010-6-21 14:52:07','','')                                  \r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(51,'Privilege Count ByControllerID','','','')                                  \r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(52,'Privilege Total','','','')                                  \r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(101,'Record Push Button Events','','0','')                                     \r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(102,'Record Door Status Events','','0','')                                     \r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(103,'Activate Log Query','','0','')                                            \r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(111,'ActivateDontDisplayAccessControl','','0','')                              \r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(112,'ActivateDontDisplayAttendance','','0','')                                 \r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(113,'ActivateOtherShiftSchedule','','0','')                                    \r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(121,'ActivateTimeProfile','','0','')                                           \r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(122,'ActivateRemoteOpenDoor','','0','')                                        \r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(123,'ActivateAccessKeypad','','0','')                                          \r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(124,'ActivatePeripheralControl','','0','')                                     \r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(125,'ActivateControllerZone','','0','')                                     \r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(131,'ActivateControllerTaskList','','0','')                                    \r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(132,'ActivateAntiPassBack','','0','')                                          \r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(133,'ActivateInterLock','','0','')                                             \r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(134,'ActivateMultiCardAccess','','0','')                                       \r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(135,'ActivateFirstCardOpen','','0','')                                         \r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(136,'ActivateTimeSegLimittedAccess','','0','')                                 \r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(141,'ActivateWarnForceWithCard','','0','')                                     \r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(142,'ActivateDontAutoLoadPrivileges','','0','')                                \r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(143,'ActivateDontAutoLoadSwipeRecords','','0','')\r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(152,'ActivateDontDisplayQuickConfig','','0','')\r\n\r\nInsert Into t_s_Operator(f_OperatorName,f_Password,f_OperatePrivilege,f_Notes) Values('admin','admin','Super Manager','不能删除和修改权限级别，可以改名和密码')\r\n\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,1,'mnu1BasicConfigure','Basic Configure',0,1)\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,2,'mnuControllers','Controllers',0,1)\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,3,'mnuGroups','Department',0,1)\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,4,'mnuConsumers','Personnel',0,1)\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,5,'mnuCardLost','Lost card register',0,1)\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,6,'mnu1DoorControl','Access Control',0,1)\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,7,'mnuControlSeg','Time Profile',0,1)\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,8,'mnuPrivilege','Access Privilege',0,1)\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,9,'mnuPeripheral','Peripheral Control',0,1)\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,10,'mnuPasswordManagement','Password Management',0,1)\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,11,'mnuAntiBack','Anti-passback',0,1)\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,12,'mnuInterLock','Inter Lock',0,1)\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,13,'mnuMoreCards','Multi-card access',0,1)\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,14,'mnuFirstCard','First Card Open',0,1)\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,15,'mnu1BasicOperate','Basic Operate',0,1)\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,16,'mnuTotalControl','Console',0,1)\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,17,'mnuCheckController','Check',0,1)\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,18,'mnuAdjustTime','Adjust Time',0,1)\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,19,'mnuUpload','Upload',0,1)\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,20,'mnuMonitor','Monitor',0,1)\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,21,'mnuGetCardRecords','Download',0,1)\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,22,'TotalControl_RemoteOpen','Remote Open',0,1)\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,23,'mnuCardRecords','Query Card Records',0,1)\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,24,'mnu1Attendence','Attendance',0,1)\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,25,'mnuShiftNormalConfigure','Normal Shift Configuration',0,1)\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,26,'mnuShiftRule','Other Shift Rules',0,1)\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,27,'mnuShiftSet','Other Shift Types',0,1)\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,28,'mnuShiftArrange','Other Shift Schedule',0,1)\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,29,'mnuHolidaySet','Holiday',0,1)\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,30,'mnuLeave','Leave / Business Trip',0,1)\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,31,'mnuManualCardRecord','Manual Sign In',0,1)\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,32,'mnuAttendenceData','Attendance Report',0,1)\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,33,'mnu1Tool','Tools',0,1)\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,34,'cmdChangePasswor','Change Password',0,1)\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,35,'cmdOperatorManage','Operator Management',0,1)\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,36,'mnuDBBackup','DB Backup',0,1)\r\n/*Insert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,37,'mnuControllerCommPasswordSet','Controller Communication Password',0,1)*/\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,37,'mnuExtendedFunction','Extended Functions',0,1)\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,38,'mnuOption','Option',0,1)\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,39,'mnuTaskList','Periodically update access method',0,1)\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,40,'mnuLogQuery','Log Query',0,1)\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,41,'mnu1Help','Help',0,1)\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,42,'mnuAbout','About...',0,1)\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,43,'mnuManual','Manual',0,1)\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,44,'mnuSystemCharacteristic','System Characteristic',0,1)\r\n\r\n/*****************************************2011-12-19_10:38:34 增加 到7.3部分 ********************/\r\n/** 增加电子地图选项**/\r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(114,'ActivateMaps','','0','') \r\n/***2010-11-8 08:51:37 总控制台电子地图**/\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,45,'btnMaps','Maps',0,1)\r\n/***2010-11-8 08:51:37 区域管理 只有全功能才允许修改***/\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,46,'btnZoneManage','Zones',0,1) \r\n/***2011-2-19 12:35:29  实时提取 只有全功能才允许修改***/\r\nInsert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl) Values(1,47,'mnuRealtimeGetRecords','RealtimeGetRecords',0,1) \r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(137,'ActivatePCCheckAccess','','0','') \r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(54,'Invalid Records Not As Attendance','','0','')\r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(144,'ActivateElevator','','0','') \r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(55,'Off Duty latest time for Normal Attendance','','00:00:00','') \r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(56,'Earliest time as on-duty for Normal Attendance','','0','') \r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(57,'Earliest time as on-duty, Latest time as off-duty','','0','') \r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(58,'Normal Work Time (hr)','','8.0','') \r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(59,'Only On-duty','','0','') \r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(145,'ActivateHouse','','0','')      \r\n\r\n/***2012-4-6_16:31:18 启用 门作开关设置***/\r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(146,'Activate Door As Switch','','0','')\r\n/***2012-4-6_16:31:18 启用 有效刷卡间隔***/\r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(147,'Activate Valid Swipe Gap','','0','')\r\n/***2012-5-8_09:20:25 启用 操作员管理***/\r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(148,'Activate Operator Management','','0','')\r\n/***2012-5-16_07:47:33 分区表 ***/\r\nInsert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(53,'Created Partition','0','0','2010-8-3 16:07:48')\r\n\r\n/***2013-12-21_12:57:18 启用 验证方式设置***/\r\nInsert Into t_d_AuthModes(f_AuthMode, f_AuthDesc, f_AuthDescCh, f_AuthDevType) Values(0, 'Fingerprint|Card|Password', '指纹|卡|密码', 'A30');\r\nInsert Into t_d_AuthModes(f_AuthMode, f_AuthDesc, f_AuthDescCh, f_AuthDevType) Values(1, 'Fingerprint+Card', '指纹+卡', 'A30');\r\nInsert Into t_d_AuthModes(f_AuthMode, f_AuthDesc, f_AuthDescCh, f_AuthDevType) Values(2, 'Fingerprint+Password', '指纹+密码', 'A30');\r\nInsert Into t_d_AuthModes(f_AuthMode, f_AuthDesc, f_AuthDescCh, f_AuthDevType) Values(3, 'Card+Password', '卡+密码', 'A30');\r\nInsert Into t_d_AuthModes(f_AuthMode, f_AuthDesc, f_AuthDescCh, f_AuthDevType) Values(4, 'Fingerprint+Card+Password', '指纹+卡+密码', 'A30');\r\nInsert Into t_d_AuthModes(f_AuthMode, f_AuthDesc, f_AuthDescCh, f_AuthDevType) Values(0, 'Face|Fingerprint|Card|Password', '人脸|指纹|卡|密码', 'F500');\r\nInsert Into t_d_AuthModes(f_AuthMode, f_AuthDesc, f_AuthDescCh, f_AuthDevType) Values(1, 'Fingerprint', '指纹', 'F500');\r\nInsert Into t_d_AuthModes(f_AuthMode, f_AuthDesc, f_AuthDescCh, f_AuthDevType) Values(2, 'Card+Fingerprint', '卡+指纹', 'F500');\r\nInsert Into t_d_AuthModes(f_AuthMode, f_AuthDesc, f_AuthDescCh, f_AuthDevType) Values(3, 'Card', '卡', 'F500');\r\nInsert Into t_d_AuthModes(f_AuthMode, f_AuthDesc, f_AuthDescCh, f_AuthDevType) Values(5, 'Password', '密码', 'F500');\r\nInsert Into t_d_AuthModes(f_AuthMode, f_AuthDesc, f_AuthDescCh, f_AuthDevType) Values(6, 'Card+Password', '卡+密码', 'F500');\r\nInsert Into t_d_AuthModes(f_AuthMode, f_AuthDesc, f_AuthDescCh, f_AuthDevType) Values(7, 'Finger+Password', '指纹+密码', 'F500');\r\nInsert Into t_d_AuthModes(f_AuthMode, f_AuthDesc, f_AuthDescCh, f_AuthDevType) Values(8, 'Finger+Card+Password', '指纹+卡+密码', 'F500');\r\nInsert Into t_d_AuthModes(f_AuthMode, f_AuthDesc, f_AuthDescCh, f_AuthDevType) Values(9, 'Face', '人脸', 'F500');\r\nInsert Into t_d_AuthModes(f_AuthMode, f_AuthDesc, f_AuthDescCh, f_AuthDevType) Values(10, 'Card+Face', '卡+人脸', 'F500');\r\nInsert Into t_d_AuthModes(f_AuthMode, f_AuthDesc, f_AuthDescCh, f_AuthDevType) Values(11, 'Face+Password', '人脸+密码', 'F500');\r\nInsert Into t_d_AuthModes(f_AuthMode, f_AuthDesc, f_AuthDescCh, f_AuthDevType) Values(12, 'Face+Card+Password', '人脸+卡+密码', 'F500');\r\nInsert Into t_d_AuthModes(f_AuthMode, f_AuthDesc, f_AuthDescCh, f_AuthDevType) Values(13, 'Face+Finger', '人脸+指纹', 'F500');\r\nInsert Into t_d_AuthModes(f_AuthMode, f_AuthDesc, f_AuthDescCh, f_AuthDevType) Values(0, 'Face|Card|Password', '人脸|卡|密码', 'F300AC');\r\nInsert Into t_d_AuthModes(f_AuthMode, f_AuthDesc, f_AuthDescCh, f_AuthDevType) Values(9, 'Face', '人脸', 'F300AC');\r\nInsert Into t_d_AuthModes(f_AuthMode, f_AuthDesc, f_AuthDescCh, f_AuthDevType) Values(10, 'Card+Face', '卡+人脸', 'F300AC');\r\nInsert Into t_d_AuthModes(f_AuthMode, f_AuthDesc, f_AuthDescCh, f_AuthDevType) Values(11, 'Face+Password', '人脸+密码', 'F300AC');\r\nInsert Into t_d_AuthModes(f_AuthMode, f_AuthDesc, f_AuthDescCh, f_AuthDevType) Values(12, 'Face+Card+Password', '人脸+卡+密码', 'F300AC');\r\n\r\nInsert Into t_d_FaceTemplChanged(f_Changed) Values(0);\r\n";
        public string MSGTITLE = wgToolsAA.MSGTITLE;
        private string strDbConnection;
        private const string wgDatabaseDefaultName = "AccessData";
        private const string wgDatabaseDefaultNameOfAdroitor = "AccessData";
        private string wgDatabaseName = "AccessData";

        public frmSqlServerSetup()
        {
            InitializeComponent();

            ComponentResourceManager manager =
                new ComponentResourceManager(typeof(frmSqlServerSetup));
            manager.ApplyResources(this, "$this");
            manager.ApplyResources(this.btnAdvanced, "btnAdvanced");
            manager.ApplyResources(this.btnBackupDB, "btnBackupDB");
            manager.ApplyResources(this.btnCheckDBVersion, "btnCheckDBVersion");
            manager.ApplyResources(this.btnConfirm, "btnConfirm");
            manager.ApplyResources(this.btnCreateDB, "btnCreateDB");
            manager.ApplyResources(this.btnExit, "btnExit");
            manager.ApplyResources(this.btnRestoreDB, "btnRestoreDB");
            manager.ApplyResources(this.btnUpgradeMsAccessToSqlServer, "btnUpgradeMsAccessToSqlServer");
            manager.ApplyResources(this.btnUseMsAccessDatabase, "btnUseMsAccessDatabase");
            manager.ApplyResources(this.chkSelectDirectory, "chkSelectDirectory");
            manager.ApplyResources(this.cmdConnectTest, "cmdConnectTest");
            manager.ApplyResources(this.Label1, "Label1");
            manager.ApplyResources(this.Label3, "Label3");
            manager.ApplyResources(this.Label4, "Label4");
            manager.ApplyResources(this.Label5, "Label5");
            manager.ApplyResources(this.Label6, "Label6");
            manager.ApplyResources(this.Label7, "Label7");
            manager.ApplyResources(this.optSqlAuthentication, "optSqlAuthentication");
            manager.ApplyResources(this.optWindowsAuthentication, "optWindowsAuthentication");
        }

        private bool backupDatabase2011(string databaseName)
        {
            bool flag = false;
            if (databaseName != "")
            {
                bool flag2 = false;
                try
                {
                    SqlConnection.ClearAllPools();
                    this.strDbConnection = this.getConSql("master");
                    this.strDbConnection = this.strDbConnection + ";Connection Timeout=5";
                    SqlCommand command = new SqlCommand();
                    SqlConnection connection = new SqlConnection(this.strDbConnection);
                    string cmdText = " SELECT name FROM sysdatabases ";
                    command = new SqlCommand(cmdText, connection);
                    command.CommandTimeout = 5;
                    connection.Open();
                    object obj2 = command.ExecuteScalar();
                    connection.Close();
                    if (obj2 == null)
                    {
                        XMessageBoxAA.Show(CommonStr.strConnectFailed);
                        return flag;
                    }
                    cmdText = "SELECT  SERVERPROPERTY('productversion'), SERVERPROPERTY ('productlevel'), SERVERPROPERTY ('edition')";
                    command = new SqlCommand(cmdText, connection);
                    command.CommandTimeout = 5;
                    connection.Open();
                    object valA = command.ExecuteScalar();
                    connection.Close();
                    if (valA == null)
                    {
                        XMessageBoxAA.Show(CommonStr.strConnectFailed);
                        return flag;
                    }
                    string fileName = string.Format("{0}_sql_{1}_{2}.bak", this.wgDatabaseName, wgToolsAA.SetObjToStr(valA), DateAndTime.Now.ToString("yyyyMMdd_HHmmss"));
                    if (this.chkSelectDirectory.Checked)
                    {
                        this.saveFileDialog1.Filter = " (*.bak)|*.bak| (*.*)|*.*";
                        this.saveFileDialog1.FilterIndex = 1;
                        this.saveFileDialog1.RestoreDirectory = true;
                        this.saveFileDialog1.FileName = string.Format("{0}_sql_{1}_{2}.bak", this.wgDatabaseName, wgToolsAA.SetObjToStr(valA), DateAndTime.Now.ToString("yyyyMMdd_HHmmss"));
                        if (this.saveFileDialog1.ShowDialog(this) != DialogResult.OK)
                        {
                            return false;
                        }
                        fileName = this.saveFileDialog1.FileName;
                    }
                    connection.Open();
                    command = new SqlCommand(string.Format("BACKUP DATABASE [{0}] TO DISK = '{1}'", this.wgDatabaseName, fileName), connection);
                    Cursor.Current = Cursors.WaitCursor;
                    command.CommandTimeout = 300;
                    command.ExecuteNonQuery();
                    flag2 = true;
                    Cursor.Current = Cursors.Default;
                    XMessageBoxAA.Show(CommonStr.strBackupDatabaseSuccess);
                }
                catch (Exception exception)
                {
                    XMessageBoxAA.Show(CommonStr.strFailedToBackupDatabase + "\r\n\r\n" + exception.ToString());
                }
                finally
                {
                    flag = flag2;
                }
            }
            return flag;
        }

        private void btnAdvanced_Click(object sender, EventArgs e)
        {
            base.Size = new Size(600, 405);
        }

        private void btnBackupDB_Click(object sender, EventArgs e)
        {
            this.backupDatabase2011(this.wgDatabaseName);
        }

        private void btnCheckDBVersion_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.showDbVersion2010();
            Cursor.Current = Cursors.Default;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (this.createDatabase2010(this.wgDatabaseName, false))
            {
                XMessageBoxAA.Show(CommonStr.strSaveOK);
                base.Close();
            }
            else
            {
                XMessageBoxAA.Show(CommonStr.strFailed);
            }
        }

        private void btnCreateDB_Click(object sender, EventArgs e)
        {
            Cursor current = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            if (XMessageBoxAA.Show(CommonStr.strCreateNewDatabaseCheck, "", MessageBoxButtons.OKCancel) != DialogResult.Cancel)
            {
                try
                {
                    GC.Collect();
                    SqlConnection.ClearAllPools();
                    if (this.createDatabase2010(this.wgDatabaseName, true))
                    {
                        this.UpgradeDatabase();
                        XMessageBoxAA.Show(CommonStr.strDatabaseCreationSuccess);
                    }
                    else
                    {
                        XMessageBoxAA.Show(CommonStr.strFailedToCreateDatabase);
                    }
                }
                catch (Exception exception)
                {
                    XMessageBoxAA.Show(CommonStr.strFailedToCreateDatabase + "\r\n\r\n" + exception.Message);
                }
                finally
                {
                    Cursor.Current = current;
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnRestoreDB_Click(object sender, EventArgs e)
        {
            Cursor current = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            if (XMessageBoxAA.Show(CommonStr.strRestoreDatabaseCheck, "", MessageBoxButtons.OKCancel) != DialogResult.Cancel)
            {
                this.openFileDialog1.Filter = " (*.bak)|*.bak| (*.*)|*.*";
                this.openFileDialog1.FilterIndex = 1;
                this.openFileDialog1.RestoreDirectory = true;
                this.openFileDialog1.Title = (sender as Button).Text;
                this.openFileDialog1.FileName = "";
                if (this.openFileDialog1.ShowDialog(this) == DialogResult.OK)
                {
                    string fileName = this.openFileDialog1.FileName;
                    try
                    {
                        if (!this.restoreDatabase2011(this.wgDatabaseName, fileName))
                        {
                            XMessageBoxAA.Show(CommonStr.strFailedToRestoreDatabase);
                        }
                    }
                    catch (Exception exception)
                    {
                        XMessageBoxAA.Show(CommonStr.strFailedToRestoreDatabase + "\r\n\r\n" + exception.Message);
                    }
                    finally
                    {
                        Cursor.Current = current;
                    }
                }
            }
        }

        private void btnUpgradeMsAccessToSqlServer_Click(object sender, EventArgs e)
        {
            Cursor current = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            if (XMessageBoxAA.Show(CommonStr.strUpgradeDatabaseFromMsAccessCheck, "", MessageBoxButtons.OKCancel) != DialogResult.Cancel)
            {
                string fileName = "";
                this.openFileDialog1.Filter = " (*.mdb)|*.mdb| (*.*)|*.*";
                this.openFileDialog1.FilterIndex = 1;
                this.openFileDialog1.RestoreDirectory = true;
                this.openFileDialog1.InitialDirectory = Application.StartupPath;
                this.openFileDialog1.Title = (sender as Button).Text;
                this.openFileDialog1.FileName = accessDbName;
                if (this.openFileDialog1.ShowDialog(this) == DialogResult.OK)
                {
                    fileName = this.openFileDialog1.FileName;
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        this.lblRunInfo.Visible = true;
                        try
                        {
                            GC.Collect();
                            SqlConnection.ClearAllPools();
                            Cursor.Current = Cursors.WaitCursor;
                            if (this.createDatabase2010(this.wgDatabaseName, true))
                            {
                                this.UpgradeDatabase();
                                if (this.upgradeFromAccess(fileName))
                                {
                                    this.UpgradeDatabase();
                                    XMessageBoxAA.Show(CommonStr.strUpgradeSuccess);
                                    return;
                                }
                            }
                            XMessageBoxAA.Show(CommonStr.strFailed);
                        }
                        catch (Exception exception)
                        {
                            XMessageBoxAA.Show(CommonStr.strFailed + "\r\n\r\n" + exception.Message);
                        }
                        finally
                        {
                            this.lblRunInfo.Visible = false;
                            Cursor.Current = current;
                        }
                    }
                }
            }
        }

        private void btnUseMsAccessDatabase_Click(object sender, EventArgs e)
        {
            if (XMessageBoxAA.Show(CommonStr.strSelectMsAccessDatabaseCheck, "", MessageBoxButtons.OKCancel) != DialogResult.Cancel)
            {
                wgToolsAA.UpdateKeyVal("dbConnection", "");
                XMessageBoxAA.Show(CommonStr.strUseAccessSuccess);
                base.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OleDbConnection connection = new OleDbConnection(string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source= {0}.mdb;User ID=admin;Password=;JET OLEDB:Database Password=passaccess", accessDbName));
            connection.Open();
            object[] restrictions = new object[4];
            restrictions[3] = "TABLE";
            DataTable oleDbSchemaTable = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, restrictions);
            for (int i = 0; i < oleDbSchemaTable.Rows.Count; i++)
            {
                if (wgToolsAA.SetObjToStr(oleDbSchemaTable.Rows[i]["TABLE_TYPE"]) == "TABLE")
                {
                    object[] objArray2 = new object[4];
                    objArray2[2] = wgToolsAA.SetObjToStr(oleDbSchemaTable.Rows[i]["table_name"]);
                    connection.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, objArray2);
                }
            }
        }

        private void cboDBs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cboDBs.Visible)
            {
                this.txtDBName.Text = this.cboDBs.Text;
            }
        }

        private void cmdConnectTest_Click(object sender, EventArgs e)
        {
            this.bSqlExress = false;
            if (this.ConnectTest2010())
            {
                this.btnCheckDBVersion.Enabled = true;
                this.btnCreateDB.Enabled = true;
                this.btnConfirm.Enabled = true;
                this.btnRestoreDB.Enabled = true;
                this.btnUpgradeMsAccessToSqlServer.Enabled = true;
                XMessageBoxAA.Show(CommonStr.strConnectSuccessfully);
            }
            else
            {
                this.bSqlExress = true;
                if ((this.bSqlExress && (this.txtServer.Text.IndexOf(@"\", 2) <= 0)) && this.ConnectTest2010())
                {
                    this.txtServer.Text = this.txtServer.Text + @"\sqlexpress";
                    this.btnCheckDBVersion.Enabled = true;
                    this.btnCreateDB.Enabled = true;
                    this.btnConfirm.Enabled = true;
                    this.btnRestoreDB.Enabled = true;
                    this.btnUpgradeMsAccessToSqlServer.Enabled = true;
                    XMessageBoxAA.Show(CommonStr.strConnectSuccessfully);
                }
                else
                {
                    XMessageBoxAA.Show(CommonStr.strConnectFailed);
                }
            }
        }

        private bool ConnectTest2010()
        {
            Cursor.Current = Cursors.WaitCursor;
            bool flag = false;
            try
            {
                string connectionString = this.getConSql("master") + ";Connection Timeout=5";
                SqlCommand command = new SqlCommand();
                SqlConnection connection = new SqlConnection(connectionString);
                string cmdText = " SELECT name FROM sysdatabases ";
                command = new SqlCommand(cmdText, connection);
                command.CommandTimeout = 5;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                bool flag2 = false;
                this.cboDBs.Items.Clear();
                while (reader.Read())
                {
                    flag2 = true;
                    this.cboDBs.Items.Add(reader[0]);
                }
                connection.Close();
                if (!flag2)
                {
                    return false;
                }
                flag = true;
            }
            catch (Exception)
            {
                flag = false;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
            return flag;
        }

        private bool createDatabase2010(string databaseName, bool bCreate)
        {
            bool flag = false;
            if (databaseName != "")
            {
                bool flag2 = false;
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    this.strDbConnection = this.getConSql("master");
                    this.strDbConnection = this.strDbConnection + ";Connection Timeout=5";
                    if (!bCreate)
                    {
                        wgToolsAA.UpdateKeyVal("dbConnection", this.getConSql(databaseName));
                        flag2 = true;
                        return true;
                    }
                    Cursor.Current = Cursors.WaitCursor;
                    SqlCommand command = new SqlCommand();
                    SqlConnection connection = new SqlConnection(this.strDbConnection);
                    string cmdText = " SELECT name FROM sysdatabases ";
                    command = new SqlCommand(cmdText, connection);
                    command.CommandTimeout = 5;
                    connection.Open();
                    object obj2 = command.ExecuteScalar();
                    connection.Close();
                    if (obj2 == null)
                    {
                        XMessageBoxAA.Show(CommonStr.strConnectFailed);
                        return flag;
                    }
                    cmdText = " SELECT  convert( int, LEFT(convert(nvarchar,SERVERPROPERTY('ProductVersion')),CHARINDEX('.',convert(nvarchar,SERVERPROPERTY('ProductVersion')))-1)) ";
                    command = new SqlCommand(cmdText, connection);
                    command.CommandTimeout = 5;
                    connection.Open();
                    object obj3 = command.ExecuteScalar();
                    connection.Close();
                    if (obj3 == null)
                    {
                        XMessageBoxAA.Show(CommonStr.strConnectFailed);
                        return flag;
                    }
                    connection.Open();
                    command = new SqlCommand(string.Format("IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N{0})", wgToolsAA.PrepareStr(this.wgDatabaseName)) + "\r\n DROP DATABASE [" + this.wgDatabaseName + "]", connection);
                    command.CommandTimeout = 300;
                    command.ExecuteNonQuery();
                    cmdText = " CREATE DATABASE [" + this.wgDatabaseName + "] ";
                    Cursor.Current = Cursors.WaitCursor;
                    command = new SqlCommand(cmdText, connection);
                    command.CommandTimeout = 300;
                    command.ExecuteNonQuery();
                    command = new SqlCommand(this.defaultDBFileNameSqlString.Replace("AccessData", this.wgDatabaseName).Replace("\r\nGO", "\r\n").Replace(" COLLATE Chinese_PRC_CI_AS", " "), connection);
                    command.CommandTimeout = 300;
                    Cursor.Current = Cursors.WaitCursor;
                    command.ExecuteNonQuery();
                    Cursor.Current = Cursors.WaitCursor;
                    try
                    {
                        cmdText = " CREATE PARTITION FUNCTION [RangePrivilegePF1](int) AS RANGE LEFT FOR VALUES (N'1',N'2',N'3',N'4',N'5',N'6',N'7',N'8',N'9',N'10',N'11',N'12',N'13',N'14',N'15',N'16',N'17',N'18',N'19',N'20',N'21',N'22',N'23',N'24',N'25',N'26',N'27',N'28',N'29',N'30',N'31',N'32',N'33',N'34',N'35',N'36',N'37',N'38',N'39',N'40',N'41',N'42',N'43',N'44',N'45',N'46',N'47',N'48',N'49',N'50',N'51',N'52',N'53',N'54',N'55',N'56',N'57',N'58',N'59',N'60',N'61',N'62',N'63',N'64',N'65',N'66',N'67',N'68',N'69',N'70',N'71',N'72',N'73',N'74',N'75',N'76',N'77',N'78',N'79',N'80',N'81',N'82',N'83',N'84',N'85',N'86',N'87',N'88',N'89',N'90',N'91',N'92',N'93',N'94',N'95',N'96',N'97',N'98',N'99',N'100',N'101',N'102',N'103',N'104',N'105',N'106',N'107',N'108',N'109',N'110',N'111',N'112',N'113',N'114',N'115',N'116',N'117',N'118',N'119',N'120',N'121',N'122',N'123',N'124',N'125',N'126',N'127',N'128',N'129',N'130',N'131',N'132',N'133',N'134',N'135',N'136',N'137',N'138',N'139',N'140',N'141',N'142',N'143',N'144',N'145',N'146',N'147',N'148',N'149',N'150',N'151',N'152',N'153',N'154',N'155',N'156',N'157',N'158',N'159',N'160',N'161',N'162',N'163',N'164',N'165',N'166',N'167',N'168',N'169',N'170',N'171',N'172',N'173',N'174',N'175',N'176',N'177',N'178',N'179',N'180',N'181',N'182',N'183',N'184',N'185',N'186',N'187',N'188',N'189',N'190',N'191',N'192',N'193',N'194',N'195',N'196',N'197',N'198',N'199',N'200',N'201',N'202',N'203',N'204',N'205',N'206',N'207',N'208',N'209',N'210',N'211',N'212',N'213',N'214',N'215',N'216',N'217',N'218',N'219',N'220',N'221',N'222',N'223',N'224',N'225',N'226',N'227',N'228',N'229',N'230',N'231',N'232',N'233',N'234',N'235',N'236',N'237',N'238',N'239',N'240',N'241',N'242',N'243',N'244',N'245',N'246',N'247',N'248',N'249',N'250',N'251',N'252',N'253',N'254',N'255',N'256',N'257',N'258',N'259',N'260',N'261',N'262',N'263',N'264',N'265',N'266',N'267',N'268',N'269',N'270',N'271',N'272',N'273',N'274',N'275',N'276',N'277',N'278',N'279',N'280',N'281',N'282',N'283',N'284',N'285',N'286',N'287',N'288',N'289',N'290',N'291',N'292',N'293',N'294',N'295',N'296',N'297',N'298',N'299',N'300',N'301',N'302',N'303',N'304',N'305',N'306',N'307',N'308',N'309',N'310',N'311',N'312',N'313',N'314',N'315',N'316',N'317',N'318',N'319',N'320',N'321',N'322',N'323',N'324',N'325',N'326',N'327',N'328',N'329',N'330',N'331',N'332',N'333',N'334',N'335',N'336',N'337',N'338',N'339',N'340',N'341',N'342',N'343',N'344',N'345',N'346',N'347',N'348',N'349',N'350',N'351',N'352',N'353',N'354',N'355',N'356',N'357',N'358',N'359',N'360',N'361',N'362',N'363',N'364',N'365',N'366',N'367',N'368',N'369',N'370',N'371',N'372',N'373',N'374',N'375',N'376',N'377',N'378',N'379',N'380',N'381',N'382',N'383',N'384',N'385',N'386',N'387',N'388',N'389',N'390',N'391',N'392',N'393',N'394',N'395',N'396',N'397',N'398',N'399',N'400',N'401',N'402',N'403',N'404',N'405',N'406',N'407',N'408',N'409',N'410',N'411',N'412',N'413',N'414',N'415',N'416',N'417',N'418',N'419',N'420',N'421',N'422',N'423',N'424',N'425',N'426',N'427',N'428',N'429',N'430',N'431',N'432',N'433',N'434',N'435',N'436',N'437',N'438',N'439',N'440',N'441',N'442',N'443',N'444',N'445',N'446',N'447',N'448',N'449',N'450',N'451',N'452',N'453',N'454',N'455',N'456',N'457',N'458',N'459',N'460',N'461',N'462',N'463',N'464',N'465',N'466',N'467',N'468',N'469',N'470',N'471',N'472',N'473',N'474',N'475',N'476',N'477',N'478',N'479',N'480',N'481',N'482',N'483',N'484',N'485',N'486',N'487',N'488',N'489',N'490',N'491',N'492',N'493',N'494',N'495',N'496',N'497',N'498',N'499',N'500',N'501',N'502',N'503',N'504',N'505',N'506',N'507',N'508',N'509',N'510',N'511',N'512',N'513',N'514',N'515',N'516',N'517',N'518',N'519',N'520',N'521',N'522',N'523',N'524',N'525',N'526',N'527',N'528',N'529',N'530',N'531',N'532',N'533',N'534',N'535',N'536',N'537',N'538',N'539',N'540',N'541',N'542',N'543',N'544',N'545',N'546',N'547',N'548',N'549',N'550',N'551',N'552',N'553',N'554',N'555',N'556',N'557',N'558',N'559',N'560',N'561',N'562',N'563',N'564',N'565',N'566',N'567',N'568',N'569',N'570',N'571',N'572',N'573',N'574',N'575',N'576',N'577',N'578',N'579',N'580',N'581',N'582',N'583',N'584',N'585',N'586',N'587',N'588',N'589',N'590',N'591',N'592',N'593',N'594',N'595',N'596',N'597',N'598',N'599',N'600',N'601',N'602',N'603',N'604',N'605',N'606',N'607',N'608',N'609',N'610',N'611',N'612',N'613',N'614',N'615',N'616',N'617',N'618',N'619',N'620',N'621',N'622',N'623',N'624',N'625',N'626',N'627',N'628',N'629',N'630',N'631',N'632',N'633',N'634',N'635',N'636',N'637',N'638',N'639',N'640',N'641',N'642',N'643',N'644',N'645',N'646',N'647',N'648',N'649',N'650',N'651',N'652',N'653',N'654',N'655',N'656',N'657',N'658',N'659',N'660',N'661',N'662',N'663',N'664',N'665',N'666',N'667',N'668',N'669',N'670',N'671',N'672',N'673',N'674',N'675',N'676',N'677',N'678',N'679',N'680',N'681',N'682',N'683',N'684',N'685',N'686',N'687',N'688',N'689',N'690',N'691',N'692',N'693',N'694',N'695',N'696',N'697',N'698',N'699',N'700',N'701',N'702',N'703',N'704',N'705',N'706',N'707',N'708',N'709',N'710',N'711',N'712',N'713',N'714',N'715',N'716',N'717',N'718',N'719',N'720',N'721',N'722',N'723',N'724',N'725',N'726',N'727',N'728',N'729',N'730',N'731',N'732',N'733',N'734',N'735',N'736',N'737',N'738',N'739',N'740',N'741',N'742',N'743',N'744',N'745',N'746',N'747',N'748',N'749',N'750',N'751',N'752',N'753',N'754',N'755',N'756',N'757',N'758',N'759',N'760',N'761',N'762',N'763',N'764',N'765',N'766',N'767',N'768',N'769',N'770',N'771',N'772',N'773',N'774',N'775',N'776',N'777',N'778',N'779',N'780',N'781',N'782',N'783',N'784',N'785',N'786',N'787',N'788',N'789',N'790',N'791',N'792',N'793',N'794',N'795',N'796',N'797',N'798',N'799',N'800',N'801',N'802',N'803',N'804',N'805',N'806',N'807',N'808',N'809',N'810',N'811',N'812',N'813',N'814',N'815',N'816',N'817',N'818',N'819',N'820',N'821',N'822',N'823',N'824',N'825',N'826',N'827',N'828',N'829',N'830',N'831',N'832',N'833',N'834',N'835',N'836',N'837',N'838',N'839',N'840',N'841',N'842',N'843',N'844',N'845',N'846',N'847',N'848',N'849',N'850',N'851',N'852',N'853',N'854',N'855',N'856',N'857',N'858',N'859',N'860',N'861',N'862',N'863',N'864',N'865',N'866',N'867',N'868',N'869',N'870',N'871',N'872',N'873',N'874',N'875',N'876',N'877',N'878',N'879',N'880',N'881',N'882',N'883',N'884',N'885',N'886',N'887',N'888',N'889',N'890',N'891',N'892',N'893',N'894',N'895',N'896',N'897',N'898',N'899',N'900',N'901',N'902',N'903',N'904',N'905',N'906',N'907',N'908',N'909',N'910',N'911',N'912',N'913',N'914',N'915',N'916',N'917',N'918',N'919',N'920',N'921',N'922',N'923',N'924',N'925',N'926',N'927',N'928',N'929',N'930',N'931',N'932',N'933',N'934',N'935',N'936',N'937',N'938',N'939',N'940',N'941',N'942',N'943',N'944',N'945',N'946',N'947',N'948',N'949',N'950',N'951',N'952',N'953',N'954',N'955',N'956',N'957',N'958',N'959',N'960',N'961',N'962',N'963',N'964',N'965',N'966',N'967',N'968',N'969',N'970',N'971',N'972',N'973',N'974',N'975',N'976',N'977',N'978',N'979',N'980',N'981',N'982',N'983',N'984',N'985',N'986',N'987',N'988',N'989',N'990',N'991',N'992',N'993',N'994',N'995',N'996',N'997',N'998',N'999') \r\n     \r\nCREATE PARTITION SCHEME [RangePrivilegePS1] AS PARTITION [RangePrivilegePF1] TO ([PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY])\r\n/****** 对象: 表 [dbo].[t_d_Privilege]    脚本日期: 2010-05-31 13:39:40 ******/\r\nif exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[t_d_Privilege]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\r\ndrop table [dbo].[t_d_Privilege]\r\n\r\n\r\nCREATE TABLE [dbo].[t_d_Privilege](\r\n\t[f_PrivilegeRecID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,\r\n\t[f_DoorID] [int] NOT NULL,\r\n\t[f_ControlSegID] [int] NOT NULL,\r\n\t[f_ConsumerID] [int] NOT NULL,\r\n\t[f_ControllerID] [int] NOT NULL,\r\n\t[f_DoorNO] [tinyint] NOT NULL,\r\n    CONSTRAINT [PK_t_d_Privilege] PRIMARY KEY CLUSTERED \r\n    (\r\n\t[f_ControllerID] ASC,\r\n\t[f_PrivilegeRecID] ASC\r\n    )  ON [RangePrivilegePS1](f_ControllerID) \r\n)ON [PRIMARY] \r\n\r\nCREATE NONCLUSTERED INDEX [_dta_index_t_d_Privilege_12_1810105489__K4_1_2_3_5] ON [dbo].[t_d_Privilege] \r\n(\r\n\t[f_ConsumerID] ASC\r\n)\r\nINCLUDE ( [f_PrivilegeRecID],\r\n[f_DoorID],\r\n[f_ControlSegID],\r\n[f_ControllerID]) WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]\r\n\r\nUpdate t_a_SystemParam SET f_EName='1', f_Value ='1'  WHERE f_No=53 \r\n";
                        command = new SqlCommand(cmdText, connection);
                        command.CommandTimeout = 300;
                        command.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                    }
                    connection.Close();
                    flag2 = true;
                    wgToolsAA.UpdateKeyVal("dbConnection", this.getConSql(databaseName));
                }
                catch (Exception exception)
                {
                    XMessageBoxAA.Show(CommonStr.strFailedToCreateDatabase + "\r\n\r\n" + exception.ToString());
                }
                finally
                {
                    flag = flag2;
                }
            }
            return flag;
        }

        private void frmSqlServerSetup_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Control && e.Shift) && (e.KeyValue == 0x51))
            {
                this.cboDBs.Visible = true;
                this.btnBackupDB.Enabled = true;
                this.Label7.Visible = true;
                this.chkSelectDirectory.Checked = true;
                this.chkSelectDirectory.Visible = true;
            }
            if (e.KeyValue == 0x70)
            {
                try
                {
                    string keyVal = wgToolsAA.GetKeyVal("dbConnection");
                    if (!string.IsNullOrEmpty(keyVal))
                    {
                        string[] strArray = keyVal.Split(new char[] { ';' });
                        string[] strArray2 = new string[strArray.Length];
                        string[] strArray3 = new string[strArray.Length];
                        for (int i = 0; i < strArray.Length; i++)
                        {
                            string[] strArray4 = strArray[i].Split(new char[] { '=' });
                            if (strArray4.Length == 2)
                            {
                                strArray2[i] = strArray4[0];
                                strArray3[i] = strArray4[1];
                            }
                            else if (strArray4.Length == 1)
                            {
                                strArray2[i] = strArray4[0];
                                strArray3[i] = "";
                            }
                        }
                        if (keyVal.ToUpper().IndexOf("integrated security=SSPI;persist security info=True".ToUpper()) > 0)
                        {
                            this.optWindowsAuthentication.Checked = true;
                        }
                        else
                        {
                            this.optWindowsAuthentication.Checked = false;
                            this.optSqlAuthentication.Checked = true;
                        }
                        for (int j = 0; j < strArray.Length; j++)
                        {
                            if (!string.IsNullOrEmpty(strArray2[j]))
                            {
                                if (strArray2[j].ToUpper() == "Password".ToUpper())
                                {
                                    this.txtPasswd.Text = strArray3[j];
                                }
                                if (strArray2[j].ToUpper() == "User ID".ToUpper())
                                {
                                    this.txtUsername.Text = strArray3[j];
                                }
                                if (strArray2[j].ToUpper() == "initial catalog".ToUpper())
                                {
                                    this.txtDBName.Text = strArray3[j];
                                }
                                if (strArray2[j].ToUpper() == "data source".ToUpper())
                                {
                                    this.txtServer.Text = strArray3[j];
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private void frmSqlServerSetup_Load(object sender, EventArgs e)
        {
            Icon appicon = base.Icon;
            wgToolsAA.GetAppIcon(ref appicon);
            base.Icon = appicon;
            this.txtDBName.Text = "AccessData";

            string strProcessName = Process.GetCurrentProcess().ProcessName;

            // check if this process name is existing in the current running processes
            Process[] processes = Process.GetProcessesByName(strProcessName);

            // if its existing then exit

            if (processes.Length > 1)
            {
                XMessageBoxAA.Show(CommonStr.strApplicationAlreadyRunning);
                Close();
            }
        }

        private string GenerateInsertStatement(DataRow dr, int row, SqlCommand command)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i <= (dr.Table.Columns.Count - 1); i++)
            {
                if (dr.Table.Columns[i].ColumnName == "f_Templ" ||
                    dr.Table.Columns[i].ColumnName == "f_Photo")
                {
                    if (dr[i] != DBNull.Value)
                    {
                        string param = "@param" + row.ToString();
                        builder.Append(param);
                        command.Parameters.AddWithValue(param, (byte[])dr[i]);
                    }
                    else
                    {
                        builder.Append("NULL");
                    }
                }
                else
                {
                    if (!object.ReferenceEquals(dr.Table.Columns[i].DataType, typeof(int)))
                    {
                        if (string.IsNullOrEmpty(dr[i].ToString()))
                        {
                            builder.Append("NULL");
                        }
                        else
                        {
                            builder.Append("'");
                            builder.Append(dr[i].ToString().Replace("'", "''"));
                            builder.Append("'");
                        }
                    }
                    else if (string.IsNullOrEmpty(dr[i].ToString()))
                    {
                        builder.Append("NULL");
                    }
                    else
                    {
                        builder.Append(dr[i].ToString().Replace("'", "''"));
                    }
                }
                if (i != (dr.Table.Columns.Count - 1))
                {
                    builder.Append(",");
                }
            }
            builder.Append(")");
            return builder.ToString().Replace("\r\n", " ");
        }

        private string getConSql(string dbName)
        {
            string str;
            if (this.optWindowsAuthentication.Checked)
            {
                str = string.Format("data source={0};initial catalog={1};integrated security=SSPI;persist security info=True", this.txtServer.Text, dbName);
            }
            else
            {
                str = string.Format("data source={0};initial catalog={1};Password={2};Persist Security Info=True;User ID={3}", new object[] { this.txtServer.Text, dbName, this.txtPasswd.Text, this.txtUsername.Text });
            }
            if (!this.bSqlExress || (this.txtServer.Text.IndexOf(@"\", 2) > 0))
            {
                return str;
            }
            if (this.optWindowsAuthentication.Checked)
            {
                return string.Format(@"data source={0}\sqlexpress;initial catalog={1};integrated security=SSPI;persist security info=True", this.txtServer.Text, dbName);
            }
            return string.Format(@"data source={0}\sqlexpress;initial catalog={1};Password={2};Persist Security Info=True;User ID={3}", new object[] { this.txtServer.Text, dbName, this.txtPasswd.Text, this.txtUsername.Text });
        }

        private static string getOperatorPrivilegeInsertSql(int functionId, string functionName, string displayName)
        {
            return string.Format("Insert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl)  SELECT t_s_OperatorPrivilege.f_OperatorID,{0} as f_FunctionID,{1} as f_FunctionName ,{2} as f_FunctionDisplayName,0 as f_ReadOnly,1 as f_FullControl FROM  t_s_OperatorPrivilege WHERE t_s_OperatorPrivilege.f_functionID = 1  AND t_s_OperatorPrivilege.f_OperatorID NOT IN (SELECT t_s_OperatorPrivilege.f_OperatorID  FROM  t_s_OperatorPrivilege  WHERE t_s_OperatorPrivilege.f_functionID = {0} )", functionId, wgToolsPrepareStr(functionName), wgToolsPrepareStr(displayName));
        }

        private void optSqlAuthentication_CheckedChanged(object sender, EventArgs e)
        {
            this.txtPasswd.Enabled = this.optSqlAuthentication.Checked;
            this.txtUsername.Enabled = this.optSqlAuthentication.Checked;
        }

        private bool restoreDatabase2011(string databaseName, string oldDBBackupFile)
        {
            bool flag = false;
            if (databaseName != "")
            {
                bool flag2 = false;
                try
                {
                    SqlConnection.ClearAllPools();
                    this.strDbConnection = this.getConSql("master");
                    this.strDbConnection = this.strDbConnection + ";Connection Timeout=5";
                    SqlCommand command = new SqlCommand();
                    SqlConnection connection = new SqlConnection(this.strDbConnection);
                    string cmdText = " SELECT name FROM sysdatabases ";
                    command = new SqlCommand(cmdText, connection);
                    command.CommandTimeout = 5;
                    connection.Open();
                    object obj2 = command.ExecuteScalar();
                    connection.Close();
                    if (obj2 == null)
                    {
                        XMessageBoxAA.Show(CommonStr.strConnectFailed);
                        return flag;
                    }
                    cmdText = " SELECT  convert( int, LEFT(convert(nvarchar,SERVERPROPERTY('ProductVersion')),CHARINDEX('.',convert(nvarchar,SERVERPROPERTY('ProductVersion')))-1)) ";
                    command = new SqlCommand(cmdText, connection);
                    command.CommandTimeout = 5;
                    connection.Open();
                    object obj3 = command.ExecuteScalar();
                    connection.Close();
                    if (obj3 == null)
                    {
                        XMessageBoxAA.Show(CommonStr.strConnectFailed);
                        return flag;
                    }
                    connection.Open();
                    command = new SqlCommand(string.Format("IF NOT EXISTS (SELECT * FROM master.dbo.sysdatabases WHERE name = N{0})", wgToolsAA.PrepareStr(this.wgDatabaseName)) + "\r\n CREATE DATABASE [" + this.wgDatabaseName + "] ", connection);
                    command.CommandTimeout = 300;
                    command.ExecuteNonQuery();
                    command = new SqlCommand(string.Format("SELECT * FROM master.dbo.sysdatabases WHERE name = N{0}", wgToolsAA.PrepareStr(this.wgDatabaseName)), connection);
                    command.CommandTimeout = 300;
                    string str3 = "";
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        str3 = wgToolsAA.SetObjToStr(reader["filename"]);
                    }
                    reader.Close();
                    command = new SqlCommand(string.Format("RESTORE FILELISTONLY FROM DISK = '{1}'  ", this.wgDatabaseName, oldDBBackupFile), connection);
                    command.CommandTimeout = 300;
                    string str4 = "";
                    string str5 = "";
                    try
                    {
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader["type"].ToString() == "D")
                            {
                                str4 = wgToolsAA.SetObjToStr(reader["LogicalName"]);
                            }
                            else if (reader["type"].ToString() == "L")
                            {
                                str5 = wgToolsAA.SetObjToStr(reader["LogicalName"]);
                            }
                        }
                        reader.Close();
                    }
                    catch (Exception exception)
                    {
                        wgToolsAA.wgDebugWrite(exception.ToString());
                    }
                    if ((str4 != "") && (str5 != ""))
                    {
                        cmdText = string.Format("RESTORE DATABASE [{0}] FROM DISK = '{1}'   WITH REPLACE, MOVE '{2}' TO '{3}', MOVE '{4}' TO '{5}'  ", new object[] { this.wgDatabaseName, oldDBBackupFile, str4, str3, str5, str3.Replace(".mdf", "_log.ldf") });
                    }
                    else if (str4 != "")
                    {
                        cmdText = string.Format("RESTORE DATABASE [{0}] FROM DISK = '{1}'   WITH REPLACE, MOVE '{2}' TO '{3}' ", new object[] { this.wgDatabaseName, oldDBBackupFile, str4, str3 });
                    }
                    else if (str5 != "")
                    {
                        cmdText = string.Format("RESTORE DATABASE [{0}] FROM DISK = '{1}'   WITH REPLACE, MOVE '{2}' TO '{3}', MOVE '{4}' TO '{5}'  ", new object[] { this.wgDatabaseName, oldDBBackupFile, str5, str3.Replace(".mdf", "_log.ldf") });
                    }
                    else
                    {
                        cmdText = string.Format("RESTORE DATABASE [{0}] FROM DISK = '{1}'   WITH REPLACE ", this.wgDatabaseName, oldDBBackupFile);
                    }
                    try
                    {
                        command = new SqlCommand(cmdText, connection);
                        command.CommandTimeout = 300;
                        Cursor.Current = Cursors.WaitCursor;
                        command.ExecuteNonQuery();
                    }
                    catch (Exception exception2)
                    {
                        wgToolsAA.wgDebugWrite(exception2.ToString());
                        command = new SqlCommand(string.Format("RESTORE DATABASE [{0}] FROM DISK = '{1}'   WITH REPLACE ", this.wgDatabaseName, oldDBBackupFile), connection);
                        command.CommandTimeout = 300;
                        Cursor.Current = Cursors.WaitCursor;
                        command.ExecuteNonQuery();
                    }
                    flag2 = true;
                    Cursor.Current = Cursors.Default;
                    XMessageBoxAA.Show(CommonStr.strRestoreSuccess);
                }
                catch (Exception exception3)
                {
                    XMessageBoxAA.Show(CommonStr.strFailedToRestoreDatabase + "\r\n\r\n" + exception3.ToString());
                }
                finally
                {
                    flag = flag2;
                }
            }
            return flag;
        }

        private void showDbVersion2010()
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                string str;
                if (this.optWindowsAuthentication.Checked)
                {
                    str = string.Format("data source={0};initial catalog={1};integrated security=SSPI;persist security info=True", this.txtServer.Text, "master");
                }
                else
                {
                    str = string.Format("data source={0};initial catalog={1};Password={2};Persist Security Info=True;User ID={3}", new object[] { this.txtServer.Text, "master", this.txtPasswd.Text, this.txtUsername.Text });
                }
                SqlCommand command = new SqlCommand();
                SqlConnection connection = new SqlConnection(str);
                command = new SqlCommand(" SELECT name FROM sysdatabases WHERE name =" + wgToolsAA.PrepareStr(this.txtDBName.Text), connection);
                command.CommandTimeout = 5;
                connection.Open();
                object valA = command.ExecuteScalar();
                connection.Close();
                if (valA == null)
                {
                    XMessageBoxAA.Show(CommonStr.strDBNotExist);
                }
                else
                {
                    if (this.optWindowsAuthentication.Checked)
                    {
                        str = string.Format("data source={0};initial catalog={1};integrated security=SSPI;persist security info=True", this.txtServer.Text, this.txtDBName.Text);
                    }
                    else
                    {
                        str = string.Format("data source={0};initial catalog={1};Password={2};Persist Security Info=True;User ID={3}", new object[] { this.txtServer.Text, this.txtDBName.Text, this.txtPasswd.Text, this.txtUsername.Text });
                    }
                    connection = new SqlConnection(str);
                    string cmdText = " SELECT f_Value FROM t_a_SystemParam WHERE f_No = 9";
                    command = new SqlCommand(cmdText, connection);
                    command.CommandTimeout = 5;
                    connection.Open();
                    valA = command.ExecuteScalar();
                    connection.Close();
                    if (valA != null)
                    {
                        string text = this.txtDBName.Text;
                        string strDBExist = CommonStr.strDBExist;
                        string strVersion = CommonStr.strVersion;
                        string str6 = text + ":\t" + strDBExist;
                        text = str6 + "\r\n" + strVersion + ":\t" + wgToolsAA.SetObjToStr(valA);
                        this.btnBackupDB.Enabled = true;
                        XMessageBoxAA.Show(text);
                    }
                }
            }
            catch (Exception)
            {
                XMessageBoxAA.Show(CommonStr.strConnectFailed);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void txtDBName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.txtDBName.Text == "")
                {
                    this.wgDatabaseName = "AccessData";
                }
                else
                {
                    this.wgDatabaseName = this.txtDBName.Text.Trim();
                }
            }
            catch (Exception exception)
            {
                XMessageBoxAA.Show(CommonStr.strFailed);
                XMessageBoxAA.Show(exception.ToString());
            }
        }

        private void UpgradeDatabase()
        {
            wgToolsAA.dbConString = wgToolsAA.GetKeyVal("dbConnection");
            SqlConnection connection = new SqlConnection(wgToolsAA.dbConString);
            try
            {
                string cmdText = "";
                float result = 0f;
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                cmdText = "SELECT f_Value FROM t_a_SystemParam WHERE f_No=9 ";
                using (SqlCommand command = new SqlCommand(cmdText, connection))
                {
                    float.TryParse(command.ExecuteScalar().ToString(), out result);
                }
                if (result > dbVersionNewest)
                {
                    Application.Exit();
                }
                else
                {
                    UpgradeDatabase_common(result);
                    if (result != dbVersionNewest)
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }
                        wgToolsAA.setSystemParamValue(9, "Database Version", dbVersionNewest.ToString(), string.Concat(new object[] { "V", result, " => V", dbVersionNewest }));
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                connection.Dispose();
            }
        }

        public static void UpgradeDatabase_common(float dbversion)
        {
            string strSql = "";
            if (dbversion == 73f)
            {
                if (wgAppConfigGetSystemParamByNO(0x92) == "")
                {
                    strSql = "Insert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(146,'Activate Door As Switch','','0','')";
                    wgAppConfigRunUpdateSql(strSql);
                }
                if (wgAppConfigGetSystemParamByNO(0x93) == "")
                {
                    strSql = "Insert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(147,'Activate Valid Swipe Gap','','0','')";
                    wgAppConfigRunUpdateSql(strSql);
                }
                if (wgAppConfigGetSystemParamByNO(0x94) == "")
                {
                    strSql = "Insert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(148,'Activate Operator Management','','0','')";
                    wgAppConfigRunUpdateSql(strSql);
                }
            }
            if (dbversion <= 73.1f)
            {
                try
                {
                    strSql = "Insert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(149,'Activate Meeting','','0','')";
                    wgAppConfigRunUpdateSql(strSql);
                }
                catch (Exception exception)
                {
                    wgToolsWgDebugWrite(exception.ToString());
                }
                try
                {
                    try
                    {
                        if (!wgAppConfigIsAccessDB())
                        {
                            strSql = "CREATE TABLE  [t_d_Reader4Meeting] ( ";
                            wgAppConfigRunUpdateSql(strSql + "f_MeetingNO   [nvarchar] (15)   NOT NULL," + "[f_ReaderID] INT  NULL )");
                        }
                        else
                        {
                            strSql = "CREATE TABLE  [t_d_Reader4Meeting] ( ";
                            wgAppConfigRunUpdateSql(strSql + "f_MeetingNO TEXT (15) NOT NULL," + "[f_ReaderID] INT  NULL )");
                        }
                    }
                    catch (Exception exception2)
                    {
                        wgToolsWgDebugWrite(exception2.ToString());
                    }
                    try
                    {
                        if (!wgAppConfigIsAccessDB())
                        {
                            strSql = "CREATE TABLE t_d_Meeting ( ";
                            wgAppConfigRunUpdateSql((((strSql + "f_MeetingNO   [nvarchar] (15)   NOT NULL," + "f_MeetingName   [nvarchar] (255)  NULL,") + "f_MeetingAdr   [nvarchar] (255)  NULL," + "f_MeetingDateTime   DATETIME NOT NULL,") + "f_SignStartTime   DATETIME NOT NULL," + "f_SignEndTime   DATETIME NOT NULL,") + "f_Content   [nvarchar] (255)  NULL," + "f_Notes      [ntext]  NULL  )");
                            strSql = " ALTER TABLE [t_d_Meeting] WITH NOCHECK ADD ";
                            wgAppConfigRunUpdateSql(strSql + "\tCONSTRAINT [PK_t_d_Meeting] PRIMARY KEY  CLUSTERED " + " ([f_MeetingNO])  ");
                        }
                        else
                        {
                            strSql = "CREATE TABLE t_d_Meeting ( ";
                            wgAppConfigRunUpdateSql(((((strSql + "f_MeetingNO TEXT (15) NOT NULL,") + "f_MeetingName TEXT (255) NULL ," + "f_MeetingAdr TEXT (255) NULL ,") + "f_MeetingDateTime   DATETIME NOT NULL," + "f_SignStartTime   DATETIME NOT NULL,") + "f_SignEndTime   DATETIME NOT NULL," + "f_Content TEXT (255) NULL ,") + "f_Notes MEMO ," + "CONSTRAINT PK_t_d_Meeting PRIMARY KEY  (f_MeetingNO))");
                        }
                    }
                    catch (Exception exception3)
                    {
                        wgToolsWgDebugWrite(exception3.ToString());
                    }
                    try
                    {
                        if (!wgAppConfigIsAccessDB())
                        {
                            strSql = "CREATE TABLE t_d_MeetingAdr ( ";
                            wgAppConfigRunUpdateSql((strSql + "f_MeetingAdr   [nvarchar] (255)  NOT NULL,") + "f_ReaderID   INT   NOT NULL Default 0," + "f_Notes      [ntext]  NULL  )");
                        }
                        else
                        {
                            strSql = "CREATE TABLE t_d_MeetingAdr ( ";
                            wgAppConfigRunUpdateSql((strSql + "f_MeetingAdr TEXT (255) NOT NULL ,") + "f_ReaderID   INT   NOT NULL Default 0," + "f_Notes MEMO )");
                        }
                    }
                    catch (Exception exception4)
                    {
                        wgToolsWgDebugWrite(exception4.ToString());
                    }
                    try
                    {
                        if (!wgAppConfigIsAccessDB())
                        {
                            strSql = "CREATE TABLE t_d_MeetingConsumer ( ";
                            wgAppConfigRunUpdateSql(((((strSql + " f_Id        [int] IDENTITY (1, 1) NOT NULL  ,") + " f_MeetingNO   [nvarchar] (15)   NOT NULL," + " f_ConsumerID   [int]  NOT NULL Default(0) ,") + " f_MeetingIdentity    INT NOT NULL   DEFAULT -1," + " f_Seat   [nvarchar] (255)  NULL,") + " f_SignWay   [int]  NOT NULL Default(0)," + " f_SignRealTime   DATETIME NULL,") + " f_RecID  INT NOT NULL   DEFAULT 0 ," + " f_Notes      [ntext]  NULL  )");
                        }
                        else
                        {
                            strSql = "CREATE TABLE t_d_MeetingConsumer ( ";
                            wgAppConfigRunUpdateSql(((((strSql + " f_Id AUTOINCREMENT NOT NULL ,") + " f_MeetingNO   TEXT (15)   NOT NULL," + " f_ConsumerID    INT NOT NULL   DEFAULT 0 ,") + " f_MeetingIdentity    INT NOT NULL   DEFAULT -1," + " f_Seat   TEXT (255) NULL,") + " f_SignWay   int  NOT NULL Default 0," + " f_SignRealTime  DATETIME NULL,") + " f_RecID  INT NOT NULL   DEFAULT 0 ," + " f_Notes      MEMO  )");
                        }
                    }
                    catch (Exception exception5)
                    {
                        wgToolsWgDebugWrite(exception5.ToString());
                    }
                }
                catch (Exception exception6)
                {
                    wgToolsWgDebugWrite(exception6.ToString());
                }
                try
                {
                    strSql = "CREATE TABLE  [t_d_Reader4Meal] ( ";
                    wgAppConfigRunUpdateSql(strSql + "[f_ReaderID] INT  NULL, f_CostMorning   Numeric(10,2) NOT   NULL  DEFAULT -1 , f_CostLunch   Numeric(10,2)  NOT NULL  DEFAULT -1 , f_CostEvening   Numeric(10,2) NOT  NULL   DEFAULT -1 , f_CostOther   Numeric(10,2) NOT  NULL  DEFAULT -1  )");
                }
                catch (Exception exception7)
                {
                    wgToolsWgDebugWrite(exception7.ToString());
                }
                try
                {
                    try
                    {
                        strSql = "   CREATE TABLE  [t_b_MealSetup] ";
                        if (!wgAppConfigIsAccessDB())
                        {
                            strSql = strSql + "( [f_ID] INT NOT NULL , [f_Value] INT NULL , [f_BeginHMS] DATETIME NULL ,[f_EndHMS] DATETIME NULL , [f_ParamVal]   Numeric(10,2)   NULL , f_Notes      [ntext]  NULL  ) ";
                        }
                        else
                        {
                            strSql = strSql + "( [f_ID] INT NOT NULL , [f_Value] INT NULL , [f_BeginHMS] DATETIME NULL ,[f_EndHMS] DATETIME NULL , [f_ParamVal]   Numeric(10,2)   NULL ,  f_Notes      MEMO   ) ";
                        }
                        wgAppConfigRunUpdateSql(strSql);
                    }
                    catch (Exception exception8)
                    {
                        wgToolsWgDebugWrite(exception8.ToString());
                    }
                    try
                    {
                        strSql = " INSERT INTO [t_b_MealSetup] ([f_ID], [f_Value], [f_BeginHMS] ,[f_EndHMS] , [f_ParamVal]) ";
                        wgAppConfigRunUpdateSql(strSql + "VALUES (1, 0,NULL, NULL,60)");
                    }
                    catch (Exception exception9)
                    {
                        wgToolsWgDebugWrite(exception9.ToString());
                    }
                    try
                    {
                        strSql = " INSERT INTO [t_b_MealSetup] ([f_ID], [f_Value], [f_BeginHMS] ,[f_EndHMS] , [f_ParamVal]) ";
                        string str2 = strSql;
                        wgAppConfigRunUpdateSql(str2 + "VALUES (2, 1," + wgToolsPrepareStr("04:00", true, " HH:mm") + "," + wgToolsPrepareStr("09:59", true, " HH:mm") + ",0)");
                    }
                    catch (Exception exception10)
                    {
                        wgToolsWgDebugWrite(exception10.ToString());
                    }
                    try
                    {
                        strSql = " INSERT INTO [t_b_MealSetup] ([f_ID], [f_Value], [f_BeginHMS] ,[f_EndHMS] , [f_ParamVal]) ";
                        string str3 = strSql;
                        wgAppConfigRunUpdateSql(str3 + "VALUES (3, 1," + wgToolsPrepareStr("10:00", true, " HH:mm") + "," + wgToolsPrepareStr("15:59", true, " HH:mm") + ",0)");
                    }
                    catch (Exception exception11)
                    {
                        wgToolsWgDebugWrite(exception11.ToString());
                    }
                    try
                    {
                        strSql = " INSERT INTO [t_b_MealSetup] ([f_ID], [f_Value], [f_BeginHMS] ,[f_EndHMS] , [f_ParamVal]) ";
                        string str4 = strSql;
                        wgAppConfigRunUpdateSql(str4 + "VALUES (4, 1," + wgToolsPrepareStr("16:00", true, " HH:mm") + "," + wgToolsPrepareStr("21:59", true, " HH:mm") + ",0)");
                    }
                    catch (Exception exception12)
                    {
                        wgToolsWgDebugWrite(exception12.ToString());
                    }
                    try
                    {
                        strSql = " INSERT INTO [t_b_MealSetup] ([f_ID], [f_Value], [f_BeginHMS] ,[f_EndHMS] , [f_ParamVal]) ";
                        string str5 = strSql;
                        wgAppConfigRunUpdateSql(str5 + "VALUES (5, 1," + wgToolsPrepareStr("22:00", true, " HH:mm") + "," + wgToolsPrepareStr("03:59", true, " HH:mm") + ",0)");
                    }
                    catch (Exception exception13)
                    {
                        wgToolsWgDebugWrite(exception13.ToString());
                    }
                }
                catch (Exception exception14)
                {
                    wgToolsWgDebugWrite(exception14.ToString());
                }
            }
            if (dbversion <= 73.2f)
            {
                try
                {
                    strSql = "Insert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(27,'AbsentTimeout (minute)','','30','')";
                    wgAppConfigRunUpdateSql(strSql);
                }
                catch (Exception exception15)
                {
                    wgToolsWgDebugWrite(exception15.ToString());
                }
                try
                {
                    strSql = "Insert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(28,'AllowTimeout (minute)','','10','')";
                    wgAppConfigRunUpdateSql(strSql);
                }
                catch (Exception exception16)
                {
                    wgToolsWgDebugWrite(exception16.ToString());
                }
                try
                {
                    strSql = "Insert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(29,'LogCreatePatrolReport','','','')";
                    wgAppConfigRunUpdateSql(strSql);
                }
                catch (Exception exception17)
                {
                    wgToolsWgDebugWrite(exception17.ToString());
                }
                try
                {
                    strSql = "CREATE TABLE  [t_b_Reader4Patrol] ( ";
                    wgAppConfigRunUpdateSql(strSql + "[f_ReaderID] INT  NULL )");
                }
                catch (Exception exception18)
                {
                    wgToolsWgDebugWrite(exception18.ToString());
                }
                try
                {
                    strSql = "CREATE TABLE  [t_d_PatrolUsers] ( ";
                    wgAppConfigRunUpdateSql(strSql + "[f_ConsumerID] INT  NULL )");
                }
                catch (Exception exception19)
                {
                    wgToolsWgDebugWrite(exception19.ToString());
                }
                try
                {
                    strSql = " CREATE TABLE t_d_PatrolRouteDetail (";
                    if (wgAppConfigIsAccessDB())
                    {
                        strSql = (((strSql + "f_RecId AUTOINCREMENT NOT NULL ,") + " f_RouteID int   ," + " f_Sn int   ,") + "f_ReaderID int , " + "f_patroltime TEXT(5)  NULL   , ") + "f_NextDay int , " + "  CONSTRAINT PK_t_b_PatrolRouteDetail PRIMARY KEY ( f_RouteID,f_Sn)) ";
                    }
                    else
                    {
                        strSql = (((strSql + " f_RecId [int] IDENTITY (1, 1) NOT NULL  ,") + " f_RouteID int   ," + " f_Sn int   ,") + "f_ReaderID int , " + "f_patroltime  [nvarchar] (5)   NULL   , ") + "f_NextDay int , " + "  CONSTRAINT PK_t_b_PatrolRouteDetail PRIMARY KEY ( f_RouteID,f_Sn)) ";
                    }
                    wgAppConfigRunUpdateSql(strSql);
                }
                catch (Exception exception20)
                {
                    wgToolsWgDebugWrite(exception20.ToString());
                }
                try
                {
                    strSql = " CREATE TABLE t_d_PatrolRouteList (";
                    strSql = strSql + "f_RouteID  INT NOT NULL   ,";
                    if (wgAppConfigIsAccessDB())
                    {
                        strSql = strSql + " f_RouteName  TEXT (50) NOT NULL ," + " f_Description NOTE , ";
                    }
                    else
                    {
                        strSql = strSql + " f_RouteName   [nvarchar] (50) NOT NULL ," + " f_Description [ntext] NULL , ";
                    }
                    wgAppConfigRunUpdateSql(strSql + "  CONSTRAINT PK_t_d_PatrolRouteList PRIMARY KEY ( f_RouteID)) ");
                    strSql = "    CREATE UNIQUE INDEX idxf_RouteName_1 ";
                    wgAppConfigRunUpdateSql(strSql + "   ON t_d_PatrolRouteList (f_RouteName)");
                }
                catch (Exception exception21)
                {
                    wgToolsWgDebugWrite(exception21.ToString());
                }
                try
                {
                    strSql = " CREATE TABLE t_d_PatrolPlanData (";
                    if (wgAppConfigIsAccessDB())
                    {
                        strSql = (strSql + "f_RecID AUTOINCREMENT NOT NULL ") + " , f_ConsumerID INT  NULL  " + " , f_DateYM TEXT(10)  NULL  ";
                    }
                    else
                    {
                        strSql = (strSql + " f_RecID [int] IDENTITY (1, 1) NOT NULL  ") + " , f_ConsumerID INT  NULL  " + " , f_DateYM  [nvarchar](10)  NULL  ";
                    }
                    for (int i = 1; i <= 0x1f; i++)
                    {
                        strSql = strSql + " , f_RouteID_" + i.ToString().PadLeft(2, '0') + "  INT   DEFAULT -1  ";
                    }
                    strSql = strSql + " , f_LogDate  DATETIME   NULL  ";
                    if (wgAppConfigIsAccessDB())
                    {
                        strSql = strSql + " , f_Notes MEMO NULL ";
                    }
                    else
                    {
                        strSql = strSql + " ,  f_Notes      [ntext]  NULL ";
                    }
                    wgAppConfigRunUpdateSql(strSql + " )");
                }
                catch (Exception exception22)
                {
                    wgToolsWgDebugWrite(exception22.ToString());
                }
                try
                {
                    strSql = " CREATE TABLE t_d_PatrolDetailData (";
                    if (wgAppConfigIsAccessDB())
                    {
                        strSql = strSql + "f_RecId AUTOINCREMENT NOT NULL ,";
                    }
                    else
                    {
                        strSql = strSql + " f_RecId [int] IDENTITY (1, 1) NOT NULL  ,";
                    }
                    wgAppConfigRunUpdateSql((((strSql + " f_ConsumerID int   ," + " f_PatrolDate  DATETIME NULL    ,") + " f_RouteID int   ," + " f_ReaderID int   ,") + " f_PlanPatrolTime DATETIME NULL ," + " f_RealPatrolTime DATETIME NULL ,") + " f_EventDesc  int ," + "  CONSTRAINT PK_t_d_PatrolDetailData PRIMARY KEY ( f_RecId)) ");
                }
                catch (Exception exception23)
                {
                    wgToolsWgDebugWrite(exception23.ToString());
                }
                try
                {
                    strSql = " CREATE TABLE t_d_PatrolStatistic (";
                    if (wgAppConfigIsAccessDB())
                    {
                        strSql = strSql + "f_RecId AUTOINCREMENT NOT NULL ,";
                    }
                    else
                    {
                        strSql = strSql + " f_RecId [int] IDENTITY (1, 1) NOT NULL  ,";
                    }
                    wgAppConfigRunUpdateSql((((strSql + " f_ConsumerID int   ," + " f_PatrolDateStart  DATETIME NULL    ,") + " f_PatrolDateEnd  DATETIME NULL    ," + " f_TotalLate int   ,") + " f_TotalEarly int   ," + " f_TotalAbsence int   ,") + " f_TotalNormal int   ," + "  CONSTRAINT PK_t_d_PatrolStatistic PRIMARY KEY ( f_RecId)) ");
                }
                catch (Exception exception24)
                {
                    wgToolsWgDebugWrite(exception24.ToString());
                }
                try
                {
                    if (wgAppConfigGetSystemParamByNO(0x95) == "")
                    {
                        strSql = "Insert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(149,'Activate Meeting','','0','')";
                        wgAppConfigRunUpdateSql(strSql);
                    }
                }
                catch (Exception exception25)
                {
                    wgToolsWgDebugWrite(exception25.ToString());
                }
                try
                {
                    if (wgAppConfigGetSystemParamByNO(150) == "")
                    {
                        strSql = "Insert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(150,'Activate Meal','','0','')";
                        wgAppConfigRunUpdateSql(strSql);
                    }
                }
                catch (Exception exception26)
                {
                    wgToolsWgDebugWrite(exception26.ToString());
                }
                try
                {
                    if (wgAppConfigGetSystemParamByNO(0x97) == "")
                    {
                        strSql = "Insert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(151,'Activate Patrol','','0','')";
                        wgAppConfigRunUpdateSql(strSql);
                    }
                }
                catch (Exception exception27)
                {
                    wgToolsWgDebugWrite(exception27.ToString());
                }
            }
            if (dbversion <= 73.3f)
            {
                try
                {
                    wgAppConfigRunUpdateSql(getOperatorPrivilegeInsertSql(0x30, "mnuPatrolDetailData", "Patrol"));
                }
                catch (Exception exception28)
                {
                    wgToolsWgDebugWrite(exception28.ToString());
                }
                try
                {
                    wgAppConfigRunUpdateSql(getOperatorPrivilegeInsertSql(0x31, "mnuConstMeal", "Meal"));
                }
                catch (Exception exception29)
                {
                    wgToolsWgDebugWrite(exception29.ToString());
                }
                try
                {
                    wgAppConfigRunUpdateSql(getOperatorPrivilegeInsertSql(50, "mnuMeeting", "Meeting"));
                }
                catch (Exception exception30)
                {
                    wgToolsWgDebugWrite(exception30.ToString());
                }
                try
                {
                    wgAppConfigRunUpdateSql(getOperatorPrivilegeInsertSql(0x33, "mnuElevator", "Elevator"));
                }
                catch (Exception exception31)
                {
                    wgToolsWgDebugWrite(exception31.ToString());
                }
            }
            if (dbversion <= 73.5f)
            {
                try
                {
                    strSql = "Insert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(60,'Active Fire_Broadcast','','0','')";
                    wgAppConfigRunUpdateSql(strSql);
                }
                catch (Exception exception33)
                {
                    wgToolsWgDebugWrite(exception33.ToString());
                }
                try
                {
                    strSql = "Insert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(61,'Active Interlock_Broadcast','','0','')";
                    wgAppConfigRunUpdateSql(strSql);
                }
                catch (Exception exception34)
                {
                    wgToolsWgDebugWrite(exception34.ToString());
                }
                try
                {
                    strSql = "Insert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(62,'Active Antiback_Broadcast','','0','')";
                    wgAppConfigRunUpdateSql(strSql);
                }
                catch (Exception exception35)
                {
                    wgToolsWgDebugWrite(exception35.ToString());
                }
            }
        }

        private bool upgradeFromAccess(string dbfile)
        {
            bool flag = false;
            Cursor.Current = Cursors.WaitCursor;
            string str = "";
            string str2 = "";
            try
            {
                string str4;
                string connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source= {0};User ID=admin;Password=;JET OLEDB:Database Password=passaccess", dbfile);
                if (this.optWindowsAuthentication.Checked)
                {
                    str4 = string.Format("data source={0};initial catalog={1};integrated security=SSPI;persist security info=True", this.txtServer.Text, this.txtDBName.Text);
                }
                else
                {
                    str4 = string.Format("data source={0};initial catalog={1};Password={2};Persist Security Info=True;User ID={3}", new object[] { this.txtServer.Text, this.txtDBName.Text, this.txtPasswd.Text, this.txtUsername.Text });
                }
                SqlConnection connection = new SqlConnection(str4);
                OleDbConnection connection2 = new OleDbConnection(connectionString);
                connection2.Open();
                object[] restrictions = new object[4];
                restrictions[3] = "TABLE";
                DataTable oleDbSchemaTable = connection2.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, restrictions);
                connection2.Close();
                SqlCommand command = new SqlCommand("", connection);
                OleDbCommand command2 = new OleDbCommand("", connection2);
                OleDbDataAdapter adapter = new OleDbDataAdapter();
                DataTable dataTable = new DataTable();
                float result = 0f;
                str = "SELECT f_Value FROM t_a_SystemParam WHERE f_No=9 ";
                command2.CommandText = str;
                connection2.Open();
                float.TryParse(command2.ExecuteScalar().ToString(), out result);
                connection2.Close();
                if (result > dbVersionNewest)
                {
                    XMessageBoxAA.Show(CommonStr.strFailed + "\r\n\r\n" + CommonStr.strAccessHigh);
                    return false;
                }
                connection.Open();
                connection2.Open();
                for (int i = 0; i < oleDbSchemaTable.Rows.Count; i++)
                {
                    if (wgToolsAA.SetObjToStr(oleDbSchemaTable.Rows[i]["TABLE_TYPE"]).ToUpper() == "TABLE")
                    {
                        string str5 = wgToolsAA.SetObjToStr(oleDbSchemaTable.Rows[i]["table_name"]);
                        this.lblRunInfo.Text = string.Format("{0:d}_{1:d}: {2}", oleDbSchemaTable.Rows.Count, i + 1, str5);
                        this.Refresh();
                        str = string.Format("TRUNCATE TABLE [{0}] ", str5);
                        command.CommandText = str;
                        command.ExecuteNonQuery();
                        str2 = string.Format("SELECT * FROM [{0}]", str5);
                        command2.CommandText = str2;
                        command2.CommandTimeout = 300;
                        dataTable = new DataTable();
                        dataTable.TableName = str5;
                        adapter.SelectCommand = command2;
                        adapter.Fill(dataTable);
                        command2.CommandTimeout = 30;
                        bool flag2 = false;
                        try
                        {
                            str = string.Format("SET IDENTITY_INSERT [{0}] ON", str5);
                            command.CommandText = str;
                            command.ExecuteNonQuery();
                            flag2 = true;
                        }
                        catch
                        {
                        }
                        string str6 = "";
                        str6 = string.Format("INSERT INTO [{0}] ( ", str5);
                        StringBuilder builder = new StringBuilder();
                        bool flag3 = true;
                        builder.AppendFormat("INSERT INTO [{0}](", dataTable.TableName);
                        foreach (DataColumn column in dataTable.Columns)
                        {
                            if (!flag3)
                            {
                                builder.Append(",");
                            }
                            flag3 = false;
                            builder.Append(column.ColumnName);
                        }
                        builder.Append(") VALUES(");
                        str6 = builder.ToString();
                        builder = new StringBuilder();
                        int num3 = 0;
                        int num4 = 0;
                        command.Parameters.Clear();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            str = string.Format("{0} {1}", str6, this.GenerateInsertStatement(row, num3, command));
                            builder.Append(str);
                            builder.Append(";");
                            num3++;
                            num4++;
                            if (num3 == 1000)
                            {
                                command.Parameters.Clear();
                                command.CommandText = builder.ToString();
                                command.ExecuteNonQuery();
                                builder.Remove(0, builder.Length);
                                num3 = 0;
                                this.lblRunInfo.Text = string.Format("{0:d}_{1:d}: {2}\r\n{3}_{4}", new object[] { oleDbSchemaTable.Rows.Count, i + 1, str5, dataTable.Rows.Count, num4 });
                                this.Refresh();
                            }
                        }
                        if (builder.Length > 0)
                        {
                            command.CommandText = builder.ToString();
                            command.ExecuteNonQuery();
                        }
                        if (flag2)
                        {
                            str = string.Format("SET IDENTITY_INSERT [{0}] OFF", str5);
                            command.CommandText = str;
                            command.ExecuteNonQuery();
                        }
                    }
                }
                connection2.Close();
                connection.Close();
                flag = true;
            }
            catch (Exception exception)
            {
                XMessageBoxAA.Show(exception.ToString());
            }
            Cursor.Current = Cursors.Default;
            return flag;
        }

        private static string wgAppConfigGetSystemParamByNO(int ParaNo)
        {
            return wgToolsAA.getSystemParamByNO(ParaNo);
        }

        private static bool wgAppConfigIsAccessDB()
        {
            return false;
        }

        private static int wgAppConfigRunUpdateSql(string strSql)
        {
            return wgToolsAA.runUpdateSql(strSql);
        }

        private static string wgToolsPrepareStr(object obj)
        {
            return wgToolsAA.PrepareStr(obj);
        }

        private static string wgToolsPrepareStr(object obj, bool bDate, string dateFormat)
        {
            return wgToolsAA.PrepareStr(obj, bDate, dateFormat);
        }

        private static void wgToolsWgDebugWrite(string info)
        {
            wgToolsAA.wgDebugWrite(info);
        }

        public static string accessDbName
        {
            get
            {
                return "AccessDB";
            }
        }
    }
}
