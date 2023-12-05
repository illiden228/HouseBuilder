using SceneLogic;
using UnityEngine;

public class FloorsSceneEntryPoint : MonoBehaviour
{
    // ��� �����
    // � ����� ����� GamePm ����� �������
    [SerializeField] private bool _debug = false;
    [SerializeField] private FloorsContextView _floorsContextView;
    [SerializeField] private FloorsSceneSettings _settings;

    private FloorsRoot _root;

    private void Awake()
    {
        if (_debug)
            _root = new FloorsRoot(new FloorsRoot.Ctx
            {
                sceneContext = _floorsContextView,
                floorsSettings = _settings
            });
    }
}
