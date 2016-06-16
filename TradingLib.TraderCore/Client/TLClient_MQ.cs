using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Xml;
using TradingLib.API;
using TradingLib.Common;
using Common.Logging;


/*���ڿͻ��������˵����ӻ���
 * 1.�ͻ���������ͨ��ZeroMQ�����������
 * 2.�ͻ�����������˫����������,�ͻ��˰�һ��Ƶ�������˷���HeartBeat������Ϣ,���߷���˸ÿͻ��˴��״̬,�������ͻ��˲��ϵ�����tick�Լ���������
 * �ͻ��˼�¼������ϴ���Ϣʱ��,������һ��ʱ����,�ͻ������������˷���һ��������Ϣ(HeartBeatRequest)������������������˸�������Ϣ,��ͻ���֪����
 * ����˴���������Ϣ�ر��쳣��ر����ӣ�����Mode()��������
 * 3.connectֻ������ϴ�Mode��������֮��Ե�ǰ�����ӳ�����������
 *   Mode��ͨ��TLFound��������������б�,�����õķ���˻�������,�����ӵ�һ����������
 * 4.�ͻ��˵�����ά�����Ʒ��������쳣��������Mode.
 * 5.�ͻ���TLSend�������ֿͻ����Ӷ�ʧ,������retryconnect����connect��ǰ����(ע �����Ӳ�������Mode ������TLFound ������б�)
 * 20�ͻ���200k/s(ͨ��kvmessage���Խ�����50k/s����) ��̨�����Ӧ�ÿ������䵽500���ͻ��˲������������2̨����ͬ�����Ƶķ����� Ӧ�ÿ���֧��1000���������ҡ�
 * 
 * 
 * 
 * 
 * 
 * */
namespace TradingLib.TraderCore
{
    /// <summary>
    /// ���ڽ�����������������,�������ݻ��߽�����ϢͨѶ
    /// </summary>
    public class TLClient_MQ
    {
        string PROGRAME = "TLClient_MQ";
        ILog logger = null;


        const string _skip = "        ";
        /// <summary>
        /// �������ͣ�����/�ɽ�/����ͬʱ֧��
        /// </summary>
        public QSEnumProviderType ProviderType { get; set; }

        AsyncClient _mqcli = null;//ͨѶclient���
        int _tickerrors = 0;//tick���ݴ���������
        int port = Const.TLDEFAULTBASEPORT;//Ĭ�Ϸ���˿�
        int _wait = 5;//��̨�������״̬Ƶ��
        public const int DEFAULTWAIT = Const.DEFAULTWAIT;//��������̼߳��Ƶ��

        int heartbeatperiod = Const.HEARTBEATPERIOD;//�����˷���������Ϣ���

        bool _started = false;//��̨�������״̬�߳��Ƿ�����
        bool _connect = false;//�ͻ����Ƿ����ӵ������

        int _tickheartbeatdead = Const.TICKHEARTBEATDEADMS;
        int _tickprocesscheckfreq = Const.TICKHEARTBEATCHECKFREQ;

        int _sendheartbeat = Const.SENDHEARTBEATMS;//��������������
        int _heartbeatdeadat = Const.HEARTBEATDEADMS;//�����������
        long _lastheartbeat = 0;//�������ʱ��
        long _tickhartbeat = 0;//���tick����ʱ��
        bool _requestheartbeat = false;//���������ظ�
        bool _recvheartbeat = false;//�յ������ظ�
        bool _reconnectreq = false;//������������

        List<MessageTypes> _rfl = new List<MessageTypes>();
        public List<MessageTypes> RequestFeatureList { get { return _rfl; } }//�����б�


        List<string> serverip = new List<string>();//�����IP�б� ����������IP��ַ�б�
        //����������TLFound��ѯ ֪����Щ��ַ�ķ�����Ǽ���ģ��������ַ��¼
        List<Providers> servers = new List<Providers>();//��ǰ���÷����
        List<string> avabileip = new List<string>();//��ǰ���õ�IP�б�

        string tickip = "";
        int tickport=5572;


        //�ͻ��˱�ʶ
        string _name = string.Empty;
        public string Name { get { return _name; } set { _name = value; } }
        //�������Ӵ���
        int _disconnectretry = 3;
        public const int DEFAULTRETRIES = 3;//Ĭ�ϳ������Ӵ���
        public int DisconnectRetries { get { return _disconnectretry; } set { _disconnectretry = value; } }

        int _remodedelay = Const.RECONNECTDELAY;//���������������½��������� Modeʧ�ܺ��ٴ�Mode��ʱ���� ��λ��
        int _modeRetries = Const.RECONNECTTIMES;//������������ͨ��Mode�������������б� ���������ӣ����Դ���
        public int ModeRetries { get { return _modeRetries; } set { _modeRetries = value; } }
        //���÷�����б�
        public Providers[] ProvidersAvailable { get { return servers.ToArray(); } }
        //��ǰ���ӷ�������
        int _curprovider = -1;
        public int ProviderSelected { get { return _curprovider; } }
        //BrokerName
        Providers _bn = Providers.Unknown;
        public Providers BrokerName { get { return _bn; } }
        //����˰汾
        private int _serverversion;
        public int ServerVersion { get { return _serverversion; } }
        //����˰汾��ͻ���API�汾�Ƿ�ƥ��
        public bool IsAPIOK { get { return Util.Version >= _serverversion; } }

