using System;
using System.Net;
using System.Text;
using System.IO;

using UnityEngine;

namespace CloudContentDelivery
{
    public class HttpUtil
    {
        public static HttpWebRequest setHttpHeader(string url, string method, string requestBody = null)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            byte[] coskey = Encoding.ASCII.GetBytes(Parameters.cosKey + ":");

            request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(coskey));
            request.ContentType = Parameters.contentType;
            request.Method = method;

            if (requestBody != null)
            {
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] bodyByte = encoding.GetBytes(requestBody);
                request.ContentLength = bodyByte.Length;

                Stream newStream = request.GetRequestStream();
                newStream.Write(bodyByte, 0, bodyByte.Length);
                newStream.Close();
            }

            return request;
        }

        public static string getHttpResponse(string url, string method, string requestBody = null)
        {
            int i = 0;
            Exception ex = null;
            while (i < Parameters.maxRetries)
            {
                HttpWebRequest request = setHttpHeader(url, method, requestBody);
                try
                {
                    return getHttpResponseInternal(request);
                }
                catch (Exception e)
                {
                    Debug.LogError("Http Error : " + e.Message);
                    ex = e;
                }

                i++;
            }

            throw ex;
        }

        public static string getHttpResponseInternal(HttpWebRequest request)
        {
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                if (((int)response.StatusCode).ToString().StartsWith("2"))
                {
                    return reader.ReadToEnd();
                }
                else
                {
                    throw new Exception(reader.ReadToEnd());
                }
            }
        }

        public static HttpResponse getHttpResponseWithHeaders(string url, string method, string requestBody = null)
        {
            int i = 0;
            Exception ex = null;
            while (i < Parameters.maxRetries)
            {
                HttpWebRequest request = setHttpHeader(url, method, requestBody);
                try
                {
                    return getHttpResponseInternalWithHeaders(request);
                }
                catch (Exception e)
                {
                    Debug.LogError("Http Error : " + e.Message);
                    ex = e;
                }

                i++;
            }

            throw ex;
        }

        public static HttpResponse getHttpResponseInternalWithHeaders(HttpWebRequest request)
        {
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                if(((int)response.StatusCode).ToString().StartsWith("2"))
                {
                    HttpResponse resp = new HttpResponse();
                    resp.headers = response.Headers;
                    resp.responseBody = reader.ReadToEnd();
                    return resp;
                }
                else
                {
                    throw new Exception(reader.ReadToEnd());
                }

            }
        }
    }
}
