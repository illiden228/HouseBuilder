using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingBuilder : MonoBehaviour
{
    [SerializeField] private Transform _floorPrefab;
    [SerializeField] private TextMeshProUGUI _availableFloorsText;
    [SerializeField] private TextMeshProUGUI _availableFloorsHeaderText;
    [SerializeField] private Button _placeFloor;
    [SerializeField] private Button _loadResourcesScene;
    [SerializeField] private Transform _virtualCameraTarget;
    [SerializeField] private float _floorOffsetY = 3f;

    private int _availableFloors;
    private List<Transform> _floors = new List<Transform>();

    private void Start()
    {
        _availableFloors = UserData.Instance.CountFloors;
        _availableFloorsText.text = _availableFloors.ToString();

        _placeFloor.onClick.AddListener(PlaceFloor);
        _loadResourcesScene.onClick.AddListener(LoadResourcesScene);
    }

    private void PlaceFloor()
    {
        if (_availableFloors <= 0)
        {
            _availableFloorsHeaderText.gameObject.SetActive(false);
            _availableFloorsText.gameObject.SetActive(false);
            _placeFloor.gameObject.SetActive(false);
            _loadResourcesScene.gameObject.SetActive(true);
            return;
        }

        var prefabInstance = Instantiate(_floorPrefab, transform);

        if (_floors.Count != 0)
            prefabInstance.transform.position = _floors[_floors.Count - 1].position + Vector3.up * _floorOffsetY;

        _virtualCameraTarget.position = prefabInstance.transform.position;
        _floors.Add(prefabInstance);

        _availableFloors--;
        UserData.Instance.CountFloors = _availableFloors;
        _availableFloorsText.text = _availableFloors.ToString();
    }

    private void LoadResourcesScene() => LoadSceneController.Instance.LoadResourcesScene();
}
