using Core;
using Logic.Idle.Monitors;
using UniRx;
using UnityEngine;

namespace Logic.Idle.Monitors
{
    public class BuildingsMonitorPm : BaseDisposable, IMonitor
    {
        public struct Ctx
        {
            public Transform uiParent;
            public IResourceLoader resourceLoader;
        }

        private readonly Ctx _ctx;
        private BuildingsMonitorView _view;
        private const string VIEW_PREFAB_NAME = "BuildingsMonitorView";

        public BuildingsMonitorPm(Ctx ctx)
        {
            _ctx = ctx;
            
            AddDispose(_ctx.resourceLoader.LoadPrefab("fakebundles", VIEW_PREFAB_NAME, OnPrefabLoaded));
        }
        
        private void OnPrefabLoaded(GameObject prefab)
        {
            _view = GameObject.Instantiate(prefab, _ctx.uiParent).GetComponent<BuildingsMonitorView>();
            
            _view.Init(new BuildingsMonitorView.Ctx
            {
                viewDisposable = AddDispose(new CompositeDisposable())
            });
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