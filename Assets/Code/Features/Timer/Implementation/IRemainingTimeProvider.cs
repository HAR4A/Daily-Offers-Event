namespace Code.Features.Timer.Implementation
{
    public interface IRemainingTimeProvider
    {
        long GetRemainingTime();
        void OnExpired();
    }
}