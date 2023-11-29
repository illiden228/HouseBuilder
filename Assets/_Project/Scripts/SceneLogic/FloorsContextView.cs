using Core;
using UnityEngine;

namespace SceneLogic
{
    public class FloorsContextView : SceneContextView
    {
        [SerializeField] private TowerBuilderView _towerBuilder;
        [SerializeField] private FloorHUDView _floorHUDView;
        [SerializeField] private FloorsSceneSettings _settings;
        //исползовать этот вместо floors scene context

        public TowerBuilderView TowerBuilderView => _towerBuilder;
        public FloorHUDView FloorHUDView => _floorHUDView;
    }
}