using System;

namespace Core
{
    public interface ISceneLoader : IDisposable
    {
        void LoadScene(int sceneIndex, Action onUnload, Action onComplete);
    }
}