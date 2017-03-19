using Assets.Scripts.Entities.ApplicationObjects;
using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.Interfaces.Services
{
    public interface IMovementSpeedService
    {
        void SetHorizontalSpeed(DirectionEnum horizontalValue, BaseAppObject movingObj, bool isRunning);

        void SetVerticalSpeed(DirectionEnum verticalValue, BaseAppObject movingObj, bool isRunning);

        void SetDiagonalSpeed(DirectionEnum horizontalValue, DirectionEnum verticalValue, BaseAppObject movingObj, bool isRunning);
    }
}
