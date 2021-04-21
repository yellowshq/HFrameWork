using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HFrameWork.Core
{
    public class MsgCenter : Singleton<MsgCenter>
    {
        private Dictionary<int, List<Action<object>>> m_MsgMap;
        protected override void Init()
        {
            m_MsgMap = new Dictionary<int, List<Action<object>>>();
        }

        public void Subscribe(int msgId, Action<object> action, object sender = null)
        {
            List<Action<object>> callbacks = null;
            if (!m_MsgMap.TryGetValue(msgId, out callbacks))
            {
                callbacks = new List<Action<object>>();
                m_MsgMap[msgId] = callbacks;
            }
            else
            {
                if (callbacks.Contains(action))
                {
                    Logger.LogWarring("MsgCenter Subscribe duplicate " + msgId);
                    return;
                }
            }
            callbacks.Add(action);
        }

        public void Unsubscribe(int msgId, Action<object> action = null, bool removeAll = false)
        {
            List<Action<object>> callbacks = null;
            if (m_MsgMap.TryGetValue(msgId, out callbacks))
            {
                if (action == null && removeAll)
                {
                    callbacks.Clear();
                    m_MsgMap.Remove(msgId);
                }
                else if (action != null)
                {
                    callbacks.Remove(action);
                }
            }
        }

        public void Broadcast(int msgId, object args = null)
        {
            List<Action<object>> callbacks = null;
            if (m_MsgMap.TryGetValue(msgId, out callbacks))
            {
                for (int i = callbacks.Count - 1; i >= 0; --i)
                {
                    callbacks[i](args);
                }
            }
        }

        public void ReInit()
        {
            foreach (var item in m_MsgMap)
            {
                item.Value.Clear();
            }
            m_MsgMap.Clear();
        }
    }
}

