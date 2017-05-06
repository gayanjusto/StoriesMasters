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

namespace Assets.Scripts.Services
{
    public class AttackService : IAttackService
    {

        #region PRIVATE METHODS
        bool StockAttack(BaseAppObject attackingObj, BaseAppObject target)
        {
            if (target == null)
            {
                return false;
            }

            if (AttackTarget(attackingObj, target) != null)
            {
                MiniStunTarget(target);

                return true;
            }

            return false;
        }

        bool SwingAttack(BaseAppObject attackingObj, BaseAppObject[] targets)
        {
            if (targets == null)
            {
                return false;
            }

            IList<BaseAppObject> hitTargets = AttackTargets(attackingObj, targets);

            if (hitTargets.Count > 0)
            {
                //REFACTOR
                //Must MiniStun only those that were hit by the attack, out of the IF condition (it can stun the first, but not the last)
                MiniStunTargets(hitTargets.ToArray());

                return true;
            }

            return false;
        }

        bool AttackByType(BaseAppObject attackingObj, BaseAppObject[] targets)
        {

            switch (attackingObj.GetAttackTypeForEquippedWeapon())
            {
                case AttackTypeEnum.Stock:
                return StockAttack(attackingObj, targets[0]);
                case AttackTypeEnum.Swing:
                return SwingAttack(attackingObj, targets);
                case AttackTypeEnum.SemiSwing:
                //AttackTargetService: get targets surrouding 3 blocks object
                //attack target
                break;
                case AttackTypeEnum.Ranged:
                break;
                default:
                return false;
            }

            return false;
        }



        void MiniStunTargets(BaseAppObject[] targets)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                MiniStunTarget(targets[i]);
            }
        }

        BaseAppObject AttackTarget(BaseAppObject attackerObj, BaseAppObject target)
        {

            //Calculate chances of hitting target or if target has already blocked this attacker
            if (attackerObj.CanAttackTarget(target))
            {
                var equippedWeapon = attackerObj.EquippedItensManager.GetEquippedWeapon();
                double damageDealtToTarget = attackerObj.GetDamageDealt(equippedWeapon);

                target.ReceiveDamage(damageDealtToTarget);

                DisableParryDefense(target);

                return target;
            }

            DisableParryDefense(target);

            return null;
        }

        IList<BaseAppObject> AttackTargets(BaseAppObject attackerObj, BaseAppObject[] targetsObjs)
        {
            double damageDealtToTarget = attackerObj.GetDamageDealt(attackerObj.EquippedItensManager.GetEquippedWeapon());

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
            defender.CombatManager.SetIsDefending(false);
            defender.CombatManager.SetHasCastAction(false);
            defender.CombatManager.SetParryingTarget(null);
        }

        bool CanAttackTarget(BaseAppObject target, BaseAppObject attacker)
        {
            //Target has already defended the attack from this attacker
            if (target.CombatManager.GetIsDefending() && target.CombatManager.GetParryingTarget() == attacker)
            {
                Debug.Log("Blocked attack with parry");

                return false;
            }

            //If is attacking a target that is parrying a different attacker, then this attacker will have
            //an instant success.
            if (target.CombatManager.GetIsDefending() && target.CombatManager.GetParryingTarget() != attacker)
            {
                Debug.Log(target.GameObject.name + " foi atacado por outro objeto e perdeu a defesa");
                return true;
            }

            //Target is trying to block with a shield
            if (target.CombatManager.GetIsBlockingWithShield())
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
            var attackerTargets = attackingObj.CombatManager.GetTargets();

            //Can attack is already checked by input
            //Can attack even without targets
            //if (attackingObj.CombatManager.CanAttack() && attackerTargets != null)
            //{

            //Increase attack sequence
            attackingObj.CombatManager.IncreaseSequenceWaitForAction();

            bool successfulAttack = AttackByType(attackingObj, attackerTargets);

            if (successfulAttack)
            {
                attackingObj.IncreaseCombatSkillPoint();
            }

            return successfulAttack;
            //}
        }

        public bool TargetIsBlockingAttackerDirectionsWithShield(BaseAppObject attacker, BaseAppObject target)
        {
            IMovementController movementController = IoCContainer.GetImplementation<IMovementController>();
            IDirectionService directionService = IoCContainer.GetImplementation<IDirectionService>();

            DirectionEnum attackerFacingDirection = attacker.MovementManager.GetFacingDirection();
            DirectionEnum[] targetBlockingDirections = movementController.GetNeighboringDirections(target);

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
            target.CombatManager.DisableAttackerActions(2.5f);
        }

        public void MiniStunTarget(BaseAppObject target, float timeToStun)
        {
            target.CombatManager.DisableAttackerActions(timeToStun);
        }

        public float GetTimeForAttackDelay(BaseAppObject target)
        {
            //ToDo: Calculate time based on skills / equipment / weight
            return 5.0f;
        }
    }
}
