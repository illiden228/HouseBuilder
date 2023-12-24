using System;
using System.Collections.Generic;
using _Project.Scripts.Containers.Data;
using UnityEngine;

namespace Containers.Data
{
    public class GameConfig
    {
        public Prices prices = new();
        public WorkerConfig workerConfig = new();
        public BuildingsConfig buildingsConfig = new();
        public MainSettings mainSettings = new();

        public class Prices
        {
            public List<int> nextLevelPrices = new();
            public List<int> mergeUpPrices = new();
        }

        public class WorkerConfig
        {
            public List<int> moneyIncomeValues = new();
            public List<int> workIncomeValues = new();
            public List<float> timeSpeedValues = new();

            public WorkerInfo GetStartWorkerInfo()
            {
                return new WorkerInfo
                {
                    baseIncomeMoney = moneyIncomeValues[0],
                    baseIncomeWork = workIncomeValues[0],
                    baseTimeToWork = timeSpeedValues[0],
                    grade = 1,
                    id = $"worker_{Guid.NewGuid()}"
                };
            }
        }

        public class BuildingsConfig
        {
            public List<FloorInfo> floors = new();
            public List<BuildingInfo> buildings = new();
        }
    }
}