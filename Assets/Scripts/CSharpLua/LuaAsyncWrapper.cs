using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;

namespace HFrameWork.Core
{
    public class LuaAsyncWrapper : Singleton<LuaAsyncWrapper>
    {

        public void SendHttpRequest(string url, string method = UnityWebRequest.kHttpVerbGET, Action<UnityWebRequest> onComplete = null, DownloadHandler downloadHandler = null, string postData = null, int timeout = 10)
        {
            UnityWebRequest request = null;
            if (method.Equals(UnityWebRequest.kHttpVerbGET))
            {
                request = UnityWebRequest.Get(url);
            }
            else if (method.Equals(UnityWebRequest.kHttpVerbPOST))
            {
                request = UnityWebRequest.Post(url, postData);
            }
            if (request == null)
            {
                onComplete?.Invoke(null);
                return;
            }
            if (downloadHandler != null)
            {
                request.downloadHandler = downloadHandler;
            }
            request.timeout = timeout;
            request.SendWebRequest().completed += operate =>
            {
                onComplete?.Invoke(request);
            };
        }

        public CancellationTokenSource Invoke(float delayTime, Action onNext = null, bool ignoreTimeScale = false, GameObject go = null)
        {
            if (go == null)
            {
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                InvokeInterval(delayTime, onNext, tokenSource.Token, ignoreTimeScale, go);
                return tokenSource;
            }
            else
            {
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                var token = go.GetCancellationTokenOnDestroy();
                InvokeInterval(delayTime, onNext, token, ignoreTimeScale, go);
                return CancellationTokenSource.CreateLinkedTokenSource(token, tokenSource.Token);
            }

        }

        private async void InvokeInterval(float interval, Action onNext = null, CancellationToken cancellationToken = default, bool ignoreTime = false, GameObject go = null)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(interval), ignoreTime, PlayerLoopTiming.Update, cancellationToken);
            onNext?.Invoke();
            onNext = null;
        }
    }
}

