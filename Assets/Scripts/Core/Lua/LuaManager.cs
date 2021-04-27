using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;
using XLua.LuaDLL;

namespace HFrameWork.Core
{
    public class LuaManager : MonoSingletonBehavior<LuaManager>
    {
        public LuaEnv luaEnv;
        internal static float lastGCTime = 0;
        internal const float GCInterval = 1;//1 second
        public int GCCount = 30;
        private int count = 0;
        protected override void Init()
        {
            InitLuaEnv();
        }
        public void InitLuaEnv()
        {
            luaEnv = new LuaEnv();

#if UNITY_EDITOR
            var dir = Directory.GetCurrentDirectory();
            if (File.Exists(dir + "/emmy_core.dll"))
            {
                luaEnv.DoString("local dbg = require('emmy_core') dbg.tcpListen('localhost', 9966)" +
                              "print('启动lua')");
            }
#endif
            luaEnv.AddBuildin("pb", Lua.LoadPB);
            luaEnv.AddBuildin("pb.io", Lua.LoadPB_IO);
            luaEnv.AddBuildin("pb.conv", Lua.LoadPB_CONV);
            luaEnv.AddBuildin("pb.buffer", Lua.LoadPB_BUFFER);
            luaEnv.AddBuildin("pb.slice", Lua.LoadPB_SLICE);
            luaEnv.AddBuildin("rapidjson", Lua.LoadRapidJson);

            luaEnv.AddLoader(CustomLoaderMethod);
        }

        private byte[] CustomLoaderMethod(ref string fileName)
        {
            fileName = $"{fileName}.lua";

            return readFile(fileName);
        }

        private void Start()
        {
            //luaEnv.DoString("require 'testPb'");
        }

        void Update()
        {
            if (Time.time - lastGCTime > GCInterval && luaEnv != null)
            {
                luaEnv.Tick();
                count++;
                if (count >= GCCount && luaEnv.Memroy > 25000)
                {
                    //if (cache.CanGCCollect)
                    //{
                    //    luaEnv.FullGc();
                    //    count = 0;
                    //}
                }
                lastGCTime = Time.time;
            }
        }

        [LuaCallCSharp]
        public byte[] LoadPbFile(string fileName)
        {
            byte[] lua = AssetCacheManager.Instance.LoadPbFile(fileName);
            if (lua.Length > 0)
            {

            }
            return lua;
        }

        /// <summary>
        /// 加载lua脚本
        /// </summary>
        public void LoadFile(LuaTable scriptEnv, string fileName, Action<bool, object[]> onFinished = null, string chunkName = "chunk")
        {
            byte[] bytes = readFile(fileName);
            object[] objs;
            if (bytes == null)
            {
                Debug.Log($"{fileName}读取失败");
            }
            if (bytes.Length > 0)
            {
                objs = luaEnv.DoString(bytes, chunkName, scriptEnv);
                onFinished?.Invoke(true, objs);
            }
            else
            {
                onFinished?.Invoke(false, null);
            }
            onFinished = null;
            scriptEnv.Dispose(true);
            scriptEnv = null;
        }

        private byte[] readFile(string fileName)
        {
            return AssetCacheManager.Instance.LoadLuaFile(fileName);
        }

        public LuaTable Global
        {
            get
            {
                return luaEnv.Global;
            }
        }

        public LuaTable NewTable()
        {
            return luaEnv.NewTable();
        }

        public LuaTable NewTable(bool setGlobalEnv = false)
        {
            LuaTable luaTable = luaEnv.NewTable();
            LuaTable meta = luaEnv.NewTable();
            meta.Set("__index", luaEnv.Global);
            luaTable.SetMetaTable(meta);
            meta.Dispose();
            meta = null;
            return luaTable;
        }

        private void OnDestroy()
        {
            if (luaEnv!=null)
            {
                UniqueGameLoop.ClearAll();
                CSharpWrapper.Instance.Clear();
                luaEnv.DoString("require 'Utils.CheckRef_CSharp'");
                luaEnv.Dispose();
                luaEnv = null;
            }
        }

        public object DoString(string command)
        {
            return luaEnv.DoString(command);
        }

        public static object CallGlobalFunction(string command,params object[] args)
        {
            if (Instance.luaEnv == null)
            {
                return null;
            }
            var g = Instance.luaEnv.Global;
            var func = g.Get<Func<object[], object>>(command);
            if (func == null)
            {
                return null;
            }
            object ret = func(args);
            func = null;
            return ret;
        }

        public static T GetFunc<T>(string funcName)
        {
            var g = Instance.luaEnv.Global;
            var func = g.Get<T>(funcName);
            return func;
        }

        public bool CheckRequire(string requireName)
        {
            string fileName = $"{requireName}.lua";
            return AssetCacheManager.Instance.HasCached(fileName, "public");
        }

        public void LaunchGame()
        {
            LuaTable scriptTable = Instance.NewTable(false);
            Instance.LoadFile(scriptTable, "Core.Launcher.lua", (hasLua, objs) => {
                if (hasLua)
                {
                    Action action = scriptTable.Get<Action>("DoInit");
                    action?.Invoke();
                    action = null;
                }
                scriptTable.Dispose(true);
                scriptTable = null;
            }, "main");
        }
    }
}