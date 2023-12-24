using Core;
using UnityEngine;

namespace UI
{
    public class UpgradeButtonsView : BaseMonobehaviour
    {
        [SerializeField] private UpgradeButtonView _effectiencyUpButton;
        [SerializeField] private UpgradeButtonView _timeSpeedUpButton;
        [SerializeField] private UpgradeButtonView _addWorkerButton;
        [SerializeField] private UpgradeButtonView _mergeButton;
        
        public struct Ctx
        {
            
        }

        private Ctx _ctx;

        public UpgradeButtonView EffectiencyUpButton => _effectiencyUpButton;

        public UpgradeButtonView TimeSpeedUpButton => _timeSpeedUpButton;

        public UpgradeButtonView AddWorkerButton => _addWorkerButton;

        public UpgradeButtonView MergeButton => _mergeButton;

        public void Init(Ctx ctx)
        {
            _ctx = ctx;
        }
    }
}

