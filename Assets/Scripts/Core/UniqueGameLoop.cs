using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HFrameWork.Core
{
    public class UniqueGameLoop : MonoSingletonBehavior<UniqueGameLoop>
    {
        private static readonly Dictionary<Action, int> _listeners = new Dictionary<Action, int>();
        private static readonly Queue<System.Action> _executeQueue = new Queue<Action>();
        private static int _index = 0;//队列第几次执行
        public const int MAX_UPDATE_COUNT = 20;//最多执行20次update，如果超过这个数，在下一帧执行
        private const float MAX_ALLOC_TIME = 0.03f;

        public static void AddListener(Action func,int frequeque)
        {
            if (!_listeners.ContainsKey(func))
            {
                _listeners.Add(func, frequeque);
            }
            else
            {
                _listeners[func] = frequeque;
            }
        }

        public static void RemoveListener(Action func)
        {
            if (_listeners.ContainsKey(func))
            {
                _listeners.Remove(func);
            }
        }

        public static void ClearAll()
        {
            _listeners.Clear();
        }

        private void Update()
        {
            ExcuteQueueUpdateFunctions();
        }

        private static void ExcuteQueueUpdateFunctions()
        {
            if (_executeQueue.Count == 0)
            {//队列已经执行完毕，重新生成队列
                foreach (var funcPair in _listeners)
                {
                    var frequence = funcPair.Value <= 0 ? 1 : funcPair.Value;
                    if (_index % frequence == 0)
                    {
                        _executeQueue.Enqueue(funcPair.Key);
                    }
                }
                _index++;
            }
            int allCount = _executeQueue.Count;
            float startTime = Time.realtimeSinceStartup;
            float taskTime = startTime;
            for (int i = 0; i < allCount; i++)
            {
                var func = _executeQueue.Dequeue();
                func?.Invoke();
                float now = Time.realtimeSinceStartup;
                if (now - startTime > MAX_ALLOC_TIME)
                {
                    break;
                }
            }
        }
    }
}

