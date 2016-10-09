using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;
using Common.Logging;
using TradingLib.DataCore;
using System.ComponentModel;

namespace TradingLib.DataFarmManager
{
    public class BarUploader
    {
        ILog logger = LogManager.GetLogger("BarUploader");
        /// <summary>
        /// 交易所
        /// </summary>
        public string Exchange { get; set; }
        /// <summary>
        /// 合约
        /// </summary>
        public string Symbol { get; set; }
        /// <summary>
        /// 周期类别
        /// </summary>
        public BarInterval IntervalType { get; set; }

        /// <summary>
        /// 周期数
        /// </summary>
        public int Interval { get; set; }


        string _filename = string.Empty;
        public BarUploader()
        {
            this.Exchange = string.Empty;
            this.Symbol = string.Empty;
            this.IntervalType = BarInterval.CustomTime;
            this.Interval = 0;
        }

        public void SetBarFile(string file)
        {
            _filename = file;
        }

        BackgroundWorker worker = null;
        public void Start() 
        {
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            //bg.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(bg_ProgressChanged);
            worker.RunWorkerAsync();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            UploadProcess();
        }

        void UploadProcess()
        {
            using (BarReader br = new BarReader(this._filename))
            {

                List<BarImpl> barlist = new List<BarImpl>();
                br.GotBar += new Action<BarImpl>(new Action<BarImpl>((bar) => { barlist.Add(bar); }));
                while (br.NextTick())
                {

                }
                logger.Info(string.Format("Load {0} Bars into memory", br.Count));

                UploadBarDataRequest response = new UploadBarDataRequest();
                response.Header.Exchange = this.Exchange;
                response.Header.Symbol = this.Symbol;
                response.Header.IntervalType = this.IntervalType;
                response.Header.Interval = this.Interval;

                int j = 0;
                for (int i = 0; i < barlist.Count; i++)
                {
                    response.Add(barlist[i]);
                    j++;
                    if (j == BATCHSIZE)
                    {
                        response.Header.BarCount = response.Bars.Count;
                        //一定数目的Bar之后 发送数据 同时判断是否是最后一条
                        DataCoreService.DataClient.SendPacket(response);
                        //Util.sleep();
                        //不是最后一条数据则生成新的Response
                        if (!(i == barlist.Count - 1))
                        {
                            response = new UploadBarDataRequest();
                            response.Header.Exchange = this.Exchange;
                            response.Header.Symbol = this.Symbol;
                            response.Header.IntervalType = this.IntervalType;
                            response.Header.Interval = this.Interval;
                        }
                        j = 0;
                    }
                }
                //最后一部分数据如果留尾 则发送
                if (response.Bars.Count > 0)
                {
                    response.Header.BarCount = response.Bars.Count;
                    DataCoreService.DataClient.SendPacket(response);
                }

                logger.Info("Bar upload success");
            }
        }

        const int BATCHSIZE = 1000;
        

    }
}
