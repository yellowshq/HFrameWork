using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace HFrameWork.Core
{
    public class Launcher : MonoSingletonBehavior<Launcher>
    {
        private async void Start()
        {
            await PreLoadAsset();
            LuaManager.Instance.LaunchGame();
        }

        public async Task PreLoadAsset()
        {
            await AssetCacheManager.Instance.LoadLuaAsync("LuaFile");
            await AssetCacheManager.Instance.LoadLuaAsync("LuaPb");
        }
    }
}

