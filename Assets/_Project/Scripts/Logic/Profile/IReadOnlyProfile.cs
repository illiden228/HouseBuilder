using System.Collections.Generic;
using Containers;
using Containers.Modificators;
using Core;
using Logic.Idle.Workers;
using Logic.Model;
using UniRx;

namespace Logic.Profile
{
    public interface IReadOnlyProfile
    {
        public IReactiveProperty<int> Moneys { get; }
        public IReactiveCollection<WorkerModel> Workers { get; }
        public IReactiveCollection<BuildingModel> Buildings { get; }
        public IReactiveCollection<ModificatorInfo> Modificators { get; }
        public IReactiveProperty<BuildProgressModel> CurrentBuildingWorkProgress { get; }
        public IReactiveProperty<BuildProgressModel> CurrentBuildingFloorProgress { get; }
        public IReactiveProperty<Scenes> CurrentScene { get; }
        public IReactiveProperty<int> CurrentEffectiencyLevel { get; }
        public IReactiveProperty<int> CurrentMergeLevel { get; }
        public IReactiveProperty<int> CurrentTimeSpeedLevel { get; }
        public IReactiveProperty<int> CurrentEffectiency { get; }
        public IReactiveProperty<float> CurrentTimeSpeed { get; }
        public UpgradeModel UpgradeModel { get; }
        public IReactiveProperty<int> CurrentBuildIndex { get; }
        public IReactiveCollection<BuildProgressModel> QueueBuildProgress { get; }
        public bool CanMerge(out int grade);
        public List<WorkerModel> GetWorkersForMerge(int grade);
        public IReactiveProperty<int> CurrentAddWorkerLevel { get; }
    }
}