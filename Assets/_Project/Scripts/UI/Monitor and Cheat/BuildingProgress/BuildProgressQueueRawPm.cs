using System;
using Core;
using UniRx;
using UnityEngine;

namespace Logic.Idle.Monitors
{
    public class BuildProgressQueueRawPm : BaseDisposable
    {
        public struct Ctx
        {
            public IResourceLoader resourceLoader;
            public Transform uiParent;
            public Action build;
            public ReactiveProperty<int> floorsCount;
            public IReactiveProperty<int> maxFloorsCount;
            public ReactiveProperty<int> income;
        }

        private readonly Ctx _ctx;
        private const string VIEW_PREFAB_NAME = "BuildProgressQueueRawView";
        private BuildProgressQueueRawView _view;

        public BuildProgressQueueRawPm(Ctx ctx)
        {
            _ctx = ctx;

            _ctx.resourceLoader.LoadPrefab("fakebundles", VIEW_PREFAB_NAME, OnPrefabLoaded);
        }

        private void OnPrefabLoaded(GameObject prefab)
        {
            _view = GameObject.Instantiate(prefab, _ctx.uiParent).GetComponent<BuildProgressQueueRawView>();
            
            _view.Init(new BuildProgressQueueRawView.Ctx
            {
                viewDisposable = AddDispose(new CompositeDisposable()),
                floorsCount = _ctx.floorsCount,
                build = _ctx.build,
                buildingIncome = _ctx.income,
                maxFloorsCount = _ctx.maxFloorsCount
            });
        }

        protected override void OnDispose()
        {
            if(_view != null)
                GameObject.Destroy(_view.gameObject);
            base.OnDispose();
        }
    }
}