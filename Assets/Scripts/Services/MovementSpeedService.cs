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
        private readonly BaseCreature _plainMoveableEntity;

        public MovementSpeedService()
        {
            _plainMoveableEntity = IoCContainer.GetImplementation<IBaseCreatureFactory>().GetPlainMoveableEntity();
        }


        public void SetHorizontalSpeed(DirectionEnum horizontalValue, GameObject movingObj, bool isRunning)
        {
            SetStraightLineSpeed(Vector3.right, movingObj, GetHorizontalDirectionValue(horizontalValue), isRunning, _plainMoveableEntity.GetStraightLineSpeed);
        }

        public void SetVerticalSpeed(DirectionEnum verticalValue, GameObject movingObj, bool isRunning)
        {
            SetStraightLineSpeed(Vector3.up, movingObj, GetVerticalDirectionValue(verticalValue), isRunning, _plainMoveableEntity.GetStraightLineSpeed);
        }


        public void SetDiagonalSpeed(DirectionEnum horizontalValue, DirectionEnum verticalValue, GameObject movingObj, bool isRunning)
        {
            //Horizontal
            SetStraightLineSpeed(Vector3.right, movingObj, GetHorizontalDirectionValue(horizontalValue), isRunning, _plainMoveableEntity.GetDiagionalSpeed);

            //Vertical
            SetStraightLineSpeed(Vector3.up, movingObj, GetVerticalDirectionValue(verticalValue), isRunning, _plainMoveableEntity.GetDiagionalSpeed);

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
        }

    }
}
