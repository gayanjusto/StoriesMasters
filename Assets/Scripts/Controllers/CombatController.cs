using Assets.Scripts.Constants;
using Assets.Scripts.Interfaces.Controllers;
using Assets.Scripts.Interfaces.Managers.Combat;
using UnityEngine;
using Assets.Scripts.Interfaces.Managers.Itens;
using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces.Managers.Objects;
using Assets.Scripts.Entities.IntelligentBodies;
using Assets.Scripts.Interfaces.Services;
using Assets.Scripts.IoC;
using Assets.Scripts.Interfaces.Managers.Movement;
using Assets.Scripts.Interfaces.Managers.Inputs;
using Assets.Scripts.Entities.Itens.Equippable;
using Assets.Scripts.Entities.ApplicationObjects;

namespace Assets.Scripts.Controllers
{
    public class CombatController : ICombatController
    {
        private readonly ITargetService _targetService;
        private readonly ISkillPointService _skillPointService;
        private readonly IAttackService _attackService;
        private readonly IAttackTargetService _attackTargetService;


        public CombatController()
        {
            _attackService = IoCContainer.GetImplementation<IAttackService>();
            _attackTargetService = IoCContainer.GetImplementation<IAttackTargetService>();

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

        public BaseAppObject[] StartAttack(BaseAppObject attackingObj)
        {

            BaseAppObject[] targets = _attackTargetService.SetTargetsForAttack(attackingObj);

            attackingObj.CombatManager.SetIsAttacking(true);

            //REFACTOR
            attackingObj.CombatManager.DisableAttackerActions(GetTimeForAttackDelay(attackingObj)); //<- DelayTime must a method of BaseAppObj

            return targets;
        }

        public bool Attack(BaseAppObject attackingObj)
        {
            bool attackSuccess = _attackService.Attack(attackingObj);
            attackingObj.CombatManager.SetIsAttacking(false);

            //REFACTOR
            //Should disable for a longer period if attack has been blocked
            attackingObj.GameObject.GetComponent<IAttackTiming>().WaitDelayAfterAttack(5.5f);

            return attackSuccess;
        }


        public bool ParryAttack(BaseAppObject parryingObj)
        {
            //REFACTOR: must get disable value from obj dexterity calculation
            parryingObj.CombatManager.DisableAttackerActions(1.0f);

            ITargetService attackTargetService = IoCContainer.GetImplementation<ITargetService>();

            if (parryingObj.EquippedItensManager.GetEquippedWeapon().ItemType == EquippableItemTypeEnum.Weapon)
            {
                var target = attackTargetService.GetTargetForFacingDirection(parryingObj.GameObject, parryingObj.MovementManager.GetFacingDirection());
                if (target == null)
                {
                    return false;
                }

                ICombatManager targetCombatManager = target.GetComponent<ICombatManager>();
                if (targetCombatManager.GetIsAttacking())
                {
                    parryingObj.CombatManager.SetParryingTarget(target.GetComponent<IObjectManager>().GetBaseAppObject());

                    bool isDefending = parryingObj.DefendAttack(target.GetComponent<IObjectManager>().GetBaseAppObject(), DefenseTypeEnum.Block);

                    parryingObj.CombatManager.SetIsDefending(isDefending);

                    return isDefending;
                }
            }
            return false;
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

        public float GetAttackDelayBasedOnEquippedWeapon(BaseAppObject attackingObj, bool isLastAttackSequence)
        {

            if (isLastAttackSequence)
            {
                return attackingObj.GetTimeToRecoverFromLastAttack();
            }

            return attackingObj.GetTimeToRecoverFromAction();
        }

        public float GetTimeForAttackDelay(BaseAppObject attackingObj)
        {
            return 5.0f;
        }
    }
}
