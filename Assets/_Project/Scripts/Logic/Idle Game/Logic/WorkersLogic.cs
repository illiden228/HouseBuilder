using System;
using System.Collections.Generic;
using Core;
using Logic.Idle.Workers;
using Tools.Extensions;
using UniRx;
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
            IDisposable sub = ReactiveExtensions.StartUpdate(() =>
            {
                workerModel.CurrentIncomeTime.Value += Time.deltaTime;
                if (workerModel.TimeSpeed.Value <= workerModel.CurrentIncomeTime.Value)
                {
                    _ctx.moneys.Value += workerModel.MoneyIncome.Value * workerModel.Grade.Value.number;
                    _ctx.currentBuild.Value.CurrentFloor.Value.CurrentWorkCount.Value += workerModel.WorkIncome.Value * workerModel.Grade.Value.number;
                    workerModel.CurrentIncomeTime.Value = 0;
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