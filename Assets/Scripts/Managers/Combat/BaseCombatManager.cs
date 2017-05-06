using Assets.Scripts.Interfaces.Controllers;
using Assets.Scripts.Entities.IntelligentBodies;
using Assets.Scripts.IoC;
using Assets.Scripts.Interfaces.Managers.Objects;
using Assets.Scripts.Interfaces.Managers.Combat;
using UnityEngine;
using System;
using Assets.Scripts.Entities.ApplicationObjects;
using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces.Managers.Movement;
using Assets.Scripts.Interfaces.Observers;
using Assets.Scripts.Interfaces.Services;

namespace Assets.Scripts.Managers.Combat
{
    public class BaseCombatManager : TickTimeManager, ICombatManager
    {
        public int _attackSequence;
        public float _combatInputTime;
        public const float _combatInputDelayTime = .1f;
        public float _timeResetAttackSequence;
        public BaseAppObject[] _lockedAttacktargets;
        public bool _isAttacking;
        public BaseAppObject _parryingTarget;

        //Player or NPC has set targets and ready for attack
        //Variable will be read by AttackObserver
        public bool _hasAttackReady;


        /// <summary>
        /// Object is defending an attack and waiting for its final action.
        /// </summary>
        public bool _isDefending;

        /// <summary>
        /// Defines wether an object is blocking with a shield or not
        /// </summary>
        public bool _isBlockingWithShield;

        protected BaseAppObject _appObject;
        protected IMovementController _movementController;
        protected ICombatController _combatController;

        protected IObjectManager _objectManager;

        protected IAttackService _attackService;
        protected IAttackTargetService _attackTargetService;


        #region PRIVATE METHODS
        void Start()
        {
            _movementController = IoCContainer.GetImplementation<IMovementController>();
            _combatController = IoCContainer.GetImplementation<ICombatController>();

            _appObject = GetComponent<IObjectManager>().GetBaseAppObject();

            _attackService = IoCContainer.GetImplementation<IAttackService>();
            _attackTargetService = IoCContainer.GetImplementation<IAttackTargetService>();
        }

        protected void WaitForActionDelay()
        {
            if (_hasCastAction && IsWaitingFreezeTime())
            {
                Debug.Log(_freezeTickTime);
                TickTime();
            }
            else if(_hasCastAction)
            {
                EnableAttackerActions();
            }
        }

        protected void ResetAttackSequence()
        {
            _attackSequence = 0;

        }

        protected float GetTimeToResetAttackSequence()
        {
            _timeResetAttackSequence = _combatController.GetAttackDelayBasedOnEquippedWeapon(GetComponent<IObjectManager>().GetBaseAppObject(), false) + 3.0f;

            return _timeResetAttackSequence;
        }

        protected bool IsInAttackSequence()
        {
            return _timeResetAttackSequence > 0;
        }

        protected void TickAttackSequenceTime()
        {
            _timeResetAttackSequence -= Time.deltaTime;
        }

        protected void CheckIfAttackSequenceIsValid()
        {
            if (IsInAttackSequence())
            {
                TickAttackSequenceTime();
            }
            else if (_attackSequence > 0)
            {
                ResetAttackSequence();
            }
        }
        #endregion

        public virtual void EnableAttackerActions()
        {
             ResetTickTime();
            _hasCastAction = false;
            _isAttacking = false;
            _hasAttackReady = false;
        }

        public virtual void DisableAttackerActions()
        {
            _movementController.DisableMovement(gameObject);
        }

        public virtual void DisableAttackerActions(float freezeTime)
        {
            _currentTickTime = freezeTime;
            _hasCastAction = true;
            DisableAttackerActions();
        }

        public virtual void IncreaseSequenceWaitForAction()
        {
            _hasCastAction = true;
            GetTimeToResetAttackSequence();

            if (_attackSequence == _appObject.GetMaximumAttacks())
            {
                _currentTickTime = _combatController.GetAttackDelayBasedOnEquippedWeapon(_appObject, true);
                ResetAttackSequence();
                return;
            }

            _currentTickTime = _combatController.GetAttackDelayBasedOnEquippedWeapon(_appObject, false);
            ++_attackSequence;
        }

        public int GetAttackSequence()
        {
            return _attackSequence;
        }

        /// <summary>
        /// Is not waiting for any action that prevents object to attack.
        /// </summary>
        /// <returns></returns>
        public bool CanAttack()
        {
            //Is not waiting for anything nor has cast an action or is ready for attack
            return !IsWaitingFreezeTime() || (!_hasCastAction && !_hasAttackReady);
        }


        public void SetTargets(BaseAppObject[] targets)
        {
            _lockedAttacktargets = targets;
        }

        public BaseAppObject[] GetTargets()
        {
            return _lockedAttacktargets;
        }

        public void SetIsAttacking(bool isAttacking)
        {
            _isAttacking = isAttacking;
        }

        public bool GetIsAttacking()
        {
            return _isAttacking;
        }

        public bool GetIsDefending()
        {
            return _isDefending;
        }

        public void SetIsDefending(bool isDefending)
        {
            _isDefending = isDefending;
        }

        public bool GetIsBlockingWithShield()
        {
            return _isBlockingWithShield;
        }

        public void SetIsBlocking(bool isBlocking)
        {
            _isBlockingWithShield = isBlocking;

            //If blocking with shield MovementObserver should allow to change direction, but not move
            GetComponent<IMovementObserver>().Enable();
        }

        public void SetParryingTarget(BaseAppObject target)
        {
            _parryingTarget = target;
            _hasCastAction = true;

            GetComponent<IMovementObserver>().Enable();
        }

        public BaseAppObject GetParryingTarget()
        {
            return _parryingTarget;
        }

        public DirectionEnum[] GetBlockingDirections()
        {
            return _movementController.GetNeighboringDirections(_appObject);
        }

        public void SetAttackReady(bool isAttackReady)
        {
            _hasAttackReady = isAttackReady;

            if (_hasAttackReady)
            {
                //Set targets for attack
                _lockedAttacktargets = _attackTargetService.GetTargetsForAttack(_appObject);
                Debug.Log(_lockedAttacktargets);

                //Get time to disable actions while is attacking
                float attackDelayTime = _attackService.GetTimeForAttackDelay(_appObject);
                DisableAttackerActions(attackDelayTime);

                //AttackObserver will read status and cast Attack if possible
                GetComponent<IAttackObserver>().Enable();

                //MovementObserver will read status and prevent object from moving, if possible
                GetComponent<IMovementObserver>().Enable();
            }
        }

        public bool IsReadyToAttack()
        {
            return _hasAttackReady;
        }

        public bool IsAbleToInitiateAttack()
        {
            return !_hasAttackReady && !_hasCastAction && !IsWaitingFreezeTime();
        }

        public void SetHasCastAction(bool hasCastAnAction)
        {
            _hasCastAction = hasCastAnAction;
        }

        public bool GetHasCastAction()
        {
            return _hasCastAction;
        }
    }
}
