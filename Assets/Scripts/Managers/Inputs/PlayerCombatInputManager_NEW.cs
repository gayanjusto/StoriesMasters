using System;
using Assets.Scripts.Interfaces.Managers.Inputs;
using Assets.Scripts.Interfaces.Services;
using Assets.Scripts.IoC;
using Assets.Scripts.Managers.Combat;
using UnityEngine;

namespace Assets.Scripts.Managers.Inputs
{
    public class PlayerCombatInputManager_NEW : CombatEntryPointManager, IPlayerCombatInputManager
    {
        public float _timeDefenseInputPressed;

        private void Start()
        {
            base.SetBaseDependencies();
        }

        private void Update()
        {
            //will check only if not waiting for an action or suffering an action
            if (_combatManager.CanAttack())
            {
                CheckAttackInput();
                CheckDefenseInput();
            }
        }

        bool CheckAttackInput()
        {
            if (Input.GetKey(KeyCode.E))
            {
                base.InitiateAttack();
                return true;
            }

            return false;
        }

        void CheckDefenseInput()
        {
            if (Input.GetKey(KeyCode.Q))
            {
                this._timeDefenseInputPressed += Time.deltaTime;

                if (HasHoldInputForShieldBlock())
                {
                    base.StartShieldBlock();
                }
                else if (HasHoldInputForParry())
                {
                    //To parry an attack, defender must be facing the attacker
                    //and attacker must be acting his attack
                    base.StartParryingBlock();
                }
            }

            if (Input.GetKeyUp(KeyCode.Q))
            {
                _timeDefenseInputPressed = 0;
                base.CancelBlocking();
            }
        }

        bool HasHoldInputForParry()
        {
            return _timeDefenseInputPressed <= 0.3f;
        }

        bool HasHoldInputForShieldBlock()
        {
            return _timeDefenseInputPressed >= 0.3f;
        }
    }
}
