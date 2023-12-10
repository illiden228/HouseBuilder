using Containers;
using System.Numerics;
using UniRx;

namespace Logic.Model
{
    public class BuildingModel
    {
        public ReactiveProperty<BuildingInfo> Info { get; }
        public ReactiveProperty<float> TimeSpeed { get; }
        public ReactiveProperty<int> MoneyIncome { get; }
        public ReactiveProperty<int> CurrentFloorsCount { get; }

        public BuildingModel()
        {
            Info = new ReactiveProperty<BuildingInfo>();
            TimeSpeed = new ReactiveProperty<float>();
            MoneyIncome = new ReactiveProperty<int>();            
            CurrentFloorsCount = new ReactiveProperty<int>();
        }
    }
}