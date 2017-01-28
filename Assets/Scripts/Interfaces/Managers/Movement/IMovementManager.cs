
using Assets.Scripts.Enums;

namespace Assets.Scripts.Interfaces.Managers.Movement
{
    public interface IMovementManager : IBaseManager
    {
        bool IsMoving();
        DirectionEnum GetVerticalDirection();
        DirectionEnum GetHorizontalDirection();
        DirectionEnum GetVerticalFacingDirection();
        DirectionEnum GetHorizontalFacingDirection();
    }
}
