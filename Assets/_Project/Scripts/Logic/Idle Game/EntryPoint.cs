using System.Data;
using Containers;
using Containers.Data;
using Core;
using Logic.Intro;
using Logic.Model;
using Logic.Profile;
using Tools.Extensions;
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
        [SerializeField] private GameConfig _gameConfig;

        private IResourceLoader _resourceLoader;
        private IStorageService _storageService;
        private ISceneLoader _sceneLoader;
        private UserDataLoader _userDataLoader;
        private DataLoader _dataLoader;
        private GamePm _game;
        private CoreIdleLogic _coreLogic;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            _resourceLoader = CreateResourceLoader(_resourceLoadType);
            _storageService = CreateStorageService(_localStorageType);
            _sceneLoader = CreateSceneLoader(_resourceLoadType);
            _userDataLoader = new UserDataLoader(_storageService); // TODO: изжил себя, нужен только для поддержания старого кода
            
            ProfileClient profile = new ProfileClient(new ProfileClient.Ctx { });

            DataLoader.Ctx dataLoaderCtx = new DataLoader.Ctx
            {
                storageService = _storageService,
                profile = profile,
                gameConfig = _gameConfig,
            };
            _dataLoader = new DataLoader(dataLoaderCtx);

            _coreLogic = new CoreIdleLogic(new CoreIdleLogic.Ctx
            {
                profile = profile,
                buildinReadyEvent = new ReactiveEvent<BuildingInfo>() // TODO: пока что заглушка
            });
            
            _game = new GamePm(new GamePm.Ctx
            {
                resourceLoader = _resourceLoader,
                sceneLoader = _sceneLoader,
                userDataLoader = _userDataLoader,
                profile = profile,
                workers = profile.Workers,
                gameConfig = _gameConfig
                //currentScene = new ReactiveProperty<Scenes>(Scenes.IdleScene)
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
            _dataLoader.Dispose();
            _coreLogic.Dispose();
            base.OnDestroy();
        }
    }
}