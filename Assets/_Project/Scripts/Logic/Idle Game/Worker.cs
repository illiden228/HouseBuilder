using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Animator))]
public class Worker : MonoBehaviour
{
    [SerializeField] private Transform _bagPoint;
    [SerializeField] private FloorBuilder _floorBuilder;
    [SerializeField] private Storage _storage;
    [SerializeField] private float _speed;
    private Animator _animator;
    private Transform _currentTarget;
    private bool _isCarry;
    private int _bagCount;
    private bool _initred;
    
    public void Init()
    {
        _animator = GetComponent<Animator>();
        _currentTarget = _storage.EnterPoint;
        _initred = true;
    }

    private void Update()
    {
        if(!_initred)
            return;
        if(_currentTarget == null)
            return;
        Vector3 direction = _currentTarget.position - transform.position;
        if (direction.sqrMagnitude > 0.01)
        {
            float delta = _speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _currentTarget.position, delta);
            transform.forward = direction;
        }
        else
        {
            _currentTarget = null;
            TargetReached();
        }
        
    }

    private void TargetReached()
    {
        if (_isCarry)
        {
            _animator.SetTrigger(AnimatorNames.Move);
            _storage.ReturnBag();
            _floorBuilder.AddPregress(_bagCount);
            _currentTarget = _storage.EnterPoint;
            _bagCount = 0;
            _isCarry = false;
        }
        else
        {
            _animator.SetTrigger(AnimatorNames.Carry);
            _bagCount = _storage.GetBag(_bagPoint);
            _currentTarget = _floorBuilder.EnterPoint;
            _isCarry = true;
        }
    }
}
