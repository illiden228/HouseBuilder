using System;
using _Project.Scripts.Logic.Factories;
using Containers.Data;
using Core;
using Logic.Idle.Monitors;
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
            public IReadOnlyProfile profile;
            public GameConfig gameConfig;
        }

        private readonly Ctx _ctx;

        public IdleScenePm(Ctx ctx)
        {
            _ctx = ctx;

            FactorySystem.Ctx factorySystemCtx = new FactorySystem.Ctx
            {
                resourceLoader = _ctx.resourceLoader,
                workers = _ctx.profile.Workers,
                sceneContext = _ctx.sceneContext
            };
            AddDispose(new FactorySystem(factorySystemCtx));

            MainMonitorPm.Ctx monitorCtx = new MainMonitorPm.Ctx
            {
                resourceLoader = _ctx.resourceLoader,
                gameConfig = _ctx.gameConfig,
                uiParent = _ctx.sceneContext.UiParent.transform,
                profile = _ctx.profile
            };
            AddDispose(new MainMonitorPm(monitorCtx));
            
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