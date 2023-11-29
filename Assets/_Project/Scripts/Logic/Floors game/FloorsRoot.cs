using Core;
using SceneLogic;
using System;
using Tools.Extensions;

public class FloorsRoot : BaseDisposable
{
    //obsolete

    private readonly Ctx _ctx;

    public struct Ctx
    {
        public FloorsContextView sceneContext;
        public FloorsSceneSettings floorsSettings;
    }
       
    public FloorsRoot(Ctx ctx)
    {
        _ctx = ctx;

        ReactiveEvent onClickReleaseFloorButton = new ReactiveEvent();

        FloorHUDPm mainHUD = AddDispose(new FloorHUDPm(new FloorHUDPm.Ctx
        {
            //resourceLoader = resourceLoader,
            viewOnScene = _ctx.sceneContext.FloorHUDView,
            //profile = profile,
            releaseFloorButton = onClickReleaseFloorButton
        }));

        TowerBuilderPm towerBuilderPm = AddDispose(new TowerBuilderPm(new TowerBuilderPm.Ctx 
        { 
            towerBuilderView = _ctx.sceneContext.TowerBuilderView,            
            onReleaseFloor = onClickReleaseFloorButton,
            floorViewPrefab = _ctx.floorsSettings.FloorPrefab
        }));        
    }   
}
