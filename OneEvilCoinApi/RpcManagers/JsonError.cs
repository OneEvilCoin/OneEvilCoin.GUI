using Newtonsoft.Json;

namespace OneEvil.OneEvilCoinAPI.RpcManagers
{
    public class JsonError
    {
        [JsonProperty("code")]
        public int Code { get; private set; }

        [JsonProperty("message")]
        public string Message { get; private set; }
    }
}
