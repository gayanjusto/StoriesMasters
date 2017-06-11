using System;
using System.Collections.Generic;
using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces.Managers.Movement;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Managers.Movement
{
    public class NpcMovementManager : BaseMonoBehaviour, IMovementManager, INpcMovementManager
    {
        public Transform target;
        NavMeshAgent agent;
        float facingAngle;

        bool _canOnlyChangeDirection;

        #region PRIVATE METHODS
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            //target = GameObject.Find("Player").transform;
            //Set navMeshAgentSpeed
            //agent.destination = GameObject.Find("Player").transform.position;//target.position;
        }

        void Update()
        {
            if(TargetIsAvailable())
            {
                //facingDirection = GetFacingDirection();
                if (GetDistanceFromTarget() > 0.5f)
                {
                    agent.destination = target.position;
                }
            }
            else
            {
                Disable();
            }
          
        }

        bool TargetIsAvailable()
        {
            return target != null && target.gameObject.activeSelf;
        }
        #endregion
        public DirectionEnum GetFacingDirection()
        {
            facingAngle = transform.localEulerAngles.y;
            facingAngle = (facingAngle > 180) ? facingAngle - 360 : facingAngle;


            if(facingAngle < 0)
            {
                    if(facingAngle <= 0 && facingAngle >= -22.5f)
                    {
                        return DirectionEnum.Up;
                    }

                    if (facingAngle <= -22.5f && facingAngle >= -67.5f)
                    {
                        return DirectionEnum.UpLeft;
                    }

                    if (facingAngle <= -67.5f && facingAngle >= -112.5f)
                    {
                        return DirectionEnum.Left;
                    }

                    if (facingAngle <= -112.5f && facingAngle >= -157f)
                    {
                        return DirectionEnum.DownLeft;
                    }

                    if (facingAngle <= -157f && facingAngle >= -180f)
                    {
                        return DirectionEnum.Down;
                    }
            }
            else
            {
                if (facingAngle >= 0 && facingAngle <= 22.5f)
                {
                    return DirectionEnum.Up;
                }

                if (facingAngle >= 22.5f && facingAngle <= 67.5f)
                {
                    return DirectionEnum.UpRight;
                }

                if (facingAngle >= 67.5f && facingAngle <= 112.5f)
                {
                    return DirectionEnum.Right;
                }

                if (facingAngle >= 112.5f && facingAngle <= 157f)
                {
                    return DirectionEnum.DownRight;
                }

                if (facingAngle >= 157f && facingAngle <= 180f)
                {
                    return DirectionEnum.Down;
                }
            }

            return DirectionEnum.None;
        }

        public bool IsMoving()
        {
            throw new NotImplementedException();
        }

        public float GetDistanceFromTarget()
        {
            return Vector3.Distance(transform.position, target.position);
        }

        public void SetTarget(GameObject target)
        {
            this.target = target.transform;
        }

        public void SetCanChangeDirectionButNotMove(bool canOnlyChangeDirection)
        {
            _canOnlyChangeDirection = canOnlyChangeDirection;
        }
    }
}
