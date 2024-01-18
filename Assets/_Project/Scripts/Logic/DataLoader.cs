using System.Collections.Generic;
using Containers;
using Containers.Data;
using Containers.Modificators;
using Core;
using Logic.Idle.Workers;
using Logic.Model;
using Logic.Profile;

namespace Logic.Idle
{
    public class DataLoader : BaseDisposable
    {
        public struct Ctx
        {
            public IStorageService storageService;
            public ProfileClient profile;
            public GameConfig gameConfig;
        }

        private readonly Ctx _ctx;
        private const string PROFILE_KEY = "profile";

        public DataLoader(Ctx ctx)
        {
            _ctx = ctx;

            ProfileInfo profileInfo = null;
            _ctx.storageService.LoadAndPopulate<ProfileInfo>(PROFILE_KEY, profileInfo, () =>
            {
                if (profileInfo == null)
                    profileInfo = CreateStartProfileInfo();

                SetProfile(profileInfo);
            });

            if (profileInfo == null)
            {
                profileInfo = CreateStartProfileInfo();
                SetProfile(profileInfo);
            }
        }

        private ProfileInfo CreateStartProfileInfo()
        {
            return new ProfileInfo
            {
                id = "start_test_profile",
                buildings = new List<BuildingInfo>(),
                modificators = new List<ModificatorInfo>
                {
                    new EffectiencyModificatorInfo("base"),
                    new TimeSpeedModificatorInfo("base")
                },
                moneys = 0,
                workers = new List<WorkerInfo>
                {
                    _ctx.gameConfig.workerConfig.GetStartWorkerInfo()
                },
                currentBuildIndex = 0,
                currentFloorsCount = 0,
                currentFloor = _ctx.gameConfig.buildingsConfig.floors[0],
                effectiencyPrice = _ctx.gameConfig.prices.nextLevelPrices[0],
                timeSpeedPrice = _ctx.gameConfig.prices.nextLevelPrices[0],
                addWorkerPrice = _ctx.gameConfig.prices.nextLevelPrices[0],
                mergePrice = _ctx.gameConfig.prices.mergeUpPrices[0],
                currentEffectiency = _ctx.gameConfig.workerConfig.workIncomeValues[0],
                currentTimeSpeed = _ctx.gameConfig.workerConfig.timeSpeedValues[0],
            };
        }

        private void SetProfile(ProfileInfo info)
        {
            _ctx.profile.CurrentScene.Value = Scenes.IdleScene;
            _ctx.profile.Moneys.Value = info.moneys;
            _ctx.profile.UpgradeModel.CurrentEffectiencyUpPrice.Value = info.effectiencyPrice;
            _ctx.profile.UpgradeModel.CurrentTimeSpeedUpPrice.Value = info.timeSpeedPrice;
            _ctx.profile.UpgradeModel.CurrentAddWorkerPrice.Value = info.addWorkerPrice;
            _ctx.profile.UpgradeModel.CurrentMergePrice.Value = info.mergePrice;
            
            foreach (var buildingInfo in info.buildings)
            {
                _ctx.profile.Buildings.Add(CreateBuildingModel(buildingInfo));
            }

            foreach (var workerInfo in info.workers)
            {
                _ctx.profile.Workers.Add(CreateWorkerModel(workerInfo));
            }
            
            foreach (var modificatorInfo in info.modificators)
            {
                _ctx.profile.Modificators.Add(modificatorInfo);
            }

            BuildProgressModel progressModel = new BuildProgressModel();
            progressModel.Building.Value = CreateBuildingModel(_ctx.gameConfig.buildingsConfig.buildings[info.currentBuildIndex]);
            progressModel.CurrentFloorIndex.Value = info.currentFloorsCount;

            foreach (var floorInfo in progressModel.Building.Value.Info.Value.floors)
            {
                progressModel.NeededFloors.Add(floorInfo);
            }

            progressModel.CurrentFloor.Value = CreateFloorModel(progressModel.NeededFloors[progressModel.CurrentFloorIndex.Value]);
            
            _ctx.profile.CurrentBuildingWorkProgress.Value = progressModel;

            _ctx.profile.CurrentEffectiency.Value = info.currentEffectiency;
            _ctx.profile.CurrentTimeSpeed.Value = info.currentTimeSpeed;
        }

        private WorkerModel CreateWorkerModel(WorkerInfo info)
        {
            WorkerModel model = new WorkerModel(info);
            return model;
        }

        private BuildingModel CreateBuildingModel(BuildingInfo info)
        {
            BuildingModel model = new BuildingModel();
            model.Info.Value = info;
            model.MoneyIncome.Value = info.income;
            model.TimeSpeed.Value = info.timeSpeed;
            return model;
        }

        private FloorModel CreateFloorModel(FloorInfo info)
        {
            FloorModel model = new FloorModel();
            model.Info.Value = info;
            model.CurrentWorkCount.Value = info.currentWorkCount;
            return model;
        }
    }
}