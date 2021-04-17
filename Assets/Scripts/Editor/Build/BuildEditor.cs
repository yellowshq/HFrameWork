using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using System.Linq;
using UnityEditor.AddressableAssets.Build;
using System.Text;
using System;

public class BuildEditor
{
    public static string initRootPath = "Assets/Sources/Product/Init";
    public static string directoryRootPath = "Assets/Sources/Product/Directory";
    public static string singleRootPath = "Assets/Sources/Product/Single";

    [MenuItem("AddOn/Build/AutoSetGroup")]
    public static void AutoSetGroup()
    {
        GetAllDirectory(initRootPath,true);

        GetAllDirectory(directoryRootPath);

        GetSingleDirectory(singleRootPath);
    }

    public static void GetAllDirectory(string _path,bool isInit = false)
    {
        var guids = AssetDatabase.FindAssets("t:Object", new[] { _path });
        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            if (!string.IsNullOrEmpty(path) && Directory.Exists(path) && Directory.GetDirectories(path).Length<=0)
            {
                SetGroup(path,false, isInit);
            }
        }
    }

    public static void GetSingleDirectory(string _path)
    {
        SetGroup(_path,true);
    }

    static void SetGroup(string _path,bool isSingle = false, bool isInit = false)
    {
        var guids = AssetDatabase.FindAssets("t:Object", new[] { _path });
        string[] pa = _path.Split('/');
        string groupName = pa[pa.Length-1];
        AddressableAssetGroup group = CreateGroup(groupName, false, isSingle, isInit);

        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            if (!string.IsNullOrEmpty(path) && !Directory.Exists(path))
            {
                string address = Path.GetFileName(path);
                AddAssetEntry(group, guid, address);
            }
        }
    }

    static AddressableAssetEntry AddAssetEntry(AddressableAssetGroup group,string assetguid,string address)
    {
        AddressableAssetEntry assetEntry = group.entries.FirstOrDefault(e => assetguid == e.guid);
        if (assetEntry == null)
        {
            assetEntry = Settings.CreateOrMoveEntry(assetguid, group, false, false);
        }
        assetEntry.address = address;
        assetEntry.SetLabel(group.Name, true, false, false);
        return assetEntry;
    }

    static AddressableAssetSettings Settings
    {
        get
        {
            return AddressableAssetSettingsDefaultObject.Settings;
        }
    }

    static AddressableAssetGroup CreateGroup(string groupName,bool staticContent = false , bool isSingle = false,bool isInit = false)
    {
        AddressableAssetGroup group = Settings.FindGroup(groupName);
        if (group == null)
        {
            group = Settings.CreateGroup(groupName, true, false, false, null, typeof(ContentUpdateGroupSchema), typeof(BundledAssetGroupSchema));
            ContentUpdateGroupSchema schema = group.GetSchema<ContentUpdateGroupSchema>();
            schema.StaticContent = staticContent;
            BundledAssetGroupSchema buildSchema = group.GetSchema<BundledAssetGroupSchema>();
            buildSchema.BuildPath.SetVariableByName(Settings, isInit ? AddressableAssetSettings.kLocalBuildPath : AddressableAssetSettings.kRemoteBuildPath);
            buildSchema.LoadPath.SetVariableByName(Settings, isInit ? AddressableAssetSettings.kLocalLoadPath : AddressableAssetSettings.kRemoteLoadPath);
            buildSchema.BundleMode = isSingle ? BundledAssetGroupSchema.BundlePackingMode.PackSeparately : BundledAssetGroupSchema.BundlePackingMode.PackTogether;
        }
        Settings.AddLabel(groupName,false);
        return group;
    }


    [MenuItem("AddOn/Build/Build All Content")]
    public static void BuildAllContent()
    {
        AddressableAssetSettings.BuildPlayerContent();
        Debug.Log("BuildPlayerContent Finished");
    }

    [MenuItem("AddOn/Build/Prepare Update Content")]
    public static void CheckForUpdateContent()
    {
        try
        {
            string buildPath = ContentUpdateScript.GetContentStateDataPath(false);
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            List<AddressableAssetEntry> entrys = ContentUpdateScript.GatherModifiedEntries(settings, buildPath);
            if (entrys.Count != 0)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("Need Update Assets:");
                foreach (var item in entrys)
                {
                    stringBuilder.AppendLine(item.address);
                }
                Debug.Log(stringBuilder.ToString());
                var groupName = string.Format("UpdateGroup_{0}", DateTime.Now.ToString("yyyyMMdd"));
                ContentUpdateScript.CreateContentUpdateGroup(settings, entrys, groupName);
            }
        }
        finally
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    [MenuItem("AddOn/Build/BuildUpdate")]
    public static void BuildUpdate()
    {
        string buildPath = ContentUpdateScript.GetContentStateDataPath(false);
        var settings = AddressableAssetSettingsDefaultObject.Settings;
        AddressablesPlayerBuildResult result = ContentUpdateScript.BuildContentUpdate(settings, buildPath);
        Debug.Log("Build Finished path = " + settings.RemoteCatalogBuildPath.GetValue(settings));
    }
}
