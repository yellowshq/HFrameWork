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
    public class HFrameWorkCoreAssetCacheManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(HFrameWork.Core.AssetCacheManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 8, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CacheObjects", _m_CacheObjects);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadAndInstantiateAsync", _m_LoadAndInstantiateAsync);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadSceneAsync", _m_LoadSceneAsync);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReleaseGroup", _m_ReleaseGroup);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadLuaAsync", _m_LoadLuaAsync);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadLuaFile", _m_LoadLuaFile);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadPbFile", _m_LoadPbFile);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "HasCached", _m_HasCached);
			
			
			
			
			
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
					
					HFrameWork.Core.AssetCacheManager gen_ret = new HFrameWork.Core.AssetCacheManager();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to HFrameWork.Core.AssetCacheManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CacheObjects(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HFrameWork.Core.AssetCacheManager gen_to_be_invoked = (HFrameWork.Core.AssetCacheManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<string[]>(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action<System.Collections.Generic.IList<UnityEngine.Object>>>(L, 4)) 
                {
                    string[] _keys = (string[])translator.GetObject(L, 2, typeof(string[]));
                    string _group = LuaAPI.lua_tostring(L, 3);
                    System.Action<System.Collections.Generic.IList<UnityEngine.Object>> _onComplete = translator.GetDelegate<System.Action<System.Collections.Generic.IList<UnityEngine.Object>>>(L, 4);
                    
                    gen_to_be_invoked.CacheObjects( _keys, _group, _onComplete );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<string[]>(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)) 
                {
                    string[] _keys = (string[])translator.GetObject(L, 2, typeof(string[]));
                    string _group = LuaAPI.lua_tostring(L, 3);
                    
                    gen_to_be_invoked.CacheObjects( _keys, _group );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to HFrameWork.Core.AssetCacheManager.CacheObjects!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadAndInstantiateAsync(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HFrameWork.Core.AssetCacheManager gen_to_be_invoked = (HFrameWork.Core.AssetCacheManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action<UnityEngine.GameObject>>(L, 4)) 
                {
                    string _key = LuaAPI.lua_tostring(L, 2);
                    string _group = LuaAPI.lua_tostring(L, 3);
                    System.Action<UnityEngine.GameObject> _onComplete = translator.GetDelegate<System.Action<UnityEngine.GameObject>>(L, 4);
                    
                    gen_to_be_invoked.LoadAndInstantiateAsync( _key, _group, _onComplete );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)) 
                {
                    string _key = LuaAPI.lua_tostring(L, 2);
                    string _group = LuaAPI.lua_tostring(L, 3);
                    
                    gen_to_be_invoked.LoadAndInstantiateAsync( _key, _group );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to HFrameWork.Core.AssetCacheManager.LoadAndInstantiateAsync!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadSceneAsync(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HFrameWork.Core.AssetCacheManager gen_to_be_invoked = (HFrameWork.Core.AssetCacheManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action>(L, 3)) 
                {
                    string _key = LuaAPI.lua_tostring(L, 2);
                    System.Action _onComplete = translator.GetDelegate<System.Action>(L, 3);
                    
                    gen_to_be_invoked.LoadSceneAsync( _key, _onComplete );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _key = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.LoadSceneAsync( _key );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to HFrameWork.Core.AssetCacheManager.LoadSceneAsync!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReleaseGroup(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HFrameWork.Core.AssetCacheManager gen_to_be_invoked = (HFrameWork.Core.AssetCacheManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _group = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.ReleaseGroup( _group );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadLuaAsync(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HFrameWork.Core.AssetCacheManager gen_to_be_invoked = (HFrameWork.Core.AssetCacheManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<string[]>(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action<System.Collections.Generic.IList<UnityEngine.TextAsset>>>(L, 4)) 
                {
                    string[] _Labels = (string[])translator.GetObject(L, 2, typeof(string[]));
                    string _groupName = LuaAPI.lua_tostring(L, 3);
                    System.Action<System.Collections.Generic.IList<UnityEngine.TextAsset>> _onComplete = translator.GetDelegate<System.Action<System.Collections.Generic.IList<UnityEngine.TextAsset>>>(L, 4);
                    
                        System.Threading.Tasks.Task gen_ret = gen_to_be_invoked.LoadLuaAsync( _Labels, _groupName, _onComplete );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<string[]>(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)) 
                {
                    string[] _Labels = (string[])translator.GetObject(L, 2, typeof(string[]));
                    string _groupName = LuaAPI.lua_tostring(L, 3);
                    
                        System.Threading.Tasks.Task gen_ret = gen_to_be_invoked.LoadLuaAsync( _Labels, _groupName );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& translator.Assignable<string[]>(L, 2)) 
                {
                    string[] _Labels = (string[])translator.GetObject(L, 2, typeof(string[]));
                    
                        System.Threading.Tasks.Task gen_ret = gen_to_be_invoked.LoadLuaAsync( _Labels );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to HFrameWork.Core.AssetCacheManager.LoadLuaAsync!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadLuaFile(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HFrameWork.Core.AssetCacheManager gen_to_be_invoked = (HFrameWork.Core.AssetCacheManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)) 
                {
                    string _fileName = LuaAPI.lua_tostring(L, 2);
                    string _groupName = LuaAPI.lua_tostring(L, 3);
                    
                        byte[] gen_ret = gen_to_be_invoked.LoadLuaFile( _fileName, _groupName );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _fileName = LuaAPI.lua_tostring(L, 2);
                    
                        byte[] gen_ret = gen_to_be_invoked.LoadLuaFile( _fileName );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to HFrameWork.Core.AssetCacheManager.LoadLuaFile!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadPbFile(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HFrameWork.Core.AssetCacheManager gen_to_be_invoked = (HFrameWork.Core.AssetCacheManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)) 
                {
                    string _fileName = LuaAPI.lua_tostring(L, 2);
                    string _groupName = LuaAPI.lua_tostring(L, 3);
                    
                        byte[] gen_ret = gen_to_be_invoked.LoadPbFile( _fileName, _groupName );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _fileName = LuaAPI.lua_tostring(L, 2);
                    
                        byte[] gen_ret = gen_to_be_invoked.LoadPbFile( _fileName );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to HFrameWork.Core.AssetCacheManager.LoadPbFile!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HasCached(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HFrameWork.Core.AssetCacheManager gen_to_be_invoked = (HFrameWork.Core.AssetCacheManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _fileName = LuaAPI.lua_tostring(L, 2);
                    string _groupName = LuaAPI.lua_tostring(L, 3);
                    
                        bool gen_ret = gen_to_be_invoked.HasCached( _fileName, _groupName );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
