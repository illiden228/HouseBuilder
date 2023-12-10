using System;
using System.Collections.Generic;
using Core;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.Idle.Monitors
{
    public class MainMonitorView : BaseMonobehaviour
    {
        [Serializable]
        public struct MonitorButton
        {
            public MonitorType type;
            public Button button;
        }

        [SerializeField] private List<MonitorButton> _monitorButtons;
        [SerializeField] private Button _close;
        
        public struct Ctx
        {
            public CompositeDisposable viewDisposable;
            public Action close;
            public Action<MonitorType> openMonitor;
        }

        private Ctx _ctx;

        public void Init(Ctx ctx)
        {
            _ctx = ctx;

            foreach (var monitor in _monitorButtons)
            {
                SubscribeButton(monitor.button, () => _ctx.openMonitor?.Invoke(monitor.type));
            }
            SubscribeButton(_close, _ctx.close);
        }
        
        public void SubscribeButton(Button button, Action action)
        {
            button.OnClickAsObservable().Subscribe(_ => action?.Invoke()).AddTo(_ctx.viewDisposable);
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}

