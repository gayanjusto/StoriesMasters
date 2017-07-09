using Assets.Scripts.Entities.ApplicationObjects;
using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.Interfaces.Services
{
    public interface IMotionService
    {
        void SetHorizontalSpeed(DirectionEnum horizontalValue, GameAppObject movingObj, bool isRunning);

        void SetVerticalSpeed(DirectionEnum verticalValue, GameAppObject movingObj, bool isRunning);

        void SetDiagonalSpeed(DirectionEnum horizontalValue, DirectionEnum verticalValue, GameAppObject movingObj, bool isRunning);
    }
}
