using Assets.Scripts.Entities.ApplicationObjects;
using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces.Managers.Objects;
using Assets.Scripts.Interfaces.Services;
using Assets.Scripts.IoC;
using System.Linq;
using System;
using Assets.Scripts.Interfaces.Managers.Movement;
using Assets.Scripts.Interfaces.Managers.Combat;

namespace Assets.Scripts.Services
{
    public class AttackTargetService : IAttackTargetService
    {
        private readonly ITargetService _targetService;

        public AttackTargetService()
        {
            _targetService = IoCContainer.GetImplementation<ITargetService>();
        }
        #region PRIVATE METHODS
        BaseAppObject[] GetTargetsByAttackType(BaseAppObject attackingObj)
        {
            //IEquippedItensManager equippedItensManager = attackingObj.GetComponent<IEquippedItensManager>();

            BaseAppObject[] returningTargets = null;

            switch (attackingObj.GetMonoBehaviourObject<IAttackStatusManager>().CurrentAttackType())
            {
                case AttackTypeEnum.AttackThrust:
                GetTargertFacingDirection(attackingObj, ref returningTargets);
                break;
                case AttackTypeEnum.AttackSwing:
                GetTargetsSemiCircleArea(attackingObj, ref returningTargets);//
                break;
                //AttackTargetService: get targets surrouding 3 blocks object
                //attack target
                break;
                case AttackTypeEnum.Ranged:
                break;
            }

            return returningTargets;
        }

        BaseAppObject[] GetTargertFacingDirection(BaseAppObject attackingObj, ref BaseAppObject[] returningTargets)
        {

            var target = _targetService.GetTargetForFacingDirection(attackingObj.GameObject, attackingObj.GetFacingDirection());
            if (target != null)
            {
                returningTargets = new BaseAppObject[1];
                returningTargets[0] = target.GetComponent<IObjectManager>().GetBaseAppObject();
            }

            return returningTargets;
        }

        BaseAppObject[] GetTargetsSemiCircleArea(BaseAppObject attackingObj, ref BaseAppObject[] returningTargets)
        {
            ITargetService attackTargetService = IoCContainer.GetImplementation<ITargetService>();

            var targets = attackTargetService.GetTargetsForSemiCircle(attackingObj.GameObject, attackingObj.GetFacingDirection());
            if (targets != null && targets.Length > 0)
            {
                returningTargets = new BaseAppObject[targets.Length];
                returningTargets = targets.Select(x => x.GetComponent<IObjectManager>().GetBaseAppObject()).ToArray();
            }

            return returningTargets;
        }
        #endregion

        public BaseAppObject[] GetTargetsForAttack(BaseAppObject attackingObj)
        {
            BaseAppObject[] targets = GetTargetsByAttackType(attackingObj);

            return targets;
        }

        public BaseAppObject GetFacingTarget(BaseAppObject actionObj)
        {
            BaseAppObject[] targets = null;
            GetTargertFacingDirection(actionObj, ref targets);

            if(targets == null)
            {
                return null;
            }
            return targets.FirstOrDefault();
        }
    }
}
