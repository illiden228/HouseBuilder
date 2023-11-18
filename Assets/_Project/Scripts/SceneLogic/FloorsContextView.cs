using Core;
using UnityEngine;

namespace SceneLogic
{
    public class FloorsContextView : SceneContextView
    {
        [SerializeField] private TowerBuilderView _buildingBuilder;

        public TowerBuilderView BuildingBuilder => _buildingBuilder;
    }
}