using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using COSXML;
using COSXML.Auth;
using COSXML.Model.Bucket;
using COSXML.Model.Object;
using COSXML.Utils;
using UnityEngine;

namespace CloudContentDelivery
{
    public class CosUtils
    {

        public static CosXml cosXml = getCosXml();

        public static void uploadSingle(string objectKey, string srcPath)
        {
            try
            {
                PutObjectRequest request = new PutObjectRequest(Parameters.CosBucket, objectKey, srcPath);
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
                request.SetCosProgressCallback(delegate (long completed, long total)
                {
                    Parameters.alreadyUploadSize4Current = completed;
                });

                checkAuth();
                PutObjectResult result = getCosXml().PutObject(request);
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Debug.LogError("CosClientException: " + clientEx);
                throw clientEx;
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Debug.LogError("CosServerException: " + serverEx.GetInfo());
                throw serverEx;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static string uploadPart(string objectKey, int partNum, string uploadId, string srcPath, long start, long contentLength)
        {
            try
            {
                UploadPartRequest uploadPartRequest = new UploadPartRequest(Parameters.CosBucket, objectKey, partNum, uploadId, srcPath, start, contentLength);
                uploadPartRequest.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
                uploadPartRequest.SetCosProgressCallback(delegate (long completed, long total)
                {
                    Parameters.alreadyUploadSize4Current = Parameters.alreadyUploadPartsSize4Current + completed;
                });

                checkAuth();
                UploadPartResult result = cosXml.UploadPart(uploadPartRequest);

                Parameters.alreadyUploadPartsSize4Current += contentLength;
                return result.eTag;
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Debug.LogError("CosClientException: " + clientEx);
                throw clientEx;
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Debug.LogError("CosServerException: " + serverEx.GetInfo());
                throw serverEx;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static string getUploadId(string objectKey)
        {
            try
            {
                InitMultipartUploadRequest request = new InitMultipartUploadRequest(Parameters.CosBucket, objectKey);
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);

                checkAuth();
                InitMultipartUploadResult result = cosXml.InitMultipartUpload(request);
                
                return result.initMultipartUpload.uploadId;
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Debug.LogError("CosClientException: " + clientEx);
                throw clientEx;
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Debug.LogError("CosServerException: " + serverEx.GetInfo());
                throw serverEx;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static List<COSXML.Model.Tag.ListParts.Part> getUploadsAlready(string objectKey, string uploadId)
        {
            try
            {
                ListPartsRequest request = new ListPartsRequest(Parameters.CosBucket, objectKey, uploadId);
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);

                checkAuth();
                ListPartsResult result = cosXml.ListParts(request);

                List<COSXML.Model.Tag.ListParts.Part> alreadyUploadParts = result.listParts.parts;
                return alreadyUploadParts;
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Debug.LogError("CosClientException: " + clientEx);
                throw clientEx;
            }

            catch (COSXML.CosException.CosServerException serverEx)
            {
                Debug.LogError("CosServerException: " + serverEx.GetInfo());
                throw serverEx;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public static void completeUpload(UploadParts uploadParts)
        {
            try
            {
                CompleteMultipartUploadRequest request = new CompleteMultipartUploadRequest(Parameters.CosBucket,
    uploadParts.objectKey, uploadParts.uploadId);
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);

                for (int i = 0; i < uploadParts.totalParts; i++)
                {
                    PartStructure part = uploadParts.parts[i];
                    request.SetPartNumberAndETag(part.partId, part.eTag);
                }

                checkAuth();
                CompleteMultipartUploadResult res = cosXml.CompleteMultiUpload(request);

                // Debug.Log(res.GetResultInfo());
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Debug.LogError("CosClientException: " + clientEx);
                throw clientEx;
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Debug.LogError("CosServerException: " + serverEx.GetInfo());
                throw serverEx;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public static CosXml getCosXml()
        {
            CosXmlConfig config = new CosXmlConfig.Builder()
                .SetConnectionTimeoutMs(60000)  //设置连接超时时间，单位毫秒，默认45000ms
                .SetReadWriteTimeoutMs(40000)  //设置读写超时时间，单位毫秒，默认45000ms
                .IsHttps(true)  //设置默认 HTTPS 请求
                .SetAppid(Parameters.CosAppId) //设置腾讯云账户的账户标识 APPID
                .SetRegion(Parameters.CosRegion) //设置一个默认的存储桶地域
                .Build();

            DefaultSessionQCloudCredentialProvider qCloudCredentialProvider = new DefaultSessionQCloudCredentialProvider(TemporaryAuth.secretId,
               TemporaryAuth.secretKey, TemporaryAuth.expiredTime, TemporaryAuth.token);

            return new CosXmlServer(config, qCloudCredentialProvider);
        }

        private static void checkAuth()
        {
            if (TemporaryAuth.isNearExpired())
            {
                try
                {
                    TemporaryAuth.refresh();
                    cosXml = getCosXml();
                }
                catch (Exception e)
                {
                    Debug.Log("Refresh temporary token failed.");
                    throw e;
                }
            }
        }
    }
}
