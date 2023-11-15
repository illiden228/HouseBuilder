using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingBuilder : MonoBehaviour
{
    [SerializeField] private Transform _floorPrefab;
    [SerializeField] private TextMeshProUGUI _availableFloorsText;
    [SerializeField] private TextMeshProUGUI _availableFloorsHeaderText;
    [SerializeField] private TextMeshProUGUI _builtFloorsText;
    [SerializeField] private Button _placeFloor;
    [SerializeField] private Button _loadResourcesScene;
    [SerializeField] private Transform _virtualCameraTarget;
    [SerializeField] private float _floorOffsetY = 3f;
    [SerializeField] private ParticleSystem _floorBuiltEffect;

    private int _availableFloors;
    private List<Transform> _floors = new List<Transform>();

    private Transform _currentFloor;

    private void Start()
    {
        _placeFloor.onClick.AddListener(PlaceFloor);
        _loadResourcesScene.onClick.AddListener(() => LoadSceneController.Instance.LoadResourcesScene());

        _placeFloor.gameObject.SetActive(true);
        _loadResourcesScene.gameObject.SetActive(false);

        _currentFloor = Instantiate(_floorPrefab, transform);
    }

    private void PlaceFloor()
    {
        _floorBuiltEffect.Play();                
    }

    private void FixedUpdate()
    {
        if (!_currentFloor)
            return;


    }
}
