using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.Interfaces.Services
{
    public interface IDirectionService
    {
        Vector3 GetVector3ByDirections(DirectionEnum horizontalValue, DirectionEnum verticalValue);
        int GetAxisValueByHorizontalDirection(DirectionEnum horizontalValue);
        float GetAxisUnitValueByHorizontalDirection(DirectionEnum horizontalValue);
        int GetAxisValueByVerticalDirection(DirectionEnum verticalValue);
        float GetAxisUnitValueByVerticalDirection(DirectionEnum verticalValue);
        DirectionEnum GetFacingDirection(DirectionEnum horizontalValue, DirectionEnum verticalValue);
    }
}
