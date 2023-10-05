using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class FloorBuilder : MonoBehaviour
{
    [SerializeField] private Transform _enterPoint;
    [SerializeField] private int _countForFloor;
    [SerializeField] private ParticleSystem _smokeEffect;
    [SerializeField] private float _smokeDuration;
    private int _currentCount;

    public event Action AddFloor;
    public event Action<int> ChangeResources;

    public Transform EnterPoint => _enterPoint;
    public int ResourcesForFloor => _countForFloor;

    private void Start()
    {
        _currentCount = UserData.Instance.CountResources;
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
