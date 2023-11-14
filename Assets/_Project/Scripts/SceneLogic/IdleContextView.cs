using Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace SceneLogic
{
    public class IdleContextView : SceneContextView
    {
        [SerializeField] private FloorBuilder _floorBuilder;
        [FormerlySerializedAs("_builder")] [SerializeField] private Worker worker;
        [SerializeField] private Storage _storage;
        [SerializeField] private FloorRegistrator _floorRegistrator;

        public FloorBuilder FloorBuilder => _floorBuilder;

        public Worker Worker => worker;

        public Storage Storage => _storage;

        public FloorRegistrator FloorRegistrator => _floorRegistrator;
    }
}