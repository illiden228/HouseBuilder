using Core;
using Logic.Idle.Workers;
using UniRx;
using UnityEngine;

namespace Logic.Idle.Monitors
{
    public class WorkerMonitorRowPm : BaseDisposable
    {
        public struct Ctx
        {
            public Transform uiParent;
            public IResourceLoader resourceLoader;
            public WorkerModel model;
        }

        private readonly Ctx _ctx;
        private const string ROW_PREFAB_NAME = "WorkerMonitorRowView";
        private WorkerMonitorRowView _view;

        public WorkerMonitorRowPm(Ctx ctx)
        {
            _ctx = ctx;
            
            AddDispose(_ctx.resourceLoader.LoadPrefab("fakebundles", ROW_PREFAB_NAME, OnPrefabLoaded));
        }

        private void OnPrefabLoaded(GameObject prefab)
        {
            _view = GameObject.Instantiate(prefab, _ctx.uiParent).GetComponent<WorkerMonitorRowView>();
            
            _view.Init(new WorkerMonitorRowView.Ctx
            {
                viewDisposable = AddDispose(new CompositeDisposable()),
                grade = _ctx.model.Grade,
                moneyIncome = _ctx.model.MoneyIncome,
                timeSpeed = _ctx.model.TimeSpeed,
                workIncome = _ctx.model.WorkIncome,
                currentIncomeTime = _ctx.model.CurrentIncomeTime
            });
        }
    }
}