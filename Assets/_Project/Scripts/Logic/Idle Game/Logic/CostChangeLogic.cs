using Core;
using Logic.Profile;
using UniRx;

namespace Logic.Model
{
    public class CostChangeLogic : BaseDisposable
    {
        public struct Ctx
        {
            public ProfileClient profile;
        }

        private readonly Ctx _ctx;
        private UpgradeModel UpgradeModel => _ctx.profile.UpgradeModel;

        public CostChangeLogic(Ctx ctx)
        {
            _ctx = ctx;

            AddDispose(UpgradeModel.CurrentEffectiencyUpPrice.Subscribe(price => CalcCost(price, UpgradeModel.EffectiencyUpCost)));
            AddDispose(UpgradeModel.CurrentTimeSpeedUpPrice.Subscribe(price => CalcCost(price, UpgradeModel.TimeSpeedUpCost)));
            AddDispose(UpgradeModel.CurrentAddWorkerPrice.Subscribe(price => CalcCost(price, UpgradeModel.AddWorkerCost)));
            AddDispose(UpgradeModel.CurrentMergePrice.Subscribe(price => CalcCost(price, UpgradeModel.MergeCost)));
        }

        private void CalcCost(int price, ReactiveProperty<int> cost)
        {
            cost.Value = (int) (price * (1 + (float) UpgradeModel.UpgradesCount.Value / 100));
        }
    }
}