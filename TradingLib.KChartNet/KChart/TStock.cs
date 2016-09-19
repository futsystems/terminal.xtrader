using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using Common.Logging;
using TradingLib.MarketData;

namespace CStock
{
    public partial class TStock : UserControl
    {
        ILog logger = LogManager.GetLogger("KChart");

        

        void D(string msg)
        {
            logger.Info(msg);
        }

        double NA = double.MinValue;// -10000000.0;
        public enum TCursorType {
            ctNone, 
            ctZoom,//��K�������� ѡ���� �Ŵ���С
            ctMove,//�϶�K��
            ctDrawLine, //�Ի���״̬
            ctInLine,//������Ի�����
            FsSize,//����ڷ�ʱ�ļ������
            KSize,//�����K�ߵļ������
            BSize,
            InDiLei//�������Ϣ������
        };



        string[] WeekStr = new string[] { "��", "һ", "��", "��", "��", "��", "��" };
        Color volc = Color.FromArgb(192, 192, 0);
        Color down = Color.FromArgb(0, 230, 0);
        Color highV = Color.FromArgb(192, 0, 192);


        XLine FSelectLine=null;
        int FSelectWhere;

        XLine FLine = new XLine(0); // ���ڻ�����
        int FLineCount; // ���ڻ��ڼ�����
        TCursorType FCursorType=TCursorType.ctNone; // ���״̬
        /// <summary>
        /// ���ڼ�¼��ǰλ�ð�����ͼ������ܸ߶�
        /// �������ͼ���� ��ֵΪ0
        /// ����ٵڶ�ָ������ ��ֵΪ��ͼ����ĸ߶� �����ø�ֵ�ж��Ƿ�����ͼ����
        /// </summary>
        int tophigh;


        
        //ѵ������
        Boolean Training = false;//ѵ��ģʽ 

        int LineHeight = 15;//����и�
        //������ʾ����
        Label[] HL = new Label[22];
        Point oriPoint;//����ԭ��λ�� �϶���������
        MDSymbol FCurStock = null;


        Label[] debugLabels = new Label[10];

        //���½Ǵ���
        //string[] ws = { "��", "��", "ָ", "��", "ֵ" };
        string[] wfc ={
                             "MA","VOL","MACD","FSL","DMI","DMA","MIKE","BRAR",
                             "CR","OBV","ASI","EMV","RSI","WR","MA","KDJ","CCI",
                             "ROC","MTM","BOLL","PSY","BIAS","PBX","EXPMA",
                             "WVAD","VR","FS","VOL","BDT","TWR","BTX","DB6","TRIX"
                          };
        List<string> TabList = new List<string>();
        int TabType = 0;
        float[] TabWidth = null;

        //public int myTabIndex = 0;
        public int TabValue
        {
            get { 
                //return myTabIndex; 
                return ctDetailsBoard1.TabValue;
            }
        }

        //Panel[] PBox = new Panel[5];
        //public delegate void TabClickHandle(object sender, int Index);
        
        //bool focus=false;
        /// <summary>
        /// ˢ��ָ����Tab����0~4
        /// </summary>
        /// <param name="Index"></param>
        //public void TabPaint(int Index)
        //{
        //    //if ((Index > -1) && (Index < PBox.Length))
        //    //{
        //    //    PBox[Index].Invalidate();
        //    //}
        //    ctDetailsBoard1.TabPaint(Index);
        //}

        bool focus = false;
        //��--�б�
        //public List<Tick> FenBiList=new List<Tick>();
        /// <summary>
        /// ��--�߶���ʾ����
        /// ���������˲�ѯ�۱ʳɽ�����
        /// </summary>
        public int TabHigh
        {
            get
            {
                return ctDetailsBoard1.TradesRowCount;
                //return (pbox1.Height - 2) / LineHeight;
            }
        }




        //ָ--ָ��
        TGongSi zs_k; 
        TGongSi zs_vol;
        public void zs_SetName(string name)
        {
            zs_k.StkName = name;
            //pbox3.Invalidate();
        }
        public void zs_Clear()
        {
            zs_k.cleardata();
            zs_vol.cleardata();
            //pbox3.Invalidate();
        }

        public void zs_AddAll(string name, double[] f1, int len, Boolean repaint)
        {
            zs_k.Add(name, f1, len);
            if (repaint)
            {
                zs_k.run();
                zs_vol.run();
                //pbox3.Invalidate();
            }
        }


