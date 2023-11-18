using System;
using System.Collections.Generic;
using Containers.Modificators;
using Core;
using Logic.Idle.Workers;
using Tools.Extensions;
using UniRx;

namespace Logic.Model
{
    public class ApplyModificator : BaseDisposable
    {
        public struct Ctx
        {
            public IReadOnlyReactiveCollection<WorkerModel> workers;
            public IReactiveCollection<ModificatorInfo> modificators;
        }

        private readonly Ctx _ctx;
        private Dictionary<ModificatorInfo, IDisposable> _disposables;

        public ApplyModificator(Ctx ctx)
        {
            _ctx = ctx;
            _disposables = new Dictionary<ModificatorInfo, IDisposable>();
            
            DateTime currentTime = DateTime.Now;
            for (int i = _ctx.modificators.Count - 1; i > 0; i--)
            {
                float timebeToEnd = currentTime.Subtract(_ctx.modificators[i].start).Seconds;
                if (timebeToEnd > _ctx.modificators[i].duration)
                {
                    _ctx.modificators.RemoveAt(i);
                    continue;
                }
                
                IDisposable disposable = ReactiveExtensions.DelayedCall(timebeToEnd, 
                    () => OnModificatorTimeEnded(_ctx.modificators[i]));

                _disposables[_ctx.modificators[i]] = disposable;
            }

            foreach (var workerModel in _ctx.workers)
            {
                ApplyAllModificators(workerModel);
            }

            AddDispose(_ctx.workers.ObserveAdd().Subscribe(OnAddWorer));
            AddDispose(_ctx.modificators.ObserveAdd().Subscribe(OnAddModificator));
        }

        private void OnAddWorer(CollectionAddEvent<WorkerModel> addEvent)
        {
            foreach (var modificatorInfo in _ctx.modificators)
            {
                ApplyModificatorToWorker(addEvent.Value, modificatorInfo);
            }
        }
        
        private void OnAddModificator(CollectionAddEvent<ModificatorInfo> addEvent)
        {
            foreach (var workerModel in _ctx.workers)
            {
                ApplyModificatorToWorker(workerModel,addEvent.Value);
            }
        }

        private void OnModificatorTimeEnded(ModificatorInfo modificatorInfo)
        {
            foreach (var workerModel in _ctx.workers)
            {
                RemoveModificatorFromWorker(workerModel, modificatorInfo);
            }
        }

        private void ApplyAllModificators(WorkerModel workerModel)
        {
            workerModel.MoneyIncome.Value = workerModel.Info.Value.baseIncomeMoney;
            workerModel.WorkIncome.Value = workerModel.Info.Value.baseIncomeWork;
            workerModel.TimeSpeed.Value = workerModel.Info.Value.baseTimeToWork;
            
            DateTime currentTime = DateTime.Now;
            foreach (var modificator in _ctx.modificators)
            {
                ApplyModificatorToWorker(workerModel, modificator);
            }
        }
        
        private void RemoveModificatorFromWorker(WorkerModel model, ModificatorInfo modificator)
        {
            if (modificator is EffectiencyModificatorInfo effectiencyModificatorInfo)
            {
                model.MoneyIncome.Value -= model.Info.Value.baseIncomeMoney * effectiencyModificatorInfo.incomeMoneyModifier;
                model.WorkIncome.Value -= model.Info.Value.baseIncomeWork * effectiencyModificatorInfo.incomeWorkModifier;
            }
            else if (modificator is TimeSpeedModificatorInfo timeSpeedModificatorInfo)
            {
                model.TimeSpeed.Value -= model.Info.Value.baseTimeToWork * timeSpeedModificatorInfo.timeSpeedModifier;
            }
        }

        private void ApplyModificatorToWorker(WorkerModel model, ModificatorInfo modificator)
        {
            if (modificator is EffectiencyModificatorInfo effectiencyModificatorInfo)
            {
                model.MoneyIncome.Value += model.Info.Value.baseIncomeMoney * effectiencyModificatorInfo.incomeMoneyModifier;
                model.WorkIncome.Value += model.Info.Value.baseIncomeWork * effectiencyModificatorInfo.incomeWorkModifier;
            }
            else if (modificator is TimeSpeedModificatorInfo timeSpeedModificatorInfo)
            {
                model.TimeSpeed.Value += model.Info.Value.baseTimeToWork * timeSpeedModificatorInfo.timeSpeedModifier;
            }
        }

        protected override void OnDispose()
        {
            foreach (var modificator in _ctx.modificators)
            {
                _disposables[modificator]?.Dispose();
            }
            base.OnDispose();
        }
    }
}