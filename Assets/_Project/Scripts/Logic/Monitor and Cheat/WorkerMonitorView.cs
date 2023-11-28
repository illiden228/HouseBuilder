using System;
using Core;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.Idle.Monitor_and_Cheat
{
    public class WorkerMonitorView : BaseMonobehaviour
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _addWorkerButton;
        [SerializeField] private Transform _container;
        
        public struct Ctx
        {
            public CompositeDisposable viewDisposable;
            public Action back;
            public Action addWorker;
        }

        private Ctx _ctx;

        public Transform Container => _container;

        public void Init(Ctx ctx)
        {
            _ctx = ctx;

            _backButton.OnClickAsObservable().Subscribe(_ => _ctx.back?.Invoke()).AddTo(_ctx.viewDisposable);
            _addWorkerButton.OnClickAsObservable().Subscribe(_ => _ctx.addWorker?.Invoke()).AddTo(_ctx.viewDisposable);
        }
    }
}

