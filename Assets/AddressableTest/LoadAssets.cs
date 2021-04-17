using HFrameWork.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LoadAssets : MonoBehaviour
{
    private List<object> _updateKeys;

    void LoadAsset(string name)
    {
        //Addressables.LoadAssetAsync<GameObject>(name).Completed += OnLoadComplete;
        //AssetCacheManager.Instance.CacheObject<GameObject>(name, "Test", go=> {
        //    AssetCacheManager.Instance.CreateObject<GameObject>(name, "Test");
        //});
        //AssetCacheManager.Instance.LoadAndInstantiateAsync(name, "Test", go =>
        //{
        //    HFrameWork.Core.Logger.LogError("aaaaaaaaa");
        //});

        AssetCacheManager.Instance.CacheObjects(new string[] { "Cube", "Sphere" }, "Test", list =>
          {
              for (int i = 0; i < list.Count; i++)
              {
                  HFrameWork.Core.Logger.Log(list[i].name);
              }
              AssetCacheManager.Instance.CreateObject<GameObject>(name, "Test");
          });
    }

    private void OnLoadComplete(GameObject obj)
    {
        
    }

    private void OnLoadComplete(AsyncOperationHandle<GameObject> obj)
    {
        GameObject go = obj.Result;
        GameObject goInstance = Instantiate<GameObject>(go);
        StartCoroutine(WaiSenonds(5f, () =>
        {
            Destroy(goInstance);
            Addressables.ReleaseInstance(go);
        }));
    }

    IEnumerator WaiSenonds(float time,Action action)
    {
        yield return new WaitForSeconds(time);
        action?.Invoke();
    }

    public async void UpdateCatalog()
    {
        //开始连接服务器检查更新
        var handle = Addressables.CheckForCatalogUpdates(false);
        await handle.Task;
        Debug.Log("check catalog status " + handle.Status);
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            if (_updateKeys == null)
            {
                _updateKeys = new List<object>();
            }
            List<string> catalogs = handle.Result;
            if (catalogs != null && catalogs.Count > 0)
            {
                foreach (var catalog in catalogs)
                {
                    Debug.Log("catalog  " + catalog);
                }
                Debug.Log("download catalog start ");
                var updateHandle = Addressables.UpdateCatalogs(catalogs, false);
                await updateHandle.Task;
                foreach (var item in updateHandle.Result)
                {
                    Debug.Log("catalog result " + item.LocatorId);
                    foreach (var key in item.Keys)
                    {
                        Debug.Log("catalog key " + key);
                    }
                    _updateKeys.AddRange(item.Keys);
                }
                Debug.Log("download catalog finish " + updateHandle.Status);
            }
            else
            {
                Debug.Log("dont need update catalogs");
            }
        }
        Addressables.Release(handle);
    }

    public IEnumerator DownAssetImpl()
    {
        var downloadsize = Addressables.GetDownloadSizeAsync(_updateKeys);
        yield return downloadsize;
        Debug.Log("start download size :" + downloadsize.Result);

        if (downloadsize.Result > 0)
        {
            var download = Addressables.DownloadDependenciesAsync(_updateKeys, Addressables.MergeMode.Union);
            yield return download;
            //await download.Task;
            Debug.Log("download result type " + download.Result.GetType());
            foreach (var item in download.Result as List<UnityEngine.ResourceManagement.ResourceProviders.IAssetBundleResource>)
            {
                var ab = item.GetAssetBundle();
                Debug.Log("ab name " + ab.name);
                foreach (var name in ab.GetAllAssetNames())
                {
                    Debug.Log("asset name " + name);
                }
            }
            Addressables.Release(download);
        }
        Addressables.Release(downloadsize);
    }

    public void DownLoad()
    {
        StartCoroutine(DownAssetImpl());
    }

    private void OnGUI()
    {
        int y = 0;
        if(GUI.Button(new Rect(0, y, 100, 60), "加载Cube"))
        {
            LoadAsset("Cube");
        }
        y += 120;
        if (GUI.Button(new Rect(0, y, 100, 60), "加载Sphere"))
        {
            LoadAsset("Sphere");
        }
        y += 120;
        if (GUI.Button(new Rect(0, y, 100, 60), "加载 LocalNoStatic"))
        {
            LoadAsset("LocalNoStatic");
        }
        y += 120;
        if (GUI.Button(new Rect(0, y, 100, 60), "加载 RemoteStatic"))
        {
            LoadAsset("RemoteStatic");
        }
        y += 120;
        if (GUI.Button(new Rect(0, y, 100, 60), "加载 RemoteNoStatic"))
        {
            LoadAsset("RemoteNoStatic");
        }

        y += 120;
        if (GUI.Button(new Rect(0, y, 100, 60), "Release"))
        {
            AssetCacheManager.Instance.ReleaseGroup("Test");
        }

        y += 120;
        if (GUI.Button(new Rect(0, y, 100, 60), "Create Cube"))
        {
            AssetCacheManager.Instance.CreateObject<GameObject>("Cube", "Test");
        }


        if (GUI.Button(new Rect(200, 0, 100, 60), "UpdateCatalog"))
        {
            UpdateCatalog();
        }

        if (GUI.Button(new Rect(200, 120, 100, 60), "DownLoad"))
        {
            DownLoad();
        }


    }
}
