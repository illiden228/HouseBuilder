using UnityEngine;

[CreateAssetMenu(fileName = "FloorsSceneSettings", menuName = "ScriptableObjects/FloorsSceneSettings", order = 1)]
public class FloorsSceneSettings : ScriptableObject
{
    [SerializeField] private FloorView _floorPrefab;

    public FloorView FloorPrefab => _floorPrefab;
}