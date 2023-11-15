using System.Collections.Generic;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingBuilder : BaseMonobehaviour
{
    public struct Ctx
    {
        public UserDataLoader userDataLoader;
        public ISceneLoader sceneLoader;
    }
    [SerializeField] private Transform _floorPrefab;
    [SerializeField] private TextMeshProUGUI _availableFloorsText;
    [SerializeField] private TextMeshProUGUI _availableFloorsHeaderText;
    [SerializeField] private TextMeshProUGUI _builtFloorsText;
    [SerializeField] private Button _placeFloor;
    [SerializeField] private Button _loadResourcesScene;
    [SerializeField] private Transform _virtualCameraTarget;
    [SerializeField] private float _floorOffsetY = 3f;
    [SerializeField] private ParticleSystem _floorBuiltEffect;

    private Ctx _ctx;
    private int _availableFloors;
    private List<Transform> _floors = new List<Transform>();

    private void Init(Ctx ctx)
    {
        _ctx = ctx;
        _availableFloors = _ctx.userDataLoader.CountFloors;

        int savedFloors = _ctx.userDataLoader.SavedFloors;
        _builtFloorsText.text = savedFloors.ToString();

        for (int i = 0; i < savedFloors; i++)
        {
            var floorInstance = Instantiate(_floorPrefab, transform);

            if (i != 0)
                floorInstance.transform.position = _floors[_floors.Count - 1].position + Vector3.up * _floorOffsetY;

            _floors.Add(floorInstance);
        }

        if (savedFloors != 0)
            _virtualCameraTarget.position = _floors[_floors.Count - 1].transform.position;

        _availableFloorsText.text = _availableFloors.ToString();

        _placeFloor.onClick.AddListener(PlaceFloor);
        _loadResourcesScene.onClick.AddListener(() => _ctx.sceneLoader.LoadScene((int) Scenes.IdleScene, null, null));

        if (_availableFloors <= 0)
        {
            _availableFloorsHeaderText.gameObject.SetActive(false);
            _availableFloorsText.gameObject.SetActive(false);
            _placeFloor.gameObject.SetActive(false);
            _loadResourcesScene.gameObject.SetActive(true);            
        }
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
        _ctx.userDataLoader.CountFloors = _availableFloors;
        _ctx.userDataLoader.SavedFloors++;
        _builtFloorsText.text = _ctx.userDataLoader.SavedFloors.ToString();
        _availableFloorsText.text = _availableFloors.ToString();

        _floorBuiltEffect.Play();

        if (_availableFloors <= 0)
        {
            _availableFloorsHeaderText.gameObject.SetActive(false);
            _availableFloorsText.gameObject.SetActive(false);
            _placeFloor.gameObject.SetActive(false);
            _loadResourcesScene.gameObject.SetActive(true);            
        }
    }
}
