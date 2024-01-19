using Core;
using TMPro;
using UniRx;
using UnityEngine;

namespace Logic.Idle.Monitors
{
    public class BuildingMonitorRowView : BaseMonobehaviour
    {
        [SerializeField] private TMP_Text _speedText;
        [SerializeField] private FloatProgressSliderView _incomeProgressSlider;
        public struct Ctx
        {
            public CompositeDisposable viewDisposable;
            public IReadOnlyReactiveProperty<float> currentTime;
            public IReadOnlyReactiveProperty<float> timeSpeed;
            public IReadOnlyReactiveProperty<int> income;
        }

        private Ctx _ctx;

        public void Init(Ctx ctx)
        {
            _ctx = ctx;

            _ctx.income.Subscribe(_ => UpdateSpeed()).AddTo(_ctx.viewDisposable);
            _ctx.timeSpeed.Subscribe(_ => UpdateSpeed()).AddTo(_ctx.viewDisposable);
            
            _incomeProgressSlider.Init(new FloatProgressSliderView.Ctx
            {
                viewDisposable = _ctx.viewDisposable,
                current = _ctx.currentTime,
                max = _ctx.timeSpeed
            });
        }

        private void UpdateSpeed()
        {
            _speedText.text = $"{_ctx.income.Value} / {_ctx.timeSpeed.Value}";
        }
    }
}

