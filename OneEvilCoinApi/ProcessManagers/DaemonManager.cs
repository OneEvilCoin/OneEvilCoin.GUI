﻿using OneEvil.OneEvilCoinAPI.RpcManagers;
using OneEvil.OneEvilCoinAPI.RpcManagers.Daemon.Http.Requests;
using OneEvil.OneEvilCoinAPI.RpcManagers.Daemon.Http.Responses;
using OneEvil.OneEvilCoinAPI.RpcManagers.Daemon.Json.Requests;
using OneEvil.OneEvilCoinAPI.RpcManagers.Daemon.Json.Responses;
using OneEvil.OneEvilCoinAPI.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace OneEvil.OneEvilCoinAPI.ProcessManagers
{
    public class DaemonManager : BaseProcessManager, IBaseProcessManager
    {
        public event EventHandler BlockchainSynced;
        public event EventHandler<NetworkInformationChangingEventArgs> NetworkInformationChanging;

        private static readonly string[] ProcessArgumentsDefault = { "--log-level 0" };
        private List<string> ProcessArgumentsExtra { get; set; }

        private Timer TimerQueryNetworkInformation { get; set; }
        private Timer TimerSaveBlockchain { get; set; }

        private RpcWebClient RpcWebClient { get; set; }

        private bool _isRpcAvailable;
        public bool IsRpcAvailable {
            get { return _isRpcAvailable; }

            private set {
                _isRpcAvailable = value;
                if (value && IsBlockchainSavable) {
                    TimerSaveBlockchain.StartOnce(TimerSettings.DaemonSaveBlockchainPeriod);
                }
            }
        }

        private bool _isBlockchainSavable;
        internal bool IsBlockchainSavable {
            get { return _isBlockchainSavable; }

            set {
                if (value == _isBlockchainSavable) return;
                _isBlockchainSavable = value;

                if (value && IsRpcAvailable) {
                    TimerSaveBlockchain.StartOnce(TimerSettings.DaemonSaveBlockchainPeriod);
                }
            }
        }

        private bool _isBlockchainSynced;
        public bool IsBlockchainSynced {
            get { return _isBlockchainSynced; }

            private set {
                _isBlockchainSynced = value;
                if (BlockchainSynced != null && value) BlockchainSynced(this, EventArgs.Empty);
            }
        }

        private NetworkInformation _networkInformation;
        public NetworkInformation NetworkInformation {
            get { return _networkInformation; }

            private set {
                if (NetworkInformationChanging != null) NetworkInformationChanging(this, new NetworkInformationChangingEventArgs(value));
                _networkInformation = value;
            }
        }

        internal DaemonManager(RpcWebClient rpcWebClient, PathSettings paths) : base(paths.SoftwareDaemon)
        {
            OutputReceived += Process_OutputReceived;

            RpcWebClient = rpcWebClient;

            var rpcSettings = RpcWebClient.RpcSettings;

            ProcessArgumentsExtra = new List<string>(3) {
                "--data-dir \"" + paths.DirectoryDaemonData + "\"",
                "--rpc-bind-port " + rpcSettings.UrlPortDaemon
            };

            if (rpcSettings.UrlHost != StaticObjects.RpcUrlDefaultLocalhost) {
                ProcessArgumentsExtra.Add("--rpc-bind-ip " + rpcSettings.UrlHost);
            }

            TimerQueryNetworkInformation = new Timer(delegate { QueryNetworkInformation(); });
            TimerSaveBlockchain = new Timer(delegate { SaveBlockchain(); });
        }

        public void Start()
        {
            StartProcess(ProcessArgumentsDefault.Concat(ProcessArgumentsExtra).ToArray());
        }

        public void Stop()
        {
            KillBaseProcess();
        }

        public void Restart()
        {
            Stop();
            Start();
        }

        private void StartRpcServices()
        {
            TimerQueryNetworkInformation.StartImmediately(TimerSettings.DaemonQueryNetworkInformationPeriod);
            IsRpcAvailable = true;
        }

        private void QueryNetworkInformation()
        {
            TimerQueryNetworkInformation.Stop();

            var output = HttpGetData<NetworkInformation>(HttpRpcCommands.DaemonGetInformation);
            if (output != null && output.BlockHeightTotal != 0) {
                var blockHeaderLast = GetBlockHeaderLast();
                if (blockHeaderLast != null) {
                    output.BlockTimeCurrent = blockHeaderLast.Timestamp;

                    NetworkInformation = output;

                    if (output.BlockHeightRemaining == 0 && !IsBlockchainSynced) {
                        IsBlockchainSynced = true;
                    }
                }
            }

            TimerQueryNetworkInformation.StartOnce(TimerSettings.DaemonQueryNetworkInformationPeriod);
        }

        private void SaveBlockchain()
        {
            Send("save");
        }

        public BlockHeader GetBlockHeaderLast()
        {
            var blockHeaderValueContainer = JsonQueryData<BlockHeaderValueContainer>(new GetBlockHeaderLast());
            if (blockHeaderValueContainer != null) {
                return blockHeaderValueContainer.Value;
            }

            return null;
        }

        public object GetTransactions(IList<string> transactionIds)
        {
            var transactions = HttpPostData<TransactionBlobList>(HttpRpcCommands.DaemonGetTransactions, new GetTransactions(transactionIds));
            if (transactions != null) {
                return transactions;
            }

            return null;
        }

        private void Process_OutputReceived(object sender, string e)
        {
            var dataLower = e.ToLower(Helper.InvariantCulture);

            if (dataLower.Contains("blockchain stored")) {
                TimerSaveBlockchain.StartOnce(TimerSettings.DaemonSaveBlockchainPeriod);
            }

            if (dataLower.Contains("rpc server initialized")) {
                StartRpcServices();
            }
        }

        private T HttpGetData<T>(string command) where T : RpcResponse
        {
            var output = RpcWebClient.HttpGetData<T>(RpcPortType.Daemon, command);
            if (output != null && output.Status == RpcResponseStatus.Ok) {
                return output;
            }

            return null;
        }

        private T HttpPostData<T>(string command, HttpRpcRequest httpRpcRequest) where T : RpcResponse
        {
            var output = RpcWebClient.HttpPostData<T>(RpcPortType.Daemon, command, httpRpcRequest);
            if (output != null && output.Status == RpcResponseStatus.Ok) {
                return output;
            }

            return null;
        }

        private T JsonQueryData<T>(JsonRpcRequest jsonRpcRequest) where T : RpcResponse
        {
            var output = RpcWebClient.JsonQueryData<T>(RpcPortType.Daemon, jsonRpcRequest);
            if (output != null && output.Status == RpcResponseStatus.Ok) {
                return output;
            }

            return null;
        }

        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing) {
                TimerQueryNetworkInformation.Dispose();
                TimerQueryNetworkInformation = null;

                TimerSaveBlockchain.Dispose();
                TimerSaveBlockchain = null;

                base.Dispose(true);
            }
        }
    }
}
