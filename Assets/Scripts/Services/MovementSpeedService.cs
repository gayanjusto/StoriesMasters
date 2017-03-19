using Assets.Scripts.Entities.ApplicationObjects;
using Assets.Scripts.Entities.IntelligentBodies;
using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces.Factories;
using Assets.Scripts.Interfaces.Services;
using Assets.Scripts.IoC;
using System;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class MovementSpeedService : IMovementSpeedService
    {

        public void SetHorizontalSpeed(DirectionEnum horizontalValue, BaseAppObject movingObj, bool isRunning)
        {
            SetStraightLineSpeed(Vector3.right, movingObj.GameObject, GetHorizontalDirectionValue(horizontalValue), isRunning, movingObj.GetStraightLineSpeed);
        }

        public void SetVerticalSpeed(DirectionEnum verticalValue, BaseAppObject movingObj, bool isRunning)
        {
            SetStraightLineSpeed(Vector3.up, movingObj.GameObject, GetVerticalDirectionValue(verticalValue), isRunning, movingObj.GetStraightLineSpeed);
        }


        public void SetDiagonalSpeed(DirectionEnum horizontalValue, DirectionEnum verticalValue, BaseAppObject movingObj, bool isRunning)
        {
            //Horizontal
            SetStraightLineSpeed(Vector3.right, movingObj.GameObject, GetHorizontalDirectionValue(horizontalValue), isRunning, movingObj.GetDiagionalSpeed);

            //Vertical
            SetStraightLineSpeed(Vector3.up, movingObj.GameObject, GetVerticalDirectionValue(verticalValue), isRunning, movingObj.GetDiagionalSpeed);

        }

        private int GetHorizontalDirectionValue(DirectionEnum horizontalValue)
        {
            int directionValue = 0;
            if (horizontalValue == DirectionEnum.Right)
            {
                directionValue = 1;
            }
            else
            {
                directionValue = -1;
            }

            return directionValue;
        }

        private int GetVerticalDirectionValue(DirectionEnum verticalValue)
        {
            int directionValue = 0;
            if (verticalValue == DirectionEnum.Up)
            {
                directionValue = 1;
            }
            else
            {
                directionValue = -1;
            }

            return directionValue;
        }

        private void SetStraightLineSpeed(Vector3 direction, GameObject movingObj, int directionValue, bool isRunning, Func<bool, float> speedCalculationMethod)
        {
            movingObj.transform.Translate((direction * speedCalculationMethod(isRunning)) * directionValue * Time.deltaTime);
            //movingObj.GetComponent<Rigidbody>().AddForce((Vector3.up * speedCalculationMethod(isRunning)) * directionValue);
        }

    }
}
