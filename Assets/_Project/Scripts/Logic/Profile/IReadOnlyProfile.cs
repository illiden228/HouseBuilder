using Containers;
using Containers.Modificators;
using Core;
using UniRx;

namespace Logic.Profile
{
    public interface IReadOnlyProfile
    {
        public IReactiveProperty<int> Moneys { get; }
        public IReactiveCollection<WorkerInfo> Workers { get; }
        public IReactiveCollection<BuildingInfo> Buildings { get; }
        public IReactiveCollection<Modificator> Modificators { get; }
        public ReactiveProperty<Scenes> CurrentScene { get; }
    }
}