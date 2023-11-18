using System;
using System.Collections.Generic;
using Containers;
using Containers.Data;
using Core;
using Logic.Idle.Workers;
using Tools.Extensions;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.Logic.Factories
{
    public class WorkerFactory : BaseDisposable
    {
        public struct Ctx
        {
            public IReactiveCollection<WorkerModel> workers;
            public WorkerView prefab;
            public List<WorkerMovePoint> path;
            public Transform spawnPoint;
        }

        private readonly Ctx _ctx;
        private Dictionary<GameObject, List<IDisposable>> _disposables;

        public WorkerFactory(Ctx ctx)
        {
            _ctx = ctx;
            _disposables = new Dictionary<GameObject, List<IDisposable>>();

            foreach (var workerModel in _ctx.workers)
            {
                CreateWorker(workerModel);
            }

            AddDispose(_ctx.workers.ObserveAdd().Subscribe(OnAddWorkerModel));
        }

        private void OnAddWorkerModel(CollectionAddEvent<WorkerModel> addEvent)
        {
            CreateWorker(addEvent.Value);
        }

        private void CreateWorker(WorkerModel model)
        {
            GameObject workerObject = GameObject.Instantiate(_ctx.prefab.gameObject, _ctx.spawnPoint.position, Quaternion.identity);
            WorkerView workerView = workerObject.GetComponent<WorkerView>();
            CompositeDisposable viewDisposable = new CompositeDisposable();
            workerView.Init(new WorkerView.Ctx
            {
                viewDisposable = viewDisposable
            });

            WorkerPm.Ctx workerCtx = new WorkerPm.Ctx
            {
                model = model,
                view = workerView,
                path = _ctx.path,
            };
            WorkerPm workerPm = new WorkerPm(workerCtx);

            if (!_disposables.ContainsKey(workerObject))
                _disposables[workerObject] = new List<IDisposable>();
            
            _disposables[workerObject].Add(workerPm);
            _disposables[workerObject].Add(viewDisposable);
        }
    }
}