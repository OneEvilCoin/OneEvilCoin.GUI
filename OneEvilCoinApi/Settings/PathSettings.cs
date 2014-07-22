﻿using System;
using System.IO;

namespace OneEvil.OneEvilCoinAPI.Settings
{
    public class PathSettings
    {
        private const string DefaultRelativePathDirectoryWalletData = "WalletData\\";
        private const string DefaultRelativePathDirectorySoftware = "Resources\\Software\\";
        
        public static readonly string DefaultDirectoryDaemonData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "OneEvilCoin");
        public const string DefaultDirectoryWalletBackups = DefaultRelativePathDirectoryWalletData + "Backups\\";
        public const string DefaultFileWalletData = DefaultRelativePathDirectoryWalletData + "wallet.bin";
        public const string DefaultSoftwareDaemon = DefaultRelativePathDirectorySoftware + "oneevilcoind.exe";
        public const string DefaultSoftwareWallet = DefaultRelativePathDirectorySoftware + "simplewallet.exe";
        public const string DefaultSoftwareMiner = DefaultRelativePathDirectorySoftware + "simpleminer.exe";

        private static readonly string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        private string _directoryDaemonData = DefaultDirectoryDaemonData;
        public string DirectoryDaemonData {
            get { return _directoryDaemonData; }
            set { _directoryDaemonData = value; }
        }

        public string DirectoryWalletData {
            get {
                var lastIndexOfSlash = FileWalletData.LastIndexOf('\\');
                return lastIndexOfSlash >= 0 ? FileWalletData.Substring(0, FileWalletData.LastIndexOf('\\')) : BaseDirectory;
            }
        }

        private string _directoryWalletBackups = DefaultDirectoryWalletBackups;
        public string DirectoryWalletBackups {
            get { return _directoryWalletBackups; }
            set { _directoryWalletBackups = value; }
        }

        private string _fileWalletData = DefaultFileWalletData;
        public string FileWalletData {
            get { return _fileWalletData; }
            set { _fileWalletData = value; }
        }

        public string FileWalletDataKeys {
            get { return FileWalletData + ".keys"; }
        }

        private string _softwareDaemon = DefaultSoftwareDaemon;
        public string SoftwareDaemon {
            get { return _softwareDaemon; }
            set { _softwareDaemon = value; }
        }

        private string _softwareWallet = DefaultSoftwareWallet;
        public string SoftwareWallet {
            get { return _softwareWallet; }
            set { _softwareWallet = value; }
        }

        private string _softwareMiner = DefaultSoftwareMiner;
        public string SoftwareMiner {
            get { return _softwareMiner; }
            set { _softwareMiner = value; }
        }
    }
}
