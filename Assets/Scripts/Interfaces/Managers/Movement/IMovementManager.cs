namespace Assets.Scripts.Interfaces.Managers.Movement
{
    public interface IMovementManager : IBaseMonoBehaviour
    {
        bool GetIsMoving();
        void SetIsMoving(bool isMoving);

        bool GetLockedMovement();
        void SetLockedMovement(bool lockedMovement);

        bool GetIsRunning();
        void SetIsRunning(bool isRunning);

        bool GetObjectFrozen();
        void SetObjectFrozen(bool isFrozen);
    }
}
