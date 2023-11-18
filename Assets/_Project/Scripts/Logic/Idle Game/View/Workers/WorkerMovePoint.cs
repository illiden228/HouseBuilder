using UnityEngine;
using UnityEngine.Serialization;

namespace Logic.Idle.Workers
{
    [System.Serializable]
    public class WorkerMovePoint
    {
        public Transform Point;
        public bool AddBag;
        public bool RemoveBag;
    }
}