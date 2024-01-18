using Core;
using Logic.Model;
using UniRx;

namespace Logic.Idle
{
    public class BuildingsCreator : BaseDisposable
    {
        public struct Ctx
        {
            public IReactiveCollection<BuildProgressModel> queueBuildProgress;
            public ReactiveProperty<bool> canBuild;
            public IReactiveProperty<BuildProgressModel> currentBuildingFloorProgress;
            public IReactiveProperty<Scenes> currentScene;
            public IReactiveCollection<BuildingModel> buildings;
        }

        private readonly Ctx _ctx;

        public BuildingsCreator(Ctx ctx)
        {
            _ctx = ctx;

            AddDispose(_ctx.queueBuildProgress.ObserveCountChanged().Subscribe(count =>
            {
                _ctx.canBuild.SetValueAndForceNotify(count > 0);
            }));

            AddDispose(_ctx.canBuild.Subscribe(can =>
            {
                if (!can)
                    return;

                _ctx.currentBuildingFloorProgress.Value = _ctx.queueBuildProgress[0];
            }));

            AddDispose(_ctx.currentScene.Skip(1).Subscribe(scene =>
            {
                if (scene != Scenes.IdleScene)
                    return;

                BuildingModel building = _ctx.currentBuildingFloorProgress.Value.Building.Value;
                if(building.CurrentFloorsCount.Value != 0)
                    return;
                
                _ctx.buildings.Add(building);
                _ctx.queueBuildProgress.Remove(_ctx.currentBuildingFloorProgress.Value);
            }));
        }
    }
}