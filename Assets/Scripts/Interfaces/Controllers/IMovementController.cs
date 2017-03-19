using Assets.Scripts.Entities.ApplicationObjects;
using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.Interfaces.Controllers
{
    public interface IMovementController
    {
        void SetMovement(DirectionEnum horizontalValue, DirectionEnum verticalValue, bool isRunning, BaseAppObject movingObj);
        void StopMoving(BaseAppObject movingObj);
        void DisableMovement(GameObject movingObj);
        void EnableMovement(GameObject movingObj);
        DirectionEnum[] GetNeighboringDirections(BaseAppObject movingObj);
    }
}
