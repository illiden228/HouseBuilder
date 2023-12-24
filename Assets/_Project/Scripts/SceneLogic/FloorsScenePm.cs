using Core;
using Logic.Model;
using System;
using Tools.Extensions;
using UniRx;

namespace SceneLogic
{
    public class FloorsScenePm : BaseDisposable
    {
        //to do:
        //Передача на сцену этажей для постройки здания
        //Создание здания определенного качества
        //Сохранение построенных этажей на сцене и загрузка их и постройка при повторном заходе на сцену

        public struct Ctx
        {
            public IReactiveProperty<Scenes> currentScene;
            public UserDataLoader userDataLoader;
            public FloorsContextView sceneContext;
            public Action onBackToIdleScene;
            public IResourceLoader resourceLoader;
            public IReactiveProperty<BuildingModel> building; // модель строящегося здания
            public IReactiveProperty<FloorsProgressModel> floorsProgress; // прогресс строящегося здания
        }

        private readonly Ctx _ctx;

        private TowerBuilderPm _towerBuilderPm;

        public FloorsScenePm(Ctx ctx)
        {
            _ctx = ctx;

            ReactiveEvent onClickReleaseFloorButton = new ReactiveEvent();

            AddDispose(onClickReleaseFloorButton);

            FloorHUDPm mainHUD = AddDispose(new FloorHUDPm(new FloorHUDPm.Ctx
            {
                viewOnScene = _ctx.sceneContext.FloorHUDView,                
                releaseFloorButton = onClickReleaseFloorButton
            }));

            _towerBuilderPm = AddDispose(new TowerBuilderPm(new TowerBuilderPm.Ctx
            {
                towerBuilderView = _ctx.sceneContext.TowerBuilderView,
                onReleaseFloor = onClickReleaseFloorButton,
                resourceLoader = _ctx.resourceLoader,
                onBackToIdleScene = _ctx.onBackToIdleScene,
            }));
        }
    }
}