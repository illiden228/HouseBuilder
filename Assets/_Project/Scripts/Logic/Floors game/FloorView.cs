using BezierSolution;
using System;
using Tools.Extensions;
using UnityEngine;

public class FloorView : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;

    public Rigidbody Rigidbody => _rigidbody;

    public bool CanCheckCollision = false;

    private ReactiveEvent _onFloorCollision;

    public void SetCollisionEvent(ReactiveEvent onFloorCollisionEvent)
    {
        _onFloorCollision = onFloorCollisionEvent;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out FloorView floorView)) 
        { 
            if (CanCheckCollision)
                _onFloorCollision?.Notify();
        }            
    }
}
