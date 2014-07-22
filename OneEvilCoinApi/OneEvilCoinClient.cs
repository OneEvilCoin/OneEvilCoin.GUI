using OneEvil.OneEvilCoinAPI.ProcessManagers;
using OneEvil.OneEvilCoinAPI.RpcManagers;
using OneEvil.OneEvilCoinAPI.Settings;
using System;

namespace OneEvil.OneEvilCoinAPI
{
    public class OneEvilCoinClient : IDisposable
    {
        private RpcWebClient RpcWebClient { get; set; }
        private PathSettings Paths { get; set; }

        public DaemonManager Daemon { get; private set; }
        public WalletManager Wallet { get; private set; }

        public OneEvilCoinClient(PathSettings paths, RpcSettings rpcSettings)
        {
            RpcWebClient = new RpcWebClient(rpcSettings);
            Paths = paths;

            Daemon = new DaemonManager(RpcWebClient, Paths);
            Wallet = new WalletManager(RpcWebClient, Paths, Daemon);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing) {
                if (Wallet != null) {
                    Wallet.Dispose();
                    Wallet = null;
                }

                if (Daemon != null) {
                    Daemon.Dispose();
                    Daemon = null;
                }
            }
        }
    }
}
