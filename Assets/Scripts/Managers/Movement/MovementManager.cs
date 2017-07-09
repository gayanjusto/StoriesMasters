using System;
using Assets.Scripts.Interfaces.Managers.Movement;

namespace Assets.Scripts.Managers.Movement
{
    public class MovementManager : BaseMonoBehaviour, IMovementManager
    {
        public bool _isRunning;
        public bool _isMoving;
        public bool _movementLocked;
        public bool _objectFroze;

        public bool GetIsMoving()
        {
            return _isMoving;
        }

        public void SetIsMoving(bool isMoving)
        {
            if (!_movementLocked)
            {
                _isMoving = isMoving;
            }
        }

        public bool GetLockedMovement()
        {
            return _movementLocked;
        }

        public void SetLockedMovement(bool lockMovement)
        {
            _movementLocked = lockMovement;
        }

        public bool GetIsRunning()
        {
            return _isRunning;
        }

        public void SetIsRunning(bool isRunning)
        {
            _isRunning = isRunning;
        }

        public bool GetObjectFrozen()
        {
            return _objectFroze;
        }

        public void SetObjectFrozen(bool isFrozen)
        {
            _objectFroze = isFrozen;
        }
    }
}
