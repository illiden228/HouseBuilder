using System;
using _Project.Scripts.Logic.Factories;
using Containers.Data;
using Core;
using Logic.Idle.Monitor_and_Cheat;
using Logic.Idle.Workers;
using Logic.Profile;
using UniRx;

namespace SceneLogic
{
    public class IdleScenePm : BaseDisposable
    {
        public struct Ctx
        {
            public IdleContextView sceneContext;
            public ReactiveProperty<Scenes> currentScene;
            public UserDataLoader userDataLoader;
            public IResourceLoader resourceLoader;
            public IReactiveCollection<WorkerModel> workers;
            public GameConfig gameConfig;
        }

        private readonly Ctx _ctx;

        public IdleScenePm(Ctx ctx)
        {
            _ctx = ctx;

            FactorySystem.Ctx factorySystemCtx = new FactorySystem.Ctx
            {
                resourceLoader = _ctx.resourceLoader,
                workers = _ctx.workers,
                sceneContext = _ctx.sceneContext
            };
            AddDispose(new FactorySystem(factorySystemCtx));

            WorkerMonitorPm.Ctx workerMonitorCtx = new WorkerMonitorPm.Ctx
            {
                resourceLoader = _ctx.resourceLoader,
                workers = _ctx.workers,
                gameConfig = _ctx.gameConfig,
                uiParent = _ctx.sceneContext.UiParent.transform
            };
            AddDispose(new WorkerMonitorPm(workerMonitorCtx));
            
            _ctx.sceneContext.FloorBuilder.Init(new FloorBuilder.Ctx
            {
                userDataLoader = _ctx.userDataLoader
            });

            _ctx.sceneContext.Storage.Init();
            
            _ctx.sceneContext.FloorRegistrator.Init(new FloorRegistrator.Ctx
            {
                userDataLoader = _ctx.userDataLoader,
                currentScene = _ctx.currentScene 
            });
        }
    }
}