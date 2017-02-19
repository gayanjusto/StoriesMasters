using System;
using Assets.Scripts.Constants;
using Assets.Scripts.Interfaces.Controllers;
using Assets.Scripts.Interfaces.Managers.Combat;
using Assets.Scripts.Managers.Inputs;
using UnityEngine;
using Assets.Scripts.Interfaces.Managers.Itens;
using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces.Managers.Objects;
using Assets.Scripts.Entities.IntelligentBodies;
using Assets.Scripts.Interfaces.Services;
using Assets.Scripts.IoC;
using Assets.Scripts.Interfaces.Managers.Movement;
using Assets.Scripts.Interfaces.Managers.Inputs;

namespace Assets.Scripts.Controllers
{
    public class CombatController : ICombatController
    {

        #region PRIVATE METHODS
        bool StockAttack(GameObject attackingObj)
        {
            IAttackTargetService attackTargetService = IoCContainer.GetImplementation<IAttackTargetService>();
            IAttackService attackService = IoCContainer.GetImplementation<IAttackService>();
            IMovementManager movementManager = attackingObj.GetComponent<IMovementManager>();

            var target = attackTargetService.GetTargetForStockAttack(attackingObj, movementManager.GetFacingDirection());

            if(target == null)
            {
                return false;
            }

            if(attackService.AttackTarget(attackingObj, target) != null)
            {
                MiniStunTarget(target);

                return true;
            }

            return false;
        }

        bool SwingAttack(GameObject attackingObj)
        {
            IAttackTargetService attackTargetService = IoCContainer.GetImplementation<IAttackTargetService>();
            IAttackService attackService = IoCContainer.GetImplementation<IAttackService>();
            IMovementManager movementManager = attackingObj.GetComponent<IMovementManager>();

            var targets = attackTargetService.GetTargetsForSwingAttack(attackingObj, movementManager.GetFacingDirection());

            if (targets == null)
            {
                return false;
            }

            if (attackService.AttackTargets(attackingObj, targets) != null)
            {
                MiniStunTargets(targets);

                return true;
            }

            return false;
        }


        //<summary>
        //Prevent aditional input flow
        //</summary>
        void DisablePlayerInputMovement(GameObject playerObj)
        {
            playerObj.GetComponent<IMovementManager>().Disable();
        }
        void EnablePlayerInputMovement(GameObject playerObj)
        {
            playerObj.GetComponent<IMovementManager>().Enable();
        }

        void MiniStunTarget(GameObject target)
        {
            ICombatManager targetCombatManager = target.GetComponent<ICombatManager>();
            targetCombatManager.DisableAttackerActions(2.5f);
        }

        void MiniStunTargets(GameObject[] targets)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                MiniStunTarget(targets[i]);
            }
        }
        #endregion

        public bool Attack(GameObject attackingObj)
        {
            ICombatManager attackerCombatManager = attackingObj.GetComponent<ICombatManager>();
            ISkillPointService skillPointService = IoCContainer.GetImplementation<ISkillPointService>();
            if (attackerCombatManager.CanAttack())
            {
                attackerCombatManager.DisableAttackerActions();
                attackerCombatManager.IncreaseSequenceWaitForAction();
                bool successfulAttack = AttackByType(attackingObj);

                if (successfulAttack)
                {
                    skillPointService.IncreaseCombatSkillPoint(attackingObj);
                }

                return successfulAttack;
                //if attack was successful:
                //disable target for brief time, while it recovers from attack
            }

            return false;
        }

        public bool AttackByType(GameObject attackingObj)
        {
            IEquippedItensManager equippedItensManager = attackingObj.GetComponent<IEquippedItensManager>();
            

            if (equippedItensManager.GetEquippedWeapon() != null)
            {
                switch (equippedItensManager.GetEquippedWeapon().AttackType)
                {
                    case AttackTypeEnum.Stock:
                    return StockAttack(attackingObj);
                    case AttackTypeEnum.Swing:
                    return SwingAttack(attackingObj);
                    case AttackTypeEnum.SemiSwing:
                    //AttackTargetService: get targets surrouding 3 blocks object
                    //attack target
                    break;
                    case AttackTypeEnum.Ranged:
                    break;
                    default:
                    return false;
                }
            }

            return false;
        }

        public void Defend()
        {

        }

        public void DisableCombatInput(GameObject attackingObj)
        {
            if (attackingObj.tag == Tags.PlayerTag)
            {
                attackingObj.GetComponent<IPlayerCombatInputManager>().Disable();
            }
        }

        public void EnableCombatInput(GameObject attackingObj)
        {
            if (attackingObj.tag == Tags.PlayerTag)
            {
                attackingObj.GetComponent<IPlayerCombatInputManager>().Enable();
            }
        }

        public float GetAttackDelayBasedOnEquippedWeapon(GameObject attackingObj, bool isLastAttackSequence)
        {
            IEquippedItensManager equippedItensManager = attackingObj.GetComponent<IEquippedItensManager>();
            IObjectManager objectManager = attackingObj.GetComponent<IObjectManager>();

            BaseCreature entity = objectManager.GetBaseCreature();

            if (isLastAttackSequence)
            {
                return entity.GetTimeToRecoverFromLastAttack(equippedItensManager.GetEquippedWeapon());
            }

            return entity.GetTimeToRecoverFromAction(equippedItensManager.GetEquippedWeapon());
        }
    }
}
