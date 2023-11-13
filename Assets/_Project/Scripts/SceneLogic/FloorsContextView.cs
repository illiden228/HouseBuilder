using Core;
using UnityEngine;

namespace SceneLogic
{
    public class FloorsContextView : SceneContextView
    {
        [SerializeField] private BuildingBuilder _buildingBuilder;

        public BuildingBuilder BuildingBuilder => _buildingBuilder;
    }
}