using Containers.Data;
using Core;
using Logic.Model;
using Logic.Profile;
using UniRx;

namespace UI
{
    public class UpgradeButtonsPm : BaseDisposable
    {
        public struct Ctx
        {
            public IReadOnlyProfile profile;
            public UpgradeButtonsView view;
        }

        private readonly Ctx _ctx;

        public UpgradeButtonsPm(Ctx ctx)
        {
            _ctx = ctx;

            UpgradeButtonPm.Ctx effectiencyUpgradeCtx = new UpgradeButtonPm.Ctx
            {
                view = _ctx.view.EffectiencyUpButton,
                cost = _ctx.profile.UpgradeModel.EffectiencyUpCost,
                currentLevel = _ctx.profile.CurrentEffectiencyLevel,
                upgradeClick = _ctx.profile.UpgradeModel.EffectiencyUp.Notify
            };
            AddDispose(new UpgradeButtonPm(effectiencyUpgradeCtx));
            
            UpgradeButtonPm.Ctx addWorkerUpgradeCtx = new UpgradeButtonPm.Ctx
            {
                view = _ctx.view.AddWorkerButton,
                cost = _ctx.profile.UpgradeModel.AddWorkerCost,
                currentLevel = _ctx.profile.CurrentAddWorkerLevel,
                upgradeClick = _ctx.profile.UpgradeModel.AddWorker.Notify
            };
            AddDispose(new UpgradeButtonPm(addWorkerUpgradeCtx));
            
            UpgradeButtonPm.Ctx timeSpeedUpgradeCtx = new UpgradeButtonPm.Ctx
            {
                view = _ctx.view.TimeSpeedUpButton,
                cost = _ctx.profile.UpgradeModel.TimeSpeedUpCost,
                currentLevel = _ctx.profile.CurrentTimeSpeedLevel,
                upgradeClick = _ctx.profile.UpgradeModel.TimeSpeedUp.Notify
            };
            AddDispose(new UpgradeButtonPm(timeSpeedUpgradeCtx));
            
            UpgradeButtonPm.Ctx mergeUpgradeCtx = new UpgradeButtonPm.Ctx
            {
                view = _ctx.view.MergeButton,
                cost = _ctx.profile.UpgradeModel.MergeCost,
                currentLevel = _ctx.profile.CurrentMergeLevel,
                upgradeClick = _ctx.profile.UpgradeModel.Merge.Notify
            };
            AddDispose(new UpgradeButtonPm(mergeUpgradeCtx));
        }
    }
}