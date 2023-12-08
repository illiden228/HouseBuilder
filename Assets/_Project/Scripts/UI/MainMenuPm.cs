using Containers.Data;
using Core;
using Logic.Idle.Monitors;
using Logic.Profile;
using UniRx;
using UnityEngine;

namespace UI
{
    public class MainMenuPm : BaseDisposable
    {
        public struct Ctx
        {
            public Transform uiParent;
            public IResourceLoader resourceLoader;
            public GameConfig gameConfig;
            public IReadOnlyProfile profile;
            public bool viewTestUI;
        }

        private readonly Ctx _ctx;
        private const string VIEW_PREFAB_NAME = "MainMenuView";
        private MainMenuView _view;
        private ReactiveProperty<bool> _openTestMonitor;
        private MainMonitorPm _mainMonitor;

        public MainMenuPm(Ctx ctx)
        {
            _ctx = ctx;
            _openTestMonitor = new ReactiveProperty<bool>(false);
            
            AddDispose(_ctx.resourceLoader.LoadPrefab("fakebundles", VIEW_PREFAB_NAME, OnPrefabLoaded));
        }

        private void OnPrefabLoaded(GameObject prefab)
        {
            _view = GameObject.Instantiate(prefab, _ctx.uiParent).GetComponent<MainMenuView>();
            
            _view.Init(new MainMenuView.Ctx
            {
                viewDisposable = AddDispose(new CompositeDisposable()),
                openMonitor = OpenMainMonitor,
                viewTestUI = _ctx.viewTestUI
            });
        }

        private void OpenMainMonitor()
        {
            if (_mainMonitor == null)
            {
                MainMonitorPm.Ctx monitorCtx = new MainMonitorPm.Ctx
                {
                    resourceLoader = _ctx.resourceLoader,
                    gameConfig = _ctx.gameConfig,
                    uiParent = _ctx.uiParent,
                    profile = _ctx.profile,
                    isOpen = _openTestMonitor,
                };
                _mainMonitor = AddDispose(new MainMonitorPm(monitorCtx));
            }

            _openTestMonitor.Value = true;
        }
    }
}