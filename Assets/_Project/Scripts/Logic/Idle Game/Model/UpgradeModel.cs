using Tools.Extensions;
using UniRx;

namespace Logic.Model
{
    public class UpgradeModel
    {
        public ReactiveEvent AddWorker = new();
        public ReactiveEvent Merge = new();
        public ReactiveEvent TimeSpeedUp = new();
        public ReactiveEvent EffectiencyUp = new();
        
        public ReactiveProperty<int> CurrentMergePrice = new();
        public ReactiveProperty<int> CurrentAddWorkerPrice = new();
        public ReactiveProperty<int> CurrentEffectiencyUpPrice = new();
        public ReactiveProperty<int> CurrentTimeSpeedUpPrice = new();
        
        public ReactiveProperty<int> UpgradesCount = new();

        public ReactiveProperty<int> MergeCost = new();
        public ReactiveProperty<int> AddWorkerCost = new();
        public ReactiveProperty<int> EffectiencyUpCost = new();
        public ReactiveProperty<int> TimeSpeedUpCost = new();
    }
}