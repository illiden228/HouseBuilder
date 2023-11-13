using System;
using Tools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;

namespace Core
{
  public class SceneLoader : BaseDisposable, ISceneLoader
  {
    public struct Ctx
    {
      public IResourceLoader resourceLoader;
    }

    private readonly Ctx _ctx;

    private bool _isLoading;
    private IDisposable _unloadingLevel;
    private IDisposable _loadingLevel;

    public SceneLoader(Ctx ctx)
    {
      _ctx = ctx;
      _isLoading = false;
    }

    public void LoadScene(int sceneIndex, Action onUnload, Action onComplete)
    {
      if (_isLoading)
      {
        Debug.LogError($"Can't start load {sceneIndex}. Level loader is busy");
        onComplete?.Invoke();
        return;
      }

      _isLoading = true;
      Debug.Log($"Trying to load scene {sceneIndex}");
      Scene oldScene = SceneManager.GetActiveScene();
      _ctx.resourceLoader.LoadSceneBundle(sceneIndex, OnNewSceneLoadedToMemory);

      void OnNewSceneLoadedToMemory(bool result)
      {
        Debug.Log($"Scene {sceneIndex} bundle loaded to memory");
        LoadSceneAsync(sceneIndex, OnNewSceneLoaded);
      }

      void OnNewSceneLoaded()
      {
        Debug.Log($"Scene {sceneIndex} loaded");
        _isLoading = false;
        _loadingLevel?.Dispose();
        onUnload?.Invoke();
        TryUnloadScene(oldScene, onComplete);
      }
    }

    protected override void OnDispose()
    {
      Reset();
      base.OnDispose();
    }

    private void Reset()
    {
      _isLoading = false;
      _unloadingLevel?.Dispose();
      _loadingLevel?.Dispose();
    }

    private void LoadSceneAsync(int index, Action onComplete)
    {
      // Resources.UnloadUnusedAssets();
      _loadingLevel?.Dispose();
      //AsyncOperation loadingSceneOp = SceneManager.LoadSceneAsync(name,new LoadSceneParameters(LoadSceneMode.Additive, LocalPhysicsMode.Physics3D));
      AsyncOperation loadingSceneOp = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
      _loadingLevel = loadingSceneOp.AsAsyncOperationObservable().Take(1).Subscribe(_ => { onComplete?.Invoke(); });
    }

    private void TryUnloadScene(Scene sceneToUnload, Action onComplete)
    {
      _unloadingLevel?.Dispose();
      try
      {
        // SceneManager.CreateScene(EMPTY_SCENE_NAME);
        AsyncOperation unloadingSceneOp = SceneManager.UnloadSceneAsync(sceneToUnload);
        if (unloadingSceneOp == null)
        {
          onComplete?.Invoke();
          return;
        }

        _unloadingLevel = unloadingSceneOp.AsAsyncOperationObservable().Take(1)
          .Subscribe(_ => { onComplete?.Invoke(); });
      }
      catch (Exception e)
      {
        Debug.LogError($"Exception in unloading scene {e}.");
        onComplete?.Invoke();
      }
    }
  }
}