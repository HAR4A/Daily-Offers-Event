namespace Code.Features.Timer.Implementation
{
    public interface ITimerService
    {
        void Register(IRemainingTimeProvider provider);
        void Unregister(IRemainingTimeProvider provider);
    }
}