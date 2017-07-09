using System;
using Assets.Scripts.Interfaces.Managers.Combat;
using Assets.Scripts.MonoBehaviours;
using Assets.Scripts.Interfaces.Services.Combat;
using Assets.Scripts.Factories.Services.Combat;
using System.Collections;
using UnityEngine;
using Assets.Scripts.Interfaces.Managers.Movement;
using Assets.Scripts.Entities.ApplicationObjects;

namespace Assets.Scripts.Managers.Combat
{
    public class CombatManager : AppObjectMonoBehaviour, ICombatManager
    {
        private IAttackTargetService _attackTargetService;
        private IAttackService _attackService; 

        public override void Start()
        {
            base.Start();
            _attackTargetService = AttackTargetServiceFactory.GetInstance();
            _attackService = AttackServiceFactory.GetInstance();
        }

        public bool isDefending;
        public bool isAttacking;

      
        public bool GetIsDefending()
        {
            return isDefending;
        }

        public void SetIsDefending(bool isDefending)
        {
            this.isDefending = isDefending;
        }

        public bool GetIsAttacking()
        {
            return isAttacking;
        }

        public void SetIsAttacking(bool isAttacking)
        {
            this.isAttacking = isAttacking;
        }

        public bool CanCastAction()
        {
            return !isDefending && !isAttacking;
        }

        public void InitiateAttack()
        {
            isAttacking = true;

            //Set object froze so it won't move while it attacks
            GetComponent<IMovementManager>().SetObjectFrozen(true);

            //get targets
            //ToDo: get targets based on the attack type the object is using
            var targets = _attackTargetService.GetFacingTargets(base.gameAppObject);

            //get delay time for attack
            var delayAttackTime = base.gameAppObject.attackDelay.GetAttackDelay();

            //intiate attack procedure and receive delay for attack
            StartCoroutine(Attack(delayAttackTime, targets));
        }

        IEnumerator Attack(float time, GameAppObject[] targets)
        {
            //wait for attack initiation delay
            yield return new WaitForSeconds(time);

            if(targets != null)
            {
                //call attackService to calculate the attack effectiviness
                _attackService.AttackTargets(targets, base.gameAppObject);
            }
            
            //Finish attack
            StartCoroutine(FinishAttack());
        }

        IEnumerator FinishAttack()
        {
            //get delay time for attack recover
            //wait for delay
            yield return new WaitForSeconds(base.gameAppObject.attackDelay.GetAttackRecoverDelay());

            //set attack off
            isAttacking = false;

            //defroze object
            GetComponent<IMovementManager>().SetObjectFrozen(false);
        }
    }
}
