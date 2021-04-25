using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace HFrameWork.Core
{
    public class MsgHandleHelper:Singleton<MsgHandleHelper>
    {
        private MsgHandler _msgHandler;
        private MsgHandler MsgHandler
        {
            get
            {
                if (_msgHandler == null)
                {
                    _msgHandler = new MsgHandler();
                }
                return _msgHandler;
            }
        }

        private Dictionary<int, MethodInfo> handlers = new Dictionary<int, MethodInfo>();

        private MethodInfo GetHandler(int id,string msgName)
        {
            if (handlers.ContainsKey(id))
            {
                return handlers[id];
            }
            string handlerName = msgName + "Handler";
            MethodInfo methodInfo = MsgHandler.GetType().GetMethod(handlerName);
            handlers[id] = methodInfo;
            return methodInfo;
        }
        public void CallHandler(ProtoMap proto,Message message)
        {
            MethodInfo handler = GetHandler(proto.msgID, proto.msgName);
            object[] parameters = new object[] { message.GetData() };
            handler.Invoke(MsgHandler, parameters);
        }
    }
}

