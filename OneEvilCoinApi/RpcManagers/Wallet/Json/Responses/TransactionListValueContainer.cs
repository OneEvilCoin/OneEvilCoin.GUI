using Newtonsoft.Json;
using System.Collections.Generic;

namespace OneEvil.OneEvilCoinAPI.RpcManagers.Wallet.Json.Responses
{
    class TransactionListValueContainer : RpcResponse, IValueContainer<IList<Transaction>>
    {
        [JsonProperty("transfers")]
        public IList<Transaction> Value { get; private set; }
    }
}
