using Assets.Scripts.Interfaces.Controllers;
using Assets.Scripts.Entities.IntelligentBodies;
using Assets.Scripts.IoC;
using Assets.Scripts.Interfaces.Managers.Objects;
using Assets.Scripts.Interfaces.Managers.Combat;

namespace Assets.Scripts.Managers.Combat
{
    public class BaseCombatManager : TickTimeManager, ICombatManager
    {
        protected int _attackSequence;
        protected float _combatInputTime;
        protected const float _combatInputDelayTime = .05f;
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


        void ResetAttackSequence()
        {
            _attackSequence = 0;

        }
        #endregion
        public virtual void DisableAttackerActions()
        {
            _movementController.DisableMovement(gameObject);
        }
        public virtual void IncreaseSequenceWaitForAction()
        {
            _hasCastAction = true;

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
