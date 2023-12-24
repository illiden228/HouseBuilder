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
            public IReactiveProperty<int> moneys;
            public GameConfig gameConfig;
        }

        private readonly Ctx _ctx;
        private const string VIEW_PREFAB_NAME = "WorkerMonitorView";
        
        private WorkerMonitorView _view;
        private Dictionary<WorkerModel, WorkerMonitorRowPm> _workerRows;
        private ReactiveProperty<int> _workersCount;
        
        public WorkerMonitorPm(Ctx ctx)
        {
            _ctx = ctx;
            _workerRows = new Dictionary<WorkerModel, WorkerMonitorRowPm>();
            _workersCount = new ReactiveProperty<int>();

            AddDispose(_ctx.resourceLoader.LoadPrefab("fakebundles", VIEW_PREFAB_NAME, OnPrefabLoaded));
            AddDispose(_ctx.workers.ObserveAdd().Subscribe(OnAddWorker));
            AddDispose(_ctx.workers.ObserveRemove().Subscribe(OnRemoveWorker));
            AddDispose(_ctx.workers.ObserveCountChanged().Subscribe(count => _workersCount.Value = count));
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
                addWorker = AddWorker,
                effectiencyUp = () => _ctx.currentEffectiencyLevel.Value++,
                speedUp = () => _ctx.currentSpeedLevel.Value++,
                merge = () => Debug.Log("Try Merge"),
                effectiencyLevel = _ctx.currentEffectiencyLevel,
                speedLevel = _ctx.currentSpeedLevel,
                moneys = _ctx.moneys,
                workersCount = _workersCount
            });
            
            foreach (var workerModel in _ctx.workers)
            {
                CreateWorkerMonitorRow(workerModel);
            }
        }

        private void AddWorker()
        {
            WorkerInfo workerInfo = _ctx.gameConfig.workerConfig.GetStartWorkerInfo();
            WorkerModel model = new WorkerModel(workerInfo);
            
            _ctx.workers.Add(model);
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