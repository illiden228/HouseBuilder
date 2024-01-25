using Core;
using System;
using TMPro;
using Tools.Extensions;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class FloorHUDView : BaseMonobehaviour
{
    public struct Ctx
    {
        public CompositeDisposable viewDisposables;        
        public IReadOnlyReactiveProperty<int> availableFloors;
        public IReadOnlyReactiveProperty<int> builtFloors;
        public ReactiveEvent releaseFloorButton;
        public ReactiveEvent<int> onFloorPlaced;
    }

    [SerializeField] private Button _releaseFloorButton;
    [SerializeField] private Button _loadSceneResourcesButton;
    [SerializeField] private TextMeshProUGUI _availableFloorsText;
    [SerializeField] private TextMeshProUGUI _availableFloorsHeaderText;
    [SerializeField] private TextMeshProUGUI _builtFloorsText;

    private Ctx _ctx;

    public void Init(Ctx ctx)
    {
        _ctx = ctx;

        _releaseFloorButton.OnClickAsObservable()
            .Subscribe(_ => ButtonClick())
            .AddTo(_ctx.viewDisposables);

        _ctx.onFloorPlaced.SubscribeOnceWithSkip((value) => { _availableFloorsText.text = value.ToString(); })
            .AddTo(_ctx.viewDisposables);

        //_loadSceneResourcesButton.OnClickAsObservable()
        //    .Subscribe(_ => _ctx.loadSceneButtonClick?.Invoke())
        //    .AddTo(_ctx.viewDisposables);
        //_ctx.availableFloors.Subscribe(count =>
        //{
        //    _availableFloorsText.text = count.ToString();
        //}).AddTo(_ctx.viewDisposables);

        //_ctx.builtFloors.Subscribe(count =>
        //{
        //    _availableFloorsText.text = count.ToString();
        //}).AddTo(_ctx.viewDisposables);
    }

    private void ButtonClick()
    {
        _ctx.releaseFloorButton.Notify();
    }
}
