using Assets.Scripts.Entities.ApplicationObjects;
using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces.Services;
using System;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class MotionService : IMotionService
    {

        private Vector3 _verticalMovement = Vector3.forward;
        private Vector3 _horizontalMovement = Vector3.right;

        public void SetHorizontalSpeed(DirectionEnum horizontalValue, GameAppObject movingObj, bool isRunning)
        {
            SetStraightLineSpeed(_horizontalMovement, movingObj.gameObject, GetHorizontalDirectionValue(horizontalValue), isRunning, movingObj.moveSpeed.GetStraightLineSpeed);
        }

        public void SetVerticalSpeed(DirectionEnum verticalValue, GameAppObject movingObj, bool isRunning)
        {
            SetStraightLineSpeed(_verticalMovement, movingObj.gameObject, GetVerticalDirectionValue(verticalValue), isRunning, movingObj.moveSpeed.GetStraightLineSpeed);
        }


        public void SetDiagonalSpeed(DirectionEnum horizontalValue, DirectionEnum verticalValue, GameAppObject movingObj, bool isRunning)
        {
            //Horizontal
            SetStraightLineSpeed(_horizontalMovement, movingObj.gameObject, GetHorizontalDirectionValue(horizontalValue), isRunning, movingObj.moveSpeed.GetDiagionalSpeed);

            //Vertical
            SetStraightLineSpeed(_verticalMovement, movingObj.gameObject, GetVerticalDirectionValue(verticalValue), isRunning, movingObj.moveSpeed.GetDiagionalSpeed);
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
