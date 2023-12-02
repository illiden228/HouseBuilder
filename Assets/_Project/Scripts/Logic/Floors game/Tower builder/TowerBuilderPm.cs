using BezierSolution;
using Core;
using Tools.Extensions;
using UnityEngine;

public class TowerBuilderPm : BaseDisposable
{
    public struct Ctx
    {
        public TowerBuilderView towerBuilderView;
        //public UserDataLoader userDataLoader;
        //public ISceneLoader sceneLoader;
        public GameObject floorViewPrefab;        
        public ReactiveEvent onReleaseFloor;        
    }

    private readonly Ctx _ctx;

    private FloorView _floorToPlace;
    private Vector3 _additionalForceToFloor = new Vector3(0f, -9.81f, 0f);
    private float _initialXImpulse = 6.5f;
    private bool _floorReleased = false;
    private float _offsetYFloor = 3f;
    private float _targetYPos = 0f;
    private BezierWalkerWithSpeed _bezierWalkerWithSpeed;
    private bool _canPlaceFloor = true;
    private bool _gameLoose = false;

    public TowerBuilderPm(Ctx ctx)
    {
        _ctx = ctx;

        ReactiveEvent floorFailEvent = new ReactiveEvent();

        _ctx.towerBuilderView.Init(new TowerBuilderView.Ctx
        {
            onFloorFallInDeathZone = floorFailEvent
        });

        _bezierWalkerWithSpeed = _ctx.towerBuilderView.CableStart.GetComponent<BezierWalkerWithSpeed>();

        AddDispose(_ctx.onReleaseFloor.SubscribeWithSkip(PlaceFloor));
        AddDispose(ReactiveExtensions.StartFixedUpdate(FixedUpdate));
        AddDispose(floorFailEvent.SubscribeOnceWithSkip(OnFloorFallInDeathZone));

        //_availableFloors = _ctx.userDataLoader.CountFloors;

        //int savedFloors = _ctx.userDataLoader.SavedFloors;

        //for (int i = 0; i < savedFloors; i++)
        //{
        //    Transform floorInstance = GameObject.Instantiate(_ctx.floorViewPrefab, _ctx.towerBuilderView.transform).transform;

        //    if (i != 0)
        //        floorInstance.transform.position = _floors[_floors.Count - 1].position + Vector3.up * _floorOffsetY;

        //    _floors.Add(floorInstance);
        //}

        CreateNewFloor();
    }

    private void CreateNewFloor(bool moveCrane = false)
    {
        var instance = GameObject.Instantiate(_ctx.floorViewPrefab, _ctx.towerBuilderView.transform);
        _floorToPlace = instance.GetComponent<FloorView>();
        _floorToPlace.transform.SetParent(_ctx.towerBuilderView.CableStart);
        _ctx.towerBuilderView.CableStart.SetParent(_floorToPlace.transform);
        _floorToPlace.transform.localPosition = new Vector3(0f, -3.25f, 0f);

        if (moveCrane)
        {
            _ctx.towerBuilderView.Crane.transform.position += new Vector3(0f, _offsetYFloor, 0f);
        }
    }

    private void FixedUpdate()
    {
        if (_floorToPlace == null)
            return;

        if (_floorReleased)
            _floorToPlace.Rigidbody.AddForce(_additionalForceToFloor, ForceMode.Force);

        //Vector3 normalizedDirection = _floorToPlace.Rigidbody.velocity.normalized;
        //RaycastHit hit;
        //if (Physics.Raycast(_floorToPlace.transform.position + _floorOffsetRayCast, normalizedDirection, out hit, Mathf.Infinity))
        //{
        //    Debug.DrawRay(_floorToPlace.transform.position + _floorOffsetRayCast, normalizedDirection * hit.distance, Color.yellow);
        //}
        //else
        //{
        //    Debug.DrawRay(_floorToPlace.transform.position + _floorOffsetRayCast, normalizedDirection * 1000, Color.white);
        //}
    }

    private void PlaceFloor()
    {
        if (!_floorToPlace.Rigidbody || !_canPlaceFloor)
            return;

        _canPlaceFloor = false;
        _floorToPlace.transform.SetParent(_ctx.towerBuilderView.transform);
        _floorToPlace.Rigidbody.isKinematic = false;
        _floorToPlace.Rigidbody.useGravity = true;
        _floorReleased = true;

        float impulseX = 0f;

        if (_floorToPlace.transform.position.x > 0f)
            impulseX = _bezierWalkerWithSpeed.MovingForward ? 1f : -1f * Mathf.InverseLerp(0f, 7.61f, _floorToPlace.transform.position.x);
        else if (_floorToPlace.transform.position.x < 0f)
            impulseX = _bezierWalkerWithSpeed.MovingForward ? 1f : -1f * Mathf.InverseLerp(-7.61f, 0f, _floorToPlace.transform.position.x);

        _floorToPlace.Rigidbody.AddForce(new Vector3(impulseX * _initialXImpulse, 0f, 0f), ForceMode.Impulse);

        _floorToPlace.OnCollision += OnFloorCollision;

        //if (_availableFloors <= 0)
        //{
        //    _availableFloorsHeaderText.gameObject.SetActive(false);
        //    _availableFloorsText.gameObject.SetActive(false);
        //    _placeFloor.gameObject.SetActive(false);
        //    _loadResourcesScene.gameObject.SetActive(true);
        //    return;
        //}

        //var prefabInstance = Instantiate(_floorPrefab, transform);

        //if (_floors.Count != 0)
        //    prefabInstance.transform.position = _floors[_floors.Count - 1].position + Vector3.up * _floorOffsetY;

        //_virtualCameraTarget.position = prefabInstance.transform.position;
        //_floors.Add(prefabInstance);

        //_availableFloors--;
        //_ctx.userDataLoader.CountFloors = _availableFloors;
        //_ctx.userDataLoader.SavedFloors++;
        //_builtFloorsText.text = _ctx.userDataLoader.SavedFloors.ToString();
        //_availableFloorsText.text = _availableFloors.ToString();

        //_floorBuiltEffect.Play();

        //if (_availableFloors <= 0)
        //{
        //    _availableFloorsHeaderText.gameObject.SetActive(false);
        //    _availableFloorsText.gameObject.SetActive(false);
        //    _placeFloor.gameObject.SetActive(false);
        //    _loadResourcesScene.gameObject.SetActive(true);
        //}
    }

    private void OnFloorCollision()
    {
        _floorToPlace.OnCollision -= OnFloorCollision;

        _floorToPlace.Rigidbody.isKinematic = true;
        _floorToPlace.Rigidbody.useGravity = false;
        _floorReleased = false;
        _floorToPlace.transform.position = new Vector3(_floorToPlace.transform.position.x, _targetYPos, _floorToPlace.transform.position.z);
        _targetYPos += _offsetYFloor;
        CreateNewFloor(true);
        _ctx.towerBuilderView.PlayFloorPlaceEffect();
        _canPlaceFloor = true;
    }

    private void OnFloorFallInDeathZone()
    {
        _gameLoose = true;
    }
}
