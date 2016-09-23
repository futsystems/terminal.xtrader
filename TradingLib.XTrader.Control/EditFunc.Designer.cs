using System.Windows.Forms;
namespace CStock
{
    partial class EditFunc : Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.password = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.color = new System.Windows.Forms.Button();
            this.input = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.drawk = new System.Windows.Forms.CheckBox();
            this.title = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.name = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.SubList = new System.Windows.Forms.ListBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.Gongsi = new System.Windows.Forms.TextBox();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.Log = new System.Windows.Forms.TextBox();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.xx = new System.Windows.Forms.ToolStripStatusLabel();
            this.yy = new System.Windows.Forms.ToolStripStatusLabel();
            this.sb3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.sb4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.sb5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.sb6 = new System.Windows.Forms.ToolStripStatusLabel();
            this.cd1 = new System.Windows.Forms.ColorDialog();
            this.Notes = new System.Windows.Forms.ListBox();
            this.panel1.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.password);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.color);
            this.panel1.Controls.Add(this.input);
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.drawk);
            this.panel1.Controls.Add(this.title);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.name);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(683, 68);
            this.panel1.TabIndex = 0;
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(276, 11);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(83, 21);
            this.password.TabIndex = 10;
            this.password.TextChanged += new System.EventHandler(this.password_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(244, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "密码:";
            // 
            // color
            // 
            this.color.Location = new System.Drawing.Point(401, 38);
            this.color.Name = "color";
            this.color.Size = new System.Drawing.Size(74, 25);
            this.color.TabIndex = 8;
            this.color.Text = "插入颜色";
            this.color.UseVisualStyleBackColor = true;
            this.color.Click += new System.EventHandler(this.color_Click);
            // 
            // input
            // 
            this.input.Location = new System.Drawing.Point(401, 11);
            this.input.Name = "input";
            this.input.Size = new System.Drawing.Size(74, 25);
            this.input.TabIndex = 7;
            this.input.Text = "插入参数";
            this.input.UseVisualStyleBackColor = true;
            this.input.Click += new System.EventHandler(this.input_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.button4);
            this.panel5.Controls.Add(this.button3);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel5.Location = new System.Drawing.Point(582, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(101, 68);
            this.panel5.TabIndex = 6;
            // 
            // button4
            // 
            this.button4.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button4.Location = new System.Drawing.Point(16, 38);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(72, 25);
            this.button4.TabIndex = 2;
            this.button4.Text = "取消";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button3.Location = new System.Drawing.Point(16, 9);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(72, 25);
            this.button3.TabIndex = 1;
            this.button3.Text = "确定";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(493, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(72, 25);
            this.button1.TabIndex = 5;
            this.button1.Text = "大写";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(494, 38);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(71, 25);
            this.button2.TabIndex = 0;
            this.button2.Text = "测试";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // drawk
            // 
            this.drawk.AutoSize = true;
            this.drawk.Location = new System.Drawing.Point(302, 41);
            this.drawk.Name = "drawk";
            this.drawk.Size = new System.Drawing.Size(66, 16);
            this.drawk.TabIndex = 4;
            this.drawk.Text = "显示K线";
            this.drawk.UseVisualStyleBackColor = true;
            this.drawk.CheckedChanged += new System.EventHandler(this.drawk_CheckedChanged);
            // 
            // title
            // 
            this.title.Location = new System.Drawing.Point(59, 38);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(220, 21);
            this.title.TabIndex = 3;
            this.title.TextChanged += new System.EventHandler(this.title_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "公式描述:";
            // 
            // name
            // 
            this.name.Location = new System.Drawing.Point(59, 11);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(179, 21);
            this.name.TabIndex = 1;
            this.name.TextChanged += new System.EventHandler(this.name_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "公式名称:";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.panel3.Controls.Add(this.splitter1);
            this.panel3.Controls.Add(this.SubList);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Controls.Add(this.Notes);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 71);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(683, 416);
            this.panel3.TabIndex = 2;
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(508, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 416);
            this.splitter1.TabIndex = 3;
            this.splitter1.TabStop = false;
            // 
            // SubList
            // 
            this.SubList.Dock = System.Windows.Forms.DockStyle.Right;
            this.SubList.FormattingEnabled = true;
            this.SubList.IntegralHeight = false;
            this.SubList.ItemHeight = 12;
            this.SubList.Location = new System.Drawing.Point(511, 0);
            this.SubList.Name = "SubList";
            this.SubList.Size = new System.Drawing.Size(172, 416);
            this.SubList.TabIndex = 2;
            this.SubList.SelectedIndexChanged += new System.EventHandler(this.SubList_SelectedIndexChanged);
            this.SubList.DoubleClick += new System.EventHandler(this.SubList_DoubleClick);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.panel4.Controls.Add(this.Gongsi);
            this.panel4.Controls.Add(this.splitter2);
            this.panel4.Controls.Add(this.Log);
            this.panel4.Controls.Add(this.listBox2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(442, 416);
            this.panel4.TabIndex = 0;
            // 
            // Gongsi
            // 
            this.Gongsi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Gongsi.HideSelection = false;
            this.Gongsi.Location = new System.Drawing.Point(0, 0);
            this.Gongsi.Multiline = true;
            this.Gongsi.Name = "Gongsi";
            this.Gongsi.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Gongsi.Size = new System.Drawing.Size(442, 314);
            this.Gongsi.TabIndex = 5;
            this.Gongsi.WordWrap = false;
            this.Gongsi.Click += new System.EventHandler(this.Gongsi_Click);
            this.Gongsi.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Gongsi_KeyDown);
            // 
            // splitter2
            // 
            this.splitter2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(0, 314);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(442, 3);
            this.splitter2.TabIndex = 4;
            this.splitter2.TabStop = false;
            // 
            // Log
            // 
            this.Log.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Log.Location = new System.Drawing.Point(0, 317);
            this.Log.Multiline = true;
            this.Log.Name = "Log";
            this.Log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Log.Size = new System.Drawing.Size(442, 99);
            this.Log.TabIndex = 2;
            this.Log.WordWrap = false;
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.ItemHeight = 12;
            this.listBox2.Items.AddRange(new object[] {
            "HIGH 最高价",
            "返回该周期最高价。",
            "用法： HIGH",
            "",
            "H 最高价",
            "返回该周期最高价。",
            "用法： H",
            "",
            "LOW 最低价",
            "返回该周期最低价。",
            "用法： LOW",
            "",
            "L 最低价",
            "返回该周期最低价。",
            "用法： L",
            "",
            "CLOSE 收盘价",
            "返回该周期收盘价。",
            "用法： CLOSE",
            "",
            "C 收盘价",
            "返回该周期收盘价。",
            "用法： C",
            "",
            "VOL 成交量",
            "返回该周期成交量。",
            "用法： VOL",
            "",
            "V 成交量",
            "返回该周期成交量。",
            "用法： V",
            "",
            "OPEN 开盘价",
            "返回该周期开盘价。",
            "用法： OPEN",
            "",
            "O： 开盘价",
            "返回该周期开盘价。",
            "用法： O",
            "",
            "ADVANCE 上涨家数",
            "返回该周期上涨家数。",
            "用法： ADVANCE　(本函数仅对大盘有效)",
            "",
            "DECLINE 下跌家数",
            "返回该周期下跌家数。",
            "用法： DECLINE　(本函数仅对大盘有效)",
            "",
            "AMOUNT 成交额",
            "返回该周期成交额。",
            "用法： AMOUNT",
            "",
            "ASKPRICE 委卖价",
            "返回委卖1--委卖3价格。",
            "用法： ASKPRICE(N)　N取1—3。",
            "(本函数仅个股在分笔成交分析周期有效)",
            "",
            "ASKVOL 委卖量",
            "返回委卖1--委卖3量。",
            "用法： ASKVOL(N)　N取1—3。",
            "(本函数仅个股在分笔成交分析周期有效)",
            "",
            "BIDPRICE 委买价",
            "返回委买1--委买3价格。",
            "用法： BIDPRICE(N)　N取1—3。",
            "(本函数仅个股在分笔成交分析周期有效)",
            "",
            "BIDVOL 委买量",
            "返回委买1--委买3量。",
            "用法： BIDVOL(N)　N取1—3。",
            "(本函数仅个股在分笔成交分析周期有效)",
            "",
            "BUYVOL 主动性买盘",
            "返回主动性买单量。",
            "用法：　BUYVOL　当本笔成交为主动性买盘时，其数值等于成交量，否则为0。",
            "(本函数仅个股在分笔成交分析周期有效)",
            "",
            "SELLVOL 主动性卖盘",
            "返回主动性卖单量。",
            "用法：　SELLVOL　当本笔成交为主动性卖盘时，其数值等于成交量，否则为0。",
            "(本函数仅个股在分笔成交分析周期有效)",
            "",
            "ISBUYORDER 主动性买单",
            "返回该成交是否为主动性买单。",
            "用法： ISBUYORDER　当本笔成交为主动性买盘时，返回1，否则为0。",
            "(本函数仅个股在分笔成交分析周期有效)",
            "",
            "ISSELLORDER 主动性卖单",
            "返回该成交是否为主动性卖单。",
            "用法： ISSELLORDER　当本笔成交为主动性卖盘时，返回1，否则为0。",
            "(本函数仅个股在分笔成交分析周期有效)",
            "",
            "ATE 日期",
            "取得该周期从1900以来的年月日。",
            "用法： DATE　例如函数返回1000101，表示2000年1月1日。",
            "",
            "TIME 时间",
            "取得该周期的时分秒。",
            "用法： TIME　函数返回有效值范围为(000000-235959)。",
            "",
            "YEAR 年份",
            "取得该周期的年份。",
            "用法：YEAR",
            "",
            "MONTH 月份",
            "取得该周期的月份。",
            "用法：MONTH　函数返回有效值范围为(1-12)。",
            "",
            "WEEK 星期",
            "取得该周期的星期数。",
            "用法： WEEK　函数返回有效值范围为(0-6)，0表示星期天。",
            "",
            "DAY 日期",
            "取得该周期的日期。",
            "用法： DAY　函数返回有效值范围为(1-31)。",
            "",
            "HOUR 小时",
            "取得该周期的小时数。",
            "用法： HOUR　函数返回有效值范围为(0-23)，对于日线及更长的分析周期值为0。",
            "",
            "MINUTE 分钟",
            "取得该周期的分钟数。",
            "用法： MINUTE　函数返回有效值范围为(0-59)，对于日线及更长的分析周期值为0。",
            "",
            "FROMOPEN 分钟",
            "求当前时刻距开盘有多长时间。",
            "用法：　FROMOPEN　返回当前时刻距开盘有多长时间，单位为分钟。",
            "例如:　 FROMOPEN　当前时刻为早上十点，则返回31。",
            "",
            "DRAWNULL 无效数",
            "返回无效数。",
            "用法： DRAWNULL",
            "例如：　IF(CLOSE>REF(CLOSE，1)，CLOSE，DRAWNULL)　表示下跌时分析图上不画线。",
            "",
            "BACKSET 向前赋值",
            "将当前位置到若干周期前的数据设为1。",
            "用法：　BACKSET(X，N)　若X非0，则将当前位置到N周期前的数值设为1。",
            "例如：　BACKSET(CLOSE>OPEN，2)　若收阳则将该周期及前一周期数值设为1，否则为0。",
            "",
            "BARSCOUNT 有效数据周期数",
            "求总的周期数。",
            "用法：　BARSCOUNT(X)　第一个有效数据到当前的天数。",
            "例如：　BARSCOUNT(CLOSE)　对于日线数据取得上市以来总交易日数，对于分笔成交取得当日成交笔数，对于1分钟线取得当日交易分钟数。",
            "",
            "CURRBARSCOUNT 到最后交易日的周期数",
            "求到最后交易日的周期数.",
            "用法:",
            "CURRBARSCOUNT 求到最后交易日的周期数",
            "",
            "TOTALBARSCOUNT 总的周期数",
            "求总的周期数.",
            "用法:",
            "TOTALBARSCOUNT 求总的周期数 ",
            "",
            "BARSLAST 上一次条件成立位置",
            "上一次条件成立到当前的周期数。",
            "用法：　BARSLAST(X)　上一次X不为0到现在的天数。",
            "例如：　BARSLAST(CLOSE/REF(CLOSE,1)>=1.1)　表示上一个涨停板到当前的周期数。",
            "",
            "BARSSINCE 第一个条件成立位置",
            "第一个条件成立到当前的周期数。",
            "用法：　BARSSINCE(X)　第一次X不为0到现在的天数。",
            "例如：　BARSSINCE(HIGH>10)　表示股价超过10元时到当前的周期数。",
            "",
            "COUNT 统计",
            "统计满足条件的周期数。",
            "用法：　COUNT(X，N)　统计N周期中满足X条件的周期数，若N=0则从第一个有效值开始。",
            "例如：　COUNT(CLOSE>OPEN，20)　表示统计20周期内收阳的周期数。",
            "",
            "HHV 最高值",
            "求最高值。",
            "用法：　HHV(X，N)　求N周期内X最高值，N=0则从第一个有效值开始。",
            "例如：　HHV(HIGH,30)　表示求30日最高价。",
            "",
            "HHVBARS 上一高点位置",
            "求上一高点到当前的周期数。",
            "用法：　HHVBARS(X，N)　求N周期内X最高值到当前周期数，N=0表示从第一个有效值开始统计。",
            "例如：　HHVBARS(HIGH，0)　求得历史新高到到当前的周期数。",
            "",
            "LLV 最低值",
            "求最低值。",
            "用法：　LLV(X，N)　求N周期内X最低值，N=0则从第一个有效值开始。",
            "例如：　LLV(LOW，0)　表示求历史最低价。",
            "",
            "LLVBARS 上一低点位置",
            "求上一低点到当前的周期数。",
            "用法：　LLVBARS(X，N)　求N周期内X最低值到当前周期数，N=0表示从第一个有效值开始统计。",
            "例如：　LLVBARS(HIGH，20)　求得20日最低点到当前的周期数。",
            "",
            "REVERSE 求相反数",
            "求相反数。",
            "用法：　REVERSE(X)　返回-X。",
            "例如：　REVERSE(CLOSE)　返回-CLOSE。",
            "",
            "REF 向前引用",
            "引用若干周期前的数据。",
            "用法：　REF(X，A)　引用A周期前的X值。",
            "例如：　REF(CLOSE，1)　表示上一周期的收盘价，在日线上就是昨收。",
            "",
            "REFDATE 指定引用",
            "引用指定日期的数据。",
            "用法：　REFDATE(X，A)　引用A日期的X值。",
            "例如：　REF(CLOSE，20011208)　表示2001年12月08日的收盘价。",
            "",
            "SUM 总和",
            "求总和。",
            "用法：　SUM(X，N)　统计N周期中X的总和，N=0则从第一个有效值开始。",
            "例如：　SUM(VOL，0)　表示统计从上市第一天以来的成交量总和。",
            "",
            "FILTER 过滤",
            "过滤连续出现的信号。",
            "用法：　FILTER(X，N)　X满足条件后，删除其后N周期内的数据置为0。",
            "例如：　FILTER(CLOSE>OPEN，5)　查找阳线，5天内再次出现的阳线不被记录在内。",
            "",
            "SUMBARS 累加到指定值的周期数",
            "向前累加到指定值到现在的周期数。",
            "用法：　SUMBARS(X，A)　将X向前累加直到大于等于A，返回这个区间的周期数。",
            "例如：　SUMBARS(VOL，CAPITAL)　求完全换手到现在的周期数。",
            "",
            "SMA 移动平均",
            "返回移动平均。",
            "用法：　SMA(X，N，M)　X的M日移动平均，M为权重，如Y=(X*M+Y\'*(N-M))/N",
            "",
            "MA 简单移动平均",
            "返回简单移动平均。",
            "用法：　MA(X，M)　X的M日简单移动平均。",
            "",
            "DMA 动态移动平均",
            "求动态移动平均。",
            "用法：　DMA(X，A)　求X的动态移动平均。",
            "算法：　若Y=DMA(X，A)则 Y=A*X+(1-A)*Y\'，其中Y\'表示上一周期Y值，A必须小于1。",
            "例如：　DMA(CLOSE，VOL/CAPITAL)　表示求以换手率作平滑因子的平均价。",
            "",
            "EMA(或EXPMA) 指数移动平均",
            "返回指数移动平均。",
            "用法：　EMA(X，M)　X的M日指数移动平均。",
            "",
            "MEMA 平滑移动平均",
            "返回平滑移动平均",
            "用法：　MEMA(X，M)　X的M日平滑移动平均。",
            "",
            "MEMA(X,N)与MA的差别在于起始值为一平滑值,而不是初始值",
            "EXPMEMA 指数平滑移动平均",
            "返回指数平滑移动平均。",
            "用法：　EXPMEMA(X，M)　X的M日指数平滑移动平均。",
            "",
            "EXPMEMA同EMA(即EXPMA)的差别在于他的起始值为一平滑值",
            "RANGE 介于某个范围之间",
            "用法：　RANGE(A,B,C)　A在B和C。",
            "例如：　RANGE(A，B，C)表示A大于B同时小于C时返回1，否则返回0。",
            "",
            "CONST 取值设为常数",
            "用法: 　CONST(A)　取A最后的值为常量.",
            "例如：　CONST(INDEXC)表示取大盘现价。",
            "",
            "CROSS 上穿",
            "两条线交叉。",
            "用法：　CROSS(A，B)　表示当A从下方向上穿过B时返回1，否则返回0。",
            "例如：　CROSS(MA(CLOSE，5)，MA(CLOSE，10))　表示5日均线与10日均线交金叉。",
            "",
            "LONGCROSS 维持一定周期后上穿",
            "两条线维持一定周期后交叉。",
            "用法：　LONGCROSS(A，B，N)　表示A在N周期内都小于B，本周期从下方向上穿过B时返回1，否则返回0。",
            "",
            "UPNDAY 连涨",
            "返回是否连涨周期数。",
            "用法：　UPNDAY(CLOSE,M)　表示连涨M个周期。",
            "",
            "DOWNNDAY 连跌",
            "返回是否连跌周期。",
            "用法：　DOWNNDAY(CLOSE，M)　表示连跌M个周期。",
            "",
            "NDAY 连大",
            "返回是否持续存在X>Y。",
            "用法：　NDAY(CLOSE，OPEN，3)　表示连续3日收阳线。",
            "",
            "EXIST 存在",
            "是否存在。",
            "用法：　EXIST(CLOSE>OPEN，10)　表示前10日内存在着阳线。",
            "",
            "EVERY 一直存在",
            "一直存在。",
            "用法：　EVERY(CLOSE>OPEN，10)　表示前10日内一直阳线。",
            "",
            "LAST 持续存在",
            "用法：　LAST(X,A,B)　 A>B，表示从前A日到前B日一直满足X条件。若A为0，表示从第一天开始，B为0，表示到最后日止。",
            "例如：　LAST(CLOSE>OPEN，10，5)　表示从前10日到前5日内一直阳线。",
            "",
            "NOT 取反",
            "求逻辑非。",
            "用法：　NOT(X)　返回非X，即当X=0时返回1，否则返回0。",
            "例如：　NOT(ISUP)　表示平盘或收阴。",
            "",
            "IF 逻辑判断",
            "根据条件求不同的值。",
            "用法：　IF(X，A，B)　若X不为0则返回A，否则返回B。",
            "例如：　IF(CLOSE>OPEN，HIGH，LOW)表示该周期收阳则返回最高值，否则返回最低值。",
            "",
            "IFF 逻辑判断",
            "根据条件求不同的值。",
            "用法：　IFF(X，A，B)　若X不为0则返回A，否则返回B。",
            "例如：　IFF(CLOSE>OPEN，HIGH，LOW)　表示该周期收阳则返回最高值，否则返回最低值。",
            "",
            "IFN 逻辑判断",
            "根据条件求不同的值。",
            "用法：　IFN(X，A，B)　若X不为0则返回B，否则返回A。",
            "例如：　IFN(CLOSE>OPEN，HIGH，LOW)　表示该周期收阴则返回最高值，否则返回最低值。",
            "",
            "MAX 较大值",
            "求最大值。",
            "用法：　MAX(A,B)　返回A和B中的较大值。",
            "例如：　MAX(CLOSE-OPEN，0)　表示若收盘价大于开盘价返回它们的差值，否则返回0。",
            "",
            "MIN 较小值",
            "求最小值。",
            "用法：　MIN(A，B)　返回A和B中的较小值。",
            "例如：　MIN(CLOSE，OPEN)　返回开盘价和收盘价中的较小值。",
            "",
            "ACOS 反余弦",
            "反余弦值。",
            "用法：　ACOS(X)　返回X的反余弦值。",
            "",
            "ASIN 反正弦",
            "反正弦值。",
            "用法：　ASIN(X)　返回X的反正弦值。",
            "",
            "ATAN 反正切",
            "反正切值。",
            "用法：　ATAN(X)　返回X的反正切值。",
            "",
            "COS 余弦",
            "余弦值。",
            "用法：　COS(X)　返回X的余弦值。",
            "",
            "SIN 正弦",
            "正弦值。",
            "用法：　SIN(X)　返回X的正弦值。",
            "",
            "TAN 正切",
            "正切值。",
            "用法：　TAN(X)　返回X的正切值。",
            "",
            "EXP 指数",
            "指数。",
            "用法：　EXP(X)　e的X次幂。",
            "例如：　EXP(CLOSE)　返回e的CLOSE次幂。",
            "",
            "LN 自然对数",
            "求自然对数。",
            "用法：　LN(X)　以e为底的对数。",
            "例如：　LN(CLOSE)　求收盘价的对数。",
            "",
            "LOG 对数",
            "求10为底的对数。",
            "用法：　LOG(X)　取得X的对数。",
            "例如：　LOG(100)　等于2。",
            "",
            "SQRT 开方",
            "开平方。",
            "用法：　SQRT(X)　 求X的平方根。",
            "例如：　SQRT(CLOSE)　收盘价的平方根。",
            "",
            "ABS 绝对值",
            "求绝对值。",
            "用法：　ABS(X)　返回X的绝对值。",
            "例如：　ABS(-34)　返回34。",
            "",
            "POW 乘幂",
            "乘幂。",
            "用法：　POW(A，B)　返回A的B次幂。",
            "例如：　POW(CLOSE，3)　求得收盘价的3次方。",
            "",
            "CEILING 向上舍入",
            "向上舍入。",
            "用法：　CEILING(A)　返回沿A数值增大方向最接近的整数。",
            "例如：　CEILING(12.3)　求得13，CEILING(-3.5)求得-3。",
            "",
            "FLOOR 向下舍入",
            "向下舍入。",
            "用法：　FLOOR(A)　返回沿A数值减小方向最接近的整数。",
            "例如：　FLOOR(12.3)　求得12，FLOOR(-3.5)求得-4。",
            "",
            "INTPART 取整",
            "用法：　INTPART(A)　返回沿A绝对值减小方向最接近的整数。",
            "例如：　INTPART(12.3)　求得12，INTPART(-3.5)求得-3。",
            "",
            "BETWEEN： 介于",
            "介于。",
            "用法：　BETWEEN(A，B，C)　表示A处于B和C之间时返回1，否则返回0。",
            "例如：　BETWEEN(CLOSE，MA(CLOSE，10)，MA(CLOSE，5))表示收盘价介于5日均线和10日均线之间。",
            "",
            "AVEDEV 平均绝对方差",
            "AVEDEV(X，N) 　返回平均绝对方差。",
            "",
            "DEVSQ 数据偏差平方和",
            "DEVSQ(X，N) 　返回数据偏差平方和。",
            "",
            "FORCAST 线性回归预测值",
            "FORCAST(X，N)　 返回线性回归预测值。",
            "",
            "SLOPE 线性回归斜率",
            "SLOPE(X，N)　 返回线性回归斜率。",
            "",
            "STD 估算标准差",
            "STD(X，N)　 返回估算标准差。",
            "",
            "STDP 总体标准差",
            "STDP(X，N)　 返回总体标准差。",
            "",
            "VAR 估算样本方差",
            "VAR(X，N)　 返回估算样本方差。",
            "",
            "VARP 总体样本方差",
            "VARP(X，N)　 返回总体样本方差 。",
            "",
            "BLOCKSETNUM 板块股票个数",
            "用法：　BLOCKSETNUM(板块名称)　返回该板块股票个数。",
            "",
            "HORCALC 多股统计",
            "用法：　HORCALC(板块名称，数据项，计算方式，权重)",
            "数据项：100-HIGH，101-OPEN，102-LOW，103-CLOSE，104-VOL，105-涨幅",
            "计算方式：　0-累加，1-排名次",
            "权重：　0-总股本，1-流通股本，2-等同权重，3-流通市值",
            "",
            "COST 成本分布",
            "成本分布情况。",
            "用法：　COST(10)，表示10%获利盘的价格是多少，即有10%的持仓量在该价格以下，其余90%在该价格以上，为套牢盘。",
            "该函数仅对日线分析周期有效。",
            "",
            "PEAK 波峰值",
            "前M个ZIG转向波峰值。",
            "用法：　PEAK(K，N，M)　表示之字转向ZIG(K，N)的前M个波峰的数值，M必须大于等于1。",
            "例如：　PEAK(1,5,1)　表示%5最高价ZIG转向的上一个波峰的数值。",
            "",
            "PEAKBARS 波峰位置",
            "前M个ZIG转向波峰到当前距离。",
            "用法：　PEAKBARS(K，N，M)　表示之字转向ZIG(K，N)的前M个波峰到当前的周期数，M必须大于等于1。",
            "例如：　PEAKBARS (0，5，1)　表示%5开盘价ZIG转向的上一个波峰到当前的周期数。",
            "",
            "SAR 抛物转向",
            "抛物转向。",
            "用法：　 SAR(N，S，M)，N为计算周期，S为步长，M为极值。",
            "例如：　SAR(10，2，20)　表示计算10日抛物转向，步长为2%，极限值为20%。",
            "",
            "SARTURN 抛物转向点",
            "抛物转向点。",
            "用法：　SARTURN(N，S，M)　N为计算周期，S为步长，M为极值，若发生向上转向则返回1，若发生向下转向则返回-1，否则为0。",
            "其用法与SAR函数相同。",
            "",
            "TROUGH 波谷值",
            "前M个ZIG转向波谷值。",
            "用法：　TROUGH(K，N，M)　表示之字转向ZIG(K，N)的前M个波谷的数值，M必须大于等于1。",
            "例如：　TROUGH(2，5，2)　表示%5最低价ZIG转向的前2个波谷的数值。",
            "",
            "TROUGHBARS 波谷位置",
            "前M个ZIG转向波谷到当前距离。",
            "用法：　TROUGHBARS(K，N，M)　表示之字转向ZIG(K，N)的前M个波谷到当前的周期数，M必须大于等于1。",
            "例如：　TROUGH(2，5，2)　表示%5最低价ZIG转向的前2个波谷到当前的周期数。",
            "",
            "WINNER 获利盘比例",
            "获利盘比例。",
            "用法：　WINNER(CLOSE)　表示以当前收市价卖出的获利盘比例。",
            "例如：　返回0.1表示10%获利盘，WINNER(10.5)表示10.5元价格的获利盘比例。",
            "该函数仅对日线分析周期有效。",
            "",
            "LWINNER 近期获利盘比例",
            "近期获利盘比例。",
            "用法：　LWINNER(5，CLOSE)　表示最近5天的那部分成本以当前收市价卖出的获利盘比例。例如返回0.1表示10%获利盘。",
            "",
            "PWINNER 远期获利盘比例",
            "远期获利盘比例。",
            "用法：　PWINNER(5，CLOSE)　表示5天前的那部分成本以当前收市价卖出的获利盘比例。例如返回0.1表示10%获利盘。",
            "",
            "COSTEX 区间成本",
            "区间成本。",
            "用法：　COSTEX(CLOSE，REF(CLOSE))，表示近两日收盘价格间筹码的成本，例如返回10表示区间成本为20元。",
            "该函数仅对日线分析周期有效。",
            "",
            "PPART 远期成本分布比例",
            "远期成本分布比例。",
            "用法：　PPART(10)，表示10前的成本占总成本的比例，0.2表示20%。",
            "",
            "ZIG 之字转向",
            "之字转向。",
            "用法：　ZIG(K，N)　当价格变化量超过N%时转向，K表示0:开盘价，1:最高价，2:最低价，3:收盘价，其余:数组信息",
            "例如：　ZIG(3，5)　表示收盘价的5%的ZIG转向。",
            "",
            "PLOYLINE 折线段",
            "在图形上绘制折线段。",
            "用法：　PLOYLINE(COND，PRICE)，当COND条件满足时，以PRICE位置为顶点画折线连接。",
            "例如：　PLOYLINE(HIGH>=HHV(HIGH，20)，HIGH)表示在创20天新高点之间画折线。",
            "",
            "",
            "DRAWLINE 绘制直线段",
            "在图形上绘制直线段。",
            "用法：　DRAWLINE(COND1，PRICE1，COND2，PRICE2，EXPAND)",
            "当COND1条件满足时，在PRICE1位置画直线起点，当COND2条件满足时，在PRICE2位置画直线终点，EXPAND为延长类型。",
            "例如：　DRAWLINE(HIGH>=HHV(HIGH，20)，HIGH，LOW<=LLV(LOW，20)，LOW，1)　表示在创20天新高与创20天新低之间画直" +
                "线并且向右延长。",
            "",
            "",
            "DRAWKLINE 绘制K线",
            "用法：　DRAWKLINE(HIGH，OPEN，LOW，CLOSE)　以HIGH为最高价，OPEN为开盘价，LOW为最低，CLOSE收盘画K线。",
            "",
            "STICKLINE 绘制柱线",
            "在图形上绘制柱线。",
            "用法：　STICKLINE(COND，PRICE1，PRICE2，WIDTH，EMPTY)，当COND条件满足时，在PRICE1和PRICE2位置之间画柱状线，宽" +
                "度为WIDTH(10为标准间距)，EMPTH不为0则画空心柱。",
            "例如：　STICKLINE(CLOSE>OPEN，CLOSE，OPEN，0.8，1)表示画K线中阳线的空心柱体部分。",
            "",
            "",
            "DRAWICON 绘制图标",
            "在图形上绘制小图标。",
            "用法：　DRAWICON(COND，PRICE，TYPE)，当COND条件满足时，在PRICE位置画TYPE号图标。",
            "例如：　DRAWICON(CLOSE>OPEN，LOW，1)　表示当收阳时在最低价位置画1号图标。图标一共有九个，图形如附图。序号，最下面的是“1”号，最上面的是" +
                "“9”号。",
            "",
            "",
            "DRAWTEXT 显示文字",
            "在图形上显示文字。",
            "用法：　DRAWTEXT(COND，PRICE，TEXT)，当COND条件满足时，在PRICE位置书写文字TEXT。",
            "例如：　DRAWTEXT(CLOSE/OPEN>1.08，LOW，\'大阳线\')表示当日涨幅大于8%时在最低价位置显示\'大阳线\'字样。",
            "",
            "COLOR 自定义色",
            "格式为COLOR+“RRGGBB”：RR、GG、BB表示红蓝色、绿色和蓝色的分量，每种颜色的取值范围是00-FF，采用了16进制。",
            "例如：MA5：MA(CLOSE，5)，COLOR00FFFF　表示纯红色与纯绿色的混合色：COLOR808000表示淡蓝色和淡绿色的混合色。",
            "COLORBLACK   画黑色",
            "COLORBLUE    画蓝色",
            "COLORGREEN   画绿色",
            "COLORCYAN    画青色",
            "COLORRED     画红色",
            "COLORMAGENTA 画洋红色",
            "COLORBROWN   画棕色",
            "COLORLIGRAY  画淡灰色",
            "COLORGRAY　  画深灰色",
            "COLORLIBLUE  画淡蓝色",
            "COLORLIGREEN 画淡绿色",
            "COLORLICYAN  画淡青色",
            "COLORLIRED   画淡红色",
            "COLORLIMAGENTA 画淡洋红色",
            "COLORYELLOW  画黄色",
            "COLORWHITE   画白色",
            "",
            "LINETHICK    线型粗细",
            "格式：“LINETHICK+(1-9)”　参数的取值范围在1—9之间，“LINETHICK1”表示最细的线，而“LINETHICK9”表示最粗的线。",
            "STICK      画柱状线",
            "COLORSTICK 画彩色柱状线",
            "VOLSTICK   画彩色柱状线",
            "成交量柱状线，当股价上涨时显示红色空心柱，则显示绿色实心柱",
            "LINESTICK  同时画出柱状线和指标线",
            "CROSSDOT   画小叉线",
            "CIRCLEDOT  画小圆圈线",
            "POINTDOT   画小圆点线"});
            this.listBox2.Location = new System.Drawing.Point(124, 36);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(224, 208);
            this.listBox2.TabIndex = 3;
            this.listBox2.Visible = false;
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.Controls.Add(this.statusStrip1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(3, 487);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(683, 25);
            this.panel2.TabIndex = 1;
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.statusStrip1.GripMargin = new System.Windows.Forms.Padding(0);
            this.statusStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.xx,
            this.yy,
            this.sb3,
            this.sb4,
            this.sb5,
            this.sb6});
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(683, 25);
            this.statusStrip1.TabIndex = 0;
            // 
            // xx
            // 
            this.xx.AutoSize = false;
            this.xx.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.xx.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.xx.Name = "xx";
            this.xx.Size = new System.Drawing.Size(60, 20);
            // 
            // yy
            // 
            this.yy.AutoSize = false;
            this.yy.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.yy.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.yy.Name = "yy";
            this.yy.Size = new System.Drawing.Size(60, 20);
            // 
            // sb3
            // 
            this.sb3.AutoSize = false;
            this.sb3.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.sb3.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.sb3.Name = "sb3";
            this.sb3.Size = new System.Drawing.Size(60, 20);
            // 
            // sb4
            // 
            this.sb4.AutoSize = false;
            this.sb4.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.sb4.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.sb4.Name = "sb4";
            this.sb4.Size = new System.Drawing.Size(60, 20);
            // 
            // sb5
            // 
            this.sb5.Name = "sb5";
            this.sb5.Size = new System.Drawing.Size(0, 20);
            // 
            // sb6
            // 
            this.sb6.AutoSize = false;
            this.sb6.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.sb6.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.sb6.Name = "sb6";
            this.sb6.Size = new System.Drawing.Size(60, 20);
            this.sb6.Click += new System.EventHandler(this.sb6_Click);
            // 
            // Notes
            // 
            this.Notes.FormattingEnabled = true;
            this.Notes.ItemHeight = 12;
            this.Notes.Items.AddRange(new object[] {
            "INPUT 可调节输入参数",
            "返回当前设定值",
            "用法: N:=INPUT(最小值，最大值，缺省值，当前值)",
            "例如:",
            "布林线",
            "TECHNAME=BOLL",
            "DRAWK=TRUE",
            "N:=INPUT(1,100,20,20)",
            "P:=INPUT(1,100,2,2)",
            "BOLL:EMA(CLOSE,N);",
            "UPPER:BOLL+2*STD(CLOSE,N);",
            "LOWER:BOLL-2*STD(CLOSE,N);",
            "CLOSE,COLORRED;",
            "DRAWK:=TRUE",
            "",
            "MOD 取余数",
            "用法：MOD(NA,NB) 取NA除以NB之后的余数",
            "例如：N:=MOD(5,3) ",
            "",
            "PARTLINE 在图形上绘制折线段",
            "用法:PARTLINE(N,升颜色，跌颜色)",
            "例如:PARTLINE(CLOSE,CLOLORRED,COLORGREEN)",
            "",
            "BTX  宝塔线",
            "用法：BTX(N,涨颜色，跌颜色)",
            "例如：BTX(CLOSE,COLORRED,COLORGREEN)",
            "",
            "HIGH 最高价",
            "返回该周期最高价。",
            "用法： HIGH",
            "",
            "H 最高价",
            "返回该周期最高价。",
            "用法： H",
            "",
            "LOW 最低价",
            "返回该周期最低价。",
            "用法： LOW",
            "",
            "L 最低价",
            "返回该周期最低价。",
            "用法： L",
            "",
            "CLOSE 收盘价",
            "返回该周期收盘价。",
            "用法： CLOSE",
            "",
            "C 收盘价",
            "返回该周期收盘价。",
            "用法： C",
            "",
            "VOL 成交量",
            "返回该周期成交量。",
            "用法： VOL",
            "",
            "V 成交量",
            "返回该周期成交量。",
            "用法： V",
            "",
            "OPEN 开盘价",
            "返回该周期开盘价。",
            "用法： OPEN",
            "",
            "O： 开盘价",
            "返回该周期开盘价。",
            "用法： O",
            "",
            "ADVANCE 上涨家数",
            "返回该周期上涨家数。",
            "用法： ADVANCE　(本函数仅对大盘有效)",
            "",
            "DECLINE 下跌家数",
            "返回该周期下跌家数。",
            "用法： DECLINE　(本函数仅对大盘有效)",
            "",
            "AMOUNT 成交额",
            "返回该周期成交额。",
            "用法： AMOUNT",
            "",
            "ASKPRICE 委卖价",
            "返回委卖1--委卖3价格。",
            "用法： ASKPRICE(N)　N取1—3。",
            "(本函数仅个股在分笔成交分析周期有效)",
            "",
            "ASKVOL 委卖量",
            "返回委卖1--委卖3量。",
            "用法： ASKVOL(N)　N取1—3。",
            "(本函数仅个股在分笔成交分析周期有效)",
            "",
            "BIDPRICE 委买价",
            "返回委买1--委买3价格。",
            "用法： BIDPRICE(N)　N取1—3。",
            "(本函数仅个股在分笔成交分析周期有效)",
            "",
            "BIDVOL 委买量",
            "返回委买1--委买3量。",
            "用法： BIDVOL(N)　N取1—3。",
            "(本函数仅个股在分笔成交分析周期有效)",
            "",
            "BUYVOL 主动性买盘",
            "返回主动性买单量。",
            "用法：　BUYVOL　当本笔成交为主动性买盘时，其数值等于成交量，否则为0。",
            "(本函数仅个股在分笔成交分析周期有效)",
            "",
            "SELLVOL 主动性卖盘",
            "返回主动性卖单量。",
            "用法：　SELLVOL　当本笔成交为主动性卖盘时，其数值等于成交量，否则为0。",
            "(本函数仅个股在分笔成交分析周期有效)",
            "",
            "ISBUYORDER 主动性买单",
            "返回该成交是否为主动性买单。",
            "用法： ISBUYORDER　当本笔成交为主动性买盘时，返回1，否则为0。",
            "(本函数仅个股在分笔成交分析周期有效)",
            "",
            "ISSELLORDER 主动性卖单",
            "返回该成交是否为主动性卖单。",
            "用法： ISSELLORDER　当本笔成交为主动性卖盘时，返回1，否则为0。",
            "(本函数仅个股在分笔成交分析周期有效)",
            "",
            "SGN 是否大于0",
            "返回：大于0 返回1  小于0返回-1  等于0 返回0",
            "",
            "REVRRSE 返回负数",
            "返回: REVRRSE(CLOSE)  为0-CLOSE",
            "",
            "ATE 日期",
            "取得该周期从1900以来的年月日。",
            "用法： DATE　例如函数返回1000101，表示2000年1月1日。",
            "",
            "TIME 时间",
            "取得该周期的时分秒。",
            "用法： TIME　函数返回有效值范围为(000000-235959)。",
            "",
            "YEAR 年份",
            "取得该周期的年份。",
            "用法：YEAR",
            "",
            "MONTH 月份",
            "取得该周期的月份。",
            "用法：MONTH　函数返回有效值范围为(1-12)。",
            "",
            "WEEK 星期",
            "取得该周期的星期数。",
            "用法： WEEK　函数返回有效值范围为(0-6)，0表示星期天。",
            "",
            "DAY 日期",
            "取得该周期的日期。",
            "用法： DAY　函数返回有效值范围为(1-31)。",
            "",
            "HOUR 小时",
            "取得该周期的小时数。",
            "用法： HOUR　函数返回有效值范围为(0-23)，对于日线及更长的分析周期值为0。",
            "",
            "MINUTE 分钟",
            "取得该周期的分钟数。",
            "用法： MINUTE　函数返回有效值范围为(0-59)，对于日线及更长的分析周期值为0。",
            "",
            "FROMOPEN 分钟",
            "求当前时刻距开盘有多长时间。",
            "用法：　FROMOPEN　返回当前时刻距开盘有多长时间，单位为分钟。",
            "例如:　 FROMOPEN　当前时刻为早上十点，则返回31。",
            "",
            "DRAWNULL 无效数",
            "返回无效数。",
            "用法： DRAWNULL",
            "例如：　IF(CLOSE>REF(CLOSE，1)，CLOSE，DRAWNULL)　表示下跌时分析图上不画线。",
            "",
            "BACKSET 向前赋值",
            "将当前位置到若干周期前的数据设为1。",
            "用法：　BACKSET(X，N)　若X非0，则将当前位置到N周期前的数值设为1。",
            "例如：　BACKSET(CLOSE>OPEN，2)　若收阳则将该周期及前一周期数值设为1，否则为0。",
            "",
            "BARSCOUNT 有效数据周期数",
            "求总的周期数。",
            "用法：　BARSCOUNT(X)　第一个有效数据到当前的天数。",
            "例如：　BARSCOUNT(CLOSE)　对于日线数据取得上市以来总交易日数，对于分笔成交取得当日成交笔数，对于1分钟线取得当日交易分钟数。",
            "",
            "CURRBARSCOUNT 到最后交易日的周期数",
            "求到最后交易日的周期数.",
            "用法:",
            "CURRBARSCOUNT 求到最后交易日的周期数",
            "",
            "TOTALBARSCOUNT 总的周期数",
            "求总的周期数.",
            "用法:",
            "TOTALBARSCOUNT 求总的周期数 ",
            "",
            "BARSLAST 上一次条件成立位置",
            "上一次条件成立到当前的周期数。",
            "用法：　BARSLAST(X)　上一次X不为0到现在的天数。",
            "例如：　BARSLAST(CLOSE/REF(CLOSE,1)>=1.1)　表示上一个涨停板到当前的周期数。",
            "",
            "BARSSINCE 第一个条件成立位置",
            "第一个条件成立到当前的周期数。",
            "用法：　BARSSINCE(X)　第一次X不为0到现在的天数。",
            "例如：　BARSSINCE(HIGH>10)　表示股价超过10元时到当前的周期数。",
            "",
            "COUNT 统计",
            "统计满足条件的周期数。",
            "用法：　COUNT(X，N)　统计N周期中满足X条件的周期数，若N=0则从第一个有效值开始。",
            "例如：　COUNT(CLOSE>OPEN，20)　表示统计20周期内收阳的周期数。",
            "",
            "HHV 最高值",
            "求最高值。",
            "用法：　HHV(X，N)　求N周期内X最高值，N=0则从第一个有效值开始。",
            "例如：　HHV(HIGH,30)　表示求30日最高价。",
            "",
            "HHVBARS 上一高点位置",
            "求上一高点到当前的周期数。",
            "用法：　HHVBARS(X，N)　求N周期内X最高值到当前周期数，N=0表示从第一个有效值开始统计。",
            "例如：　HHVBARS(HIGH，0)　求得历史新高到到当前的周期数。",
            "",
            "LLV 最低值",
            "求最低值。",
            "用法：　LLV(X，N)　求N周期内X最低值，N=0则从第一个有效值开始。",
            "例如：　LLV(LOW，0)　表示求历史最低价。",
            "",
            "LLVBARS 上一低点位置",
            "求上一低点到当前的周期数。",
            "用法：　LLVBARS(X，N)　求N周期内X最低值到当前周期数，N=0表示从第一个有效值开始统计。",
            "例如：　LLVBARS(HIGH，20)　求得20日最低点到当前的周期数。",
            "",
            "REVERSE 求相反数",
            "求相反数。",
            "用法：　REVERSE(X)　返回-X。",
            "例如：　REVERSE(CLOSE)　返回-CLOSE。",
            "",
            "REF 向前引用",
            "引用若干周期前的数据。",
            "用法：　REF(X，A)　引用A周期前的X值。",
            "例如：　REF(CLOSE，1)　表示上一周期的收盘价，在日线上就是昨收。",
            "",
            "REFDATE 指定引用",
            "引用指定日期的数据。",
            "用法：　REFDATE(X，A)　引用A日期的X值。",
            "例如：　REF(CLOSE，20011208)　表示2001年12月08日的收盘价。",
            "",
            "SUM 总和",
            "求总和。",
            "用法：　SUM(X，N)　统计N周期中X的总和，N=0则从第一个有效值开始。",
            "例如：　SUM(VOL，0)　表示统计从上市第一天以来的成交量总和。",
            "",
            "FILTER 过滤",
            "过滤连续出现的信号。",
            "用法：　FILTER(X，N)　X满足条件后，删除其后N周期内的数据置为0。",
            "例如：　FILTER(CLOSE>OPEN，5)　查找阳线，5天内再次出现的阳线不被记录在内。",
            "",
            "SUMBARS 累加到指定值的周期数",
            "向前累加到指定值到现在的周期数。",
            "用法：　SUMBARS(X，A)　将X向前累加直到大于等于A，返回这个区间的周期数。",
            "例如：　SUMBARS(VOL，CAPITAL)　求完全换手到现在的周期数。",
            "",
            "SMA 移动平均",
            "返回移动平均。",
            "用法：　SMA(X，N，M)　X的M日移动平均，M为权重，如Y=(X*M+Y\'*(N-M))/N",
            "",
            "MA 简单移动平均",
            "返回简单移动平均。",
            "用法：　MA(X，M)　X的M日简单移动平均。",
            "",
            "DMA 动态移动平均",
            "求动态移动平均。",
            "用法：　DMA(X，A)　求X的动态移动平均。",
            "算法：　若Y=DMA(X，A)则 Y=A*X+(1-A)*Y\'，其中Y\'表示上一周期Y值，A必须小于1。",
            "例如：　DMA(CLOSE，VOL/CAPITAL)　表示求以换手率作平滑因子的平均价。",
            "",
            "EMA(或EXPMA) 指数移动平均",
            "返回指数移动平均。",
            "用法：　EMA(X，M)　X的M日指数移动平均。",
            "",
            "MEMA 平滑移动平均",
            "返回平滑移动平均",
            "用法：　MEMA(X，M)　X的M日平滑移动平均。",
            "",
            "MEMA(X,N)与MA的差别在于起始值为一平滑值,而不是初始值",
            "EXPMEMA 指数平滑移动平均",
            "返回指数平滑移动平均。",
            "用法：　EXPMEMA(X，M)　X的M日指数平滑移动平均。",
            "",
            "EXPMEMA同EMA(即EXPMA)的差别在于他的起始值为一平滑值",
            "RANGE 介于某个范围之间",
            "用法：　RANGE(A,B,C)　A在B和C。",
            "例如：　RANGE(A，B，C)表示A大于B同时小于C时返回1，否则返回0。",
            "",
            "CONST 取值设为常数",
            "用法: 　CONST(A)　取A最后的值为常量.",
            "例如：　CONST(INDEXC)表示取大盘现价。",
            "",
            "CROSS 上穿",
            "两条线交叉。",
            "用法：　CROSS(A，B)　表示当A从下方向上穿过B时返回1，否则返回0。",
            "例如：　CROSS(MA(CLOSE，5)，MA(CLOSE，10))　表示5日均线与10日均线交金叉。",
            "",
            "LONGCROSS 维持一定周期后上穿",
            "两条线维持一定周期后交叉。",
            "用法：　LONGCROSS(A，B，N)　表示A在N周期内都小于B，本周期从下方向上穿过B时返回1，否则返回0。",
            "",
            "UPNDAY 连涨",
            "返回是否连涨周期数。",
            "用法：　UPNDAY(CLOSE,M)　表示连涨M个周期。",
            "",
            "DOWNNDAY 连跌",
            "返回是否连跌周期。",
            "用法：　DOWNNDAY(CLOSE，M)　表示连跌M个周期。",
            "",
            "NDAY 连大",
            "返回是否持续存在X>Y。",
            "用法：　NDAY(CLOSE，OPEN，3)　表示连续3日收阳线。",
            "",
            "EXIST 存在",
            "是否存在。",
            "用法：　EXIST(CLOSE>OPEN，10)　表示前10日内存在着阳线。",
            "",
            "EVERY 一直存在",
            "一直存在。",
            "用法：　EVERY(CLOSE>OPEN，10)　表示前10日内一直阳线。",
            "",
            "LAST 持续存在",
            "用法：　LAST(X,A,B)　 A>B，表示从前A日到前B日一直满足X条件。若A为0，表示从第一天开始，B为0，表示到最后日止。",
            "例如：　LAST(CLOSE>OPEN，10，5)　表示从前10日到前5日内一直阳线。",
            "",
            "NOT 取反",
            "求逻辑非。",
            "用法：　NOT(X)　返回非X，即当X=0时返回1，否则返回0。",
            "例如：　NOT(ISUP)　表示平盘或收阴。",
            "",
            "IF 逻辑判断",
            "根据条件求不同的值。",
            "用法：　IF(X，A，B)　若X不为0则返回A，否则返回B。",
            "例如：　IF(CLOSE>OPEN，HIGH，LOW)表示该周期收阳则返回最高值，否则返回最低值。",
            "",
            "IFF 逻辑判断",
            "根据条件求不同的值。",
            "用法：　IFF(X，A，B)　若X不为0则返回A，否则返回B。",
            "例如：　IFF(CLOSE>OPEN，HIGH，LOW)　表示该周期收阳则返回最高值，否则返回最低值。",
            "",
            "IFN 逻辑判断",
            "根据条件求不同的值。",
            "用法：　IFN(X，A，B)　若X不为0则返回B，否则返回A。",
            "例如：　IFN(CLOSE>OPEN，HIGH，LOW)　表示该周期收阴则返回最高值，否则返回最低值。",
            "",
            "MAX 较大值",
            "求最大值。",
            "用法：　MAX(A,B)　返回A和B中的较大值。",
            "例如：　MAX(CLOSE-OPEN，0)　表示若收盘价大于开盘价返回它们的差值，否则返回0。",
            "",
            "MIN 较小值",
            "求最小值。",
            "用法：　MIN(A，B)　返回A和B中的较小值。",
            "例如：　MIN(CLOSE，OPEN)　返回开盘价和收盘价中的较小值。",
            "",
            "ACOS 反余弦",
            "反余弦值。",
            "用法：　ACOS(X)　返回X的反余弦值。",
            "",
            "ASIN 反正弦",
            "反正弦值。",
            "用法：　ASIN(X)　返回X的反正弦值。",
            "",
            "ATAN 反正切",
            "反正切值。",
            "用法：　ATAN(X)　返回X的反正切值。",
            "",
            "COS 余弦",
            "余弦值。",
            "用法：　COS(X)　返回X的余弦值。",
            "",
            "SIN 正弦",
            "正弦值。",
            "用法：　SIN(X)　返回X的正弦值。",
            "",
            "TAN 正切",
            "正切值。",
            "用法：　TAN(X)　返回X的正切值。",
            "",
            "EXP 指数",
            "指数。",
            "用法：　EXP(X)　e的X次幂。",
            "例如：　EXP(CLOSE)　返回e的CLOSE次幂。",
            "",
            "LN 自然对数",
            "求自然对数。",
            "用法：　LN(X)　以e为底的对数。",
            "例如：　LN(CLOSE)　求收盘价的对数。",
            "",
            "LOG 对数",
            "求10为底的对数。",
            "用法：　LOG(X)　取得X的对数。",
            "例如：　LOG(100)　等于2。",
            "",
            "SQRT 开方",
            "开平方。",
            "用法：　SQRT(X)　 求X的平方根。",
            "例如：　SQRT(CLOSE)　收盘价的平方根。",
            "",
            "ABS 绝对值",
            "求绝对值。",
            "用法：　ABS(X)　返回X的绝对值。",
            "例如：　ABS(-34)　返回34。",
            "",
            "POW 乘幂",
            "乘幂。",
            "用法：　POW(A，B)　返回A的B次幂。",
            "例如：　POW(CLOSE，3)　求得收盘价的3次方。",
            "",
            "CEILING 向上舍入",
            "向上舍入。",
            "用法：　CEILING(A)　返回沿A数值增大方向最接近的整数。",
            "例如：　CEILING(12.3)　求得13，CEILING(-3.5)求得-3。",
            "",
            "FLOOR 向下舍入",
            "向下舍入。",
            "用法：　FLOOR(A)　返回沿A数值减小方向最接近的整数。",
            "例如：　FLOOR(12.3)　求得12，FLOOR(-3.5)求得-4。",
            "",
            "INTPART 取整",
            "用法：　INTPART(A)　返回沿A绝对值减小方向最接近的整数。",
            "例如：　INTPART(12.3)　求得12，INTPART(-3.5)求得-3。",
            "",
            "BETWEEN： 介于",
            "介于。",
            "用法：　BETWEEN(A，B，C)　表示A处于B和C之间时返回1，否则返回0。",
            "例如：　BETWEEN(CLOSE，MA(CLOSE，10)，MA(CLOSE，5))表示收盘价介于5日均线和10日均线之间。",
            "",
            "AVEDEV 平均绝对方差",
            "AVEDEV(X，N) 　返回平均绝对方差。",
            "",
            "DEVSQ 数据偏差平方和",
            "DEVSQ(X，N) 　返回数据偏差平方和。",
            "",
            "FORCAST 线性回归预测值",
            "FORCAST(X，N)　 返回线性回归预测值。",
            "",
            "SLOPE 线性回归斜率",
            "SLOPE(X，N)　 返回线性回归斜率。",
            "",
            "STD 估算标准差",
            "STD(X，N)　 返回估算标准差。",
            "",
            "STDP 总体标准差",
            "STDP(X，N)　 返回总体标准差。",
            "",
            "VAR 估算样本方差",
            "VAR(X，N)　 返回估算样本方差。",
            "",
            "VARP 总体样本方差",
            "VARP(X，N)　 返回总体样本方差 。",
            "",
            "BLOCKSETNUM 板块股票个数",
            "用法：　BLOCKSETNUM(板块名称)　返回该板块股票个数。",
            "",
            "HORCALC 多股统计",
            "用法：　HORCALC(板块名称，数据项，计算方式，权重)",
            "数据项：100-HIGH，101-OPEN，102-LOW，103-CLOSE，104-VOL，105-涨幅",
            "计算方式：　0-累加，1-排名次",
            "权重：　0-总股本，1-流通股本，2-等同权重，3-流通市值",
            "",
            "COST 成本分布",
            "成本分布情况。",
            "用法：　COST(10)，表示10%获利盘的价格是多少，即有10%的持仓量在该价格以下，其余90%在该价格以上，为套牢盘。",
            "该函数仅对日线分析周期有效。",
            "",
            "PEAK 波峰值",
            "前M个ZIG转向波峰值。",
            "用法：　PEAK(K，N，M)　表示之字转向ZIG(K，N)的前M个波峰的数值，M必须大于等于1。",
            "例如：　PEAK(1,5,1)　表示%5最高价ZIG转向的上一个波峰的数值。",
            "",
            "PEAKBARS 波峰位置",
            "前M个ZIG转向波峰到当前距离。",
            "用法：　PEAKBARS(K，N，M)　表示之字转向ZIG(K，N)的前M个波峰到当前的周期数，M必须大于等于1。",
            "例如：　PEAKBARS (0，5，1)　表示%5开盘价ZIG转向的上一个波峰到当前的周期数。",
            "",
            "SAR 抛物转向",
            "抛物转向。",
            "用法：　 SAR(N，S，M)，N为计算周期，S为步长，M为极值。",
            "例如：　SAR(10，2，20)　表示计算10日抛物转向，步长为2%，极限值为20%。",
            "",
            "SARTURN 抛物转向点",
            "抛物转向点。",
            "用法：　SARTURN(N，S，M)　N为计算周期，S为步长，M为极值，若发生向上转向则返回1，若发生向下转向则返回-1，否则为0。",
            "其用法与SAR函数相同。",
            "",
            "TROUGH 波谷值",
            "前M个ZIG转向波谷值。",
            "用法：　TROUGH(K，N，M)　表示之字转向ZIG(K，N)的前M个波谷的数值，M必须大于等于1。",
            "例如：　TROUGH(2，5，2)　表示%5最低价ZIG转向的前2个波谷的数值。",
            "",
            "TROUGHBARS 波谷位置",
            "前M个ZIG转向波谷到当前距离。",
            "用法：　TROUGHBARS(K，N，M)　表示之字转向ZIG(K，N)的前M个波谷到当前的周期数，M必须大于等于1。",
            "例如：　TROUGH(2，5，2)　表示%5最低价ZIG转向的前2个波谷到当前的周期数。",
            "",
            "WINNER 获利盘比例",
            "获利盘比例。",
            "用法：　WINNER(CLOSE)　表示以当前收市价卖出的获利盘比例。",
            "例如：　返回0.1表示10%获利盘，WINNER(10.5)表示10.5元价格的获利盘比例。",
            "该函数仅对日线分析周期有效。",
            "",
            "LWINNER 近期获利盘比例",
            "近期获利盘比例。",
            "用法：　LWINNER(5，CLOSE)　表示最近5天的那部分成本以当前收市价卖出的获利盘比例。例如返回0.1表示10%获利盘。",
            "",
            "PWINNER 远期获利盘比例",
            "远期获利盘比例。",
            "用法：　PWINNER(5，CLOSE)　表示5天前的那部分成本以当前收市价卖出的获利盘比例。例如返回0.1表示10%获利盘。",
            "",
            "COSTEX 区间成本",
            "区间成本。",
            "用法：　COSTEX(CLOSE，REF(CLOSE))，表示近两日收盘价格间筹码的成本，例如返回10表示区间成本为20元。",
            "该函数仅对日线分析周期有效。",
            "",
            "PPART 远期成本分布比例",
            "远期成本分布比例。",
            "用法：　PPART(10)，表示10前的成本占总成本的比例，0.2表示20%。",
            "",
            "ZIG 之字转向",
            "之字转向。",
            "用法：　ZIG(K，N)　当价格变化量超过N%时转向，K表示0:开盘价，1:最高价，2:最低价，3:收盘价，其余:数组信息",
            "例如：　ZIG(3，5)　表示收盘价的5%的ZIG转向。",
            "",
            "PLOYLINE 折线段",
            "在图形上绘制折线段。",
            "用法：　PLOYLINE(COND，PRICE)，当COND条件满足时，以PRICE位置为顶点画折线连接。",
            "例如：　PLOYLINE(HIGH>=HHV(HIGH，20)，HIGH)表示在创20天新高点之间画折线。",
            "",
            "",
            "DRAWLINE 绘制直线段",
            "在图形上绘制直线段。",
            "用法：　DRAWLINE(COND1，PRICE1，COND2，PRICE2，EXPAND)",
            "当COND1条件满足时，在PRICE1位置画直线起点，当COND2条件满足时，在PRICE2位置画直线终点，EXPAND为延长类型。",
            "例如：　DRAWLINE(HIGH>=HHV(HIGH，20)，HIGH，LOW<=LLV(LOW，20)，LOW，1)　表示在创20天新高与创20天新低之间画直" +
                "线并且向右延长。",
            "",
            "",
            "DRAWKLINE 绘制K线",
            "用法：　DRAWKLINE(HIGH，OPEN，LOW，CLOSE)　以HIGH为最高价，OPEN为开盘价，LOW为最低，CLOSE收盘画K线。",
            "",
            "STICKLINE 绘制柱线",
            "在图形上绘制柱线。",
            "用法：　STICKLINE(COND，PRICE1，PRICE2，WIDTH，EMPTY)，当COND条件满足时，在PRICE1和PRICE2位置之间画柱状线，宽" +
                "度为WIDTH(10为标准间距)，EMPTH不为0则画空心柱。",
            "例如：　STICKLINE(CLOSE>OPEN，CLOSE，OPEN，0.8，1)表示画K线中阳线的空心柱体部分。",
            "",
            "",
            "DRAWICON 绘制图标",
            "在图形上绘制小图标。",
            "用法：　DRAWICON(COND，PRICE，TYPE)，当COND条件满足时，在PRICE位置画TYPE号图标。",
            "例如：　DRAWICON(CLOSE>OPEN，LOW，1)　表示当收阳时在最低价位置画1号图标。图标一共有九个，图形如附图。序号，最下面的是“1”号，最上面的是" +
                "“9”号。",
            "",
            "",
            "DRAWTEXT 显示文字",
            "在图形上显示文字。",
            "用法：　DRAWTEXT(COND，PRICE，TEXT)，当COND条件满足时，在PRICE位置书写文字TEXT。",
            "例如：　DRAWTEXT(CLOSE/OPEN>1.08，LOW，大阳线)表示当日涨幅大于8%时在最低价位置显示\'大阳线\'字样。",
            "",
            "COLOR 自定义色",
            "格式为COLOR+“RRGGBB”：RR、GG、BB表示红蓝色、绿色和蓝色的分量，每种颜色的取值范围是00-FF，采用了16进制。",
            "例如：MA5：MA(CLOSE，5)，COLOR00FFFF　表示纯红色与纯绿色的混合色：COLOR808000表示淡蓝色和淡绿色的混合色。",
            "COLORBLACK   画黑色",
            "",
            "COLORBLUE    画蓝色",
            "",
            "COLORGREEN   画绿色",
            "",
            "COLORCYAN    画青色",
            "",
            "COLORRED     画红色",
            "",
            "COLORMAGENTA 画洋红色",
            "",
            "COLORBROWN   画棕色",
            "",
            "COLORLIGRAY  画淡灰色",
            "COLORGRAY　  画深灰色",
            "COLORLIBLUE  画淡蓝色",
            "COLORLIGREEN 画淡绿色",
            "COLORLICYAN  画淡青色",
            "COLORLIRED   画淡红色",
            "COLORLIMAGENTA 画淡洋红色",
            "COLORYELLOW  画黄色",
            "COLORWHITE   画白色",
            "",
            "LINETHICK    线型粗细",
            "格式：“LINETHICK+(1-9)”　参数的取值范围在1—9之间，“LINETHICK1”表示最细的线，而“LINETHICK9”表示最粗的线。",
            "",
            "STICK      画柱状线",
            "",
            "COLORSTICK 画彩色柱状线",
            "",
            "VOLSTICK   画彩色柱状线",
            "成交量柱状线，当股价上涨时显示红色空心柱，则显示绿色实心柱",
            "",
            "LINESTICK  同时画出柱状线和指标线",
            "",
            "CROSSDOT   画小叉线",
            "",
            "CIRCLEDOT  画小圆圈线",
            "",
            "POINTDOT   画小圆点线",
            "",
            " ",
            "BETWEEN 介于.",
            "用法:BETWEEN(A,B,C)表示A处于B和C之间时返回1，否则返回0",
            "例如:BETWEEN(CLOSE,MA(CLOSE,10),MA(CLOSE,5))表示收盘价介于5日均线和10日均线之间",
            "",
            "EMA  返回指数移动平均",
            "用法:",
            "EMA(X,M):X的M日指数移动平均.算法:Y=(X*2+Y\'*(N-1))/(N+1)",
            "",
            "EXPMA 返回指数移动平均",
            "用法:",
            "EXPMA(X,M):X的M日指数移动平均",
            "",
            "FORCAST  (X,N) 返回线性回归预测值",
            "",
            "TWR  宝塔线",
            "用法：TWR(N,涨颜色，跌颜色)",
            "例如：TWR(CLOSE,COLORRED,COLORGREEN)",
            "",
            "WMA 返回加权移动平均",
            "用法:",
            "WMA(X,M):X的M日加权移动平均.算法:Yn=(1*X1+2*X2+...+n*Xn)/(1+2+...+n)",
            "",
            "XMA 返回偏移移动平均",
            "用法:",
            "XMA(X,M):X的M日偏移移动平均",
            ""});
            this.Notes.Location = new System.Drawing.Point(334, 101);
            this.Notes.Name = "Notes";
            this.Notes.Size = new System.Drawing.Size(146, 112);
            this.Notes.TabIndex = 4;
            this.Notes.Visible = false;
            // 
            // EditFunc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(689, 515);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditFunc";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "公式编辑器";
            this.Load += new System.EventHandler(this.EditFunc_Load);
            this.Shown += new System.EventHandler(this.EditFunc_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditFunc_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ListBox SubList;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TextBox Log;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox drawk;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel xx;
        private System.Windows.Forms.ToolStripStatusLabel yy;
        private System.Windows.Forms.ToolStripStatusLabel sb3;
        private System.Windows.Forms.ToolStripStatusLabel sb4;
        private System.Windows.Forms.ListBox listBox2;
        private Splitter splitter1;
        public TextBox Gongsi;
        private Splitter splitter2;
        public TextBox name;
        public TextBox title;
        private Button input;
        private Button color;
        private ColorDialog cd1;
        private ToolStripStatusLabel sb5;
        private ToolStripStatusLabel sb6;
        private Label label3;
        private TextBox password;
        private ListBox Notes;


    }

}