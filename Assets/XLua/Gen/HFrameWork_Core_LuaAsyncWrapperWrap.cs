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
    public class HFrameWorkCoreLuaAsyncWrapperWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(HFrameWork.Core.LuaAsyncWrapper);
			Utils.BeginObjectRegister(type, L, translator, 0, 2, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SendHttpRequest", _m_SendHttpRequest);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Invoke", _m_Invoke);
			
			
			
			
			
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
					
					HFrameWork.Core.LuaAsyncWrapper gen_ret = new HFrameWork.Core.LuaAsyncWrapper();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to HFrameWork.Core.LuaAsyncWrapper constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SendHttpRequest(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HFrameWork.Core.LuaAsyncWrapper gen_to_be_invoked = (HFrameWork.Core.LuaAsyncWrapper)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 7&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action<UnityEngine.Networking.UnityWebRequest>>(L, 4)&& translator.Assignable<UnityEngine.Networking.DownloadHandler>(L, 5)&& (LuaAPI.lua_isnil(L, 6) || LuaAPI.lua_type(L, 6) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 7)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 2);
                    string _method = LuaAPI.lua_tostring(L, 3);
                    System.Action<UnityEngine.Networking.UnityWebRequest> _onComplete = translator.GetDelegate<System.Action<UnityEngine.Networking.UnityWebRequest>>(L, 4);
                    UnityEngine.Networking.DownloadHandler _downloadHandler = (UnityEngine.Networking.DownloadHandler)translator.GetObject(L, 5, typeof(UnityEngine.Networking.DownloadHandler));
                    string _postData = LuaAPI.lua_tostring(L, 6);
                    int _timeout = LuaAPI.xlua_tointeger(L, 7);
                    
                    gen_to_be_invoked.SendHttpRequest( _url, _method, _onComplete, _downloadHandler, _postData, _timeout );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 6&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action<UnityEngine.Networking.UnityWebRequest>>(L, 4)&& translator.Assignable<UnityEngine.Networking.DownloadHandler>(L, 5)&& (LuaAPI.lua_isnil(L, 6) || LuaAPI.lua_type(L, 6) == LuaTypes.LUA_TSTRING)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 2);
                    string _method = LuaAPI.lua_tostring(L, 3);
                    System.Action<UnityEngine.Networking.UnityWebRequest> _onComplete = translator.GetDelegate<System.Action<UnityEngine.Networking.UnityWebRequest>>(L, 4);
                    UnityEngine.Networking.DownloadHandler _downloadHandler = (UnityEngine.Networking.DownloadHandler)translator.GetObject(L, 5, typeof(UnityEngine.Networking.DownloadHandler));
                    string _postData = LuaAPI.lua_tostring(L, 6);
                    
                    gen_to_be_invoked.SendHttpRequest( _url, _method, _onComplete, _downloadHandler, _postData );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 5&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action<UnityEngine.Networking.UnityWebRequest>>(L, 4)&& translator.Assignable<UnityEngine.Networking.DownloadHandler>(L, 5)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 2);
                    string _method = LuaAPI.lua_tostring(L, 3);
                    System.Action<UnityEngine.Networking.UnityWebRequest> _onComplete = translator.GetDelegate<System.Action<UnityEngine.Networking.UnityWebRequest>>(L, 4);
                    UnityEngine.Networking.DownloadHandler _downloadHandler = (UnityEngine.Networking.DownloadHandler)translator.GetObject(L, 5, typeof(UnityEngine.Networking.DownloadHandler));
                    
                    gen_to_be_invoked.SendHttpRequest( _url, _method, _onComplete, _downloadHandler );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action<UnityEngine.Networking.UnityWebRequest>>(L, 4)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 2);
                    string _method = LuaAPI.lua_tostring(L, 3);
                    System.Action<UnityEngine.Networking.UnityWebRequest> _onComplete = translator.GetDelegate<System.Action<UnityEngine.Networking.UnityWebRequest>>(L, 4);
                    
                    gen_to_be_invoked.SendHttpRequest( _url, _method, _onComplete );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 2);
                    string _method = LuaAPI.lua_tostring(L, 3);
                    
                    gen_to_be_invoked.SendHttpRequest( _url, _method );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.SendHttpRequest( _url );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to HFrameWork.Core.LuaAsyncWrapper.SendHttpRequest!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Invoke(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HFrameWork.Core.LuaAsyncWrapper gen_to_be_invoked = (HFrameWork.Core.LuaAsyncWrapper)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 5&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<System.Action>(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)&& translator.Assignable<UnityEngine.GameObject>(L, 5)) 
                {
                    float _delayTime = (float)LuaAPI.lua_tonumber(L, 2);
                    System.Action _onNext = translator.GetDelegate<System.Action>(L, 3);
                    bool _ignoreTimeScale = LuaAPI.lua_toboolean(L, 4);
                    UnityEngine.GameObject _go = (UnityEngine.GameObject)translator.GetObject(L, 5, typeof(UnityEngine.GameObject));
                    
                        System.Threading.CancellationTokenSource gen_ret = gen_to_be_invoked.Invoke( _delayTime, _onNext, _ignoreTimeScale, _go );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<System.Action>(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    float _delayTime = (float)LuaAPI.lua_tonumber(L, 2);
                    System.Action _onNext = translator.GetDelegate<System.Action>(L, 3);
                    bool _ignoreTimeScale = LuaAPI.lua_toboolean(L, 4);
                    
                        System.Threading.CancellationTokenSource gen_ret = gen_to_be_invoked.Invoke( _delayTime, _onNext, _ignoreTimeScale );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<System.Action>(L, 3)) 
                {
                    float _delayTime = (float)LuaAPI.lua_tonumber(L, 2);
                    System.Action _onNext = translator.GetDelegate<System.Action>(L, 3);
                    
                        System.Threading.CancellationTokenSource gen_ret = gen_to_be_invoked.Invoke( _delayTime, _onNext );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    float _delayTime = (float)LuaAPI.lua_tonumber(L, 2);
                    
                        System.Threading.CancellationTokenSource gen_ret = gen_to_be_invoked.Invoke( _delayTime );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to HFrameWork.Core.LuaAsyncWrapper.Invoke!");
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
