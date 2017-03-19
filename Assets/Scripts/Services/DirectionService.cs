using System;
using System.Collections.Generic;
using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces.Services;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class DirectionService : IDirectionService
    {


        #region PRIVATE METHODS
        void SetHorizontalVector3Value(DirectionEnum horizontalValue, Vector3 directionVector3)
        {
            directionVector3.x = GetAxisValueByHorizontalDirection(horizontalValue);
        }

        void SetVerticalVector3Value(DirectionEnum verticalValue, Vector3 directionVector3)
        {
            directionVector3.y = GetAxisValueByVerticalDirection(verticalValue);
        }

        DirectionEnum[] GetTwoNeighboringDirectionCounterClock(int directionValue)
        {
            DirectionEnum[] directions = new DirectionEnum[2];
            for (int i = 0; i < 2; i++)
            {
                int enumValue = i + 1;
                int offEnumDirectionValues = 9;
                if (directionValue - (enumValue) <= 0)
                {
                    directions[i] = (DirectionEnum)offEnumDirectionValues - enumValue;
                    continue;
                }

                directions[i] = (DirectionEnum)enumValue;
            }

            return directions;
        }

        //REFACTOR: Possivelmente está retornando a direção passada + 1 vizinha, e não 2 vizinhas.
        //Example: If directionValue == 'UP'
        //It must return UpRight and Right.
        DirectionEnum[] GetTwoNeighboringDirectionWiseClock(int directionValue)
        {
            DirectionEnum[] directions = new DirectionEnum[2];
            for (int i = 0; i < 2; i++)
            {
                int enumValue = i + 1;
                int enumDirectionMaxValue = 8; //Drection = UpLeft
                int enumDirectionMinValue = 0; //Direction = None

                //Case the value is greater than the maximum it has to 
                //loop back to 1 (Up)
                if (directionValue + (enumValue) > enumDirectionMaxValue)
                {
                    directions[i] = (DirectionEnum)(enumDirectionMinValue) + enumValue;
                    continue;
                }

                directions[i] = (DirectionEnum)enumValue;
            }

            return directions;
        }

        //Return a direction from each side of the facing one
        //Example: Facing = Up -> Return UpLeft and UpRight
        DirectionEnum[] GetNeighborDirections(int directionValue)
        {
            DirectionEnum[] directions = new DirectionEnum[2];
            for (int i = 0; i < 2; i++)
            {
                //First iteration = left-side enum
                if (i == 0)
                {
                    int nextEnumValue = directionValue + 1;

                    //Greater than enum max (8 = UpLeft)
                    if (nextEnumValue > 8)
                    {
                        directions[i] = DirectionEnum.Up; //Return direction up
                        continue;
                    }

                    //if not the above:
                    directions[i] = (DirectionEnum)nextEnumValue;

                    continue;
                }

                //Second iteration = right-side enum
                if(i == 1)
                {
                int previousEnumValue = directionValue - 1;

                    //Less than enum min (0 = None)
                    if (previousEnumValue <= 0)
                    {
                        directions[i] = DirectionEnum.UpLeft; //Return direction UpLeft
                        continue;
                    }

                    //if not the above:
                    directions[i] = (DirectionEnum)previousEnumValue;

                    continue;
                }
            }

            return directions;
        }
        #endregion

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
            if (horizontalValue == DirectionEnum.Right || horizontalValue == DirectionEnum.UpRight || horizontalValue == DirectionEnum.DownRight)
            {
                return 0.2f;
            }

            if (horizontalValue == DirectionEnum.Left || horizontalValue == DirectionEnum.UpLeft || horizontalValue == DirectionEnum.DownLeft)
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
            if (verticalValue == DirectionEnum.Up || verticalValue == DirectionEnum.UpLeft || verticalValue == DirectionEnum.UpRight)
            {
                return 0.2f;
            }

            if (verticalValue == DirectionEnum.Down || verticalValue == DirectionEnum.DownLeft || verticalValue == DirectionEnum.DownRight)
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
            }
            else
            {
                return 0;
            }
        }


        public DirectionEnum GetFacingDirection(DirectionEnum horizontalValue, DirectionEnum verticalValue)
        {
            if (horizontalValue != DirectionEnum.None && verticalValue == DirectionEnum.None)
            {
                return horizontalValue;
            }

            if (horizontalValue == DirectionEnum.None && verticalValue != DirectionEnum.None)
            {
                return verticalValue;
            }

            if (horizontalValue == DirectionEnum.Right && verticalValue == DirectionEnum.Up)
            {
                return DirectionEnum.UpRight;
            }

            if (horizontalValue == DirectionEnum.Left && verticalValue == DirectionEnum.Up)
            {
                return DirectionEnum.UpLeft;
            }

            if (horizontalValue == DirectionEnum.Right && verticalValue == DirectionEnum.Down)
            {
                return DirectionEnum.DownRight;
            }

            if (horizontalValue == DirectionEnum.Left && verticalValue == DirectionEnum.Down)
            {
                return DirectionEnum.DownLeft;
            }

            return DirectionEnum.None;
        }

        public DirectionEnum[] GetFiveNeighboringDirectionsByFacingDirection(DirectionEnum facingDirection)
        {
            DirectionEnum[] directions = new DirectionEnum[5];
            DirectionEnum[] clockWiseDirections = GetTwoNeighboringDirectionWiseClock((int)facingDirection);
            DirectionEnum[] counterClockDirections = GetTwoNeighboringDirectionCounterClock((int)facingDirection);

            directions[0] = clockWiseDirections[1];
            directions[1] = clockWiseDirections[0];

            directions[2] = facingDirection;

            directions[3] = counterClockDirections[0];
            directions[4] = counterClockDirections[1];

            return directions;
        }

        public DirectionEnum GetHorizontalDirectionByValue(int value)
        {
            switch (value)
            {
                case 1:
                return DirectionEnum.Right;
                case -1:
                return DirectionEnum.Left;
                default:
                return DirectionEnum.None;
            }
        }

        public DirectionEnum GetVerticalDirectionByValue(int value)
        {
            switch (value)
            {
                case 1:
                return DirectionEnum.Up;
                case -1:
                return DirectionEnum.Down;
                default:
                return DirectionEnum.None;
            }
        }

        public DirectionEnum[] GetNeighborDirections(DirectionEnum facingDirection)
        {
            return GetNeighborDirections(facingDirection.GetHashCode());
        }

        public DirectionEnum GetOppositeDirection(DirectionEnum direction)
        {
            int directionValue = (int)direction;

            //For 8 enumValues, disregarding 0, the opposite is a total of 4 numbers away
            int enumOperator = 4;
            
            if(directionValue > 4)
            {
                return (DirectionEnum)directionValue - enumOperator;
            }

            return (DirectionEnum)directionValue + enumOperator;
        }
    }
}
