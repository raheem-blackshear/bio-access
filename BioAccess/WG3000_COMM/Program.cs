namespace WG3000_COMM
{
    using System;
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Threading;
    using System.Windows.Forms;
    using WG3000_COMM.Basic;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    internal static class Program
    {
        private static bool bSqlExress = false;
        private static float dbVersionNewest = 75f;
        private const string defaultDBFileName = "SqlDB.sql";
        public static int expcount = 0;
        public static string expStrDayHour = "";
        private static string g_cnStrAcc = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source= {0}.mdb;User ID=admin;Password=;JET OLEDB:Database Password=passaccess", Application.StartupPath + @"\" + wgAppConfig.accessDbName);
        private static Thread startSlowThread;
        private const string wgDatabaseDefaultNameOfAdroitor = "AccessData";

        private static bool ConnectTest2010()
        {
            bool flag = false;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                using (SqlConnection connection = new SqlConnection(getConSql("master") + ";Connection Timeout=5"))
                {
                    try
                    {
                        string cmdText = " SELECT name FROM sysdatabases ";
                        bool flag2 = false;
                        using (SqlCommand command = new SqlCommand(cmdText, connection))
                        {
                            command.CommandTimeout = 5;
                            connection.Open();
                            SqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                flag2 = true;
                            }
                            reader.Close();
                        }
                        if (!flag2)
                        {
                            return false;
                        }
                        flag = true;
                    }
                    catch (Exception)
                    {
                    }
                    return flag;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
            return flag;
        }

        private static bool createDatabase2010(string databaseName)
        {
            bool flag = false;
            string newValue = databaseName;
            if (databaseName != "")
            {
                bool flag2 = false;
                try
                {
                    string connectionString = getConSql("master") + ";Connection Timeout=5";
                    SqlCommand command = null;
                    SqlConnection connection = new SqlConnection(connectionString);
                    try
                    {
                        connection.Open();
                        string cmdText = " SELECT name FROM sysdatabases ";
                        object obj2 = null;
                        using (command = new SqlCommand(cmdText, connection))
                        {
                            command.CommandTimeout = 5;
                            obj2 = command.ExecuteScalar();
                        }
                        if (obj2 == null)
                        {
                            return flag;
                        }
                        cmdText = " SELECT  convert( int, LEFT(convert(nvarchar,SERVERPROPERTY('ProductVersion')),CHARINDEX('.',convert(nvarchar,SERVERPROPERTY('ProductVersion')))-1)) ";
                        object obj3 = null;
                        using (command = new SqlCommand(cmdText, connection))
                        {
                            command.CommandTimeout = 5;
                            obj3 = command.ExecuteScalar();
                        }
                        if (obj3 == null)
                        {
                            return flag;
                        }
                        string path = "";
                        string str6 = "";
                        str6 = "SqlDB.sql";
                        path = Application.StartupPath + @"\" + str6;
                        using (command = new SqlCommand(("IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'" + newValue + "')") + "\r\n DROP DATABASE [" + newValue + "]", connection))
                        {
                            command.CommandTimeout = 300;
                            command.ExecuteNonQuery();
                        }
                        using (command = new SqlCommand(" CREATE DATABASE [" + newValue + "] ", connection))
                        {
                            command.CommandTimeout = 300;
                            command.ExecuteNonQuery();
                        }
                        using (command = new SqlCommand(wgTools.ReadTextFile(path).Replace("ADCT3000", newValue).Replace("\r\nGO", "\r\n").Replace(" COLLATE Chinese_PRC_CI_AS", " "), connection))
                        {
                            command.CommandTimeout = 300;
                            command.ExecuteNonQuery();
                        }
                        bool flag3 = false;
                        try
                        {
                            cmdText = " CREATE PARTITION FUNCTION [RangePrivilegePF1](int) AS RANGE LEFT FOR VALUES (N'1',N'2',N'3',N'4',N'5',N'6',N'7',N'8',N'9',N'10',N'11',N'12',N'13',N'14',N'15',N'16',N'17',N'18',N'19',N'20',N'21',N'22',N'23',N'24',N'25',N'26',N'27',N'28',N'29',N'30',N'31',N'32',N'33',N'34',N'35',N'36',N'37',N'38',N'39',N'40',N'41',N'42',N'43',N'44',N'45',N'46',N'47',N'48',N'49',N'50',N'51',N'52',N'53',N'54',N'55',N'56',N'57',N'58',N'59',N'60',N'61',N'62',N'63',N'64',N'65',N'66',N'67',N'68',N'69',N'70',N'71',N'72',N'73',N'74',N'75',N'76',N'77',N'78',N'79',N'80',N'81',N'82',N'83',N'84',N'85',N'86',N'87',N'88',N'89',N'90',N'91',N'92',N'93',N'94',N'95',N'96',N'97',N'98',N'99',N'100',N'101',N'102',N'103',N'104',N'105',N'106',N'107',N'108',N'109',N'110',N'111',N'112',N'113',N'114',N'115',N'116',N'117',N'118',N'119',N'120',N'121',N'122',N'123',N'124',N'125',N'126',N'127',N'128',N'129',N'130',N'131',N'132',N'133',N'134',N'135',N'136',N'137',N'138',N'139',N'140',N'141',N'142',N'143',N'144',N'145',N'146',N'147',N'148',N'149',N'150',N'151',N'152',N'153',N'154',N'155',N'156',N'157',N'158',N'159',N'160',N'161',N'162',N'163',N'164',N'165',N'166',N'167',N'168',N'169',N'170',N'171',N'172',N'173',N'174',N'175',N'176',N'177',N'178',N'179',N'180',N'181',N'182',N'183',N'184',N'185',N'186',N'187',N'188',N'189',N'190',N'191',N'192',N'193',N'194',N'195',N'196',N'197',N'198',N'199',N'200',N'201',N'202',N'203',N'204',N'205',N'206',N'207',N'208',N'209',N'210',N'211',N'212',N'213',N'214',N'215',N'216',N'217',N'218',N'219',N'220',N'221',N'222',N'223',N'224',N'225',N'226',N'227',N'228',N'229',N'230',N'231',N'232',N'233',N'234',N'235',N'236',N'237',N'238',N'239',N'240',N'241',N'242',N'243',N'244',N'245',N'246',N'247',N'248',N'249',N'250',N'251',N'252',N'253',N'254',N'255',N'256',N'257',N'258',N'259',N'260',N'261',N'262',N'263',N'264',N'265',N'266',N'267',N'268',N'269',N'270',N'271',N'272',N'273',N'274',N'275',N'276',N'277',N'278',N'279',N'280',N'281',N'282',N'283',N'284',N'285',N'286',N'287',N'288',N'289',N'290',N'291',N'292',N'293',N'294',N'295',N'296',N'297',N'298',N'299',N'300',N'301',N'302',N'303',N'304',N'305',N'306',N'307',N'308',N'309',N'310',N'311',N'312',N'313',N'314',N'315',N'316',N'317',N'318',N'319',N'320',N'321',N'322',N'323',N'324',N'325',N'326',N'327',N'328',N'329',N'330',N'331',N'332',N'333',N'334',N'335',N'336',N'337',N'338',N'339',N'340',N'341',N'342',N'343',N'344',N'345',N'346',N'347',N'348',N'349',N'350',N'351',N'352',N'353',N'354',N'355',N'356',N'357',N'358',N'359',N'360',N'361',N'362',N'363',N'364',N'365',N'366',N'367',N'368',N'369',N'370',N'371',N'372',N'373',N'374',N'375',N'376',N'377',N'378',N'379',N'380',N'381',N'382',N'383',N'384',N'385',N'386',N'387',N'388',N'389',N'390',N'391',N'392',N'393',N'394',N'395',N'396',N'397',N'398',N'399',N'400',N'401',N'402',N'403',N'404',N'405',N'406',N'407',N'408',N'409',N'410',N'411',N'412',N'413',N'414',N'415',N'416',N'417',N'418',N'419',N'420',N'421',N'422',N'423',N'424',N'425',N'426',N'427',N'428',N'429',N'430',N'431',N'432',N'433',N'434',N'435',N'436',N'437',N'438',N'439',N'440',N'441',N'442',N'443',N'444',N'445',N'446',N'447',N'448',N'449',N'450',N'451',N'452',N'453',N'454',N'455',N'456',N'457',N'458',N'459',N'460',N'461',N'462',N'463',N'464',N'465',N'466',N'467',N'468',N'469',N'470',N'471',N'472',N'473',N'474',N'475',N'476',N'477',N'478',N'479',N'480',N'481',N'482',N'483',N'484',N'485',N'486',N'487',N'488',N'489',N'490',N'491',N'492',N'493',N'494',N'495',N'496',N'497',N'498',N'499',N'500',N'501',N'502',N'503',N'504',N'505',N'506',N'507',N'508',N'509',N'510',N'511',N'512',N'513',N'514',N'515',N'516',N'517',N'518',N'519',N'520',N'521',N'522',N'523',N'524',N'525',N'526',N'527',N'528',N'529',N'530',N'531',N'532',N'533',N'534',N'535',N'536',N'537',N'538',N'539',N'540',N'541',N'542',N'543',N'544',N'545',N'546',N'547',N'548',N'549',N'550',N'551',N'552',N'553',N'554',N'555',N'556',N'557',N'558',N'559',N'560',N'561',N'562',N'563',N'564',N'565',N'566',N'567',N'568',N'569',N'570',N'571',N'572',N'573',N'574',N'575',N'576',N'577',N'578',N'579',N'580',N'581',N'582',N'583',N'584',N'585',N'586',N'587',N'588',N'589',N'590',N'591',N'592',N'593',N'594',N'595',N'596',N'597',N'598',N'599',N'600',N'601',N'602',N'603',N'604',N'605',N'606',N'607',N'608',N'609',N'610',N'611',N'612',N'613',N'614',N'615',N'616',N'617',N'618',N'619',N'620',N'621',N'622',N'623',N'624',N'625',N'626',N'627',N'628',N'629',N'630',N'631',N'632',N'633',N'634',N'635',N'636',N'637',N'638',N'639',N'640',N'641',N'642',N'643',N'644',N'645',N'646',N'647',N'648',N'649',N'650',N'651',N'652',N'653',N'654',N'655',N'656',N'657',N'658',N'659',N'660',N'661',N'662',N'663',N'664',N'665',N'666',N'667',N'668',N'669',N'670',N'671',N'672',N'673',N'674',N'675',N'676',N'677',N'678',N'679',N'680',N'681',N'682',N'683',N'684',N'685',N'686',N'687',N'688',N'689',N'690',N'691',N'692',N'693',N'694',N'695',N'696',N'697',N'698',N'699',N'700',N'701',N'702',N'703',N'704',N'705',N'706',N'707',N'708',N'709',N'710',N'711',N'712',N'713',N'714',N'715',N'716',N'717',N'718',N'719',N'720',N'721',N'722',N'723',N'724',N'725',N'726',N'727',N'728',N'729',N'730',N'731',N'732',N'733',N'734',N'735',N'736',N'737',N'738',N'739',N'740',N'741',N'742',N'743',N'744',N'745',N'746',N'747',N'748',N'749',N'750',N'751',N'752',N'753',N'754',N'755',N'756',N'757',N'758',N'759',N'760',N'761',N'762',N'763',N'764',N'765',N'766',N'767',N'768',N'769',N'770',N'771',N'772',N'773',N'774',N'775',N'776',N'777',N'778',N'779',N'780',N'781',N'782',N'783',N'784',N'785',N'786',N'787',N'788',N'789',N'790',N'791',N'792',N'793',N'794',N'795',N'796',N'797',N'798',N'799',N'800',N'801',N'802',N'803',N'804',N'805',N'806',N'807',N'808',N'809',N'810',N'811',N'812',N'813',N'814',N'815',N'816',N'817',N'818',N'819',N'820',N'821',N'822',N'823',N'824',N'825',N'826',N'827',N'828',N'829',N'830',N'831',N'832',N'833',N'834',N'835',N'836',N'837',N'838',N'839',N'840',N'841',N'842',N'843',N'844',N'845',N'846',N'847',N'848',N'849',N'850',N'851',N'852',N'853',N'854',N'855',N'856',N'857',N'858',N'859',N'860',N'861',N'862',N'863',N'864',N'865',N'866',N'867',N'868',N'869',N'870',N'871',N'872',N'873',N'874',N'875',N'876',N'877',N'878',N'879',N'880',N'881',N'882',N'883',N'884',N'885',N'886',N'887',N'888',N'889',N'890',N'891',N'892',N'893',N'894',N'895',N'896',N'897',N'898',N'899',N'900',N'901',N'902',N'903',N'904',N'905',N'906',N'907',N'908',N'909',N'910',N'911',N'912',N'913',N'914',N'915',N'916',N'917',N'918',N'919',N'920',N'921',N'922',N'923',N'924',N'925',N'926',N'927',N'928',N'929',N'930',N'931',N'932',N'933',N'934',N'935',N'936',N'937',N'938',N'939',N'940',N'941',N'942',N'943',N'944',N'945',N'946',N'947',N'948',N'949',N'950',N'951',N'952',N'953',N'954',N'955',N'956',N'957',N'958',N'959',N'960',N'961',N'962',N'963',N'964',N'965',N'966',N'967',N'968',N'969',N'970',N'971',N'972',N'973',N'974',N'975',N'976',N'977',N'978',N'979',N'980',N'981',N'982',N'983',N'984',N'985',N'986',N'987',N'988',N'989',N'990',N'991',N'992',N'993',N'994',N'995',N'996',N'997',N'998',N'999') \r\n     \r\nCREATE PARTITION SCHEME [RangePrivilegePS1] AS PARTITION [RangePrivilegePF1] TO ([PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY],[PRIMARY])\r\n\r\n\r\nCREATE TABLE [dbo].[t_d_Privilege](\r\n\t[f_PrivilegeRecID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,\r\n\t[f_DoorID] [int] NOT NULL,\r\n\t[f_ControlSegID] [int] NOT NULL,\r\n\t[f_ConsumerID] [int] NOT NULL,\r\n\t[f_ControllerID] [int] NOT NULL,\r\n\t[f_DoorNO] [tinyint] NOT NULL,\r\n    CONSTRAINT [PK_t_d_Privilege] PRIMARY KEY CLUSTERED \r\n    (\r\n\t[f_ControllerID] ASC,\r\n\t[f_PrivilegeRecID] ASC\r\n    )  ON [RangePrivilegePS1](f_ControllerID) \r\n)ON [PRIMARY] \r\n\r\n";
                            using (command = new SqlCommand(cmdText, connection))
                            {
                                command.CommandTimeout = 300;
                                command.ExecuteNonQuery();
                                flag3 = true;
                            }
                        }
                        catch (Exception)
                        {
                        }
                        if (!flag3)
                        {
                            cmdText = "CREATE TABLE [dbo].[t_d_Privilege](\r\n\t[f_PrivilegeRecID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,\r\n\t[f_DoorID] [int] NOT NULL,\r\n\t[f_ControlSegID] [int] NOT NULL,\r\n\t[f_ConsumerID] [int] NOT NULL,\r\n\t[f_ControllerID] [int] NOT NULL,\r\n\t[f_DoorNO] [tinyint] NOT NULL,\r\n  CONSTRAINT [PK_t_d_Privilege] PRIMARY KEY CLUSTERED \r\n  (\r\n\t[f_ControllerID] ASC,\r\n\t[f_PrivilegeRecID] ASC\r\n  )  ON [PRIMARY] \r\n)ON [PRIMARY] \r\n\r\n";
                            using (command = new SqlCommand(cmdText, connection))
                            {
                                command.CommandTimeout = 300;
                                command.ExecuteNonQuery();
                            }
                        }
                        if (flag3)
                        {
                            cmdText = "\r\nCREATE NONCLUSTERED INDEX [_dta_index_t_d_Privilege_12_1810105489__K4_1_2_3_5] ON [dbo].[t_d_Privilege] \r\n(\r\n\t[f_ConsumerID] ASC\r\n)\r\nINCLUDE ( [f_PrivilegeRecID],\r\n[f_DoorID],\r\n[f_ControlSegID],\r\n[f_ControllerID]) WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]\r\n";
                        }
                        else
                        {
                            cmdText = "\r\nCREATE NONCLUSTERED INDEX [_dta_index_t_d_Privilege_12_1810105489__K4_1_2_3_5] ON [dbo].[t_d_Privilege] \r\n(\r\n\t[f_ConsumerID] ASC\r\n) ON [PRIMARY]\r\n";
                        }
                        using (command = new SqlCommand(cmdText, connection))
                        {
                            command.CommandTimeout = 300;
                            command.ExecuteNonQuery();
                        }
                        if (flag3)
                        {
                            cmdText = "Insert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(53,'Created Partition','1','1','2010-8-3 16:07:41')";
                        }
                        else
                        {
                            cmdText = "Insert Into t_a_SystemParam(f_No,f_Name,f_EName,f_Value,f_Notes) Values(53,'Created Partition','0','0','2010-8-3 16:07:48')";
                        }
                        using (command = new SqlCommand(cmdText, connection))
                        {
                            command.CommandTimeout = 300;
                            command.ExecuteNonQuery();
                            flag2 = true;
                        }
                    }
                    catch (Exception)
                    {
                    }
                    finally
                    {
                        connection.Dispose();
                    }
                    wgAppConfig.UpdateKeyVal("dbConnection", getConSql(databaseName));
                    wgAppConfig.wgLogWithoutDB("Create DB " + databaseName, EventLogEntryType.Information, null);
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                }
                finally
                {
                    flag = flag2;
                }
            }
            return flag;
        }

        public static int dbConnectionCheck()
        {
            string str2;
            SqlCommand command;
            int num = -1;
            string str = descDbConnection(wgAppConfig.GetKeyVal("dbConnection"));
            bool flag = true;
            if (!string.IsNullOrEmpty(str))
            {
                wgAppConfig.dbConString = str;
                flag = false;
            }
            if (string.IsNullOrEmpty(str))
            {
                wgAppConfig.IsAccessDB = true;
                wgAppConfig.dbConString = g_cnStrAcc;
            }
            else if ((str.ToUpper().IndexOf("Data Source".ToUpper()) >= 0) && (str.ToUpper().IndexOf(".OLEDB".ToUpper()) < 0))
            {
                wgAppConfig.IsAccessDB = false;
            }
            else
            {
                wgAppConfig.IsAccessDB = true;
            }
            if (wgAppConfig.IsAccessDB)
            {
                return dbConnectionCheck_Acc();
            }
            SqlConnection connection = new SqlConnection(wgAppConfig.dbConString + ";Connection Timeout=5");
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                str2 = "SELECT * FROM t_a_SystemParam WHERE f_NO = 12";
                using (command = new SqlCommand(str2, connection))
                {
                    command.CommandTimeout = 5;
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        num = 1;
                    }
                    reader.Close();
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            finally
            {
                connection.Dispose();
            }
            if ((num <= 0) && flag)
            {
                if (ConnectTest2010())
                {
                    if (createDatabase2010(wgAppConfig.dbName))
                    {
                        str = descDbConnection(wgAppConfig.GetKeyVal("dbConnection"));
                        if (!string.IsNullOrEmpty(str))
                        {
                            wgAppConfig.dbConString = str;
                            num = 1;
                        }
                    }
                }
                else
                {
                    bSqlExress = true;
                    if (ConnectTest2010())
                    {
                        SqlConnection connection2 = new SqlConnection(wgAppConfig.dbConString.Replace(";Data Source=(local)", @";Data Source=(local)\sqlexpress") + ";Connection Timeout=5");
                        try
                        {
                            if (connection2.State != ConnectionState.Open)
                            {
                                connection2.Open();
                            }
                            str2 = "SELECT * FROM t_a_SystemParam WHERE f_NO = 12";
                            using (command = new SqlCommand(str2, connection2))
                            {
                                command.CommandTimeout = 5;
                                SqlDataReader reader2 = command.ExecuteReader();
                                if (reader2.Read())
                                {
                                    num = 1;
                                }
                                reader2.Close();
                            }
                        }
                        catch (Exception exception2)
                        {
                            wgTools.WgDebugWrite(exception2.ToString(), new object[0]);
                        }
                        finally
                        {
                            connection2.Dispose();
                        }
                        if (num > 0)
                        {
                            wgAppConfig.UpdateKeyVal("dbConnection", getConSql(wgAppConfig.dbName));
                            str = descDbConnection(wgAppConfig.GetKeyVal("dbConnection"));
                            if (!string.IsNullOrEmpty(str))
                            {
                                wgAppConfig.dbConString = str;
                                num = 1;
                            }
                            else
                            {
                                num = 0;
                            }
                        }
                        else if (createDatabase2010(wgAppConfig.dbName))
                        {
                            str = descDbConnection(wgAppConfig.GetKeyVal("dbConnection"));
                            if (!string.IsNullOrEmpty(str))
                            {
                                wgAppConfig.dbConString = str;
                                num = 1;
                            }
                        }
                    }
                }
            }
            if (num > 0)
            {
                return 1;
            }
            return 0;
        }

        public static int dbConnectionCheck_Acc()
        {
            int num = -1;
            if (!wgAppConfig.IsAccessDB)
            {
                return num;
            }
            OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString);
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                string cmdText = "SELECT * FROM t_a_SystemParam WHERE f_NO = 12";
                using (OleDbCommand command = new OleDbCommand(cmdText, connection))
                {
                    command.CommandTimeout = 5;
                    OleDbDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        num = 1;
                    }
                    reader.Close();
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
            finally
            {
                connection.Dispose();
            }
            if (num <= 0)
            {
                try
                {
                    FileInfo info = new FileInfo(Application.StartupPath + @"\" + wgAppConfig.accessDbName + ".mdb");
                    if (info.Exists)
                    {
                        try
                        {
                            if ((info.Attributes & FileAttributes.ReadOnly) != 0)
                            {
                                info.Attributes &= (FileAttributes)0xfffffe;
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                    else
                    {
                        FileInfo info2 = new FileInfo(string.Format(Application.StartupPath + @"\PHOTO\{0}.mdbAA", wgAppConfig.accessDbName));
                        if (!info2.Exists)
                        {
                            FileInfo fi = new FileInfo(string.Format(Application.StartupPath + @"\{0}.mdb.gz", wgAppConfig.accessDbName));
                            if (!fi.Exists)
                            {
                                using (BinaryWriter writer = new BinaryWriter(File.Open(fi.FullName, FileMode.Create)))
                                {
                                    writer.Write(Resources.AccessDB_mdbA);
                                }
                            }
                            wgTools.Decompress(fi);
                            fi.Delete();
                            return 1;
                        }
                        info2.CopyTo(Application.StartupPath + @"\" + wgAppConfig.accessDbName + ".mdb", true);
                        info2 = new FileInfo(Application.StartupPath + @"\" + wgAppConfig.accessDbName + ".mdb");
                        info2.Attributes = FileAttributes.Archive;
                        return 1;
                    }
                }
                catch (Exception exception2)
                {
                    wgTools.WgDebugWrite(exception2.ToString(), new object[] { EventLogEntryType.Error });
                }
            }
            if (num > 0)
            {
                return 1;
            }
            return 0;
        }

        public static string descDbConnection(string strDbConnection)
        {
            string str = strDbConnection;
            try
            {
                if ((!string.IsNullOrEmpty(strDbConnection) && (strDbConnection.Length > 3)) && (strDbConnection.Substring(0, 3) == "ENC"))
                {
                    str = WGPacket.Dpt(strDbConnection.Substring(3));
                }
            }
            catch
            {
            }
            return str;
        }

        private static string getConSql(string dbName)
        {
            string str = string.Format("data source={0};initial catalog={1};integrated security=SSPI;persist security info=True", "(local)", dbName);
            if (bSqlExress)
            {
                str = string.Format("data source={0};initial catalog={1};integrated security=SSPI;persist security info=True", @"(local)\sqlexpress", dbName);
            }
            return str;
        }

        public static void getNewSoftware()
        {
            try
            {
                comMjSpecialUpdate.updateMjSpecialSoftware();
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        private static string getOperatorPrivilegeInsertSql(int functionId, string functionName, string displayName)
        {
            return string.Format("Insert Into t_s_OperatorPrivilege(f_OperatorID,f_FunctionID,f_FunctionName,f_FunctionDisplayName,f_ReadOnly,f_FullControl)  SELECT t_s_OperatorPrivilege.f_OperatorID,{0} as f_FunctionID,{1} as f_FunctionName ,{2} as f_FunctionDisplayName,0 as f_ReadOnly,1 as f_FullControl FROM  t_s_OperatorPrivilege WHERE t_s_OperatorPrivilege.f_functionID = 1  AND t_s_OperatorPrivilege.f_OperatorID NOT IN (SELECT t_s_OperatorPrivilege.f_OperatorID  FROM  t_s_OperatorPrivilege  WHERE t_s_OperatorPrivilege.f_functionID = {0} )", functionId, wgToolsPrepareStr(functionName), wgToolsPrepareStr(displayName));
        }

        public static void GlobalExceptionHandler(object sender, ThreadExceptionEventArgs e)
        {
            wgTools.WgDebugWrite(e.Exception.ToString(), new object[0]);
            try
            {
                wgAppConfig.wgLog(e.Exception.ToString(), EventLogEntryType.Error, null);
                dfrmShowError error = new dfrmShowError();
                try
                {
                    error.StartPosition = FormStartPosition.Manual;
                    error.Location = new Point(0, 0);
                    error.errInfo = e.Exception.ToString();
                    error.ShowDialog();
                }
                catch (Exception)
                {
                }
                error.Dispose();
                if (!(expStrDayHour == DateTime.Now.ToString("yyyy-MM-dd HH")))
                {
                    expStrDayHour = DateTime.Now.ToString("yyyy-MM-dd HH");
                    expcount = 1;
                }
                if (expcount >= 3)
                {
                    Thread.CurrentThread.Abort();
                }
                else
                {
                    expcount++;
                }
            }
            catch
            {
            }
        }

        public static void localize()
        {
            string cultureInfo = "";
            cultureInfo = wgAppConfig.GetKeyVal("Language");
            if ((cultureInfo != "") && !wgAppConfig.IsChineseSet(cultureInfo))
            {
                wgTools.DisplayFormat_DateYMDHMSWeek.Replace("dddd", "ddd");
                wgTools.DisplayFormat_DateYMDWeek.Replace("dddd", "ddd");
            }
            wgAppConfig.CultureInfoStr = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
            if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("DisplayFormat_DateYMD")) && wgTools.IsValidDateTimeFormat(wgAppConfig.GetKeyVal("DisplayFormat_DateYMD")))
            {
                wgTools.DisplayFormat_DateYMD = wgAppConfig.GetKeyVal("DisplayFormat_DateYMD");
            }
            if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("DisplayFormat_DateYMDWeek")) && wgTools.IsValidDateTimeFormat(wgAppConfig.GetKeyVal("DisplayFormat_DateYMDWeek")))
            {
                wgTools.DisplayFormat_DateYMDWeek = wgAppConfig.GetKeyVal("DisplayFormat_DateYMDWeek");
            }
            if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("DisplayFormat_DateYMDHMS")) && wgTools.IsValidDateTimeFormat(wgAppConfig.GetKeyVal("DisplayFormat_DateYMDHMS")))
            {
                wgTools.DisplayFormat_DateYMDHMS = wgAppConfig.GetKeyVal("DisplayFormat_DateYMDHMS");
            }
            if (!string.IsNullOrEmpty(wgAppConfig.GetKeyVal("DisplayFormat_DateYMDHMSWeek")) && wgTools.IsValidDateTimeFormat(wgAppConfig.GetKeyVal("DisplayFormat_DateYMDHMSWeek")))
            {
                wgTools.DisplayFormat_DateYMDHMSWeek = wgAppConfig.GetKeyVal("DisplayFormat_DateYMDHMSWeek");
            }
        }

        [STAThread]
        private static void Main(string[] cmdArgs)
        {
            if (cmdArgs.Length != 1 || cmdArgs[0].ToUpper() != "-R")
            {
                // Make sure only one instance running
            
                // we need to get the current process name
                string strProcessName = Process.GetCurrentProcess().ProcessName;
            
                // check if this process name is existing in the current running processes
                Process[] processes = Process.GetProcessesByName(strProcessName);

                // if its existing then exit
            
                if (processes.Length > 1)
                {
                    XMessageBox.Show(CommonStr.strApplicationAlreadyRunning, wgTools.MSGTITLE, MessageBoxButtons.OK);
                    return;
                }
            }

            wgTools.gPTC = "/jHWIsa9BCY8k9kJc+0XjQ==";
            wgAppConfig.ProductTypeOfApp = "AccessControl";
            Directory.SetCurrentDirectory(Application.StartupPath);
            wgAppConfig.gRestart = false;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += new ThreadExceptionEventHandler(Program.GlobalExceptionHandler);
            localize();
            if (!string.IsNullOrEmpty(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("CommTimeoutMsMin"))))
            {
                long.TryParse(wgTools.SetObjToStr(wgAppConfig.GetKeyVal("CommTimeoutMsMin")), out wgUdpComm.CommTimeoutMsMin);
            }
            if (cmdArgs.Length == 1)
            {
                if (cmdArgs[0].ToUpper() == "-P")
                {
                    wgAppConfig.dbConString = "";
                    Application.Run(new frmProductFormat());
                    return;
                }
                if ((cmdArgs[0].ToUpper() == "-S") || (cmdArgs[0].ToUpper() == "-WEB"))
                {
                    wgAppConfig.dbConString = "";
                    dfrmNetControllerConfig mainForm = new dfrmNetControllerConfig();
                    try
                    {
                        mainForm.btnAddToSystem.Visible = false;
                        icOperator.login("wiegand", "168668");
                        if (cmdArgs[0].ToUpper() == "-WEB")
                        {
                            mainForm.btnIPAndWebConfigure.Visible = true;
                        }
                        Application.Run(mainForm);
                    }
                    catch (Exception)
                    {
                    }
                    mainForm.Dispose();
                    return;
                }
                if ((cmdArgs[0].Length > 2) && (cmdArgs[0].ToUpper().Substring(0, 3) == "-CS"))
                {
                    wgAppConfig.dbConString = "";
                    icOperator.login("wiegand", "168668");
                    frmTestController controller = new frmTestController();
                    try
                    {
                        controller.onlyProduce();
                        Application.Run(controller);
                    }
                    catch (Exception)
                    {
                    }
                    controller.Dispose();
                    return;
                }
            }
            dfrmWait wait = new dfrmWait();
            wait.Show();
            wait.Refresh();
            if (dbConnectionCheck() <= 0)
            {
                wait.Close();
                if (wgAppConfig.IsAccessDB)
                {
                    XMessageBox.Show(CommonStr.strAccessDatabaseNotConnected, wgTools.MSGTITLE, MessageBoxButtons.OK);
                }
                else if (XMessageBox.Show(CommonStr.strSqlServerNotConnected, wgTools.MSGTITLE, MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf(@"\") + 1) + "SqlConfig.exe";
                    startInfo.UseShellExecute = true;
                    Process.Start(startInfo);
                }
            }
            else
            {
                wgAppConfig.CustConfigureInit();
                UpgradeDatabase();
                try
                {
                    string str = wgAppConfig.getSystemParamByNO(30);
                    if (string.IsNullOrEmpty(str))
                    {
                        wgAppConfig.setSystemParamValue(30, "Application Version", Application.ProductVersion, "V9 当前使用的应用软件版本");
                    }
                    else if (str != Application.ProductVersion)
                    {
                        wgAppConfig.setSystemParamValue(30, "Application Version", Application.ProductVersion, "V9 当前使用的应用软件版本");
                    }
                }
                catch (Exception)
                {
                }
                wait.Close();
                Application.Run(new frmLogin());
                if (wgAppConfig.IsLogin)
                {
                    try
                    {
                        if (int.Parse(wgAppConfig.GetKeyVal("RunTimeAt")) >= 0)
                        {
                            DateTime time = DateTime.Parse("2012-12-1");
                            if ((DateTime.Now.Date >= time.Date) && (((int.Parse(wgAppConfig.GetKeyVal("RunTimeAt")) == 0) || (DateTime.Now.Date >= time.AddDays((double) int.Parse(wgAppConfig.GetKeyVal("RunTimeAt"))).Date)) || (DateTime.Now.AddDays(32.0).Date <= time.AddDays((double) int.Parse(wgAppConfig.GetKeyVal("RunTimeAt"))).Date)))
                            {
                                startSlowThread = new Thread(new ThreadStart(Program.getNewSoftware));
                                startSlowThread.IsBackground = true;
                                startSlowThread.CurrentUICulture = new CultureInfo(wgAppConfig.CultureInfoStr, false);
                                startSlowThread.Start();
                            }
                        }
                        else
                        {
                            wgAppConfig.UpdateKeyVal("RunTimeAt", "0");
                        }
                    }
                    catch (Exception)
                    {
                        wgAppConfig.UpdateKeyVal("RunTimeAt", "0");
                    }
                    try
                    {
                        if (string.IsNullOrEmpty(wgAppConfig.getSystemParamByNO(0x31)))
                        {
                            wgAppConfig.setSystemParamValue(0x31, "Install Time", DateTime.Now.ToString(wgTools.YMDHMSFormat), "");
                        }
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        string strFileName = "";
                        strFileName = wgAppConfig.Path4Photo();
                        if (!wgAppConfig.DirectoryIsExisted(strFileName))
                        {
                            wgAppConfig.CreatePhotoDirectory(strFileName);
                        }
                        if (!wgAppConfig.DirectoryIsExisted(strFileName))
                        {
                            wgAppConfig.wgLog(strFileName + " " + CommonStr.strFileDirectoryNotVisited);
                        }
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        string str4 = "";
                        str4 = wgAppConfig.Path4PhotoDefault();
                        if (!wgAppConfig.DirectoryIsExisted(str4))
                        {
                            Directory.CreateDirectory(str4);
                        }
                        if (!wgAppConfig.DirectoryIsExisted(str4))
                        {
                            wgAppConfig.wgLog(str4 + " " + CommonStr.strFileDirectoryNotVisited);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }

                if (wgAppConfig.IsLogin)
                {
                    Application.Run(new frmADCT3000());
                    if (wgAppConfig.IsAccessDB)
                    {
                        dfrmWait wait2 = new dfrmWait();
                        wait2.Show();
                        wait2.Refresh();
                        wgAppConfig.backupBeforeExitByJustCopy();
                        wait2.Hide();
                        wait2.Close();
                    }
                    if (wgAppConfig.gRestart)
                    {
                        ProcessStartInfo info2 = new ProcessStartInfo();
                        info2.FileName = Application.ExecutablePath;
                        info2.UseShellExecute = true;
                        info2.Arguments = "-R";
                        Process.Start(info2);
                    }
                }
                if (wgMail.bSendingMail)
                {
                    for (int i = 0; i < 30; i++)
                    {
                        Thread.Sleep(0x3e8);
                        if (!wgMail.bSendingMail)
                        {
                            break;
                        }
                    }
                }
                try
                {
                    Thread.Sleep(500);
                    Environment.Exit(0);
                }
                catch (Exception)
                {
                }
            }
        }

        public static void UpgradeDatabase()
        {
            if (wgAppConfig.IsAccessDB)
            {
                UpgradeDatabase_Acc();
            }
            else
            {
                SqlConnection connection = new SqlConnection(wgAppConfig.dbConString);
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
                            wgAppConfig.setSystemParamValue(9, "Database Version", dbVersionNewest.ToString(), string.Concat(new object[] { "V", result, " => V", dbVersionNewest }));
                            wgAppConfig.wgLog(string.Concat(new object[] { "V", result, " => V", dbVersionNewest }), EventLogEntryType.Information, null);
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
        }

        public static void UpgradeDatabase_Acc()
        {
            if (wgAppConfig.IsAccessDB)
            {
                OleDbConnection connection = new OleDbConnection(wgAppConfig.dbConString);
                try
                {
                    string cmdText = "";
                    float result = 0f;
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    cmdText = "SELECT f_Value FROM t_a_SystemParam WHERE f_No=9 ";
                    using (OleDbCommand command = new OleDbCommand(cmdText, connection))
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
                            wgAppConfig.setSystemParamValue(9, "Database Version", dbVersionNewest.ToString(), string.Concat(new object[] { "V", result, " => V", dbVersionNewest }));
                            wgAppConfig.wgLog(string.Concat(new object[] { "V", result, " => V", dbVersionNewest }), EventLogEntryType.Information, null);
                        }
                    }
                }
                catch (Exception exception)
                {
                    wgTools.WgDebugWrite(exception.ToString(), new object[0]);
                }
                finally
                {
                    connection.Dispose();
                }
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

        private static string wgAppConfigGetSystemParamByNO(int ParaNo)
        {
            return wgAppConfig.getSystemParamByNO(ParaNo);
        }

        private static bool wgAppConfigIsAccessDB()
        {
            return wgAppConfig.IsAccessDB;
        }

        private static int wgAppConfigRunUpdateSql(string strSql)
        {
            return wgAppConfig.runUpdateSql(strSql);
        }

        private static string wgToolsPrepareStr(object obj)
        {
            return wgTools.PrepareStr(obj);
        }

        private static string wgToolsPrepareStr(object obj, bool bDate, string dateFormat)
        {
            return wgTools.PrepareStr(obj, bDate, dateFormat);
        }

        private static void wgToolsWgDebugWrite(string info)
        {
            wgTools.WgDebugWrite(info, new object[0]);
        }
    }
}

