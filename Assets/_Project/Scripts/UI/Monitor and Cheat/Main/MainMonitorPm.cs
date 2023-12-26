using System.Collections.Generic;
using Containers.Data;
using Core;
using Logic.Profile;
using UniRx;
using UnityEngine;

namespace Logic.Idle.Monitors
{
    
    // TODO: нужен родительский объект для объектов.
    // TODO: Сейчас каждый - отельное окно, но не отдельная панель, нужна возможность переключаться как между вкладками
    // TODO: Выделить общие классы для каждого MonitorPm и MonitorView, этот класс переименовать
    public class MainMonitorPm : BaseDisposable
    {
        public struct Ctx
        {
            public Transform uiParent;
            public IResourceLoader resourceLoader;
            public GameConfig gameConfig;
            public IReadOnlyProfile profile;
            public ReactiveProperty<bool> isOpen;
        }

        private readonly Ctx _ctx;
        private const string VIEW_PREFAB_NAME = "MainMonitorView";
        private MainMonitorView _view;
        private Dictionary<MonitorType, IMonitor> _monitors;

        public MainMonitorPm(Ctx ctx)
        {
            _ctx = ctx;
            _monitors = new Dictionary<MonitorType, IMonitor>();
            
            AddDispose(_ctx.resourceLoader.LoadPrefab("fakebundles", VIEW_PREFAB_NAME, OnPrefabLoaded));
            AddDispose(_ctx.isOpen.Subscribe(OnMainMonitorOpen));
        }

        private void OnPrefabLoaded(GameObject prefab)
        {
            _view = GameObject.Instantiate(prefab, _ctx.uiParent).GetComponent<MainMonitorView>();
            
            _view.Init(new MainMonitorView.Ctx
            {
                viewDisposable = AddDispose(new CompositeDisposable()),
                close = Close,
                openMonitor = OpenMonitor
            });
            
            _view.Close();
        }

        private void OnMainMonitorOpen(bool isOpen)
        {
            if(isOpen)
                _view.Open();
            else
                _view.Close();
        }

        private void OpenMonitor(MonitorType monitorType)
        {
            IMonitor monitor;
            if (!_monitors.TryGetValue(monitorType, out monitor))
                monitor = _monitors[monitorType] = CreateMonitor(monitorType);
                
            monitor.Open();
            Close();
        }

        private IMonitor CreateMonitor(MonitorType monitorType)
        {
            switch (monitorType)
            {
                case MonitorType.Profile:
                    return CreateProfileMonitor();
                case MonitorType.Workers:
                    return CreateWorkersMonitor();
                case MonitorType.Buildings:
                    return CreateBuildingsMonitor();
                case MonitorType.BuildProgress:
                    return CreateBuildProgressMonitor();
                case MonitorType.Cheats:
                    return CreateCheatsMonitor();
            }

            return null;
        }

        private IMonitor CreateProfileMonitor()
        {
            return new ProfileMonitorPm(new ProfileMonitorPm.Ctx
            {
                resourceLoader = _ctx.resourceLoader,
                uiParent = _ctx.uiParent,
                back = () => OnMonitorBackClick(MonitorType.Profile),
            });
        }
        
        private IMonitor CreateWorkersMonitor()
        {
            return new WorkerMonitorPm(new WorkerMonitorPm.Ctx
            {
                uiParent = _ctx.uiParent,
                resourceLoader = _ctx.resourceLoader,
                back = () => OnMonitorBackClick(MonitorType.Workers),
                workers = _ctx.profile.Workers,
                currentEffectiencyLevel = _ctx.profile.CurrentEffectiencyLevel,
                currentSpeedLevel = _ctx.profile.CurrentTimeSpeedLevel,
                moneys = _ctx.profile.Moneys,
                currentMergeLevel = _ctx.profile.CurrentMergeLevel,
                currentAddWorkerLevel = _ctx.profile.CurrentAddWorkerLevel
            });
        }

        private IMonitor CreateBuildingsMonitor()
        {
            return new BuildingsMonitorPm(new BuildingsMonitorPm.Ctx
            {
                resourceLoader = _ctx.resourceLoader,
                uiParent = _ctx.uiParent,
                back = () => OnMonitorBackClick(MonitorType.Buildings),
            });
        }

        private IMonitor CreateBuildProgressMonitor()
        {
            return new BuildingProgressMonitorPm(new BuildingProgressMonitorPm.Ctx
            {
                resourceLoader = _ctx.resourceLoader,
                uiParent = _ctx.uiParent,
                back = () => OnMonitorBackClick(MonitorType.BuildProgress),
            });
        }

        private IMonitor CreateCheatsMonitor()
        {
            return new CheatsMonitorPm(new CheatsMonitorPm.Ctx
            {
                resourceLoader = _ctx.resourceLoader,
                uiParent = _ctx.uiParent,
                back = () => OnMonitorBackClick(MonitorType.Cheats),
            });
        }

        private void OnMonitorBackClick(MonitorType monitorType)
        {
            Open();
            _monitors[monitorType].Close();
        }
        
        private void Close()
        {
            _ctx.isOpen.Value = false;
            _view.Close();
        }

        private void Open()
        {
            _view.Open();
        }
    }
}

public enum MonitorType
{
    Profile,
    Workers,
    Buildings,
    BuildProgress,
    Cheats
}