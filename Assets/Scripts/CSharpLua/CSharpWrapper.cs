using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace HFrameWork.Core
{
    /// <summary>
    /// C#测调用Lua的方法
    /// </summary>
    public delegate int MsgHandlerDelegate(ProtoMap proto,Message message);
    public class CSharpWrapper : Singleton<CSharpWrapper>
    {
        private Dictionary<string, object> luaMethods = new Dictionary<string, object>();
        public void MsgHandler(ProtoMap proto, Message message)
        {
            var func = GetFunc<MsgHandlerDelegate>("MsgHandler");
            if (func!=null)
            {
                func(proto, message);
            }
        }

        private T GetFunc<T>(string methodName)
        {
            if (!luaMethods.ContainsKey(methodName))
            {
                var func = LuaManager.GetFunc<MsgHandlerDelegate>(methodName);
                luaMethods[methodName] = func;
            }
            return (T)luaMethods[methodName];
        }

        public void Clear()
        {
            luaMethods.Clear();
        }
    }
}
