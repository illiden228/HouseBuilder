using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class FloorBuilder : BaseMonobehaviour
{
    public struct Ctx
    {
        public UserDataLoader userDataLoader;
    }
    [SerializeField] private Transform _enterPoint;
    [SerializeField] private int _countForFloor;
    [SerializeField] private ParticleSystem _smokeEffect;
    [SerializeField] private float _smokeDuration;
    
    private int _currentCount;
    private Ctx _ctx;
    
    public event Action AddFloor;
    public event Action<int> ChangeResources;

    public Transform EnterPoint => _enterPoint;
    public int ResourcesForFloor => _countForFloor;

    public void Init(Ctx ctx)
    {
        _ctx = ctx;

        _currentCount = _ctx.userDataLoader.CountResources;
    }

    public void AddPregress(int count)
    {
        _currentCount += count;
        
        if (_currentCount >= _countForFloor)
        {
            AddFloor?.Invoke();
            _currentCount -= _countForFloor;
            StartCoroutine(EnableSmoke(_smokeDuration));
        }
        
        ChangeResources?.Invoke(_currentCount);
    }

    private IEnumerator EnableSmoke(float seconds)
    {
        if (_smokeEffect == null)
            yield break;
        
        _smokeEffect.Play();
        yield return new WaitForSeconds(seconds);
        _smokeEffect.Stop();
    }
}
