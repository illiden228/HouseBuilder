using System.Collections.Generic;
using Logic.Idle.Workers;
using UnityEngine;

namespace SceneLogic
{
    public class IdleContextView : SceneContextView
    {
        [SerializeField] private FloorBuilder _floorBuilder;
        [SerializeField] private Storage _storage;
        [SerializeField] private FloorRegistrator _floorRegistrator;
        [SerializeField] private List<WorkerMovePoint> _workerPath;
        [SerializeField] private Transform _newWorkerSpawnPoint;

        public FloorBuilder FloorBuilder => _floorBuilder;

        public List<WorkerMovePoint> WorkerPath => _workerPath;

        public Storage Storage => _storage;

        public FloorRegistrator FloorRegistrator => _floorRegistrator;

        public Transform NewWorkerSpawnPoint => _newWorkerSpawnPoint;
    }
}