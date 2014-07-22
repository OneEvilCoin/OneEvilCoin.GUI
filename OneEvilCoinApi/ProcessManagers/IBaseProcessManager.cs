namespace OneEvil.OneEvilCoinAPI.ProcessManagers
{
    public interface IBaseProcessManager
    {
        void Start();
        void Stop();
        void Restart();
    }
}
