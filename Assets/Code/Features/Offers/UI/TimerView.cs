using System;
using Code.Features.Offers.Implementation;
using Code.Features.Timer.Implementation;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Code.Features.Offers.UI
{
    public class TimerView : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _timerText;

        private IRemainingTimeProvider _timerProvider;
        private readonly CompositeDisposable _disposables = new();

        [Inject]
        private void Construct(IRemainingTimeProvider timerProvider)
        {
            _timerProvider = timerProvider;
        }

        private void Start()
        {
            if (_timerProvider.GetRemainingTime() > 0)
            {
                InitializeTimer();
            }
            else
            {
                WaitOffersUpdate();
            }
        }

        private void InitializeTimer()
        {
            UpdateTimer();

            Observable.Interval(TimeSpan.FromSeconds(1), Scheduler.MainThread)
                .Subscribe(_ => UpdateTimer())
                .AddTo(_disposables);
        }

        private void WaitOffersUpdate()
        {
            Observable.FromEvent<Action, Unit>(
                    handler => () => handler(Unit.Default),
                    h => ((OfferEvent)_timerProvider).OnStateChanged += h,
                    h => ((OfferEvent)_timerProvider).OnStateChanged -= h)
                .Take(1)
                .Subscribe(_ => InitializeTimer())
                .AddTo(_disposables);
        }

        private void UpdateTimer()
        {
            long remaining = _timerProvider.GetRemainingTime();

            if (remaining <= 0)
            {
                _timerText.text = "00:00:00"; //временно!
                _disposables.Clear();
                return;
            }

            TimeSpan timeSpan = TimeSpan.FromSeconds(remaining);
            _timerText.text = timeSpan.ToString(@"hh\:mm\:ss");
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}