using Assets.Scripts.Entities.ApplicationObjects;
using Assets.Scripts.Interfaces.Controllers;
using Assets.Scripts.Interfaces.Managers.Combat;
using Assets.Scripts.Interfaces.Managers.Objects;
using Assets.Scripts.Interfaces.Services;
using Assets.Scripts.IoC;
using System;
using UnityEngine;

namespace Assets.Scripts.Managers.Combat
{
    public class CombatEntryPointManager : BaseMonoBehaviour
    {
        protected ICombatController _combatController;
        protected IObjectManager _objectManager;
        protected ICombatManager _combatManager;
        protected IAttackService _attackService;
        protected IAttackTargetService _attackTargetService;

        protected bool _hasDelegatedTriggered;

        protected bool _hasAttackReady;

        protected void SetBaseDependencies()
        {
            _combatManager = GetComponent<ICombatManager>();
            _objectManager = GetComponent<IObjectManager>();
            _combatController = IoCContainer.GetImplementation<ICombatController>();
            _attackService = IoCContainer.GetImplementation<IAttackService>();
            _attackTargetService = IoCContainer.GetImplementation<IAttackTargetService>();
        }

        protected void InitiateAttack()
        {
            //Tell CombatManager that object is ready to attack
            _combatManager.SetAttackReady(true);
        }

        protected void StartShieldBlock()
        {
            //Tell CombatManager that object is blocking
            _combatManager.SetIsBlocking(true);
        }

        protected void CancelBlocking()
        {
            //Tell CombatManager that object is not blocking nor parrying
            _combatManager.SetIsBlocking(false);
        }

        protected void StartParryingBlock()
        {
            BaseAppObject target = _attackTargetService.GetFacingTarget(_objectManager.GetBaseAppObject());

            if(target == null)
            {
                this.SetDelayAfterFailedParryAttempt();
                return;
            }

            //If target is not attacking, there's nothing to parry
            if (!target.CombatManager.GetIsAttacking())
            {
                SetDelayAfterFailedParryAttempt();
                return;
            }

            //AttackService will check if parry was successful once attacker has attacked
            _combatManager.SetParryingTarget(target);
        }

        void SetDelayAfterFailedParryAttempt()
        {
            Debug.Log("[" + this.name + "] Nenhum alvo encontrado para Parry");
            //Set delay after action
        }
    }
}
