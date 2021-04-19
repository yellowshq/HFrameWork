using System;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEditor.AddressableAssets;

namespace CloudContentDelivery
{
    public class AddressableUtil
    {
        public static bool CheckPassAddressableInit()
        {
            return AddressableAssetSettingsDefaultObject.Settings != null;
        }

        public static string getRemotebuildPath()
        {
            string profileId = AddressableAssetSettingsDefaultObject.Settings.activeProfileId;
            string pathPattern = AddressableAssetSettingsDefaultObject.Settings.profileSettings.GetValueByName(profileId, "RemoteBuildPath");
            return pathPattern.Replace("[BuildTarget]", UnityEditor.EditorUserBuildSettings.activeBuildTarget.ToString());
        }

        public static bool checkSelectedBuildPath()
        {
            string selectedBuildPath = AddressableAssetSettingsDefaultObject.Settings.RemoteCatalogBuildPath.GetName(AddressableAssetSettingsDefaultObject.Settings);

            if (String.IsNullOrEmpty(selectedBuildPath))
            {
                var buildpath = ((BundledAssetGroupSchema)AddressableAssetSettingsDefaultObject.Settings.DefaultGroup.GetSchema(typeof(BundledAssetGroupSchema))).BuildPath;
                selectedBuildPath = buildpath.GetName(AddressableAssetSettingsDefaultObject.Settings);
            }

            if (string.Equals(selectedBuildPath, "RemoteBuildPath") && Util.checkCosKey())
            {
                return true;
            }

            return false;
        }

        public static bool checkSelectedLoadPath()
        {
            string selectedLoadPath = AddressableAssetSettingsDefaultObject.Settings.RemoteCatalogLoadPath.GetName(AddressableAssetSettingsDefaultObject.Settings);

            if (String.IsNullOrEmpty(selectedLoadPath))
            {
                var loadpath = ((BundledAssetGroupSchema)AddressableAssetSettingsDefaultObject.Settings.DefaultGroup.GetSchema(typeof(BundledAssetGroupSchema))).LoadPath;
                selectedLoadPath = loadpath.GetName(AddressableAssetSettingsDefaultObject.Settings);
            }
            if (string.Equals(selectedLoadPath, "RemoteLoadPath") && Util.checkCosKey())
            {
                return true;
            }

            return false;
        }

        public static void setRemoteLoadPath(string remoteLoadPath)
        {
            string profileId = AddressableAssetSettingsDefaultObject.Settings.activeProfileId;
            AddressableAssetSettingsDefaultObject.Settings.profileSettings.SetValue(profileId, "RemoteLoadPath", remoteLoadPath);
        }
    }
}
