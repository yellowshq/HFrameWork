using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class HToosEditor
{
    [MenuItem("Tools/ConvertProto")]
    public static void ConvertProto()
    {
        string protocPath = Application.dataPath + "/LuaProto/protoc/protoc.exe";
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            protocPath = "/usr/local/bin/protoc";
        }
        Debug.Log($" protocPath {protocPath}");
        string protoPath = Application.dataPath + "/LuaProto/proto";
        var dirInfo = new DirectoryInfo(protoPath);
        FileInfo[] protoFiles = dirInfo.GetFiles();
        foreach (var protoFile in protoFiles)
        {
            if (protoFile.FullName.EndsWith(".proto"))
            {
                string pbFile = protoFile.FullName.Replace(".proto", ".pb.bytes").Replace("\\", "/").Replace("/LuaProto/proto", "/Scripts/Lua/LuaPb");
                Debug.Log(pbFile);
                string param = string.Format("-I{0} {1} -o {2}", protoPath, protoFile, pbFile);
                var ans = RunProcessSync(protocPath, param);
                Debug.Log(ans);
            }
        }
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/GenProtoMap")]
    public static void GenProtoMap()
    {
        string gencPath = Application.dataPath + "/LuaProto/protoc/GenProtoMap.exe";
        Debug.Log($" protocPath {gencPath}");
        string protoPath = Application.dataPath + "/LuaProto/proto/";
        string targetPath = Application.dataPath + "/Scripts/Lua/LuaFile/Gen/protoMap.json";
        string param = string.Format("{0} {1}", protoPath, targetPath);
        var ans = RunProcessSync(gencPath, param);
        Debug.Log(ans);
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }

    public static string RunProcessSync(string cmd, string args)
    {
#if UNITY_EDITOR
        var startInfo = new System.Diagnostics.ProcessStartInfo();
        startInfo.FileName = cmd;
        startInfo.Arguments = args;
        startInfo.UseShellExecute = false;
        startInfo.CreateNoWindow = true;
        startInfo.RedirectStandardOutput = true;
        startInfo.RedirectStandardError = true;
        string ansStr, errStr;
        using (var process = System.Diagnostics.Process.Start(startInfo))
        {
            using (var reader = process.StandardOutput)
            {
                ansStr = reader.ReadToEnd();
            }
            using (var errReader = process.StandardError)
            {
                errStr = errReader.ReadToEnd();
            }
        }
        return string.Format("stdout: {0}\n stderr: {1}", ansStr, errStr);
#else
        return null;
#endif
    }
}
