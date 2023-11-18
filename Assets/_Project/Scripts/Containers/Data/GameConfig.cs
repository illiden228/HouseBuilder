using System.Collections.Generic;
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

        public int StartWorkerCount => _startWorkerCount;

        public int BaseWorkerWorkIncome => _baseWorkerWorkIncome;

        public int BaseWorkerMoneyIncome => _baseWorkerMoneyIncome;

        public float BaseWorkerTimeSpeed => _baseWorkerTimeSpeed;

        public List<Grade> Grades => _grades;

        public BuildingInfo CurrentBuildInfo => _currentBuildInfo;
    }
}