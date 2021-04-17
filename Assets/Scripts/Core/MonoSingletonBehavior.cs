using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HFrameWork.Core
{
    public class MonoSingletonBehavior<T> : MonoBehaviour where T : MonoSingletonBehavior<T>
    {
        private static T _instance = null;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance)
            {
                Destroy(this.gameObject);
                return;
            }
            _instance = this as T;
            DontDestroyOnLoad(this.gameObject);
            Init();
        }

        public virtual void Init()
        {

        }
    }
}