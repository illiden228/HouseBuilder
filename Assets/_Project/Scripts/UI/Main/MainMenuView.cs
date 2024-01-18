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
        [SerializeField] private Button _buildButton;
        
        public struct Ctx
        {
            public CompositeDisposable viewDisposable;
            public IReadOnlyReactiveProperty<int> moneys;
            public ReactiveProperty<bool> canBuild;
            public bool viewTestUI;
            public Action openMonitor;
            public Action build;
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
            _ctx.canBuild.Subscribe(_buildButton.gameObject.SetActive).AddTo(_ctx.viewDisposable);
            _buildButton.OnClickAsObservable().Subscribe(_ => _ctx.build.Invoke()).AddTo(_ctx.viewDisposable);
        }
    }
}

