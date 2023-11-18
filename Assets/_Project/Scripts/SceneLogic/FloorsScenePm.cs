using Core;
using UniRx;

namespace SceneLogic
{
    public class FloorsScenePm : BaseDisposable
    {
        public struct Ctx
        {
            public FloorsContextView sceneContext;
            public ReactiveProperty<Scenes> currentScene;
            public UserDataLoader userDataLoader;
        }

        private readonly Ctx _ctx;

        public FloorsScenePm(Ctx ctx)
        {
            _ctx = ctx;
            
            _ctx.sceneContext.BuildingBuilder.Init(new TowerBuilderView.Ctx
            {
                userDataLoader = _ctx.userDataLoader,
                currentScene = _ctx.currentScene
            });
        }
    }
}