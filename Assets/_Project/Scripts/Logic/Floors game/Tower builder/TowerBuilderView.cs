using System;
using System.Collections.Generic;
using BezierSolution;
using Core;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class TowerBuilderView : BaseMonobehaviour
{
    public struct Ctx
    {
        public int savedFloors;
    }

    [SerializeField] private Transform _crane;
    [SerializeField] private ParticleSystem _floorBuiltEffect;
    [SerializeField] private Transform _cableStart;
    [SerializeField] private BezierSpline _bezierSpline;

    private Ctx _ctx;

    public Transform Crane => _crane;
    public Transform CableStart => _cableStart;
    public BezierSpline BezierSpline => _bezierSpline;

    public void Init(Ctx ctx)
    {
        _ctx = ctx;

        //int savedFloors = _ctx.savedFloors;
        //_builtFloorsText.text = savedFloors.ToString();

        //if (savedFloors != 0)
        //    _virtualCameraTarget.position = _floors[_floors.Count - 1].transform.position;

        //_availableFloorsText.text = _availableFloors.ToString();

        //_loadResourcesScene.onClick.AddListener(() => _ctx.sceneLoader.LoadScene((int) Scenes.IdleScene, null, null));

        //if (_availableFloors <= 0)
        //{
        //    _availableFloorsHeaderText.gameObject.SetActive(false);
        //    _availableFloorsText.gameObject.SetActive(false);
        //    _placeFloor.gameObject.SetActive(false);
        //    _loadResourcesScene.gameObject.SetActive(true);            
        //}
    }    
}
