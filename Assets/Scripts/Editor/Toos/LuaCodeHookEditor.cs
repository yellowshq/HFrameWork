using HFrameWork.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LuaCodeHookEditor : EditorWindow
{
    public string Luacode = "print(\"填入想要运行的代码\")";
    Vector2 scroll;

    [MenuItem("AddOn/测试/运行Lua代码")]
    public static void CreateWizard()
    {
        var window = EditorWindow.GetWindow<LuaCodeHookEditor>();
        window.titleContent = new GUIContent("运行lua代码");
        window.Show();
    }

    private void OnGUI()
    {
        if (!EditorApplication.isPlaying)
        {
            return;
        }
        GUILayout.BeginVertical();
        Luacode = EditorGUILayout.TextArea(Luacode, GUILayout.Height(100), GUILayout.Width(400));
        if (GUILayout.Button("执行lua的gm代码", GUILayout.Width(200)))
        {
            LuaManager.CallGlobalFunction("GMCommand", Luacode);
        }

        if (GUILayout.Button("执行Lua代码", GUILayout.Width(200)))
        {
            LuaManager.Instance.DoString(Luacode);
        }
        GUILayout.EndVertical();
    }
}
