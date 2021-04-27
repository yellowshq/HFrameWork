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
    public class HFrameWorkCoreLuaManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(HFrameWork.Core.LuaManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 7, 3, 2);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InitLuaEnv", _m_InitLuaEnv);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadPbFile", _m_LoadPbFile);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadFile", _m_LoadFile);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "NewTable", _m_NewTable);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DoString", _m_DoString);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CheckRequire", _m_CheckRequire);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LaunchGame", _m_LaunchGame);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Global", _g_get_Global);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "luaEnv", _g_get_luaEnv);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "GCCount", _g_get_GCCount);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "luaEnv", _s_set_luaEnv);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "GCCount", _s_set_GCCount);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 2, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "CallGlobalFunction", _m_CallGlobalFunction_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					HFrameWork.Core.LuaManager gen_ret = new HFrameWork.Core.LuaManager();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to HFrameWork.Core.LuaManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitLuaEnv(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HFrameWork.Core.LuaManager gen_to_be_invoked = (HFrameWork.Core.LuaManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.InitLuaEnv(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadPbFile(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HFrameWork.Core.LuaManager gen_to_be_invoked = (HFrameWork.Core.LuaManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _fileName = LuaAPI.lua_tostring(L, 2);
                    
                        byte[] gen_ret = gen_to_be_invoked.LoadPbFile( _fileName );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadFile(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HFrameWork.Core.LuaManager gen_to_be_invoked = (HFrameWork.Core.LuaManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 5&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TTABLE)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action<bool, object[]>>(L, 4)&& (LuaAPI.lua_isnil(L, 5) || LuaAPI.lua_type(L, 5) == LuaTypes.LUA_TSTRING)) 
                {
                    XLua.LuaTable _scriptEnv = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
                    string _fileName = LuaAPI.lua_tostring(L, 3);
                    System.Action<bool, object[]> _onFinished = translator.GetDelegate<System.Action<bool, object[]>>(L, 4);
                    string _chunkName = LuaAPI.lua_tostring(L, 5);
                    
                    gen_to_be_invoked.LoadFile( _scriptEnv, _fileName, _onFinished, _chunkName );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TTABLE)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action<bool, object[]>>(L, 4)) 
                {
                    XLua.LuaTable _scriptEnv = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
                    string _fileName = LuaAPI.lua_tostring(L, 3);
                    System.Action<bool, object[]> _onFinished = translator.GetDelegate<System.Action<bool, object[]>>(L, 4);
                    
                    gen_to_be_invoked.LoadFile( _scriptEnv, _fileName, _onFinished );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TTABLE)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)) 
                {
                    XLua.LuaTable _scriptEnv = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
                    string _fileName = LuaAPI.lua_tostring(L, 3);
                    
                    gen_to_be_invoked.LoadFile( _scriptEnv, _fileName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to HFrameWork.Core.LuaManager.LoadFile!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_NewTable(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HFrameWork.Core.LuaManager gen_to_be_invoked = (HFrameWork.Core.LuaManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1) 
                {
                    
                        XLua.LuaTable gen_ret = gen_to_be_invoked.NewTable(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2)) 
                {
                    bool _setGlobalEnv = LuaAPI.lua_toboolean(L, 2);
                    
                        XLua.LuaTable gen_ret = gen_to_be_invoked.NewTable( _setGlobalEnv );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 1) 
                {
                    
                        XLua.LuaTable gen_ret = gen_to_be_invoked.NewTable(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to HFrameWork.Core.LuaManager.NewTable!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DoString(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HFrameWork.Core.LuaManager gen_to_be_invoked = (HFrameWork.Core.LuaManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _command = LuaAPI.lua_tostring(L, 2);
                    
                        object gen_ret = gen_to_be_invoked.DoString( _command );
                        translator.PushAny(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CallGlobalFunction_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _command = LuaAPI.lua_tostring(L, 1);
                    object[] _args = translator.GetParams<object>(L, 2);
                    
                        object gen_ret = HFrameWork.Core.LuaManager.CallGlobalFunction( _command, _args );
                        translator.PushAny(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CheckRequire(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HFrameWork.Core.LuaManager gen_to_be_invoked = (HFrameWork.Core.LuaManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _requireName = LuaAPI.lua_tostring(L, 2);
                    
                        bool gen_ret = gen_to_be_invoked.CheckRequire( _requireName );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LaunchGame(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HFrameWork.Core.LuaManager gen_to_be_invoked = (HFrameWork.Core.LuaManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.LaunchGame(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Global(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                HFrameWork.Core.LuaManager gen_to_be_invoked = (HFrameWork.Core.LuaManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Global);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_luaEnv(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                HFrameWork.Core.LuaManager gen_to_be_invoked = (HFrameWork.Core.LuaManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.luaEnv);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_GCCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                HFrameWork.Core.LuaManager gen_to_be_invoked = (HFrameWork.Core.LuaManager)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.GCCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_luaEnv(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                HFrameWork.Core.LuaManager gen_to_be_invoked = (HFrameWork.Core.LuaManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.luaEnv = (XLua.LuaEnv)translator.GetObject(L, 2, typeof(XLua.LuaEnv));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_GCCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                HFrameWork.Core.LuaManager gen_to_be_invoked = (HFrameWork.Core.LuaManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.GCCount = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
