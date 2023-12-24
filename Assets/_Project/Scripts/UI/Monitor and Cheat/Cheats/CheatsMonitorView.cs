using System;
using Core;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.Idle.Monitors
{
    public class CheatsMonitorView : MonitorPanelView
    {
        [SerializeField] private TimeSpeedCheatView _timeSpeedCheatView;
        
        public struct Ctx
        {
            public Action back;
        }

        private Ctx _ctx;

        public TimeSpeedCheatView TimeSpeedCheatView => _timeSpeedCheatView;
        
        public void Init(BaseCtx baseCtx, Ctx ctx)
        {
            BaseInit(baseCtx);
            _ctx = ctx;
        }
    }
}

