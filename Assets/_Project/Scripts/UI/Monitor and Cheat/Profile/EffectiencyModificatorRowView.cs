using Core;
using TMPro;
using UniRx;
using UnityEngine;

namespace Logic.Idle.Monitors
{
    public class EffectiencyModificatorRowView : ModificatorBaseRowView
    {
        [SerializeField] private TMP_Text _incomeMoneysText;
        [SerializeField] private TMP_Text _incomeWorkText;
        
        public struct Ctx
        {
            public float incomeMoneys;
            public float incomeWork;
        }

        private Ctx _ctx;

        public void Init(BaseCtx baseCtx, Ctx ctx)
        {
            BaseInit(baseCtx);
            _ctx = ctx;

            _incomeMoneysText.text = _ctx.incomeMoneys.ToString();
            _incomeWorkText.text = _ctx.incomeWork.ToString();
        }
    }
}

