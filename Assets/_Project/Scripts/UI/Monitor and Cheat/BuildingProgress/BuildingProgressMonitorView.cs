﻿using Core;
using UniRx;

namespace Logic.Idle.Monitors
{
    public class BuildingProgressMonitorView : MonitorPanelView
    {
        public struct Ctx
        {
        }

        private Ctx _ctx;

        public void Init(BaseCtx baseCtx, Ctx ctx)
        {
            BaseInit(baseCtx);
            _ctx = ctx;
        }
    }
}

