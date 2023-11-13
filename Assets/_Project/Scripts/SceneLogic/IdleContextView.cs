using Core;
using UnityEngine;

namespace SceneLogic
{
    public class IdleContextView : SceneContextView
    {
        [SerializeField] private FloorBuilder _floorBuilder;
        [SerializeField] private Builder _builder;
        [SerializeField] private Storage _storage;
        [SerializeField] private FloorRegistrator _floorRegistrator;

        public FloorBuilder FloorBuilder => _floorBuilder;

        public Builder Builder => _builder;

        public Storage Storage => _storage;

        public FloorRegistrator FloorRegistrator => _floorRegistrator;
    }
}