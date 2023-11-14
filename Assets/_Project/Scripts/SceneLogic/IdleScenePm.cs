using System;
using Core;
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
        }

        private readonly Ctx _ctx;

        public IdleScenePm(Ctx ctx)
        {
            _ctx = ctx;
            
            _ctx.sceneContext.Worker.Init();
            
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