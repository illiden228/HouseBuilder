using System;
using _Project.Scripts.Logic.Factories;
using Containers.Data;
using Core;
using Logic.Idle;
using Logic.Profile;
using UI;
using UniRx;

namespace SceneLogic
{
    public class IdleScenePm : BaseDisposable
    {
        public struct Ctx
        {
            public IdleContextView sceneContext;
            public UserDataLoader userDataLoader;
            public IResourceLoader resourceLoader;
            public IReadOnlyProfile profile;
            public GameConfig gameConfig;
            public Action moveToFloorScene;
        }

        private readonly Ctx _ctx;
        private ReactiveProperty<bool> _canBuild;

        public IdleScenePm(Ctx ctx)
        {
            _ctx = ctx;
            _canBuild = new ReactiveProperty<bool>();

            FactorySystem.Ctx factorySystemCtx = new FactorySystem.Ctx
            {
                resourceLoader = _ctx.resourceLoader,
                workers = _ctx.profile.Workers,
                sceneContext = _ctx.sceneContext
            };
            AddDispose(new FactorySystem(factorySystemCtx));

            BuildingsCreator.Ctx buildingCreatorCtx = new BuildingsCreator.Ctx
            {
                canBuild = _canBuild,
                queueBuildProgress = _ctx.profile.QueueBuildProgress,
                currentBuildingFloorProgress = _ctx.profile.CurrentBuildingFloorProgress,
                currentScene = _ctx.profile.CurrentScene,
                buildings = _ctx.profile.Buildings
            };
            AddDispose(new BuildingsCreator(buildingCreatorCtx));

            MainMenuPm.Ctx mainMenuCtx = new MainMenuPm.Ctx
            {
                resourceLoader = _ctx.resourceLoader,
                uiParent = _ctx.sceneContext.UiParent.transform,
                viewTestUI = true,
                gameConfig = _ctx.gameConfig,
                profile = _ctx.profile,
                canBuild = _canBuild,
                build = _ctx.moveToFloorScene
            };
            AddDispose(new MainMenuPm(mainMenuCtx));
        }
    }
}