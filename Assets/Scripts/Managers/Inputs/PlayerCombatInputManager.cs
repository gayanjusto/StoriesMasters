using Assets.Scripts.Entities.ApplicationObjects;
using Assets.Scripts.Interfaces.Controllers;
using Assets.Scripts.Interfaces.Managers.Combat;
using Assets.Scripts.Interfaces.Managers.Inputs;
using Assets.Scripts.Interfaces.Managers.Movement;
using Assets.Scripts.Interfaces.Managers.Objects;
using Assets.Scripts.IoC;
using Assets.Scripts.Managers.Combat;
using System;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts.Managers.Inputs
{
    public class PlayerCombatInputManager : CombatEntryPointManager, IPlayerCombatInputManager, IAttackTiming
    {
        public float _holdingDefenseInputTime;
        public bool _isHoldingDefenseInput;
        private IMovementManager _movementManager;

        private const float _parryInputTime = 0.3f;

        #region PRIVATE METHODS
        void Start()
        {
            _combatManager = GetComponent<ICombatManager>();
            _objectManager = GetComponent<IObjectManager>();
            _movementManager = GetComponent<IMovementManager>();
            _combatController = IoCContainer.GetImplementation<ICombatController>();
        }
        void Update()
        {

            CheckHoldingDefenseInput();

            if (!_hasDelegatedTriggered && _combatManager.CanAttack())
            {
                CheckDefenseKeyPress();
                CheckAttackKeyPress();
            }
            else if (_hasDelegatedTriggered && _combatManager.CanAttack())
            {
                Debug.Log("Has finished waiting: " + DateTime.Now);
                delegatedAction(_objectManager.GetBaseAppObject());
                _hasDelegatedTriggered = false;
            }
        }

        void CheckDefenseKeyPress()
        {
            if (Input.GetKey(KeyCode.Q) && _holdingDefenseInputTime >= _parryInputTime)
            {
                _combatManager.SetIsBlocking(true);
                _movementManager.Disable();
                Debug.Log("Desabilitou movimentos blocking escudo");
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                _isHoldingDefenseInput = true;

                Debug.Log("Defendeu");
            }
            else if (Input.GetKeyUp(KeyCode.Q))
            {
                if (_holdingDefenseInputTime <= _parryInputTime)
                {
                    _combatController.ParryAttack(_objectManager.GetBaseAppObject());
                }

                _isHoldingDefenseInput = false;

                _combatManager.SetIsBlocking(false);
            }
        }

        void CheckAttackKeyPress()
        {
            if (Input.GetKey(KeyCode.E))
            {
                base.InitiateAttack();
            }
        }

        void CheckHoldingDefenseInput()
        {
            if (_isHoldingDefenseInput)
            {
                UpdateDefenseInputTime();
            }
            else
            {
                ResetDefenseInputTime();
            }
        }

        void UpdateDefenseInputTime()
        {
            _holdingDefenseInputTime += Time.deltaTime;
        }

        void ResetDefenseInputTime()
        {
            _holdingDefenseInputTime = 0;
        }

        #endregion

        public void WaitDelayAfterAttack(float delayTime)
        {

            _combatManager.DisableAttackerActions(delayTime);
        }
    }
}
