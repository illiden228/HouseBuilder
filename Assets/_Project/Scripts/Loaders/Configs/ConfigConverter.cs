using System;
using System.Collections.Generic;
using Containers;
using Containers.Data;
using Core;

namespace Core
{
    public class ConfigConverter : BaseDisposable
    {
        public struct Ctx
        {
            public ConfigData data;
            public GameConfig config;
        }

        private readonly Ctx _ctx;

        public ConfigConverter(Ctx ctx)
        {
            _ctx = ctx;

            SetConfigData();
        }
        
        
        public void SetConfigData()
        {
            foreach (var workerData in _ctx.data.workerData)
            {
                _ctx.config.workerConfig.moneyIncomeValues.Add(workerData.baseIncomeMoney);
                _ctx.config.workerConfig.workIncomeValues.Add(workerData.baseIncomeWork);
                _ctx.config.workerConfig.timeSpeedValues.Add(workerData.baseTimeToWork * 2); // in config time to one side
            }

            foreach (var levelUpConstData in _ctx.data.levelUpCosts)
            {
                _ctx.config.prices.nextLevelPrices.Add(levelUpConstData.nextLevelCost);
                _ctx.config.prices.mergeUpPrices.Add(levelUpConstData.nextMergeLevelCost);
            }

            foreach (var floorData in _ctx.data.floorData)
            {
                _ctx.config.buildingsConfig.floors.Add(new FloorInfo
                {
                    id = Guid.NewGuid().ToString(),
                    currentWorkCount = 0,
                    maxWorkCount = floorData.maxWorkCount
                });
            }

            int currentFloor = 0;
            List<FloorInfo> floors = _ctx.config.buildingsConfig.floors;
            foreach (var buildingData in _ctx.data.buildingData)
            {
                BuildingInfo newBuildingInfo = new BuildingInfo
                {
                    id = Guid.NewGuid().ToString(),
                    floors = new List<FloorInfo>(buildingData.floorsCount),
                    income = buildingData.income,
                    minReward = buildingData.minReward,
                    maxReward = buildingData.maxReward,
                    timeSpeed = buildingData.timeSpeed
                };
                
                for (int i = 0; i < buildingData.floorsCount; i++)
                {
                    if (currentFloor == floors.Count)
                        currentFloor = floors.Count - 1;
                    newBuildingInfo.floors.Add(floors[currentFloor++]);
                }
                
                _ctx.config.buildingsConfig.buildings.Add(newBuildingInfo);
            }
            
            _ctx.config.mainSettings = _ctx.data.mainSettings;
        }
    }
}