using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HFrameWork.Core
{
    public class SceneManager : MonoSingletonBehavior<SceneManager>
    {
        public void LoadScene(string name,Action action = null)
        {
            AssetCacheManager.Instance.LoadSceneAsync(name, action);
        }
    }
}

