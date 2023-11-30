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
        public UserDataLoader userDataLoader;
        public ReactiveProperty<Scenes> currentScene;
    }

    [SerializeField] private Transform _crane;
    [SerializeField] private ParticleSystem _floorBuiltEffect;
    [SerializeField] private Transform _cableStart;
    [SerializeField] private BezierSpline _bezierSpline;
    [SerializeField] private DeathZone _deathZone;

    private Ctx _ctx;

    public Transform Crane => _crane;
    public Transform CableStart => _cableStart;
    public BezierSpline BezierSpline => _bezierSpline;
    public DeathZone DeathZone => _deathZone;

    public void Init(Ctx ctx)
    {
        _ctx = ctx;
    }    

    public void PlayFloorPlaceEffect()
    {
        if (_floorBuiltEffect)
            _floorBuiltEffect.Play();
    }
}
