using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Features.Timer.Implementation
{
    public class TimerService : MonoBehaviour, ITimerService
    {
        private readonly SortedList<long, List<IRemainingTimeProvider>> _schedule = new();
        private readonly Dictionary<IRemainingTimeProvider, long> _expirations = new();
        private Coroutine _waitCoroutine;

        public void Register(IRemainingTimeProvider provider)
        {
            long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            long expirationTime = now + provider.GetRemainingTime();

            _expirations[provider] = expirationTime;

            if (!_schedule.TryGetValue(expirationTime, out var list))
            {
                list = new List<IRemainingTimeProvider>();
                _schedule[expirationTime] = list;
            }

            list.Add(provider);

            Reschedule();
        }

        public void Unregister(IRemainingTimeProvider provider)
        {
            if (!_expirations.TryGetValue(provider, out var expirationTime))
                return;

            if (_schedule.TryGetValue(expirationTime, out var list))
            {
                list.Remove(provider);
                if (list.Count == 0)
                    _schedule.Remove(expirationTime);
            }

            _expirations.Remove(provider);
            Reschedule();
        }

        private void Reschedule()
        {
            if (_waitCoroutine != null)
            {
                StopCoroutine(_waitCoroutine);
                _waitCoroutine = null;
            }

            if (_schedule.Count == 0)
                return;

            var nextExpiration = _schedule.Keys[0];
            float delay = Mathf.Max(0, nextExpiration - DateTimeOffset.UtcNow.ToUnixTimeSeconds());

            _waitCoroutine = StartCoroutine(WaitAndFire(delay, nextExpiration));
        }

        private IEnumerator WaitAndFire(float delay, long expirationTime)
        {
            yield return new WaitForSecondsRealtime(delay);
            if (_schedule.TryGetValue(expirationTime, out var list))
            {
                var providersSnapshot = new List<IRemainingTimeProvider>(list);

                foreach (var provider in providersSnapshot)
                {
                    provider.OnExpired();
                }

                _schedule.Remove(expirationTime);
            }

            _waitCoroutine = null;
            Reschedule();
        }
    }
}