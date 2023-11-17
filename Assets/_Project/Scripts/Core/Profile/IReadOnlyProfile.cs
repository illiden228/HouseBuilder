using System.Collections.Generic;
using Core;
using UniRx;

namespace Game.Player
{
    public interface IReadOnlyProfile
    {
        public IReadOnlyReactiveProperty<int> FloorsBuilt { get; }
        public IReadOnlyReactiveProperty<int> Floors { get; }
    }
}