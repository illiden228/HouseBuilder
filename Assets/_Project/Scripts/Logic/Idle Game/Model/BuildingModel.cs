using Containers;
using System.Numerics;
using UniRx;

namespace Logic.Model
{
    public class BuildingModel
    {
        public ReactiveProperty<BuildingInfo> Info { get; } // тут только инфу с наградами для расчета результата постройки
        public ReactiveProperty<float> TimeSpeed { get; }
        public ReactiveProperty<int> MoneyIncome { get; }
        public ReactiveProperty<int> CurrentFloorsCount { get; } // этажи для постройки, отнять когда ставится этаж

        public BuildingModel()
        {
            Info = new ReactiveProperty<BuildingInfo>();
            TimeSpeed = new ReactiveProperty<float>();
            MoneyIncome = new ReactiveProperty<int>();            
            CurrentFloorsCount = new ReactiveProperty<int>();
        }
    }
}