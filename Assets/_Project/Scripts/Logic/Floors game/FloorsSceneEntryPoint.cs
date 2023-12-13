using Core;
using Logic.Idle;
using SceneLogic;
using System;
using UnityEngine;

[Obsolete]
public class FloorsSceneEntryPoint : MonoBehaviour
{
    // для теста
    // в проде будет GamePm будет инитить
    [SerializeField] private bool _debug = false;
    [SerializeField] private FloorsContextView _floorsContextView;
    [SerializeField] private FloorsSceneSettings _settings;
    [SerializeField] private EntryPoint.ResourceLoadType _resourceLoadType;

    private FloorsScenePm _root;
    private IResourceLoader _resourceLoader;

    private void Awake()
    {
        if (_debug)
            _root = new FloorsScenePm(new FloorsScenePm.Ctx
            {
                sceneContext = _floorsContextView,
                resourceLoader = _resourceLoader                
            });
    }
}
