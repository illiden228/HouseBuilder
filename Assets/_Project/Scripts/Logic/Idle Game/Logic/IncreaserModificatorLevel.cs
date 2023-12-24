using System.Collections.Generic;
using Containers.Modificators;
using Core;
using Tools.Extensions;
using UniRx;

namespace Logic.Model
{
    public class IncreaserModificatorLevel : BaseDisposable
    {
        public struct Ctx
        {
            public List<ModificatorInfo> config;
            public IReactiveCollection<ModificatorInfo> modificators;
            public IReactiveProperty<int> currentLevel;
            public ModificatorInfo baseInfo;
            public IReactiveProperty<bool> isLastLevel;
        }

        private readonly Ctx _ctx;

        public IncreaserModificatorLevel(Ctx ctx)
        {
            _ctx = ctx;

            AddDispose(_ctx.currentLevel.Subscribe(OnCurrentLevelChanged));
        }

        public void OnCurrentLevelChanged(int currentLevel)
        {
            if (_ctx.config.Count == currentLevel)
            {
                _ctx.isLastLevel.Value = true;
                return;
            }
            
            ModificatorInfo currentInfo = _ctx.config[currentLevel - 1];
            _ctx.modificators.Add(currentInfo);
        }
    }
}