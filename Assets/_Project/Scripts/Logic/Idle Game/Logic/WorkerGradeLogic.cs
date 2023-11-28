using System;
using System.Collections.Generic;
using Core;
using Logic.Idle.Workers;
using UniRx;

namespace Logic.Model
{
    public class WorkerGradeLogic : BaseDisposable
    {
        public struct Ctx
        {
            public IReadOnlyReactiveCollection<WorkerModel> workers;
        }

        private readonly Ctx _ctx;
        private Dictionary<WorkerModel, IDisposable> _subs;
        // private Dictionary<WorkerModel, float> 

        public WorkerGradeLogic(Ctx ctx)
        {
            _ctx = ctx;
        }

        
    }
}