using Assets.Scripts.Entities.ApplicationObjects;
using Assets.Scripts.Interfaces.Controllers;
using Assets.Scripts.Interfaces.Managers.Combat;
using Assets.Scripts.Interfaces.Managers.Movement;
using Assets.Scripts.Interfaces.Managers.Objects;
using Assets.Scripts.IoC;
using System;
using UnityEngine;

namespace Assets.Scripts.Managers.Combat
{
    public class NpcTargetingManager : CombatEntryPointManager, INpcTargetingManager, IAttackTiming
    {
        private INpcMovementManager _npcMovementManager;
        private IMovementController _movementController;

        private BaseAppObject _baseAppObject;


        #region PRIVATE METHODS
        void Awake()
        {
            _objectManager = GetComponent<IObjectManager>();
            _npcMovementManager = GetComponent<INpcMovementManager>();
            _combatManager = GetComponent<ICombatManager>();

            _baseAppObject = _objectManager.GetBaseAppObject();


            _combatController = IoCContainer.GetImplementation<ICombatController>();
            _movementController = IoCContainer.GetImplementation<IMovementController>();

            Debug.Log("Teste");

            Disable();
        }

        private void Update()
        {
            if (IsCloseToTarget() && _baseAppObject.CombatManager.CanAttack() && !_hasDelegatedTriggered)
            {
                AttackTarget();
            }
            else if (_hasDelegatedTriggered && _baseAppObject.CombatManager.CanAttack() && IsCloseToTarget())
            {
                Debug.Log(gameObject.name + " Has finished waiting: " + DateTime.Now);

                //delegatedAction(_baseAppObject);
                //_hasDelegatedTriggered = false;
            }

            //REFACTOR: If has past more than half of the attack delay, should attack and suffer with recovery delay
            //Target has moved away -> Cancel attack
            if(_hasDelegatedTriggered && !IsCloseToTarget())
            {
                Debug.Log("not close");
                _hasDelegatedTriggered = false;
                //delegatedAction = null;
                _combatManager.EnableAttackerActions();
            }
        }
    
        bool IsCloseToTarget()
        {
            return _npcMovementManager.GetDistanceFromTarget() <= _baseAppObject.EquippedItensManager.GetEquippedWeapon().WeaponRange;
        }
        #endregion

        public void AttackTarget()
        {
            base.InitiateAttack();
        }

        public void WaitDelayAfterAttack(float delayTime)
        {
            //REFACTOR
            //Should disable for a longer period if attack has been blocked
            _combatManager.DisableAttackerActions(delayTime);

            Debug.Log(gameObject.name + "Is waiting delay after attack");
        }
    }
}
