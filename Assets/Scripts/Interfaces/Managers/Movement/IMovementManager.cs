namespace Assets.Scripts.Interfaces.Managers.Movement
{
    public interface IMovementManager : IBaseMonoBehaviour
    {
        bool IsMoving();
        void SetCanChangeDirectionButNotMove(bool canOnlyChangeDirection);
    }
}
