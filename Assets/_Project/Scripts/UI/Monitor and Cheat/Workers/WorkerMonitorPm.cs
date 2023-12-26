using System;
using System.Collections.Generic;
using System.Linq;
using Containers;
using Containers.Data;
using Containers.Modificators;
using Core;
using Logic.Idle.Workers;
using UniRx;
using UnityEngine;

namespace Logic.Idle.Monitors
{
    public class WorkerMonitorPm : BaseDisposable, IMonitor
    {
        public struct Ctx
        {
            public Transform uiParent;
            public IResourceLoader resourceLoader;
            public Action back;
            public IReactiveCollection<WorkerModel> workers;
            public IReactiveProperty<int> currentEffectiencyLevel;
            public IReactiveProperty<int> currentSpeedLevel;
            public IReactiveProperty<int> currentMergeLevel;
            public IReactiveProperty<int> currentAddWorkerLevel;
            public IReactiveProperty<int> moneys;
        }

        private readonly Ctx _ctx;
        private const string VIEW_PREFAB_NAME = "WorkerMonitorView";
        
        private WorkerMonitorView _view;
        private Dictionary<WorkerModel, WorkerMonitorRowPm> _workerRows;
        
        public WorkerMonitorPm(Ctx ctx)
        {
            _ctx = ctx;
            _workerRows = new Dictionary<WorkerModel, WorkerMonitorRowPm>();

            AddDispose(_ctx.resourceLoader.LoadPrefab("fakebundles", VIEW_PREFAB_NAME, OnPrefabLoaded));
            AddDispose(_ctx.workers.ObserveAdd().Subscribe(OnAddWorker));
            AddDispose(_ctx.workers.ObserveRemove().Subscribe(OnRemoveWorker));
        }

        public void Open()
        {
            _view.gameObject.SetActive(true);
        }

        public void Close()
        {
            _view.gameObject.SetActive(false);
        }

        private void OnPrefabLoaded(GameObject prefab)
        {
            _view = GameObject.Instantiate(prefab, _ctx.uiParent).GetComponent<WorkerMonitorView>();

            MonitorPanelView.BaseCtx basePanelCtx = new MonitorPanelView.BaseCtx
            {
                viewDisposable = AddDispose(new CompositeDisposable()),
                back = _ctx.back
            };
            
            _view.Init(basePanelCtx, new WorkerMonitorView.Ctx
            {
                addWorker = () => _ctx.currentAddWorkerLevel.Value++,
                effectiencyUp = () => _ctx.currentEffectiencyLevel.Value++,
                speedUp = () => _ctx.currentSpeedLevel.Value++,
                merge = () => _ctx.currentMergeLevel.Value++,
                effectiencyLevel = _ctx.currentEffectiencyLevel,
                speedLevel = _ctx.currentSpeedLevel,
                moneys = _ctx.moneys,
                workersCount = _ctx.currentAddWorkerLevel
            });
            
            foreach (var workerModel in _ctx.workers)
            {
                CreateWorkerMonitorRow(workerModel);
            }
        }

        private void OnAddWorker(CollectionAddEvent<WorkerModel> addEvent)
        {
            CreateWorkerMonitorRow(addEvent.Value);
        }
        
        private void OnRemoveWorker(CollectionRemoveEvent<WorkerModel> removeEvent)
        {
            if(_workerRows.ContainsKey(removeEvent.Value))
                _workerRows[removeEvent.Value]?.Dispose();
        }

        private void CreateWorkerMonitorRow(WorkerModel model)
        {
            WorkerMonitorRowPm.Ctx workerMonitorRowCtx = new WorkerMonitorRowPm.Ctx
            {
                model = model,
                resourceLoader = _ctx.resourceLoader,
                uiParent = _view.Container
            };
            
            _workerRows[model] = new WorkerMonitorRowPm(workerMonitorRowCtx);
        }

        protected override void OnDispose()
        {
            var rows = _workerRows.Values.ToArray();
            for (int i = 0; i < rows.Length; i++)
            {
                rows[_workerRows.Count - 1 - i].Dispose();
            }
            base.OnDispose();
        }
    }
}