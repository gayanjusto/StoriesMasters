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
        private const float targetSphereRadius = .0025f;
        public AttackTargetService()
        {
            _directionService = IoCContainer.GetImplementation<IDirectionService>();
        }

        public Vector3[] GetSemiSwingAttackVector3ByDirections(DirectionEnum horizontalValue, DirectionEnum verticalValue)
        {
            throw new NotImplementedException();
        }

        public Vector2 GetStockAttackVector2ByDirections(Vector3 attackerPosition, DirectionEnum horizontalValue, DirectionEnum verticalValue)
        {
            float horizontalOffset = _directionService.GetAxisUnitValueByHorizontalDirection(horizontalValue);
            float verticalOffset = _directionService.GetAxisUnitValueByVerticalDirection(verticalValue);

            attackerPosition.x += horizontalOffset;
            attackerPosition.y += verticalOffset;

            return new Vector2(attackerPosition.x, attackerPosition.y);
        }

        public Vector3[] GetSwingAttackVector3ByDirections(DirectionEnum horizontalValue, DirectionEnum verticalValue)
        {
            throw new NotImplementedException();
        }

        public GameObject GetTargetForStockAttack(GameObject attackingObj, DirectionEnum attackingObjHorizontalValue, DirectionEnum attackingObjVerticalValue)
        {
            Vector2 targetingPosition = GetStockAttackVector2ByDirections(attackingObj.transform.position, 
                attackingObjHorizontalValue, attackingObjVerticalValue);

            Collider2D[] targets = Physics2D.OverlapCircleAll(targetingPosition, targetSphereRadius).Where(x => x.gameObject != attackingObj).ToArray();
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
    }
}
