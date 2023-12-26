using System;
using Core;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.Idle.Monitors
{
    public class TimeSpeedCheatView : BaseMonobehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_Text _currentValueText;
        [SerializeField] private TMP_Text _minValueText;
        [SerializeField] private TMP_Text _maxValueText;
        
        public struct Ctx
        {
            public CompositeDisposable viewDisposable;
            public Action<float> changeTimeSpeedValue;
            public ReactiveProperty<float> currentValue;
            public ReactiveProperty<float> minValue;
            public ReactiveProperty<float> maxValue;
        }

        private Ctx _ctx;

        public void Init(Ctx ctx)
        {
            _ctx = ctx;

            _slider.minValue = _ctx.minValue.Value;
            _slider.maxValue = _ctx.maxValue.Value;

            _minValueText.text = _ctx.minValue.Value.ToString();
            _maxValueText.text = _ctx.maxValue.Value.ToString();
            
            _slider.ObserveEveryValueChanged(x => x.value)
                   .Subscribe(_ctx.changeTimeSpeedValue)
                   .AddTo(_ctx.viewDisposable);
            
            _ctx.currentValue
                   .Subscribe(value => _currentValueText.text = value.ToString("F1"))
                   .AddTo(_ctx.viewDisposable);
        }
    }
}

