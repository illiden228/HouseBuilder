using System.Collections.Generic;
using Core;
using UniRx;

namespace Logic.Model
{
    public class LevelUpLogic<T> : BaseDisposable
    {
        public struct Ctx
        {
            public IReactiveProperty<int> curerntLevel;
            public ReactiveProperty<int> currentPrice;
            public IReactiveProperty<T> currentProperty; 
            public List<int> priceConfig;
            public List<T> valuesConfig;
        }

        private readonly Ctx _ctx;

        public LevelUpLogic(Ctx ctx)
        {
            _ctx = ctx;

            AddDispose(_ctx.curerntLevel.Skip(1).Subscribe(OnLevelChanged));
        }

        private void OnLevelChanged(int newLevel)
        {
            if (_ctx.currentPrice != null)
            {
                if (_ctx.priceConfig != null)
                {
                    _ctx.currentPrice.Value = _ctx.priceConfig[newLevel];
                }
            }

            if (_ctx.currentProperty != null)
            {
                if (_ctx.valuesConfig != null)
                    _ctx.currentProperty.Value = _ctx.valuesConfig[newLevel];
                else
                {
                    if (_ctx.currentProperty is IReactiveProperty<int> property)
                        property.Value++;
                }
            }
        }
    }
}