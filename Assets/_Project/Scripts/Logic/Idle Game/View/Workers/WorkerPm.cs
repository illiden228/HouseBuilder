using System;
using System.Collections.Generic;
using Core;
using Tools.Extensions;
using UnityEngine;
using UniRx;

namespace Logic.Idle.Workers
{
    public class WorkerPm : BaseDisposable
    {
        public struct Ctx
        {
            public WorkerModel model;
            public WorkerView view;
            public List<WorkerMovePoint> path;
        }

        private readonly Ctx _ctx;
        private float _speed;
        private Transform _viewTransform;
        private bool _isCarry;
        private float _distance;
        private int _currentPathIndex;
        private float time;

        public WorkerPm(Ctx ctx)
        {
            _ctx = ctx;

            _viewTransform = _ctx.view.transform;
            _distance = CalcPathDistance();
            
            AddDispose(_ctx.model.TimeSpeed.Subscribe(currentTime =>
            {
                if(currentTime <= 0)
                    return;
                
                _speed = _distance / currentTime;
            }));

            AddDispose(ReactiveExtensions.StartUpdate(UpdatePosition));
        }

        private float CalcPathDistance()
        {
            float sqrDistance = 0;
            Transform currentTarget = _viewTransform;
            for (int i = 0; i < _ctx.path.Count; i++)
            {
                Transform nextTarget = _ctx.path[i].Point;
                // sqrDistance += (nextTarget.position - currentTarget.position).sqrMagnitude;
                sqrDistance += Vector3.Distance(nextTarget.position, currentTarget.position);
                    
                currentTarget = nextTarget;
            }

            return sqrDistance; //Mathf.Sqrt(sqrDistance);
        }
        
        private void UpdatePosition() // TODO: почему то не доходит за нужное время!!
        {
            if(_currentPathIndex < 0)
                return;
            
            Vector3 direction = _ctx.path[_currentPathIndex].Point.position - _viewTransform.position;
            if (direction.sqrMagnitude > 0.01)
            {
                float delta = _speed * Time.deltaTime;
                _speed = CalcPathDistance() / _ctx.model.TimeSpeed.Value; // нужно ли?
                _viewTransform.position = Vector3.MoveTowards(_viewTransform.position, _ctx.path[_currentPathIndex].Point.position, delta);
                _viewTransform.forward = direction;
            }
            else
            {
                TargetReached();
            }

            time += Time.deltaTime;
        }

        private void TargetReached()
        {
            WorkerMovePoint movePoint = _ctx.path[_currentPathIndex];
            if (_currentPathIndex == _ctx.path.Count - 1)
            {
                _currentPathIndex = 0;
                Debug.Log(time.ToString());
                time = 0;
                _distance = CalcPathDistance();
                _speed = _distance / _ctx.model.TimeSpeed.Value; // нужно ли?
            }
            else
                _currentPathIndex++;
            
            if(movePoint.AddBag)
                _ctx.view.CarryAnimation();
            
            if(movePoint.RemoveBag)
                _ctx.view.MoveAnimation();
            
            _distance = CalcPathDistance();
            _speed = _distance / _ctx.model.TimeSpeed.Value; // нужно ли?
        }
    }
}