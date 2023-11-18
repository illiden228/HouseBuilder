using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] private int _bagCount;
    [SerializeField] private GameObject _bagPrefab;
    [SerializeField] private Transform _enterPoint;
    private GameObject _bag;

    public Transform EnterPoint => _enterPoint;

    public void Init()
    {
        _bag = Instantiate(_bagPrefab);
        _bag.SetActive(false);
    }

    public void ReturnBag()
    {
        _bag.SetActive(false);
        _bag.transform.SetParent(null);
    }

    public int GetBag(Transform position)
    {
        _bag.transform.SetParent(position);
        _bag.transform.localPosition = Vector3.zero;
        _bag.SetActive(true);
        return _bagCount;
    }
}
