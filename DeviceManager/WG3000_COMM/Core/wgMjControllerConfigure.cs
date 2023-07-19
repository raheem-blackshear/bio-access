namespace WG3000_COMM.Core
{
    using System;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Text;

    public class wgMjControllerConfigure
    {
        public const int ALARMOUT_IF_24HOUR_PIN = 4;
        public const int ALARMOUT_IF_HELP_PIN = 5;
        public const int ALARMOUT_IF_SETBYCARD_PIN = 3;
        private int m_controllerSN;
        private byte[] m_needUpdateBits;
        private byte[] m_param;
        private byte[] m_param_Special;
        private string[] m_paramDesc;
        public const int WARN_MIN_TIMEOUT_DEFAULT = 30;
        public const int WARNBIT_ALARM = 6;
        public const int WARNBIT_DOORINVALIDOPEN = 2;
        public const int WARNBIT_DOORINVALIDREAD = 4;
        public const int WARNBIT_DOOROPENTOOLONG = 1;
        public const int WARNBIT_FIRELINK = 5;
        public const int WARNBIT_FORCE = 0;
        public const int WARNBIT_FORCE_WITHCARD = 7;
        public const int WARNBIT_FORCECLOSE = 3;
        public const int WARNOUT_IF_DOOROPENTOOLONG = 1;
        public const int WARNOUT_IF_FORCE = 0;
        public const int WARNOUT_NODELAY_IF_DOOROPENTOOLONG = 2;

        public wgMjControllerConfigure()
        {
            this.m_needUpdateBits = new byte[0x80];
            this.m_param_Special = new byte[0x400];
            byte[] buffer = new byte[] { 
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,//000
				0x00, 0x00, 0x03, 0x03, 0x03, 0x03, 0x00, 0x00, 
                0x00, 0x00, 0x01, 0x02, 0x03, 0x04, 0x00, 0x00,//010
				0x00, 0x00, 0xff, 0x00, 0xff, 0x00, 0x00, 0x00, 
                0x00, 0x00, 0x08, 0xfa, 0x00, 0x64, 0x00, 0xff,//020
				0x55, 0x01, 0x1e, 0x00, 0x00, 0x7e, 0x1e, 0x1e, 
                0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,//030
				0xff, 0xff, 0xff, 0xff, 0x00, 0x04, 0x32, 0xff, 
                0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,//040
				0xff, 0xff, 0x00, 0x00, 0xff, 0xff, 0xff, 0xff, 
                0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,//050
				0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 
                0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,//060
				0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 
                0xff, 0xff, 0xff, 0xff, 0xc0, 0xa8, 0x01, 0xe0,//070
				0xff, 0xff, 0xff, 0x00, 0xc0, 0xa8, 0x01, 0x01, 
                0x60, 0xea, 0x00, 0xa5, 0x00, 0x00, 0x00, 0x00,//080
				0x01, 0x00, 0x27, 0x00, 0x00, 0x00, 0x02, 0x00, 
                0x27, 0x00, 0x00, 0x00, 0x04, 0x00, 0x27, 0x00,//090
				0x00, 0x00, 0x10, 0x00, 0x00, 0x38, 0x00, 0x00, 
                0x0a, 0x00, 0x00, 0x0d, 0x00, 0x00, 0x00, 0x00,//0a0
				0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,//0b0
				0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,//0c0
				0x00, 0x00, 0x00, 0x00, 0x00, 0xff, 0xff, 0xff, 
                0xff, 0xff, 0xff, 0xff, 0x00, 0x00, 0x00, 0x00,//0d0
				0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xff, 0xff, 
                0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,//0e0
				0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,//0f0
				0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,//100
				0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,//110
				0x01, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 
                0x00, 0x00, 0x00, 0x00, 0xc0, 0xa8, 0x02, 0xe1,//120
				0xff, 0xff, 0xff, 0x00, 0xc0, 0xa8, 0x02, 0x01,
                0x60, 0xea, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,//130
				0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,//140
				0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,//150
				0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,//160
				0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,//170
				0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,//180
				0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,//190
				0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,//1a0
				0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,//1b0
				0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,//1c0
				0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,//1d0
				0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,//1e0
				0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            };
            buffer[0] = LowByte(0x7e0d);
            buffer[1] = HighByte(0x7e0d);
            buffer[2] = LowByte(30);
            buffer[3] = HighByte(30);
            buffer[4] = LowByte(30);
            buffer[5] = HighByte(30);
            buffer[6] = LowByte(30);
            buffer[7] = HighByte(30);
            buffer[8] = LowByte(30);
            buffer[9] = HighByte(30);
            buffer[0x4a] = LowByte(0xd9484);
            buffer[0x4b] = HighByte(0xd9484);
            buffer[0x80] = LowByte(0xea60);
            buffer[0x81] = HighByte(0xea60);
            buffer[140] = LowByte(30);
            buffer[0x8d] = HighByte(30);
            buffer[0x92] = LowByte(30);
            buffer[0x93] = HighByte(30);
            buffer[0x98] = LowByte(30);
            buffer[0x99] = HighByte(30);
            buffer[0x9e] = LowByte(0xbb8);
            buffer[0x9f] = HighByte(0xbb8);
            buffer[0xd4] = LowByte(0xee49);
            buffer[0xd5] = HighByte(0xee49);
            buffer[0xd6] = LowByte(0xee4a);
            buffer[0xd7] = HighByte(0xee4a);
            buffer[0xd8] = LowByte(0);
            buffer[0xd9] = HighByte(0);
            this.m_param = buffer;
            this.m_paramDesc = new string[] {
                "起始标识0D",
                "起始标识7E",
                "1号门开门延时默认30为3秒 低位", "高位",
                "2号门开门延时默认30为3秒 低位", "高位",
                "3号门开门延时默认30为3秒 低位", "高位",
                "4号门开门延时默认30为3秒 低位", "高位",
                "1号门控制 默认3在线, 2=常闭, 1=常开 0=NONE",
                "2号门控制 默认3在线, 2=常闭, 1=常开 0=NONE",
                "3号门控制 默认3在线, 2=常闭, 1=常开 0=NONE",
                "4号门控制 默认3在线, 2=常闭, 1=常开 0=NONE",
                "1号门互锁",
                "2号门互锁",
                "3号门互锁",
                "4号门互锁",
                "1号读头启用密码键盘",
                "2号读头启用密码键盘",
                "3号读头启用密码键盘",
                "4号读头启用密码键盘",
                "按钮缺省状态为FF[为高平]",
                "停用 门磁缺省状态0x00",
                "电锁正常状态值0xff",
                "记录按钮事件",
                "记录报警事件",
                "启用反潜回",
                "启用报警",
                "启用顺序验证",
                "读头指示灯亮0.8秒",
                "门打开后, 在规定的延时时间之后, (250)延长25秒, 超过了25秒则启用报警",
                "反潜时先进门后出门 (=0xa5时,可以先进, 也可先出)",
                "两张连续读卡的最长间隔100=10秒",
                "启用定时操作列表",
                "定时: 8个口触发动作4个门,4个扩展口",
                "01010101 单个读头, 进门读头启用 多卡同时开门",
                "报警自动复位 默认0x01",
                "报警的最小输出时间3秒(30)",
                "此通信时间内由电脑控制验证的开门(秒)",
                "假期约束, 默认00不启用",
                "防盗报警控制基本参数 ALARM_CONTROL_MODE_DEFAULT",
                "设防延时 30秒(30)",
                "撤防延时 30秒(30)", 
                "门内最多人数 低位", "门内最多人数 高位",
                "门内最少人数 低位", "门内最少人数 高位",
                "有效验证间隔 低位(单位:*2秒)",
                "验证有效间隔 高位",
                "心跳最大周期 低位(单位: 分钟)",
                "心跳最大周期 高位",
                "通信—接收火警信号", "通信—发送火警信号",
                "通信—接收互锁信号", "通信—发送互锁信号",
                "记录设防状态 默认是撤防 0x00",
                "单层延时", "多层延时",
                "通信—发送反潜回卡号信号", 
                "胁迫密码字节1", "胁迫密码字节2",
                "1号#1 17组开门密码(2个一组, 0xffff无效)",
                "1号读头组1 开门密码",
                "1号读头组2 开门密码",
                "1号读头组2 开门密码", 
                "1号读头组3 开门密码",
                "1号读头组3 开门密码",
                "1号读头组4 开门密码",
                "1号读头组4 开门密码",
                "2号读头组1 开门密码",
                "2号读头组1 开门密码",
                "2号读头组2 开门密码",
                "2号读头组2 开门密码",
                "2号读头组3 开门密码",
                "2号读头组3 开门密码",
                "2号读头组4 开门密码",
                "2号读头组4 开门密码",
                "3号读头组1 开门密码",
                "3号读头组1 开门密码",
                "3号读头组2 开门密码",
                "3号读头组2 开门密码", 
                "3号读头组3 开门密码",
                "3号读头组3 开门密码",
                "3号读头组4 开门密码",
                "3号读头组4 开门密码",
                "4号读头组1 开门密码",
                "4号读头组1 开门密码",
                "4号读头组2 开门密码",
                "4号读头组2 开门密码",
                "4号读头组3 开门密码",
                "4号读头组3 开门密码",
                "4号读头组4 开门密码",
                "4号读头组4 开门密码",
                "开门密码", "开门密码",
                "--停用", "--停用", "--停用",  "--停用", "--停用", "--停用", "--停用",
                "IP地址", "IP地址", "IP地址", "IP地址",
                "掩码", "掩码", "掩码", "掩码",
                "网关", "网关", "网关", "网关", 
                "端口低位 默认60000=0xEA60",
                "端口高位",
                "启用DHCP(=0xa5时表示启用) 2010-6-18",
                "启用auto_negotiation(=0xa5时表示启用) 2010-12-9",
                "停用 通信密码字节3",
                "停用 通信密码字节4",
                "停用 通信密码字节5",
                "停用 通信密码字节6",
                "扩展板1的参数表 默认四个继电器一一对应", "",
                "默认响应: 胁迫, 门打开时间过长, 强制开门, 火警", "",
                "动作时间缺省是10秒",
                "动作时间 高位",
                "扩展板2的参数表 默认四个继电器一一对应", "", 
                "默认响应: 胁迫, 门打开时间过长, 强制开门, 火警", "",
                "动作时间缺省是10秒",
                "动作时间 高位",
                "扩展板3的参数表 默认四个继电器一一对应", "",
                "默认响应: 胁迫, 门打开时间过长, 强制开门, 火警", "",
                "动作时间缺省是10秒",
                "动作时间 高位",
                "扩展板4的参数表 默认四个继电器一一对应", "", "",
                "防盗(验证,24小时,紧急",
                "动作时间缺省是300秒",
                "动作时间 高位",
                "读头性质",
                "通过密码键盘直接输入卡号,方式是:*卡号*",
                "门打开时间过长的高位[单位是25.6秒]",
                "胁迫密码字节3",
                "1号读头首卡开门",
                "2号读头首卡开门",
                "3号读头首卡开门",
                "4号读头首卡开门",
                "1号读头必须要到的人数",
                "2号读头必须要到的人数",
                "3号读头必须要到的人数",
                "4号读头必须要到的人数",
                "1号读头群1",
                "1号读头群2",
                "1号读头群3",
                "1号读头群4", 
                "1号读头群5",
                "1号读头群6",
                "1号读头群7",
                "1号读头群8",
                "2号读头群1",
                "2号读头群2",
                "2号读头群3",
                "2号读头群4",
                "2号读头群5",
                "2号读头群6",
                "2号读头群7",
                "2号读头群8",
                "3号读头群1",
                "3号读头群2",
                "3号读头群3",
                "3号读头群4", 
                "3号读头群5",
                "3号读头群6",
                "3号读头群7",
                "3号读头群8",
                "4号读头群1",
                "4号读头群2",
                "4号读头群3",
                "4号读头群4",
                "4号读头群5",
                "4号读头群6",
                "4号读头群7",
                "4号读头群8",
                "记录门磁事件[开门或关门的时间",
                "--停用 读头输出设置",
                "目标主机IP1 用于发送新的记录 默认广播全F",
                "目标主机IP2",
                "目标主机IP3",
                "目标主机IP4",
                "PC机 SERVER 工作口低位(61001)",
                "PC机 SERVER 工作口高位(61001)",
                "UDP CLIENT 控制端工作口低位(61002)",
                "UDP CLIENT 控制端工作口高位(61002)",
                "控制器主动发送的周期 低位. 默认不发",
                "控制器主动发送的周期 高位. 默认不发",
                "1号读头多卡选项",
                "2号读头多卡选项",
                "3号读头多卡选项",
                "4号读头多卡选项",
                "5号读头多卡选项",
                " 开关模式 LOCK_SWITCH_OPTION",
                "1号读头组1 开门密码第3字节", 
                "1号读头组2 开门密码第3字节",
                "1号读头组3 开门密码第3字节",
                "1号读头组4 开门密码第3字节",
                "2号读头组1 开门密码第3字节",
                "2号读头组2 开门密码第3字节",
                "2号读头组3 开门密码第3字节",
                "2号读头组4 开门密码第3字节",
                "3号读头组1 开门密码第3字节",
                "3号读头组2 开门密码第3字节",
                "3号读头组3 开门密码第3字节",
                "3号读头组4 开门密码第3字节",
                "4号读头组1 开门密码第3字节",
                "4号读头组2 开门密码第3字节",
                "4号读头组3 开门密码第3字节",
                "4号读头组4 开门密码第3字节",
				"--停用",
                "1号门 开始禁用的时段(不含0,1)",
                "2号门 开始禁用的时段(不含0,1)",
                "3号门 开始禁用的时段(不含0,1)",
                "4号门 开始禁用的时段(不含0,1)",
            };
            this.Clear();
        }

        public wgMjControllerConfigure(byte[] pkt, int startIndex)
        {
            this.m_needUpdateBits = new byte[0x80];
            this.m_param_Special = new byte[0x400];
            byte[] buffer = new byte[] { 
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 3, 3, 0, 0, 
                0, 0, 1, 2, 3, 4, 0, 0, 0, 0, 0xff, 0, 0xff, 0, 0, 0, 
                0, 0, 8, 250, 0, 100, 0, 0xff, 0x55, 1, 30, 0, 0, 0x7e, 30, 30, 
                0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0, 4, 50, 0xff, 
                0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0, 0, 0xff, 0xff, 0xff, 0xff, 
                0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 
                0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 
                0xff, 0xff, 0xff, 0xff, 0xc0, 0xa8, 0, 0, 0xff, 0xff, 0, 0, 0xc0, 0xa8, 0, 0, 
                0, 0, 0, 0xa5, 0, 0, 0, 0, 1, 0, 0x27, 0, 0, 0, 2, 0, 
                0x27, 0, 0, 0, 4, 0, 0x27, 0, 0, 0, 0x10, 0, 0, 0x38, 0, 0, 
                10, 0, 0, 13, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xff, 0xff, 0xff, 
                0xff, 0xff, 0xff, 0xff, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xff, 0xff, 
                0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
             };
            buffer[0] = LowByte(0x7e0d);
            buffer[1] = HighByte(0x7e0d);
            buffer[2] = LowByte(30);
            buffer[3] = HighByte(30);
            buffer[4] = LowByte(30);
            buffer[5] = HighByte(30);
            buffer[6] = LowByte(30);
            buffer[7] = HighByte(30);
            buffer[8] = LowByte(30);
            buffer[9] = HighByte(30);
            buffer[0x4a] = LowByte(0xd9484);
            buffer[0x4b] = HighByte(0xd9484);
            buffer[0x80] = LowByte(0xea60);
            buffer[0x81] = HighByte(0xea60);
            buffer[140] = LowByte(30);
            buffer[0x8d] = HighByte(30);
            buffer[0x92] = LowByte(30);
            buffer[0x93] = HighByte(30);
            buffer[0x98] = LowByte(30);
            buffer[0x99] = HighByte(30);
            buffer[0x9e] = LowByte(0xbb8);
            buffer[0x9f] = HighByte(0xbb8);
            buffer[0xd4] = LowByte(0xee49);
            buffer[0xd5] = HighByte(0xee49);
            buffer[0xd6] = LowByte(0xee4a);
            buffer[0xd7] = HighByte(0xee4a);
            buffer[0xd8] = LowByte(0);
            buffer[0xd9] = HighByte(0);
            this.m_param = buffer;
            this.m_paramDesc = new string[] {
                "起始标识0D",
                "起始标识7E",
                "1号门开门延时默认30为3秒 低位", "高位",
                "2号门开门延时默认30为3秒 低位", "高位",
                "3号门开门延时默认30为3秒 低位", "高位",
                "4号门开门延时默认30为3秒 低位", "高位",
                "1号门控制 默认3在线, 2=常闭, 1=常开 0=NONE",
                "2号门控制 默认3在线, 2=常闭, 1=常开 0=NONE",
                "3号门控制 默认3在线, 2=常闭, 1=常开 0=NONE",
                "4号门控制 默认3在线, 2=常闭, 1=常开 0=NONE",
                "1号门互锁",
                "2号门互锁", 
                "3号门互锁",
                "4号门互锁",
                "1号读头启用密码键盘",
                "2号读头启用密码键盘",
                "3号读头启用密码键盘",
                "4号读头启用密码键盘",
                "按钮缺省状态为FF[为高平]",
                "停用 门磁缺省状态0x00",
                "电锁正常状态值0xff",
                "记录按钮事件",
                "记录报警事件",
                "启用反潜回", 
                "启用报警",
                "启用顺序验证",
                "读头指示灯亮0.8秒",
                "门打开后, 在规定的延时时间之后, (250)延长25秒, 超过了25秒则启用报警",
                "反潜时先进门后出门 (=0xa5时,可以先进, 也可先出)",
                "两张连续读卡的最长间隔100=10秒",
                "启用定时操作列表",
                "定时: 8个口触发动作4个门,4个扩展口",
                "01010101 单个读头, 进门读头启用 多卡同时开门",
                "报警自动复位 默认0x01",
                "报警的最小输出时间3秒(30)",
                "此通信时间内由电脑控制验证的开门(秒)",
                "假期约束, 默认00不启用",
                "防盗报警控制基本参数 ALARM_CONTROL_MODE_DEFAULT",
                "设防延时 30秒(30)",
                "撤防延时 30秒(30)", 
                "门内最多人数 低位", "门内最多人数 高位",
                "门内最少人数 低位", "门内最少人数 高位",
                "有效验证间隔 低位(单位:*2秒)",
                "验证有效间隔 高位",
                "心跳最大周期 低位(单位: 分钟)",
                "心跳最大周期 高位",
                "通信—接收火警信号", "通信—发送火警信号",
                "通信—接收互锁信号", "通信—发送互锁信号",
                "记录设防状态 默认是撤防 0x00",
                "单层延时", "多层延时",
                "通信—发送反潜回卡号信号", 
                "胁迫密码字节1", "胁迫密码字节2",
                "1号#1 17组开门密码(2个一组, 0xffff无效)",
                "1号读头组1 开门密码",
                "1号读头组2 开门密码",
                "1号读头组2 开门密码", 
                "1号读头组3 开门密码",
                "1号读头组3 开门密码",
                "1号读头组4 开门密码",
                "1号读头组4 开门密码",
                "2号读头组1 开门密码",
                "2号读头组1 开门密码",
                "2号读头组2 开门密码",
                "2号读头组2 开门密码",
                "2号读头组3 开门密码",
                "2号读头组3 开门密码",
                "2号读头组4 开门密码",
                "2号读头组4 开门密码",
                "3号读头组1 开门密码",
                "3号读头组1 开门密码",
                "3号读头组2 开门密码",
                "3号读头组2 开门密码", 
                "3号读头组3 开门密码",
                "3号读头组3 开门密码",
                "3号读头组4 开门密码",
                "3号读头组4 开门密码",
                "4号读头组1 开门密码",
                "4号读头组1 开门密码",
                "4号读头组2 开门密码",
                "4号读头组2 开门密码",
                "4号读头组3 开门密码",
                "4号读头组3 开门密码",
                "4号读头组4 开门密码",
                "4号读头组4 开门密码",
                "开门密码", "开门密码",
                "--停用", "--停用", "--停用",  "--停用", "--停用", "--停用", "--停用",
                "IP地址", "IP地址", "IP地址", "IP地址",
                "掩码", "掩码", "掩码", "掩码",
                "网关", "网关", "网关", "网关", 
                "端口低位 默认60000=0xEA60",
                "端口高位",
                "启用DHCP(=0xa5时表示启用) 2010-6-18",
                "启用auto_negotiation(=0xa5时表示启用) 2010-12-9",
                "停用 通信密码字节3",
                "停用 通信密码字节4",
                "停用 通信密码字节5",
                "停用 通信密码字节6",
                "扩展板1的参数表 默认四个继电器一一对应", "",
                "默认响应: 胁迫, 门打开时间过长, 强制开门, 火警", "",
                "动作时间缺省是10秒",
                "动作时间 高位",
                "扩展板2的参数表 默认四个继电器一一对应", "", 
                "默认响应: 胁迫, 门打开时间过长, 强制开门, 火警", "",
                "动作时间缺省是10秒",
                "动作时间 高位",
                "扩展板3的参数表 默认四个继电器一一对应", "",
                "默认响应: 胁迫, 门打开时间过长, 强制开门, 火警", "",
                "动作时间缺省是10秒",
                "动作时间 高位",
                "扩展板4的参数表 默认四个继电器一一对应", "", "",
                "防盗(验证,24小时,紧急",
                "动作时间缺省是300秒",
                "动作时间 高位",
                "读头性质",
                "通过密码键盘直接输入卡号,方式是:*卡号*",
                "门打开时间过长的高位[单位是25.6秒]",
                "胁迫密码字节3",
                "1号读头首卡开门",
                "2号读头首卡开门",
                "3号读头首卡开门",
                "4号读头首卡开门",
                "1号读头必须要到的人数",
                "2号读头必须要到的人数",
                "3号读头必须要到的人数",
                "4号读头必须要到的人数",
                "1号读头群1",
                "1号读头群2",
                "1号读头群3",
                "1号读头群4", 
                "1号读头群5",
                "1号读头群6",
                "1号读头群7",
                "1号读头群8",
                "2号读头群1",
                "2号读头群2",
                "2号读头群3",
                "2号读头群4",
                "2号读头群5",
                "2号读头群6",
                "2号读头群7",
                "2号读头群8",
                "3号读头群1",
                "3号读头群2",
                "3号读头群3",
                "3号读头群4", 
                "3号读头群5",
                "3号读头群6",
                "3号读头群7",
                "3号读头群8",
                "4号读头群1",
                "4号读头群2",
                "4号读头群3",
                "4号读头群4",
                "4号读头群5",
                "4号读头群6",
                "4号读头群7",
                "4号读头群8",
                "记录门磁事件[开门或关门的时间",
                "--停用 读头输出设置",
                "目标主机IP1 用于发送新的记录 默认广播全F",
                "目标主机IP2",
                "目标主机IP3",
                "目标主机IP4",
                "PC机 SERVER 工作口低位(61001)",
                "PC机 SERVER 工作口高位(61001)",
                "UDP CLIENT 控制端工作口低位(61002)",
                "UDP CLIENT 控制端工作口高位(61002)",
                "控制器主动发送的周期 低位. 默认不发",
                "控制器主动发送的周期 高位. 默认不发",
                "1号读头多卡选项",
                "2号读头多卡选项",
                "3号读头多卡选项",
                "4号读头多卡选项",
                " 开关模式 LOCK_SWITCH_OPTION",
                "1号读头组1 开门密码第3字节", 
                "1号读头组2 开门密码第3字节",
                "1号读头组3 开门密码第3字节",
                "1号读头组4 开门密码第3字节",
                "2号读头组1 开门密码第3字节",
                "2号读头组2 开门密码第3字节",
                "2号读头组3 开门密码第3字节",
                "2号读头组4 开门密码第3字节",
                "3号读头组1 开门密码第3字节",
                "3号读头组2 开门密码第3字节",
                "3号读头组3 开门密码第3字节",
                "3号读头组4 开门密码第3字节",
                "4号读头组1 开门密码第3字节",
                "4号读头组2 开门密码第3字节",
                "4号读头组3 开门密码第3字节",
                "4号读头组4 开门密码第3字节",
				"--停用",
                "1号门 开始禁用的时段(不含0,1)",
                "2号门 开始禁用的时段(不含0,1)",
                "3号门 开始禁用的时段(不含0,1)",
                "4号门 开始禁用的时段(不含0,1)",
                
            };
            this.Clear();
            try
            {
                if (pkt[0] == 0x24)
                {
                    if (pkt[1] == 0x41)
                    {
                        this.m_controllerSN = ((pkt[8] + (pkt[9] << 8)) + (pkt[10] << 0x10)) + (pkt[11] << 0x18);
                        Array.Copy(pkt, startIndex, this.m_param_Special, 0x180, 4);
                        Array.Copy(pkt, startIndex, this.m_param, 0x74, 4);
                        Array.Copy(pkt, startIndex + 4, this.m_param_Special, 0x184, 4);
                        Array.Copy(pkt, startIndex + 4, this.m_param, 120, 4);
                        Array.Copy(pkt, startIndex + 8, this.m_param_Special, 0x188, 4);
                        Array.Copy(pkt, startIndex + 8, this.m_param, 0x7c, 4);
                        Array.Copy(pkt, startIndex + 12, this.m_param_Special, 0x80, 2);
                        Array.Copy(pkt, startIndex + 12, this.m_param, 0x80, 2);
                        Array.Copy(pkt, startIndex + 14, this.m_param_Special, 110, 6);
                        Array.Copy(pkt, startIndex + 14, this.m_param, 110, 6);
                        Array.Copy(pkt, startIndex + 20, this.m_param_Special, 0x1d4, 4);
                        Array.Copy(pkt, startIndex + 0x18, this.m_param_Special, 0x1d0, 2);
                        Array.Copy(pkt, startIndex + 0x1a, this.m_param_Special, 0x1d2, 1);
                        Array.Copy(pkt, startIndex + 0x1b, this.m_param_Special, 0x1d3, 1);
                    }
                    else if (pkt[1] == 0x11)
                    {
                        Array.Copy(pkt, startIndex, this.m_param, 0, this.m_param.Length);
                        this.m_controllerSN = ((pkt[8] + (pkt[9] << 8)) + (pkt[10] << 0x10)) + (pkt[11] << 0x18);
                        Array.Copy(pkt, startIndex, this.m_param_Special, 0, (this.m_param_Special.Length < pkt.Length) ? this.m_param_Special.Length : pkt.Length);
                    }
                }
            }
            catch (Exception exception)
            {
                wgTools.WgDebugWrite(exception.ToString(), new object[0]);
            }
        }

        private void ByteBitSet(ref byte param, int bitloc, bool value)
        {
            if ((bitloc >= 0) && (bitloc <= 7))
            {
                param = (byte) ((param & ~(((int) 1) << bitloc)) & 0xff);
                if (value)
                {
                    param = (byte) (param | ((byte) ((((int) 1) << bitloc) & 0xff)));
                }
            }
        }

        private bool ByteBitValue(byte param, int bitloc)
        {
            return (((bitloc >= 0) && (bitloc <= 7)) && ((param & (((int) 1) << bitloc)) > 0));
        }

        public void Clear()
        {
            for (int i = 0; i < this.m_needUpdateBits.Length; i++)
            {
                this.m_needUpdateBits[i] = 0;
            }
        }

        public int DoorControlGet(int DoorNO)
        {
            return this.m_param[10 + (DoorNO - 1)];
        }

        public void DoorControlSet(int DoorNO, int value)
        {
            if (this.IsValidNO_ReaderDoor(DoorNO))
            {
                this.m_param[10 + (DoorNO - 1)] = (byte) (value & 0xff);
                this.SetUpdateBits(10 + (DoorNO - 1), 1);
            }
        }

        public int DoorDelayGet(int DoorNO)
        {
            return (this.GetIntValue(2 + ((DoorNO - 1) * 2)) / 10);
        }

        public void DoorDelaySet(int DoorNO, int value)
        {
            if (DoorNO <= 4)
            {
                this.SetIntValue(2 + ((DoorNO - 1) * 2), value * 10);
            }
        }

        public int DoorDisableTimesegMinGet(int DoorNO)
        {
            return this.m_param[240 + (DoorNO - 1)];
        }

        public void DoorDisableTimesegMinSet(int DoorNO, int value)
        {
            if (this.IsValidNO_ReaderDoor(DoorNO))
            {
                this.m_param[240 + (DoorNO - 1)] = (byte) (value & 0xff);
                this.SetUpdateBits(240 + (DoorNO - 1), 1);
            }
        }

        public int DoorInterlockGet(int DoorNO)
        {
            return this.m_param[14 + (DoorNO - 1)];
        }

        public void DoorInterlockSet(int DoorNO, int value)
        {
            if (this.IsValidNO_ReaderDoor(DoorNO))
            {
                this.m_param[14 + (DoorNO - 1)] = (byte) (value & 0xff);
                this.SetUpdateBits(14 + (DoorNO - 1), 1);
            }
        }

        public int Ext_controlGet(int extPortNum)
        {
            if ((extPortNum >= 0) && (extPortNum <= 3))
            {
                return this.m_param[(0x88 + (extPortNum * 6)) + 1];
            }
            return 0;
        }

        public void Ext_controlSet(int extPortNum, int value)
        {
            if ((extPortNum >= 0) && (extPortNum <= 3))
            {
                this.m_param[(0x88 + (extPortNum * 6)) + 1] = (byte) (value & 0xff);
                this.SetUpdateBits((0x88 + (extPortNum * 6)) + 1, 1);
            }
        }

        public int Ext_doorGet(int extPortNum)
        {
            if ((extPortNum >= 0) && (extPortNum <= 3))
            {
                return this.m_param[0x88 + (extPortNum * 6)];
            }
            return 0;
        }

        public void Ext_doorSet(int extPortNum, int value)
        {
            if ((extPortNum >= 0) && (extPortNum <= 3))
            {
                this.m_param[0x88 + (extPortNum * 6)] = (byte) (value & 0xff);
                this.SetUpdateBits(0x88 + (extPortNum * 6), 1);
            }
        }

        public int Ext_timeoutGet(int extPortNum)
        {
            if ((extPortNum >= 0) && (extPortNum <= 3))
            {
                return (this.GetIntValue((0x88 + (extPortNum * 6)) + 4) / 10);
            }
            return 0;
        }

        public void Ext_timeoutSet(int extPortNum, int value)
        {
            if ((extPortNum >= 0) && (extPortNum <= 3))
            {
                this.SetIntValue((0x88 + (extPortNum * 6)) + 4, value * 10);
                this.SetUpdateBits((0x88 + (extPortNum * 6)) + 4, 2);
            }
        }

        public int Ext_warnSignalEnabled2Get(int extPortNum)
        {
            if ((extPortNum >= 0) && (extPortNum <= 3))
            {
                return this.m_param[(0x88 + (extPortNum * 6)) + 3];
            }
            return 0;
        }

        public void Ext_warnSignalEnabled2Set(int extPortNum, int value)
        {
            if ((extPortNum >= 0) && (extPortNum <= 3))
            {
                this.m_param[(0x88 + (extPortNum * 6)) + 3] = (byte) (value & 0xff);
                this.SetUpdateBits((0x88 + (extPortNum * 6)) + 3, 1);
            }
        }

        public int Ext_warnSignalEnabledGet(int extPortNum)
        {
            if ((extPortNum >= 0) && (extPortNum <= 3))
            {
                return this.m_param[(0x88 + (extPortNum * 6)) + 2];
            }
            return 0;
        }

        public void Ext_warnSignalEnabledSet(int extPortNum, int value)
        {
            if ((extPortNum >= 0) && (extPortNum <= 3))
            {
                this.m_param[(0x88 + (extPortNum * 6)) + 2] = (byte) (value & 0xff);
                this.SetUpdateBits((0x88 + (extPortNum * 6)) + 2, 1);
            }
        }

        public int FirstCardInfoGet(int DoorNO)
        {
            return this.m_param[0xa4 + (DoorNO - 1)];
        }

        public void FirstCardInfoSet(int DoorNO, int value)
        {
            if (this.IsValidNO_ReaderDoor(DoorNO))
            {
                this.m_param[0xa4 + (DoorNO - 1)] = (byte) (value & 0xff);
                this.SetUpdateBits(0xa4 + (DoorNO - 1), 1);
            }
        }

        private int GetIntValue(int paramLoc)
        {
            if ((paramLoc + 1) < this.m_param.Length)
            {
                return (this.m_param[paramLoc] + (this.m_param[paramLoc + 1] << 8));
            }
            return (this.m_param_Special[paramLoc] + (this.m_param_Special[paramLoc + 1] << 8));
        }

        private static byte HighByte(int i)
        {
            return (byte) ((i >> 8) & 0xff);
        }

        public int InputCardNOOpenGet(int ReaderNO)
        {
            if (this.IsValidNO_ReaderDoor(ReaderNO) && ((this.input_cardno & (((int) 1) << (ReaderNO - 1))) > 0))
            {
                return 1;
            }
            return 0;
        }

        public void InputCardNOOpenSet(int ReaderNO, int value)
        {
            if (this.IsValidNO_ReaderDoor(ReaderNO))
            {
                if (value > 0)
                {
                    this.input_cardno |= ((int) 1) << (ReaderNO - 1);
                }
                else
                {
                    this.input_cardno &= ~(((int) 1) << (ReaderNO - 1));
                }
            }
        }

        private bool IsValidNO_ReaderDoor(int value)
        {
            return ((value >= 1) && (value <= 4));
        }

        private static byte LowByte(int i)
        {
            return (byte) (i & 0xff);
        }

        public int MorecardGroupNeedCardsGet(int DoorNO, int GroupNO)
        {
            return this.m_param[((0xac + ((DoorNO - 1) * 8)) + GroupNO) - 1];
        }

        public void MorecardGroupNeedCardsSet(int DoorNO, int GroupNO, int value)
        {
            if (((this.IsValidNO_ReaderDoor(DoorNO) && (GroupNO >= 1)) && ((GroupNO <= 8) && (value >= 0))) && (value <= 0xff))
            {
                this.m_param[((0xac + ((DoorNO - 1) * 8)) + GroupNO) - 1] = (byte) (value & 0xff);
                this.SetUpdateBits(((0xac + ((DoorNO - 1) * 8)) + GroupNO) - 1, 1);
            }
        }

        public int MorecardNeedCardsGet(int DoorNo)
        {
            return this.m_param[(0xa8 + DoorNo) - 1];
        }

        public void MorecardNeedCardsSet(int DoorNo, int value)
        {
            if ((this.IsValidNO_ReaderDoor(DoorNo) && (value >= 0)) && (value <= 0xff))
            {
                this.m_param[(0xa8 + DoorNo) - 1] = (byte) (value & 0xff);
                this.SetUpdateBits((0xa8 + DoorNo) - 1, 1);
            }
        }

        public bool MorecardSequenceInputGet(int DoorNO)
        {
            return this.ByteBitValue(this.m_param[(0xda + DoorNO) - 1], 4);
        }

        public void MorecardSequenceInputSet(int DoorNO, bool value)
        {
            if (this.IsValidNO_ReaderDoor(DoorNO))
            {
                this.ByteBitSet(ref this.m_param[(0xda + DoorNO) - 1], 4, value);
                this.SetUpdateBits((0xda + DoorNO) - 1, 1);
            }
        }

        public bool MorecardSingleGroupEnableGet(int DoorNO)
        {
            return this.ByteBitValue(this.m_param[(0xda + DoorNO) - 1], 3);
        }

        public void MorecardSingleGroupEnableSet(int DoorNO, bool value)
        {
            if (this.IsValidNO_ReaderDoor(DoorNO))
            {
                this.ByteBitSet(ref this.m_param[(0xda + DoorNO) - 1], 3, value);
                this.SetUpdateBits((0xda + DoorNO) - 1, 1);
            }
        }

        public int MorecardSingleGroupStartNOGet(int DoorNO)
        {
            return ((this.m_param[(0xda + DoorNO) - 1] & 7) + 1);
        }

        public void MorecardSingleGroupStartNOSet(int DoorNO, int value)
        {
            if ((this.IsValidNO_ReaderDoor(DoorNO) && (value >= 1)) && (value <= 8))
            {
                this.m_param[(0xda + DoorNO) - 1] = (byte) (this.m_param[(0xda + DoorNO) - 1] & 0xf8);
                this.m_param[(0xda + DoorNO) - 1] = (byte) (this.m_param[(0xda + DoorNO) - 1] | ((byte) ((value - 1) & 7)));
                this.SetUpdateBits((0xda + DoorNO) - 1, 1);
            }
        }

        public int ReaderPasswordGet(int ReaderNO)
        {
            return this.m_param[0x16 + (ReaderNO - 1)];
        }

        public void ReaderAuthModeSet(int ReaderNO, int value)
        {
            if (this.IsValidNO_ReaderDoor(ReaderNO))
            {
                this.m_param[0x16 + (ReaderNO - 1)] = (byte) (value & 0xff);
                this.SetUpdateBits(0x16 + (ReaderNO - 1), 1);
            }
        }

        public void RestoreDefault()
        {
            int index = 0;
            while (index <= (this.m_param.Length >> 3))
            {
                this.m_needUpdateBits[index] = 0xff;
                index++;
            }
            this.m_needUpdateBits[index] = 0xff;
        }

        private void SetIntValue(int paramLoc, int value)
        {
            this.m_param[paramLoc] = LowByte(value);
            this.m_param[paramLoc + 1] = HighByte(value);
            this.SetUpdateBits(paramLoc, 2);
            this.m_param_Special[paramLoc] = LowByte(value);
            this.m_param_Special[paramLoc + 1] = HighByte(value);
        }

        private void SetUpdateBits(int paramloc, int paramByteLen)
        {
            if ((paramloc < 0x400) && ((paramloc + paramByteLen) < 0x400))
            {
                for (int i = paramloc; i < (paramloc + paramByteLen); i++)
                {
                    this.m_needUpdateBits[i >> 3] = (byte) (this.m_needUpdateBits[i >> 3] | ((byte) (((int) 1) << (i & 7))));
                }
            }
        }

        public int SuperpasswordGet(int pwdNO)
        {
            return (this.m_param[0xdf + (pwdNO - 1)] << (0x10 + this.GetIntValue(0x4c + ((pwdNO - 1) * 2))));
        }

        public void SuperpasswordSet(int pwdNO, int value)
        {
            if (pwdNO <= 0x11)
            {
                this.SetIntValue(0x4c + ((pwdNO - 1) * 2), value);
                this.SetUpdateBits(0x4c + ((pwdNO - 1) * 2), 2);
                this.m_param[0xdf + (pwdNO - 1)] = (byte) ((value >> 0x10) & 0xff);
                this.SetUpdateBits(0xdf + (pwdNO - 1), 1);
            }
        }

        public int antiback
        {
            get
            {
                return this.m_param[0x1f];
            }
            set
            {
                this.m_param[0x1f] = (byte) (value & 0xff);
                this.SetUpdateBits(0x1f, 1);
                switch (this.m_param[0x1f])
                {
                    case 1:
                        this.m_param[160] = 250;
                        break;

                    case 2:
                        this.m_param[160] = 250;
                        break;

                    case 3:
                        this.m_param[160] = 0x7e;
                        break;

                    case 4:
                        this.m_param[160] = 0xfe;
                        break;

                    default:
                        this.m_param[160] = 0;
                        break;
                }
                this.SetUpdateBits(160, 1);
            }
        }

        public int antiback_broadcast_send
        {
            get
            {
                if (this.m_param[0x3f] != 0xff)
                {
                    return this.m_param[0x3f];
                }
                return 0;
            }
            set
            {
                this.m_param[0x3f] = (byte) (value & 0xff);
                this.SetUpdateBits(0x3f, 1);
            }
        }

        public int autiback_allow_firstout_enable
        {
            get
            {
                return this.m_param[0x24];
            }
            set
            {
                this.m_param[0x24] = (byte) (value & 0xff);
                this.SetUpdateBits(0x24, 1);
            }
        }

        public int auto_negotiation_enable
        {
            get
            {
                return this.m_param[0x83];
            }
            set
            {
                this.m_param[0x83] = (byte) (value & 0xff);
                this.SetUpdateBits(0x83, 1);
            }
        }

        public int check_controller_online_timeout
        {
            get
            {
                return this.GetIntValue(0x36);
            }
            set
            {
                this.SetIntValue(0x36, value);
                this.SetUpdateBits(0x36, 2);
            }
        }

        public int controllerSN
        {
            get
            {
                return this.m_controllerSN;
            }
        }

        public int controlTaskList_enabled
        {
            get
            {
                return this.m_param[0x26];
            }
            set
            {
                this.m_param[0x26] = (byte) (value & 0xff);
                this.SetUpdateBits(0x26, 1);
            }
        }

        public int custom_cardformat_startloc
        {
            get
            {
                return this.m_param[0x85];
            }
            set
            {
                this.m_param[0x85] = (byte) (value & 0xff);
                this.SetUpdateBits(0x85, 1);
            }
        }

        public int custom_cardformat_sumcheck
        {
            get
            {
                return this.m_param[0x87];
            }
            set
            {
                this.m_param[0x87] = (byte) (value & 0xff);
                this.SetUpdateBits(0x87, 1);
            }
        }

        public int custom_cardformat_totalbits
        {
            get
            {
                return this.m_param[0x84];
            }
            set
            {
                this.m_param[0x84] = (byte) (value & 0xff);
                this.SetUpdateBits(0x84, 1);
            }
        }

        public int custom_cardformat_validbits
        {
            get
            {
                return this.m_param[0x86];
            }
            set
            {
                this.m_param[0x86] = (byte) (value & 0xff);
                this.SetUpdateBits(0x86, 1);
            }
        }

        public IPAddress dataServerIP
        {
            get
            {
                return IPAddress.Parse(string.Format("{0}.{1}.{2}.{3}", new object[] { this.m_param[0xd0], this.m_param[0xd1], this.m_param[210], this.m_param[0xd3] }));
            }
            set
            {
                value.GetAddressBytes().CopyTo(this.m_param, 0xd0);
                this.SetUpdateBits(0xd0, 4);
            }
        }

        public int dataServerPort
        {
            get
            {
                return this.GetIntValue(0xd4);
            }
            set
            {
                this.SetIntValue(0xd4, value);
                this.SetUpdateBits(0xd4, 2);
            }
        }

        public int dhcpEnable
        {
            get
            {
                return this.m_param[130];
            }
            set
            {
                this.m_param[130] = (byte) (value & 0xff);
                this.SetUpdateBits(130, 1);
            }
        }

        public int doorOpenTimeout
        {
            get
            {
                return ((this.m_param[0x23] + (this.m_param[0xa2] << 8)) / 10);
            }
            set
            {
                this.m_param[0x23] = (byte) ((value * 10) & 0xff);
                this.SetUpdateBits(0x23, 1);
                this.m_param[0xa2] = HighByte(value * 10);
                this.SetUpdateBits(0xa2, 1);
            }
        }

        public int doorstatusNormal
        {
            get
            {
                return this.m_param[0x1b];
            }
        }

        public float elevatorMultioutputDelay
        {
            get
            {
                return (((float) this.m_param[0x3e]) / 10f);
            }
            set
            {
                this.m_param[0x3e] = (byte) (((int) (value * 10f)) & 0xff);
                this.SetUpdateBits(0x3e, 1);
            }
        }

        public float elevatorSingleDelay
        {
            get
            {
                return (((float) this.m_param[0x3d]) / 10f);
            }
            set
            {
                this.m_param[0x3d] = (byte) (((int) (value * 10f)) & 0xff);
                this.SetUpdateBits(0x3d, 1);
            }
        }

        public int ext_Alarm_Status
        {
            get
            {
                return this.m_param[60];
            }
            set
            {
                this.m_param[60] = (byte) (value & 0xff);
                this.SetUpdateBits(60, 1);
            }
        }

        public int ext_AlarmControlMode
        {
            get
            {
                return this.m_param[0x2d];
            }
            set
            {
                this.m_param[0x2d] = (byte) (value & 0xff);
                this.SetUpdateBits(0x2d, 1);
            }
        }

        public int ext_SetAlarmOffDelay
        {
            get
            {
                return this.m_param[0x2f];
            }
            set
            {
                this.m_param[0x2f] = (byte) (value & 0xff);
                this.SetUpdateBits(0x2f, 1);
            }
        }

        public int ext_SetAlarmOnDelay
        {
            get
            {
                return this.m_param[0x2e];
            }
            set
            {
                this.m_param[0x2e] = (byte) (value & 0xff);
                this.SetUpdateBits(0x2e, 1);
            }
        }

        public int fire_broadcast_receive
        {
            get
            {
                if (this.m_param[0x38] != 0xff)
                {
                    return this.m_param[0x38];
                }
                return 0;
            }
            set
            {
                this.m_param[0x38] = (byte) (value & 0xff);
                this.SetUpdateBits(0x38, 1);
            }
        }

        public int fire_broadcast_send
        {
            get
            {
                if (this.m_param[0x39] != 0xff)
                {
                    return this.m_param[0x39];
                }
                return 0;
            }
            set
            {
                this.m_param[0x39] = (byte) (value & 0xff);
                this.SetUpdateBits(0x39, 1);
            }
        }

        public IPAddress gateway
        {
            get
            {
                if (wifi_channel == 0)
                    return IPAddress.Parse(string.Format("{0}.{1}.{2}.{3}",
                        new object[] { this.m_param[0x7c], this.m_param[0x7d], this.m_param[0x7e], this.m_param[0x7f] }));
                else
                    return IPAddress.Parse(string.Format("{0}.{1}.{2}.{3}",
                        new object[] { m_param[0x12c], m_param[0x12d], m_param[0x12e], m_param[0x12f] }));
            }
            set
            {
                if (wifi_channel == 0)
                {
                    value.GetAddressBytes().CopyTo(this.m_param, 0x7c);
                    this.SetUpdateBits(0x7c, 4);
                }
                else
                {
                    value.GetAddressBytes().CopyTo(m_param, 0x12c);
                    SetUpdateBits(0x12c, 4);
                }
            }
        }

        public int holidayControl
        {
            get
            {
                return this.m_param[0x2c];
            }
            set
            {
                this.m_param[0x2c] = (byte) (value & 0xff);
                this.SetUpdateBits(0x2c, 1);
            }
        }

        public int indoorPersonsMax
        {
            get
            {
                if (this.GetIntValue(0x30) != 0xffff)
                {
                    return this.GetIntValue(0x30);
                }
                return 0;
            }
            set
            {
                this.SetIntValue(0x30, value);
                this.SetUpdateBits(0x30, 2);
            }
        }

        public int indoorPersonsMin
        {
            get
            {
                if (this.GetIntValue(50) != 0xffff)
                {
                    return this.GetIntValue(50);
                }
                return 0;
            }
            set
            {
                this.SetIntValue(50, value);
                this.SetUpdateBits(50, 2);
            }
        }

        private int input_cardno
        {
            get
            {
                return this.m_param[0xa1];
            }
            set
            {
                this.m_param[0xa1] = (byte) (value & 0xff);
                this.SetUpdateBits(0xa1, 1);
            }
        }

        public int interlock_broadcast_receive
        {
            get
            {
                if (this.m_param[0x3a] != 0xff)
                {
                    return this.m_param[0x3a];
                }
                return 0;
            }
            set
            {
                this.m_param[0x3a] = (byte) (value & 0xff);
                this.SetUpdateBits(0x3a, 1);
            }
        }

        public int interlock_broadcast_send
        {
            get
            {
                if (this.m_param[0x3b] != 0xff)
                {
                    return this.m_param[0x3b];
                }
                return 0;
            }
            set
            {
                this.m_param[0x3b] = (byte) (value & 0xff);
                this.SetUpdateBits(0x3b, 1);
            }
        }

        public IPAddress ip
        {
            get
            {
                if (wifi_channel == 0)
                    return IPAddress.Parse(string.Format("{0}.{1}.{2}.{3}",
                        new object[] { this.m_param[0x74], this.m_param[0x75], this.m_param[0x76], this.m_param[0x77] }));
                else
                    return IPAddress.Parse(string.Format("{0}.{1}.{2}.{3}",
                        new object[] { m_param[0x124], m_param[0x125], m_param[0x126], m_param[0x127] }));
            }
            set
            {
                if (wifi_channel == 0)
                {
                    value.GetAddressBytes().CopyTo(this.m_param, 0x74);
                    this.SetUpdateBits(0x74, 4);
                }
                else
                {
                    value.GetAddressBytes().CopyTo(m_param, 0x124);
                    SetUpdateBits(0x124, 4);
                }
            }
        }

        public int lockNormal
        {
            get
            {
                return this.m_param[0x1c];
            }
        }

        public int lockSwitchOption
        {
            get
            {
                return ((this.m_param[0xde] ^ 0xff) & 0xff);
            }
            set
            {
                this.m_param[0xde] = (byte) ((value ^ 0xff) & 0xff);
                this.SetUpdateBits(0xde, 1);
            }
        }

        public string MACAddr
        {
            get
            {
                if (wifi_channel == 0)
                    return string.Format("{0:X2}-{1:X2}-{2:X2}-{3:X2}-{4:X2}-{5:X2}",
                        new object[] { this.m_param[0x6e], this.m_param[0x6f], this.m_param[0x70], this.m_param[0x71], this.m_param[0x72], this.m_param[0x73] });
                else
                    return string.Format("{0:X2}-{1:X2}-{2:X2}-{3:X2}-{4:X2}-{5:X2}",
                        new object[] { this.m_param[0x199], this.m_param[0x19a], this.m_param[0x19b], this.m_param[0x19c], this.m_param[0x19d], this.m_param[0x19e] });
            }
        }

        public IPAddress mask
        {
            get
            {
                if (wifi_channel == 0)
                    return IPAddress.Parse(string.Format("{0}.{1}.{2}.{3}",
                        new object[] { this.m_param[0x78], this.m_param[0x79], this.m_param[0x7a], this.m_param[0x7b] }));
                else
                    return IPAddress.Parse(string.Format("{0}.{1}.{2}.{3}",
                        new object[] { m_param[0x128], m_param[0x129], m_param[0x12A], m_param[0x12B] }));
            }
            set
            {
                if (wifi_channel == 0)
                {
                    value.GetAddressBytes().CopyTo(this.m_param, 0x78);
                    this.SetUpdateBits(0x78, 4);
                }
                else
                {
                    value.GetAddressBytes().CopyTo(m_param, 0x128);
                    SetUpdateBits(0x128, 4);
                }
            }
        }

        public int moreCardRead4Reader
        {
            get
            {
                return this.m_param[40];
            }
            set
            {
                this.m_param[40] = (byte) (value & 0xff);
                this.SetUpdateBits(40, 1);
            }
        }

        internal byte[] needUpdate
        {
            get
            {
                return this.m_needUpdateBits;
            }
        }

        public byte[] needUpdateBits
        {
            get
            {
                return this.m_needUpdateBits;
            }
        }

        public byte[] param
        {
            get
            {
                return this.m_param;
            }
        }

        internal byte[] paramData
        {
            get
            {
                return this.m_param;
            }
        }

        public string[] paramDesc
        {
            get
            {
                return this.m_paramDesc;
            }
        }

        public int pcControlSwipeTimeout
        {
            get
            {
                return this.m_param[0x2b];
            }
            set
            {
                this.m_param[0x2b] = (byte) (value & 0xff);
                this.SetUpdateBits(0x2b, 1);
            }
        }

        public string pcIPAddr { get; set; }

        public int port
        {
            get
            {
                if (wifi_channel == 0)
                    return this.GetIntValue(0x80);
                else
                    return this.GetIntValue(0x130);
            }
            set
            {
                if (wifi_channel == 0)
                {
                    this.SetIntValue(0x80, value);
                    this.SetUpdateBits(0x80, 2);
                }
                else
                {
                    SetIntValue(0x130, value);
                    SetUpdateBits(0x130, 2);
                }
            }
        }

        public int pushbuttonNormal
        {
            get
            {
                return this.m_param[0x1a];
            }
        }

        public int readerTimeout
        {
            get
            {
                return this.m_param[0x22];
            }
            set
            {
                this.m_param[0x22] = (byte) (value & 0xff);
                this.SetUpdateBits(0x22, 1);
            }
        }

        public int receventDS
        {
            get
            {
                return this.m_param[0xcc];
            }
            set
            {
                this.m_param[0xcc] = (byte) (value & 0xff);
                this.SetUpdateBits(0xcc, 1);
            }
        }

        public int receventPB
        {
            get
            {
                return this.m_param[0x1d];
            }
            set
            {
                this.m_param[0x1d] = (byte) (value & 0xff);
                this.SetUpdateBits(0x1d, 1);
            }
        }

        public int receventWarn
        {
            get
            {
                return this.m_param[30];
            }
            set
            {
                this.m_param[30] = (byte) (value & 0xff);
                this.SetUpdateBits(30, 1);
            }
        }

        public long SpecialCard_Mother1
        {
            get
            {
                return (this.GetIntValue(480) + ((this.GetIntValue(0x1e2) * 0x100L) * 0x100L));
            }
        }

        public long SpecialCard_Mother2
        {
            get
            {
                return (this.GetIntValue(0x1e8) + ((this.GetIntValue(490) * 0x100L) * 0x100L));
            }
        }

        public long SpecialCard_OnlyOpen1
        {
            get
            {
                return (this.GetIntValue(0x200) + ((this.GetIntValue(0x202) * 0x100L) * 0x100L));
            }
        }

        public long SpecialCard_OnlyOpen2
        {
            get
            {
                return (this.GetIntValue(520) + ((this.GetIntValue(0x20a) * 0x100L) * 0x100L));
            }
        }

        public int swipeGap
        {
            get
            {
                if (this.GetIntValue(0x34) != 0xffff)
                {
                    return (this.GetIntValue(0x34) * 2);
                }
                return 0;
            }
            set
            {
                this.SetIntValue(0x34, value / 2);
                this.SetUpdateBits(0x34, 2);
            }
        }

        public int swipeOrderMode
        {
            get
            {
                return this.m_param[0x21];
            }
            set
            {
                this.m_param[0x21] = (byte) (value & 0xff);
                this.SetUpdateBits(0x21, 1);
            }
        }

        public int twoCardReadTimeout
        {
            get
            {
                return this.m_param[0x25];
            }
            set
            {
                this.m_param[0x25] = (byte) (value & 0xff);
                this.SetUpdateBits(0x25, 1);
            }
        }

        public int warnAutoReset
        {
            get
            {
                return this.m_param[0x29];
            }
            set
            {
                this.m_param[0x29] = (byte) (value & 0xff);
                this.SetUpdateBits(0x29, 1);
            }
        }

        public int warnSetup
        {
            get
            {
                return this.m_param[0x20];
            }
            set
            {
                this.m_param[0x20] = (byte) (value & 0xff);
                this.SetUpdateBits(0x20, 1);
            }
        }

        public string webDateDisplayFormat
        {
            get
            {
                switch (this.m_param[0x1d2])
                {
                    case 1:
                        return "M-d-yyyy";

                    case 2:
                        return "d-M-yyyy";

                    case 3:
                        return "yyyy/M/d";

                    case 4:
                        return "M/d/yyyy";

                    case 5:
                        return "d/M/yyyy";
                }
                return "yyyy-MM-dd";
            }
        }

        public string webDateDisplayFormatCHS
        {
            get
            {
                switch (this.m_param[0x1d2])
                {
                    case 1:
                        return "月-日-年";

                    case 2:
                        return "日-月-年";

                    case 3:
                        return "年/月/日";

                    case 4:
                        return "月/日/年";

                    case 5:
                        return "日/月/年";
                }
                return "年-月-日";
            }
        }

        public string webDeviceName
        {
            get
            {
                if (((this.m_param[0xf4] == 0xff) && (this.m_param[0xf5] == 0xff)) || (this.m_param[0xf4] == 0))
                {
                    return "";
                }
                int count = 0;
                for (int i = 0; i < 0x20; i++)
                {
                    if (this.m_param[0xf4 + i] == 0)
                    {
                        break;
                    }
                    count++;
                }
                return Encoding.GetEncoding("utf-8").GetString(this.m_param, 0xf4, count);
            }
        }

        public string webLanguage
        {
            get
            {
                switch ((this.GetIntValue(0x1d4) + (this.GetIntValue(470) << 0x10)))
                {
                    case 0x2000:
                        return "中文[zh-CHS]";

                    case 0x3000:
                        return "English";

                    case 0x38000:
                        return "Other";
                }
                return "English";
            }
        }

        public int webPort
        {
            get
            {
                return this.GetIntValue(0x1d0);
            }
        }

        public int xpPassword
        {
            get
            {
                return (this.GetIntValue(0x4a) + (this.m_param[0xa3] << 0x10));
            }
            set
            {
                this.SetIntValue(0x4a, value);
                this.SetUpdateBits(0x4a, 1);
                this.m_param[0xa3] = (byte) ((value >> 0x10) & 0xff);
                this.SetUpdateBits(0xa3, 1);
            }
        }

        public byte wifi_channel
        {
            get
            {
                return m_param[0x198];
            }
            set
            {
                m_param[0x198] = value;
            }
        }

        public string getWifiSsid()
        {
            int count = 0;
            for (int i = 0; i < 0x21; i++)
            {
                if (m_param[0x133 + i] == 0)
                    break;
                count++;
            }
            return Encoding.ASCII.GetString(this.m_param, 0x133, count);
        }

        public void setWifiSsid(string ssid)
        {
            char[] raw = ssid.PadRight(0x21, '\0').ToCharArray();
            for (int i = 0; (i < 0x21) && (i < raw.Length); i++)
                m_param[0x133 + i] = (byte)(raw[i] & '\x00ff');
            SetUpdateBits(0x133, 0x21);
        }

        public string getWifiKey()
        {
            int count = 0;
            for (int i = 0; i < 0x40; i++)
            {
                if (m_param[0x158 + i] == 0)
                    break;
                count++;
            }
            return Encoding.ASCII.GetString(this.m_param, 0x158, count);
        }

        public void setWifiKey(string key)
        {
            char[] raw = key.PadRight(0x40, '\0').ToCharArray();
            for (int i = 0; (i < 0x40) && (i < raw.Length); i++)
                m_param[0x158 + i] = (byte)(raw[i] & '\x00ff');
            SetUpdateBits(0x158, 0x40);
        }

        public bool getWifiDhcp()
        {
            return (this.m_param[0x132] != 0);
        }

        public void setWiFiEnable(bool enable)
        {
            m_param[0x132] = enable ? (byte)1 : (byte)0;
            SetUpdateBits(0x132, 1);
        }

        public byte getRtCamera()
        {
            return m_param[0x19f];
        }

        public void setRtCamera(int camera)
        {
            m_param[0x19f] = (byte)camera;
            SetUpdateBits(0x19f, 1);
        }

        public void setTouchSensor(bool touch)
        {
            m_param[0x1a0] = touch ? (byte)1 : (byte)0;
            SetUpdateBits(0x1a0, 1);
        }

        public void setM1Card(bool m1)
        {
            m_param[0x1a1] = m1 ? (byte)1 : (byte)0;
            SetUpdateBits(0x1a1, 1);
        }

        public void setVolume(int volume)
        {
            m_param[0x1a2] = (byte)volume;
            SetUpdateBits(0x1a2, 1);
        }

        public void setFpIdent(bool fp_ident)
        {
            m_param[0x1a3] = fp_ident ? (byte)1 : (byte)0;
            SetUpdateBits(0x1a3, 1);
        }

        public void setFaceIdent(bool face_ident)
        {
            m_param[0x1a4] = face_ident ? (byte)1 : (byte)0;
            SetUpdateBits(0x1a4, 1);
        }
    }
}

