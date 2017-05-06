using Assets.Scripts.Entities.ApplicationObjects;
using Assets.Scripts.Enums;

namespace Assets.Scripts.Interfaces.Services
{
    public interface IMovementService
    {
        void SetMotion(DirectionEnum horizontalValue, DirectionEnum verticalValue, bool isRunning, BaseAppObject movingObj);
    }
}