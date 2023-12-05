using Core;
using UniRx;
using UnityEngine;

namespace Logic.Idle.Monitors
{
    public class ProfileMonitorPm : BaseDisposable, IMonitor
    {
        public struct Ctx
        {
            public Transform uiParent;
            public IResourceLoader resourceLoader;
        }

        private readonly Ctx _ctx;
        private ProfileMonitorView _view;
        private const string VIEW_PREFAB_NAME = "ProfileMonitorView";

        public ProfileMonitorPm(Ctx ctx)
        {
            _ctx = ctx;
            
            AddDispose(_ctx.resourceLoader.LoadPrefab("fakebundles", VIEW_PREFAB_NAME, OnPrefabLoaded));
        }

        private void OnPrefabLoaded(GameObject prefab)
        {
            _view = GameObject.Instantiate(prefab, _ctx.uiParent).GetComponent<ProfileMonitorView>();
            
            _view.Init(new ProfileMonitorView.Ctx
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