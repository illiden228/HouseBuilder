using System;
using Core;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.Idle.Monitors
{
    public class WorkerMonitorView : BaseMonobehaviour
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _addWorkerButton;
        [SerializeField] private Button _effectiencyUpButton;
        [SerializeField] private Button _speedUpButton;
        [SerializeField] private Button _mergeButton;
        [SerializeField] private Transform _container;
        [SerializeField] private TMP_Text _effectiencyLevelText;
        [SerializeField] private TMP_Text _speedLevelText;
        [SerializeField] private TMP_Text _moneysText;
        [SerializeField] private TMP_Text _workersCountText;
        
        public struct Ctx
        {
            public CompositeDisposable viewDisposable;
            public Action back;
            public Action addWorker;
            public Action effectiencyUp;
            public Action speedUp;
            public Action merge;
            public IReadOnlyReactiveProperty<int> moneys;
            public IReadOnlyReactiveProperty<int> speedLevel;
            public IReadOnlyReactiveProperty<int> effectiencyLevel;
            public IReadOnlyReactiveProperty<int> workersCount;
        }

        private Ctx _ctx;

        public Transform Container => _container;

        public void Init(Ctx ctx)
        {
            _ctx = ctx;

            SubscribeButton(_backButton, _ctx.back);
            SubscribeButton(_addWorkerButton, _ctx.addWorker);
            SubscribeButton(_effectiencyUpButton, _ctx.effectiencyUp);
            SubscribeButton(_speedUpButton, _ctx.speedUp);
            SubscribeButton(_mergeButton, _ctx.merge);

            SubscribeText(_moneysText, _ctx.moneys);
            SubscribeText(_speedLevelText, _ctx.speedLevel);
            SubscribeText(_effectiencyLevelText, _ctx.effectiencyLevel);
            SubscribeText(_workersCountText, _ctx.workersCount);
        }

        public void SubscribeButton(Button button, Action action)
        {
            button.OnClickAsObservable().Subscribe(_ => action?.Invoke()).AddTo(_ctx.viewDisposable);
        }

        public void SubscribeText(TMP_Text field, IReadOnlyReactiveProperty<int> value)
        {
            value.Subscribe(value => field.text = value.ToString()).AddTo(_ctx.viewDisposable);
        }
    }
}

