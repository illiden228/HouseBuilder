using _Project.Scripts.Logic.Factories;
using Containers.Data;
using Core;
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
            public IReactiveProperty<Scenes> currentScene;
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

            MainMenuPm.Ctx mainMenuCtx = new MainMenuPm.Ctx
            {
                resourceLoader = _ctx.resourceLoader,
                uiParent = _ctx.sceneContext.UiParent.transform,
                viewTestUI = true,
                gameConfig = _ctx.gameConfig,
                profile = _ctx.profile
            };
            AddDispose(new MainMenuPm(mainMenuCtx));
        }
    }
}