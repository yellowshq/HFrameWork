using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using XLua;

public static class XLuaTagConfig
{
    /// <summary>
    /// 指定需要做LuaCallCSharp标签的类
    /// </summary>
    [LuaCallCSharp]
    public static List<System.Type> LuaCallCSharpTypes
    {
        get
        {
            return new List<System.Type>
            {
                //UnityEngine
				typeof(object),
                typeof(UnityEngine.Object),
                typeof(Time),
                typeof(GameObject),
                typeof(Component),
                typeof(Behaviour),
                typeof(Transform),
                typeof(RectTransform),
                typeof(TextAsset),
                typeof(AnimationCurve),
                typeof(AnimationClip),
                typeof(MonoBehaviour),
                typeof(Renderer),
                typeof(RenderTexture),
                typeof(UnityEngine.Debug),
                typeof(Texture),
                typeof(Texture2D),
                typeof(PlayerPrefs),
                typeof(Mathf),
                typeof(Material),
                typeof(Color),
                typeof(UnityEngine.Vector2),
                typeof(UnityEngine.Vector3),
                typeof(UnityEngine.Quaternion),
                // typeof(Client.Library.StaticUtility),
                typeof(PlayableDirector),
                typeof(Screen),

                //ui
				typeof(UnityEngine.UI.RawImage),
                typeof(UnityEngine.UI.Image),
                typeof(UnityEngine.UI.Text),
                typeof(UnityEngine.UI.InputField),
                typeof(UnityEngine.UI.Dropdown),
                typeof(UnityEngine.UI.GridLayoutGroup),
                typeof(UnityEngine.UI.VerticalLayoutGroup),
                typeof(UnityEngine.UI.HorizontalLayoutGroup),
                typeof(UnityEngine.UI.HorizontalOrVerticalLayoutGroup),
                typeof(Canvas),
                typeof(CanvasGroup),
                typeof(CanvasRenderer),
                typeof(UnityEngine.Camera),
                typeof(UnityEngine.UI.Slider),
                typeof(UnityEngine.SceneManagement.SceneManager),

                //manager
                typeof(HFrameWork.Core.LuaManager),
                typeof(HFrameWork.Core.AssetCacheManager),
                typeof(HFrameWork.Core.SceneManager),
        };
        }
    }

    /// <summary>
    /// 指定需要做LuaCallCSharp标签的系统和引擎类等，参照xlua官方文档书写
    /// </summary>
    [LuaCallCSharp]
    public static List<System.Type> LuaCallCSharpSystemTypes
    {
        get
        {
            return new List<Type>() {
                typeof(Action),
                typeof(List<object>),
                typeof(UnityEngine.Events.UnityAction),

            };
        }
    }

    [GCOptimize]
    public static List<System.Type> GCOptimizeTypes
    {
        get
        {
            return new List<Type>() {
                typeof(Playable),
                typeof(DirectorWrapMode),
                typeof(DirectorUpdateMode),
            };
        }
    }

    [CSharpCallLua]
    public static List<System.Type> CSharpCallLuaSystemTypes
    {
        get
        {
            return new List<Type>() {
                typeof(System.Action),
                typeof(System.Action<int>),
                typeof(System.Action<float>),
                typeof(UnityEngine.Events.UnityAction),
                typeof(System.Func<int,Vector2>),

                typeof(System.Action<LuaTable>),
                typeof(System.Func<LuaTable, float>),
                typeof(System.Func<LuaTable, int>),
                typeof(System.Func<int, Vector2>),
                typeof(System.Func<LuaTable, bool>),
                typeof(System.Func<LuaTable, string>),
                typeof(System.Action<LuaTable, float>),
                typeof(System.Action<LuaTable, int>),
                typeof(System.Action<LuaTable, bool>),
                typeof(System.Action<LuaTable, string>),
                typeof(System.Action<LuaTable, Vector2>),
                typeof(System.Action<LuaTable, LuaTable>),
                typeof(System.Action<Texture>),
                typeof(System.Action<Collider>),
                typeof(System.Func<LuaTable, GameObject, LuaTable>),
                typeof(Func<LuaTable, GameObject, string>),
                typeof(Func<object[], object>),
                typeof(System.Func<LuaTable, string, GameObject, LuaTable>),
                typeof(System.Action<Vector3[]>),
            };
        }
    }

    [BlackList]
    public static List<List<string>> BlackListTypes
    {
        get
        {
            return new List<List<string>>()  {
                new List<string>(){"UnityEngine.UI.Graphic", "OnRebuildRequested"},
                new List<string>(){"UnityEngine.UI.Text", "OnRebuildRequested"},
                new List<string>(){"UnityEngine.UI.Image", "OnRebuildRequested"},
                new List<string>(){"UnityEngine.UI.RawImage", "OnRebuildRequested"},
                new List<string>(){"UnityEngine.WWW", "movie"},
				#if UNITY_WEBGL
				new List<string>(){"UnityEngine.WWW", "threadPriority"},
				#endif
				new List<string>(){"UnityEngine.Texture", "imageContentsHash"},
                new List<string>(){"UnityEngine.Texture2D", "alphaIsTransparency"},
                new List<string>(){"UnityEngine.Security", "GetChainOfTrustValue"},
                new List<string>(){"UnityEngine.CanvasRenderer", "onRequestRebuild"},
                new List<string>(){"UnityEngine.Light", "areaSize"},
                new List<string>(){"UnityEngine.AnimatorOverrideController", "PerformOverrideClipListCleanup"},
				#if !UNITY_WEBPLAYER
				new List<string>(){"UnityEngine.Application", "ExternalEval"},
				#endif
				new List<string>(){"UnityEngine.GameObject", "networkView"}, //4.6.2 not support
				new List<string>(){"UnityEngine.Component", "networkView"},  //4.6.2 not support
				new List<string>(){"System.IO.FileInfo", "GetAccessControl", "System.Security.AccessControl.AccessControlSections"},
                new List<string>(){"System.IO.FileInfo", "SetAccessControl", "System.Security.AccessControl.FileSecurity"},
                new List<string>(){"System.IO.DirectoryInfo", "GetAccessControl", "System.Security.AccessControl.AccessControlSections"},
                new List<string>(){"System.IO.DirectoryInfo", "SetAccessControl", "System.Security.AccessControl.DirectorySecurity"},
                new List<string>(){"System.IO.DirectoryInfo", "CreateSubdirectory", "System.String", "System.Security.AccessControl.DirectorySecurity"},
                new List<string>(){"System.IO.DirectoryInfo", "Create", "System.Security.AccessControl.DirectorySecurity"},
                new List<string>(){"UnityEngine.MonoBehaviour", "runInEditMode"},
                new List<string>(){"Animancer.AnimancerComponent", "AnimatorFieldName"},
                new List<string>(){"Animancer.AnimancerComponent", "InitialUpdateMode"},
                new List<string>(){"FairyGUI.UIPackage", "_loadFromAssetsPath"},
                new List<string>(){"UnityEngine.Light", "SetLightDirty"},
                new List<string>(){"UnityEngine.Light", "shadowRadius"},
                new List<string>(){"UnityEngine.Light", "shadowAngle"},
            };
        }
    }
}
