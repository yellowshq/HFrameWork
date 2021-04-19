using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace HFrameWork.Core
{
    public class UpdateManager : MonoSingletonBehavior<UpdateManager>
    {
        public List<object> _updateKeys;
        public Slider slider;
        public Text percentText;
        private AsyncOperationHandle downloadHandle;

        public override void Init()
        {
            base.Init();
            Initialize();
        }

        private async void Initialize()
        {
            //初始化Addressables
            var initHandler = Addressables.InitializeAsync();
            await initHandler.Task;

            Caching.ClearCache();

            CheckUpdate();
        }

        private async void CheckUpdate()
        {

            _updateKeys = new List<object>();

            await UpdateCatalog();

            var downloadsize = Addressables.GetDownloadSizeAsync(_updateKeys);
            await downloadsize.Task;
            Logger.LogInfo("start download size :" + downloadsize.Result);

            if (downloadsize.Result > 0)
            {
                downloadHandle = Addressables.DownloadDependenciesAsync(_updateKeys, Addressables.MergeMode.Union);
                await UniTask.WaitUntil(() => downloadHandle.IsDone);
                //await downloadHandle .Task;
                Logger.LogInfo("download result type " + downloadHandle.Result.GetType());
                foreach (var item in downloadHandle.Result as List<UnityEngine.ResourceManagement.ResourceProviders.IAssetBundleResource>)
                {
                    var ab = item.GetAssetBundle();
                    Logger.LogInfo("ab name " + ab.name);
                    foreach (var name in ab.GetAllAssetNames())
                    {
                        Logger.LogInfo("asset name " + name);
                    }
                }
                Addressables.Release(downloadHandle);
            }
            else
            {
                Logger.LogInfo("已经是最新资源");
            }
            Addressables.Release(downloadsize);
            SceneManager.Instance.LoadScene("SampleScene");
        }

        public async Task UpdateCatalog()
        {
            //开始连接服务器检查更新
            var handle = Addressables.CheckForCatalogUpdates(false);
            await handle.Task;
            Logger.LogInfo("check catalog status " + handle.Status);
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
                        Logger.LogInfo("catalog  " + catalog);
                    }
                    Logger.LogInfo("download catalog start ");
                    var updateHandle = Addressables.UpdateCatalogs(catalogs, false);
                    await updateHandle.Task;
                    foreach (var item in updateHandle.Result)
                    {
                        Logger.LogInfo("catalog result " + item.LocatorId);
                        foreach (var key in item.Keys)
                        {
                            Logger.LogInfo("catalog key " + key);
                        }
                        _updateKeys.AddRange(item.Keys);
                    }
                    Logger.LogInfo("download catalog finish " + updateHandle.Status);
                    Addressables.Release(updateHandle);
                }
                else
                {
                    Logger.LogInfo("dont need update catalogs");
                }
            }
            Addressables.Release(handle);
        }

        public void Update()
        {
            if (downloadHandle.IsValid())
            {
                if (downloadHandle.PercentComplete < 1)
                {
                    UpdateProgressBar(downloadHandle.PercentComplete);
                }
                else if (downloadHandle.IsDone)
                {
                    UpdateProgressBar(1);
                }
            }
        }

        private void UpdateProgressBar(float percent)
        {
            slider.value = percent;
            percentText.text = Mathf.CeilToInt(percent * 100f) + "%";
        }
    }

}
