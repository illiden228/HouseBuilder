using UnityEngine;

public class FloorsSceneEntryPoint : MonoBehaviour
{
    [SerializeField] private FloorsSceneContext _floorsSceneContext;
    [SerializeField] private FloorsSceneSettings _settings;

    private FloorsRoot _root;

    private void Awake()
    {
        _root = new FloorsRoot(new FloorsRoot.Ctx
        {
            sceneContext = _floorsSceneContext,
            floorsSettings = _settings
        });
    }
}
