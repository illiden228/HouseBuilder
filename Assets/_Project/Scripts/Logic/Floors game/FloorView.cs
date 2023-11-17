using BezierSolution;
using System;
using UnityEngine;

public class FloorView : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;

    public Rigidbody Rigidbody => _rigidbody;

    public Action OnCollision;

    private void OnCollisionEnter(Collision collision)
    {
        OnCollision?.Invoke();
    }
}
