using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FloorRegistrator : BaseMonobehaviour
{
    public struct Ctx
    {
        public UserDataLoader userDataLoader;
        public ISceneLoader sceneLoader;
    }
    
    [SerializeField] private FloorBuilder _floorBuilder;
    [SerializeField] private Button _setFloorButton;
    [SerializeField] private Slider _resourcesSlider;
    [SerializeField] private TMP_Text _floorsCountText;
    [SerializeField] private TMP_Text _resourcessCountText;

    private Ctx _ctx;
    
    public void Init(Ctx ctx)
    {
        _ctx = ctx;
        
        _floorBuilder.AddFloor += OnAddFloor;
        _floorBuilder.ChangeResources += OnChangeResources;
        _setFloorButton.onClick.AddListener(() => _ctx.sceneLoader.LoadScene((int) Scenes.FloorScene, null, null));
        
        SetResourcesHUD();
        SetFloorsHUD();
    }

    private void OnDisable()
    {
        _floorBuilder.AddFloor -= OnAddFloor;
    }

    private void OnAddFloor()
    {
        _ctx.userDataLoader.CountFloors++;

        SetFloorsHUD();
        
        TryEnableSetFloorButton();
    }

    private void OnChangeResources(int newValue)
    {
        _ctx.userDataLoader.CountResources = newValue;
        SetResourcesHUD();
    }

    private void SetFloorsHUD()
    {
        _floorsCountText.text = _ctx.userDataLoader.CountFloors.ToString();
        TryEnableSetFloorButton();
    }
    
    private void SetResourcesHUD()
    {
        int resourcesCount = _ctx.userDataLoader.CountResources;
        int resourcesForFloor = _floorBuilder.ResourcesForFloor;
        _resourcesSlider.value = resourcesCount / (float) resourcesForFloor;
        _resourcessCountText.text = $"{resourcesCount} / {resourcesForFloor}";
    }

    private void TryEnableSetFloorButton()
    {
        if(_ctx.userDataLoader.CountFloors > 0)
            _setFloorButton.gameObject.SetActive(true);
    }
}
