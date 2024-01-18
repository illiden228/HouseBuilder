using Core;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.Idle.Monitors
{
    public class FloatProgressSliderView : BaseMonobehaviour
    {
        [SerializeField] private TMP_Text _currentValue;
        [SerializeField] private TMP_Text _maxValue;
        [SerializeField] private Slider _slider;
        
        public struct Ctx
        {
            public CompositeDisposable viewDisposable;
            public IReadOnlyReactiveProperty<float> current;
            public IReadOnlyReactiveProperty<float> max;
        }

        private Ctx _ctx;

        public void Init(Ctx ctx)
        {
            _ctx = ctx;

            _ctx.max.Subscribe(value => _maxValue.text = value.ToString()).AddTo(_ctx.viewDisposable);
            _ctx.current.Subscribe(CurrentValueChanged).AddTo(_ctx.viewDisposable);
        }
        
        private void CurrentValueChanged(float value)
        {
            _currentValue.text = value.ToString("F0");
            _slider.value = value / _ctx.max.Value;
        }
    }
}

