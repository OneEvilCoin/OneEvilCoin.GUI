using OneEvil.OneEvilCoinAPI;

namespace OneEvil.OneEvilCoinGUI.Windows
{
    public partial class DebugWindow
    {
        private static readonly OneEvilCoinClient OneEvilCoinClient = StaticObjects.OneEvilCoinClient;

        public DebugWindow()
        {
            Icon = StaticObjects.ApplicationIconImage;

            InitializeComponent();

            ConsoleDaemon.DataContext = StaticObjects.LoggerDaemon;
            ConsoleWallet.DataContext = StaticObjects.LoggerWallet;
        }

        private void ConsoleDaemon_SendRequested(object sender, string e)
        {
            OneEvilCoinClient.Daemon.Send(e);
        }

        private void ConsoleWallet_SendRequested(object sender, string e)
        {
            OneEvilCoinClient.Wallet.Send(e);
        }
    }
}
