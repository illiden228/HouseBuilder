using System;
using Core;
using UniRx;

namespace UI
{
    public class UpgradeButtonPm : BaseDisposable
    {
        public struct Ctx
        {
            public UpgradeButtonView view;
            public IReadOnlyReactiveProperty<int> cost;
            public IReadOnlyReactiveProperty<int> currentLevel;
            public Action upgradeClick;
        }

        private readonly Ctx _ctx;

        public UpgradeButtonPm(Ctx ctx)
        {
            _ctx = ctx;
            
            _ctx.view.Init(new UpgradeButtonView.Ctx
            {
                viewDisposable = AddDispose(new CompositeDisposable()),
                currentLevel = _ctx.currentLevel,
                cost = _ctx.cost,
                upgradeClick = _ctx.upgradeClick,
            });
        }
    }
}