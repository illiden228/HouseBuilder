using System;
using System.Collections.Generic;
using Core;
using Tools.Extensions;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Core
{
    public class ResourcePreLoader : BaseDisposable, IResourceLoader
    {
        public struct Ctx
        {
            public float minLoadDelay;
            public float maxLoadDelay;
        }

        private readonly Ctx _ctx;
        private readonly ResourceConfigMain _configMain;
        private readonly Dictionary<string, GameObject> _prefabsToLoadCache;
        private readonly Dictionary<string, Sprite> _spritesToLoadCache;
        private readonly HashSet<string> _cacheImitator;

        private const string PRELOADED_FILES_PATH = "fakebundles/ResourceConfigMain";

        public ResourcePreLoader(Ctx ctx)
        {
            _ctx = ctx;
            _configMain = Resources.Load<ResourceConfigMain>(PRELOADED_FILES_PATH);
            _prefabsToLoadCache = new Dictionary<string, GameObject>();
            _cacheImitator = new HashSet<string>();

            if (_configMain == null)
            {
                Debug.LogWarning("Main Config is not found!");
                return;
            }
            
            foreach (ResourceConfigPrefabs config in _configMain.PrefabConfigs)
            {
                if (config == null)
                    continue;
                FillDictionaryFromArray(_prefabsToLoadCache, config.Prefabs);
            }
        }

        public bool CheckExistance(string bundleName)
        {
            return true;
        }

        public IDisposable LoadPrefab(string bundleName, string prefabName, Action<GameObject> onComplete)
        {
            GameObject prefab = GetResource(_prefabsToLoadCache, prefabName);
            return ImitateLoadingBundle(bundleName, () => onComplete?.Invoke(prefab));
        }

        public IDisposable LoadSprite(string bundleName, string spriteName, Action<Sprite> onComplete)
        {
            Sprite prefab = GetResource(_spritesToLoadCache, spriteName);
            return ImitateLoadingBundle(bundleName, () => onComplete?.Invoke(prefab));
        }

        public void LoadSceneBundle(int sceneIndex, Action<bool> onComplete)
        {
            onComplete?.Invoke(true);
        }

        public void Dispose()
        {
            Resources.UnloadAsset(_configMain);
        }
        
        private T GetResource<T>(IReadOnlyDictionary<string, T> map, string resourceName = null) where T : Object
        {
            if (map == null)
            {
                Debug.LogError($"Can't get resource from nullable map by '{resourceName}'");
                return null;
            }

            if (resourceName == null)
            {
                
            }
            string key = resourceName;
            if (!map.TryGetValue(key, out T ret))
            {
                Debug.LogError($"Can't get resource '{resourceName}'");
                return null;
            }
            return ret;
        }
        
        private IDisposable ImitateLoadingBundle(string bundleName, Action onComplete)
        {
            if (string.IsNullOrEmpty(bundleName))
            {
                onComplete?.Invoke();
                return null;
            }
            if (_cacheImitator.Contains(bundleName))
            {
                onComplete?.Invoke();
                return null;
            }
      
            _cacheImitator.Add(bundleName);
            if (_ctx.maxLoadDelay < 0.01f)
            {
                onComplete?.Invoke();
                return null;
            }
            float delay = Random.Range(_ctx.minLoadDelay, _ctx.maxLoadDelay);
            IDisposable delayedCall = ReactiveExtensions.DelayedCall(delay, () => { onComplete?.Invoke(); });
            AddDispose(delayedCall);
            return delayedCall;
        }
        
        private void FillDictionaryFromArray<T>(IDictionary<string, T> dict, IReadOnlyList<T> gameObjects) where T : Object
        {
            if (gameObjects == null || gameObjects.Count <= 0)
                return;
            for (int i = 0, ik = gameObjects.Count; i < ik; ++i)
            {
                T gameObj = gameObjects[i];
                if (gameObj != null)
                {
                    if (dict.ContainsKey(gameObj.name))
                        Debug.Log($"duplicating '{gameObj.name}' in content");
                    dict[gameObj.name] = gameObj;
                }
            }
        }
    }
}