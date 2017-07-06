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
using Assets.Scripts.Interfaces.Managers.Combat;
using Assets.Scripts.Interfaces.Managers.Attributes;

namespace Assets.Scripts.Managers.Inputs
{
    public class PlayerMovementInputManager : BaseMonoBehaviour, IMovementManager, IFacingDirection
    {
        private int _horizontalInputValue;
        private int _verticalInputValue;

        private IObjectManager _objectManager;
        private IDirectionService _directionService;
        private ICombatManager _combatManager;
        private IMovementService _movementService;
        private IStaminaManager _staminaManager;


        public DirectionEnum _verticalDirection;
        public DirectionEnum _horizontalDirection;

        public DirectionEnum _verticalFacingDirection;
        public DirectionEnum _horizontalFacingDirection;
        public DirectionEnum _facingDirection;
        public bool isRunning;

        public bool _canOnlyChangeDirection;

        private void Start()
        {
            _objectManager = GetComponent<IObjectManager>();
            _combatManager = GetComponent<ICombatManager>();
            _staminaManager = GetComponent<IStaminaManager>();

            _directionService = IoCContainer.GetImplementation<IDirectionService>();
            _movementService = IoCContainer.GetImplementation<IMovementService>();

        }

        void FixedUpdate()
        {
            _facingDirection = GetFacingDirection();

            _verticalInputValue = CheckUpDownMovement();
            _horizontalInputValue = CheckLeftRightMovement();
            CheckRunning();

            SetDirections();

            //If is blocking can change direction, but not move
            if (IsMoving() && !_canOnlyChangeDirection)
            {
                _movementService.SetMotion(_horizontalDirection, _verticalDirection, isRunning, _objectManager.GetBaseAppObject());
            }
            else
            {
                //If it's not moving, should not lose stamina
                if (_staminaManager.IsEnabled())
                {
                    _staminaManager.SetDecreasingStamina(false);
                }
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
            if (_horizontalInputValue == 0 && _verticalInputValue == 0)
            {
                _horizontalDirection = DirectionEnum.None;
                _verticalDirection = DirectionEnum.None;

                return;
            }

            if (_horizontalInputValue != 0 && _verticalInputValue != 0)
            {
                _horizontalDirection = _directionService.GetHorizontalDirectionByValue(_horizontalInputValue);
                _verticalDirection = _directionService.GetVerticalDirectionByValue(_verticalInputValue);

                _horizontalFacingDirection = _horizontalDirection;
                _verticalFacingDirection = _verticalDirection;
            }

            if (_horizontalInputValue != 0 && _verticalInputValue == 0)
            {
                _horizontalDirection = _directionService.GetHorizontalDirectionByValue(_horizontalInputValue);
                _horizontalFacingDirection = _horizontalDirection;
                _verticalFacingDirection = DirectionEnum.None;
                _verticalDirection = DirectionEnum.None;
            }

            if (_horizontalInputValue == 0 && _verticalInputValue != 0)
            {
                _verticalDirection = _directionService.GetVerticalDirectionByValue(_verticalInputValue);
                _verticalFacingDirection = _verticalDirection;
                _horizontalFacingDirection = DirectionEnum.None;
                _horizontalDirection = DirectionEnum.None;
            }
        }


        public bool IsMoving()
        {
            if (_canOnlyChangeDirection)
            {
                return false;
            }
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

        public void SetCanChangeDirectionButNotMove(bool canOnlyChangeDirection)
        {
            _canOnlyChangeDirection = canOnlyChangeDirection;
        }
    }
}
