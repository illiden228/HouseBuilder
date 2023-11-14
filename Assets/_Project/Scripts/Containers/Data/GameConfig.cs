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
    }
}