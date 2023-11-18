using Core;
using UniRx;
using UnityEngine;

namespace Logic.Idle.Workers
{
    public class WorkerView : BaseMonobehaviour
    {
        public struct Ctx
        {
            public CompositeDisposable viewDisposable;
        }
        
        [SerializeField] private Transform _bagPoint;
        [SerializeField] private Animator _animator;

        private Ctx _ctx;

        public Transform BagPoint => _bagPoint;
        public Animator Animator => _animator;

        public void Init(Ctx ctx)
        {
            _ctx = ctx;
            _bagPoint.gameObject.SetActive(false);
        }

        public void CarryAnimation()
        {
            _animator.SetTrigger(AnimatorNames.Carry);
            _bagPoint.gameObject.SetActive(true);
        }

        public void MoveAnimation()
        {
            _animator.SetTrigger(AnimatorNames.Move);
            _bagPoint.gameObject.SetActive(false);
        }
    }
}