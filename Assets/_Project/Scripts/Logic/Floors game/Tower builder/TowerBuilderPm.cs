using BezierSolution;
using Core;
using System;
using System.Collections.Generic;
using Tools.Extensions;
using UniRx;
using UnityEngine;

public class TowerBuilderPm : BaseDisposable
{
    public struct Ctx
    {
        public TowerBuilderView towerBuilderView;
        public IResourceLoader resourceLoader;
        public ReactiveEvent onReleaseFloor;
        public Action onBackToIdleScene;
    }

    private readonly Ctx _ctx;

    //To do replace to scriptable object
    private Vector3 _additionalForceToFloor = new Vector3(0f, -9.81f, 0f);
    private float _initialXImpulse = 6.5f;
    private float _initialYImpulse = -0.5f;
    private float _offsetYFloor = 3f;
    private float _oscillationAmplitude = 7.61f;
    private float _maxFloorDeviation = 2.535f;
    private float _timeToDestroyFailFloor = 10f;
    private float _maxAmplitudeTowerRocking = 2.61f;    
    private float _speedTowerRocking = 1f;

    private const string FloorViewPrefabName = "FloorView";
    private FloorView _floorViewPrefab;
    private float _averageTowerRockingOffsetX;
    private float _targetYPos = 0f;
    private bool _floorReleased = false;
    private FloorView _floorToReleaseInstance;
    private BezierWalkerWithSpeed _bezierWalkerWithSpeed;
    private bool _canReleaseFloor = true;
    private List<Floor> _installedFloors;
    private ReactiveEvent _floorCollisionEvent;
    private IDisposable _destroyFailFloorDisposable;
    private RigidbodyConstraints _constraints =
        RigidbodyConstraints.FreezePositionZ |
        RigidbodyConstraints.FreezeRotationX |
        RigidbodyConstraints.FreezeRotationY;
    

    public TowerBuilderPm(Ctx ctx)
    {
        _ctx = ctx;

        ReactiveEvent floorFailEvent = new ReactiveEvent();

        _ctx.towerBuilderView.Init(new TowerBuilderView.Ctx
        {
            onFloorFallInDeathZone = floorFailEvent
        });

        _bezierWalkerWithSpeed = _ctx.towerBuilderView.CableStart.GetComponent<BezierWalkerWithSpeed>();

        AddDispose(_ctx.onReleaseFloor.SubscribeWithSkip(ReleaseFloor));
        AddDispose(ReactiveExtensions.StartFixedUpdate(FixedUpdateFloorFall));
        AddDispose(ReactiveExtensions.StartUpdate(UpdateTowerRocking));
        AddDispose(floorFailEvent.SubscribeOnceWithSkip(OnFloorFallInDeathZone));
                
        _installedFloors = new List<Floor>();

        _ctx.resourceLoader.LoadPrefab("fakebundles", FloorViewPrefabName, OnResourcesLoaded);

        CreateNewFloor();
    }

    private void OnResourcesLoaded(GameObject prefab) => _floorViewPrefab = prefab.GetComponent<FloorView>();

    private void CreateNewFloor(bool moveCrane = false)
    {
        _floorToReleaseInstance = GameObject.Instantiate(_floorViewPrefab, _ctx.towerBuilderView.transform);
        _floorToReleaseInstance.transform.SetParent(_ctx.towerBuilderView.CableStart);        
        _ctx.towerBuilderView.CableStart.SetParent(_floorToReleaseInstance.transform);
        _floorToReleaseInstance.transform.localPosition = new Vector3(0f, -3.25f, 0f);
        _floorToReleaseInstance.CanCheckCollision = true;

        if (moveCrane)
            _ctx.towerBuilderView.Crane.transform.position += new Vector3(0f, _offsetYFloor, 0f);
    }

    private void UpdateTowerRocking()
    {
        //if (_installedFloors.Count < 5)
        //    return;

        //for (int i = 0; i < _installedFloors.Count; i++)
        //{
        //    float offsetX = Mathf.Sin(Time.time * _speedTowerRocking) * _installedFloors[i].floorTowerRockingOffsetX;
        //    _installedFloors[i].floorView.transform.position = new Vector3(
        //        _installedFloors[i].installationPointX + offsetX,
        //        _installedFloors[i].floorView.transform.position.y,
        //        _installedFloors[i].floorView.transform.position.z);
        //}
    }

    private void FixedUpdateFloorFall()
    {
        if (_floorToReleaseInstance == null)
            return;

        if (_floorReleased)
            _floorToReleaseInstance.Rigidbody.AddForce(_additionalForceToFloor, ForceMode.Force);
    }

