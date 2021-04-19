using System;
using System.Collections.Generic;
using UnityEngine;

namespace CloudContentDelivery
{
    [Serializable]
    public class Parameters
    {
        //constant parameters
        // public static string apiHost = "http://content-api-local.cloud.unity3d.com:22080/";
        public static string apiHost = "https://assetstreaming.unity.cn/";
        public static string proxyHost = "https://assetstream-content.unity.cn/";
        public static string contentType = "application/json";
        public static string k_CloudContentDeliverySettingsPathPrefix = "Assets/CloudContentDeliveryData/";
        public static string k_CloudContentDeliverySettingsPath = "Assets/CloudContentDeliveryData/CloudContentDeliverySettings.asset";
        public static string k_UploadPartStatusPathPrefix = "Assets/CloudContentDeliveryData/";
        public static string k_UploadPartStatusFile = "Assets/CloudContentDeliveryData/unfinishedUploads.json";

        public static string CosAppId = "1301029430";
        public static string CosRegion = "ap-shanghai";
        public static string CosBucket = "asset-streaming-1301029430";
        public static int maxRetries = 3;

        //static parameters
        public static List<string> ignoreFiles = new List<string>()
        {
            ".DS_Store"
        };
        
        public static string cosKey = CosKey.getCosKey();
        public static string oldCosKey = "";
        public static string projectGuid = CosKey.getProjectGuid();
        public static bool useOverseaConfig = CosKey.getUseOversea();

        public static int countPerpage = 10;

        public static UploadPartsStatusList unfinishedUploadList = new UploadPartsStatusList();
        public static Dictionary<string, int> unfinishedIndexObjetkeyMapping = new Dictionary<string, int>();

        public static bool syncFinished = true;
        public static long totalUploadSize = 0;
        public static long alreadyUploadSize = 0;
        public static int totalUploadFiles = 0;
        public static int alreadyUploadFiles = 0;

        public static long totalUploadSize4Current = 0;
        public static long alreadyUploadSize4Current = 0;
        public static long alreadyUploadPartsSize4Current = 0;

        public static int createdFiles = 0;
        public static int updatedFiles = 0;
        public static int failedFiles = 0;

        public static void ApplyConfigByEnvironment()
        {
            if (useOverseaConfig)
            {
                apiHost = "https://assetstreaming-oversea.unity.cn/";
                proxyHost = "https://asset-streaming-oversea-content.unity.cn/";
                CosRegion = "na-siliconvalley";
                CosBucket = "asset-streaming-oversea-1301029430";
            }
            else
            {
                // apiHost = "http://content-api-local.cloud.unity3d.com:22080/";
                apiHost = "https://assetstreaming.unity.cn/";
                proxyHost = "https://assetstream-content.unity.cn/";
                CosRegion = "ap-shanghai";
                CosBucket = "asset-streaming-1301029430";
            }
        }
    }
}