using System;
using System.Collections.Generic;
using Core;
using Tools.Extensions;
using UniRx;
using UnityEngine;

namespace Logic.Model
{
    public class BuildingsLogic : BaseDisposable
    {
        public struct Ctx
        {
            public IReactiveProperty<int> moneys;
            public IReadOnlyReactiveCollection<BuildingModel> buildings;
        }

        private readonly Ctx _ctx;
        private Dictionary<BuildingModel, IDisposable> _disposables;

        public BuildingsLogic(Ctx ctx)
        {
            _ctx = ctx;
            _disposables = new Dictionary<BuildingModel, IDisposable>();

            foreach (var buildingModel in _ctx.buildings)
            {
                OnAddBuilding(buildingModel);
            }

            AddDispose(_ctx.buildings.ObserveAdd().Subscribe(addEvent => OnAddBuilding(addEvent.Value)));
            AddDispose(_ctx.buildings.ObserveRemove().Subscribe(removeEvent => OnRemoveBuilding(removeEvent.Value)));
        }

        private void OnAddBuilding(BuildingModel buildingModel)
        {
            IDisposable sub = ReactiveExtensions.StartUpdate(() =>
            {
                buildingModel.CurrentIncomeTime.Value += Time.deltaTime;
                if (buildingModel.TimeSpeed.Value <= buildingModel.CurrentIncomeTime.Value)
                {
                    _ctx.moneys.Value += buildingModel.MoneyIncome.Value;
                    buildingModel.CurrentIncomeTime.Value = 0;
                    Debug.Log($"Income from buildong {buildingModel.Info.Value.id} {buildingModel.MoneyIncome.Value} moneys");
                }
            });
            
            _disposables[buildingModel] = sub;
        }
        
        private void OnRemoveBuilding(BuildingModel buildingModel)
        {
            _disposables[buildingModel]?.Dispose();
        }

        protected override void OnDispose()
        {
            foreach (var buildingModel in _ctx.buildings)
            {
                _disposables[buildingModel]?.Dispose();
            }
            base.OnDispose();
        }
    }

}