using System;
using System.Collections.Generic;
using System.Text;
namespace CStock
{
    public class FuncStr
    {
        public List<string> funcname = new List<string>();
        List<string> funcstr = new List<string>();
        public List<int> functype = new List<int>();

        public int Count()
        {
            return funcname.Count;
        }
        public bool Add(string name, string str,int type)
        {
            name = name.ToLower();
            if (funcname.IndexOf(name) > -1)
                return false;
            funcname.Add(name);
            funcstr.Add(str);
            functype.Add(type);
            return true;
        }

        public bool Add(string name, string str)
        {
            name = name.ToLower();
            if (funcname.IndexOf(name) > -1)
                return false;
            funcname.Add(name);
            funcstr.Add(str);
            functype.Add(1);
            return true;
        }
        public string Get(string name)
        {
            name = name.ToLower();
            int i = funcname.IndexOf(name);
            string s1 = "";
            if (i > -1)
                s1 = funcstr[i];
            return s1;
        }
        public FuncStr()
        {
            string s1;


            s1 = "TECHNAME:=MACD;\r\n";
            s1 = s1 + "TECHTITLE:=平滑异同平均;\r\n";
            s1 = s1 + "DRAWK:=FALSE;\r\n";
            s1 = s1 + "SHORT:=INPUT(2,200,12,12,\"Param日快线移动平均\");\r\n";
            s1 = s1 + "LONG:=INPUT(2,200,26,26,\"Param日慢线移动平均\");\r\n";
            s1 = s1 + "MID:=INPUT(2,200,9,9,\"Param日移动平均\");\r\n";
            s1 = s1 + "DIF:EMA(CLOSE,SHORT)-EMA(CLOSE,LONG);\r\n";
            s1 = s1 + "DEA:EMA(DIF,MID);\r\n";
            s1 = s1 + "MACD:(DIF-DEA)*2,COLORSTICK;\r\n";
            s1 = s1 + "0,LINEDOT,COLORWHITE;\r\n";
            s1 = s1 + "*DIFF线　收盘价短期、长期指数平滑移动平均线间的差\r\n";
            s1 = s1 + "*DEA线　 DIFF线的M日指数平滑移动平均线\r\n";
            s1 = s1 + "*MACD线　DIFF线与DEA线的差，彩色柱状线\r\n";
            s1 = s1 + "*参数：SHORT(短期)、LONG(长期)、M 天数，一般为12、26、9\r\n";
            s1 = s1 + "*用法：\r\n";
            s1 = s1 + "*1.DIFF、DEA均为正，DIFF向上突破DEA，买入信号。\r\n";
            s1 = s1 + "*2.DIFF、DEA均为负，DIFF向下跌破DEA，卖出信号。\r\n";
            s1 = s1 + "*3.DEA线与K线发生背离，行情反转信号。\r\n";
            s1 = s1 + "*4.分析MACD柱状线，由红变绿(正变负)，卖出信号；由绿变红，买入信号\r\n";
            Add("MACD", s1);
            s1 = "TECHNAME:=DMI;\r\n";
            s1 = s1 + "TECHTITLE:=趋向指标\r\n";
            s1 = s1 + "DRAWK:=FALSE;\r\n";
            s1 = s1 + "N:=INPUT(2,90,14,14);\r\n";
            s1 = s1 + "MM:=INPUT(2,60,6,6);\r\n";
            s1 = s1 + "MTR:=EXPMEMA(MAX(MAX(HIGH-LOW,ABS(HIGH-REF(CLOSE,1))),ABS(REF(CLOSE,1)-LOW)),N);\r\n";
            s1 = s1 + "HD :=HIGH-REF(HIGH,1);\r\n";
            s1 = s1 + "LD :=REF(LOW,1)-LOW;\r\n";
            s1 = s1 + "DMP:=EXPMEMA(IF(HD>0&HD>LD,HD,0),N);\r\n";
            s1 = s1 + "DMM:=EXPMEMA(IF(LD>0&LD>HD,LD,0),N);\r\n";
            s1 = s1 + "PDI: DMP*100/MTR;\r\n";
            s1 = s1 + "MDI: DMM*100/MTR;\r\n";
            s1 = s1 + "ADX: EXPMEMA(ABS(MDI-PDI)/(MDI+PDI)*100,MM);\r\n";
            s1 = s1 + "ADXR:EXPMEMA(ADX,MM);\r\n";
            s1 = s1 + "*用法：市场行情趋向明显时，指标效果理想。\r\n";
            s1 = s1 + "*PDI(上升方向线)    MDI(下降方向线)  ADX(趋向平均值)\r\n";
            s1 = s1 + "*1.PDI线从下向上突破MDI线，显示有新多头进场，为买进信号；\r\n";
            s1 = s1 + "*2.PDI线从上向下跌破MDI线，显示有新空头进场，为卖出信号；\r\n";
            s1 = s1 + "*3.ADX值持续高于前一日时，市场行情将维持原趋势；\r\n";
            s1 = s1 + "*4.ADX值递减，降到20以下，且横向行进时，市场气氛为盘整；\r\n";
            s1 = s1 + "*5.ADX值从上升倾向转为下降时，表明行情即将反转。\r\n";
            s1 = s1 + "*参数：N　统计天数； M  间隔天数，一般为14、6\r\n";
            Add("DMI", s1);
            s1 = "TECHNAME:=DMA;\r\n";
            s1 = s1 + "TECHTITLE:=平均差\r\n";
            s1 = s1 + "DRAWK:=FALSE;\r\n";
            s1 = s1 + "N1:=INPUT(2,60,10,10);\r\n";
            s1 = s1 + "N2:=INPUT(2,250,50,50);\r\n";
            s1 = s1 + "M:=INPUT(2,100,10,10);\r\n";
            s1 = s1 + "DIF:MA(CLOSE,N1)-MA(CLOSE,N2);\r\n";
            s1 = s1 + "AMA:MA(DIF,M);\r\n";
            s1 = s1 + "*1.DMA 向上交叉其平均线时，买进；\r\n";
            s1 = s1 + "*2.DMA 向下交叉其平均线时，卖出；\r\n";
            s1 = s1 + "*3.DMA 的交叉信号比MACD、TRIX 略快；\r\n";
            s1 = s1 + "*4.DMA 与股价产生背离时的交叉信号，可信度较高；\r\n";
            s1 = s1 + "*5.DMA、MACD、TRIX 三者构成一组指标群，互相验证。\r\n";
            Add("DMA", s1);



            s1 = "TECHNAME:=FSL\r\n";
            s1 = s1 + "TECHTITLE:=分水线\r\n";
            s1 = s1 + "N1:=input(1,100,3,3)\r\n";
            s1 = s1 + "N2:=input(1,100,10,10)\r\n";
            s1 = s1 + "M1:=input(1,100,5,5)\r\n";
            s1 = s1 + "M2:=input(1,100,20,20)\r\n";
            s1 = s1 + "LC:= REF(CLOSE,1); \r\n";
            s1 = s1 + "RSI1:=SMA(MAX(CLOSE-LC,0),N1,1)/SMA(ABS(CLOSE-LC),N1,1)*100; \r\n";
            s1 = s1 + "RSI2:=SMA(MAX(CLOSE-LC,0),N2,1)/SMA(ABS(CLOSE-LC),N2,1)*100; \r\n";
            s1 = s1 + "买:EMA(RSI1,M1),COLORRED,LINETHICK2; \r\n";
            s1 = s1 + "卖:EMA(RSI2,M2),LINETHICK2,COLORFFFF00;\r\n";
            s1 = s1 + "DRAWK:=FALSE\r\n";
            s1 = s1 + "VAR1:=REF(CLOSE,1); \r\n";
            s1 = s1 + "VAR2:SMA(MAX(CLOSE-VAR1,0),6,1)/SMA(ABS(CLOSE-VAR1),6,1)*100; \r\n";
            s1 = s1 + "好财神:=CROSS(80,VAR2);\r\n";
            s1 = s1 + "DRAWICON(好财神>0,买*1.15,2);\r\n";
            s1 = s1 + "DRAWTEXT(好财神>0,买,财神跑);\r\n";
            s1 = s1 + "VAR3:=(CLOSE-LLV(LOW,20))/(HHV(HIGH,20)-LLV(LOW,20))*100; \r\n";
            s1 = s1 + "VAR4:SMA(SMA(VAR3,3,1),3,1),COLOR00FF00;\r\n";
            s1 = s1 + "VAR5:=EMA(VAR4,5); \r\n";
            s1 = s1 + "VAR7:3*VAR4-2*VAR5,COLORGREEN;\r\n";
            s1 = s1 + "DRAWTEXT(CROSS(VAR7,VAR4) AND VAR7<21,VAR4,财神到),COLORYELLOW; \r\n";
            s1 = s1 + "DRAWICON(CROSS(VAR7,VAR4)>0 AND VAR7<21,VAR4,1);\r\n";
            Add("FSL", s1, 2);

            s1 = "TECHNAME:=TRIX;\r\n";
            s1 = s1 + "TECHTITLE:=三重指数平均线\r\n";
            s1 = s1 + "DRAWK:=FALSE;\r\n";
            s1 = s1 + "N:=INPUT(2,100,12,12);\r\n";
            s1 = s1 + "M:=INPUT(2,100,9,9);\r\n";
            s1 = s1 + "MTR:=EMA(EMA(EMA(CLOSE,N),N),N);\r\n";
            s1 = s1 + "TRIX:(MTR-REF(MTR,1))/REF(MTR,1)*100;\r\n";
            s1 = s1 + "MATRIX:MA(TRIX,9) ;\r\n";
            s1 = s1 + "*1.TRIX由下往上交叉其平均线时，为长期买进信号；\r\n";
            s1 = s1 + "*2.TRIX由上往下交叉其平均线时，为长期卖出信号；\r\n";
            s1 = s1 + "*3.DMA、MACD、TRIX 三者构成一组指标群，互相验证。\r\n";
            Add("TRIX", s1);

            s1 = "TECHNAME:=BRAR;\r\n";
            s1 = s1 + "TECHTITLE:=情绪指标\r\n";
            s1 = s1 + "DRAWK:=FALSE;\r\n";
            s1 = s1 + "N:=INPUT(2,120,26,26);\r\n";
            s1 = s1 + "BR:SUM(MAX(0,HIGH-REF(CLOSE,1)),N)/SUM(MAX(0,REF(CLOSE,1)-LOW),N)*100;\r\n";
            s1 = s1 + "AR:SUM(HIGH-OPEN,N)/SUM(OPEN-LOW,N)*100;\r\n";
            s1 = s1 + "20,linedot,COLORWHITE;\r\n";
            s1 = s1 + "400,linedot,COLORWHITE;\r\n";
            s1 = s1 + "*1.BR>400，暗示行情过热，应反向卖出；BR<40 ，行情将起死回生，应买进；\r\n";
            s1 = s1 + "*2.AR>180，能量耗尽，应卖出；AR<40 ，能量已累积爆发力，应买进；\r\n";
            s1 = s1 + "*3.BR 由300 以上的高点下跌至50以下的水平,低于AR 时,为绝佳买点；\r\n";
            s1 = s1 + "*4.BR、AR、CR、VR 四者合为一组指标群，须综合搭配使用。\r\n";
            Add("BRAR", s1);

            s1 = "TECHNAME:=CR;\r\n";
            s1 = s1 + "TECHTITLE:=带状能量线\r\n";
            s1 = s1 + "DRAWK:=FALSE;\r\n";
            s1 = s1 + "N:=INPUT(2,100,26,26);\r\n";
            s1 = s1 + "M1:=INPUT(0,100,10,10);\r\n";
            s1 = s1 + "M2:=INPUT(0,100,20,20);\r\n";
            s1 = s1 + "M3:=INPUT(0,100,40,40);\r\n";
            s1 = s1 + "M4:=INPUT(0,100,62,62);\r\n";
            s1 = s1 + "MID:=REF(HIGH+LOW,1)/2;\r\n";
            s1 = s1 + "CR:SUM(MAX(0,HIGH-MID),N)/SUM(MAX(0,MID-LOW),N)*100;\r\n";
            s1 = s1 + "MA1:REF(MA(CR,M1),M1/2.5+1);\r\n";
            s1 = s1 + "MA2:REF(MA(CR,M2),M2/2.5+1);\r\n";
            s1 = s1 + "MA3:REF(MA(CR,M3),M3/2.5+1);\r\n";
            s1 = s1 + "MA4:REF(MA(CR,M4),M4/2.5+1);\r\n";
            s1 = s1 + "*1.CR>400时，其10日平均线向下滑落，视为卖出信号；CR<40买进；\r\n";
            s1 = s1 + "*2.CR 由高点下滑至其四条平均线下方时，股价容易形成\r\n";
            Add("CR", s1);

            s1 = "TECHNAME:=VR;\r\n";
            s1 = s1 + "TECHTITLE:=成交量变异率\r\n";
            s1 = s1 + "DRAWK:=FALSE;\r\n";
            s1 = s1 + "N:=INPUT(2,100,26,26);\r\n";
            s1 = s1 + "M:=INPUT(2,100,6,6);\r\n";
            s1 = s1 + "TH:=SUM(IF(CLOSE>REF(CLOSE,1),VOL,0),N);\r\n";
            s1 = s1 + "TL:=SUM(IF(CLOSE<REF(CLOSE,1),VOL,0),N);\r\n";
            s1 = s1 + "TQ:=SUM(IF(CLOSE=REF(CLOSE,1),VOL,0),N);\r\n";
            s1 = s1 + "VR:100*(TH*2+TQ)/(TL*2+TQ);\r\n";
            s1 = s1 + "MAVR:MA(VR,M);\r\n";
            s1 = s1 + "*1.VR>450，市场成交过热，应反向卖出；\r\n";
            s1 = s1 + "*2.VR<40 ，市场成交低迷，人心看淡的际，应反向买进；\r\n";
            s1 = s1 + "*3.VR 由低档直接上升至250，股价仍为遭受阻力，此为大行情的前兆；\r\n";
            s1 = s1 + "*4.VR 除了与PSY为同指标群外，尚须与BR、AR、CR同时搭配研判\r\n";
            Add("VR", s1);

            s1 = "TECHNAME:=OBV;\r\n";
            s1 = s1 + "TECHTITLE:=累积能量线;\r\n";
            s1 = s1 + "DRAWK:=FALSE;\r\n";
            s1 = s1 + "M:=INPUT(2,100,30,30);\r\n";
            s1 = s1 + "VA:=IF(CLOSE>REF(CLOSE,1),VOL,0-VOL);\r\n";
            s1 = s1 + "OBV:SUM(IF(CLOSE=REF(CLOSE,1),0,VA),0);\r\n";
            s1 = s1 + "MAOBV:MA(OBV,M);\r\n";
            s1 = s1 + "*1.股价一顶比一顶高，而OBV 一顶比一顶低，暗示头部即将形成；\r\n";
            s1 = s1 + "*2.股价一底比一底低，而OBV 一底比一底高，暗示底部即将形成；\r\n";
            s1 = s1 + "*3.OBV 突破其Ｎ字形波动的高点次数达5 次时，为短线卖点；\r\n";
            s1 = s1 + "*4.OBV 跌破其Ｎ字形波动的低点次数达5 次时，为短线买点；\r\n";
            s1 = s1 + "*5.OBV 与ADVOL、PVT、WAD、ADL同属一组指标群，使用时应综合研判。\r\n";
            Add("OBV", s1);


            s1 = "TECHNAME:=ASI;\r\n";
            s1 = s1 + "TECHTITLE:=实质线;\r\n";
            s1 = s1 + "DRAWK:=FALSE;\r\n";
            s1 = s1 + "N:=INPUT(1,100,6,6);\r\n";
            s1 = s1 + "LC:=REF(CLOSE,1);\r\n";
            s1 = s1 + "AA:=ABS(HIGH-LC);\r\n";
            s1 = s1 + "BB:=ABS(LOW-LC);\r\n";
            s1 = s1 + "CC:=ABS(HIGH-REF(LOW,1));\r\n";
            s1 = s1 + "DD:=ABS(LC-REF(OPEN,1));\r\n";
            s1 = s1 + "R:=IF(AA>BB AND AA>CC,AA+BB/2+DD/4,IF(BB>CC AND BB>AA,BB+AA/2+DD/4,CC+DD/4));\r\n";
            s1 = s1 + "X:=(CLOSE-LC+(CLOSE-OPEN)/2+LC-REF(OPEN,1));\r\n";
            s1 = s1 + "SI:=8*X/R*MAX(AA,BB);\r\n";
            s1 = s1 + "ASI:SUM(SI,0);\r\n";
            s1 = s1 + "MASI:MA(ASI,N);\r\n";
            Add("ASI", s1);

            s1 = "TECHNAME:=EMV;\r\n";
            s1 = s1 + "TECHTITLE:=简易波动指标\r\n";
            s1 = s1 + "DRAWK:=FALSE;\r\n";
            s1 = s1 + "N:=INPUT(2,90,14,14);\r\n";
            s1 = s1 + "M:=INPUT(2,60,9,9);\r\n";
            s1 = s1 + "VOLUME:=MA(VOL,N)/VOL;\r\n";
            s1 = s1 + "MID:=100*(HIGH+LOW-REF(HIGH+LOW,1))/(HIGH+LOW);\r\n";
            s1 = s1 + "EMV:MA(MID*VOLUME*(HIGH-LOW)/MA(HIGH-LOW,N),N);\r\n";
            s1 = s1 + "MAEMV:MA(EMV,M);\r\n";
            s1 = s1 + "0,linedot,COLORWHITE;\r\n";
            s1 = s1 + "*1.EMV 由下往上穿越0 轴时，视为中期买进信号；\r\n";
            s1 = s1 + "*2.EMV 由上往下穿越0 轴时，视为中期卖出信号；\r\n";
            s1 = s1 + "*3.EMV 的平均线穿越0 轴，产生假信号的机会较少；\r\n";
            s1 = s1 + "*4.当ADX 低于±DI时，本指标失去效用；\r\n";
            s1 = s1 + "*5.须长期使用EMV 指标才能获得最佳利润。\r\n";
            Add("EMV", s1);

            s1 = "TECHNAME:=RSI;\r\n";
            s1 = s1 + "TECHTITLE:=相对强弱交易系统;\r\n";
            s1 = s1 + "DRAWK:=FALSE;\r\n";
            s1 = s1 + "N1:=INPUT(2,120,6,6);\r\n";
            s1 = s1 + "N2:=INPUT(2,250,12,12);\r\n";
            s1 = s1 + "N3:=INPUT(2,500,24,24);\r\n";
            s1 = s1 + "LC:=REF(CLOSE,1);\r\n";
            s1 = s1 + "RSI1:SMA(MAX(CLOSE-LC,0),N1,1)/SMA(ABS(CLOSE-LC),N1,1)*100;\r\n";
            s1 = s1 + "RSI2:SMA(MAX(CLOSE-LC,0),N2,1)/SMA(ABS(CLOSE-LC),N2,1)*100;\r\n";
            s1 = s1 + "RSI3:SMA(MAX(CLOSE-LC,0),N3,1)/SMA(ABS(CLOSE-LC),N3,1)*100;\r\n";
            s1 = s1 + "*1.RSI向上突破下限，买入信号；\r\n";
            s1 = s1 + "*2.RSI向下跌破上限，卖出信号；\r\n";
            s1 = s1 + "*3.参数：N　统计天数，一般取6\r\n";
            Add("RSI", s1);


            s1 = "TECHNAME:=WR;\r\n";
            s1 = s1 + "TECHTITLE:=威廉指标\r\n";
            s1 = s1 + "DRAWK:=FALSE;\r\n";
            s1 = s1 + "N:=INPUT(2,100,10,10);\r\n";
            s1 = s1 + "N1:=INPUT(2,100,6,6);\r\n";
            s1 = s1 + "WR1:100*(HHV(HIGH,N)-CLOSE)/(HHV(HIGH,N)-LLV(LOW,N));\r\n";
            s1 = s1 + "WR2:100*(HHV(HIGH,N1)-CLOSE)/(HHV(HIGH,N1)-LLV(LOW,N1));\r\n";
            s1 = s1 + "*输出WR1:100*(N日内最高价的最高值-收盘价)/(N日内最高价的最高值-N日内最低价的最低值)\r\n";
            s1 = s1 + "*输出WR2:100*(N1日内最高价的最高值-收盘价)/(N1日内最高价的最高值-N1日内最低价的最低值)\r\n";
            Add("WR", s1);

            s1 = "TECHNAME:=KDJ;\r\n";
            s1 = s1 + "TECHTITLE:=经典版KDJ;\r\n";
            s1 = s1 + "DRAWK:=FALSE;\r\n";
            s1 = s1 + "N:=INPUT(2,90,9,9,\"天数:Param天\");\r\n";
            s1 = s1 + "M1:=INPUT(2,30,3,3,\"天数:Param天\");\r\n";
            s1 = s1 + "M2:=INPUT(2,30,3,3,\"天数:Param天\");\r\n";
            s1 = s1 + "RSV:=(CLOSE-LLV(LOW,N))/(HHV(HIGH,N)-LLV(LOW,N))*100;\r\n";
            s1 = s1 + "K:SMA(RSV,M1,1);\r\n";
            s1 = s1 + "D:SMA(K,M2,1);\r\n";
            s1 = s1 + "J:3*K-2*D;\r\n";
            s1 = s1 + "*1.指标>80 时，回档机率大；指标<20时，反弹机率大；\r\n";
            s1 = s1 + "*2.K在20左右向上交叉D时，视为买进信号；\r\n";
            s1 = s1 + "*3.K在80左右向下交叉D时，视为卖出信号；\r\n";
            s1 = s1 + "*4.J>100 时，股价易反转下跌；J<0 时，股价易反转上涨；\r\n";
            s1 = s1 + "*5.KDJ 波动于50左右的任何信号，其作用不大。\r\n";
            Add("KDJ", s1);

            s1 = "TECHNAME:=CCI;\r\n";
            s1 = s1 + "TECHTITLE:=商品路径指标\r\n";
            s1 = s1 + "DRAWK:=FALSE;\r\n";
            s1 = s1 + "N:=INPUT(2,100,14,14);\r\n";
            s1 = s1 + "TYP:=(HIGH+LOW+CLOSE)/3;\r\n";
            s1 = s1 + "CCI:(TYP-MA(TYP,N))/(0.015*AVEDEV(TYP,N));\r\n";
            s1 = s1 + "-100,linedot,COLORWHITE;\r\n";
            s1 = s1 + "0,linedot,COLORWHITE;\r\n";
            s1 = s1 + "100,linedot,COLORWHITE;\r\n";
            s1 = s1 + "*TYP赋值:(最高价+最低价+收盘价)/3\r\n";
            s1 = s1 + "*输出CCI:(TYP-TYP的N日简单移动平均)/(0.015*TYP的N日平均绝对偏差)\r\n";
            Add("CCI", s1);

            s1 = "TECHNAME:=ROC;\r\n";
            s1 = s1 + "TECHTITLE:=变动速率交易系统;\r\n";
            s1 = s1 + "DRAWK:=FALSE\r\n";
            s1 = s1 + "N:=INPUT(1,100,12,12);\r\n";
            s1 = s1 + "M:=INPUT(1,20,6,6);\r\n";
            s1 = s1 + "ROC:100*(CLOSE-REF(CLOSE,N))/REF(CLOSE,N);\r\n";
            s1 = s1 + "MAROC:MA(ROC,M);\r\n";
            s1 = s1 + "-6.5,linedot,COLORWHITE;\r\n";
            s1 = s1 + "0,linedot,COLORWHITE;\r\n";
            s1 = s1 + "6.5,linedot,COLORWHITE;\r\n";
            s1 = s1 + "*1.ROC向下跌破零，卖出信号；\r\n";
            s1 = s1 + "*2.ROC向上突破零，买入信号；\r\n";
            s1 = s1 + "*3.参数：N 间隔天数；M　计算移动平均的天数\r\n";
            Add("ROC", s1);

            s1 = "TECHNAME:=MTM;\r\n";
            s1 = s1 + "TECHTITLE:=动力指标交易系统;\r\n";
            s1 = s1 + "DRAWK:=FALSE;\r\n";
            s1 = s1 + "N:=INPUT(2,120,12,12);\r\n";
            s1 = s1 + "M:=INPUT(2,60,6,6);\r\n";
            s1 = s1 + "MTM:CLOSE-REF(CLOSE,N);\r\n";
            s1 = s1 + "MTMMA:MA(MTM,M);\r\n";
            s1 = s1 + "*参数：N 　间隔天数，也是求移动平均的天数，一般为6\r\n";
            s1 = s1 + "*1.MTMMA线向上突破零，买入信号；\r\n";
            s1 = s1 + "*2.MTMMA线向下跌破零，卖出信号；\r\n";
            Add("MTM", s1);



            s1 = "TECHNAME:=BOLL;\r\n";
            s1 = s1 + "TECHTITLE:=布林带交易系统;\r\n";
            s1 = s1 + "DRAWK:=TRUE\r\n";
            s1 = s1 + "N:=INPUT(5,100,20,20);\r\n";
            s1 = s1 + "MID:MA(CLOSE,N);\r\n";
            s1 = s1 + "UPPER:MID+2*STD(CLOSE,N);\r\n";
            s1 = s1 + "LOWER:MID-2*STD(CLOSE,N);\r\n";
            s1 = s1 + "*一、收盘价向上突破下限，为买入信号；\r\n";
            s1 = s1 + "*二、收盘价向上突破上限，为卖出信号；\r\n";
            s1 = s1 + "*三、参数： N  天数，在计算布林带时用，一般26天\r\n";
            s1 = s1 + "*       P　一般为20，用于调整上限和下限的值\r\n";
            Add("BOLL", s1);

            s1 = "TECHNAME:=PSY;\r\n";
            s1 = s1 + "TECHTITLE:=心理线\r\n";
            s1 = s1 + "DRAWK:=FALSE;\r\n";
            s1 = s1 + "N:=INPUT(2,100,12,12);\r\n";
            s1 = s1 + "M:=INPUT(2,100,6,6);\r\n";
            s1 = s1 + "PSY:COUNT(CLOSE>REF(CLOSE,1),N)/N*100;\r\n";
            s1 = s1 + "PSYMA:MA(PSY,M);\r\n";
            s1 = s1 + "25,linedot,COLORWHITE;\r\n";
            s1 = s1 + "75,linedot,COLORWHITE;\r\n";
            s1 = s1 + "#1.PSY>75，形成Ｍ头时，股价容易遭遇压力；\r\n";
            s1 = s1 + "#2.PSY<25，形成Ｗ底时，股价容易获得支撑；\r\n";
            s1 = s1 + "#3.PSY 与VR 指标属一组指标群，须互相搭配使用。\r\n";
            Add("PSY", s1);

            s1 = "TECHNAME:=BIAS;\r\n";
            s1 = s1 + "TECHTITLE:=乖离率\r\n";
            s1 = s1 + "DRAWK:=FALSE;\r\n";
            s1 = s1 + "N1:=INPUT(2,250,6,6);\r\n";
            s1 = s1 + "N2:=INPUT(2,250,12,12);\r\n";
            s1 = s1 + "N3:=INPUT(2,250,24,24);\r\n";
            s1 = s1 + "BIAS1:(CLOSE-MA(CLOSE,N1))/MA(CLOSE,N1)*100;\r\n";
            s1 = s1 + "BIAS2:(CLOSE-MA(CLOSE,N2))/MA(CLOSE,N2)*100;\r\n";
            s1 = s1 + "BIAS3:(CLOSE-MA(CLOSE,N3))/MA(CLOSE,N3)*100;\r\n";
            s1 = s1 + "*输出BIAS1 :(收盘价-收盘价的N1日简单移动平均)/收盘价的N1日简单移动平均*100\r\n";
            s1 = s1 + "*输出BIAS2 :(收盘价-收盘价的N2日简单移动平均)/收盘价的N2日简单移动平均*100\r\n";
            s1 = s1 + "*输出BIAS3 :(收盘价-收盘价的N3日简单移动平均)/收盘价的N3日简单移动平均*100\r\n";
            Add("BIAS", s1);

            s1 = "TECHNAME:=MIKE;\r\n";
            s1 = s1 + "TECHTITLE:=麦克支撑压力;\r\n";
            s1 = s1 + "N:=INPUT(2,120,10,10);\r\n";
            s1 = s1 + "DRAWK:=TRUE;\r\n";
            s1 = s1 + "HLC:=REF(MA((HIGH+LOW+CLOSE)/3,N),1);\r\n";
            s1 = s1 + "HV:=EMA(HHV(HIGH,N),3);\r\n";
            s1 = s1 + "LV:=EMA(LLV(LOW,N),3);\r\n";
            s1 = s1 + "STOR:EMA(2*HV-LV,3);\r\n";
            s1 = s1 + "MIDR:EMA(HLC+HV-LV,3);\r\n";
            s1 = s1 + "WEKR:EMA(HLC*2-LV,3);\r\n";
            s1 = s1 + "WEKS:EMA(HLC*2-HV,3);\r\n";
            s1 = s1 + "MIDS:EMA(HLC-HV+LV,3);\r\n";
            s1 = s1 + "STOS:EMA(2*LV-HV,3);\r\n";
            s1 = s1 + "#1.MIKE指标共有六条曲线，上方三条压力线，下方三条支撑线；\r\n";
            s1 = s1 + "#2.当股价往压力线方向涨升时，其下方支撑线不具参考价值；\r\n";
            s1 = s1 + "#3.当股价往支撑线方向下跌时，其上方压力线不具参考价值；\r\n";
            Add("MIKE", s1);

            s1 = "TECHNAME:=WVAD;\r\n";
            s1 = s1 + "TECHTITLE:=威廉变异离散量\r\n";
            s1 = s1 + "DRAWK:=FALSE;\r\n";
            s1 = s1 + "N:=INPUT(2,120,24,24);\r\n";
            s1 = s1 + "M:=INPUT(2,60,6,6);\r\n";
            s1 = s1 + "WVAD:SUM((CLOSE-OPEN)/(HIGH-LOW)*VOL,N)/10000;\r\n";
            s1 = s1 + "MAWVAD:MA(WVAD,M);\r\n";
            s1 = s1 + "*1.WVAD由下往上穿越0 轴时，视为长期买进信号；\r\n";
            s1 = s1 + "*2.WVAD由上往下穿越0 轴时，视为长期卖出信号； \r\n";
            s1 = s1 + "*3.当ADX 低于±DI时，本指标失去效用；\r\n";
            s1 = s1 + "*4.长期使用WVAD指标才能获得最佳利润；\r\n";
            s1 = s1 + "*5.本指标可与EMV 指标搭配使用。\r\n";
            Add("WVAD", s1);



            s1 = "TECHNAME:=FS;\r\n";
            s1 = s1 + "TECHTITLE:=分时均线;\r\n";
            s1 = s1 + "VOL1:=SUM(VOL,0);\r\n";
            s1 = s1 + "VK:=VOL*CLOSE;\r\n";
            s1 = s1 + "V2:=SUM(VK,0);\r\n";
            s1 = s1 + "JJ:=V2/VOL1;\r\n";
            s1 = s1 + "均价线:IF(JJ>0,V2/VOL1,CLOSE),COLORYELLOW;\r\n";
            s1 = s1 + "收盘价:CLOSE,COLORWHITE;\r\n";
            s1 = s1 + "DRAWK:=FALSE;\r\n";
            Add("FS", s1,3);
            /*
            s1 = "TECHNAME:=MCST;\r\n";
            s1 = s1 + "DRAWK:=TRUE;\r\n";
            s1 = s1 + "MCST:DMA(AMOUNT/(100*VOL),VOL/CAPITAL);\r\n";
            s1 = s1 + "TECHTITLE:=市场成本;\r\n";
            s1 = s1 + "*1.MCST是市场平均成本价格；\r\n";
            s1 = s1 + "*2.MCST上升表明市场筹码平均持有成本上升；\r\n";
            s1 = s1 + "*3.MCST下降表明市场筹码平均持有成本下降。\r\n";
            Add("MCST", s1);
            */
            s1 = "TECHNAME:=EXPMA;\r\n";
            s1 = s1 + "TECHTITLE:=指数平均线;\r\n";
            s1 = s1 + "M1:=INPUT(2,250,12,12);\r\n";
            s1 = s1 + "M2:=INPUT(2,250,50,50);\r\n";
            s1 = s1 + "EXP1:EXPMA(CLOSE,M1);\r\n";
            s1 = s1 + "EXP2:EXPMA(CLOSE,M2);\r\n";
            s1 = s1 + "*1.EXPMA 一般以观察12日和50日二条均线为主；\r\n";
            s1 = s1 + "*2.12日指数平均线向上交叉50日指数平均线时，买进；\r\n";
            s1 = s1 + "*3.12日指数平均线向下交叉50日指数平均线时，卖出；\r\n";
            s1 = s1 + "*4.EXPMA 是多种平均线计算方法的一；\r\n";
            s1 = s1 + "*5.EXPMA 配合MTM 指标使用，效果更佳。\r\n";
            Add("EXPMA", s1, 2);

            s1 = "TECHNAME:=BDT;\r\n";
            s1 = s1 + "TECHTITLE:=波段图;\r\n";
            s1 = s1 + "DRAWK:=FALSE;\r\n";
            s1 = s1 + "N:=INPUT(1,100,18,18);\r\n";
            s1 = s1 + "M:=INPUT(1,100,9,9);\r\n";
            s1 = s1 + "N2:=INPUT(1,100,31,31);\r\n";
            s1 = s1 + "VAR3:=(CLOSE-LLV(CLOSE,N))/(HHV(CLOSE,N)-LLV(CLOSE,N));\r\n";
            s1 = s1 + "VAR4:=SMA(VAR3,M,1);\r\n";
            s1 = s1 + "VAR5:=SMA(VAR4,1,1);\r\n";
            s1 = s1 + "J:3*VAR4-2*VAR5;\r\n";
            s1 = s1 + "A2:EMA(VAR4,N2);\r\n";
            s1 = s1 + "PASSWORD:=WXB\r\n";
            s1 = s1 + "STICKLINE(J,A2,COLORRED,COLORGREEN);\r\n";
            Add("BDT", s1, 2);

            s1 = "TECHNAME:=DB6;\r\n";
            s1 = s1 + "TECHTITLE:=波段图;\r\n";
            s1 = s1 + "DRAWK:=FALSE;\r\n";
            s1 = s1 + "N:=INPUT(1,100,9,9);\r\n";
            s1 = s1 + "M:=INPUT(1,100,6,6);\r\n";
            s1 = s1 + "K:=INPUT(1,100,3,3);\r\n";
            s1 = s1 + "A:=(CLOSE-LLV(CLOSE,N))/(HHV(CLOSE,N)-LLV(CLOSE,N));\r\n";
            s1 = s1 + "B:=SMA(A,M,1);\r\n";
            s1 = s1 + "FCLOSE:=SMA(B,K,1);\r\n";
            s1 = s1 + "PARTLINE(FCLOSE,COLORRED,COLORGREEN);\r\n";
            s1 = s1 + "BTX(FCLOSE,COLORRED,COLORGREEN);\r\n";
            Add("DB6", s1, 2);

            s1 = "TECHNAME:=BTX\r\n";
            s1 = s1 + "TECHTITLE:=宝塔线一;\r\n";
            s1 = s1 + "DRAWK:=FALSE\r\n";
            s1 = s1 + "MA5:MA(CLOSE,5)\r\n";
            s1 = s1 + "MA10:MA(CLOSE,10)\r\n";
            s1 = s1 + "MA20:MA(CLOSE,20)\r\n";
            s1 = s1 + "MA30:MA(CLOSE,30)\r\n";
            s1 = s1 + "MA60:MA(CLOSE,40)\r\n";
            s1 = s1 + "BTX(CLOSE,COLORRED,COLORGREEN)\r\n";
            Add("BTX", s1, 2);

            s1 = "TECHNAME:=TWR;\r\n";
            s1 = s1 + "TECHTITLE:=宝塔线二;\r\n";
            s1 = s1 + "DRAWK:=FALSE\r\n";
            s1 = s1 + "MA5:MA(CLOSE,5);\r\n";
            s1 = s1 + "MA10:MA(CLOSE,10);\r\n";
            s1 = s1 + "MA20:MA(CLOSE,20);\r\n";
            s1 = s1 + "TWR(CLOSE,COLORRED,COLORGREEN)\r\n";
            Add("TWR", s1, 2);

            s1 = "TECHNAME:=PBX;\r\n";
            s1 = s1 + "TECHTITLE:=瀑布线;\r\n";
            s1 = s1 + "DRAWK:=TRUE;\r\n";
            s1 = s1 + "M1:=INPUT(3,10,4,4);\r\n";
            s1 = s1 + "M2:=INPUT(3,20,6,6);\r\n";
            s1 = s1 + "M3:=INPUT(3,30,9,9);\r\n";
            s1 = s1 + "M4:=INPUT(3,40,13,13);\r\n";
            s1 = s1 + "M5:=INPUT(3,50,18,18);\r\n";
            s1 = s1 + "M6:=INPUT(3,60,24,24);\r\n";
            s1 = s1 + "PBX1:(EXPMA(CLOSE,M1)+MA(CLOSE,M1*2)+MA(CLOSE,M1*4))/3;\r\n";
            s1 = s1 + "PBX2:(EXPMA(CLOSE,M2)+MA(CLOSE,M2*2)+MA(CLOSE,M2*4))/3;\r\n";
            s1 = s1 + "PBX3:(EXPMA(CLOSE,M3)+MA(CLOSE,M3*2)+MA(CLOSE,M3*4))/3;\r\n";
            s1 = s1 + "PBX4:(EXPMA(CLOSE,M4)+MA(CLOSE,M4*2)+MA(CLOSE,M4*4))/3;\r\n";
            s1 = s1 + "PBX5:(EXPMA(CLOSE,M5)+MA(CLOSE,M5*2)+MA(CLOSE,M5*4))/3;\r\n";
            s1 = s1 + "PBX6:(EXPMA(CLOSE,M6)+MA(CLOSE,M6*2)+MA(CLOSE,M6*4))/3;\r\n";
            Add("PBX", s1, 2);

            s1 = "TECHNAME:=PBX1;\r\n";
            s1 = s1 + "TECHTITLE:=瀑布线;\r\n";
            s1 = s1 + "DRAWK:=FALSE;\r\n";
            s1 = s1 + "S1:SUM(CLOSE,5);\r\n";
            Add("PBX1", s1, 2);

            s1 = "TECHNAME:=LB;\r\n";
            s1 = s1 + "TECHTITLE:=量比;\r\n";
            s1 = s1 + "K:=BARSCOUNT(CLOSE);\r\n";
            s1 = s1 + "PV1:=SUM(V,0)/K;\r\n";
            s1 = s1 + "量比:PV1/100000;\r\n";
            Add("LB", s1, 2);

            s1 = "TECHNAME:=FSVOL;\r\n";
            s1 = s1 + "TECHTITLE:=分时成交量;\r\n";
            s1 = s1 + "DRAWK:=FALSE;\r\n";
            s1 = s1 + "VOLUME:VOL,STICK,COLORYELLOW;\r\n";
            Add("FSVOL", s1, 3);

            s1 = "TECHNAME:=MA;\r\n";
            s1 = s1 + "TECHTITLE:=均线;\r\n";
            s1 = s1 + "DRAWK:=TRUE;\r\n";
            s1 = s1 + "M1:=INPUT(2,1000,5,5);\r\n";
            s1 = s1 + "M2:=INPUT(2,1000,10,10);\r\n";
            s1 = s1 + "M3:=INPUT(2,1000,20,20);\r\n";
            s1 = s1 + "M4:=INPUT(2,1000,30,30);\r\n";
            s1 = s1 + "M5:=INPUT(2,1000,60,60);\r\n";
            s1 = s1 + "MA1:MA(CLOSE,M1);\r\n";
            s1 = s1 + "MA2:MA(CLOSE,M2);\r\n";
            s1 = s1 + "MA3:MA(CLOSE,M3);\r\n";
            s1 = s1 + "MA4:MA(CLOSE,M4);\r\n";
            s1 = s1 + "MA5:MA(CLOSE,M5);\r\n";
            s1 = s1 + "*1.股价高于平均线，视为强势；股价低于平均线，视为弱势\r\n";
            s1 = s1 + "*2.平均线向上涨升，具有助涨力道；平均线向下跌降，具有助跌\r\n";
            Add("MA", s1, 3);
            s1 = "TECHNAME:=VOL;\r\n";
            s1 = s1 + "DRAWK:=FALSE;\r\n";
            s1 = s1 + "C1:=REF(CLOSE,1);\r\n";
            s1 = s1 + "MA5:MA(VOL,5);\r\n";
            s1 = s1 + "MA10:MA(VOL,10);\r\n";
            s1 = s1 + "MA20:MA(VOL,20);\r\n";
            s1 = s1 + "MA40:MA(VOL,40);\r\n";
            s1 = s1 + "VOLUME:VOL,VOLSTICK;\r\n";
            Add("VOL", s1, 3);

            s1 = "TECHNAME:=MYTEST;\r\n";
            s1 = s1 + "DRAWK:=FALSE;\r\n";
            s1 = s1 + "均:SUM(CLOSE*VOL,240)/SUM(VOL,240),COLORFFFF80,DRAWLINE;\r\n";
            s1 = s1 + "A:=EMA(CLOSE,12)-EMA(CLOSE,26)+AVEDEV(TIME,1);\r\n";
            s1 = s1 + "信号一:=CROSS(A,0) AND COUNT(CLOSE>均,5)>=1;\r\n";
            s1 = s1 + "信号二:=CROSS(0,A) AND COUNT(CLOSE>均,5)>=1;\r\n";
            s1 = s1 + "STICKLINE(信号一 AND REF(A,1)<A,均,CLOSE,4,0),COLORGREEN;\r\n";
            s1 = s1 + "STICKLINE(信号二 AND REF(A,1)>A,均,CLOSE,4,9),COLORRED;\r\n";
            s1 = s1 + "DRAWTEXT(信号一 AND REF(A,1)<A,CLOSE,买);\r\n";
            s1 = s1 + "DRAWICON(信号二 AND REF(A,1)>A,CLOSE,1);\r\n";
            Add("MYTEST", s1, 2);

        }

    }

}