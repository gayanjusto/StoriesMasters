using Assets.Scripts.Entities.ApplicationObjects;
using Assets.Scripts.Interfaces.Controllers;
using Assets.Scripts.Interfaces.Managers.Combat;
using Assets.Scripts.Interfaces.Managers.Itens;
using Assets.Scripts.Interfaces.Managers.Movement;
using Assets.Scripts.Interfaces.Managers.Objects;
using Assets.Scripts.Interfaces.Services;
using Assets.Scripts.IoC;
using System;
using UnityEngine;

namespace Assets.Scripts.Managers.Combat
{
    public class NpcAttackManager : CombatEntryPointManager, INpcAttackManager
    {
        private INpcMovementManager _npcMovementManager;
        private IMovementController _movementController;
        private IEquippedItensManager _equippedItensManager;

        private BaseAppObject _baseAppObject;


        #region PRIVATE METHODS
        void Awake()
        {
            _objectManager = GetComponent<IObjectManager>();
            _npcMovementManager = GetComponent<INpcMovementManager>();
            _combatManager = GetComponent<ICombatManager>();
            _baseAppObject = _objectManager.GetBaseAppObject();
            _equippedItensManager = GetComponent<IEquippedItensManager>();

            _movementController = IoCContainer.GetImplementation<IMovementController>();

            base.SetBaseDependencies();
            Disable();
        }

        private void Update()
        {
            if (IsCloseToTarget() && _combatManager.CanAttack())
            {
                AttackTarget();
            }
            else if (_combatManager.CanAttack() && IsCloseToTarget())
            {
                Debug.Log(gameObject.name + " Has finished waiting: " + DateTime.Now);

                //delegatedAction(_baseAppObject);
                //_hasDelegatedTriggered = false;
            }

            //REFACTOR: If has past more than half of the attack delay, should attack and suffer with recovery delay
            //Target has moved away -> Cancel attack
            if(!IsCloseToTarget() && !_attackService.AttackIsPastHalfWay(_baseAppObject))
            {
                //Debug.Log("not close");
                _combatManager.EnableAttackerActions();
            }
        }
    
        bool IsCloseToTarget()
        {

            return _npcMovementManager.GetDistanceFromTarget() <= _equippedItensManager.GetEquippedWeapon().WeaponRange;
        }

        #endregion

        public void AttackTarget()
        {
            //set attackType
            _combatManager.SetAttackType(base._attackService.GenerateAttackTypeForNpc(_baseAppObject));
            base.InitiateAttack();
        }
    }
}
