using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace CloudContentDelivery
{
    public class CosKey : ScriptableObject
    {
        [SerializeField] 
        private string cosKey;

        [SerializeField] 
        private string projectGuid;

        [SerializeField]
        private bool oversea;

        internal static CosKey GetOrCreateSetting()
        {
            if (false == Directory.Exists(Parameters.k_CloudContentDeliverySettingsPathPrefix))
            {
                Directory.CreateDirectory(Parameters.k_CloudContentDeliverySettingsPathPrefix);
            }
            var setting = AssetDatabase.LoadAssetAtPath<CosKey>(Parameters.k_CloudContentDeliverySettingsPath);
            if (setting == null)
            {
                setting = ScriptableObject.CreateInstance<CosKey>();
                setting.cosKey = "";
                setting.projectGuid = "";
                setting.oversea = false;
                AssetDatabase.CreateAsset(setting, Parameters.k_CloudContentDeliverySettingsPath);
                AssetDatabase.SaveAssets();
            }

            return setting;
        }

        internal static void SaveSetting(string cosKey)
        {
            var setting = AssetDatabase.LoadAssetAtPath<CosKey>(Parameters.k_CloudContentDeliverySettingsPath);
            setting.cosKey = cosKey;
            EditorUtility.SetDirty(setting);
        }

        public static string getCosKey()
        {
            return GetOrCreateSetting().cosKey;
        }

        internal static void SaveSetting(bool oversea)
        {
            var setting = AssetDatabase.LoadAssetAtPath<CosKey>(Parameters.k_CloudContentDeliverySettingsPath);
            setting.oversea = oversea;
            EditorUtility.SetDirty(setting);
        }

        public static bool getUseOversea()
        {
            return GetOrCreateSetting().oversea;
        }

        public static void SaveProjectGuid(string projectGuid) {
            var setting = AssetDatabase.LoadAssetAtPath<CosKey>(Parameters.k_CloudContentDeliverySettingsPath);
            setting.projectGuid = projectGuid;
            EditorUtility.SetDirty(setting);
        }

        public static string getProjectGuid() {
            return GetOrCreateSetting().projectGuid;
        }
        
        static class CloudContentDeliverySettingsIMGUIRegister
        {
            [SettingsProvider]
            public static SettingsProvider CreateCloudContentDeliverySettingsProvider()
            {
                var provider = new SettingsProvider("Project/Cloud Content Delivery", SettingsScope.Project)
                {
                    label = "Cloud Content Delivery",
                    guiHandler = (searchContext) =>
                    {
                        var setting = CosKey.GetOrCreateSetting();
                        setting.cosKey = EditorGUILayout.TextField("COS Key", setting.cosKey);
                        if (!setting.cosKey.Equals(Parameters.cosKey))
                        {
                            CosKey.SaveSetting(setting.cosKey);
                            Parameters.cosKey = setting.cosKey;
                            string projectGuid = Util.getProjectGuid();
                            CosKey.SaveProjectGuid(projectGuid);
                            Parameters.projectGuid = projectGuid;
                            TemporaryAuth.refresh();
                        }

                        setting.oversea = EditorGUILayout.Toggle("Use Oversea Config", setting.oversea);
                        if (!setting.oversea.Equals(Parameters.useOverseaConfig))
                        {
                            CosKey.SaveSetting(setting.oversea);
                            Parameters.useOverseaConfig = setting.oversea;
                            Parameters.ApplyConfigByEnvironment();
                        }

                        if (GUILayout.Button("Go To Cloud Content Delivery Website."))
                        {
                            Application.OpenURL("https://assetstreaming.unity.cn/onboarding/");
                        }
                    },
                    keywords = new HashSet<string>(new[] { "COS Key" })
                };
                
                AssetDatabase.SaveAssets();
                return provider;
            }
        }
    }
}