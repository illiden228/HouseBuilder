using System;
using System.Collections.Generic;
using Containers;
using Core;
using Tools.Extensions;
using UniRx;
using UnityEngine;

namespace Logic.Model
{
    public class BuildProgressLogic : BaseDisposable
    {
        public struct Ctx
        {
            public List<BuildingInfo> config;
            public IReactiveProperty<int> currentBuildIndex;
            public IReactiveProperty<BuildProgressModel> currentBuild;
            public IReactiveCollection<BuildProgressModel> queueBuildProgress;
        }

        private readonly Ctx _ctx;
        private IDisposable _workSub;
        private IDisposable _floorSub;

        public BuildProgressLogic(Ctx ctx)
        {
            _ctx = ctx;

            AddDispose(_ctx.currentBuild.Subscribe(build =>
            {
                _workSub?.Dispose();
                _floorSub?.Dispose();
                _workSub = _ctx.currentBuild.Value.CurrentFloor.Value.CurrentWorkCount.Subscribe(OnCurrentWorkCountAddeed);
                _floorSub = _ctx.currentBuild.Value.CurrentFloorIndex.Skip(1).Subscribe(OnFloorsCountChanged);
            }));
        }

        private void OnCurrentWorkCountAddeed(int currentCount)
        {
            Debug.Log($"Add Work, current count: {currentCount} work. Floor {_ctx.currentBuild.Value.CurrentFloor.Value.CurrentWorkCount.Value} /" +
                      $" {_ctx.currentBuild.Value.CurrentFloor.Value.Info.Value.maxWorkCount}");
            if(currentCount < _ctx.currentBuild.Value.CurrentFloor.Value.Info.Value.maxWorkCount)
                return;

            _ctx.currentBuild.Value.CurrentFloorIndex.Value++;
        }

        private void OnFloorsCountChanged(int currentCount)
        {
            _ctx.currentBuild.Value.Building.Value.CurrentFloorsCount.Value++;
            if (currentCount < _ctx.currentBuild.Value.Building.Value.Info.Value.floors.Count)
            {
                _ctx.currentBuild.Value.CurrentFloor.Value.CurrentWorkCount.Value -= _ctx.currentBuild.Value.CurrentFloor.Value.Info.Value.maxWorkCount;
                FloorInfo info = _ctx.currentBuild.Value.Building.Value.Info.Value.floors[currentCount];
                _ctx.currentBuild.Value.CurrentFloor.Value.Info.Value = info;
            }
            else
            {
                _ctx.queueBuildProgress.Add(_ctx.currentBuild.Value);
                _ctx.currentBuildIndex.Value++;
                BuildProgressModel newModel = new BuildProgressModel();
                newModel.Building.Value = new BuildingModel();
                newModel.Building.Value.Info.Value = _ctx.config[_ctx.currentBuildIndex.Value];
                newModel.Building.Value.MoneyIncome.Value = newModel.Building.Value.Info.Value.income;
                newModel.Building.Value.TimeSpeed.Value = newModel.Building.Value.Info.Value.timeSpeed;
                newModel.CurrentFloor.Value = new FloorModel();
                newModel.CurrentFloor.Value.Info.Value = newModel.Building.Value.Info.Value.floors[0];
                _ctx.currentBuild.Value = newModel;
                Debug.Log($"Building {_ctx.currentBuild.Value.Building.Value.Info.Value.id} ended");
            }
        }

        protected override void OnDispose()
        {
            _workSub?.Dispose();
            _floorSub?.Dispose();
            base.OnDispose();
        }
    }
}