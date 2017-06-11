using System;
using Assets.Scripts.Interfaces.Managers.Inputs;
using Assets.Scripts.Interfaces.Services;
using Assets.Scripts.IoC;
using Assets.Scripts.Managers.Combat;
using UnityEngine;

namespace Assets.Scripts.Managers.Inputs
{
    public class PlayerCombatInputManager : CombatEntryPointManager, IPlayerCombatInputManager
    {
        public float _timeDefenseInputPressed;
        public float _timeAttackInputPressed;

        public float _timeForParryInput;
        public float _timeToCancelBlockInput;


        IAttackTypeService _attackTypeService;

        private void Start()
        {
            base.SetBaseDependencies();

            _attackTypeService = IoCContainer.GetImplementation<IAttackTypeService>();
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
                _timeAttackInputPressed += Time.deltaTime;
            }

            if (Input.GetKeyUp(KeyCode.E))
            {
                StartAttack();
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
                    return;
                }
            }

            if (Input.GetKeyUp(KeyCode.Q))
            {
                if (HasHoldInputForParry())
                {
                    //To parry an attack, defender must be facing the attacker
                    //and attacker must be acting his attack
                    if (!base.StartParryingBlock())
                    {
                        Reset();
                    }
                }
                else
                {
                    Reset();
                    base.CancelBlocking();
                }

            }
        }

        bool HasHoldInputForParry()
        {
            return _timeDefenseInputPressed >= _timeToCancelBlockInput && _timeDefenseInputPressed <= _timeForParryInput;
        }

        bool HasHoldInputForShieldBlock()
        {
            return _timeDefenseInputPressed >= _timeForParryInput;
        }

        void StartAttack()
        {
            _attackTypeService.SetAttackTypeForPlayer(base._objectManager.GetBaseAppObject(), _timeAttackInputPressed);
            base.InitiateAttack();
            _timeAttackInputPressed = 0;
        }

        public void Reset()
        {
            _timeAttackInputPressed = 0;
            _timeDefenseInputPressed = 0;
        }
    }
}
