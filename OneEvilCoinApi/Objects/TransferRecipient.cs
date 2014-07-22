using Newtonsoft.Json;

namespace OneEvil.OneEvilCoinAPI
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TransferRecipient
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("amount")]
        public ulong Amount { get; set; }

        public TransferRecipient(string address, ulong amount)
        {
            Address = address;
            Amount = amount;
        }
    }
}
