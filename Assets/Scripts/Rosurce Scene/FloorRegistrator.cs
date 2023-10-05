using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FloorRegistrator : MonoBehaviour
{
    [SerializeField] private FloorBuilder _floorBuilder;
    [SerializeField] private Button _setFloorButton;
    [SerializeField] private Slider _resourcesSlider;
    [SerializeField] private TMP_Text _floorsCountText;
    [SerializeField] private TMP_Text _resourcessCountText;

    private void Awake()
    {
        _floorBuilder.AddFloor += OnAddFloor;
        _floorBuilder.ChangeResources += OnChangeResources;
        _setFloorButton.onClick.AddListener(() => LoadSceneController.Instance.LoadFloorScene());
    }

    private void Start()
    {
        SetResourcesHUD();
        SetFloorsHUD();
    }

    private void OnDisable()
    {
        _floorBuilder.AddFloor -= OnAddFloor;
    }

    private void OnAddFloor()
    {
        UserData.Instance.CountFloors++;

        SetFloorsHUD();
        
        TryEnableSetFloorButton();
    }

    private void OnChangeResources(int newValue)
    {
        UserData.Instance.CountResources = newValue;
        SetResourcesHUD();
    }

    private void SetFloorsHUD()
    {
        _floorsCountText.text = UserData.Instance.CountFloors.ToString();
        TryEnableSetFloorButton();
    }
    
    private void SetResourcesHUD()
    {
        int resourcesCount = UserData.Instance.CountResources;
        int resourcesForFloor = _floorBuilder.ResourcesForFloor;
        _resourcesSlider.value = resourcesCount / (float) resourcesForFloor;
        _resourcessCountText.text = $"{resourcesCount} / {resourcesForFloor}";
    }

    private void TryEnableSetFloorButton()
    {
        if(UserData.Instance.CountFloors > 0)
            _setFloorButton.gameObject.SetActive(true);
    }
}
