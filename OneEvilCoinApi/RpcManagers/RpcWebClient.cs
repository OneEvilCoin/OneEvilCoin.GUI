﻿using OneEvil.OneEvilCoinAPI.Settings;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Threading;

namespace OneEvil.OneEvilCoinAPI.RpcManagers
{
    public sealed class RpcWebClient
    {
        public RpcSettings RpcSettings { get; private set; }

        private static readonly JsonSerializer JsonSerializer = new JsonSerializer { NullValueHandling = NullValueHandling.Ignore };
        private static readonly Encoding EncodingUtf8 = Encoding.UTF8;

        internal RpcWebClient(RpcSettings rpcSettings)
        {
            RpcSettings = rpcSettings;
        }

        public T HttpGetData<T>(RpcPortType portType, string command)
        {
            var jsonString = HttpQueryString(portType, "GET", command);
            var output = JsonSerializer.DeserializeObject<T>(jsonString);

            return output;
        }

        public T HttpPostData<T>(RpcPortType portType, string command, HttpRpcRequest httpRpcRequest)
        {
            var jsonString = PostString(portType, command, JsonSerializer.SerializeObject(httpRpcRequest));
            var output = JsonSerializer.DeserializeObject<T>(jsonString);

            return output;
        }

        private string HttpQueryString(RpcPortType portType, string method, string relativeUrl)
        {
            var request = CreateHttpWebRequest(portType, method, relativeUrl);
            return request.GetResponseString();
        }

        public T JsonQueryData<T>(RpcPortType portType, JsonRpcRequest jsonRpcRequest)
        {
            var jsonString = PostString(portType, "json_rpc", JsonSerializer.SerializeObject(jsonRpcRequest));
            var output = JsonSerializer.DeserializeObject<JsonRpcResponse<T>>(jsonString);

            return output.Result;
        }

        public T JsonQueryData<T>(RpcPortType portType, string command)
        {
            return JsonQueryData<T>(portType, new JsonRpcRequest(command));
        }

        private HttpWebRequest CreateHttpWebRequest(RpcPortType portType, string method, string relativeUrl)
        {
            var request = WebRequest.CreateHttp(GetBaseUrl(portType) + relativeUrl);

            request.Method = method;
            request.Timeout = Timeout.Infinite;
            request.ReadWriteTimeout = Timeout.Infinite;

            return request;
        }

        private string PostString(RpcPortType portType, string relativeUrl, string postData)
        {
            var request = CreateHttpWebRequest(portType, "POST", relativeUrl);
            request.ContentType = "application/json";

            var postBytes = EncodingUtf8.GetBytes(postData);
            request.ContentLength = postBytes.Length;

            using (var requestStream = request.GetRequestStream()) {
                requestStream.Write(postBytes, 0, postBytes.Length);
            }

            return request.GetResponseString();
        }

        private string GetBaseUrl(RpcPortType portType)
        {
            ushort port;
            switch (portType) {
                case RpcPortType.Wallet:
                    port = RpcSettings.UrlPortWallet;
                    break;

                default:
                    port = RpcSettings.UrlPortDaemon;
                    break;
            }

            return "http://" + RpcSettings.UrlHost + ":" + port + "/";
        }
    }
}
