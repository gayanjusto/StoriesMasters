using System;
using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces.Services;
using UnityEngine;
using Assets.Scripts.IoC;
using System.Linq;
using Assets.Scripts.Constants;

namespace Assets.Scripts.Services
{
    public class AttackTargetService : IAttackTargetService
    {
        private readonly IDirectionService _directionService;
        private const float targetStockSphereRadius = .0025f;
        private const float targetSwingSphereRadius = .2f;


        public AttackTargetService()
        {
            _directionService = IoCContainer.GetImplementation<IDirectionService>();
        }

  
        public Vector3 GetAttackPivotPositionByFacingDirection(Vector3 attackerPosition, DirectionEnum facingDirection)
        {
            float horizontalOffset = _directionService.GetAxisUnitValueByHorizontalDirection(facingDirection);
            float verticalOffset = _directionService.GetAxisUnitValueByVerticalDirection(facingDirection);

            attackerPosition.x += horizontalOffset;
            attackerPosition.z += verticalOffset;

            return new Vector3(attackerPosition.x, attackerPosition.y, attackerPosition.z);
        }

        public Vector3[] GetSwingAttackVector3ByDirections(DirectionEnum horizontalValue, DirectionEnum verticalValue)
        {
            throw new NotImplementedException();
        }

        public GameObject GetTargetForStockAttack(GameObject attackingObj, DirectionEnum facingDirection)
        {
            Vector3 targetingPosition = GetAttackPivotPositionByFacingDirection(attackingObj.transform.position, facingDirection);

            Collider2D[] targets = Physics2D.OverlapCircleAll(targetingPosition, targetStockSphereRadius).Where(x => x.gameObject != attackingObj).ToArray();
            if(targets == null || targets.Length == 0)
            {
                return null;
            };

            if(targets[0].tag != Tags.Targetable || targets[0].gameObject == attackingObj)
            {
                return null;
            }

            return targets[0].gameObject;
        }

        public GameObject[] GetTargetsForSwingAttack(GameObject attackingObj, DirectionEnum facingDirection)
        {

            Vector3 targetingPosition = GetAttackPivotPositionByFacingDirection(attackingObj.transform.position, facingDirection);

            Collider[] targets = Physics.OverlapSphere(targetingPosition, targetSwingSphereRadius)
                .Where(x => x.gameObject != attackingObj && x.tag == Tags.Targetable).ToArray();

            if (targets == null || targets.Length == 0)
            {
                return null;
            };

            //if (targets[0].tag != Tags.Targetable || targets[0].gameObject == attackingObj)
            //{
            //    return null;
            //}

            return targets.Select(x => x.gameObject).ToArray();
        }
    }
}
