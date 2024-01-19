using Core;
using UniRx;
using UnityEngine;

namespace Logic.Idle.Monitors
{
    public class BuildingsMonitorView : MonitorPanelView
    {
        [SerializeField] private Transform _container;
        
        public struct Ctx
        {
        }

        private Ctx _ctx;

        public Transform Container => _container;

        public void Init(BaseCtx baseCtx, Ctx ctx)
        {
            BaseInit(baseCtx);
            _ctx = ctx;
        }
    }
}

