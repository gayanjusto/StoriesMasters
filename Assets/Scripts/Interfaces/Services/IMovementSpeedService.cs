using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.Interfaces.Services
{
    public interface IMovementSpeedService
    {
        void SetHorizontalSpeed(DirectionEnum horizontalValue, GameObject movingObj, bool isRunning);

        void SetVerticalSpeed(DirectionEnum verticalValue, GameObject movingObj, bool isRunning);

        void SetDiagonalSpeed(DirectionEnum horizontalValue, DirectionEnum verticalValue, GameObject movingObj, bool isRunning);
    }
}
