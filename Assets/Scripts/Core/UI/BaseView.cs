using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HFrameWork.Core
{

    public enum UIType
    {
        Normal = 1,
        Fixed = 2,
        PopUp = 3,
        Other = 4,
    }

    public class BaseView : MonoBehaviour
    {
        public UIType uiType = UIType.Normal;
        public bool isClosed = false;
        public string uiName;
        public virtual void Awake()
        {
        }

        public virtual void OnEnter(params object[] obj)
        {

        }

        public virtual void OnEnable()
        {

        }

        public virtual void OnPause()
        {

        }

        public virtual void OnResume()
        {

        }

        public virtual void OnAfterClose()
        {
            isClosed = true;
        }

        public virtual void Hide()
        {

        }

        public virtual void Redisplay()
        {

        }

        public virtual void OnClose()
        {
            OnAfterClose();
        }

        public virtual void OnDestory()
        {

        }

        public virtual void CloseView()
        {
        }
    }
}