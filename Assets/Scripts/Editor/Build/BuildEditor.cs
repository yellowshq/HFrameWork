using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using System.Linq;

public class BuildEditor
{
    public static string directoryRootPath = "Assets/Sources/Product/Directory";
    public static string singleRootPath = "Assets/Sources/Product/Single";

    [MenuItem("AddOn/Build/AutoSetGroup")]
    public static void AutoSetGroup()
    {
        GetAllDirectory(directoryRootPath);

        GetSingleDirectory(singleRootPath);
    }

    public static void GetAllDirectory(string _path)
    {
        var guids = AssetDatabase.FindAssets("t:Object", new[] { _path });
        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            if (!string.IsNullOrEmpty(path) && Directory.Exists(path) && Directory.GetDirectories(path).Length<=0)
            {
                SetGroup(path);
            }
        }
    }

    public static void GetSingleDirectory(string _path)
    {
        SetGroup(_path,true);
    }

    static void SetGroup(string _path,bool isSingle = false)
    {
        var guids = AssetDatabase.FindAssets("t:Object", new[] { _path });
        string[] pa = _path.Split('/');
        string groupName = pa[pa.Length-1];
        AddressableAssetGroup group = CreateGroup(groupName,false,isSingle);

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

    static AddressableAssetGroup CreateGroup(string groupName,bool staticContent = false , bool isSingle = false)
    {
        AddressableAssetGroup group = Settings.FindGroup(groupName);
        if (group == null)
        {
            group = Settings.CreateGroup(groupName, true, false, false, null, typeof(ContentUpdateGroupSchema), typeof(BundledAssetGroupSchema));
            ContentUpdateGroupSchema schema = group.GetSchema<ContentUpdateGroupSchema>();
            schema.StaticContent = staticContent;
            BundledAssetGroupSchema buildSchema = group.GetSchema<BundledAssetGroupSchema>();
            buildSchema.BuildPath.SetVariableByName(Settings, AddressableAssetSettings.kRemoteBuildPath);
            buildSchema.LoadPath.SetVariableByName(Settings, AddressableAssetSettings.kRemoteLoadPath);
            buildSchema.BundleMode = isSingle ? BundledAssetGroupSchema.BundlePackingMode.PackSeparately : BundledAssetGroupSchema.BundlePackingMode.PackTogether;
        }
        Settings.AddLabel(groupName,false);
        return group;
    }
}
