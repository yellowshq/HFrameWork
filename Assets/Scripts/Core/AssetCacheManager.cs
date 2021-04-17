using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace HFrameWork.Core
{
    public class CacheItem
    {
        public UnityEngine.Object obj;
        public List<UnityEngine.Object> instances;
        public string key = "";
        public bool isUse = false;
        public bool isLoadAndInstance = false;
        public AsyncOperationHandle<GameObject> handler;
        public bool isMerge = false;
        public AsyncOperationHandle<IList<UnityEngine.Object>> mergeHandler;
    }

    public class AssetCacheManager : MonoSingletonBehavior<AssetCacheManager>
    {
        private Dictionary<string, List<CacheItem>> cacheObjects;
        private List<CacheItem> cacheItems;
        public override void Init()
        {
            cacheItems = new List<CacheItem>(1024);
            cacheObjects = new Dictionary<string, List<CacheItem>>();
        }

        private void AddCache(string key ,string group, UnityEngine.Object obj,bool isLoadAndInstance=false, AsyncOperationHandle<GameObject> handler = default)
        {
            if (cacheObjects.ContainsKey(group) == false)
            {
                cacheObjects[group] = new List<CacheItem>();
            }
            List<CacheItem> caches = cacheObjects[group];
            int length = caches.Count;
            for (int i = 0; i < length; i++)
            {
                if(caches[i].key == key && caches[i].isLoadAndInstance == isLoadAndInstance)
                {
                    return;
                }
            }
            CacheItem cacheItem = GetCacheItem(key);
            cacheItem.obj = obj;
            cacheItem.isUse = true;
            cacheItem.key = key;
            cacheItem.isLoadAndInstance = isLoadAndInstance;
            cacheItem.handler = handler;
            cacheObjects[group].Add(cacheItem);
        }

        private void AddMergeCache(string key, string group, UnityEngine.Object obj, AsyncOperationHandle<IList<UnityEngine.Object>> handler = default)
        {
            if (cacheObjects.ContainsKey(group) == false)
            {
                cacheObjects[group] = new List<CacheItem>();
            }
            List<CacheItem> caches = cacheObjects[group];
            int length = caches.Count;
            for (int i = 0; i < length; i++)
            {
                if (caches[i].key == key)
                {
                    return;
                }
            }
            CacheItem cacheItem = GetCacheItem(key);
            cacheItem.obj = obj;
            cacheItem.isUse = true;
            cacheItem.key = key;
            cacheItem.isMerge = true;
            cacheItem.mergeHandler = handler;
            cacheObjects[group].Add(cacheItem);
        }

        private CacheItem GetCacheItem(string key)
        {
            int length = cacheItems.Count;
            for (int i = 0; i < length; i++)
            {
                if(cacheItems[i].isUse == false)
                {
                    return cacheItems[i];
                }
            }
            return new CacheItem() { instances = new List<UnityEngine.Object>() };
        }

        public async void CacheObject<T>(string key,string group,Action<T> onCompleted = null)
        {
            if(string.IsNullOrEmpty(key))
            {
                Logger.LogError("CacheObject Key 为空 ");
                return;
            }
            if (string.IsNullOrEmpty(group))
            {
                Logger.LogError("CacheObject Group 为空 ");
                return;
            }
            var handler = Addressables.LoadAssetAsync<T>(key);
            await handler.Task;
            AddCache(key, group, handler.Result as UnityEngine.Object);
            onCompleted?.Invoke(handler.Result);
        }

        public T CreateObject<T>(string key,string group) where T:UnityEngine.Object
        {
            if (cacheObjects.ContainsKey(group) == false)
            {
                Logger.LogError("缓存组中没有对应的Group " + group);
                return default;
            }
            List<CacheItem> caches = cacheObjects[group];
            foreach (var item in caches)
            {
                if(!string.IsNullOrEmpty(item.key) && item.key == key)
                {
                    UnityEngine.Object insObj = Instantiate(item.obj);
                    item.instances.Add(insObj);
                    return insObj as T;
                }
            }
            Logger.LogError("缓存组Group " + group + "中没有找到 " + key);
            return default;
        }

        public async void CacheObjects(string[] keys,string group,Action<IList<UnityEngine.Object>> onComplete =  null)
        {
            if (keys == null)
            {
                Logger.LogError("CacheObject Key 为空 ");
                return;
            }
            if (string.IsNullOrEmpty(group))
            {
                Logger.LogError("CacheObject Group 为空 ");
                return;
            }

            //List<UnityEngine.Object> objects = new List<UnityEngine.Object>();
            //int length = keys.Length;
            //for (int i = 0; i < length; i++)
            //{
            //    CacheObject<UnityEngine.Object>(keys[i], group,go => {
            //        objects.Add(go);
            //        if(i== length-1)
            //        {
            //            onComplete?.Invoke(objects);
            //        }
            //    });
            //}
            var handler = Addressables.LoadAssetsAsync<UnityEngine.Object>(keys, null, Addressables.MergeMode.Union);
            await handler.Task;
            int length = handler.Result.Count;
            for (int i = 0; i < length; i++)
            {
                AddMergeCache(keys[i], group, handler.Result[i], handler);
            }
            onComplete?.Invoke(handler.Result);
        }

        /// <summary>
        /// 加载并实例化GameObject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public async void LoadAndInstantiateAsync(string key,string group,Action<GameObject> onComplete=null)
        {
            if (string.IsNullOrEmpty(key))
            {
                Logger.LogError("CacheObject Key 为空 ");
                return;
            }
            if (string.IsNullOrEmpty(group))
            {
                Logger.LogError("CacheObject Group 为空 ");
                return;
            }
            var handler = Addressables.InstantiateAsync(key);
            await handler.Task;
            AddCache(key,group, handler.Result,true, handler);
            onComplete?.Invoke(handler.Result);
        }

        public void ReleaseGroup(string group)
        {
            if (cacheObjects.ContainsKey(group) == false)
            {
                Logger.LogError("缓存组中没有对应的Group " + group);
                return;
            }
            List<CacheItem> caches = cacheObjects[group];
            foreach (var item in caches)
            {
                if (item.isLoadAndInstance)
                {
                    Addressables.ReleaseInstance(item.handler);
                }
                else
                {
                    if (item.instances != null)
                    {
                        int length = item.instances.Count;
                        for (int i = 0; i < length; i++)
                        {
                            Destroy(item.instances[i]);
                        }
                        item.instances.Clear();
                    }
                    if (item.isMerge)
                    {
                        if (item.mergeHandler.IsValid())
                        {
                            Addressables.Release(item.mergeHandler);
                        }
                    }
                    else
                    {
                        if (item.obj != null)
                        {
                            Addressables.Release(item.obj);
                        }
                        item.obj = null;
                    }
                }
                item.isUse = false;
                item.key = "";
            }
            caches.Clear();
        }

    }
}

