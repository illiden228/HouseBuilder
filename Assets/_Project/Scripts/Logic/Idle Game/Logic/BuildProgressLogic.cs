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
            public IReactiveProperty<BuildProgressModel> currentBuild;
            public ReactiveEvent<BuildingInfo> buildinReadyEvent;
        }

        private readonly Ctx _ctx;

        public BuildProgressLogic(Ctx ctx)
        {
            _ctx = ctx;

            AddDispose(_ctx.currentBuild.Value.CurrentFloor.Value.CurrentWorkCount.Subscribe(OnCurrentWorkCountAddeed));
            AddDispose(_ctx.currentBuild.Value.CurrentFloorIndex.Subscribe(OnFloorsCountChanged));
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
            if (currentCount < _ctx.currentBuild.Value.BuildingInfo.Value.floors.Count)
            {
                FloorInfo info = _ctx.currentBuild.Value.BuildingInfo.Value.floors[currentCount];
                _ctx.currentBuild.Value.CurrentFloor.Value.Info.Value = info;
                _ctx.currentBuild.Value.CurrentFloor.Value.CurrentWorkCount.Value = 0;
            }
            else
            {
                _ctx.buildinReadyEvent.Notify(_ctx.currentBuild.Value.BuildingInfo.Value);
                Debug.Log($"Building {_ctx.currentBuild.Value.BuildingInfo.Value.id} ended");
            }
        }
    }
}