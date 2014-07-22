using OneEvil.OneEvilCoinAPI.RpcManagers.Wallet.Json.Responses;

namespace OneEvil.OneEvilCoinAPI
{
    public class BalanceChangingEventArgs : ValueChangingEventArgs<Balance>
    {
        internal BalanceChangingEventArgs(Balance newValue) : base(newValue)
        {

        }
    }
}
