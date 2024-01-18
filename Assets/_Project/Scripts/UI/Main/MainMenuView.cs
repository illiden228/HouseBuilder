using System;
using Core;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuView : BaseMonobehaviour
    {
        [SerializeField] private Button _openMonitor;
        [SerializeField] private UpgradeButtonsView _upgradeButtonsView;
        [SerializeField] private TMP_Text _moneys;
        
        public struct Ctx
        {
            public CompositeDisposable viewDisposable;
            public IReadOnlyReactiveProperty<int> moneys;
            public bool viewTestUI;
            public Action openMonitor;
        }

        private Ctx _ctx;

        public UpgradeButtonsView UpgradeButtonsView => _upgradeButtonsView;

        public void Init(Ctx ctx)
        {
            _ctx = ctx;

            _openMonitor.gameObject.SetActive(_ctx.viewTestUI);
            if (_ctx.viewTestUI)
            {
                _openMonitor.OnClickAsObservable().Subscribe(_ => _ctx.openMonitor?.Invoke())
                    .AddTo(_ctx.viewDisposable);
            }

            _ctx.moneys.Subscribe(value => _moneys.text = value.ToString()).AddTo(_ctx.viewDisposable);
        }
        
        
    }
}

