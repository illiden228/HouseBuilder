using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class LoadSceneController : MonoBehaviour
{
    [SerializeField] private int _resourcesSceneIndex = 0;
    [SerializeField] private int _floorSceneIndex = 1;

    private AsyncOperation asyncLoad;
    private WaitForSecondsRealtime _waitBeforeLoad = new WaitForSecondsRealtime(0.2f);

    private static LoadSceneController instance;
    public static LoadSceneController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new LoadSceneController();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public void LoadResourcesScene(bool allowSceneActivation = true) =>
        StartCoroutine(DoSwitchScene(_resourcesSceneIndex, allowSceneActivation));

    public void LoadFloorScene(bool allowSceneActivation = true) =>
        StartCoroutine(DoSwitchScene(_floorSceneIndex, allowSceneActivation));

    private IEnumerator DoSwitchScene(int index, bool allowSceneActivation = true)
    {
        asyncLoad = SceneManager.LoadSceneAsync(index);
        asyncLoad.allowSceneActivation = false;

        if (allowSceneActivation)
        {
            yield return (asyncLoad.progress >= 1f);
            yield return _waitBeforeLoad;
            asyncLoad.allowSceneActivation = allowSceneActivation;
        }
        else
            yield break;
    }
}