        //ֵ---���»��߷ֿ�
        List<string> zhilist = new List<string>();
        public void AddZhi(string key, string value)
        {
            zhilist.Add(key + "_" + value);
            //pbox5.Invalidate();
        }


        bool _IsIntraView = false;
        Boolean FShowLeft = true, FShowTop = true, FShowRight = true, FShowBottom = true;

        public List<infolist> DL = null;//��Ϣ����
        Int32 DiLei = -1;
        //����ί��
        public delegate void DiLeiClickHandle(object sender, int DiLeiNo);
        //�����¼�
        public event DiLeiClickHandle DiLeiClicked;


        Point PressXY, NowXY;
        int curx = -1, cury = -1;

        //TGongSi FS_K, FS_VOL;
        //int FS_KH, FS_VOLH;


        int fswindows = 2;
        public TGongSi[] FSGS = new TGongSi[10];
        int[] FSGSH = new int[10];


        int techwindows = 3;
        public TGongSi[] GS = new TGongSi[10];
        int[] GSH = new int[10];

        public TGongSi curgs = null;
        private float[] Fi = new float[35];

        public void SetDiLei(List<infolist> value)
        {
            FSGS[0].DL = value;
            GS[0].DL = value;
            DL = value;
            Invalidate();
        }

        /// <summary>
        /// �Ƿ��Ѿ��ӷ������ϻ�ȡ��������
        /// </summary>
        bool noMoreData = false;

        /// <summary>
        /// 
        /// </summary>
        public bool NoMoreBarDate
        {
            get { return noMoreData; }
            set 
            {
                noMoreData = value;
            }
        }
        
        


        public Boolean Train
        {
            get { return Training; }
            set
            {
                Training = value;
                for (int i = 0; i < 10; i++)
                {
                    GS[i].training = value;
                    GS[i].TrainEnd = GS[0].RecordCount;
                }
                this.Invalidate();
            }
        }

        public int TrainEnd
        {
            get { return GS[0].TrainEnd; }
            set
            {
                for (int i = 0; i < 10; i++)
                    GS[i].TrainEnd = value;
                this.Invalidate();
            }
        }

       
        public bool ShowCurWindow
        {
            get { return GS[0].ShowCurWindow; }
            set
            {
                for (int i = 0; i < 10; i++)
                {
                    FSGS[i].ShowCurWindow = value;
                    GS[i].ShowCurWindow = value;
                }
                Invalidate();
            }
        }


        


        private void Debug()
        {
            debugLabels[0].Text = string.Format("FCursorType:{0}", FCursorType);
            debugLabels[1].Text = string.Format("Press Point    ({0},{1})", PressXY.X, PressXY.Y);
            debugLabels[2].Text = string.Format("Now Point      ({0},{1})", NowXY.X, NowXY.Y);
            debugLabels[3].Text = string.Format("Current Point  ({0},{1})", curx, cury);
            debugLabels[3].Text = string.Format("tophigh:{0}", tophigh);
            debugLabels[4].Text = string.Format("Left:{0} Right:{1} Curr:{2}", this.StartIndex, 0, CurBar);

        }

        #region ��ʾ���Բ���

        

        /// <summary>
        /// ��õ�ǰBarλ��
        /// </summary>
        public int CurBar
        {
            get
            {
                if (this.IsIntraView)
                    return FSGS[0].FCurBar;
                if (this.IsBarView)
                    return GS[0].FCurBar;
                return 0;
            }
        }


        public double PreClose
        {
            get { return FSGS[0].PreClose; }
            set
            {
                FSGS[0].PreClose = value;
                GS[0].PreClose = value;
                this.Invalidate();
            }
        }


        

