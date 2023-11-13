using Core;
using Logic.Intro;
using SceneLogic;
using UniRx;
using UnityEngine;

namespace Logic.Idle
{
    public class EntryPoint : BaseMonobehaviour
    {
        public enum ResourceLoadType
        {
            FakeBoundles,
        }

        public enum LocalStorageType
        {
            JsonFile,
        }

        [SerializeField] private ResourceLoadType _resourceLoadType;
        [SerializeField] private LocalStorageType _localStorageType;
        [SerializeField] private int _resourcesSceneIndex = 0;
        [SerializeField] private int _floorSceneIndex = 1;

        private IResourceLoader _resourceLoader;
        private IStorageService _storageService;
        private ISceneLoader _sceneLoader;
        private GamePm _game;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            _resourceLoader = CreateResourceLoader(_resourceLoadType);
            _storageService = CreateStorageService(_localStorageType);
            _sceneLoader = CreateSceneLoader(_resourceLoadType);
            UserDataLoader userDataLoader = new UserDataLoader(_storageService);

            _game = new GamePm(new GamePm.Ctx
            {
                resourceLoader = _resourceLoader,
                sceneLoader = _sceneLoader,
                userDataLoader = userDataLoader,
                currentScene = new ReactiveProperty<Scenes>(Scenes.IdleScene)
            });
        }

        private IResourceLoader CreateResourceLoader(ResourceLoadType type)
        {
            switch (type)
            {
                case ResourceLoadType.FakeBoundles:
                    return new ResourcePreLoader(new ResourcePreLoader.Ctx
                    {
                        maxLoadDelay = 0.1f,
                        minLoadDelay = 0.1f
                    });
            }

            Debug.LogError($"Resource Loader type {type} is not found");
            return null;
        }

        private IStorageService CreateStorageService(LocalStorageType type)
        {
            switch (type)
            {
                case LocalStorageType.JsonFile:
                    return new JsonToFileStorageService(new JsonToFileStorageService.Ctx());
            }

            Debug.LogError($"storage Service type {type} is not found");
            return null;
        }

        private ISceneLoader CreateSceneLoader(ResourceLoadType type)
        {
            switch (type)
            {
                case ResourceLoadType.FakeBoundles:
                    return new SceneLoader(new SceneLoader.Ctx
                    {
                        resourceLoader = _resourceLoader
                    });
            }

            Debug.LogError($"Resource Loader type {type} is not found");
            return null;
        }

        protected override void OnDestroy()
        {
            _resourceLoader.Dispose();
            _sceneLoader.Dispose();
            _game.Dispose();
            base.OnDestroy();
        }
    }
}