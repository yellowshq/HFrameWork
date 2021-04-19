using System;
using System.Net;

namespace CloudContentDelivery
{ 
    [Serializable]
    public class HttpResponse
    {
        public WebHeaderCollection headers;
        public string responseBody;
    }
}