        /// <summary>
        /// ����֤ȯ����
        /// </summary>
        public String StkName
        {
            get { return GS[0].StkName; }
            set
            {
                FSGS[0].StkName = value;
                GS[0].StkName = value;
                ctDetailsBoard1.StockLabel = StkCode + " " + value;
                //StkLabel.Text = StkCode + " " + value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// ����֤ȯ����
        /// </summary>
        public string StkCode
        {
            get { return GS[0].StkCode; }
            set
            {
                FSGS[0].StkCode = value;
                GS[0].StkCode = value;
                ctDetailsBoard1.StockLabel = value + " " + StkName;
                //StkLabel.Text = value + " " + StkName;
                this.Invalidate();
            }
        }

        public string StkWeek
        {
            get { return GS[0].StkWeek; }
            set
            {
                FSGS[0].StkWeek = value;
                GS[0].StkWeek = value;
                this.Invalidate();
            }
        }




        

        public bool ShowDebug
        {
            set
            {
                if (value)
                {
                    if (!debugBox.Visible)
                        debugBox.Visible = true;
                }
                else
                {
                    if (debugBox.Visible)
                        debugBox.Visible = false;
                }
            }
        }
       

        
        bool Ftab = true;
        




        

        #endregion

        string[] dx = new string[]{ "һ", "��", "��", "��", "��", "��", "��", "��", "��", "ʮ" };
        public TStock()
        {

            SetStyle(ControlStyles.DoubleBuffer|ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            
            int i, j;

            string[] ws ={ "����", "����", "����", "5������", "15������", "30������", "60������", "120������", "�ֱ���", "1������" };
            

            string[] fq ={ "����Ȩ", "ǰ��Ȩ", "��Ȩ" };
            string[] sw = {
                       "ָ���÷�ע��", "����ָ�����", "�޸�ָ�깫ʽ","�ָ�Ĭ��ָ��", "����ָ�깫ʽ", "-",
                       "����ָ������","-",
                       "����ͼ��","-","��Ȩ����","-",
                       "��������", "����ָ��", "���ڸ���","���շ�ʱͼ", "-", 
                       "��ϵ��ʽ"
                   };


            //��ʾ����
            string[] fs = { "��ʾ��Ϣ", "��ʾ���", "��ʱ�л�", "��ʾָ����", "��ʱȫ��", "��ʱȫ��", "��ʾ����", "��ʾ�ױ�", "��ʾ���", "��ʾ�ұ�" };
            InitializeComponent();
            this.SetStyle(ControlStyles.Selectable|ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);

            this.BackColor = Color.Black;
            //this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.TStock_MouseWheel);
            this.DoubleBuffered = true;

            //FS_KH = -1;
            //FS_VOLH = -1;
            for (i = 0; i < 10; i++)
            {
                FSGSH[i] = -1;
                GSH[i] = -1;
            }
            DataHint.BackColor = Color.FromArgb(177, 177, 177);
            for (i = 0; i < 22; i++)
            {
                HL[i] = new Label();
                HL[i].AutoSize = false;
                HL[i].SetBounds(0, i * LineHeight, phint.Width, LineHeight);
                HL[i].ForeColor = Color.FromArgb(177, 177, 177);
                HL[i].Name = "HLK" + i.ToString();
                if ((i % 2) == 0)
                    HL[i].TextAlign = ContentAlignment.MiddleLeft;
                else
                    HL[i].TextAlign = ContentAlignment.MiddleRight;
                if (i > 0)
                {
                    HL[i].MouseDown += new MouseEventHandler(phint_MouseDown);
                    HL[i].MouseUp += new MouseEventHandler(phint_MouseUp);
                    HL[i].MouseMove += new MouseEventHandler(phint_MouseMove);
                    HL[i].Cursor = Cursors.SizeAll;
                }
                phint.Controls.Add(HL[i]);
            }
            HL[0].MouseClick += new MouseEventHandler(HintClose_Click);


            for (i = 0; i < 10; i++)
            {
                debugLabels[i] = new Label();
                debugLabels[i] = new Label();
                debugLabels[i].AutoSize = false;
                debugLabels[i].SetBounds(0, i * LineHeight, debugBox.Width, LineHeight);
                //debugLabels[i].ForeColor = Color.FromArgb(177, 177, 177);
                debugLabels[i].Name = "Debug-" + i.ToString();
                debugLabels[i].Text = i.ToString();
                debugBox.Controls.Add(debugLabels[i]);
            }
            debugBox.Height = 10 * LineHeight + 2;

            ctDetailsBoard1.TabSwitch += new Action<object, TabSwitchEventArgs>(ctDetailsBoard1_TabSwitch);
            ctDetailsBoard1.TabDoubleClick += new Action<object, TabDoubleClickEventArgs>(ctDetailsBoard1_TabDoubleClick);
            //StockTab.BackColor = Color.Black;
            //PBox[0] = pbox1;
            //PBox[1] = pbox2;
            //PBox[2] = pbox3;
            //PBox[3] = pbox4;
            //PBox[4] = pbox5;
            //for (i = 0; i < 5; i++)
            //{
            //    PBox[i].BackColor = Color.Black;
            //    PBox[i].Dock = DockStyle.Fill;
            //}

            PressXY.X = -1;
            PressXY.Y = -1;
            NowXY = PressXY;
            Rectangle r1 = new Rectangle(0, 0, this.Width, this.Height);
            zs_k = new TGongSi(this);
            zs_k.Main = true;
            zs_k.ShowTop = true;
            zs_k.ShowLeft = false;
            zs_k.ShowBottom = false;
            zs_k.ShowRight = true;
            zs_k.ShowFs = true;
            zs_k.LoadWfc("fs");
            zs_k.StkName = "��ָ֤��-399001";
            zs_k.ShowCursor = true;
            zs_k.CurWindow = false;

            zs_vol = new TGongSi(zs_k,this);
            zs_vol.CurWindow = false;
            zs_vol.Main = false;
            zs_vol.ShowFs = true;

            zs_vol.ShowTop = false;
            zs_vol.ShowLeft = false;
            zs_vol.ShowRight = true;
            zs_vol.ShowBottom = false;

            zs_vol.LoadWfc("vol");
            zs_vol.ShowCursor = true;

            #region ��ʼ����ʱ��ʽ
            FSGS[0] = new TGongSi(this);
            FSGS[0].Main = true;
            FSGS[0].ShowBottom = false;
            FSGS[0].ShowRight = true;
            FSGS[0].ShowFs = true;
            FSGS[0].LoadWfc("fs");
            FSGS[0].ShowCursor = true;
            FSGS[0].CurWindow = true;
            for (i = 1; i < 10; i++)
            {
                FSGS[i] = new TGongSi(FSGS[0],this);
                FSGS[i].CurWindow = false;
                FSGS[i].Main = false;
                FSGS[i].ShowFs = true;
                FSGS[0].CurWindow =false;
                FSGS[i].ShowTop = false;
                FSGS[i].ShowRight = true;
                FSGS[i].ShowBottom = false;
                FSGS[i].LoadWfc(wfc[i].ToLower());
                FSGS[i].ShowCursor = true;
            }

            FSGS[fswindows - 1].ShowBottom = true;
            #endregion


            #region ��ʼ��K�߹�ʽ
            GS[0] = new TGongSi(this);
            GS[0].Main = true;
            GS[0].CurWindow = false;
            GS[0].LoadWfc("ma");
            GS[0].ShowCursor = true;
            GS[0].ShowBottom = false;
            GS[0].CurWindow = true;
            for (i = 1; i < 10; i++)
            {
                GS[i] = new TGongSi(GS[0],this);
                GS[i].Main = false;
                GS[i].CurWindow = false;
                GS[i].ShowBottom = false;
                GS[i].ShowCursor = true;
                GS[i].LoadWfc(wfc[i].ToLower());
                GSH[i] = -1;
            }
            GS[techwindows - 1].ShowBottom = true;

            #endregion

            FuncStr FS = new FuncStr();
            TabList.Clear();
            TabList.Add("ָ��");
            TabList.Add("ȫ��");
            TabList.Add("�߼�");
            for (i = 0; i < FS.Count(); i++)
            {
                if (FS.functype[i] == 1)
                {
                    TabList.Add(FS.funcname[i]);
                }
            }
            TabWidth = new float[TabList.Count];


            #region ��ʼ���˵�
            StockMenu.Items.Clear();
            //for (j = 0; j < sw.Length; j++)
            //{
            //    if (sw[j] == "-")
            //    {
            //        ToolStripSeparator m2 = new System.Windows.Forms.ToolStripSeparator();
            //        StockMenu.Items.Add(m2);
            //        continue;
            //    }
            //    ToolStripMenuItem m1 = new ToolStripMenuItem(sw[j]);
            //    m1.Click += StockMenu_Click;
            //    StockMenu.Items.Add(m1);
            //    if (sw[j] == "����ָ��")
            //    {
            //        m1.Click -= StockMenu_Click;

            //        ToolStripMenuItem m2 = new ToolStripMenuItem("���ؿ⹫ʽ");
            //        m1.DropDownItems.Add(m2);
            //        m2.Click += StockMenu_Click;
            //        ToolStripSeparator m21 = new System.Windows.Forms.ToolStripSeparator();
            //        m1.DropDownItems.Add(m21);
            //        /*
            //        for (i = 0; i < wfc.Length; i++)
            //        {
            //            ToolStripMenuItem m22 = new ToolStripMenuItem(wfc[i]);
            //            m22.Tag = 0x8000 + i;
            //            m1.DropDownItems.Add(m22);
            //            m22.Click += StockMenu_Click;
            //        }
            //        */
            //        FuncStr FS = new FuncStr();
            //        TabList.Clear();
            //        TabList.Add("ָ��");
            //        TabList.Add("ȫ��");
            //        TabList.Add("�߼�");
            //        for (i = 0; i < FS.Count(); i++)
            //        {
            //            if (FS.functype[i] == 1)
            //            {
            //                TabList.Add(FS.funcname[i]);
            //                m2 = new ToolStripMenuItem(FS.funcname[i].ToUpper());
            //                m2.Tag = 0x8000 + i;
            //                m1.DropDownItems.Add(m2);
            //                m2.Click += StockMenu_Click;
            //            }
            //        }
            //        TabWidth = new float[TabList.Count];

            //    }
            //    //if (sw[j] == "��Ȩ����")
            //    //{
            //    //    m1.Click -= StockMenu_Click;
            //    //    for (i = 0; i < fq.Length; i++)
            //    //    {
            //    //        ToolStripMenuItem m2 = new ToolStripMenuItem(fq[i]);
            //    //        m1.DropDownItems.Add(m2);
            //    //        m2.Click += StockMenu_Click;
            //    //    }
            //    //}

            //    //if (sw[j] == "���ڸ���")
            //    //{
            //    //    m1.Click -= StockMenu_Click;
            //    //    for (i = 0; i < 10; i++)
            //    //    {
            //    //        ToolStripMenuItem m2 = new ToolStripMenuItem(dx[i] + "������");
            //    //        m2.Tag = 0x2000 + i + 1;
            //    //        m1.DropDownItems.Add(m2);
            //    //        m2.Click += StockMenu_Click;
            //    //    }
            //    //}



            //    if (sw[j] == "��������")
            //    {
            //        m1.Click -= StockMenu_Click;
            //        for (int k = 0; k < fs.Length; k++)
            //        {
            //            ToolStripMenuItem m2 = new ToolStripMenuItem(fs[k]);
            //            m2.Tag = 0x3000 + k;
            //            m1.DropDownItems.Add(m2);
            //            m2.Click += StockMenu_Click;
            //        }
            //    }
            //    if (sw[j] == "���շ�ʱͼ")
            //    {
            //        m1.Click -= StockMenu_Click;
            //        for (i = 0; i < 10; i++)
            //        {
            //            string s1="���շ�ʱͼ";
            //            if (i>0)
            //                s1=String.Format("���{0}��",i+1);
            //            ToolStripMenuItem m2 = new ToolStripMenuItem(s1);
            //            m2.Tag = 0x5000 + i ;
            //            m1.DropDownItems.Add(m2);
            //            m2.Click += StockMenu_Click;
            //            if (i == 0)
            //            {
            //                ToolStripSeparator m21 = new System.Windows.Forms.ToolStripSeparator();
            //                m1.DropDownItems.Add(m21);
            //            }
            //        }
            //    }

            //}
            #endregion

        }

        void ctDetailsBoard1_TabDoubleClick(object arg1, TabDoubleClickEventArgs arg2)
        {
            if (TabDoubleClick != null)
            {
                TabDoubleClick(arg1, arg2);
            }
        }


        /// <summary>
        /// ��DetailsBoard�еײ�Tab�л��¼����ⲿ��¶ �����л�״̬ʱ �����˲�ѯ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="Index"></param>
        void ctDetailsBoard1_TabSwitch(object sender, TabSwitchEventArgs arg)
        {
            if (TabSwitch != null)
            {
                TabSwitch(this, arg);
            }
        }




        #region ��ؼ������,����,�������

        /// <summary>
        /// �趨��ǰ��������
        /// </summary>
        /// <param name="symbol"></param>
        public void SetStock(MDSymbol symbol)
        {
            if (symbol == null)
                return;

            
            //if (BuyValue[0] == null)
            //    return;
            FCurStock = symbol;
            FSGS[0].Symbol = symbol;
            GS[0].Symbol = symbol;



            ctDetailsBoard1.SetStock(symbol);

            this.PreClose = symbol.PreClose;
            //FSGS[0].PreClose = symbol.PreClose;// symbol.now.last;
            //this.PreClose = symbol.Precision;
            //���س�Ȩ����
            this.SetQuan(symbol.PowerData);

            this.BarWidth = 8;
            this.StartFix = false;
            this.Invalidate();
        }

        /// <summary>
        /// ���һ��Bar����
        /// </summary>
        public int LastDate
        {
            get
            {
                int dataLength = GS[0].RecordCount;
                TBian data = GS[0].check("date");
                if (data != null)
                {
                    return (int)data.value[dataLength - 1];
                }
                return -1;
            }
        }

        /// <summary>
        /// ���һ��Barʱ��
        /// </summary>
        public int LastTime
        {
            get
            {
                int dataLength = GS[0].RecordCount;
                TBian data = GS[0].check("time");
                if (data != null)
                {
                    return (int)data.value[dataLength - 1];
                }
                return -1;
            }
        }

        
        

        

        

        /// <summary>
        /// ���»���
        /// </summary>
        public void ReDraw(bool forward=false)
        {

            this.Invalidate();
        }

        

       
        #endregion











        #region TSotck�ؼ�����¼��뺯����д

        void ReSizeBarChart(bool calcInd)
        {
            int h1 = Height;
            if (Ftab)
                h1 -= Tab.Height;

            int hh = (h1 / (techwindows + 1));
            GSH[0] = hh * 2;
            for (int i = 1; i < techwindows; i++)
                GSH[i] = hh;
            if (techwindows > 1)
                GSH[techwindows - 1] = h1 - hh * techwindows;
            if (Ftab)
                Tab.Invalidate();
            this.ReCalculate("Resize", calcInd);
            this.Invalidate();
        }


        private void TStock_Resize(object sender, EventArgs e)
        {
            int h1 = Height;
            int hh0 = (h1 / (fswindows + 1));
            FSGSH[0] = hh0 * 2;
            for (int i = 1; i < fswindows; i++)
                FSGSH[i] = hh0;
            if (fswindows > 1)
                FSGSH[fswindows - 1] = h1 - hh0 * fswindows;
            ReSizeBarChart(false);
        }




        private void TStock_Click(object sender, EventArgs e)
        {
            if (FCursorType == TCursorType.InDiLei)
            {
                if (DiLeiClicked != null)
                {
                    DiLeiClicked(this, DiLei);
                    return;
                }
            }
            Focus();
            //��굥�� ������Ϣ����
            //DataHint.Visible ^= true;

        }

        /// <summary>
        /// ����� Enter��ؼ� ��󻯲����ᵼ��Leave�ؼ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TStock_Enter(object sender, EventArgs e)
        {
            focus = true;
            this.Invalidate();
        }


        private void TStock_Leave(object sender, EventArgs e)
        {
            focus = false;
            this.Invalidate();
        }

        /// <summary>
        /// �ؼ���һ�μ���ʱ�� ִ�г�ʼ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TStock_Load(object sender, EventArgs e)
        {


        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            Invalidate(false);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            Invalidate(false);
        }

        #endregion

        public override bool Focused
        {
            get
            {
                return true;
            }
        }






        private void StockTab_Resize(object sender, EventArgs e)
        {
            //TabBox.Invalidate();
        }



       

        /// <summary>
        /// �Ҳ��̿���ϸ�����С�ı���Ҫ���»��Ƶ�ǰ�ؼ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Board_Resize(object sender, EventArgs e)
        {
            this.Invalidate();
            //TabBox.Invalidate();
        }

       

        //private void pbox3_Resize(object sender, EventArgs e)
        //{
        //    pbox3.Invalidate();
        //}

        //private void pbox3_Paint(object sender, PaintEventArgs e)
        //{
        //    Graphics cv = e.Graphics;
        //    Rectangle r1 = pbox3.ClientRectangle;
        //    cv.FillRectangle(Brushes.Black, r1);
        //    int kh = pbox3.Height * 2 / 3;
        //    if (kh < 30)
        //        return;
        //    if (zs_k.RecordCount== 0)
        //        return;
        //    Bitmap bm1 = new Bitmap(pbox3.Width, kh);
        //    Bitmap bm2 = new Bitmap(pbox3.Width, pbox3.Height-kh);
            
        //    zs_k.SetCurxy(-1, -1);
        //    zs_k.drawline(bm1);
        //    zs_vol.SetCurxy(-1, -1);
        //    zs_vol.drawline(bm2);
        //    cv.DrawImage(bm1, 0, 0);
        //    cv.DrawImage(bm2, 0,kh);
        //    bm1.Dispose();
        //    bm2.Dispose();
        //}

        //private void pbox4_Resize(object sender, EventArgs e)
        //{
        //    pbox4.Invalidate();
        //}

        //private void pbox4_Paint(object sender, PaintEventArgs e)
        //{
        //    Graphics cv = e.Graphics;
        //    Rectangle r1 = pbox4.ClientRectangle;
        //    Brush bb = new SolidBrush(pbox4.BackColor);
        //    cv.FillRectangle(bb, r1);
        //    bb.Dispose();
        //}

        //private void pbox5_Resize(object sender, EventArgs e)
        //{
        //    pbox5.Invalidate();
        //}

        //private void pbox5_Paint(object sender, PaintEventArgs e)
        //{
        //    Graphics cv = e.Graphics;
        //    Rectangle r1 = pbox5.ClientRectangle;
        //    Brush bb = new SolidBrush(pbox5.BackColor);
        //    cv.FillRectangle(bb, r1);
        //    bb.Dispose();

        //    if (zhilist.Count == 0)
        //        return;
        //    int i=0;
        //    int h=(pbox5.Height-2)/LineHeight;
        //    if (zhilist.Count>h)
        //        i=zhilist.Count-h;
        //    string keys,values;
        //    for (int j = i; j < zhilist.Count; j++)
        //    {
        //        keys = zhilist[j];
        //        values="";
        //        int k = keys.IndexOf('_');
        //        if (k > 0)
        //        {
        //            values = keys.Substring(k + 1);
        //            keys = keys.Substring(0, k);
        //        }
        //        cv.DrawString(keys, Font, Brushes.Gray, r1.Left + 2, r1.Top + (j - i) * LineHeight + 2);
        //        SizeF si = cv.MeasureString(values, Font);
        //        cv.DrawString(values, Font, Brushes.Gray,pbox5.Width-si.Width-2 , r1.Top + (j - i) * LineHeight + 2);
        //    }
        //}

      



        public bool SetQuanStyle(QuanType value)
        {
            bool b1 = GS[0].SetQuanStyle(value);
            if (b1)
            {
                for (int i = 0; i < techwindows; i++)
                    GS[i].run();
                Invalidate();
            }
            return b1;
        }

        //�ǵ�ͣ����
        public Boolean PR10
        {
            get { return FSGS[0].percent10; }
            set
            {
                FSGS[0].percent10 = value;
                Invalidate();
            }

        }



        /// <summary>
        /// ���ȨϢ��Ϣ
        /// </summary>
        void ClearQuan()
        {
            GS[0].QuanInfo.Clear();
        }
        /// <summary>
        /// ����ȨϢ��Ϣ
        /// </summary>
        /// <param name="value"></param>
        void SetQuan(PowerData value)
        {
            if (value.QuanLen > 0)
            {
                for (int i = 0; i < value.QuanLen; i++)
                {
                    GS[0].QuanInfo.Add(value.quan[i]);
                }
                this.Invalidate();
            }
        }
        /// <summary>
        /// ���ø�Ȩ����
        /// </summary>
        [Description("��Ȩ����")]
        public QuanType Quan
        {
            get { return GS[0].QuanStyle; }

            set
            {
                GS[0].SetQuanStyle(value);
                Invalidate();
            }

        }


        public void BeginUpdate()
        {
            this.SuspendLayout();
            // Do paint events
            EndUpdate();
        }

        public void EndUpdate()
        {
            this.ResumeLayout();
            // Raise an event if needed.
        }


        private void TStock_DoubleClick(object sender, EventArgs e)
        {
            this.ShowCrossCursor ^= true;
        }

        

    }


}