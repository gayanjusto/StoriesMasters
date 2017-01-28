using Assets.Scripts.Enums;
using Assets.Scripts.Managers.Movement;
using UnityEngine;

namespace Assets.Scripts.Managers.Inputs
{
    public class PlayerMovementInputManager : BaseMovementManager
    {
        private int _hVal;
        private int _vVal;

        public bool isRunning;

        void FixedUpdate()
        {
            _vVal= CheckUpDownMovement();
            _hVal = CheckLeftRightMovement();
            CheckRunning();

            SetDirections();

            if (IsMoving())
            {
                _movementController.SetMovement(_horizontalDirection, _verticalDirection, isRunning, gameObject);
            }
            else
            {
                _movementController.StopMoving(gameObject);
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
                _horizontalDirection = GetHorizontalValueByInput();
                _verticalDirection = GetVerticalValueByInput();

                _horizontalFacingDirection = _horizontalDirection;
                _verticalFacingDirection = _verticalDirection;
            }

            if(_hVal != 0 && _vVal == 0)
            {
                _horizontalDirection = GetHorizontalValueByInput();
                _horizontalFacingDirection = _horizontalDirection;
                _verticalFacingDirection = DirectionEnum.None;
                _verticalDirection = DirectionEnum.None;
            }

            if (_hVal == 0 && _vVal != 0)
            {
                _verticalDirection = GetVerticalValueByInput();
                _verticalFacingDirection = _verticalDirection;
                _horizontalFacingDirection = DirectionEnum.None;
                _horizontalDirection = DirectionEnum.None;
            }
        }


        DirectionEnum GetHorizontalValueByInput()
        {
            switch (_hVal)
            {
                case 1:
                return DirectionEnum.Right;
                case -1:
                return DirectionEnum.Left;
                default:
                return DirectionEnum.None;
            }
        }

        DirectionEnum GetVerticalValueByInput()
        {
            switch (_vVal)
            {
                case 1:
                return DirectionEnum.Up;
                case -1:
                return DirectionEnum.Down;
                default:
                return DirectionEnum.None;
            }
        }
    }
}
