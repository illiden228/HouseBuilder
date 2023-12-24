using Core;
using UniRx;
using UnityEngine;

namespace Logic.Idle.Monitors
{
    public class TimeSpeedCheatPm : BaseDisposable
    {
        public struct Ctx
        {
            public TimeSpeedCheatView view;
            public float minValue;
            public float maxValue;
        }

        private readonly Ctx _ctx;
        private ReactiveProperty<float> _currentTimeSpeed;

        public TimeSpeedCheatPm(Ctx ctx)
        {
            _ctx = ctx;
            _currentTimeSpeed = new ReactiveProperty<float>(Time.timeScale);
            
            _ctx.view.Init(new TimeSpeedCheatView.Ctx
            {
                currentValue = _currentTimeSpeed,
                maxValue = new ReactiveProperty<float>(_ctx.maxValue),
                minValue = new ReactiveProperty<float>(_ctx.minValue),
                viewDisposable = AddDispose(new CompositeDisposable()),
                changeTimeSpeedValue = OnChangeTimeSpeedValue
            });
        }

        private void OnChangeTimeSpeedValue(float value)
        {
            Time.timeScale = value;
            _currentTimeSpeed.Value = value;
        }
    }
}