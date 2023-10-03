using UnityEngine;

public class BuildingBuilder : MonoBehaviour
{
    [SerializeField] private Transform _floorPrefab;
    [SerializeField] private int _availableFloorsTest;

    private int _availableFloors;

    private void Start()
    {
        _availableFloors = _availableFloorsTest;
    }

    
}
