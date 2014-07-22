using OneEvil.OneEvilCoinAPI.ProcessManagers;

namespace OneEvil.OneEvilCoinAPI
{
    static class StaticObjects
    {
        public const string RpcUrlDefaultLocalhost = "localhost";

        public static readonly JobManager JobManager = new JobManager();
    }
}
