using Core;
using Logic.Model;
using UniRx;
using UnityEngine;

namespace Logic.Idle.Monitors
{
    public class BuildingMonitorRowPm : BaseDisposable
    {
        public struct Ctx
        {
            public IResourceLoader resourceLoader;
            public Transform uiParent;
            public BuildingModel buildingModel;
        }

        private readonly Ctx _ctx;
        private const string VIEW_PREFAB_NAME = "BuildingMonitorRowView";
        private BuildingMonitorRowView _view;

        public BuildingMonitorRowPm(Ctx ctx)
        {
            _ctx = ctx;

            _ctx.resourceLoader.LoadPrefab("fakebundles", VIEW_PREFAB_NAME, OnPrefabLoaded);
        }

        private void OnPrefabLoaded(GameObject prefab)
        { 
            _view = GameObject.Instantiate(prefab, _ctx.uiParent).GetComponent<BuildingMonitorRowView>();
            
            _view.Init(new BuildingMonitorRowView.Ctx
            {
                viewDisposable = AddDispose(new CompositeDisposable()),
                timeSpeed = _ctx.buildingModel.TimeSpeed,
                income = _ctx.buildingModel.MoneyIncome,
                currentTime = _ctx.buildingModel.CurrentIncomeTime,
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