        public bool IsConnected { get { return _mqcli==null?false:_mqcli.isConnected; } }//�Ƿ�����

        public bool IsTickConnected { get { return _mqcli == null ? false : _mqcli.isTickConnected; } }
        //������Ӧ�Ƿ����� �������� ���� �����������������һ��(ȷ�����������ظ�������Ƿ��յ������ر�)
        public bool isHeartbeatOk { get { return _connect && (_requestheartbeat == _recvheartbeat); } }

        #region Event
        /// <summary>
        /// ����ͨ�������¼�
        /// </summary>
        public event ConnectDel OnConnectEvent;
        /// <summary>
        /// ����ͨ���Ͽ��¼�
        /// </summary>
        public event DisconnectDel OnDisconnectEvent;
        /// <summary>
        /// ����ͨ�������¼�
        /// </summary>
        public event DataPubConnectDel OnDataPubConnectEvent;
        /// <summary>
        /// ����ͨ���Ͽ��¼�
        /// </summary>
        public event DataPubDisconnectDel OnDataPubDisconnectEvent;
        
        /// <summary>
        /// ����ر��¼�
        /// </summary>
        public event LoginResponseDel OnLoginResponse;

        /// <summary>
        /// �����߼����ݰ��¼�
        /// </summary>
        public event IPacketDelegate OnPacketEvent;
        #endregion


        #region ��̨ά���߳�

        #region tick����ά���߳� ���ڼ��tick���� ����Ϣ���ж�,�Զ�����

        void StartTickWatcher()
        {
            if (ticktrackergo) return;
            ticktrackergo = true;
            _tickwatchthread = new Thread(tickprocess);
            _tickwatchthread.IsBackground = true;
            _tickwatchthread.Start();
            logger.Info(PROGRAME + " :TickWatcher threade started");
        }

        void StopTickWatcher()
        {
            if (!ticktrackergo) return;
            ticktrackergo = false;
            _tickwatchthread.Abort();
            _tickwatchthread = null;
            logger.Info(PROGRAME + " :TickWatcher threade stopped");
        }
        Thread _tickwatchthread = null;
        bool tickenable = false;//�Ƿ����tick����
        
        bool ticktrackergo = false;
        bool _tickreconnectreq = false;

        void tickprocess()
        {
            while (ticktrackergo)
            {
                // ��õ�ǰʱ��
                long now = DateTime.Now.Ticks;
                //�����ϴ�heartbeat������ʱ����
                long diff = (now - _tickhartbeat) / 10000;//(ticks/10000�õ�MS)
                //debug("connect:" + _connect.ToString() + " reconnecttick:" + _tickreconnectreq.ToString() + " diff:" + diff.ToString() + " _tickheartbeatdead:" + _tickheartbeatdead.ToString());
                if (!(_connect && (!_tickreconnectreq) && (!_reconnectreq)  && (diff < _tickheartbeatdead)))//�κ�һ�����������㽫��������Ĳ���
                {
                    //debug("connect:" + _connect.ToString() + " reconnecttick:" + _tickreconnectreq.ToString() + " diff:" + diff.ToString() + " _tickheartbeatdead:" + _tickheartbeatdead.ToString() + " now:" + now.ToString() + " _lastick:" + _tickhartbeat.ToString());
                    if (_tickreconnectreq == false)
                    {
                        logger.Warn(PROGRAME + ":Tick Publisher stream stopped");
                        //��������tick��������
                        _tickreconnectreq = true;
                        new Thread(reconnectTick).Start();
                    }

                }

                Thread.Sleep(_tickprocesscheckfreq);
            }
            
        
        }
        #endregion


        #region ��������Ӽ���߳�(�÷�������Ӵ���ί��/�ɽ�/��ѯ)
        Thread _bwthread = null;
        void StartBW()
        {
            if (_started) return;

            _started = true;
            _bwthread = new Thread(_bw_DoWork);
            _bwthread.IsBackground = true;
            _bwthread.Start();
            logger.Info(PROGRAME + " :BW Backend threade started");
        }

        void StopBW()
        {
            if (!_started) return;

            _started = false;
            _bwthread.Abort();
            _bwthread = null;
            logger.Info(PROGRAME +" :BW Backend threade stopped");
        }
    
