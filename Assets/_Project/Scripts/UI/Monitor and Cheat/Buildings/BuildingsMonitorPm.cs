using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Logic.Idle.Monitors;
using Logic.Idle.Workers;
using Logic.Model;
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
            public Action back;
            public IReadOnlyReactiveCollection<BuildingModel> buildings;
        }

        private readonly Ctx _ctx;
        private BuildingsMonitorView _view;
        private const string VIEW_PREFAB_NAME = "BuildingsMonitorView";
        private Dictionary<BuildingModel, BuildingMonitorRowPm> _buildingRows;

        public BuildingsMonitorPm(Ctx ctx)
        {
            _ctx = ctx;
            _buildingRows = new Dictionary<BuildingModel, BuildingMonitorRowPm>();
            
            AddDispose(_ctx.resourceLoader.LoadPrefab("fakebundles", VIEW_PREFAB_NAME, OnPrefabLoaded));
            AddDispose(_ctx.buildings.ObserveAdd().Subscribe(OnAddBuilding));
            AddDispose(_ctx.buildings.ObserveRemove().Subscribe(OnRemoveBuilding));
        }
        
        private void OnPrefabLoaded(GameObject prefab)
        {
            _view = GameObject.Instantiate(prefab, _ctx.uiParent).GetComponent<BuildingsMonitorView>();
            
            MonitorPanelView.BaseCtx basePanelCtx = new MonitorPanelView.BaseCtx
            {
                viewDisposable = AddDispose(new CompositeDisposable()),
                back = _ctx.back
            };
            
            _view.Init(basePanelCtx, new BuildingsMonitorView.Ctx
            {
                
            });
            
            foreach (var buildingModel in _ctx.buildings)
            {
                CreateBuildingMonitorRow(buildingModel);
            }
        }
        
        private void OnAddBuilding(CollectionAddEvent<BuildingModel> addEvent)
        {
            CreateBuildingMonitorRow(addEvent.Value);
        }
        
        private void OnRemoveBuilding(CollectionRemoveEvent<BuildingModel> removeEvent)
        {
            if(_buildingRows.ContainsKey(removeEvent.Value))
                _buildingRows[removeEvent.Value]?.Dispose();
        }

        private void CreateBuildingMonitorRow(BuildingModel model)
        {
            BuildingMonitorRowPm.Ctx buildingRowCtx = new BuildingMonitorRowPm.Ctx
            {
                buildingModel = model,
                resourceLoader = _ctx.resourceLoader,
                uiParent = _view.Container,
            };
            
            _buildingRows[model] = new BuildingMonitorRowPm(buildingRowCtx);
        }

        public void Open()
        {
            _view.gameObject.SetActive(true);
        }

        public void Close()
        {
            _view.gameObject.SetActive(false);
        }

        protected override void OnDispose()
        {
            var rows = _buildingRows.Values.ToArray();
            for (int i = 0; i < rows.Length; i++)
            {
                rows[_buildingRows.Count - 1 - i].Dispose();
            }
            if(_view != null)
                GameObject.Destroy(_view);
            base.OnDispose();
        }
    }
}