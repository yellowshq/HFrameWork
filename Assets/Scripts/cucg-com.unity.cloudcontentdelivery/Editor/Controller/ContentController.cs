using System;
using System.Collections.Generic;
using COSXML.Model.Tag;
using UnityEngine;

namespace CloudContentDelivery
{
    public class ContentController
    {
        public static Dictionary<string, string> contentTypeMapping = new Dictionary<string, string>
        {
            {".hash", "application/octet-stream"},
            {".json", "text/plain; charset=utf-8"},
            {".bundle", "application/octet-stream"}
        };
        
        public static void UploadContent(EntryInfo entryInfo)
        {
            resetCurrentUploadParams(entryInfo);
            if (entryInfo.content_size < UploadConfig.divisionForUpload)
            {
                uploadSingle(entryInfo);
            }
            else
            {
                uploadMulti(entryInfo);
            }
        }

        private static void uploadSingle(EntryInfo entryInfo)
        {
            Debug.Log("Upload Single");
            CosUtils.uploadSingle(entryInfo.objectKey, entryInfo.full_path);
        }

        private static void uploadMulti(EntryInfo entryInfo)
        {
            Debug.Log("Upload Multi");

            string objectKey = entryInfo.objectKey;

            int index = -1;
            if (Parameters.unfinishedIndexObjetkeyMapping.ContainsKey(objectKey))
            {
                index = Parameters.unfinishedIndexObjetkeyMapping[objectKey];
            }
            UploadParts partsToUpload = SliceContent(entryInfo, index == -1 ? null : Parameters.unfinishedUploadList.unfinishedList[index].uploadId);

            partsToUpload = CheckUploadParts(partsToUpload);

            if (index == -1)
            {
                UploadPartsStatus ups = new UploadPartsStatus();
                ups.uploadId = partsToUpload.uploadId;
                ups.objectKey = objectKey;
                Parameters.unfinishedUploadList.unfinishedList.Add(ups);
                Parameters.unfinishedIndexObjetkeyMapping.Add(objectKey, Parameters.unfinishedUploadList.unfinishedList.Count - 1);
                EntryController.WriteUnfinishedUploadsIntoFile();
            }

            uploadParts(partsToUpload);
        }

        private static void uploadParts(UploadParts uploadParts)
        {
            
            for (int i=0; i < uploadParts.totalParts; i++)
            {
                PartStructure part = uploadParts.parts[i];
                if (part.hasUploaded)
                {
                    Parameters.alreadyUploadPartsSize4Current += part.partLength;
                    continue;
                }
                // Debug.Log("Upload Part : " + part.partId);
                string etag = CosUtils.uploadPart(uploadParts.objectKey, part.partId, uploadParts.uploadId,
                    uploadParts.fullPath, part.partStart, part.partLength);

                part.hasUploaded = true;
                part.eTag = etag;

            }

            CosUtils.completeUpload(uploadParts);
        }

        private static UploadParts CheckUploadParts(UploadParts uploadParts)
        {
            if (uploadParts.uploadId != null)
            {
                return ListMultiParts(uploadParts);
            }
            else
            {
                return InitMultiUploadPart(uploadParts);
            }
        }

        private static UploadParts ListMultiParts(UploadParts uploadParts)
        {
            List<ListParts.Part> alreadyUploadParts = CosUtils.getUploadsAlready(uploadParts.objectKey, uploadParts.uploadId);
            if (alreadyUploadParts != null)
            {
                Dictionary<int, string> remoteParts = new Dictionary<int, string>();
                foreach (ListParts.Part part in alreadyUploadParts)
                {
                    int num = -1;
                    bool parse = int.TryParse(part.partNumber, out num);
                    if (!parse) throw new ArgumentException("ListParts.Part parse error");

                    remoteParts.Add(num, part.eTag);
                }

                foreach (PartStructure part in uploadParts.parts)
                {
                    if (remoteParts.ContainsKey(part.partId))
                    {
                        part.hasUploaded = true;
                        part.eTag = remoteParts[part.partId];
                    }
                    else
                    {
                        part.hasUploaded = false;
                        part.eTag = null;
                    }
                }
            }

            return uploadParts;
        }

        private static UploadParts InitMultiUploadPart(UploadParts uploadParts)
        {
            string uploadId = CosUtils.getUploadId(uploadParts.objectKey);
            uploadParts.uploadId = uploadId;
            return uploadParts;
        }

        private static UploadParts SliceContent(EntryInfo entryInfo, string uploadId = null)
        {

            long size = entryInfo.content_size;

            List<PartStructure> parts = new List<PartStructure>();

            int i = 1;
            while (size > 0)
            {
                PartStructure part = new PartStructure();
                part.partId = i;
                part.hasUploaded = false;
                part.partStart = entryInfo.content_size - size;
                part.partLength = size > UploadConfig.sliceSizeForUpload ? UploadConfig.sliceSizeForUpload : size;
                part.partEnd = part.partStart + part.partLength - 1;
                part.eTag = null;
                parts.Add(part);

                size = size - UploadConfig.sliceSizeForUpload;
                i++;
            }

            UploadParts uploadParts = new UploadParts(entryInfo, parts);
            if (uploadId != null)
            {
                uploadParts.uploadId = uploadId;
            }
            return uploadParts;   
        }

        private static void resetCurrentUploadParams(EntryInfo entryInfo)
        {
            Parameters.totalUploadSize4Current = entryInfo.content_size;
            Parameters.alreadyUploadSize4Current = 0;
            Parameters.alreadyUploadPartsSize4Current = 0;
        }
    }
}
