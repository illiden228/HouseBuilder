using Containers;
using UniRx;

namespace Logic.Model
{
    public class FloorModel
    {
        public ReactiveProperty<FloorInfo> Info { get; }
        public ReactiveProperty<int> CurrentWorkCount { get; }

        public FloorModel()
        {
            Info = new ReactiveProperty<FloorInfo>();
            CurrentWorkCount = new ReactiveProperty<int>();
        }
    }
}