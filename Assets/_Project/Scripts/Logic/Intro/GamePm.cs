using System;
using System.Linq;
using Core;
using SceneLogic;
using Tools;
using UniRx;
using Unity.VisualScripting;
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
            public ReactiveProperty<Scenes> currentScene;
            public UserDataLoader userDataLoader;
        }

        private readonly Ctx _ctx;

        public GamePm(Ctx ctx)
        {
            _ctx = ctx;

            AddDispose(_ctx.currentScene.Subscribe(OnSceneChanged));
        }

        private void OnSceneChanged(Scenes scene)
        {
            _ctx.sceneLoader.LoadScene((int) scene, OnSceneUnload, () => OnSceneLoaded(scene));
        }
        
        private void OnSceneUnload()
        {
            
        }

        private void OnSceneLoaded(Scenes scene)
        {
            switch (scene)
            {
                case Scenes.IdleScene:
                    CreateIdleScene();
                    return;
                case Scenes.FloorScene:
                    CreateFloorScene();
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void CreateIdleScene()
        {
            SceneContextView sceneContext = FindContext(Scenes.IdleScene);
            if (sceneContext is not IdleContextView idleContextView)
            {
                Debug.LogError("IdleContextView was null");
                return;
            }

            IdleScenePm.Ctx idleSceneCtx =new IdleScenePm.Ctx
            {
                currentScene = _ctx.currentScene,
                userDataLoader = _ctx.userDataLoader,
                sceneContext = idleContextView
            };
            AddDispose(new IdleScenePm(idleSceneCtx));
        }
        
        private void CreateFloorScene()
        {
            SceneContextView sceneContext = FindContext(Scenes.FloorScene);
            if (sceneContext is not FloorsContextView floorsContextView)
            {
                Debug.LogError("FloorsContextView was null");
                return;
            }
            
            FloorsScenePm.Ctx floorsSceneCtx = new FloorsScenePm.Ctx
            {
                currentScene = _ctx.currentScene,
                userDataLoader = _ctx.userDataLoader,
                sceneContext = floorsContextView
            };
            AddDispose(new FloorsScenePm(floorsSceneCtx));
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