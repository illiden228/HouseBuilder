﻿using Core;
using UniRx;

namespace Logic.Idle.Monitors
{
    public class BuildingProgressMonitorView : BaseMonobehaviour
    {
        public struct Ctx
        {
            public CompositeDisposable viewDisposable;
        }

        private Ctx _ctx;

        public void Init(Ctx ctx)
        {
            _ctx = ctx;
        }
    }
}
