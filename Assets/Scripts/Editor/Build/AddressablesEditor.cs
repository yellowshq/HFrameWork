using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

public class AddressablesEditor
{
    private static string staticPath = "Assets/Product/LocalStatic";
    [MenuItem("AddOn/Build/LocalStatic")]
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
}
