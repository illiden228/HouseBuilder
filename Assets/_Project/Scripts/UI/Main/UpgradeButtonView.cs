using System;
using Core;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UpgradeButtonView : BaseMonobehaviour
    {
        [SerializeField] private Button _upgradeButton;
        [SerializeField] private TMP_Text _currentLevelText;
        [SerializeField] private TMP_Text _currentCost;
        
        public struct Ctx
        {
            public CompositeDisposable viewDisposable;
            public IReadOnlyReactiveProperty<int> cost;
            public IReadOnlyReactiveProperty<int> currentLevel;
            public Action upgradeClick;
        }

        private Ctx _ctx;

        public void Init(Ctx ctx)
        {
            _ctx = ctx;

            _upgradeButton.OnClickAsObservable().Subscribe(_ => _ctx.upgradeClick?.Invoke()).AddTo(_ctx.viewDisposable);
            _ctx.currentLevel.Subscribe(value => _currentLevelText.text = value.ToString()).AddTo(_ctx.viewDisposable);
            _ctx.cost.Subscribe(value => _currentCost.text = value.ToString()).AddTo(_ctx.viewDisposable);
        }
    }
}