﻿using Containers;
using Containers.Data;
using Core;
using Logic.Profile;
using Tools.Extensions;
using UniRx;

namespace Logic.Model
{
    public class CoreIdleLogic : BaseDisposable
    {
        public struct Ctx
        {
            public ProfileClient profile;
            public ReactiveEvent<BuildingInfo> buildingReadyEvent;
            public GameConfig config;
        }

        private readonly Ctx _ctx;

        public CoreIdleLogic(Ctx ctx)
        {
            _ctx = ctx;

            ApplyModificatorLogic.Ctx applyModificatorCtx = new ApplyModificatorLogic.Ctx
            {
                modificators = _ctx.profile.Modificators,
                workers = _ctx.profile.Workers
            };
            AddDispose(new ApplyModificatorLogic(applyModificatorCtx));

            WorkersLogic.Ctx workerLogicCtx = new WorkersLogic.Ctx
            {
                moneys = _ctx.profile.Moneys,
                workers = _ctx.profile.Workers,
                currentBuild = _ctx.profile.CurrentBuilding,
            };
            AddDispose(new WorkersLogic(workerLogicCtx));
            
            BuildingsLogic.Ctx buildingsLogicCtx = new BuildingsLogic.Ctx
            {
                moneys = _ctx.profile.Moneys,
                buildings = _ctx.profile.Buildings
            };
            AddDispose(new BuildingsLogic(buildingsLogicCtx));

            BuildProgressLogic.Ctx buildProgressLogicCtx = new BuildProgressLogic.Ctx
            {
                currentBuild = _ctx.profile.CurrentBuilding,
                buildinReadyEvent = _ctx.buildingReadyEvent
            };
            AddDispose(new BuildProgressLogic(buildProgressLogicCtx));

            UpgradeLogic.Ctx upgradeCtx = new UpgradeLogic.Ctx
            {
                profile = _ctx.profile,
                config = _ctx.config
            };
            AddDispose(new UpgradeLogic(upgradeCtx));
        }
    }
}