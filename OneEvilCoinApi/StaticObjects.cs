using OneEvil.OneEvilCoinAPI.ProcessManagers;
using System;

namespace OneEvil.OneEvilCoinAPI
{
    static class StaticObjects
    {
        public const string RpcUrlDefaultLocalhost = "localhost";

        public static readonly string ApplicationDirectory = AppDomain.CurrentDomain.BaseDirectory;

        public static readonly JobManager JobManager = new JobManager();
    }
}
