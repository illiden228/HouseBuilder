using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Logic.Model;
using UniRx;
using UnityEngine;

namespace Logic.Idle.Monitors
{
    public class BuildingProgressMonitorPm : BaseDisposable, IMonitor
    {
        public struct Ctx
        {
            public Transform uiParent;
            public IResourceLoader resourceLoader;
            public IReactiveProperty<BuildProgressModel> currentBuilding;
            public IReactiveCollection<BuildProgressModel> queueBuildProgress;
            public IReactiveCollection<BuildingModel> buildings;
            public Action back;
        }

        private readonly Ctx _ctx;
        private BuildingProgressMonitorView _view;
        private const string VIEW_PREFAB_NAME = "BuildingProgressMonitorView";
        private ReactiveProperty<int> _currentFloorMaxWorkCount;
        private ReactiveProperty<int> _currentFloorWorkCount;
        private ReactiveProperty<int> _currentBuildingFloorsCount;
        private ReactiveProperty<int> _currentBuildingMaxFloorCount;
        private IDisposable _workCountSub;
        private IDisposable _floorsCountSub;
        private Dictionary<BuildProgressModel, IDisposable> _raws;
        
        public BuildingProgressMonitorPm(Ctx ctx)
        {
            _ctx = ctx;
            _currentFloorMaxWorkCount = new ReactiveProperty<int>();
            _currentFloorWorkCount = new ReactiveProperty<int>();
            _currentBuildingFloorsCount = new ReactiveProperty<int>();
            _currentBuildingMaxFloorCount = new ReactiveProperty<int>();
            _raws = new Dictionary<BuildProgressModel, IDisposable>();
            
            AddDispose(_ctx.resourceLoader.LoadPrefab("fakebundles", VIEW_PREFAB_NAME, OnPrefabLoaded));
            ProgressSubs();
        }

        private void OnPrefabLoaded(GameObject prefab)
        {
            _view = GameObject.Instantiate(prefab, _ctx.uiParent).GetComponent<BuildingProgressMonitorView>();
            
            MonitorPanelView.BaseCtx basePanelCtx = new MonitorPanelView.BaseCtx
            {
                viewDisposable = AddDispose(new CompositeDisposable()),
                back = _ctx.back
            };
            
            _view.Init(basePanelCtx, new BuildingProgressMonitorView.Ctx
            {
                currentFloorWork = _currentFloorWorkCount,
                currentFloorMaxWork = _currentFloorMaxWorkCount,
                currentFloor = _currentBuildingFloorsCount,
                maxFloors = _currentBuildingMaxFloorCount,
            });

            SetQueue();
            AddDispose(_ctx.queueBuildProgress.ObserveAdd().Subscribe(OnAddBuildingModel));
            AddDispose(_ctx.queueBuildProgress.ObserveRemove().Subscribe(OnRemoveBuildingModel));
        }

        private void SetQueue()
        {
            foreach (var progressModel in _ctx.queueBuildProgress)
            {
                _raws[progressModel] = CreateRaw(progressModel);
            }
        }

        private void OnAddBuildingModel(CollectionAddEvent<BuildProgressModel> addEvent)
        {
            _raws[addEvent.Value] = CreateRaw(addEvent.Value);
        }
        
        private void OnRemoveBuildingModel(CollectionRemoveEvent<BuildProgressModel> removeEvent)
        {
            _raws[removeEvent.Value]?.Dispose();
            _raws.Remove(removeEvent.Value);
        }

        private BuildProgressQueueRawPm CreateRaw(BuildProgressModel progressModel)
        {
            BuildProgressQueueRawPm.Ctx rawCtx = new BuildProgressQueueRawPm.Ctx
            {
                resourceLoader = _ctx.resourceLoader,
                floorsCount = progressModel.Building.Value.CurrentFloorsCount,
                build = () =>
                {
                    _ctx.buildings.Add(progressModel.Building.Value);
                    _ctx.queueBuildProgress.Remove(progressModel);
                },
                uiParent = _view.Container,
                income = progressModel.Building.Value.MoneyIncome,
                maxFloorsCount = progressModel.CurrentFloorIndex
            };
            return new BuildProgressQueueRawPm(rawCtx);
        }

        private void ProgressSubs()
        {
            AddDispose(_ctx.currentBuilding.Subscribe(buildingProgress =>
            {
                _currentBuildingMaxFloorCount.Value = buildingProgress.Building.Value.Info.Value.floors.Count;
                _currentBuildingFloorsCount.Value = 0;
                
                _workCountSub?.Dispose();
                _workCountSub = _ctx.currentBuilding.Value.CurrentFloor.Value.CurrentWorkCount.Subscribe(work =>
                {
                    _currentFloorWorkCount.Value = _ctx.currentBuilding.Value.CurrentFloor.Value.CurrentWorkCount.Value;
                });
                
                _floorsCountSub?.Dispose();
                _floorsCountSub = _ctx.currentBuilding.Value.Building.Value.CurrentFloorsCount.Subscribe(floors =>
                {
                    _currentFloorMaxWorkCount.Value = _ctx.currentBuilding.Value.CurrentFloor.Value.Info.Value.maxWorkCount;
                    _currentBuildingFloorsCount.Value = floors;
                });
            }));
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
            _floorsCountSub?.Dispose();
            _workCountSub?.Dispose();
            var rows = _raws.Values.ToArray();
            for (int i = 0; i < rows.Length; i++)
            {
                rows[_raws.Count - 1 - i].Dispose();
            }
            if(_view != null)
                GameObject.Destroy(_view);
            base.OnDispose();
        }
    }
}