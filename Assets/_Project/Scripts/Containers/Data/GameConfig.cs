using System;
using System.Collections.Generic;
using _Project.Scripts.Containers.Data;
using UnityEngine;

namespace Containers.Data
{
    [CreateAssetMenu(fileName = "Config", menuName = "Presets/Config", order = 0)]
    public class GameConfig : ScriptableObject
    {
        [SerializeField] private int _startWorkerCount;
        [SerializeField] private int _baseWorkerWorkIncome;
        [SerializeField] private int _baseWorkerMoneyIncome;
        [SerializeField] private float _baseWorkerTimeSpeed;
        [SerializeField] private List<Grade> _grades;
        [SerializeField] private BuildingInfo _currentBuildInfo;
        private ConfigData _data;
        private List<int> _effectiencyUpCosts = new();
        private List<int> _timeSpeedUpCosts = new();
        private List<int> _mergeUpCosts = new();
        
        private List<int> _moneyIncomeValues = new();
        private List<int> _workIncomeValues = new();
        private List<float> _timeSpeedValues = new();

        public int StartWorkerCount => _startWorkerCount;

        public int BaseWorkerWorkIncome => _baseWorkerWorkIncome;

        public int BaseWorkerMoneyIncome => _baseWorkerMoneyIncome;

        public float BaseWorkerTimeSpeed => _baseWorkerTimeSpeed;

        public List<Grade> Grades => _grades;

        public BuildingInfo CurrentBuildInfo => _currentBuildInfo;

        public WorkerInfo GetStartWorkerInfo()
        {
            return GetWorkerInfoByLevel(1);
        }

        public WorkerInfo GetWorkerInfoByLevel(int level)
        {
            level = level - 1;
            if (level < 0)
            {
                Debug.LogWarning("Try Get Worker Info Config Value with index less than 0");
                level = 0;
            }
            
            WorkerData workerData = _data.workerInfos[level];
            return new WorkerInfo
            {
                baseIncomeMoney = workerData.baseIncomeMoney,
                baseIncomeWork = workerData.baseIncomeWork,
                baseTimeToWork = workerData.baseTimeToWork,
                grade = 1,
                id = $"worker_{Guid.NewGuid()}"
            };
        }
        
        public FloorInfo GetFloorInfoByLevel(int level)
        {
            level = level - 1;
            if (level < 0)
            {
                Debug.LogWarning("Try Get Floor Info Config Value with index less than 0");
                level = 0;
            }
            
            FloorData floorData = _data.floorInfos[level];
            return new FloorInfo
            {
                id = $"floor_{Guid.NewGuid()}",
                currentWorkCount = 0,
                maxWorkCount = floorData.maxWorkCount
            };
        }

        public void SetConfigData(ConfigData data)
        {
            _data = data;

            foreach (var workerData in _data.workerInfos)
            {
                _moneyIncomeValues.Add(workerData.baseIncomeMoney);
                _workIncomeValues.Add(workerData.baseIncomeWork);
                _timeSpeedValues.Add(workerData.baseTimeToWork * 2); // in config time to one side
            }

            foreach (var levelUpConstData in _data.levelUpCosts)
            {
                _effectiencyUpCosts.Add(levelUpConstData.nextLevelCost);
                _timeSpeedUpCosts.Add(levelUpConstData.nextLevelCost);
                _mergeUpCosts.Add(levelUpConstData.nextMergeLevelCost);
            }
        }
    }
}