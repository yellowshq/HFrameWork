using Cysharp.Threading.Tasks;
using HFrameWork.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ATest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //LuaAsyncWrapper.Instance.Invoke(2f, () =>
        //{
        //    Debug.Log("LuaAsyncWrapper CallBack");
        //});
        Delay(3);
    }

    private async void Delay(double delayTime)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(delayTime),true);
        Debug.Log("Delay Finished");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
