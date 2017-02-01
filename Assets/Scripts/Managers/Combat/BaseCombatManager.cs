using Assets.Scripts.Interfaces.Controllers;
using Assets.Scripts.Entities.IntelligentBodies;
using Assets.Scripts.IoC;
using Assets.Scripts.Interfaces.Managers.Objects;
using Assets.Scripts.Interfaces.Managers.Combat;
using UnityEngine;

namespace Assets.Scripts.Managers.Combat
{
    public class BaseCombatManager : TickTimeManager, ICombatManager
    {
        public int _attackSequence;
        public float _combatInputTime;
        public const float _combatInputDelayTime = .1f;
        public float _timeResetAttackSequence;

        protected BaseCreature _creature;
        protected IMovementController _movementController;
        protected ICombatController _combatController;

        #region PRIVATE METHODS
        void Start()
        {
            _movementController = IoCContainer.GetImplementation<IMovementController>();
            _combatController = IoCContainer.GetImplementation<ICombatController>();

            _creature = gameObject.GetComponent<IObjectManager>().GetBaseCreature();
        }

        void WaitForActionDelay()
        {
            if (_hasCastAction && IsWaitingFreezeTime())
            {
                TickTime();
            }
            else
            {
                ResetTickTime();
                _hasCastAction = false;
                _movementController.EnableMovement(gameObject);
            }
        }

        protected void ResetAttackSequence()
        {
            _attackSequence = 0;

        }

        protected void GetTimeToResetAttackSequence()
        {
            _timeResetAttackSequence = _combatController.GetAttackDelayBasedOnEquippedWeapon(gameObject, false) + 3.0f;
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
        public virtual void DisableAttackerActions()
        {
            _movementController.DisableMovement(gameObject);
        }
        public virtual void IncreaseSequenceWaitForAction()
        {
            _hasCastAction = true;
            GetTimeToResetAttackSequence();

            if (_attackSequence == _creature.GetMaximumAttacks())
            {
                _currentTickTime = _combatController.GetAttackDelayBasedOnEquippedWeapon(gameObject, true);
                ResetAttackSequence();
                return;
            }

            _currentTickTime = _combatController.GetAttackDelayBasedOnEquippedWeapon(gameObject, false);
            ++_attackSequence;
        }

        public int GetAttackSequence()
        {
            return _attackSequence;
        }

        public bool CanAttack()
        {
            return !IsWaitingFreezeTime() || !_hasCastAction;
        }

    }
}
