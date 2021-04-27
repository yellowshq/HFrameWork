using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;

namespace HFrameWork.Core
{
    [ProtoContract]
    class Person
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
        [ProtoMember(3)]
        public Address Address { get; set; }
    }
    [ProtoContract]
    class Address
    {
        [ProtoMember(1)]
        public string Line1 { get; set; }
        [ProtoMember(2)]
        public string Line2 { get; set; }
    }

    [System.Serializable]
    public struct ProtoMap : ISerializationCallbackReceiver
    {
        public int msgID;
        public string msgName;
        public string pkgName;
        public string msgType;

        public void OnAfterDeserialize()
        {
            if (!string.IsNullOrEmpty(msgName) && !string.IsNullOrEmpty(pkgName))
            {
                msgType = string.Format("{0}.{1}", pkgName, msgName);
            }
        }

        public void OnBeforeSerialize()
        {
            
        }
    }
    [System.Serializable]
    public class ProtoMapArray: ISerializationCallbackReceiver
    {
        public List<ProtoMap> protoMaps;
        public Dictionary<int, ProtoMap> keyValues;

        /// <summary>
        /// 通过协议名字得到协议数据
        /// </summary>
        public Dictionary<string, ProtoMap> protoNameMaps;

        public void OnAfterDeserialize()
        {
            if (keyValues == null)
            {
                keyValues = new Dictionary<int, ProtoMap>();
            }
            if (protoNameMaps == null)
            {
                protoNameMaps = new Dictionary<string, ProtoMap>();
            }
            foreach (var item in protoMaps)
            {
                if (!keyValues.ContainsKey(item.msgID))
                {
                    keyValues.Add(item.msgID, item);
                }
                if (!protoNameMaps.ContainsKey(item.msgName))
                {
                    protoNameMaps.Add(item.msgName, item);
                }
            }
        }

        public void OnBeforeSerialize()
        {
          
        }
    }
    public class NetManager : MonoSingletonBehavior<NetManager>
    {

        private Dictionary<int, ProtoMap> protoMaps;
        private Dictionary<string, ProtoMap> protoNameMaps;


        protected override void Init()
        {
            //encode();
            //decode();
            //Connect();
        }

        public void InitProtoMap()
        {
            TextAsset protoMap = AssetCacheManager.Instance.LoadAsset<TextAsset>("protoMap", "public");
            Debug.Log(protoMap.text);
            ProtoMapArray proto = JsonUtility.FromJson<ProtoMapArray>(protoMap.text);
            protoMaps = proto.keyValues;
            protoNameMaps = proto.protoNameMaps;
            MsgDistribution.ProtoMaps = protoMaps;
        }

        byte[] data;
        private void encode()
        {
            Debug.Log("开始序列化数据");
            Person p = new Person { Address = new Address { Line1 = "中国", Line2 = "四川成都" }, Id = 1, Name = "YellowsHQ" };
            using(var stream = new MemoryStream())
            {
                Serializer.Serialize(stream, p);
                data = stream.ToArray();
            }
        }

        private void decode()
        {
            Debug.Log("开始反序列化数据");
            using(var stream = new MemoryStream(data))
            {
                var p = Serializer.Deserialize<Person>(stream);
                Debug.Log(p.Address.Line1 + p.Address.Line2);
            }
        }

        private Socket socket;
        private int buffCount = 0;
        private int msgLength = 0;
        /// <summary>
        /// Len(4字节) ID(4字节)
        /// </summary>
        private const int headLength = 8;
        private byte[] lenBytes = new byte[headLength];
        const int BUFFER_SIZE = 8192;
        byte[] readBuff = new byte[BUFFER_SIZE];

        private MsgDistribution distribution;
        private MsgDistribution MsgDistribution
        {
            get
            {
                if (distribution == null)
                {
                    distribution = new MsgDistribution();
                }
                return distribution;
            }
        }
        public void Connect()
        {
            if (socket == null)
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.ReceiveTimeout = 1000;
                socket.SendTimeout = 1000;
                socket.NoDelay = true;
            }
            string host = "127.0.0.1";
            int port = 7777;
            try
            {
                socket.BeginConnect(host, port,OnConnect,null);
            }
            catch(Exception e)
            {
                Close();
                Logger.LogError(e.Message);
            }
        }

        private void OnConnect(IAsyncResult ar)
        {
            try
            {
                socket.BeginReceive(readBuff, buffCount, BUFFER_SIZE-buffCount, SocketFlags.None, OnReceive, readBuff);
            }catch(Exception e)
            {
                Logger.LogError(e.Message);
                Close();
            }
        }

        public void Close()
        {
            try
            {
                if (socket != null)
                {
                    socket.Close();
                    socket = null;
                }
            }
            catch(Exception e)
            {
                Logger.LogError(e.Message);
            }
        }

        public ProtoMap GetMsgProto(string msgName)
        {
            if (protoNameMaps.ContainsKey(msgName))
            {
               return protoNameMaps[msgName];
            }
            Logger.LogError("protoNameMaps中没有对应的msgName " + msgName);
            return default;
        }

        public void SendMsg(string msgName,byte[] data)
        {
            ProtoMap proto = GetMsgProto(msgName);
            if (proto.msgID != 0)
            {
                int id = proto.msgID;
                SendMsg(id, data);
            }
        }

        public void SendMsg(int id,byte[] data)
        {
            if (!CheckSocket())
            {
                Logger.LogError("请检测网络");
                return;
            }
            Message message = new Message(id, data);
            byte[] sendDatas = DataPack.Pack(message);
            try
            {
                socket.Send(sendDatas);
            }
            catch { }
        }
        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                if (!CheckSocket())
                {
                    return;
                }
                int count = socket.EndReceive(ar);
                buffCount = buffCount + count;
                ProcessData();
                socket.BeginReceive(readBuff, buffCount, BUFFER_SIZE-buffCount, SocketFlags.None, OnReceive, readBuff);
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
                socket.Close();
            }
        }

        private bool CheckSocket()
        {
            return socket != null && socket.Connected;
        }

        private void ProcessData()
        {
            if (!CheckSocket())
            {
                return;
            }
            if (buffCount < headLength)
            {
                return;
            }
            if(buffCount < DataPack.GetPackDataLen(readBuff) + headLength)
            {
                return;
            }
            Message message = DataPack.Unpack(readBuff);
            lock (MsgDistribution.msgList)
            {
                MsgDistribution.msgList.Add(message);
            }
            Debug.Log(message);
            int count = buffCount - headLength - message.GetDataLen();
            Array.Copy(readBuff, headLength + message.GetDataLen(), readBuff, 0, count);
            buffCount = count;
            if (buffCount > 0)
            {
                ProcessData();
            }
        }

        private void Update()
        {
            MsgDistribution.Update();
        }

        private void OnDestroy()
        {
            Close();
        }
    }

}
