using System;
using System.Collections.Generic;
using Containers.Modificators;
using Core;
using Logic.Idle.Workers;
using Tools.Extensions;
using UniRx;

namespace Logic.Model
{
    public class ApplyModificatorLogic : BaseDisposable
    {
        public struct Ctx
        {
            public IReadOnlyReactiveCollection<WorkerModel> workers;
            public IReactiveCollection<ModificatorInfo> modificators;
        }

        private readonly Ctx _ctx;
        private Dictionary<ModificatorInfo, IDisposable> _modificatorSubs;
        private Dictionary<WorkerModel, List<IDisposable>> _workerSubs;

        public ApplyModificatorLogic(Ctx ctx)
        {
            _ctx = ctx;
            _modificatorSubs = new Dictionary<ModificatorInfo, IDisposable>();
            _workerSubs = new Dictionary<WorkerModel, List<IDisposable>>();
            
            DateTime currentTime = DateTime.Now; // TODO: переделать подсчет на Time.deltaTime??
            for (int i = _ctx.modificators.Count - 1; i > 0; i--)
            {
                if(_ctx.modificators[i].duration < 0)
                    continue;
                
                float timebeToEnd = currentTime.Subtract(_ctx.modificators[i].start).Seconds;
                if (timebeToEnd > _ctx.modificators[i].duration)
                {
                    _ctx.modificators.RemoveAt(i);
                    continue;
                }
                
                IDisposable disposable = ReactiveExtensions.DelayedCall(timebeToEnd, 
                    () => OnModificatorTimeEnded(_ctx.modificators[i]));

                _modificatorSubs[_ctx.modificators[i]] = disposable;
            }

            foreach (var workerModel in _ctx.workers)
            {
                ApplyAllModificators(workerModel);

                if (!_workerSubs.ContainsKey(workerModel))
                    _workerSubs[workerModel] = new List<IDisposable>();
                
                // TODO: дописать случаи, когда изменилась базовая скорость, нужно заново перерасчет для модели 
            }

            AddDispose(_ctx.workers.ObserveAdd().Subscribe(OnAddWorer));
            AddDispose(_ctx.modificators.ObserveAdd().Subscribe(OnAddModificator));
        }

        private void OnAddWorer(CollectionAddEvent<WorkerModel> addEvent)
        {
            if (!_workerSubs.ContainsKey(addEvent.Value))
                _workerSubs[addEvent.Value] = new List<IDisposable>();
            
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
            foreach (var modificator in _ctx.modificators)
            {
                ApplyModificatorToWorker(workerModel, modificator);
            }
        }
        
        private void RemoveModificatorFromWorker(WorkerModel model, ModificatorInfo modificator)
        {
            if (modificator is EffectiencyModificatorInfo effectiencyModificatorInfo)
            {
                RemoveMoneyIncomeToWorker(model, effectiencyModificatorInfo);
                RemoveWorkIncomeToWorker(model, effectiencyModificatorInfo);
            }
            else if (modificator is TimeSpeedModificatorInfo timeSpeedModificatorInfo)
            {
                RemoveTimeSpeedToWorker(model, timeSpeedModificatorInfo);
            }
        }

        private void ApplyModificatorToWorker(WorkerModel model, ModificatorInfo modificator)
        {
            if (modificator is EffectiencyModificatorInfo effectiencyModificatorInfo)
            {
                AddMoneyIncomeToWorker(model, effectiencyModificatorInfo);
                AddWorkIncomeToWorker(model, effectiencyModificatorInfo);
            }
            else if (modificator is TimeSpeedModificatorInfo timeSpeedModificatorInfo)
            {
                AddTimeSpeedToWorker(model, timeSpeedModificatorInfo);
            }
        }

        private void AddMoneyIncomeToWorker(WorkerModel model, EffectiencyModificatorInfo modificator)
        {
            model.MoneyIncome.Value += model.BaseMoneyIncome.Value * modificator.incomeMoneyModifier;
        }
        
        private void AddWorkIncomeToWorker(WorkerModel model, EffectiencyModificatorInfo modificator)
        {
            model.WorkIncome.Value += model.BaseWorkIncome.Value * modificator.incomeWorkModifier;
        }
        
        private void AddTimeSpeedToWorker(WorkerModel model, TimeSpeedModificatorInfo modificator)
        {
            model.TimeSpeed.Value += model.BaseTimeSpeed.Value * modificator.timeSpeedModifier;
        }
        
        private void RemoveMoneyIncomeToWorker(WorkerModel model, EffectiencyModificatorInfo modificator)
        {
            model.MoneyIncome.Value -= model.BaseMoneyIncome.Value * modificator.incomeMoneyModifier;
        }
        
        private void RemoveWorkIncomeToWorker(WorkerModel model, EffectiencyModificatorInfo modificator)
        {
            model.WorkIncome.Value -= model.BaseWorkIncome.Value * modificator.incomeWorkModifier;
        }
        
        private void RemoveTimeSpeedToWorker(WorkerModel model, TimeSpeedModificatorInfo modificator)
        {
            model.TimeSpeed.Value -= model.BaseTimeSpeed.Value * modificator.timeSpeedModifier;
        }

        protected override void OnDispose()
        {
            foreach (var modificator in _ctx.modificators)
            {
                if(_modificatorSubs.ContainsKey(modificator))
                    _modificatorSubs[modificator]?.Dispose();
            }
            
            foreach (var worker in _ctx.workers)
            {
                if(!_workerSubs.TryGetValue(worker, out var subs))
                    continue;
                for (int i = subs.Count; i > 0; i--)
                {
                    subs[i]?.Dispose();
                }
                
            }
            base.OnDispose();
        }
    }
}