using Core;
using Tools.Extensions;
using Tools;
using UniRx;
using UnityEngine;
using Game.Player;
using System;

public class FloorHUDPm : BaseDisposable
{
    public struct Ctx
    {
        public IResourceLoader resourceLoader;
        public IReadOnlyProfile profile;
        public FloorHUDView viewOnScene;
        public ReactiveEvent releaseFloorButton;
    }

    private readonly Ctx _ctx;
    //private const string VIEW_PREFAB_NAME = "FloorHUDView";
        
    public FloorHUDPm(Ctx ctx)
    {
        _ctx = ctx;

        _ctx.viewOnScene.Init(new FloorHUDView.Ctx
        {
            viewDisposables = AddDispose(new CompositeDisposable()),
            //availableFloors = _ctx.profile.Floors,
            releaseFloorButton = _ctx.releaseFloorButton
        });

        //_ctx.resourceLoader.LoadPrefab("fake", VIEW_PREFAB_NAME, OnViewLoaded);
    }
}
