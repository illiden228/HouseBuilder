using System;
using UnityEngine;

namespace Core
{
    public interface IResourceLoader : IDisposable
    {
        bool CheckExistance(string bundleName);
        IDisposable LoadPrefab(string bundleName, string prefabName, Action<GameObject> onComplete);
        IDisposable LoadSprite(string bundleName, string spriteName, Action<Sprite> onComplete);
        void LoadSceneBundle(int sceneIndex, Action<bool> onComplete);
    }
}