using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.Interfaces.Controllers
{
    public interface IMovementController
    {
        void SetMovement(DirectionEnum horizontalValue, DirectionEnum verticalValue, bool isRunning, GameObject movingObj);
        void StopMoving(GameObject movingObj);
        void DisableMovement(GameObject movingObj);
        void EnableMovement(GameObject movingObj);
    }
}
