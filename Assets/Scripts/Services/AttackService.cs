using Assets.Scripts.Entities.ApplicationObjects;
using Assets.Scripts.Entities.IntelligentBodies;
using Assets.Scripts.Interfaces.Managers.Itens;
using Assets.Scripts.Interfaces.Managers.Objects;
using Assets.Scripts.Interfaces.Services;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces.Controllers;
using Assets.Scripts.IoC;
using Assets.Scripts.Interfaces.Managers.Movement;
using Assets.Scripts.Interfaces.Managers.Combat;

namespace Assets.Scripts.Services
{
    public class AttackService : IAttackService
    {
        private ICombatVisualInformationService _combatVisualInfoService = IoCContainer.GetImplementation<ICombatVisualInformationService>();

        public AttackService()
        {
            _combatVisualInfoService = IoCContainer.GetImplementation<ICombatVisualInformationService>();
        }

        #region PRIVATE METHODS
        IList<BaseAppObject> GetTargetsHitByStockAttack(BaseAppObject attackingObj, BaseAppObject target, ref IList<BaseAppObject> targetsHit)
        {
            if (target == null)
            {
                return targetsHit;
            }

            BaseAppObject targetHit = AttackTarget(attackingObj, target);
            if (targetHit != null)
            {
                targetsHit.Add(targetHit);
            }

            return targetsHit;
        }

        IList<BaseAppObject> GetTargetsHitBySwingAttack(BaseAppObject attackingObj, BaseAppObject[] targets, ref IList<BaseAppObject> hitTargets)
        {

            hitTargets = AttackTargets(attackingObj, targets);


            return hitTargets;
        }

        IList<BaseAppObject> AttackByType(BaseAppObject attackingObj, BaseAppObject[] targets)
        {
            IList<BaseAppObject> hitTargets = new List<BaseAppObject>();

            switch (attackingObj.GetAttackTypeForEquippedWeapon())
            {
                case AttackTypeEnum.Stock:
                return GetTargetsHitByStockAttack(attackingObj, targets[0], ref hitTargets);
                break;
                case AttackTypeEnum.Swing:
                GetTargetsHitBySwingAttack(attackingObj, targets, ref hitTargets);
                break;
                case AttackTypeEnum.SemiSwing:
                //AttackTargetService: get targets surrouding 3 blocks object
                //attack target
                break;
                case AttackTypeEnum.QuickRanged:
                break;
                default:
                return null;
            }

            return hitTargets;
        }



        void MiniStunTargets(IList<BaseAppObject> targets)
        {
            Debug.Log("Mini stunned targets");

            for (int i = 0; i < targets.Count; i++)
            {
                MiniStunTarget(targets[i]);
            }
        }

        BaseAppObject AttackTarget(BaseAppObject attackerObj, BaseAppObject target)
        {
            //attacker has suffered any action that prevents him from continue his attack
            if (attackerObj.HasActionsPrevented)
            {
                return null;
            }

            //Calculate chances of hitting target or if target has already blocked this attacker
            if (attackerObj.CanAttackTarget(target))
            {
                var equippedWeapon = attackerObj.GetMonoBehaviourObject<IEquippedItensManager>().GetEquippedWeapon();
                double damageDealtToTarget = attackerObj.GetDamageDealt(equippedWeapon);

                target.ReceiveDamage(damageDealtToTarget);

                DisableParryDefense(target);

                return target;
            }

            // DisableParryDefense(target);

            return null;
        }

        IList<BaseAppObject> AttackTargets(BaseAppObject attackerObj, BaseAppObject[] targetsObjs)
        {
            if (attackerObj.HasActionsPrevented)
            {
                return null;
            }

            double damageDealtToTarget = attackerObj.GetDamageDealt(attackerObj.GetMonoBehaviourObject<IEquippedItensManager>().GetEquippedWeapon());

            List<BaseAppObject> creaturesHit = new List<BaseAppObject>();
            for (int i = 0; i < targetsObjs.Length; i++)
            {

                //Calculate chances of hitting target or if target has already blocked this attacker
                if (!attackerObj.CanAttackTarget(targetsObjs[i]))
                {
                    break; //If target has simply dodged, it should continue the attack to the next target
                }

                //Decrease damage by each target
                if (i > 0)
                {
                    damageDealtToTarget -= (damageDealtToTarget * (i * 10)) / (100);
                }

                targetsObjs[i].ReceiveDamage(damageDealtToTarget);
                creaturesHit.Add(targetsObjs[i]);
            }

            //After a target being attacked, it should disable his parry defense
            //for it's no longer necessary
            for (int i = 0; i < targetsObjs.Length; i++)
            {
                DisableParryDefense(targetsObjs[i]);
            }

            return creaturesHit;
        }

