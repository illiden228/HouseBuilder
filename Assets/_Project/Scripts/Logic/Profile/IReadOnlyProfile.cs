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
        public ReactiveProperty<BuildProgressModel> CurrentBuilding { get; }
        public ReactiveProperty<Scenes> CurrentScene { get; }
    }
}