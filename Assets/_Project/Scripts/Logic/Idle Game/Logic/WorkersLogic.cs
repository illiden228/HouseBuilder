using System;
using System.Collections.Generic;
using Core;
using Logic.Idle.Workers;
using Tools.Extensions;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

namespace Logic.Model
{
    public class WorkersLogic : BaseDisposable
    {
        public struct Ctx
        {
            public IReactiveProperty<int> moneys;
            public IReactiveProperty<BuildProgressModel> currentBuild;
            public IReadOnlyReactiveCollection<WorkerModel> workers;
        }

        private readonly Ctx _ctx;
        private Dictionary<WorkerModel, IDisposable> _disposables;

        public WorkersLogic(Ctx ctx)
        {
            _ctx = ctx;
            _disposables = new Dictionary<WorkerModel, IDisposable>();

            foreach (var workerModel in _ctx.workers)
            {
                OnAddWorker(workerModel);
            }

            AddDispose(_ctx.workers.ObserveAdd().Subscribe(addEvent => OnAddWorker(addEvent.Value)));
            AddDispose(_ctx.workers.ObserveRemove().Subscribe(removeEvent => OnRemoveWorker(removeEvent.Value)));
        }

        private void OnAddWorker(WorkerModel workerModel)
        {
            float time = Time.time + workerModel.TimeSpeed.Value;
            IDisposable sub = ReactiveExtensions.StartUpdate(() =>
            {
                if (Time.time >= time)
                {
                    _ctx.moneys.Value += workerModel.MoneyIncome.Value;
                    _ctx.currentBuild.Value.CurrentFloor.Value.CurrentWorkCount.Value += workerModel.WorkIncome.Value;
                    time = Time.time + workerModel.TimeSpeed.Value;
                }
            });
            
            _disposables[workerModel] = sub;
        }
        
        private void OnRemoveWorker(WorkerModel workerModel)
        {
            _disposables[workerModel]?.Dispose();
        }

        protected override void OnDispose()
        {
            foreach (var workerModel in _ctx.workers)
            {
                _disposables[workerModel]?.Dispose();
            }
            base.OnDispose();
        }
    }
}