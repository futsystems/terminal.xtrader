using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace DataAPI.TDX
{
    public partial class TDXDataAPI
    {
        /// <summary>
        /// 向服务端提交一个请求
        /// 服务端对应给出一个返回
        /// </summary>
        /// <param name="request"></param>
        /// <param name="Len"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        bool Command(byte[] request, int Len, ref byte[] response)
        {
            bool rt = false;
            if (Senddata(request, Len))
                rt = RecvData(ref response);
            return rt;
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="v"></param>
        /// <param name="Len"></param>
        /// <returns></returns>
        bool Senddata(byte[] v, int Len)
        {
            if (m_hSocket == null)
            {
                logger.Error("Server is not connected");
                return false;
            }

            try
            {
                m_hSocket.Send(v, Len, SocketFlags.None);
                return true;
            }
            catch (Exception ex)
            {
                logger.Error("SendData error:" + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 接受数据
        /// </summary>
        /// <param name="recvbuf"></param>
        /// <returns></returns>
        bool RecvData(ref byte[] recvbuf)
        {
            if (m_hSocket == null)
            {
                logger.Error("Server is not connected");
                return false;
            }

            byte[] DHeader = new byte[16];
            recvbuf = null;
            string s1;
            int Len;
            try
            {
                Len = m_hSocket.Receive(DHeader, 16, SocketFlags.None);
                if (Len != 16)
                {
                    return false;
                }
                RecvDataHeader rhd = new RecvDataHeader();
                rhd = (RecvDataHeader)TDX.TDXDecoder.BytesToStuct(DHeader, 0, rhd.GetType());
                if (rhd.CheckSum != 7654321)
                {
                    return false;
                }

                byte[] dbuf = new byte[rhd.Size];
                int elen = rhd.Size;
                int fcur = 0;
                int Len1, min1 = 1024;
                while (fcur < elen)
                {
                    min1 = Math.Min(1024, elen - fcur);
                    Len1 = m_hSocket.Receive(dbuf, fcur, min1, SocketFlags.None);
                    if (Len1 > 0)
                        fcur += Len1;
                }
                if (fcur != elen)
                {
                    return false;
                }
                recvbuf = new byte[rhd.DePackSize + 1];
                if ((rhd.EncodeMode & 0x10) == 0x10)
                {
                    int LL = TDX.TDXDecoder.Decompress(dbuf, rhd.DePackSize, ref recvbuf);
                    if (LL != rhd.DePackSize)
                    {
                        s1 = "解压出错:长度不同=depacksize=" + rhd.DePackSize.ToString() + " 解压长度:=" + LL.ToString();
                        logger.Error(s1);
                    }
                }
                else
                {
                    dbuf.CopyTo(recvbuf, 0);
                }
                int t = 0;
                switch (rhd.msgid)
                {
                    case 0x526:
                    case 0x527: t = 0x39; break;
                    case 0x551: t = 0x49; break;
                    case 0x556: t = 0x69; break;
                    case 0x56e:
                    case 0x573: t = 0x77; break;
                }
                if (t > 0)
                {
                    for (int i = 0; i < rhd.DePackSize; i++)
                        recvbuf[i] = (byte)(recvbuf[i] ^ t);
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.Error("Recv Data Error:" + ex.ToString());
            }
            return false;
        }

    }
}