        /// <summary>
        /// ����ά���߳�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _bw_DoWork()
        {
            //int p = (int)e.Argument;
            while (_started)
            {
                // ��õ�ǰʱ��
                long now = DateTime.Now.Ticks;
                //�����ϴ�heartbeat������ʱ����
                long diff = (now - _lastheartbeat) / 10000;//(ticks/10000�õ�MS)
                //debug("����:" + _connect.ToString() + " ������������:" + (!_reconnectreq).ToString() + "�������"+(diff < _sendheartbeat).ToString()+" �ϴ�����ʱ��:" + _lastheartbeat.ToString() + " Diff:" + diff.ToString() + " �����������:" + _sendheartbeat.ToString());
                if (!(_connect && (!_reconnectreq) && (diff < _sendheartbeat)))//�κ�һ�����������㽫��������Ĳ���
                {
                    try
                    {
                        if (isHeartbeatOk)
                        {
                            //debug(PROGRAME + ":heartbeat request at: " + DateTime.Now.ToString()+" _heartbeatdeadat:"+_heartbeatdeadat.ToString() + " _diff:"+diff.ToString(),QSEnumDebugLevel.INFO);
                            //���õ���Ӧ�����,_recvheartbeat = !_recvheartbeat; ����ڷ�����һ��hearbeatrequest�� ��û�еõ�����������ǰ�����ٴ����·���
                            _requestheartbeat = !_recvheartbeat;
                            //��������������Ӧ
                            HeartBeatRequest hbr = RequestTemplate<HeartBeatRequest>.CliSendRequest(0);

                            TLSend(hbr);
                        }
                        else if (diff > _heartbeatdeadat)//���������ʱ��,�����������˵������ر�,�������˵�������Ӧ������������ʱ��,�����ǳ��� ���½�������
                        {
                            //debug("xxxxxxxxxxxxxxx diff:" + diff.ToString() + " dead:" + _heartbeatdeadat.ToString());
                            if (_reconnectreq == false)
                            {
                                _reconnectreq = true;//�����������ӱ�ʶ,�����ظ���������
                                logger.Warn(PROGRAME + ":heartbeat is dead, reconnecting at: " + DateTime.Now.ToString());
                                new Thread(reconnect).Start();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex);
                    }
                }
                //ע�ȴ�Ҫ�������,������Щ�����ֹͣ�˷���_started = false,���Ǹղż������ڵȴ����Ӷ���������ļ����̣�stop���Ӻ� �Զ�������
                Thread.Sleep(_wait * 10);//ÿ��������������ʱ��MS
               
            }
        }
        #endregion 


        #region ������� �߳� ���ڶ�ʱ�������������������
        bool _heartbeatgo = false;
        Thread _heartbeatthread = null;

        void _hbproc()
        {
            while (_heartbeatgo)
            {
                HeartBeat();
                Thread.Sleep(heartbeatperiod * 1000);
            }
        }
        private void StartHartBeat()
        {
            if (_heartbeatgo) return;
            _heartbeatgo = true;
            _heartbeatthread = new Thread(_hbproc);
            _heartbeatthread.IsBackground = true;
            _heartbeatthread.Start();
            logger.Info(PROGRAME + " :HeartBeatSend Backend threade started");
        }

        void StopHeartBeat()
        {
            if (!_heartbeatgo) return;
            _heartbeatgo = false;
            _heartbeatthread.Abort();
            _heartbeatthread = null;
            logger.Info(PROGRAME + " :HeartBeatSend Backend threade stoped");
        }
        #endregion

        #endregion 

        #region TLClient_IP ���캯��
        /// <summary>
        /// ÿ������ڵ����tick���ݷַ�/�ɽ����� ����ĵ������������ռ�ù����γɵ������
        /// ϵͳ���� ���ĳɽ������� + ��Χ�������������̨ �ͻ���ͨ����Χ������������Ծͽ����� ����һ̨������
        /// �Ϻ�һ̨������  �Ϸ�һ̨������ ���ķ����� ����̨�������������ͨѶ/�������������ʹ������� ֻҪ�����ٱ�֤���ɡ�
        /// ���������Ĺ���ֻ�ᷢ��������������������������⿪��4���˿� 5570 5571 5572 ssh�˿��ڴ�linux�����С�
        /// ���ķ���������Χ������ ͨ�� ����ǽ����ͨѶ��
        /// </summary>
        /// <param name="servers">ip��ַ�б�</param>
        /// <param name="srvport">����˿�</param>
        /// <param name="ClientName">�ͻ�����������</param>
        /// <param name="deb">��־����ص�</param>
        /// <param name="verbose">�Ƿ������ϸ��־</param>
        public TLClient_MQ(string[] servers, int srvport, string name)
        {
            PROGRAME = PROGRAME+"-"+name;
            logger = LogManager.GetLogger(PROGRAME);

            logger.Info(PROGRAME +" inited");

            port = srvport;//�������˿�
            foreach (string s in servers)//��������ַ
                serverip.Add(s);
        }

        #endregion




        #region Start Stop Section
        /// <summary>
        /// ��������
        /// </summary>
        public void Start()
        {
            
            logger.Info(PROGRAME + ":Start to connect to server....");

            bool _modesuccess = false;
            int _retry = 0;
            Stop();
            while (_modesuccess == false && _retry < _modeRetries)
            {
                _retry++;
                logger.Debug(PROGRAME + ":attempting connect to server... retry times:" + _retry.ToString());
                _modesuccess = Mode(_curprovider, false);//�������ӵ�һ���÷����,��һ��IP��ַ���з����ѯ��,�����÷���˷�����У����������ӵ�һ�������
                //�������������Mode������,����������������б�Ĺ���
                Thread.Sleep(_remodedelay * 1000);
            }
            if (!_modesuccess)
            {
                logger.Error(PROGRAME + ":�������,�޷����ӵ�������");
                throw new QSAsyncClientError();
            } 
        }
        /// <summary>
        /// �˳�,���������������clearclient��Ϣ
        /// </summary>
        public void Exit()
        {
            try
            {
                if (_mqcli != null && _mqcli.isConnected) //���ʵ���Ѿ�stop��brokerfeed ����ɷ�����ѭ����Ӧ��Ӧ�ý�_stated�������������Ӧ
                {
                    //TLSend(MessageTypes.CLEARCLIENT, Name);//�����������clearClient��Ϣ����ע���ͻ���
                }
            }
            catch (Exception ex)
            { 
                
            }
        }
        /// <summary>
        /// ֹͣ���ӷ���
        /// </summary>
        public void Stop(bool closeprocess=false)
        {
            logger.Info(PROGRAME + ":Stop TLCLient_MQ....");
            try
            {
                StopTickWatcher();//ֹͣtick���
                StopBW();//ֹͣ��̨�������
                StopHeartBeat();//ֹͣ��������
                if (_mqcli!=null && _mqcli.isConnected) //���ʵ���Ѿ�stop��brokerfeed ����ɷ�����ѭ����Ӧ��Ӧ�ý�_stated�������������Ӧ
                {

                    try
                    {
                        UnregisterClientRequest req = RequestTemplate<UnregisterClientRequest>.CliSendRequest(0);

                        TLSend(req);//�����������clearClient��Ϣ����ע���ͻ���
                        if (closeprocess) { Util.sleep(1000); System.Diagnostics.Process.GetCurrentProcess().Kill(); }
                        _mqcli.Disconnect();
                        markdisconnect();
                    }
                    catch (Exception ex){
                        logger.Error("stop mqcli error:" + ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error stopping TLClient_MQ " + ex.Message + ex.StackTrace);
            }
            finally
            {
                logger.Info(PROGRAME+":Realse asyncClient and thread resource");
                _mqcli = null;
                _bwthread = null;
                _heartbeatthread = null;
                _tickwatchthread = null;
                
            }
            logger.Info(PROGRAME+":Client:" + Name+" Disconnected");
        }


        #endregion


        #region ������Ͽ�����

        bool connect() { return connect(_curprovider != -1 ? _curprovider : 0); }//���ӵ���ǰ����˻����ǵ�һ�����
        bool connect(int providerindex) { return connect(providerindex, false); }
        /// <summary>
        /// ��ʼ��mqclient��������Ӧ������ͨ��
        /// </summary>
        /// <param name="providerindex"></param>
        /// <param name="showwarn"></param>
        /// <returns></returns>
        bool connect(int providerindex, bool showwarn)
        {
            logger.Info(PROGRAME + ":[connect] Connect to prvider....");
            if ((providerindex >= servers.Count) || (providerindex < 0))
            {
                logger.Warn(_skip + " Ensure provider is running and Mode() is called with correct provider number.   invalid provider: " + providerindex);
                return false;
            }

            try
            {
                logger.Debug(_skip+"Attempting connection to server: " + avabileip[providerindex]);
                //���ԭ�������Ӵ�� ���ȶϿ�����
                if ((_mqcli != null) && (_mqcli.isConnected))
                {
                    logger.Debug(_skip + "Disconnect old Connection...");
                    _mqcli.Disconnect();
                    markdisconnect();
                }
                bool acceptable = true;////false;
                /*
                try
                {
                    string r = AsyncClient.RequestAccept(avabileip[providerindex], port,"127.0.0.1" ,v);
                    if (r.Equals("OK"))
                        acceptable = true;
                }
                catch (QSNoServerException ex)
                {
                    debug(_skip + "Request accept error" + ex.ToString());
                    //����ڲ�ѯ����˵�ʱ����ִ�����������IP���,��������һ��IP�ķ���˼��
                    acceptable = false;
                }
                **/
                if (acceptable)
                {
                    //ʵ����asyncClient���󶨶��ѵĺ���
                    _mqcli = new AsyncClient(avabileip[providerindex], port);
                    _mqcli.SendTLMessage += new Action<Message>(handle);
                    //��ʼ��������
                    _mqcli.Start();
                    updateheartbeat();
                    if (_mqcli.isConnected)
                    {
                        // set our name ������ӵ�Ψһ��ʶ���
                        _name = _mqcli.ID;
                        // notify
                        logger.Info(_skip + "connected to server: " + serverip[providerindex] + ":" + this.port + " via:" + Name);
                        _reconnectreq = false;//עͨ��Mod���½������ӵĹ�����,�����̻߳�ֹͣ�� TLFound�����У���һֱ�ȴ����������ط�����
                        _recvheartbeat = true;
                        _requestheartbeat = true;
                        _connect = true;//�������ӱ�ʶ
                        _tickreconnectreq = false;
                        //��ʼ�������� ע��,����FeatureList,����汾��
                        InitConnection();
                    }
                    else
                    {
                        _connect = false;
                        logger.Info(_skip + "unable to connect to server at: " + serverip[providerindex].ToString());
                    }
                }
                else
                {
                    _connect = false;
                    logger.Info(_skip + "unable to connect to server at: " + serverip[providerindex].ToString());
                }

            }
            catch (Exception ex)
            {
                logger.Error(_skip+"exception creating connection to: " + serverip[providerindex].ToString() + ex.ToString());
                logger.Error(ex.Message + ex.StackTrace);
                _connect = false;
            }
            return _connect;
        }

        //�򵥵�ͨ���������»ָ��Ե�ǰ����˵�����
        //���ؿͻ��˵�С���⵼�µ�������ʱʧЧ ����ͨ���ٴγ�������ԭ���ķ���˽�������Ȼ��ָ�����ͨѶ
        //��������޷����� ���������������� ���з��������������Ļ��ƣ�����������ƽ�����TLFound��ѯIP�б���������Ч�ķ����.
        bool retryconnect()
        {
            logger.Info("     disconnected from server: " + serverip[_curprovider] + ", attempting reconnect...");
            bool rok = false;
            int count = 0;
            while (count++ < _disconnectretry)
            {
                rok = connect(_curprovider, false);
                if (rok)
                    break;
            }
            logger.Info(rok ? "reconnect suceeded." : "reconnect failed.");
            return rok;
        }

        void reconnect()
        {

            bool _modesuccess = false;
            int _retry = 0;
            Stop();
            while (_modesuccess == false && _retry < _modeRetries)
            {
                _retry++;
                logger.Info(PROGRAME + ":attempting reconnect... retry times:" + _retry.ToString());
                _modesuccess = Mode();//�������ӵ�һ���÷����,��һ��IP��ַ���з����ѯ��,�����÷���˷�����У����������ӵ�һ�������
                //�������������Mode������,����������������б�Ĺ���
                Thread.Sleep(_remodedelay * 1000);
            }
            if (!_modesuccess)
            {
                logger.Info(PROGRAME + ":�������,�޷����ӵ�������");
                //MessageBox.Show("�������,�޷����ӵ�������,���Ժ�F9���³���!");
                //throw new QSAsyncClientError();
            }
        }
        /// <summary>
        /// ��������tick���ݷ���
        /// </summary>
        void reconnectTick()
        {
            try
            {
                _mqcli.StopTickReciver();
                if (OnDataPubDisconnectEvent != null)
                    OnDataPubDisconnectEvent();
                _mqcli.StartTickReciver();
                _tickhartbeat = DateTime.Now.Ticks;

                if (OnDataPubConnectEvent != null)
                    OnDataPubConnectEvent();
                _tickreconnectreq = false;
            }
            catch (Exception ex)
            {
                logger.Error(PROGRAME + ":reconnect tick error:" + ex.ToString());
            }
        }

        /// <summary>
        /// �״�����tick���ݷ���
        /// </summary>
        void connectTick()
        {
            //debug("xxxxxxxxxxxxxxxxxxx ����tick��������...............",QSEnumDebugLevel.MUST);
            _mqcli.StopTickReciver();
            if (OnDataPubDisconnectEvent != null)
                OnDataPubDisconnectEvent();


            _mqcli.StartTickReciver(false);//����tickpublisher������
            _tickhartbeat = DateTime.Now.Ticks;//����ǰʱ���趨Ϊtickheartbeatʱ��
            tickenable = true;
            StartTickWatcher();
            //�����г����ݽ���֮��Żᴥ��datapubavabile,��������߼��ſ���ע���г�����
            if (OnDataPubConnectEvent != null)
                OnDataPubConnectEvent();

            _tickreconnectreq = false;
        }

        /// <summary>
        /// ��������ͨ��
        /// </summary>
        public void StartTick()
        {
            connectTick();
        }


        #endregion


        #region Mode ����Ѱ�ҿ��÷��� ����������
        /// <summary>
        /// Ĭ�ϴ����0��ʼ���ӷ�����
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <returns></returns>
        public bool Mode() { return Mode(0, true); }
        public bool Mode(int ProviderIndex, bool showwarning)
        {
            //1.�ڶ�Ӧ�ķ������б��в�ѯ���ṩ����ķ����
            logger.Info(PROGRAME + ":[Mode] Mode to Provider");
            TLFound();//��ѯ�ṩ��IP���Ƿ���ڶ�Ӧ�ķ�����Ӧ
            //��������Ч������ֱ�ӷ���
            if (servers.Count == 0)
            {
                logger.Info(PROGRAME + ": There is no any server avabile... try angain later");
                return false;
            }
            // see if called from start
            if (ProviderIndex < 0)
            {
                logger.Info("provider index cannot be less than zero, using first provider.");
                ProviderIndex = 0;
            }
            
            //2.��ʽ���������������,������½�ʵ�� ������һ���µĻỰ����
            bool ok = connect(ProviderIndex, false);//_mqcli��connect���洴��,���崴���߼���connect����
            
            //3.������ӵ���Ӧ�ķ������ɹ� ��������ά���߳������������߳�
            if (ok)
            {
                StartBW();
                StartHartBeat();
            }
            else
            {
                logger.Info("Unable to connect to provider: " + ProviderIndex);
                return false;
            }

            try
            {
                _curprovider = ProviderIndex;
                _bn = servers[_curprovider];
                //�����������ʼ����� ͳһ����Start�Ϳ�����ȷ���õ�����Ļص�
                //��ʼ��initconnection/������̨�߳���Ϻ󴥷��¼�
                if (OnConnectEvent != null)
                {
                    OnConnectEvent();
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + ex.StackTrace);

            }
            logger.Info("Server not found at index: " + ProviderIndex);
            return false;
        }

        /// <summary>
        /// ��ʼ������Э��ͨѶ
        /// </summary>
        void InitConnection()
        {
            logger.Info(PROGRAME + ":InitConnection......");
            // register ourselves with provider ע��
            Register();
            // request list of features from provider ������֧���б�
            RequestFeatures();
            //request server version;��ѯ�������汾
            ReqServerVersion();
            //�����ǵõ��������汾�� �����趨 _reconnectreq = false; ���� �Ͳ���һֱ��������
        }

        void debug(string msg)
        {
            logger.Info(msg);
        }
        /// <summary>
        /// ����ͨ��ip��ַ����ö�Ӧ��provider����,���Լ���Ƿ��ж�Ӧ�ķ������
        /// ע�� �������ݹ�����serverlistδ��ȫ����Ч,��TLFound��Ҫ����Ч�ı�������Ч���б���
        /// </summary>
        /// <returns></returns>
        public Providers[] TLFound()
        {
            logger.Info(PROGRAME + ":[TLFound] Searching provider list...");
            logger.Debug(_skip+"clearing existing list of available providers");
            servers.Clear();
            avabileip.Clear();
            // get name for every server provided by client
            //ע������Ҫ�����õķ����IP���������б�,�����ͻ��Զ������ӵ�������õķ����,
            foreach (string ip in serverip)
            {
                logger.Info(_skip+"Attempting to found server at : " + ip + ":" + port.ToString());
                //ֻ��Ҫ��asynclient��ĳ���ض���ip��ַ���͸�Ѱ����Ϣ����
                int pcode = (int)Providers.Unknown;
                //�����ӳٴ����˳����� �����Ϳ��Գ����������������� FIX ME
                try
                {
                    string r = AsyncClient.HelloServer(ip, port, debug);
                    logger.Info("got brokernameresponse:" + r);
                    pcode = Convert.ToInt16(r);
                }
                catch (QSNoServerException ex)
                {
                    logger.Info(_skip+"There is no service avabile at:" + ip);
                    //����ڲ�ѯ����˵�ʱ����ִ�����������IP���,��������һ��IP�ķ���˼��
                    continue;
                }
				//???? fix android ???????????,???????
				//??hello2server????????
				catch(Exception ex) {
					logger.Info (_skip+"HelloServer To Server error:"+ex.ToString());
					try
					{
						Thread.Sleep(100);
						string r = AsyncClient.HelloServer(ip, port,debug);
						pcode = Convert.ToInt16(r);
					}
					catch (QSNoServerException nex)
					{
						logger.Info(_skip+"There is no service avabile at:" + ip);
						//����ڲ�ѯ����˵�ʱ����ִ�����������IP���,��������һ��IP�ķ���˼��
						continue;
					}
					catch(Exception nex)
					{
						continue;
					}
				}
                //����������Ч���¼��provider����¼��Ӧ��server IP
                try
                {
                    Providers p = (Providers)pcode;
                    if (p != Providers.Unknown)
                    {
                        logger.Info(_skip+"Found provider: " + p.ToString() + " at: " + ip + ":" + port.ToString());
                        servers.Add(p);//�����õ�brokername���浽server��
                        avabileip.Add(ip);
                    }
                    else
                    {
                        logger.Info(_skip + "skipping unknown provider at: " + ip + ":" + port.ToString());
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(_skip+"error adding providing at: " + ip + ":" + port.ToString() + " pcode: " + pcode);
                    logger.Error(ex.Message + ex.StackTrace);
                }
            }
            logger.Info(_skip+"Total Found " + servers.Count + " providers.");
            return servers.ToArray();
        }
        #endregion


        #region TLSend
        MessageTypes[] _initfl = new MessageTypes[] { MessageTypes.REGISTERCLIENT,MessageTypes.CLEARCLIENT, MessageTypes.FEATUREREQUEST, MessageTypes.VERSIONREQUEST,MessageTypes.HEARTBEAT,MessageTypes.HEARTBEATREQUEST,MessageTypes.LOGINREQUEST };

        public long TLSend(IPacket packet)
        {
            try
            {

                if (_mqcli != null && _mqcli.isConnected)
                {
                    _mqcli.Send(packet.Data);

                    logger.Info(">>Send Packet type:" + packet.Type.ToString() + " content:" + packet.ToString());
        
                    return 0;
                }
                else
                {
                    //��������Ϣ�Ĺ����У������ǰ������Ч,�����ǳ������½�����ǰ����
                    //ע��:����heartbeat�̻߳�һ��ͨ��TLSend����������Ϣ��������,�����������ж�,�����������ﴥ�����½������ӵ�Ҫ��
                    if (_started)
                        retryconnect();
                    return 0;
                }
            }
            catch (Exception ex)
            {
                logger.Error("error sending packet " + packet.ToString());
                logger.Error(ex.Message + ex.StackTrace);
                return -1;
            }   
        }

        #endregion




        int requestid = 0;

        #region ��������---> TLClient ������TLServer��������Ĳ���
        /// <summary>
        /// ע��
        /// </summary>
        void Register()
        {
            logger.Info(PROGRAME + ":ע�ᵽ�����...");
            RegisterClientRequest req = RequestTemplate<RegisterClientRequest>.CliSendRequest(++requestid);
            TLSend(req);
        }
        /// <summary>
        /// �����������б�
        /// </summary>
        void RequestFeatures()
        {
            logger.Info(PROGRAME + ":�������˹����б�...");
            _rfl.Clear();
            FeatureRequest request = RequestTemplate<FeatureRequest>.CliSendRequest(++requestid);
            TLSend(request);
        }

        /// <summary>
        /// ����������汾
        /// </summary>
        void ReqServerVersion()
        {
            logger.Info(PROGRAME + ":�������˰汾...");
            VersionRequest request = RequestTemplate<VersionRequest>.CliSendRequest(++requestid);
            request.ClientVersion = "2.0";
            request.DeviceType = "PC";
            TLSend(request);
        }
        /// <summary>
        /// ����brokername
        /// </summary>
        void ReqBrokerName()
        {

        }


        //SymbolBasket _lastbasekt;
        //List<string> symlist = new List<string>();
        ///// <summary>
        ///// �����г�����
        ///// </summary>
        ///// <param name="mb"></param>
        //public void Subscribe(string[] symbols)
        //{
        //    //���mb=null ������������(���ķ����ߵ�ǰ����������������)
        //    if (symbols == null || symbols.Length == 0)
        //    {
        //        SubscribeAll_sub();//
        //        return;
        //    }
        //    else
        //    {
        //        //֪ͨ����˸ÿͻ�������symbol basket����
        //        //Unsubscribe();//�����ԭ����������
        //        logger.Info("TLClientע���Լ�б�:" + string.Join(",",symbols));
        //        List<string> newlist = new List<string>();
        //        foreach (string sym in newlist)
        //        {
        //            if (!symlist.Contains(sym))
        //            {
        //                newlist.Add(sym);
        //            }
        //        }
        //        foreach(string sym in newlist)
        //        {
        //            Subscribe_sub(sym);
        //        }
        //    }
        //}

        /// <summary>
        /// ����ĳ����Լ
        /// </summary>
        /// <param name="symbol"></param>
        public void Subscribe(string symbol)
        {
            if (_mqcli != null && _mqcli.isConnected)
            {
                _mqcli.Subscribe(symbol);//���ĺ�Լ

            }
        }
        /// <summary>
        /// ȡ������ĳ����Լ
        /// </summary>
        /// <param name="symbol"></param>
        public void UnSubscribe(string symbol)
        {
            if (_mqcli != null && _mqcli.isConnected)
            {
                _mqcli.UnSubscribe(symbol);//���ĺ�Լ

            }
        }

        //#region tick subscriber ���� �˶� �Ⱥ���
        //void Subscribe_sub(string symbol)
        //{
        //    if (_mqcli != null && _mqcli.isConnected)
        //    {
        //        _mqcli.Subscribe(symbol);//���ĺ�Լ

        //    }
        //}
        //void SubscribeAll_sub()
        //{
        //    if (_mqcli != null && _mqcli.isConnected)
        //    {
        //        _mqcli.SubscribeAll();

        //    }
        //}
        ///// <summary>
        ///// sub �� pubע��symbol��������
        ///// </summary>
        //void Unsubscribe_sub(string symbol)
        //{
        //    if (_mqcli != null && _mqcli.isConnected)
        //    {
        //        //_mqcli.Unsubscribe(symbol);
        //    }
        //}
        ///// <summary>
        ///// sub �� pubע��������������
        ///// </summary>
        //void UnsubscribeAll_sub()
        //{
        //    logger.Info("ע�������г�����...");
        //    if (_mqcli != null && _mqcli.isConnected)
        //    {
        //        if (_lastbasekt != null)
        //        {
        //            //foreach (Security sec in _lastbasekt)
        //            //{
        //            //    _mqcli.Unsubscribe(sec.Symbol);
        //            //}
        //        }

        //    }
        //}
        //#endregion


        ///// <summary>
        ///// ע���г�����
        ///// </summary>
        //public void Unsubscribe()
        //{
        //    //���߷�����������
        //    UnregisterSymbolTickRequest req = RequestTemplate<UnregisterSymbolTickRequest>.CliSendRequest(0);
        //    TLSend(req);
        //    UnsubscribeAll_sub();

        //}
        /// <summary>
        /// ����������Ӧ ���߷����� �ÿͻ��˴��
        /// </summary>
        /// <returns></returns>
        public void HeartBeat()
        {
            HeartBeat hb = RequestTemplate<HeartBeat>.CliSendRequest(0);
            TLSend(hb);
        }
        #endregion


        string _srvversion = string.Empty;
        string _uuid = string.Empty;

        #region �ͻ�����Ϣ�ر�������

        /// <summary>
        /// �ͻ�����Ӧ�汾��ѯ����ر�
        /// </summary>
        /// <param name="response"></param>
        void CliOnVersionResponse(VersionResponse response)
        {
            _srvversion = response.Version.BuildNum.ToString();
            _uuid = response.ClientID;
            logger.Info("Client got version response, version:" + _srvversion + " uuid:" + _uuid);
        }

        /// <summary>
        /// �ͻ�����Ӧ�������ѯ����ر�
        /// </summary>
        /// <param name="response"></param>
        void CliOnFeatureResponse(FeatureResponse response)
        {
            _rfl.Clear();
            foreach (MessageTypes mt in response.Features)
            {
                _rfl.Add(mt);
            }
            //����Ƿ�֧��tickȻ�����ǾͿ�������tickreceive
            //ֻ�е����������б�֧�����ݷ���ʱ,�����趨�ͻ���ͬʱ֧�� ���ݷ���ʱ TLClient_MQ�Ż�ע�ᵽ���ݷ����ַ
            //checkTickSupport();
        }

        /// <summary>
        /// �ͻ�����Ӧ��������ر�
        /// </summary>
        /// <param name="response"></param>
        void CliOnLoginResponse(LoginResponse response)
        {
            if (OnLoginResponse != null)
                OnLoginResponse(response);
        }
        /// <summary>
        /// �ͻ�����Ӧ��������ر�
        /// </summary>
        /// <param name="response"></param>
        void CliOnHeartbeatResponse(HeartBeatResponse response)
        {
            _recvheartbeat = !_recvheartbeat;
        }

        #endregion


        #region ���ܺ���

        void updateheartbeat()
        {
            _lastheartbeat = DateTime.Now.Ticks;
        }

        //�Ͽ����Ӻ�������Ҫ���б�ʶ������¼�
        void markdisconnect()
        {
            _connect = false;
            _mqcli.SendTLMessage -= new Action<Message>(handle);
            _mqcli = null;//��_mqcli��null �ڴ�Żᱻ����

            if (OnDisconnectEvent != null)
                OnDisconnectEvent();
        }

        //���з������Է��������Ӧ�ķ����֧��tick��������Ҫ��������tick���ݷ���
        //����ʹ�ò�ͬ�����������������Լ�����һ��Providerͬʱ�������ݺͽ��׵�Ҫ��ʱ,���ǵĽ�������Ҳ�����Featuresupport�Զ�ע�ᵽ����˵�Tick�ַ��ӿ�.������������Ҫ
        //��provider�����ͽ�����֤.��TLClient����Ӧ��������DataFeed����Execution�������֡��������ݾͲ���ӦΪ���ע�� ���Tick���ݵ��ظ�
        private void checkTickSupport()
        {
            //logger.Info(PROGRAME + ":Checing TickDataSupport...");
            //logger.Info(_skip+"providertype:" + ProviderType.ToString());
            //if (_rfl.Contains(MessageTypes.TICKNOTIFY) && (ProviderType == QSEnumProviderType.DataFeed || ProviderType == QSEnumProviderType.Both))
            //{
            //    logger.Info(_skip+"Spuuort Tick we subscribde tick data server");
            //    new Thread(connectTick).Start();
            //}
        }

        /// <summary>
        /// ���ͻ���API�汾������API�汾 ���ڰ汾���
        /// </summary>
        /// <param name="srvVersion"></param>
        /// <returns></returns>
        private bool checkVersion(int srvVersion)
        {
            //debug(PROGRAME + " :API�汾���...");
            if (srvVersion > Util.Version)
            {
                logger.Info(PROGRAME + " :API�汾���-->�����API");
                return false;
            }
            else
            {
                logger.Info(PROGRAME + " :API�汾���-->API����");
                return true;
            }
        }
        #endregion

        //��Ϣ�����߼�
        void handle(Message msg)
        {
            IPacket packet = PacketHelper.CliRecvResponse(msg);

                //logger.Debug(">>Recv Packet type:" + type.ToString() + " message:" + msg);
            MessageTypes type = msg.Type;
            
                if (packet.Type != MessageTypes.TICKHEARTBEAT && type != MessageTypes.HEARTBEATRESPONSE && type != MessageTypes.TICKNOTIFY)
                {

                    logger.Debug(">>Recv Packet type:" + type.ToString() + " message:" + msg);
                }
                switch (packet.Type)
                {
                    //���������ر�
                    case MessageTypes.TICKHEARTBEAT:
                        {
                            _tickhartbeat = DateTime.Now.Ticks;
                            //debug("tickheartbeat:"+_tickhartbeat.ToString(),QSEnumDebugLevel.MUST);
                            break;
                        }

                    //����ر�
                    case MessageTypes.LOGINRESPONSE:
                        {
                            updateheartbeat();
                            CliOnLoginResponse(packet as LoginResponse);
                        }
                        break;
                    //���������ر�
                    case MessageTypes.FEATURERESPONSE:
                        {
                            updateheartbeat();
                            CliOnFeatureResponse(packet as FeatureResponse);
                        }
                        break;
                    //�汾�ر�
                    case MessageTypes.VERSIONRESPONSE:
                        {
                            updateheartbeat();
                            CliOnVersionResponse(packet as VersionResponse);
                        }
                        break;
                    //��������ر�
                    case MessageTypes.HEARTBEATRESPONSE:
                        {
                            updateheartbeat();
                            CliOnHeartbeatResponse(packet as HeartBeatResponse);
                        }
                        break;
                    //�����߼����ݰ�
                    default:
                        {
                            updateheartbeat();
                            if (OnPacketEvent != null)
                            {
                                OnPacketEvent(packet);
                            }
                        }
                        break;
                }
        }
        
    }
}
