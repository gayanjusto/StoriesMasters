
using Assets.Scripts.Enums;

namespace Assets.Scripts.Interfaces.Managers.Movement
{
    public interface IMovementManager : IBaseManager
    {
        bool IsMoving();
        DirectionEnum GetFacingDirection();
    }
}
