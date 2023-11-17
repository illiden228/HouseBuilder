using BezierSolution;
using UnityEngine;

public class FloorsSceneContext : MonoBehaviour
{
    [SerializeField] private TowerBuilderView _towerBuilder;
    [SerializeField] private FloorHUDView _floorHUDView;

    public TowerBuilderView TowerBuilderView => _towerBuilder;
    public FloorHUDView FloorHUDView => _floorHUDView;
}
