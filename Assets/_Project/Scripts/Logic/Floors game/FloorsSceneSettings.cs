using UnityEngine;

[CreateAssetMenu(fileName = "FloorsSceneSettings", menuName = "ScriptableObjects/FloorsSceneSettings", order = 1)]
public class FloorsSceneSettings : ScriptableObject
{
    [SerializeField] private GameObject _floorPrefab;

    public GameObject FloorPrefab => _floorPrefab;
}