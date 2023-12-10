using SceneLogic;
using UnityEngine;

public class FloorsSceneEntryPoint : MonoBehaviour
{
    // для теста
    // в проде будет GamePm будет инитить
    [SerializeField] private bool _debug = false;
    [SerializeField] private FloorsContextView _floorsContextView;
    [SerializeField] private FloorsSceneSettings _settings;

    private FloorsScenePm _root;

    private void Awake()
    {
        if (_debug)
            _root = new FloorsScenePm(new FloorsScenePm.Ctx
            {
                sceneContext = _floorsContextView               
            });
    }
}
