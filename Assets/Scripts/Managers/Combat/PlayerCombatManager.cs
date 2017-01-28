using Assets.Scripts.Entities.IntelligentBodies;
using Assets.Scripts.Interfaces.Managers.Objects;
using Assets.Scripts.Interfaces.Controllers;
using Assets.Scripts.IoC;
using UnityEngine;

namespace Assets.Scripts.Managers.Combat
{

    public class PlayerCombatManager : BaseCombatManager
    {

        #region PRIVATE METHODS
        void Update()
        {
            WaitForActionDelay();
            WaitForCombatInputDelay();
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

        void WaitForCombatInputDelay()
        {
            if (_combatInputTime > 0)
            {
                TickCombatInputTime();
            }
            else
            {
                _combatController.EnableCombatIinput(gameObject);
            }
        }

        void SetCombatInputTime()
        {
            _combatInputTime = _currentTickTime + _combatInputDelayTime;
        }
        void TickCombatInputTime()
        {
            _combatInputTime -= Time.deltaTime;
        }

        void ResetCombatInputTime()
        {
            _combatInputTime = 0;
        }

        void ResetAttackSequence()
        {
            _attackSequence = 0;
        }
        #endregion

        public override void DisableAttackerActions()
        {
            _movementController.DisableMovement(gameObject);
            _combatController.DisableCombatIinput(gameObject);
        }

        public override void IncreaseSequenceWaitForAction()
        {
            _hasCastAction = true;

            if (_attackSequence == _creature.GetMaximumAttacks())
            {
                _currentTickTime = _combatController.GetAttackDelayBasedOnEquippedWeapon(gameObject, true);
                SetCombatInputTime();
                ResetAttackSequence();

                return;
            }

            _currentTickTime = _combatController.GetAttackDelayBasedOnEquippedWeapon(gameObject, false);
            SetCombatInputTime();

            ++_attackSequence;
        }

    }
}
