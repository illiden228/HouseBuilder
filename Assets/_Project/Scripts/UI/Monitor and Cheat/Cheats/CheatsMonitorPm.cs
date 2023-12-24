using System;
using Core;
using Logic.Idle.Monitors;
using UniRx;
using UnityEngine;

namespace Logic.Idle.Monitors
{
    public class CheatsMonitorPm : BaseDisposable, IMonitor
    {
        public struct Ctx
        {
            public Transform uiParent;
            public IResourceLoader resourceLoader;
            public Action back;
        }

        private readonly Ctx _ctx;
        private CheatsMonitorView _view;
        private const string VIEW_PREFAB_NAME = "CheatsMonitorView";

        public CheatsMonitorPm(Ctx ctx)
        {
            _ctx = ctx;
            
            AddDispose(_ctx.resourceLoader.LoadPrefab("fakebundles", VIEW_PREFAB_NAME, OnPrefabLoaded));
        }

        private void OnPrefabLoaded(GameObject prefab)
        {
            _view = GameObject.Instantiate(prefab, _ctx.uiParent).GetComponent<CheatsMonitorView>();
            
            MonitorPanelView.BaseCtx basePanelCtx = new MonitorPanelView.BaseCtx
            {
                viewDisposable = AddDispose(new CompositeDisposable()),
                back = _ctx.back
            };
            
            _view.Init(basePanelCtx, new CheatsMonitorView.Ctx
            {
                
            });

            TimeSpeedCheatPm.Ctx timeSpeedCheatCtx = new TimeSpeedCheatPm.Ctx
            {
                maxValue = 10,
                minValue = 1,
                view = _view.TimeSpeedCheatView
            };
            AddDispose(new TimeSpeedCheatPm(timeSpeedCheatCtx));
        }

        public void Open()
        {
            _view.gameObject.SetActive(true);
        }

        public void Close()
        {
            _view.gameObject.SetActive(false);
        }
    }
}