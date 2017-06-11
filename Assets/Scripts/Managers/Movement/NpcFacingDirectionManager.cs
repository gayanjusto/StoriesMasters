using System;
using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces.Managers.Movement;
using UnityEngine;

namespace Assets.Scripts.Managers.Movement
{
    public class NpcFacingDirectionManager : BaseMonoBehaviour, INpcFacingDirectionManager, IFacingDirection
    {
        public Transform _currentFacingTarget;
        public float _horizontalOffset = .5f;
        public float _verticalOffset = .5f;
        public DirectionEnum _facingDirection;

        public void SetCurrentFacingTarget(Transform target)
        {
            _currentFacingTarget = target;
        }

        private void Update()
        {
            if(_currentFacingTarget != null)
            {
                SetFacingDirectionForTarget();
            }
          
        }

        void SetFacingDirectionForTarget()
        {
            //Check Left Side
            if (TargetFarLeftFromTheObject())
            {
                if (TargetFarUpFromTheObject())
                {
                    _facingDirection = DirectionEnum.UpLeft;
                    return;
                }

                if (TargetFarDownFromTheObject())
                {
                    _facingDirection = DirectionEnum.DownLeft;
                    return;
                }

                _facingDirection = DirectionEnum.Left;
                return;
            }


            //Check Right Side
            if (TargetFarRightFromTheObject())
            {
                if (TargetFarUpFromTheObject())
                {
                    _facingDirection = DirectionEnum.UpRight;
                    return;
                }

                if (TargetFarDownFromTheObject())
                {
                    _facingDirection = DirectionEnum.DownRight;
                    return;
                }

                _facingDirection = DirectionEnum.Right;
                return;
            }

            //Check Up side
            if (TargetFarUpFromTheObject())
            {
                if (TargetFarLeftFromTheObject())
                {
                    _facingDirection = DirectionEnum.UpLeft;
                    return;
                }

                if (TargetFarDownFromTheObject())
                {
                    _facingDirection = DirectionEnum.UpRight;
                    return;
                }

                _facingDirection = DirectionEnum.Up;
                return;
            }

            //Check Down side
            if (TargetFarDownFromTheObject())
            {
                if (TargetFarLeftFromTheObject())
                {
                    _facingDirection = DirectionEnum.DownLeft;
                    return;
                }

                if (TargetFarRightFromTheObject())
                {
                    _facingDirection = DirectionEnum.DownRight;
                    return;
                }

                _facingDirection = DirectionEnum.Down;
                return;
            }
        }

        bool TargetFarLeftFromTheObject()
        {
            return _currentFacingTarget.transform.position.x < (transform.position.x - _horizontalOffset);
        }

        bool TargetFarRightFromTheObject()
        {
            return _currentFacingTarget.position.x > (transform.position.x + _horizontalOffset);
        }

        bool TargetFarUpFromTheObject()
        {
            return _currentFacingTarget.position.z > (transform.position.z + _verticalOffset);
        }

        bool TargetFarDownFromTheObject()
        {
            return _currentFacingTarget.position.z < (transform.position.z - _horizontalOffset);
        }

        public DirectionEnum GetFacingDirection()
        {
            return _facingDirection;
        }
    }
}
