using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace HFrameWork.Core
{
    public class LoadingManager : MonoSingletonBehavior<LoadingManager>
    {
        private CanvasGroup canvasGroup;
        private Slider slider;
        private Text tipText;
        private Text percentText;
        private CompositeDisposable disposables;
        protected override void Init()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            disposables = new CompositeDisposable();
            slider = transform.Find("Slider").GetComponent<Slider>();
            percentText = transform.Find("percentText").GetComponent<Text>();
        }

        public void ShowLoadingProcess(string msg, AsyncOperationHandle handle)
        {
            if (handle.IsValid())
            {
                Logger.LogInfo(msg);
                canvasGroup.alpha = 1;
                showProgress(msg, 0);
                Observable.EveryUpdate().Subscribe(_ =>
                {
                    float percent = handle.PercentComplete;
                    showProgress(msg, percent);
                    if (handle.IsDone)
                    {
                        HideLoadingProcess();
                        disposables.Clear();
                    }
                }).AddTo(disposables);
            }
        }

        private void showProgress(string msg,float value)
        {
            slider.value = value;
            percentText.text = msg + Mathf.CeilToInt(value * 100f) + "%";
        }

        public void HideLoadingProcess()
        {
            canvasGroup.alpha = 0;
            disposables.Clear();
        }
    }
}
