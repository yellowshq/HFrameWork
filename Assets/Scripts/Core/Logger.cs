using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HFrameWork.Core
{
    public enum LogType:int
    {
        Info = 0,
        Warring = 1,
        Error = 2,
    }
    public static class Logger
    {
        public static int LogLevel = 0;
        public static void Log(string message,LogType logType = LogType.Info)
        {
            if ((LogType)LogLevel > logType)
            {
                return;
            }
            switch (logType)
            {
                case LogType.Info:
                    Debug.Log(message);
                    break;
                case LogType.Warring:
                    Debug.LogWarning(message);
                    break;
                case LogType.Error:
                    Debug.LogError(message);
                    break;
                default:
                    break;
            }
        }

        public static void LogInfo(string message)
        {
            Log(message, LogType.Info);
        }

        public static void LogWarring(string message)
        {
            Log(message, LogType.Warring);
        }

        public static void LogError(string message)
        {
            Log(message, LogType.Error);
        }
    }
}

