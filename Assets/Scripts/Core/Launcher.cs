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
            await UpdateManager.Instance.StartUpdate();
            Enter();
        }

        private async void Enter()
        {
            await PreLoadAsset();
            LuaManager.Instance.LaunchGame();
            SceneManager.Instance.LoadScene("SampleScene");
        }

        public async Task PreLoadAsset()
        {
            await AssetCacheManager.Instance.LoadLuaAsync(new string[] { "LuaFile" , "LuaPb" });
        }
    }
}

