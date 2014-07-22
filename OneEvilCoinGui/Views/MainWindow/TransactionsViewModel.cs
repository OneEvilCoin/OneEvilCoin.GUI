using OneEvil.OneEvilCoinAPI;
using OneEvil.OneEvilCoinAPI.RpcManagers.Wallet.Json.Responses;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OneEvil.OneEvilCoinGUI.Views.MainWindow
{
    sealed class TransactionsViewModel : INotifyPropertyChanged
    {
        private ConcurrentReadOnlyObservableCollection<Transaction> _dataSourceTransactions;
        public ConcurrentReadOnlyObservableCollection<Transaction> DataSourceTransactions {
            get { return _dataSourceTransactions; }

            set {
                _dataSourceTransactions = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
