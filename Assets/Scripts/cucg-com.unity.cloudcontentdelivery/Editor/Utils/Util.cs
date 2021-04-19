using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using COSXML;
using COSXML.Auth;
using COSXML.Model.Object;
using COSXML.Utils;
using UnityEditor;
using UnityEngine;

namespace CloudContentDelivery
{
    public class Util
    {
        public static Dictionary<string, string> contentTypeMapping = new Dictionary<string, string>
        {
            {".hash", "application/octet-stream"},
            {".json", "text/plain; charset=utf-8"},
            {".bundle", "application/octet-stream"}
        };
        
        public static Dictionary<string, EntryInfo> getLocalFiles(string rootPath)
        {
            Dictionary<string, EntryInfo> localFiles = new Dictionary<string, EntryInfo>();
            DirectoryInfo root = new DirectoryInfo(rootPath);
            FileInfo[] files = root.GetFiles();
                 
            foreach (FileInfo file in files)
            {
                string fullPath = file.FullName;
                string path = file.Name;
                long size = file.Length;
                string contentType = getContentTypeFromExtension(path);
                string hash = Util.getFiletHash(file.FullName);

                if (Parameters.ignoreFiles.Contains(path))
                {
                    continue;
                }

                EntryInfo entry = new EntryInfo(fullPath, path, hash, size, contentType);
                localFiles.Add(path, entry);
            }

            return localFiles;
        }

        public static EntryInfo getEntryInfoFromLocalFile(string filepath)
        {
            FileInfo file = new FileInfo(filepath);

            string fullPath = file.FullName;
            string path = file.Name;
            long size = file.Length;
            string contentType = getContentTypeFromExtension(path);
            string hash = getFiletHash(file.FullName);

            return new EntryInfo(fullPath, path, hash, size, contentType);

        }


        public static string getContentTypeFromExtension(string path)
        {
            string ext = Path.GetExtension(path);
            if (contentTypeMapping.ContainsKey(ext))
            {
                return contentTypeMapping[ext];
            }
            else
            {
                return "application/octet-stream";
            }
        }
        
        public static string getFiletHash(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(fs);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }

                return sb.ToString();
            }
        }

        /*
        
        public static void refreshProjectGuidJob()
        {
            System.Timers.Timer t = new System.Timers.Timer(3000);
            t.Elapsed += new System.Timers.ElapsedEventHandler(refreshProjectGuid);
            t.AutoReset = true;
            t.Enabled = true;
        }

        public static void refreshProjectGuid(object source, System.Timers.ElapsedEventArgs e)
        {
            // Debug.Log("Refreshing");
            if (!Parameters.oldCosKey.Equals(Parameters.cosKey))
            {
                Debug.Log("Refresh Project Info");
                Parameters.oldCosKey = Parameters.cosKey;
                Parameters.projectGuid = getProjectGuid();
                CosKey.SaveProjectGuid(Parameters.projectGuid);
            }
        }
        */

        public static string getProjectGuid()
        {
            if (Parameters.cosKey.Equals(""))
            {
                return "";
            }

            try
            {
                string responseBody = HttpUtil.getHttpResponse(Parameters.apiHost + "api/v1/users/me/projectguid", "GET");
                ProjectInfo projectInfo = JsonUtility.FromJson<ProjectInfo>(responseBody);
                Debug.Log(string.Format("Refresh project info successfully : {0}", projectInfo.projectGuid));
                return projectInfo.projectGuid;
            }
            catch (Exception e)
            {
                Debug.LogError(string.Format("Refresh project info failed : {0}", e.Message));
                return "";
            }
        }
        
        public static bool checkCosKey()
        {
            if (string.IsNullOrEmpty(Parameters.cosKey))
            {
                EditorUtility.DisplayDialog("Warning", "Please Set COS Key at Edit -> Project Settings -> Cloud Content Delivery!", "OK");
                return false;
            }

            return true;
        }

        public static string getHeader(WebHeaderCollection headers, string key)
        {
            for (int i = 0; i < headers.Keys.Count; i++)
            {
                if (headers.Keys[i].Equals(key))
                {
                    return headers[i];
                }
            }
            return "";
        }
    }
}
