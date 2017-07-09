using Assets.Scripts.Entities.ApplicationObjects;
using Assets.Scripts.Enums;
using Assets.Scripts.Factories.Services.Movement;
using Assets.Scripts.Interfaces.Managers;
using Assets.Scripts.Interfaces.Managers.Movement;
using Assets.Scripts.Interfaces.Services;

namespace Assets.Scripts.Services
{
    public class PlayerMovementService : IPlayerMovementService
    {
        private readonly IMotionService _motionService;
        private readonly IDirectionService _directionService;

        public PlayerMovementService()
        {
            _motionService = MotionServiceFactory.GetInstance();
            _directionService = DirectionServiceFactory.GetInstance();
        }

        public void SetPlayerInMotion(int horizontalValue, int verticalValue, GameAppObject movingObject)
        {
            //Get facing direction from inputs
            DirectionEnum horizontalDirection = _directionService.GetHorizontalDirectionByValue(horizontalValue);
            DirectionEnum verticalDirection = _directionService.GetVerticalDirectionByValue(verticalValue);

            DirectionEnum facingDirection = _directionService.GetFacingDirection(horizontalDirection, verticalDirection);
           
            //Set facing direction for direction manager
            movingObject.gameObject.GetComponent<IDirectionManager>().SetFacingDirection(facingDirection);

            //Get object MovementManager
            IMovementManager objMovManager = movingObject.gameObject.GetComponent<IMovementManager>();
            //Get isRunning from MovementManager
            bool isRunning = objMovManager.GetIsRunning();

            //Only set speed if Player can move. It might be able to change directions, but not move.
            if (!objMovManager.GetLockedMovement())
            {
                //Set player speed
                if (horizontalDirection != DirectionEnum.None && verticalDirection != DirectionEnum.None)
                {
                    _motionService.SetDiagonalSpeed(horizontalDirection, verticalDirection, movingObject, isRunning);
                    return;
                }

                if (horizontalDirection != DirectionEnum.None)
                {
                    _motionService.SetHorizontalSpeed(horizontalDirection, movingObject, isRunning);
                    return;
                }

                if (verticalDirection != DirectionEnum.None)
                {
                    _motionService.SetVerticalSpeed(verticalDirection, movingObject, isRunning);
                    return;
                }
            }
        }
    }
}
