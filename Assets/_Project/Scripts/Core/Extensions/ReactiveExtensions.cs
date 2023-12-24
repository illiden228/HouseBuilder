﻿using System;
using Containers;
using UniRx;

namespace Tools.Extensions
{
    public static class ReactiveExtensions
    {
        public static IDisposable DelayedCall(float delaySec, Action action)
        {
            if (delaySec <= 0)
            {
                action?.Invoke();
                return null;
            }
            return Observable.Timer(TimeSpan.FromSeconds(delaySec)).Take(1).Subscribe(_ => action?.Invoke());
        }

        public static IDisposable RepeatableDelayedCall(float delaySec, Action action)
        {
            if (delaySec <= 0)
            {
                action?.Invoke();
                return null;
            }

            return Observable.Interval(TimeSpan.FromSeconds(delaySec)).Subscribe(_ => action?.Invoke());
        }

        public static IDisposable StartUpdate(Action onIteration)
        {
            return Observable.EveryUpdate().Subscribe(_ => { onIteration?.Invoke(); });
        }
        
        public static IDisposable StartLateUpdate(Action onIteration)
        {
            return Observable.EveryLateUpdate().Subscribe(_ => { onIteration?.Invoke(); });
        }

        public static IDisposable StartFixedUpdate(Action onIteration)
        {
            return Observable.EveryFixedUpdate().Subscribe(_ => { onIteration?.Invoke(); });
        }
    }
}