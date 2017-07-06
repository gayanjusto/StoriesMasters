using System;
using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces.Services;
using UnityEngine;
using Assets.Scripts.IoC;
using System.Linq;
using Assets.Scripts.Constants;

namespace Assets.Scripts.Services
{
    public class TargetService : ITargetService
    {
        private readonly IDirectionService _directionService;
        private const float targetStockSphereRadius = 1.0f;
        private const float targetArchRadius = .1f;
        private const float targetSemiCircleRadius = .5f;


        public TargetService()
        {
            _directionService = IoCContainer.GetImplementation<IDirectionService>();
        }

  
        public Vector3 GetTargetPivotPositionByFacingDirection(Vector3 attackerPosition, DirectionEnum facingDirection)
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

      
        public GameObject GetTargetForFacingDirection(GameObject attackingObj, DirectionEnum facingDirection)
        {
            Vector3 targetingPosition = GetTargetPivotPositionByFacingDirection(attackingObj.transform.position, facingDirection);

            int collisionObjsLayerId = LayerMask.NameToLayer("CollisionObjects");
            int layerMask = 1 << collisionObjsLayerId;

            Collider[] targets = Physics.OverlapSphere(targetingPosition, targetStockSphereRadius, layerMask)
                .Where(x => x.gameObject != attackingObj && x.tag.StartsWith(Tags.Targetable) && !x.tag.Equals(attackingObj.tag) ).ToArray();
            if(targets == null || targets.Length == 0)
            {
                return null;
            };

            return targets[0].gameObject;
        }

      
        public GameObject[] GetTargetsForSemiCircle(GameObject attackingObj, DirectionEnum facingDirection)
        {

            Vector3 targetingPosition = GetTargetPivotPositionByFacingDirection(attackingObj.transform.position, facingDirection);

            Collider[] targets = Physics.OverlapSphere(targetingPosition, targetSemiCircleRadius)
                .Where(x => x.gameObject != attackingObj && x.tag.StartsWith(Tags.Targetable)).ToArray();

            if (targets == null || targets.Length == 0)
            {
                return null;
            };

            return targets.Select(x => x.gameObject).ToArray();
        }

        //<summary>
        //Will return 3 possible targets:
        //1 - Facing the object
        //2 - Right corner facing the object
        //3 - Left corner facing the object
        //</summary>
        public GameObject[] GetTargetsForArchRadius(GameObject attackingObj, DirectionEnum facingDirection)
        {

            Vector3 targetingPosition = GetTargetPivotPositionByFacingDirection(attackingObj.transform.position, facingDirection);

            Collider[] targets = Physics.OverlapSphere(targetingPosition, targetArchRadius)
                .Where(x => x.gameObject != attackingObj && x.tag.StartsWith(Tags.Targetable)).ToArray();

            if (targets == null || targets.Length == 0)
            {
                return null;
            };

       
            return targets.Select(x => x.gameObject).ToArray();
        }
    }
}
