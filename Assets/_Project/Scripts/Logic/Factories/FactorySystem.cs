using System.Collections.Generic;
using Containers;
using Core;
using Logic.Idle.Workers;
using SceneLogic;
using Tools.Extensions;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.Logic.Factories
{
    public class FactorySystem : BaseDisposable
    {
        public struct Ctx
        {
            public IResourceLoader resourceLoader;
            public IReactiveCollection<WorkerModel> workers;
            public IdleContextView sceneContext;
        }

        private readonly Ctx _ctx;
        private ReactiveEvent<WorkerInfo> _workerSpawnEvent;
        private const string WORKER_PREFAB = "Worker";

        public FactorySystem(Ctx ctx)
        {
            _ctx = ctx;

            AddDispose(_ctx.resourceLoader.LoadPrefab("fakeboundles", WORKER_PREFAB, CreateWorkerFactory));
        }

        private void CreateWorkerFactory(GameObject prefab)
        {
            WorkerFactory.Ctx workerFactoryCtx = new WorkerFactory.Ctx
            {
                workers = _ctx.workers,
                prefab = prefab.GetComponent<WorkerView>(),
                path = _ctx.sceneContext.WorkerPath,
                spawnPoint = _ctx.sceneContext.NewWorkerSpawnPoint
            };
            AddDispose(new WorkerFactory(workerFactoryCtx));
        }
    }
}