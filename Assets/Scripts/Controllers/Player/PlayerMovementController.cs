using Assets.Scripts.Factories.Services.Movement;
using Assets.Scripts.Interfaces.Managers.Movement;
using Assets.Scripts.Interfaces.Services;
using UnityEngine;
using Assets.Scripts.MonoBehaviours;
using Assets.Scripts.Interfaces.Tests.Controllers;

namespace Assets.Scripts.Controllers.Player
{
    public class PlayerMovementController : AppObjectMonoBehaviour, IPlayerMovementInput_Testable
    {
        private int _horizontalInputValue;
        private int _verticalInputValue;

        public bool isRunning;
        public bool _canOnlyChangeDirection;

        private IMovementManager _movementManager;

        private IPlayerMovementService _playerMovementService;

        public override void Start()
        {
            base.Start();

            _movementManager = GetComponent<IMovementManager>();
            _playerMovementService = PlayerMovementServiceFactory.GetInstance();
        }

        public void FixedUpdate()
        {

            CheckUpDownMovementInput();
            CheckLeftRightMovementInput();

            CheckRunningInput();

            //If player is not frozen, it can move

            if (!_movementManager.GetObjectFrozen() && AnyMovementInput())
            {
                MovePlayer();
            }
            else
            {
                _movementManager.SetIsMoving(false);
            }
        }

        public void MovePlayer()
        {
            _movementManager.SetIsMoving(true);
            _playerMovementService.SetPlayerInMotion(_horizontalInputValue, _verticalInputValue, base.gameAppObject);
        }

        void CheckUpDownMovementInput()
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                _verticalInputValue = 1;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                _verticalInputValue = -1;
            }
            else
            {
                _verticalInputValue = 0;
            }
        }

        void CheckLeftRightMovementInput()
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                _horizontalInputValue = -1;
                return;
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                _horizontalInputValue = 1;
                return;
            }

            _horizontalInputValue = 0;
        }

        void CheckRunningInput()
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _movementManager.SetIsRunning(true);
            }
            else
            {
                _movementManager.SetIsRunning(false);
            }
        }

        public bool AnyMovementInput()
        {
            if (_canOnlyChangeDirection)
            {
                return false;
            }
            return _horizontalInputValue != 0 || _verticalInputValue != 0;
        }

        #region TESTS METHODS
#if DEBUG
        public int SetHorizontalMovementValue(KeyCode keyPress)
        {
            if (keyPress == KeyCode.LeftArrow)
            {
                _horizontalInputValue = -1;
            }
            else if (keyPress == KeyCode.RightArrow)
            {
                _horizontalInputValue = 1;
            }
            else
            {
                _horizontalInputValue = 0;
            }

            return _horizontalInputValue;
        }

        public int SetVerticalMovementValue(KeyCode keyPress)
        {
            if (keyPress == KeyCode.UpArrow)
            {
                _verticalInputValue = 1;
            }
            else if (keyPress == KeyCode.DownArrow)
            {
                _verticalInputValue = -1;
            }
            else
            {
                _verticalInputValue = 0;
            }

            return _verticalInputValue;
        }

        public void SetRunningValue(KeyCode keyPress)
        {
            if (keyPress == KeyCode.LeftShift)
            {
                _movementManager.SetIsRunning(true);
            }
            else
            {
                _movementManager.SetIsRunning(false);

            }
        }
#endif
        #endregion
    }
}
