using System;
using Core;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.Idle.Monitors
{
    public class BuildProgressQueueRawView : BaseMonobehaviour
    {
        [SerializeField] private TMP_Text _floorsProgressText;
        [SerializeField] private TMP_Text _incomeText;
        [SerializeField] private Button _buildButton;
        
        public struct Ctx
        {
            public CompositeDisposable viewDisposable;
            public IReadOnlyReactiveProperty<int> floorsCount;
            public IReadOnlyReactiveProperty<int> buildingIncome;
            public IReadOnlyReactiveProperty<int> maxFloorsCount;
            public Action build;
        }

        private Ctx _ctx;

        public void Init(Ctx ctx)
        {
            _ctx = ctx;

            _ctx.floorsCount.Subscribe(value => _floorsProgressText.text = $"{value} / {_ctx.maxFloorsCount.Value}")
                .AddTo(_ctx.viewDisposable);
            _ctx.buildingIncome.Subscribe(value => _incomeText.text = value.ToString()).AddTo(_ctx.viewDisposable);
            _buildButton.OnClickAsObservable().Subscribe(_ => _ctx.build.Invoke()).AddTo(_ctx.viewDisposable);
        }
    }
}

