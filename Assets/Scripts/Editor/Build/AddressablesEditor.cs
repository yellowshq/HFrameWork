using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

public class AddressablesEditor
{
    private static string staticPath = "Assets/Product/LocalStatic";
    [MenuItem("AddressablesEditor/SetGroup/LocalStatic")]
    public static void SetStaticContentGroup()
    {
        try
        {
            string[] guids = AssetDatabase.FindAssets("t:Object", new string[] { staticPath });
            AddressableAssetSettings addressSettings = AddressableAssetSettingsDefaultObject.Settings;
            if (addressSettings)
            {
                foreach (var guid in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    if (File.Exists(path))
                    {
                        Debug.Log("path " + path);
                        //var asset = AssetDatabase.LoadAssetAtPath<Object>(path);
                        var localStatic = addressSettings.FindGroup("LocalStatic");
                        AddressableAssetEntry assetEntry = addressSettings.CreateOrMoveEntry(guid, localStatic);
                        string name = Path.GetFileNameWithoutExtension(path);
                        assetEntry.address = name;
                    }
                }
            }
        }
        finally{
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

    }

    [MenuItem("AddressablesEditor/Build All Content")]
    public static void BuildAllContent()
    {
        AddressableAssetSettings.BuildPlayerContent();
        Debug.Log("BuildPlayerContent Finished");
    }

    [MenuItem("AddressablesEditor/Prepare Update Content")]
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

    [MenuItem("AddressablesEditor/BuildUpdate")]
    public static void BuildUpdate()
    {
        string buildPath = ContentUpdateScript.GetContentStateDataPath(false);
        var settings = AddressableAssetSettingsDefaultObject.Settings;
        AddressablesPlayerBuildResult  result = ContentUpdateScript.BuildContentUpdate(settings, buildPath);
        Debug.Log("Build Finished path = " + settings.RemoteCatalogBuildPath.GetValue(settings));
    }
}