    private void ReleaseFloor()
    {
        if (!_floorToReleaseInstance.Rigidbody || !_canReleaseFloor)
            return;

        _canReleaseFloor = false;
        _floorToReleaseInstance.transform.SetParent(_ctx.towerBuilderView.transform);
        _floorToReleaseInstance.Rigidbody.isKinematic = false;
        _floorToReleaseInstance.Rigidbody.useGravity = true;
        _floorReleased = true;

        float impulseX = 0f;
                
        if (_floorToReleaseInstance.transform.position.x > 0f)
            impulseX = _bezierWalkerWithSpeed.MovingForward ? 1f : -1f * Mathf.InverseLerp(0f, _oscillationAmplitude, _floorToReleaseInstance.transform.position.x);
        else if (_floorToReleaseInstance.transform.position.x < 0f)
            impulseX = _bezierWalkerWithSpeed.MovingForward ? 1f : -1f * Mathf.InverseLerp(-_oscillationAmplitude, 0f, _floorToReleaseInstance.transform.position.x);

        _floorToReleaseInstance.Rigidbody.AddForce(new Vector3(impulseX * _initialXImpulse, _initialYImpulse, 0f), ForceMode.Impulse);

        _floorCollisionEvent = new ReactiveEvent();
        _floorToReleaseInstance.SetCollisionEvent(_floorCollisionEvent);
        _floorCollisionEvent.SubscribeWithSkip(OnFloorCollision);        
    }

    private void OnFloorCollision()
    {
        _floorCollisionEvent.Dispose();

        _floorToReleaseInstance.CanCheckCollision = false;
        _floorToReleaseInstance.Rigidbody.isKinematic = true;
        _floorToReleaseInstance.Rigidbody.useGravity = false;

        _floorReleased = false;        
        _ctx.towerBuilderView.PlayFloorPlaceEffect();

        float moduleFloorDeviation = 0f;

        if (CheckIfDeviationGreaterThanPossible(ref moduleFloorDeviation))
        {
            Debug.Log("Deviation greater than possible");
            _canReleaseFloor = false;
            FloorGameOver();
        }
        else
        {
            _floorToReleaseInstance.transform.position = new Vector3(
                _floorToReleaseInstance.transform.position.x,
                _targetYPos,
                _floorToReleaseInstance.transform.position.z);
            _targetYPos += _offsetYFloor;

            float installationCoef = Mathf.InverseLerp(0, _maxFloorDeviation, moduleFloorDeviation);

            _installedFloors.Add(new Floor
            {
                floorView = _floorToReleaseInstance,
                installationPointX = _floorToReleaseInstance.transform.position.x,                
                installationCoefficient = installationCoef
            });

            CalculateAverageAmplitudeTowerRocking();

            Debug.Log("Tower placed with installationCoefficient: " + installationCoef);           

            CreateNewFloor(true);
            _canReleaseFloor = true;
        }
    }

    private void CalculateAverageAmplitudeTowerRocking()
    {
        float installationCoefficientSum = 0f;

        for (int i = 0; i < _installedFloors.Count; i++)
        {
            installationCoefficientSum += Mathf.Lerp(0f, _maxAmplitudeTowerRocking, _installedFloors[i].installationCoefficient);
        }

        _averageTowerRockingOffsetX = installationCoefficientSum / _installedFloors.Count > 35 ? _installedFloors.Count - 35 : _installedFloors.Count;

        for (int i = 0; i < _installedFloors.Count; i++)
        {
            float towerHeightLerp = Mathf.InverseLerp(5, 40, _installedFloors.Count);
            _installedFloors[i].floorTowerRockingOffsetX = Mathf.Lerp(0f, _maxAmplitudeTowerRocking, _averageTowerRockingOffsetX * towerHeightLerp);
        }
    }

    private bool CheckIfDeviationGreaterThanPossible(ref float moduleFloorDeviation)
    {
        if (_installedFloors.Count > 0)
        {
            moduleFloorDeviation = Mathf.Abs(_installedFloors[_installedFloors.Count - 1].floorView.transform.position.x - _floorToReleaseInstance.transform.position.x);

            if (moduleFloorDeviation > _maxFloorDeviation)
                return true;
            else
                return false;
        }
        else
            return false;
    }

    private void OnFloorFallInDeathZone()
    {
        _canReleaseFloor = false;
        Debug.Log("Death zone collision");
        FloorGameOver();
    }

    private void FloorGameOver()
    {
        _floorToReleaseInstance.Rigidbody.isKinematic = false;
        _floorToReleaseInstance.Rigidbody.constraints = _constraints;
        _floorToReleaseInstance.Rigidbody.useGravity = true;

        _destroyFailFloorDisposable = ReactiveExtensions.DelayedCall(_timeToDestroyFailFloor, () =>
        {
            if (_floorToReleaseInstance != null)
                GameObject.Destroy(_floorToReleaseInstance.gameObject);
            _destroyFailFloorDisposable.Dispose();
        });
        AddDispose(_destroyFailFloorDisposable);
    }

    public class Floor
    {
        public FloorView floorView;
        public float installationCoefficient;
        public float installationPointX;
        public float floorTowerRockingOffsetX;
    }
}
