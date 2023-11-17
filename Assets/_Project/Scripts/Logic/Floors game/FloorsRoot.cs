using Core;
using System;
using Tools.Extensions;

public class FloorsRoot : BaseDisposable
{
    private readonly Ctx _ctx;

    public struct Ctx
    {
        public FloorsSceneContext sceneContext;
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
