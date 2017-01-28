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

            var target = attackTargetService.GetTargetForStockAttack(attackingObj,
             movementManager.GetHorizontalFacingDirection(), movementManager.GetVerticalFacingDirection());

            if(target == null)
            {
                return false;
            }

            return attackService.AttackTarget(attackingObj, target);
        }
        #endregion
        public bool Attack(GameObject attackingObj)
        {
            ICombatManager attackerCombatManager = attackingObj.GetComponent<ICombatManager>();

            if (attackerCombatManager.CanAttack())
            {
                attackerCombatManager.DisableAttackerActions();
                attackerCombatManager.IncreaseSequenceWaitForAction();
                return AttackByType(attackingObj);

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
                    //AttackTargetService: get targets surrouding object
                    //attack target
                    break;
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

        public void DisableCombatIinput(GameObject attackingObj)
        {
            if (attackingObj.tag == Tags.PlayerTag)
            {
                attackingObj.GetComponent<IPlayerCombatInputManager>().Disable();
            }
        }

        public void EnableCombatIinput(GameObject attackingObj)
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
        #region PRIVATE METHODS

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
        #endregion
    }
}
