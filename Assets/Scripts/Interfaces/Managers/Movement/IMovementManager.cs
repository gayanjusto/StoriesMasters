
using Assets.Scripts.Enums;
using System.Collections.Generic;

namespace Assets.Scripts.Interfaces.Managers.Movement
{
    public interface IMovementManager : IBaseMonoBehaviour
    {
        bool IsMoving();
        DirectionEnum GetFacingDirection();
        void SetCanChangeDirectionButNotMove(bool canOnlyChangeDirection);
    }
}
