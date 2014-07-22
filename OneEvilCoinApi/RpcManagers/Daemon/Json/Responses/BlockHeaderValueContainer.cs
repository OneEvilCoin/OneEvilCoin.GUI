using Newtonsoft.Json;

namespace OneEvil.OneEvilCoinAPI.RpcManagers.Daemon.Json.Responses
{
    class BlockHeaderValueContainer : RpcResponse, IValueContainer<BlockHeader>
    {
        [JsonProperty("block_header")]
        public BlockHeader Value { get; private set; }
    }
}
