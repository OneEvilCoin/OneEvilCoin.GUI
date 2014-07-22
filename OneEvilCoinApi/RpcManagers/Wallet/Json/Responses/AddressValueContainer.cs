using Newtonsoft.Json;

namespace OneEvil.OneEvilCoinAPI.RpcManagers.Wallet.Json.Responses
{
    class AddressValueContainer : IValueContainer<string>
    {
        [JsonProperty("address")]
        public string Value { get; private set; }
    }
}
