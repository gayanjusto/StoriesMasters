using Assets.Scripts.Entities.ApplicationObjects;

namespace Assets.Scripts.Interfaces.Services
{
    public interface IPlayerMovementService
    {
        void SetPlayerInMotion(int horizontalValue, int verticalValue, GameAppObject movingObject);
    }
}
