using OneEvil.OneEvilCoinAPI.RpcManagers.Daemon.Http.Responses;

namespace OneEvil.OneEvilCoinAPI
{
    public class NetworkInformationChangingEventArgs : ValueChangingEventArgs<NetworkInformation>
    {
        internal NetworkInformationChangingEventArgs(NetworkInformation newValue) : base(newValue)
        {

        }
    }
}
