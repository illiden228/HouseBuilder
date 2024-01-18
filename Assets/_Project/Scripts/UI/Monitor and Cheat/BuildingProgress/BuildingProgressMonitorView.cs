using Core;
using UniRx;
using UnityEngine;

namespace Logic.Idle.Monitors
{
    public class BuildingProgressMonitorView : MonitorPanelView
    {
        [SerializeField] private Transform _container;
        [SerializeField] private IntProgressSliderView _floorProgress;
        [SerializeField] private IntProgressSliderView _buildProgress;
        
        public struct Ctx
        {
            public ReactiveProperty<int> currentFloorWork;
            public ReactiveProperty<int> currentFloorMaxWork;
            public ReactiveProperty<int> currentFloor;
            public ReactiveProperty<int> maxFloors;
        }

        private Ctx _ctx;

        public Transform Container => _container;

        public void Init(BaseCtx baseCtx, Ctx ctx)
        {
            BaseInit(baseCtx);
            _ctx = ctx;

            Observable.IntervalFrame(1).Subscribe(_ => InitSliders()).AddTo(_baseCtx.viewDisposable);
        }
        
        private void InitSliders()
        {
            _floorProgress.Init(new IntProgressSliderView.Ctx
            {
                viewDisposable = _baseCtx.viewDisposable,
                current = _ctx.currentFloorWork,
                max = _ctx.currentFloorMaxWork,
            });
            
            _buildProgress.Init(new IntProgressSliderView.Ctx
            {
                viewDisposable = _baseCtx.viewDisposable,
                current = _ctx.currentFloor,
                max = _ctx.maxFloors,
            });
        }
    }
}

