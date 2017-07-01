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
        protected IObjectManager _objectManager;
        protected ICombatManager _combatManager;
        protected IAttackService _attackService;
        protected IAttackTargetService _attackTargetService;

        protected bool _hasAttackReady;

        protected void SetBaseDependencies()
        {
            _combatManager = GetComponent<ICombatManager>();
            _objectManager = GetComponent<IObjectManager>();
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

        protected bool StartParryingBlock()
        {
            BaseAppObject target = _attackTargetService.GetFacingTarget(_objectManager.GetBaseAppObject());

            if(target == null)
            {
                this.SetDelayAfterFailedParryAttempt();
                return false;
            }

            //If target is not attacking, there's nothing to parry
            if (!target.GetMonoBehaviourObject<ICombatManager>().GetIsAttacking())
            {
                SetDelayAfterFailedParryAttempt();
                return false;
            }

            //An attack can be parried only if the timing is right (is past half the way of the attack)
            if (_attackService.AttackIsPastHalfWay(target))
            {
                //REFACTOR: AttackService will check if parry was successful once attacker has attacked
                //Parry should be available only if the attacker has cast more than half of his attack delay time
                _combatManager.SetParryingTarget(target);

                _combatManager.SetIsAttemptingToParryAttack(true);
            }

            return true;
        
        }

        void SetDelayAfterFailedParryAttempt()
        {
            Debug.Log("[" + this.name + "] Nenhum alvo encontrado para Parry");
            //Set delay after action
        }
    }
}
