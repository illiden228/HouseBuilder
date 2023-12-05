using Containers;
using Containers.Modificators;
using Core;
using Logic.Idle.Workers;
using Logic.Model;
using UniRx;

namespace Logic.Profile
{
    public class ProfileClient : IReadOnlyProfile
    {
        public struct Ctx
        {
            
        }

        private readonly Ctx _ctx;
        
        public IReactiveProperty<int> Moneys { get; }
        public IReactiveCollection<WorkerModel> Workers { get; }
        public IReactiveCollection<BuildingModel> Buildings { get; }
        public IReactiveCollection<ModificatorInfo> Modificators { get; }
        public ReactiveProperty<BuildProgressModel> CurrentBuilding { get; }
        public ReactiveProperty<Scenes> CurrentScene { get; }
        public IReactiveProperty<int> CurrentEffectiencyLevel { get; }
        public IReactiveProperty<int> CurrentSpeedLevel { get; }

        public ProfileClient(Ctx ctx)
        {
            _ctx = ctx;

            Moneys = new ReactiveProperty<int>();
            Workers = new ReactiveCollection<WorkerModel>();
            Modificators = new ReactiveCollection<ModificatorInfo>();
            Buildings = new ReactiveCollection<BuildingModel>();
            CurrentScene = new ReactiveProperty<Scenes>();
            CurrentBuilding = new ReactiveProperty<BuildProgressModel>();
            CurrentEffectiencyLevel = new ReactiveProperty<int>();
            CurrentSpeedLevel = new ReactiveProperty<int>();
        }
    }
}