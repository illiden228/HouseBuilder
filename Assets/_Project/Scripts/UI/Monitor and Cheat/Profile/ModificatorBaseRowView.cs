using System;
using Core;
using TMPro;
using UniRx;
using UnityEngine;

namespace Logic.Idle.Monitors
{
    public class ModificatorBaseRowView : BaseMonobehaviour
    {
        [SerializeField] private TMP_Text _dateStartText;
        [SerializeField] private TMP_Text _durationText;
        
        public struct BaseCtx
        {
            public CompositeDisposable viewDisposable;
            public ReactiveProperty<DateTime> dateStart;
            public ReactiveProperty<float> duration;
        }

        private BaseCtx _baseCtx;

        public void BaseInit(BaseCtx baseCtx)
        {
            _baseCtx = baseCtx;

            SubscribeText(_dateStartText, _baseCtx.dateStart);
            SubscribeText(_durationText, _baseCtx.duration);
        }
        
        protected void SubscribeText<T>(TMP_Text field, IReadOnlyReactiveProperty<T> value)
        {
            value.Subscribe(value => field.text = value.ToString()).AddTo(_baseCtx.viewDisposable);
        }
    }
}

