using Assets.Scripts.Interfaces.Managers.Inputs;
using UnityEngine;

namespace Assets.Scripts.Managers.Combat
{
    public class PlayerCombatManager : BaseCombatManager
    {

        #region PRIVATE METHODS
        void Update()
        {
            WaitForActionDelay();
            CheckIfAttackSequenceIsValid();
            WaitForCombatInputDelay();
        }

        //<summary>
        //Methods used to enable combat input after an action
        //</summary>
        void WaitForCombatInputDelay()
        {
            if (_combatInputTime > 0)
            {
                TickCombatInputTime();
            }
            else if(!GetComponent<IPlayerCombatInputManager>().IsEnabled())
            {
                _combatController.EnableCombatInput(gameObject);
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
        #endregion

        public override void DisableAttackerActions()
        {
            _movementController.DisableMovement(gameObject);
            _combatController.DisableCombatInput(gameObject);
        }

        public override void IncreaseSequenceWaitForAction()
        {
            _hasCastAction = true;
            GetTimeToResetAttackSequence();

            if (_attackSequence == _appObject.GetMaximumAttacks())
            {
                _currentTickTime = _combatController.GetAttackDelayBasedOnEquippedWeapon(_appObject, true);
                SetCombatInputTime();
                ResetAttackSequence();

                return;
            }

            _currentTickTime = _combatController.GetAttackDelayBasedOnEquippedWeapon(_appObject, false);
            SetCombatInputTime();

            ++_attackSequence;
        }

    }
}
