﻿using OneEvil.OneEvilCoinAPI;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace OneEvil.OneEvilCoinGUI.Views.MainWindow
{
    sealed class OverviewViewModel : INotifyPropertyChanged
    {
        private ConcurrentReadOnlyObservableCollection<Transaction> _dataSourceTransactions;
        public ConcurrentReadOnlyObservableCollection<Transaction> DataSourceTransactions {
            get { return _dataSourceTransactions; }

            set {
                _dataSourceTransactions = value;
                OnPropertyChanged();
            }
        }

        private Visibility _addressVisibility = Visibility.Hidden;
        public Visibility AddressVisibility {
            get { return _addressVisibility; }

            private set {
                _addressVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility _initializerVisibility = Visibility.Visible;
        public Visibility InitializerVisibility {
            get { return _initializerVisibility; }

            private set {
                _initializerVisibility = value;
                OnPropertyChanged();
            }
        }

        private string _address;
        public string Address {
            get { return _address; }

            set {
                _address = value;
                OnPropertyChanged();

                if (!string.IsNullOrEmpty(value)) {
                    InitializerVisibility = Visibility.Hidden;
                    AddressVisibility = Visibility.Visible;

                } else {
                    AddressVisibility = Visibility.Hidden;
                    InitializerVisibility = Visibility.Visible;
                }
            }
        }

        private ulong? _balanceSpendable;
        public ulong? BalanceSpendable {
            get { return _balanceSpendable; }

            set {
                _balanceSpendable = value;
                OnPropertyChanged();
            }
        }

        private ulong? _balanceUnconfirmed;
        public ulong? BalanceUnconfirmed {
            get { return _balanceUnconfirmed; }

            set {
                _balanceUnconfirmed = value;
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
