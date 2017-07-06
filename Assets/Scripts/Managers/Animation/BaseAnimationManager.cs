using System;
using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces.Managers;
using Assets.Scripts.Interfaces.Managers.Combat;
using Assets.Scripts.Interfaces.Managers.Movement;
using UnityEngine;

namespace Assets.Scripts.Managers.Animation
{
    public class BaseAnimationManager : BaseMonoBehaviour, IBaseAnimationManager
    {
        protected Transform _animationRender;

        protected Animator[] _animators;

        protected IMovementManager _movementManager;
        protected IFacingDirection _facingDirection;
        protected IDefenseStatusManager _defenseStatus;
        protected ICombatManager _combatManager;


        //Direction
        protected int directionParameterId = Animator.StringToHash("Direction");
        protected DirectionEnum _currentFacingDirection;
        protected DirectionEnum _previousFacingDirection;

        //Walking
        protected int walkingParameterId = Animator.StringToHash("Walking");
        protected bool _previousWalkingCondition;
        public bool _currentMovingCondition;

        //Running 
        protected int runningParameterId = Animator.StringToHash("Running");

        //Defending
        protected int defendingParameterId = Animator.StringToHash("Defending");
        protected bool _previousDefendingCondition;
        public bool _currentDefendingCondition;

        //DefenseType
        protected int defenseTypeParameterId = Animator.StringToHash("DefenseType");

        //Attacking
        protected int attackingParameterId = Animator.StringToHash("Attacking");
        public bool _currentAttackingCondition;

        //Attack Sequence
        protected int attackSequenceParameterId = Animator.StringToHash("AttackSequence");

        //Attack
        protected int attackTypeParameterId = Animator.StringToHash("AttackType");

        //Combat Active
        protected int combatActiveParameterId = Animator.StringToHash("CombatActive");
        public bool _currentCombatActiveCondition;
        public bool _previousCombatActiveCondition;


        protected virtual void Start()
        {
            _facingDirection = GetComponent<IFacingDirection>();
            _movementManager = GetComponent<IMovementManager>();
            _defenseStatus = GetComponent<IDefenseStatusManager>();
            _combatManager = GetComponent<ICombatManager>();
        }

        protected void Update()
        {
            _currentMovingCondition = _movementManager.IsMoving();
            _currentDefendingCondition = _defenseStatus.IsDefending();
            _currentFacingDirection = _facingDirection.GetFacingDirection();
            _currentCombatActiveCondition = _combatManager.IsCombatActive();

            SetDefendingParameters();
            SetWalkingParamenters();
            SetDirectionParameters();
            SetCombatActiveParameters();
        }


        protected void SetDefendingParameters()
        {
            SetIntParamValue(defenseTypeParameterId, _defenseStatus.CurrentDefenseType().GetHashCode());

            if (_currentDefendingCondition != _previousDefendingCondition)
            {
                SetBoolParamValue(defendingParameterId, _currentDefendingCondition);

                //Object cannot defend and move at the same time
                if (_currentMovingCondition == true)
                {
                    SetBoolParamValue(walkingParameterId, false);
                    SetBoolParamValue(runningParameterId, false);
                    SetIntParamValue(defenseTypeParameterId, 0);
                }

                _previousDefendingCondition = _currentDefendingCondition;
            }
        }

        protected void SetWalkingParamenters()
        {
            //Cannot walk and defend at the same time
            if (_currentMovingCondition != _previousWalkingCondition && _currentDefendingCondition == false)
            {
                SetBoolParamValue(walkingParameterId, _currentMovingCondition);
                _previousWalkingCondition = _currentMovingCondition;
            }
        }

        protected void SetDirectionParameters()
        {
            if (_currentFacingDirection != _previousFacingDirection)
            {
                SetIntParamValue(directionParameterId, _currentFacingDirection.GetHashCode());
                _previousFacingDirection = _currentFacingDirection;
                return;
            }
        }

        protected void SetCombatActiveParameters()
        {
            if (_currentCombatActiveCondition != _previousCombatActiveCondition)
            {
                SetBoolParamValue(combatActiveParameterId, _currentCombatActiveCondition);
            }
        }

        protected void SetBoolParamValue(int paramId, bool value)
        {
            for (int i = 0; i < _animators.Length; i++)
            {
                _animators[i].SetBool(paramId, value);
            }
        }

        protected void SetIntParamValue(int paramId, int value)
        {
            for (int i = 0; i < _animators.Length; i++)
            {
                _animators[i].SetInteger(paramId, value);

            }
        }

        protected void SetTriggerParamValue(int paramId)
        {
            for (int i = 0; i < _animators.Length; i++)
            {
                _animators[i].SetTrigger(paramId);

            }
        }

        public void PlayAttackAnimation(int attackSequence, AttackTypeEnum attackType)
        {
            SetIntParamValue(attackSequenceParameterId, attackSequence);
            SetIntParamValue(attackTypeParameterId, attackType.GetHashCode());
            SetTriggerParamValue(attackingParameterId);
        }
    }
}
