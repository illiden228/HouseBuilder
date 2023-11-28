﻿using Containers;
using Core;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.Idle.Monitor_and_Cheat
{
    public class WorkerMonitorRowView : BaseMonobehaviour
    {
        [SerializeField] private TMP_Text _timeSpeedText;
        [SerializeField] private TMP_Text _moneyIncomeText;
        [SerializeField] private TMP_Text _workIncomeText;
        [SerializeField] private TMP_Text _gradeText;
        [SerializeField] private TMP_Text _currentTimeIncomeText;
        [SerializeField] private TMP_Text _progressIncomeText;
        [SerializeField] private Slider _sliderIncomeProgress;
        public struct Ctx
        {
            public CompositeDisposable viewDisposable;
            public IReadOnlyReactiveProperty<float> timeSpeed;
            public IReadOnlyReactiveProperty<int> moneyIncome;
            public IReadOnlyReactiveProperty<int> workIncome;
            public IReadOnlyReactiveProperty<Grade> grade;
            public IReadOnlyReactiveProperty<float> currentIncomeTime;
        }

        private Ctx _ctx;

        public void Init(Ctx ctx)
        {
            _ctx = ctx;

            SubscribeText(_ctx.timeSpeed, _timeSpeedText);
            SubscribeText(_ctx.moneyIncome, _moneyIncomeText);
            SubscribeText(_ctx.workIncome, _workIncomeText);
            SubscribeText(_ctx.grade, _gradeText);
            SubscribeText(_ctx.currentIncomeTime, _currentTimeIncomeText);
            SubscribeText(_ctx.timeSpeed, _progressIncomeText);

            _ctx.currentIncomeTime.Subscribe(value => ChangeSliderValue(value / _ctx.timeSpeed.Value)).AddTo(_ctx.viewDisposable);
        }

        private void SubscribeText<T>(IReadOnlyReactiveProperty<T> property, TMP_Text field)
        {
            property.Subscribe(value => field.text = value.ToString()).AddTo(_ctx.viewDisposable);
        }

        private void ChangeSliderValue(float value)
        {
            _sliderIncomeProgress.value = value;
        }
    }
}

