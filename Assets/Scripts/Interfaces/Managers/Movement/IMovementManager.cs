
using Assets.Scripts.Enums;
using System.Collections.Generic;

namespace Assets.Scripts.Interfaces.Managers.Movement
{
    public interface IMovementManager : IBaseManager
    {
        bool IsMoving();
        DirectionEnum GetFacingDirection();
    }
}
