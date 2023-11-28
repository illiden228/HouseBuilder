using System;
using System.Collections.Generic;
using Containers;
using Containers.Data;
using Core;
using Logic.Idle.Workers;
using UniRx;
using UnityEngine;

namespace Logic.Idle.Monitor_and_Cheat
{
    public class WorkerMonitorPm : BaseDisposable
    {
        public struct Ctx
        {
            public Transform uiParent;
            public IResourceLoader resourceLoader;
            public Action back;
            public IReactiveCollection<WorkerModel> workers;
            public GameConfig gameConfig;
        }

        private readonly Ctx _ctx;
        private const string VIEW_PREFAB_NAME = "WorkerMonitorView";
        
        private WorkerMonitorView _view;
        private List<WorkerMonitorRowPm> _workerRows;
        
        public WorkerMonitorPm(Ctx ctx)
        {
            _ctx = ctx;
            _workerRows = new List<WorkerMonitorRowPm>();

            AddDispose(_ctx.resourceLoader.LoadPrefab("fakebundles", VIEW_PREFAB_NAME, OnPrefabLoaded));
            AddDispose(_ctx.workers.ObserveAdd().Subscribe(OnAddWorker));
        }

        private void OnPrefabLoaded(GameObject prefab)
        {
            _view = GameObject.Instantiate(prefab, _ctx.uiParent).GetComponent<WorkerMonitorView>();
            
            _view.Init(new WorkerMonitorView.Ctx
            {
                viewDisposable = AddDispose(new CompositeDisposable()),
                back = _ctx.back,
                addWorker = AddWorker
            });
            
            foreach (var workerModel in _ctx.workers)
            {
                CreateWorkerMonitorRow(workerModel);
            }
        }

        private void AddWorker()
        {
            WorkerInfo workerInfo = new WorkerInfo
            {
                id = "worker_test",
                grade = _ctx.gameConfig.Grades[0],
                baseIncomeMoney = _ctx.gameConfig.BaseWorkerMoneyIncome,
                baseIncomeWork = _ctx.gameConfig.BaseWorkerWorkIncome,
                baseTimeToWork = _ctx.gameConfig.BaseWorkerTimeSpeed,
            };

            WorkerModel model = new WorkerModel(workerInfo);
            
            _ctx.workers.Add(model);
        }

        private void OnAddWorker(CollectionAddEvent<WorkerModel> addEvent)
        {
            CreateWorkerMonitorRow(addEvent.Value);
        }

        private void CreateWorkerMonitorRow(WorkerModel model)
        {
            WorkerMonitorRowPm.Ctx workerMonitorRowCtx = new WorkerMonitorRowPm.Ctx
            {
                model = model,
                resourceLoader = _ctx.resourceLoader,
                uiParent = _view.Container
            };
            
            _workerRows.Add(new WorkerMonitorRowPm(workerMonitorRowCtx));
        }

        protected override void OnDispose()
        {
            for (int i = 0; i < _workerRows.Count; i++)
            {
                _workerRows[_workerRows.Count - 1 - i].Dispose();
            }
            base.OnDispose();
        }
    }
}