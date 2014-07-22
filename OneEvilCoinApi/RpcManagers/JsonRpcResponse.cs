using Newtonsoft.Json;

namespace OneEvil.OneEvilCoinAPI.RpcManagers
{
    public class JsonRpcResponse<T>
    {
        [JsonProperty("result")]
        public T Result { get; private set; }

        [JsonProperty("error")]
        public JsonError Error { get; private set; }
    }
}
