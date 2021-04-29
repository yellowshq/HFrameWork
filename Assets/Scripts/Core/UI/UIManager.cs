using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HFrameWork.Core
{
    public class UIManager : MonoSingletonBehavior<UIManager>
    {
        public Dictionary<string, BaseView> allViews;
        public Stack<BaseView> showStackViews;
        public Dictionary<string, BaseView> showViews;

        private Transform NormalRoot;
        private Transform FixedRoot;
        private Transform PopUpRoot;
        protected override void Init()
        {
            NormalRoot = transform.Find("Canvas/NormalRoot");
            FixedRoot = transform.Find("Canvas/FixedRoot");
            PopUpRoot = transform.Find("Canvas/PopUpRoot");

            allViews = new Dictionary<string, BaseView>();
            showStackViews = new Stack<BaseView>();
            showViews = new Dictionary<string, BaseView>();
        }

        public void ShowView(string viewName, params object[] args)
        {
            if (string.IsNullOrEmpty(viewName)) return;
            LoadView(viewName, baseView =>
            {
                baseView.transform.SetAsLastSibling();
                baseView.uiName = viewName;
                showView(baseView, args);
            });

        }

        private void showView(BaseView baseView, params object[] args)
        {
            switch (baseView.uiType)
            {
                case UIType.Normal:
                    ShowNormalView(baseView,args);
                    break;
                case UIType.Fixed:
                    break;
                case UIType.PopUp:
                    ShowPopUpView(baseView, args);
                    break;
                case UIType.Other:
                    break;
                default:
                    break;
            }
        }

        private void ShowNormalView(BaseView baseView, params object[] args)
        {
            if (showViews.ContainsKey(baseView.uiName)) return;
            showViews.Add(baseView.uiName, baseView);
            baseView.OnEnter(args);
        }

        private void ShowPopUpView(BaseView baseView, params object[] args)
        {
            if (showStackViews.Count > 0)
            {
                BaseView preView = showStackViews.Peek();
                preView.OnPause();
            }
            baseView.OnEnter(args);
            showStackViews.Push(baseView);
        }

        public void CloseOrReturnView(string viewName)
        {
            if (string.IsNullOrEmpty(viewName)) return;
            BaseView baseView = null;
            if (!allViews.TryGetValue(viewName, out baseView))
            {
                return;
            }
            switch (baseView.uiType)
            {
                case UIType.Normal:
                    ExitNormalView(baseView);
                    break;
                case UIType.Fixed:
                    break;
                case UIType.PopUp:
                    PopView();
                    break;
                case UIType.Other:
                    break;
            }
        }

        private void PopView()
        {
            if (showStackViews.Count <= 0) return;
            BaseView baseView = showStackViews.Pop();
            baseView.Hide();

            if (showStackViews.Count >= 2)
            { 
                BaseView showView = showStackViews.Peek();
                showView.Redisplay();
            }
        }

        private void ExitNormalView(BaseView showView)
        {
            if (!showViews.ContainsKey(showView.uiName)) return;
            showView.Hide();
            showViews.Remove(showView.uiName);
        }

        private void LoadView(string viewName, Action<BaseView> onfinished)
        {
            BaseView baseView;
            if (allViews.TryGetValue(viewName, out baseView))
            {
                onfinished(baseView);
                return;
            }
            AssetCacheManager.Instance.CacheObject<GameObject>(viewName, "view", go =>
            {
                GameObject viewGo = AssetCacheManager.Instance.CreateObject<GameObject>(viewName, "view");
                baseView = viewGo.GetComponent<BaseView>();
                SetViewRoot(baseView.uiType, viewGo.transform);
                allViews[viewName] = baseView;
                onfinished(baseView);
            });
        }

        private void SetViewRoot(UIType uiType,Transform viewTrans)
        {
            switch (uiType)
            {
                case UIType.Normal:
                    viewTrans.SetParent(NormalRoot);
                    break;
                case UIType.Fixed:
                    viewTrans.SetParent(FixedRoot);
                    break;
                case UIType.PopUp:
                    viewTrans.SetParent(PopUpRoot);
                    break;
                case UIType.Other:
                    break;
            }
        }
    }
}