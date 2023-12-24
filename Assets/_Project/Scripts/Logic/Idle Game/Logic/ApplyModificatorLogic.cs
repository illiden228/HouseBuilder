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
                
                float timebeToEnd = currentTime.Subtract(_ctx.modificators[i].start).Seconds; // TODO: откуда этот старт берется? Задавать при осздании?
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
                ApplyAllModificatorsToWorker(workerModel);

                if (!_workerSubs.ContainsKey(workerModel))
                    _workerSubs[workerModel] = new List<IDisposable>();
                
                // TODO: дописать случаи, когда изменилась базовая скорость, нужно заново перерасчет для модели 
            }

            AddDispose(_ctx.workers.ObserveAdd().Subscribe(OnAddWorer));
            AddDispose(_ctx.modificators.ObserveAdd().Subscribe(OnAddModificator));
            AddDispose(_ctx.modificators.ObserveRemove().Subscribe(OnRemoveModificator));
        }

        private void OnAddWorer(CollectionAddEvent<WorkerModel> addEvent)
        {
            if (!_workerSubs.ContainsKey(addEvent.Value))
                _workerSubs[addEvent.Value] = new List<IDisposable>();
            
            ApplyAllModificatorsToWorker(addEvent.Value);
        }
        
        private void OnRemoveModificator(CollectionRemoveEvent<ModificatorInfo> removeEvent)
        {
            RemoveModificatorFromAllWorker(removeEvent.Value);
        }
        
        private void OnAddModificator(CollectionAddEvent<ModificatorInfo> addEvent)
        {
            if (_ctx.modificators.TryGetValueById(addEvent.Value, out var existModificatorInfo))
            {
                _ctx.modificators.Remove(existModificatorInfo);
                if(_modificatorSubs.TryGetValue(existModificatorInfo, out var disposable))
                    disposable?.Dispose();
            }

            if (addEvent.Value.duration > -1)
            {
                // IDisposable disposable = ReactiveExtensions.DelayedCall(addEvent.Value.duration, 
                //     () => OnModificatorTimeEnded(_ctx.modificators[i]));
                //
                // _modificatorSubs[addEvent.Value] = disposable; // TODO: тут бы применить время модификатора (когда временные модификаторы доделаю). Временный модификатор - нужна модель + в отдельный класс
            }
                

            ApplyModificatorToAllWorker(addEvent.Value);
        }

        private void OnModificatorTimeEnded(ModificatorInfo modificatorInfo)
        {
            RemoveModificatorFromAllWorker(modificatorInfo);
        }
        
        private void RemoveModificatorFromAllWorker(ModificatorInfo modificatorInfo)
        {
            foreach (var workerModel in _ctx.workers)
            {
                RemoveModificatorFromWorker(workerModel, modificatorInfo);
            }
        }
        
        private void ApplyModificatorToAllWorker(ModificatorInfo modificatorInfo)
        {
            foreach (var workerModel in _ctx.workers)
            {
                ApplyModificatorToWorker(workerModel, modificatorInfo);
            }
        }
        
        private void ApplyAllModificatorsToAllWorker()
        {
            foreach (var workerModel in _ctx.workers)
            {
                ApplyAllModificatorsToWorker(workerModel);
            }
        }

        private void ApplyAllModificatorsToWorker(WorkerModel workerModel)
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