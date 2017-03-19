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

namespace Assets.Scripts.Managers.Combat
{
    public class BaseCombatManager : TickTimeManager, ICombatManager
    {
        public int _attackSequence;
        public float _combatInputTime;
        public const float _combatInputDelayTime = .1f;
        public float _timeResetAttackSequence;
        public BaseAppObject[] _targets;
        public bool _isAttacking;
        public BaseAppObject _parryingTarget;



        /// <summary>
        /// Object is defending an attack and waiting for its final action.
        /// </summary>
        public bool _isDefending;

        /// <summary>
        /// Defines wether an object is blocking with a shield or not
        /// </summary>
        public bool _isBlocking;

        protected BaseAppObject _appObject;
        protected IMovementController _movementController;
        protected ICombatController _combatController;

        #region PRIVATE METHODS
        void Start()
        {
            _movementController = IoCContainer.GetImplementation<IMovementController>();
            _combatController = IoCContainer.GetImplementation<ICombatController>();

            _appObject = GetComponent<IObjectManager>().GetBaseAppObject();
        }

        protected void WaitForActionDelay()
        {
            if (_hasCastAction && IsWaitingFreezeTime())
            {
                TickTime();
            }
            else if(!_hasCastAction)
            {
                EnableAttackerActions();
            }
        }

        protected void ResetAttackSequence()
        {
            _attackSequence = 0;

        }

        protected void GetTimeToResetAttackSequence()
        {
            _timeResetAttackSequence = _combatController.GetAttackDelayBasedOnEquippedWeapon(GetComponent<IObjectManager>().GetBaseAppObject(), false) + 3.0f;
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
            _movementController.EnableMovement(gameObject);
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
            return !IsWaitingFreezeTime() || !_hasCastAction;
        }

        public bool GetHasCastAction()
        {
            return _hasCastAction;
        }

        public void SetTargets(BaseAppObject[] targets)
        {
            _targets = targets;
        }

        public BaseAppObject[] GetTargets()
        {
            return _targets;
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

        public bool GetIsBlocking()
        {
            return _isBlocking;
        }

        public void SetIsBlocking(bool isBlocking)
        {
            _hasCastAction = isBlocking;
            _isBlocking = isBlocking;
        }

        public void SetParryingTarget(BaseAppObject target)
        {
            _parryingTarget = target;
        }

        public BaseAppObject GetParryingTarget()
        {
            return _parryingTarget;
        }

        public DirectionEnum[] GetBlockingDirections()
        {
            return _movementController.GetNeighboringDirections(_appObject);
        }
    }
}
