using Assets.Scripts.Entities.ApplicationObjects;
using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces.Managers.Objects;
using Assets.Scripts.Interfaces.Services;
using Assets.Scripts.IoC;
using System.Linq;

namespace Assets.Scripts.Services
{
    public class AttackTargetService : IAttackTargetService
    {
        #region PRIVATE METHODS
        BaseAppObject[] GetTargetsByAttackType(BaseAppObject attackingObj)
        {
            //IEquippedItensManager equippedItensManager = attackingObj.GetComponent<IEquippedItensManager>();

            BaseAppObject[] returningTargets = null;

            switch (attackingObj.GetAttackTypeForEquippedWeapon())
            {
                case AttackTypeEnum.Stock:
                GetTargetByStockAttack(attackingObj, ref returningTargets);
                break;
                case AttackTypeEnum.Swing:
                GetTargetsBySwingAttack(attackingObj, ref returningTargets);//
                break;
                case AttackTypeEnum.SemiSwing:
                //AttackTargetService: get targets surrouding 3 blocks object
                //attack target
                break;
                case AttackTypeEnum.Ranged:
                break;
            }

            return returningTargets;
        }

        BaseAppObject[] GetTargetByStockAttack(BaseAppObject attackingObj, ref BaseAppObject[] returningTargets)
        {
            ITargetService targetService = IoCContainer.GetImplementation<ITargetService>();

            var target = targetService.GetTargetForFacingDirection(attackingObj.GameObject, attackingObj.MovementManager.GetFacingDirection());
            if (target != null)
            {
                returningTargets = new BaseAppObject[1];
                returningTargets[0] = target.GetComponent<IObjectManager>().GetBaseAppObject();
            }

            return returningTargets;
        }

        BaseAppObject[] GetTargetsBySwingAttack(BaseAppObject attackingObj, ref BaseAppObject[] returningTargets)
        {
            ITargetService attackTargetService = IoCContainer.GetImplementation<ITargetService>();

            var targets = attackTargetService.GetTargetsForSemiCircle(attackingObj.GameObject, attackingObj.MovementManager.GetFacingDirection());
            if (targets != null && targets.Length > 0)
            {
                returningTargets = new BaseAppObject[targets.Length];
                returningTargets = targets.Select(x => x.GetComponent<IObjectManager>().GetBaseAppObject()).ToArray();
            }

            return returningTargets;
        }
        #endregion

        public BaseAppObject[] SetTargetsForAttack(BaseAppObject attackingObj)
        {
            ITargetService targetService = IoCContainer.GetImplementation<ITargetService>();
            BaseAppObject[] targets = null;

            if (attackingObj.CombatManager.CanAttack())
            {
                targets = GetTargetsByAttackType(attackingObj);
            }

            attackingObj.CombatManager.SetTargets(targets);

            return targets;
        }
    }
}
