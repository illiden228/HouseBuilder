using Core;
using UniRx;

namespace Game.Player
{
    public class Profile : BaseDisposable, IReadOnlyProfile
    {
        public struct Ctx
        {
            public int floorsAvailable;
            public int floorsBuilt;
        }

        private readonly Ctx _ctx;
        private ReactiveProperty<int> _floorsAvailable;
        private ReactiveProperty<int> _floorsBuilt;

        public Profile(Ctx ctx)
        {
            _ctx = ctx;
            _floorsAvailable = new ReactiveProperty<int>(_ctx.floorsAvailable);
            _floorsBuilt = new ReactiveProperty<int>(_ctx.floorsBuilt);
        }
        
        public IReadOnlyReactiveProperty<int> FloorsBuilt => _floorsBuilt;

        public IReadOnlyReactiveProperty<int> Floors => _floorsAvailable;

        public void AddAvailableFloors(int value)
        {
            _floorsAvailable.Value += value;
        }

        public void AddBuiltFloors(int value)
        {
            _floorsBuilt.Value += value;
        }

        public bool TryRemoveAvailableFloor(int count)
        {
            bool possible = count <= _floorsAvailable.Value;
            if (possible)
                _floorsAvailable.Value -= count;
            return possible;
        }

        public bool TryRemoveBuiltFloor(int count)
        {
            bool possible = count <= _floorsBuilt.Value;
            if (possible)
                _floorsBuilt.Value -= count;
            return possible;
        }
    }
}