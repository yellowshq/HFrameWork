using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HFrameWork.Core
{
    public class Singleton<T> where T : Singleton<T>, new()
    {
        private static T _instance = null;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                }
                return _instance;
            }
        }

        protected Singleton()
        {
            Init();
        }

        protected virtual void Init() { }
    }
}
