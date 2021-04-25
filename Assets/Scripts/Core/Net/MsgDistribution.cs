using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HFrameWork.Core
{
    public class MsgDistribution
    {
        /// <summary>
        /// 每帧处理的消息数量
        /// </summary>
        public int num=15;
        /// <summary>
        /// 消息列表
        /// </summary>
        public List<Message> msgList = new List<Message>();

        public Dictionary<int, ProtoMap> ProtoMaps { set; get; }

        public void Update()
        {
            for(int i = 0; i < num; i++)
            {
                if (msgList.Count > 0)
                {
                    DispatchMsgEvent(msgList[0]);
                    lock (msgList)
                    msgList.RemoveAt(0);
                }
                else
                {
                    break;
                }
            }
        }

        private void DispatchMsgEvent(Message message)
        {
            int id = message.GetMsgID();
            if (ProtoMaps.ContainsKey(id))
            {
                ProtoMap proto = ProtoMaps[id];
                MsgHandleHelper.Instance.CallHandler(proto, message);
            }
            else
            {
                Logger.LogError("ProtoMap中没有对应的Msg");
            }
        }
    }
}

