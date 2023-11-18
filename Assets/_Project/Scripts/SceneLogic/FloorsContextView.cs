using Core;
using UnityEngine;

namespace SceneLogic
{
    public class FloorsContextView : SceneContextView
    {
        //исползовать этот вместо floors scene context

        [SerializeField] private TowerBuilderView _buildingBuilder;

        public TowerBuilderView BuildingBuilder => _buildingBuilder;
    }
}