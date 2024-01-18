using System;
using System.Linq;
using Containers.Data;
using Core;
using Logic.Idle.Workers;
using Logic.Profile;
using SceneLogic;
using UniRx;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Logic.Intro
{
    public class GamePm : BaseDisposable
    {
        public struct Ctx
        {
            public ISceneLoader sceneLoader;
            public IResourceLoader resourceLoader;
            public IReadOnlyProfile profile;
            //public ReactiveProperty<Scenes> currentScene;
            public UserDataLoader userDataLoader;
            public GameConfig gameConfig;
        }

        private readonly Ctx _ctx;
        private IDisposable _currentScene;

        public GamePm(Ctx ctx)
        {
            _ctx = ctx;

            AddDispose(_ctx.profile.CurrentScene.Subscribe(OnSceneChanged));
        }

        private void OnSceneChanged(Scenes scene)
        {
            _ctx.sceneLoader.LoadScene((int) scene, OnSceneUnload, () => OnSceneLoaded(scene));
        }
        
        private void OnSceneUnload()
        {
            _currentScene?.Dispose();
        }

        private void OnSceneLoaded(Scenes scene)
        {
            switch (scene)
            {
                case Scenes.IdleScene:
                    _currentScene = CreateIdleScene();
                    return;
                case Scenes.FloorScene:
                    _currentScene = CreateFloorScene();
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private IDisposable CreateIdleScene()
        {
            SceneContextView sceneContext = FindContext(Scenes.IdleScene);
            if (sceneContext is not IdleContextView idleContextView)
            {
                Debug.LogError("IdleContextView was null");
                return null;
            }

            IdleScenePm.Ctx idleSceneCtx =new IdleScenePm.Ctx
            {
                userDataLoader = _ctx.userDataLoader,
                sceneContext = idleContextView,
                resourceLoader = _ctx.resourceLoader,
                profile = _ctx.profile,
                gameConfig = _ctx.gameConfig,
                moveToFloorScene = () => _ctx.profile.CurrentScene.Value = Scenes.FloorScene,
            };
            return new IdleScenePm(idleSceneCtx);
        }
        
        private IDisposable CreateFloorScene()
        {
            SceneContextView sceneContext = FindContext(Scenes.FloorScene);
            if (sceneContext is not FloorsContextView floorsContextView)
            {
                Debug.LogError("FloorsContextView was null");
                return null;
            }
            
            FloorsScenePm.Ctx floorsSceneCtx = new FloorsScenePm.Ctx
            {
                currentScene = _ctx.profile.CurrentScene,
                userDataLoader = _ctx.userDataLoader,
                sceneContext = floorsContextView,
                building = _ctx.profile.CurrentBuildingFloorProgress.Value.Building,
                floorsProgress = _ctx.profile.CurrentBuildingFloorProgress.Value.FloorsProgress,
                resourceLoader = _ctx.resourceLoader,
                onBackToIdleScene = () => _ctx.profile.CurrentScene.Value = Scenes.IdleScene,
            };
            return new FloorsScenePm(floorsSceneCtx);
        }

        private SceneContextView FindContext(Scenes scene)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneAt(0));
            Debug.Log($"Scene is {SceneManager.GetActiveScene().name} {SceneManager.GetActiveScene().path}");
            SceneContextView[] sceneContexts = GameObject.FindObjectsOfType<SceneContextView>();
            SceneContextView sceneContext = sceneContexts.FirstOrDefault(ctx =>
            {
                return scene switch
                {
                    Scenes.IdleScene => ctx is IdleContextView,
                    Scenes.FloorScene => ctx is FloorsContextView,
                    _ => false
                };
            });
            return sceneContext;
        }
    }
}