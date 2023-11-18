using Containers;
using SceneLogic;
using UniRx;

namespace Logic.Idle.Workers
{
    public class WorkerModel
    {
        public ReactiveProperty<WorkerInfo> Info { get; }
        public ReactiveProperty<float> TimeSpeed { get; }
        public ReactiveProperty<int> MoneyIncome { get; }
        public ReactiveProperty<int> WorkIncome { get; }

        public WorkerModel()
        {
            Info = new ReactiveProperty<WorkerInfo>();
            TimeSpeed = new ReactiveProperty<float>();
            MoneyIncome = new ReactiveProperty<int>();
            WorkIncome = new ReactiveProperty<int>();
        }
    }
}