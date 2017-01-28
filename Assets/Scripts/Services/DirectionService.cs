using System;
using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces.Services;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class DirectionService : IDirectionService
    {
        public Vector3 GetVector3ByDirections(DirectionEnum horizontalValue, DirectionEnum verticalValue)
        {
            var directionVector3 = new Vector3(0, 0, 0);
            if (horizontalValue == DirectionEnum.None && verticalValue == DirectionEnum.None)
            {
                return directionVector3;
            }

            SetHorizontalVector3Value(horizontalValue, directionVector3);
            SetVerticalVector3Value(verticalValue, directionVector3);


            return directionVector3;

        }

        public int GetAxisValueByHorizontalDirection(DirectionEnum horizontalValue)
        {
            if (horizontalValue == DirectionEnum.Right)
            {
                return 1;
            }

            if (horizontalValue == DirectionEnum.Left)
            {
                return -1;
            }
            else //None
            {
                return 0;
            }
        }

        public float GetAxisUnitValueByHorizontalDirection(DirectionEnum horizontalValue)
        {
            if (horizontalValue == DirectionEnum.Right)
            {
                return 0.2f;
            }

            if (horizontalValue == DirectionEnum.Left)
            {
                return -0.2f;
            }
            else //None
            {
                return 0;
            }
        }

        public float GetAxisUnitValueByVerticalDirection(DirectionEnum verticalValue)
        {
            if (verticalValue == DirectionEnum.Up)
            {
                return 0.2f;
            }

            if (verticalValue == DirectionEnum.Down)
            {
                return -0.2f;
            }
            else
            {
                return 0;
            }
        }

        public int GetAxisValueByVerticalDirection(DirectionEnum verticalValue)
        {
            if (verticalValue == DirectionEnum.Up)
            {
                return 1;
            }

            if (verticalValue == DirectionEnum.Down)
            {
                return -1;
            }else
            {
                return 0;
            }
        }

        void SetHorizontalVector3Value(DirectionEnum horizontalValue, Vector3 directionVector3)
        {
            directionVector3.x = GetAxisValueByHorizontalDirection(horizontalValue);
        }

        void SetVerticalVector3Value(DirectionEnum verticalValue, Vector3 directionVector3)
        {
                directionVector3.y = GetAxisValueByVerticalDirection(verticalValue);
        }
    }
}
