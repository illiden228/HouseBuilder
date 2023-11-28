using Containers;
using SceneLogic;
using UniRx;

namespace Logic.Idle.Workers
{
    public class WorkerModel
    {
        public ReactiveProperty<WorkerInfo> StartInfo { get; }
        public ReactiveProperty<float> BaseTimeSpeed { get; }
        public ReactiveProperty<int> BaseMoneyIncome { get; }
        public ReactiveProperty<int> BaseWorkIncome { get; }
        public ReactiveProperty<float> TimeSpeed { get; }
        public ReactiveProperty<int> MoneyIncome { get; }
        public ReactiveProperty<int> WorkIncome { get; }
        public ReactiveProperty<Grade> Grade { get; }
        public ReactiveProperty<float> CurrentIncomeTime;

        public WorkerModel(WorkerInfo info)
        {
            StartInfo = new ReactiveProperty<WorkerInfo>(info);
            TimeSpeed = new ReactiveProperty<float>(info.baseTimeToWork);
            MoneyIncome = new ReactiveProperty<int>(info.baseIncomeMoney);
            WorkIncome = new ReactiveProperty<int>(info.baseIncomeWork);
            BaseTimeSpeed = new ReactiveProperty<float>(info.baseTimeToWork);
            BaseMoneyIncome = new ReactiveProperty<int>(info.baseIncomeMoney);
            BaseWorkIncome = new ReactiveProperty<int>(info.baseIncomeWork);
            Grade = new ReactiveProperty<Grade>(info.grade);
            CurrentIncomeTime = new ReactiveProperty<float>(0);
        }
    }
}