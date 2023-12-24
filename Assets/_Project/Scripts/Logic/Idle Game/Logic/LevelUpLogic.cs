using System.Collections.Generic;
using Core;
using UniRx;

namespace Logic.Model
{
    public class LevelUpLogic<T> : BaseDisposable
    {
        public struct Ctx
        {
            public ReactiveProperty<int> curerntLevel;
            public ReactiveProperty<int> currentPrice;
            public ReactiveProperty<T> currentProperty; 
            public List<int> priceConfig;
            public List<T> valuesConfig;
        }

        private readonly Ctx _ctx;

        public LevelUpLogic(Ctx ctx)
        {
            _ctx = ctx;

            AddDispose(_ctx.curerntLevel.Subscribe(OnLevelChanged));
        }

        private void OnLevelChanged(int newLevel)
        {
            _ctx.currentPrice.Value = _ctx.priceConfig[newLevel];
            _ctx.currentProperty.Value = _ctx.valuesConfig[newLevel];
        }
    }
}