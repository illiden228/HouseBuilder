using System;
using Core;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.Idle.Monitors
{
    public abstract class MonitorPanelView : BaseMonobehaviour
    {
        [SerializeField] private Button _backButton;
        
        public struct BaseCtx
        {
            public CompositeDisposable viewDisposable;
            public Action back;
        }

        protected BaseCtx _baseCtx;

        public void BaseInit(BaseCtx baseCtx)
        {
            _baseCtx = baseCtx;
            
            SubscribeButton(_backButton, _baseCtx.back);
        }
        
        protected void SubscribeButton(Button button, Action action)
        {
            button.OnClickAsObservable().Subscribe(_ => action?.Invoke()).AddTo(_baseCtx.viewDisposable);
        }
        
        protected void SubscribeText(TMP_Text field, IReadOnlyReactiveProperty<int> value)
        {
            value.Subscribe(value => field.text = value.ToString()).AddTo(_baseCtx.viewDisposable);
        }
    }
}

