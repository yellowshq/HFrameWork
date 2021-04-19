using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace CloudContentDelivery
{
    public class EntryController
    {
        public static ParametersEntry pe = ParametersEntry.GetParametersEntry();
        public static ParametersBucket pb = ParametersBucket.GetParametersBucket();

        public static void LoadEntries(int page = 1)
        {
            if (!Util.checkCosKey() || string.IsNullOrEmpty(pb.selectedBucketUuid))
            {
                return;
            }

            if (page < 1)
            {
                return;
            }

            /*
            if (page > Parameters.totalEntryPages)
            {
                return;
            }
            */

            if (page == 1)
            {
                pe.entryPreviousButton = false;
            }
            else
            {
                pe.entryPreviousButton = true;
            }

            string url = string.Format("{0}api/v1/buckets/{1}/entries/?page={2}&per_page={3}", Parameters.apiHost, pb.selectedBucketUuid, page, Parameters.countPerpage);
            try
            {
                HttpResponse resp = HttpUtil.getHttpResponseWithHeaders(url, "GET");
                string entriesJson = resp.responseBody;
                string pagesPattern = Util.getHeader(resp.headers, "Content-Range"); // 1-10/14
                if (!string.IsNullOrEmpty(pagesPattern))
                {
                    pe.totalEntryCounts = int.Parse(pagesPattern.Split('/')[1]);
                    pe.totalEntryPages = ( pe.totalEntryCounts - 1 ) / 10 + 1;
                    if (page == pe.totalEntryPages)
                    {
                        pe.entryNextButton = false;
                    }
                    else
                    {
                        pe.entryNextButton = true;
                    }
                }

                pe.entryList = JsonUtility.FromJson<RootEntries>("{\"Entries\":" + entriesJson + "}").Entries;
                pe.entryNameList = new String[pe.entryList.Length];

                for (int i = 0; i < pe.entryList.Length; i++)
                {
                    pe.entryNameList[i] = pe.entryList[i].path;
                }

                if (pe.entryList.Length > 0)
                {
                    Debug.Log("Total " + pe.totalEntryCounts + " Entries");
                }
                else
                {
                    Debug.Log("No Entry for this bucket");
                }

                pe.currentEntryPage = page;
                pe.selectedEntryIndex = 0;
            }
            catch (Exception e)
            {
                Debug.LogError(string.Format("Load entries error : {0}.", e.Message));
            }
        }
        
        public static void ViewEntry()
        {
            if (pe.entryList.Length > 0)
            {
                EditorUtility.DisplayDialog("Entry Info", pe.entryList[pe.selectedEntryIndex].ToMessage(), "OK");
            }
        }

        public static void UploadFileManual(object pathObj)
        {
            if (string.IsNullOrEmpty(pb.selectedBucketUuid))
            {
                return;
            }

            ResetUploadParameters();
            Parameters.unfinishedUploadList = LoadOrCreateUnfinishedUploadFile();

            string filepath = (string) pathObj;

            Entry[] remoteEntries = null;
            string url = string.Format("{0}api/v1/buckets/{1}/entries/?page=0&per_page=1000", Parameters.apiHost, pb.selectedBucketUuid);
            try
            {
                string entriesJson = HttpUtil.getHttpResponse(url, "GET");
                remoteEntries = JsonUtility.FromJson<RootEntries>("{\"Entries\":" + entriesJson + "}").Entries;
            }
            catch (Exception e)
            {
                Debug.LogError(string.Format("Get remote entries error : {0}", e.Message));
                return;
            }

            EntryInfo ei = Util.getEntryInfoFromLocalFile(filepath);

            bool isUpdate = false;

            foreach (Entry entry in remoteEntries)
            {
                string entryPath = entry.path.StartsWith("/") ? entry.path.Substring(1) : entry.path;
                if (entryPath.Equals(ei.path))
                {
                    if (ei.content_hash.Equals(entry.content_hash))
                    {
                        Debug.Log("Current file already exist in remote bucket.");
                        return;
                    }
                    else
                    {
                        Debug.Log(string.Format("Update Entry {0}", entry.path));
                        try
                        {
                            isUpdate = true;
                            Parameters.totalUploadFiles = 1;
                            Parameters.totalUploadSize = ei.content_size;

                            ContentController.UploadContent(ei);
                            UpdateEntry(entry.entryid, ei);
                            DeleteEntryInUnfinished(ei.objectKey);
                            LoadEntries(pe.currentEntryPage);

                            Parameters.alreadyUploadFiles += 1;
                            Parameters.alreadyUploadSize += ei.content_size;

                            Debug.Log("Upload successfully");
                        }
                        catch (Exception e)
                        {
                            Debug.LogError(string.Format("Update entry for {0} failed - {1}", entry.path, e.Message));
                        }
                    }
                    break;
                }
            }

            if (!isUpdate)
            {
                Debug.Log(string.Format("Create Entry {0}", ei.path));
                try
                {
                    Parameters.totalUploadFiles = 1;
                    Parameters.totalUploadSize = ei.content_size;

                    ContentController.UploadContent(ei);
                    CreateEntry(ei);
                    DeleteEntryInUnfinished(ei.objectKey);
                    LoadEntries();

                    Parameters.alreadyUploadFiles += 1;
                    Parameters.alreadyUploadSize += ei.content_size;

                    Debug.Log("Upload successfully");
                }
                catch (Exception e)
                {
                    Debug.LogError(string.Format("Update entry for {0} failed - {1}", ei.path, e.Message));
                }
                
            }

            Parameters.syncFinished = true;
        }

        public static void SyncEntries(object pathObj)
        {

            if (string.IsNullOrEmpty(pb.selectedBucketUuid))
            {
                return;
            }
  
            ResetUploadParameters();
            string path = (string) pathObj;
            Debug.Log("Sync Entries in path : " + path);

            Entry[] remoteEntries = null;
            string url = string.Format("{0}api/v1/buckets/{1}/entries/?page=0&per_page=1000", Parameters.apiHost, pb.selectedBucketUuid);
            try
            {
                string entriesJson = HttpUtil.getHttpResponse(url, "GET");
                remoteEntries = JsonUtility.FromJson<RootEntries>("{\"Entries\":" + entriesJson + "}").Entries;
            }
            catch (Exception e)
            {
                Debug.LogError(string.Format("Get remote entries error : {0}", e.Message));
                return;
            }

            Parameters.unfinishedUploadList = LoadOrCreateUnfinishedUploadFile();

            Dictionary<string, EntryInfo> localFiles = Util.getLocalFiles(path);
            List<EntryInfo> createEntries = new List<EntryInfo>();
            Dictionary<string, EntryInfo> updateEntries = new Dictionary<string, EntryInfo>();

            foreach (Entry entry in remoteEntries)
            {
                string entryPath = entry.path.StartsWith("/") ? entry.path.Substring(1) : entry.path;
                if (localFiles.ContainsKey(entryPath))
                {
                    EntryInfo ei = localFiles[entryPath];
                    if (!ei.content_hash.Equals(entry.content_hash))
                    {
                        updateEntries.Add(entry.entryid, ei);
                        Parameters.totalUploadSize += ei.content_size;
                    }
                    localFiles.Remove(entryPath);
                }
            }

            foreach (KeyValuePair<string, EntryInfo> kvp in localFiles)
            {
                createEntries.Add(kvp.Value);
                Parameters.totalUploadSize += kvp.Value.content_size;
            }

            Parameters.totalUploadFiles = updateEntries.Count + createEntries.Count;

            foreach (EntryInfo entry in createEntries)
            {
                Debug.Log("Creating Entry : " + entry.path);
                try
                {
                    ContentController.UploadContent(entry);
                    CreateEntry(entry);
                    DeleteEntryInUnfinished(entry.objectKey);

                    Parameters.alreadyUploadFiles += 1;
                    Parameters.alreadyUploadSize += entry.content_size;
                }
                catch (Exception e)
                {
                    Debug.LogError(string.Format("Create entry for {0} failed - {1}", entry.path, e.Message));
                    Parameters.failedFiles++;
                }
            }

            foreach (KeyValuePair<string, EntryInfo> kvp in updateEntries)
            {
                Debug.Log("Updating Entry : " + kvp.Value.path);
                try
                {
                    ContentController.UploadContent(kvp.Value);
                    UpdateEntry(kvp.Key, kvp.Value);
                    DeleteEntryInUnfinished(kvp.Value.objectKey);

                    Parameters.alreadyUploadFiles += 1;
                    Parameters.alreadyUploadSize += kvp.Value.content_size;
                }
                catch (Exception e)
                {
                    Debug.LogError(string.Format("Update entry for {0} failed - {1}", kvp.Value.path, e.Message));
                    Parameters.failedFiles++;
                }
            }
            
            Parameters.syncFinished = true;
            Debug.Log("Sync Finished.");
            if (Parameters.failedFiles != 0)
            {
                Debug.LogError(string.Format("Fail to upload {0} entries.", Parameters.failedFiles));
            }
            else
            {
                Debug.Log(string.Format("All {0} entries upload successfully.", Parameters.totalUploadFiles));
            }
        }

        public static Entry CreateEntry(EntryInfo entryInfo)
        {
            string url = string.Format("{0}api/v1/buckets/{1}/entries/", Parameters.apiHost, pb.selectedBucketUuid);
            string requestBody = JsonUtility.ToJson(new EntryParams(entryInfo));

            string responseBody = HttpUtil.getHttpResponse(url, "POST", requestBody);
            return JsonUtility.FromJson<Entry>(responseBody);
        }

        public static Entry UpdateEntry(string entryId, EntryInfo entryInfo)
        {
            string url = string.Format("{0}api/v1/buckets/{1}/entries/{2}/", Parameters.apiHost, pb.selectedBucketUuid, entryId);
            string requestBody = JsonUtility.ToJson(new EntryParams(entryInfo));

            string responseBody = HttpUtil.getHttpResponse(url, "PUT", requestBody);
            return JsonUtility.FromJson<Entry>(responseBody);
        }

        public static void DeleteEntry(string entryId, string entryName = "")
        {
            if (string.IsNullOrEmpty(entryId) || string.IsNullOrEmpty(pb.selectedBucketUuid))
            {
                return;
            }
                   
            string url = string.Format("{0}api/v1/buckets/{1}/entries/{2}/", Parameters.apiHost, pb.selectedBucketUuid, entryId);

            try
            {
                string responseBody = HttpUtil.getHttpResponse(url, "DELETE");
                Debug.Log(string.Format("Delete Entry : {0}", entryName != "" ? entryName : entryId));
            }
            catch (Exception e)
            {
                EditorUtility.DisplayDialog("Delete Entry Error", e.Message, "OK");
                // Debug.LogError(string.Format("Create bucket error : {0}", e.Message));
            }

        }

        public static void ResetUploadParameters()
        {
            Parameters.syncFinished = false;
            Parameters.unfinishedUploadList = new UploadPartsStatusList();
            Parameters.unfinishedIndexObjetkeyMapping = new Dictionary<string, int>();

            Parameters.alreadyUploadSize = 0;
            Parameters.alreadyUploadFiles = 0;
            Parameters.totalUploadSize = 0;
            Parameters.totalUploadFiles = 0;
            Parameters.failedFiles = 0;

            Parameters.totalUploadSize4Current = 0;
            Parameters.alreadyUploadSize4Current = 0;
            Parameters.alreadyUploadPartsSize4Current = 0;

            if (false == Directory.Exists(Parameters.k_UploadPartStatusPathPrefix))
            {
                Directory.CreateDirectory(Parameters.k_UploadPartStatusPathPrefix);
            }
        }

        public static UploadPartsStatusList LoadOrCreateUnfinishedUploadFile()
        {
            if (File.Exists(Parameters.k_UploadPartStatusFile))
            {
                using (StreamReader sr = File.OpenText(Parameters.k_UploadPartStatusFile))
                {
                    string data = sr.ReadToEnd();

                    UploadPartsStatusList uploadPartsStatusList = JsonUtility.FromJson<UploadPartsStatusList>(data);
                    for (int i = 0; i < uploadPartsStatusList.unfinishedList.Count; i++)
                    {
                        Parameters.unfinishedIndexObjetkeyMapping.Add(uploadPartsStatusList.unfinishedList[i].objectKey, i);
                    }

                    return JsonUtility.FromJson<UploadPartsStatusList>(data);
                }

            }
            else
            {
                using (FileStream fs = new FileStream(Parameters.k_UploadPartStatusFile, FileMode.CreateNew))
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write("{}");
                    sw.Flush();
                }

                return new UploadPartsStatusList();
            }
        }

        public static void DeleteEntryInUnfinished(string objectKey)
        {
            if (Parameters.unfinishedIndexObjetkeyMapping.ContainsKey(objectKey))
            {
                int index = Parameters.unfinishedIndexObjetkeyMapping[objectKey];
                Parameters.unfinishedUploadList.unfinishedList.RemoveAt(index);
                Parameters.unfinishedIndexObjetkeyMapping.Remove(objectKey);

                WriteUnfinishedUploadsIntoFile();
            } 
        }

        public static void WriteUnfinishedUploadsIntoFile()
        {
            if (!File.Exists(Parameters.k_UploadPartStatusFile))
            {
                return;
            }

            File.WriteAllText(Parameters.k_UploadPartStatusFile, JsonUtility.ToJson(Parameters.unfinishedUploadList));
        }
    }
}
