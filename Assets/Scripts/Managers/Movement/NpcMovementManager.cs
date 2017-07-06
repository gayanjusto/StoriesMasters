using System;
using System.Collections.Generic;
using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces.Managers.Movement;
using UnityEngine;
using UnityEngine.AI;
using Assets.Scripts.Interfaces.Managers.Combat;

namespace Assets.Scripts.Managers.Movement
{
    public class NpcMovementManager : BaseMonoBehaviour, IMovementManager, INpcMovementManager, IFacingDirection
    {
        public Transform _target;
        public Transform _directionRotator;
        IAttackStatusManager _attackStatusManager;

        NavMeshAgent _agent;
        float _facingAngle;

        public Vector3 _previousPosition;

        bool _canOnlyChangeDirection;

        #region PRIVATE METHODS
        void Start()
        {
            _previousPosition = transform.position;
            _directionRotator = transform.Find("DirectionRotator");
            _agent = GetComponent<NavMeshAgent>();
            _attackStatusManager = GetComponent<IAttackStatusManager>();

            //target = GameObject.Find("Player").transform;
            //Set navMeshAgentSpeed
            //agent.destination = GameObject.Find("Player").transform.position;//target.position;
        }

        void Update()
        {
            if (TargetIsAvailable())
            {
                _directionRotator.LookAt(_target.transform.position);
                if (GetDistanceFromTarget() > 0.5f)
                {
                    _agent.destination = _target.position;
                }
            }
            else
            {
                Disable();
            }

        }

        bool TargetIsAvailable()
        {
            return _target != null && _target.gameObject.activeSelf;
        }
        #endregion

        public DirectionEnum GetFacingDirection()
        {
            _facingAngle = _directionRotator.localEulerAngles.y;
            _facingAngle = (_facingAngle > 180) ? _facingAngle - 360 : _facingAngle;


            if (_facingAngle < 0)
            {
                if (_facingAngle <= 0 && _facingAngle >= -22.5f)
                {
                    return DirectionEnum.Up;
                }

                if (_facingAngle <= -22.5f && _facingAngle >= -67.5f)
                {
                    return DirectionEnum.UpLeft;
                }

                if (_facingAngle <= -67.5f && _facingAngle >= -112.5f)
                {
                    return DirectionEnum.Left;
                }

                if (_facingAngle <= -112.5f && _facingAngle >= -157f)
                {
                    return DirectionEnum.DownLeft;
                }

                if (_facingAngle <= -157f && _facingAngle >= -180f)
                {
                    return DirectionEnum.Down;
                }
            }
            else
            {
                if (_facingAngle >= 0 && _facingAngle <= 22.5f)
                {
                    return DirectionEnum.Up;
                }

                if (_facingAngle >= 22.5f && _facingAngle <= 67.5f)
                {
                    return DirectionEnum.UpRight;
                }

                if (_facingAngle >= 67.5f && _facingAngle <= 112.5f)
                {
                    return DirectionEnum.Right;
                }

                if (_facingAngle >= 112.5f && _facingAngle <= 157f)
                {
                    return DirectionEnum.DownRight;
                }

                if (_facingAngle >= 157f && _facingAngle <= 180f)
                {
                    return DirectionEnum.Down;
                }
            }

            return DirectionEnum.None;
        }

        public bool IsMoving()
        {
            if (transform.position != _previousPosition && !_attackStatusManager.IsAttacking())
            {
                _previousPosition = transform.position;
                return true;
            }

            return false;
        }

        public float GetDistanceFromTarget()
        {
            return Vector3.Distance(transform.position, _target.position);
        }

        public void SetTarget(GameObject target)
        {
            this._target = target.transform;
        }

        public void SetCanChangeDirectionButNotMove(bool canOnlyChangeDirection)
        {
            _canOnlyChangeDirection = canOnlyChangeDirection;
        }
    }
}
