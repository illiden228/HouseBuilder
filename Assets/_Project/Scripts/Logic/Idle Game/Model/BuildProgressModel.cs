using Containers;
using UniRx;

namespace Logic.Model
{
    public class BuildProgressModel
    {
        public IReactiveProperty<BuildingInfo> BuildingInfo { get; }
        public IReactiveCollection<FloorInfo> NeededFloors { get; }
        public IReactiveProperty<FloorModel> CurrentFloor { get; }
        public IReactiveProperty<int> CurrentFloorIndex { get; }

        public BuildProgressModel()
        {
            BuildingInfo = new ReactiveProperty<BuildingInfo>();
            NeededFloors = new ReactiveCollection<FloorInfo>();
            CurrentFloor = new ReactiveProperty<FloorModel>();
            CurrentFloorIndex = new ReactiveProperty<int>();
        }
    }
}