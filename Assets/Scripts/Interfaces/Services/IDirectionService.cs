using Assets.Scripts.Entities.ApplicationObjects;
using Assets.Scripts.Enums;
using System.Collections.Generic;
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
        DirectionEnum GetHorizontalDirectionByValue(int value);
        DirectionEnum GetVerticalDirectionByValue(int value);

        /// <summary>
        /// /Returns neighboring direction from each side
        /// </summary>
        /// <param name="facingDirection"></param>
        /// <returns></returns>
        DirectionEnum[] GetNeighborDirections(GameAppObject movingObj);
     
        /// <summary>
        /// Returns the opposite direction of a given one
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        DirectionEnum GetOppositeDirection(DirectionEnum direction);
    }
}
