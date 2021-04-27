#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class HFrameWorkCoreMsgCenterWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(HFrameWork.Core.MsgCenter);
			Utils.BeginObjectRegister(type, L, translator, 0, 4, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Subscribe", _m_Subscribe);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Unsubscribe", _m_Unsubscribe);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Broadcast", _m_Broadcast);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReInit", _m_ReInit);
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					HFrameWork.Core.MsgCenter gen_ret = new HFrameWork.Core.MsgCenter();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to HFrameWork.Core.MsgCenter constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Subscribe(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HFrameWork.Core.MsgCenter gen_to_be_invoked = (HFrameWork.Core.MsgCenter)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<System.Action<object>>(L, 3)&& translator.Assignable<object>(L, 4)) 
                {
                    int _msgId = LuaAPI.xlua_tointeger(L, 2);
                    System.Action<object> _action = translator.GetDelegate<System.Action<object>>(L, 3);
                    object _sender = translator.GetObject(L, 4, typeof(object));
                    
                    gen_to_be_invoked.Subscribe( _msgId, _action, _sender );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<System.Action<object>>(L, 3)) 
                {
                    int _msgId = LuaAPI.xlua_tointeger(L, 2);
                    System.Action<object> _action = translator.GetDelegate<System.Action<object>>(L, 3);
                    
                    gen_to_be_invoked.Subscribe( _msgId, _action );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to HFrameWork.Core.MsgCenter.Subscribe!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Unsubscribe(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HFrameWork.Core.MsgCenter gen_to_be_invoked = (HFrameWork.Core.MsgCenter)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<System.Action<object>>(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    int _msgId = LuaAPI.xlua_tointeger(L, 2);
                    System.Action<object> _action = translator.GetDelegate<System.Action<object>>(L, 3);
                    bool _removeAll = LuaAPI.lua_toboolean(L, 4);
                    
                    gen_to_be_invoked.Unsubscribe( _msgId, _action, _removeAll );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<System.Action<object>>(L, 3)) 
                {
                    int _msgId = LuaAPI.xlua_tointeger(L, 2);
                    System.Action<object> _action = translator.GetDelegate<System.Action<object>>(L, 3);
                    
                    gen_to_be_invoked.Unsubscribe( _msgId, _action );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    int _msgId = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.Unsubscribe( _msgId );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to HFrameWork.Core.MsgCenter.Unsubscribe!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Broadcast(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HFrameWork.Core.MsgCenter gen_to_be_invoked = (HFrameWork.Core.MsgCenter)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<object>(L, 3)) 
                {
                    int _msgId = LuaAPI.xlua_tointeger(L, 2);
                    object _args = translator.GetObject(L, 3, typeof(object));
                    
                    gen_to_be_invoked.Broadcast( _msgId, _args );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    int _msgId = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.Broadcast( _msgId );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to HFrameWork.Core.MsgCenter.Broadcast!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReInit(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HFrameWork.Core.MsgCenter gen_to_be_invoked = (HFrameWork.Core.MsgCenter)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ReInit(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
