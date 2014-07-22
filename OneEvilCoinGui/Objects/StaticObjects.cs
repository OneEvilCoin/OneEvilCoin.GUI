﻿using OneEvil.OneEvilCoinAPI;
using OneEvil.OneEvilCoinAPI.Settings;
using OneEvil.OneEvilCoinGUI.Windows;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
//using System.Net;
using System.Reflection;
using System.Windows.Media;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;

namespace OneEvil.OneEvilCoinGUI
{
    static class StaticObjects
    {
        public const string DefaultLanguageCode = "default";

        public const double CoinAtomicValueDivider = 1000000000000;
        public const string StringFormatCoinDisplayValue = "0.000000000000";
        public const string StringFormatCoinBalance = "{0:" + StringFormatCoinDisplayValue + "} {1}";

        public static readonly Brush BrushForegroundDefault = Brushes.Black;
        public static readonly Brush BrushForegroundWarning = Brushes.OrangeRed;
        public static readonly Brush BrushBackgroundError = Brushes.LightPink;

        public static readonly Assembly ApplicationAssembly = Assembly.GetExecutingAssembly();
        public static readonly AssemblyName ApplicationAssemblyName = ApplicationAssembly.GetName();

        public static readonly Version ApplicationVersion = ApplicationAssemblyName.Version;
        public static readonly string ApplicationVersionString = ApplicationVersion.ToString(3);

        public static readonly Icon ApplicationIcon = Icon.ExtractAssociatedIcon(ApplicationAssembly.Location);
        public static readonly ImageSource ApplicationIconImage = Icon.ExtractAssociatedIcon(ApplicationAssembly.Location).ToImageSource();

        public static readonly string ApplicationPath = ApplicationAssemblyName.CodeBase;
        public static readonly string ApplicationStartupShortcutPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Startup),
            Helper.GetAssemblyAttribute<AssemblyTitleAttribute>().Title + ".lnk"
        );

        public static readonly string ApplicationBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        private static bool _isUnhandledExceptionLoggingEnabled = true;
        public static bool IsUnhandledExceptionLoggingEnabled {
            get { return _isUnhandledExceptionLoggingEnabled; }
            set { _isUnhandledExceptionLoggingEnabled = value; }
        }

        public static int ApplicationFirstInstanceActivatorMessage { get; set; }

        public static MainWindow MainWindow { get; set; }

        public static OneEvilCoinClient OneEvilCoinClient { get; private set; }

        public static Logger LoggerDaemon { get; private set; }
        public static Logger LoggerWallet { get; private set; }

        public static ObservableCollection<SettingsManager.ConfigElementContact> DataSourceAddressBook { get; private set; }

        public static void Initialize()
        {
            var storedPathSettings = SettingsManager.Paths;
            var pathSettings = new PathSettings {
                DirectoryDaemonData = storedPathSettings.DirectoryDaemonData,
                DirectoryWalletBackups = storedPathSettings.DirectoryWalletBackups,
                FileWalletData = storedPathSettings.FileWalletData,
                SoftwareDaemon = storedPathSettings.SoftwareDaemon,
                SoftwareWallet = storedPathSettings.SoftwareWallet,
                SoftwareMiner = storedPathSettings.SoftwareMiner,
            };

            var storedNetworkSettings = SettingsManager.Network;
            var rpcSettings = new RpcSettings(
                storedNetworkSettings.RpcUrlHost,
                storedNetworkSettings.RpcUrlPortDaemon,
                storedNetworkSettings.RpcUrlPortWallet
            );

            // TODO: Add support for using proxies
            //if (networkSettings.IsProxyEnabled) {
            //    if (!string.IsNullOrEmpty(networkSettings.ProxyHost) && networkSettings.ProxyPort != null) {
            //        rpcSettings.Proxy = new WebProxy(networkSettings.ProxyHost, (int)networkSettings.ProxyPort);
            //    }
            //}

            OneEvilCoinClient = new OneEvilCoinClient(pathSettings, rpcSettings);

            LoggerDaemon = new Logger();
            LoggerWallet = new Logger();

            DataSourceAddressBook = new ObservableCollection<SettingsManager.ConfigElementContact>(SettingsManager.AddressBook.Elements);
            DataSourceAddressBook.CollectionChanged += DataSourceAddressBook_CollectionChanged;
        }

        private static void DataSourceAddressBook_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Save the collection's changes into the configuration file

            if (DataSourceAddressBook.Count == 0) {
                SettingsManager.AddressBook.Elements.Clear();
                return;
            }

            var oldItems = e.OldItems;
            if (oldItems != null) {
                for (var i = oldItems.Count - 1; i >= 0; i--) {
                    SettingsManager.AddressBook.Elements.Remove(oldItems[i] as SettingsManager.ConfigElementContact);
                }
            }

            var newItems = e.NewItems;
            if (newItems != null) {
                for (var i = newItems.Count - 1; i >= 0; i--) {
                    SettingsManager.AddressBook.Elements.Add(newItems[i] as SettingsManager.ConfigElementContact);
                }
            }
        }
    }
}
