using Assets.Scripts.Entities.ApplicationObjects;
using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces.Services;
using Assets.Scripts.IoC;

namespace Assets.Scripts.Services
{
    public class MovementService : IMovementService
    {
        private readonly IMovementSpeedService _movementSpeedService;

        public MovementService()
        {
            _movementSpeedService = IoCContainer.GetImplementation<IMovementSpeedService>();
        }

        public void SetMotion(DirectionEnum horizontalValue, DirectionEnum verticalValue, bool isRunning, BaseAppObject movingObj)
        {
            if (isRunning)
            {
                movingObj.DecreaseSteamina();
            }

            if (horizontalValue != DirectionEnum.None && verticalValue != DirectionEnum.None)
            {
                _movementSpeedService.SetDiagonalSpeed(horizontalValue, verticalValue, movingObj, isRunning);
                return;
            }

            if (horizontalValue != DirectionEnum.None)
            {
                _movementSpeedService.SetHorizontalSpeed(horizontalValue, movingObj, isRunning);
                return;
            }

            if (verticalValue != DirectionEnum.None)
            {
                _movementSpeedService.SetVerticalSpeed(verticalValue, movingObj, isRunning);
                return;
            }

        }
    }
}
