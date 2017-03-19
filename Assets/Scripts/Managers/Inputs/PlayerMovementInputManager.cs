using System;
using System.Collections.Generic;
using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces.Controllers;
using Assets.Scripts.Interfaces.Managers.Movement;
using Assets.Scripts.Interfaces.Managers.Objects;
using Assets.Scripts.Interfaces.Services;
using Assets.Scripts.IoC;
using Assets.Scripts.Managers.Movement;
using UnityEngine;

namespace Assets.Scripts.Managers.Inputs
{
    public class PlayerMovementInputManager : BaseManager, IMovementManager
    {
        private int _hVal;
        private int _vVal;

        private IObjectManager _objectManager;
        private IDirectionService _directionService;
        protected IMovementController _movementController;

        public DirectionEnum _verticalDirection;
        public DirectionEnum _horizontalDirection;

        public DirectionEnum _verticalFacingDirection;
        public DirectionEnum _horizontalFacingDirection;
        public DirectionEnum _facingDirection;
        public bool isRunning;

        private void Start()
        {
            _objectManager = GetComponent<IObjectManager>();
            _directionService = IoCContainer.GetImplementation<IDirectionService>();
            _movementController = IoCContainer.GetImplementation<IMovementController>();
        }

        void FixedUpdate()
        {
            _facingDirection = GetFacingDirection();
            _vVal = CheckUpDownMovement();
            _hVal = CheckLeftRightMovement();
            CheckRunning();

            SetDirections();

            if (IsMoving())
            {
                _movementController.SetMovement(_horizontalDirection, _verticalDirection, isRunning, _objectManager.GetBaseAppObject());
            }
            else if(_objectManager.GetBaseAppObject().StaminaManager.IsEnabled())
            {
                _movementController.StopMoving(_objectManager.GetBaseAppObject());
            }
        }

        int CheckUpDownMovement()
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                return 1;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        int CheckLeftRightMovement()
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                return -1;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                return 1;
            }
            else
            {
                return 0;
            }

        }

        void CheckRunning()
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                isRunning = true;
            }
            else
            {
                isRunning = false;
            }
        }

        void SetDirections()
        {
            if (_hVal == 0 && _vVal == 0)
            {
                _horizontalDirection = DirectionEnum.None;
                _verticalDirection = DirectionEnum.None;

                return;
            }

            if (_hVal != 0 && _vVal != 0)
            {
                _horizontalDirection = _directionService.GetHorizontalDirectionByValue(_hVal);
                _verticalDirection = _directionService.GetVerticalDirectionByValue(_vVal);

                _horizontalFacingDirection = _horizontalDirection;
                _verticalFacingDirection = _verticalDirection;
            }

            if (_hVal != 0 && _vVal == 0)
            {
                _horizontalDirection = _directionService.GetHorizontalDirectionByValue(_hVal);
                _horizontalFacingDirection = _horizontalDirection;
                _verticalFacingDirection = DirectionEnum.None;
                _verticalDirection = DirectionEnum.None;
            }

            if (_hVal == 0 && _vVal != 0)
            {
                _verticalDirection = _directionService.GetVerticalDirectionByValue(_vVal);
                _verticalFacingDirection = _verticalDirection;
                _horizontalFacingDirection = DirectionEnum.None;
                _horizontalDirection = DirectionEnum.None;
            }
        }


        public bool IsMoving()
        {
            return _horizontalDirection != DirectionEnum.None || _verticalDirection != DirectionEnum.None;
        }

        public DirectionEnum GetFacingDirection()
        {
            if (_directionService != null)
            {
                return _directionService.GetFacingDirection(_horizontalFacingDirection, _verticalFacingDirection);
            }

            return DirectionEnum.None;
        }
    }
}
