using Assets.Scripts.Interfaces.Managers.Inputs;
using Assets.Scripts.Interfaces.Managers.Movement;
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


        //public override void IncreaseSequenceWaitForAction()
        //{
        //    _hasCastAction = true;

        //    //REFACTOR: What does this do???
        //    base._timeResetAttackSequence = GetTimeToResetAttackSequence();

        //    //Player has reached the maximum of his attack sequence
        //    if (_attackSequence == _appObject.GetMaximumAttacks())
        //    {
        //        //Set time to wait after sequence
        //        _currentTickTime = _combatController.GetAttackDelayBasedOnEquippedWeapon(_appObject, true);
        //        SetCombatInputTime();


        //        ResetAttackSequence();

        //        return;
        //    }

        //    //Set time to wait after sequence
        //    _currentTickTime = _combatController.GetAttackDelayBasedOnEquippedWeapon(_appObject, false);
        //    SetCombatInputTime();

        //    ++_attackSequence;
        //}

        public override void EnableAttackerActions()
        {
            base.EnableAttackerActions();

            GetComponent<IPlayerCombatInputManager>().Enable();
            base.SetIsBlocking(false);
        }

        public override void DisableAttackerActions()
        {
            base.DisableAttackerActions();
            IPlayerCombatInputManager combatInputManager = GetComponent<IPlayerCombatInputManager>();
            combatInputManager.Reset();
            combatInputManager.Disable();
        }

    }
}
