using Containers;
using Containers.Modificators;
using Core;
using UniRx;

namespace Logic.Profile
{
    public class ProfileClient : IReadOnlyProfile
    {
        public struct Ctx
        {
            public ProfileInfo profileInfo;
        }

        private readonly Ctx _ctx;
        
        public IReactiveProperty<int> Moneys { get; }
        public IReactiveCollection<WorkerInfo> Workers { get; }
        public IReactiveCollection<BuildingInfo> Buildings { get; }
        public IReactiveCollection<Modificator> Modificators { get; }
        public ReactiveProperty<Scenes> CurrentScene { get; }

        public ProfileClient(Ctx ctx)
        {
            _ctx = ctx;
        }
    }
}