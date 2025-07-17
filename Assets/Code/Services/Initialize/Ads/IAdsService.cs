using System;

namespace Code.Services.Initialize.Ads
{
    public interface IAdsService
    {
        event Action<string> OnAdCompleted;
        void LoadRewardedAd();
        void ShowRewardedAd(string offerId);
    }
}