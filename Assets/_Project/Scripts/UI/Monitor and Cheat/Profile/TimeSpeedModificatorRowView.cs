using Core;
using TMPro;
using UnityEngine;

namespace Logic.Idle.Monitors
{
    public class TimeSpeedModificatorRowView : BaseMonobehaviour
    {
        [SerializeField] private TMP_Text _timeSpeedText;
        
        public struct Ctx
        {
            
        }

        private Ctx _ctx;

        public void Init(Ctx ctx)
        {
            _ctx = ctx;
        }
    }
}

