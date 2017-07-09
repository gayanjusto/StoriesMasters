using Assets.Scripts.Enums;

namespace Assets.Scripts.Interfaces.Managers
{
    public interface IDirectionManager
    {
        void SetFacingDirection(DirectionEnum facingDirection);
        DirectionEnum GetFacingDirection();
    }
}
