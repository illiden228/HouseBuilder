using Containers;
using UniRx;
using UnityEngine;

namespace Logic.Model
{
    public class BuildProgressModel
    {
        public IReactiveCollection<FloorInfo> NeededFloors { get; }
        public IReactiveProperty<FloorModel> CurrentFloor { get; }
        public IReactiveProperty<int> CurrentFloorIndex { get; }
        public IReactiveProperty<BuildingModel> Building { get; }
        public IReactiveProperty<FloorsProgressModel> FloorsProgress { get; }

        public BuildProgressModel()
        {
            NeededFloors = new ReactiveCollection<FloorInfo>();
            CurrentFloor = new ReactiveProperty<FloorModel>();
            CurrentFloorIndex = new ReactiveProperty<int>();
            Building = new ReactiveProperty<BuildingModel>();
            FloorsProgress = new ReactiveProperty<FloorsProgressModel>(new FloorsProgressModel());
        }
    }
}