        void DisableParryDefense(BaseAppObject defender)
        {
            var defenderCombatManager = defender.GetMonoBehaviourObject<ICombatManager>();
            defenderCombatManager.SetHasCastAction(false);
            defenderCombatManager.SetParryingTarget(null);
            defenderCombatManager.SetIsAttemptingToParryAttack(false);

        }

        bool CanAttackTarget(BaseAppObject target, BaseAppObject attacker)
        {
            var targetCombatManager = target.GetMonoBehaviourObject<ICombatManager>();
            //Target has already defended the attack from this attacker
            if (targetCombatManager.GetIsAttemptingToParry() && targetCombatManager.GetParryingTarget() == attacker)
            {
                Debug.Log("Blocked attack with parry");

                return false;
            }

            //If is attacking a target that is parrying a different attacker, then this attacker will have
            //an instant success.
            if (targetCombatManager.GetIsAttemptingToParry() && targetCombatManager.GetParryingTarget() != attacker)
            {
                Debug.Log(target.GameObject.name + " foi atacado por outro objeto e perdeu a defesa");
                return true;
            }

            //Target is trying to block with a shield
            if (targetCombatManager.GetIsBlockingWithShield())
            {
                MiniStunTarget(target);

                if (TargetIsBlockingAttackerDirectionsWithShield(attacker, target))
                {
                    Debug.Log(target.GameObject.name + " is trying to block with shield");
                    return target.DefendAttack(attacker, DefenseTypeEnum.Block);
                }
                else //Target is not blocking attacker direction resulting in Instant hit
                {
                    return true;
                }
            }

            //return: Int + Dex + Skill + Rand 100 X enemie's
            return true;
        }
        #endregion

        public bool Attack(BaseAppObject attackingObj)
        {
            //Remove highlight of incoming attack for player
            _combatVisualInfoService.RemoveHighlightAttackerInformation(attackingObj);

            var attackCombatManager = attackingObj.GetMonoBehaviourObject<ICombatManager>();
            //has already casted an attack, so it should be set to false
            //It's set to true in AttackObserver
            attackCombatManager.SetIsAttacking(false);

            //Set delay after attack
            //REFACTOR: Get delay time based on attributes
            attackCombatManager.DisableAttackerActions(2.0f);

            var attackerTargets = attackCombatManager.GetTargets();

            //Increase attack sequence
            // attackingObj.CombatManager.IncreaseSequenceWaitForAction();

            if (attackerTargets == null || attackerTargets.Count() == 0)
            {
                return false;
            }

            IList<BaseAppObject> targetsHitByAttack = AttackByType(attackingObj, attackerTargets);


            //Was a successful attack?
            if (targetsHitByAttack != null && targetsHitByAttack.Count > 0)
            {
                MiniStunTargets(targetsHitByAttack);
                attackingObj.IncreaseCombatSkillPoint();

                //check if any target has died
                for (int i = 0; i < targetsHitByAttack.Count; i++)
                {
                    if (!targetsHitByAttack[i].IsAlive())
                    {
                        targetsHitByAttack[i].Die();
                    }
                }
                return true;
            }

            return false;
        }

        public bool TargetIsBlockingAttackerDirectionsWithShield(BaseAppObject attacker, BaseAppObject target)
        {
            IMovementController movementController = IoCContainer.GetImplementation<IMovementController>();
            IDirectionService directionService = IoCContainer.GetImplementation<IDirectionService>();

            DirectionEnum attackerFacingDirection = attacker.GameObject.GetComponent<IFacingDirection>().GetFacingDirection();
            DirectionEnum[] targetBlockingDirections = directionService.GetNeighborDirections(target);

            for (int i = 0; i < targetBlockingDirections.Length; i++)
            {
                if (targetBlockingDirections[i] == directionService.GetOppositeDirection(attackerFacingDirection))
                {
                    return true;
                }
            }

            return false;
        }

        public void MiniStunTarget(BaseAppObject target)
        {
            target.GetMonoBehaviourObject<ICombatManager>().DisableAttackerActions(2.5f);
            target.HasActionsPrevented = true;
        }

        public void MiniStunTarget(BaseAppObject target, float timeToStun)
        {
            target.GetMonoBehaviourObject<ICombatManager>().DisableAttackerActions(timeToStun);
        }

        public float GetTimeForAttackDelay(BaseAppObject target)
        {
            //ToDo: Calculate time based on skills / equipment / weight
            return 5.0f;
        }

        public bool AttackIsPastHalfWay(BaseAppObject attacker)
        {
            //attack has spend more than half of the total of the swing/freeze time
            var attackerCombatManager = attacker.GetMonoBehaviourObject<ICombatManager>();
            return attackerCombatManager.GetCurrentTimeForAttackDelay() < (attackerCombatManager.GetTotalFreezeTime() / 2);
        }
    }
}
