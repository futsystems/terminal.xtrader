namespace TradingLib.KryptonControl
{
    partial class PageEasyChart
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            Easychart.Finance.ExchangeIntraday exchangeIntraday1 = new Easychart.Finance.ExchangeIntraday();
            this.WinChartControl = new Easychart.Finance.Win.ChartWinControl();
            this.sizeToolControl1 = new Easychart.Finance.Win.SizeToolControl();
            this.SuspendLayout();
            // 
            // WinChartControl
            // 
            this.WinChartControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.WinChartControl.CausesValidation = false;
            this.WinChartControl.DefaultFormulas = "";
            this.WinChartControl.Designing = false;
            this.WinChartControl.EndTime = new System.DateTime(((long)(0)));
            this.WinChartControl.FavoriteFormulas = "VOLMA;RSI;CCI;OBV;ATR;FastSTO;SlowSTO;ROC;TRIX;WR;AD;CMF;PPO;StochRSI;ULT;BBWidth" +
                ";PVO";
            exchangeIntraday1.TimePeriods = new Easychart.Finance.TimePeriod[0];
            exchangeIntraday1.TimeZone = -4D;
            this.WinChartControl.IntradayInfo = exchangeIntraday1;
            this.WinChartControl.Location = new System.Drawing.Point(0, 0);
            this.WinChartControl.Margin = new System.Windows.Forms.Padding(0);
            this.WinChartControl.MaxPrice = 0D;
            this.WinChartControl.MinPrice = 0D;
            this.WinChartControl.Name = "WinChartControl";
            this.WinChartControl.PriceLabelFormat = "";
            this.WinChartControl.Size = new System.Drawing.Size(780, 400);
            this.WinChartControl.StartTime = new System.DateTime(((long)(0)));
            this.WinChartControl.Symbol = "";
            this.WinChartControl.TabIndex = 0;
            // 
            // sizeToolControl1
            // 
            this.sizeToolControl1.ChartControl = this.WinChartControl;
            this.sizeToolControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.sizeToolControl1.Location = new System.Drawing.Point(0, 400);
            this.sizeToolControl1.Name = "sizeToolControl1";
            this.sizeToolControl1.Size = new System.Drawing.Size(780, 20);
            this.sizeToolControl1.TabIndex = 1;
            // 
            // ctlEasyChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.sizeToolControl1);
            this.Controls.Add(this.WinChartControl);
            this.Name = "ctlEasyChart";
            this.Size = new System.Drawing.Size(780, 420);
            this.ResumeLayout(false);

        }

        #endregion

        private Easychart.Finance.Win.ChartWinControl WinChartControl;
        private Easychart.Finance.Win.SizeToolControl sizeToolControl1;
    }
}
