using System.Collections.Generic;
using Containers;
using Containers.Data;
using Core;
using Logic.Idle.Workers;
using Logic.Profile;
using UniRx;
using UnityEngine;

namespace Logic.Model
{
    public class UpgradeLogic : BaseDisposable
    {
        public struct Ctx
        {
            public ProfileClient profile;
            public GameConfig config;
        }

        private readonly Ctx _ctx;
        private int _currentGradeForMerge;

        private UpgradeModel UpgradeModel => _ctx.profile.UpgradeModel;
        private ReactiveProperty<int> EffectiencyUpCost => UpgradeModel.EffectiencyUpCost;
        private ReactiveProperty<int> TimeSpeedUpCost => UpgradeModel.TimeSpeedUpCost;
        private ReactiveProperty<int> AddWorkerCost => UpgradeModel.AddWorkerCost;
        private ReactiveProperty<int> MergeCost => UpgradeModel.MergeCost;

        public UpgradeLogic(Ctx ctx)
        {
            _ctx = ctx;

            LevelUpLogic<int>.Ctx effectiencyUpLogicCtx = new LevelUpLogic<int>.Ctx
            {
                curerntLevel = _ctx.profile.CurrentEffectiencyLevel,
                currentPrice = UpgradeModel.CurrentEffectiencyUpPrice,
                currentProperty = _ctx.profile.CurrentEffectiency,
                priceConfig = _ctx.config.prices.nextLevelPrices,
                valuesConfig = _ctx.config.workerConfig.moneyIncomeValues,
            };
            AddDispose(new LevelUpLogic<int>(effectiencyUpLogicCtx));
            
            LevelUpLogic<float>.Ctx timeSpeedUpLogicCtx = new LevelUpLogic<float>.Ctx
            {
                curerntLevel = _ctx.profile.CurrentTimeSpeedLevel,
                currentPrice = UpgradeModel.CurrentTimeSpeedUpPrice,
                currentProperty = _ctx.profile.CurrentTimeSpeed,
                priceConfig = _ctx.config.prices.nextLevelPrices,
                valuesConfig = _ctx.config.workerConfig.timeSpeedValues,
            };
            AddDispose(new LevelUpLogic<float>(timeSpeedUpLogicCtx));
            
            LevelUpLogic<int>.Ctx addWorkerUpLogicCtx = new LevelUpLogic<int>.Ctx
            {
                curerntLevel = _ctx.profile.CurrentAddWorkerLevel,
                currentPrice = UpgradeModel.CurrentAddWorkerPrice,
                priceConfig = _ctx.config.prices.nextLevelPrices,
            };
            AddDispose(new LevelUpLogic<int>(addWorkerUpLogicCtx));
            
            LevelUpLogic<int>.Ctx mergeLogicCtx = new LevelUpLogic<int>.Ctx
            {
                curerntLevel = _ctx.profile.CurrentMergeLevel,
                currentPrice = UpgradeModel.CurrentMergePrice,
                priceConfig = _ctx.config.prices.mergeUpPrices,
            };
            AddDispose(new LevelUpLogic<int>(mergeLogicCtx));

            CostChangeLogic.Ctx costChangeCtx = new CostChangeLogic.Ctx
            {
                profile = _ctx.profile
            };
            AddDispose(new CostChangeLogic(costChangeCtx));

            AddDispose(UpgradeModel.EffectiencyUp.SubscribeWithSkip(OnEffectiencyUpgrade));
            AddDispose(UpgradeModel.TimeSpeedUp.SubscribeWithSkip(OnTimeSpeedUpgrade));
            AddDispose(UpgradeModel.AddWorker.SubscribeWithSkip(OnAddWorker));
            AddDispose(UpgradeModel.Merge.SubscribeWithSkip(OnMerge));
            
            AddDispose(_ctx.profile.CurrentEffectiency.Skip(1).Subscribe(ChangeBaseEffectiency));
            AddDispose(_ctx.profile.CurrentTimeSpeed.Skip(1).Subscribe(ChangeBaseSpeed));
            AddDispose(_ctx.profile.CurrentAddWorkerLevel.Skip(1).Subscribe(AddWorker));
            AddDispose(_ctx.profile.CurrentMergeLevel.Skip(1).Subscribe(_ => Merge()));
        }

        private void OnEffectiencyUpgrade()
        {
            TryUpgradeProperty(_ctx.profile.CurrentEffectiencyLevel, EffectiencyUpCost);
        }
        
        private void OnTimeSpeedUpgrade()
        {
            TryUpgradeProperty(_ctx.profile.CurrentTimeSpeedLevel, TimeSpeedUpCost);
        }
        
        private void OnAddWorker()
        {
            TryUpgradeProperty(_ctx.profile.CurrentAddWorkerLevel, AddWorkerCost);
        }
        
        private void OnMerge()
        {
            if (!_ctx.profile.CanMerge(out int grade))
                return;
            _currentGradeForMerge = grade;
            if(!TryUpgradeProperty(_ctx.profile.CurrentMergeLevel, MergeCost))
                return;

        }

        private bool TryUpgradeProperty(IReactiveProperty<int> upgradeProperty, IReactiveProperty<int> cost)
        {
            if(_ctx.profile.Moneys.Value < cost.Value)
                return false;

            upgradeProperty.Value++;
            UpgradeModel.UpgradesCount.Value++;
            
            return true;
        }

        private void AddWorker(int count)
        {
            WorkerInfo workerInfo = _ctx.config.workerConfig.GetStartWorkerInfo();
            WorkerModel model = new WorkerModel(workerInfo);
            
            _ctx.profile.Workers.Add(model);
        }

        private void ChangeBaseSpeed(float newSpeed)
        {
            foreach (var worker in _ctx.profile.Workers)
            {
                worker.BaseTimeSpeed.Value = newSpeed;
            }
        }
        
        private void ChangeBaseEffectiency(int newEffectiency)
        {
            foreach (var worker in _ctx.profile.Workers)
            {
                worker.BaseMoneyIncome.Value = newEffectiency;
                worker.BaseWorkIncome.Value = newEffectiency;
            }
        }
        
        private void Merge()
        {
            IReactiveCollection<WorkerModel> allWorkers = _ctx.profile.Workers;

            if (_currentGradeForMerge == 0)
            {
                if(!_ctx.profile.CanMerge(out _currentGradeForMerge))
                    return;
            }
            
            List<WorkerModel> mergeWorkers = _ctx.profile.GetWorkersForMerge(_currentGradeForMerge);

            if (mergeWorkers == null)
            {
                Debug.LogError("Workers for merge is null");
                return;
            }
            
            WorkerModel newGradeWorker = mergeWorkers[0];
            newGradeWorker.Grade.Value++;

            foreach (var mergeWorker in mergeWorkers)
            {
                allWorkers.Remove(mergeWorker);
            }
            
            allWorkers.Add(newGradeWorker);
            _currentGradeForMerge = 0;
        }

        
    }
}