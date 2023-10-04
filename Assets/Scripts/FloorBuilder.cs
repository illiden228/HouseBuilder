using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class FloorBuilder : MonoBehaviour
{
    [SerializeField] private Transform _enterPoint;
    [SerializeField] private int _countForFloor;
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
        }
        
        ChangeResources?.Invoke(_currentCount);
    }
}
