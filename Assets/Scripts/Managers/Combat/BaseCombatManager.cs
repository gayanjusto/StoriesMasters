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
using Assets.Scripts.Interfaces.Observers;
using Assets.Scripts.Interfaces.Services;

namespace Assets.Scripts.Managers.Combat
{
    public class BaseCombatManager : TickTimeManager, ICombatManager
    {
        public Color originalStatusColor;

        public int _attackSequence;
        public float _combatInputTime;
        public const float _combatInputDelayTime = .1f;
        public float _timeResetAttackSequence;
        public BaseAppObject[] _lockedAttacktargets;
        public bool _isAttacking;
        public BaseAppObject _parryingTarget;

        //Player or NPC has set targets and ready for attack
        //Variable will be read by AttackObserver
        public bool _hasAttackReady;


        /// <summary>
        /// Object is parrying an attack and waiting for its final action.
        /// </summary>
        public bool _isAttemptingToParry;

        /// <summary>
        /// Defines wether an object is blocking with a shield or not
        /// </summary>
        public bool _isBlockingWithShield;

        protected BaseAppObject _appObject;
        protected IMovementController _movementController;

        protected IObjectManager _objectManager;

        protected IAttackService _attackService;
        protected IAttackTargetService _attackTargetService;
        protected IDirectionService _directionService;

        #region PRIVATE METHODS
        void Start()
        {
            _movementController = IoCContainer.GetImplementation<IMovementController>();

            _appObject = GetComponent<IObjectManager>().GetBaseAppObject();

            _attackService = IoCContainer.GetImplementation<IAttackService>();
            _attackTargetService = IoCContainer.GetImplementation<IAttackTargetService>();
            _directionService = IoCContainer.GetImplementation<IDirectionService>();

            originalStatusColor = transform.FindChild("SpriteRenderer").GetComponent<SpriteRenderer>().color;
        }

        protected void WaitForActionDelay()
        {
            if (_hasCastAction && IsWaitingFreezeTime())
            {
                TickTime();
            }
            else if(_hasCastAction && !IsWaitingFreezeTime())
            {
               
                EnableAttackerActions();
            }

            //Change color back to original (DEBUG purpose only)
            if (!_hasCastAction)
            {
                SpriteRenderer spriteRenderer = transform.FindChild("SpriteRenderer").GetComponent<SpriteRenderer>();
                spriteRenderer.color = originalStatusColor;
            }
        }

        protected void ResetAttackSequence()
        {
            _attackSequence = 0;

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

        //This is the basic algorithm for Enabling Attacker Actions
        //Further algorithms should be overriden in their own class
        public virtual void EnableAttackerActions()
        {
            //Debug.Log("Enabled attacker actions");

             ResetTickTime();
            _hasCastAction = false;
            _isAttacking = false;
            _hasAttackReady = false;

            //Set AttackObserver to false, since it's not attacking anymore
            GetComponent<IAttackObserver>().Disable();

            GetComponent<IObjectManager>().GetBaseAppObject().HasActionsPrevented = false;
            GetComponent<IMovementManager>().Enable();
        }

        public virtual void DisableAttackerActions()
        {
            GetComponent<IMovementManager>().Disable();
        }

        public virtual void DisableAttackerActions(float freezeTime)
        {
            //Set color for freezing
            SpriteRenderer spriteRenderer = transform.FindChild("SpriteRenderer").GetComponent<SpriteRenderer>();
            spriteRenderer.color = Color.blue;

            //Keep the amount of time the object will wait
            _freezeTickTime = freezeTime;

            _currentTickTime = freezeTime;
            _hasCastAction = true;
            DisableAttackerActions();
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
            //Is not waiting for anything nor has cast an action or is ready for attack
            return !IsWaitingFreezeTime() && !_hasCastAction;
        }


        public void SetTargets(BaseAppObject[] targets)
        {
            _lockedAttacktargets = targets;
        }

        public BaseAppObject[] GetTargets()
        {
            return _lockedAttacktargets;
        }

        public void SetIsAttacking(bool isAttacking)
        {
            _isAttacking = isAttacking;
        }

        public bool GetIsAttacking()
        {
            return _isAttacking;
        }

        public bool GetIsAttemptingToParry()
        {
            return _isAttemptingToParry;
        }

        public bool GetIsBlockingWithShield()
        {
            return _isBlockingWithShield;
        }

        public void SetIsBlocking(bool isBlocking)
        {
            _isBlockingWithShield = isBlocking;

            //If blocking with shield MovementObserver should allow to change direction, but not move
            GetComponent<IMovementObserver>().Enable();
        }

        public void SetParryingTarget(BaseAppObject target)
        {
            _parryingTarget = target;
            _hasCastAction = true;


            GetComponent<IMovementObserver>().Enable();
        }

        public BaseAppObject GetParryingTarget()
        {
            return _parryingTarget;
        }

        public DirectionEnum[] GetBlockingDirections()
        {
            return _directionService.GetNeighborDirections(_appObject);
        }

        public void SetAttackReady(bool isAttackReady)
        {
            _hasAttackReady = isAttackReady;

            if (_hasAttackReady)
            {
                //Set targets for attack
                _lockedAttacktargets = _attackTargetService.GetTargetsForAttack(_appObject);
                Debug.Log(_lockedAttacktargets);

                if(_lockedAttacktargets == null)
                {
                    return;
                }

                //Get time to disable actions while is casting attack
                float attackDelayTime = _attackService.GetTimeForAttackDelay(_appObject);
                DisableAttackerActions(attackDelayTime);
                Debug.Log("Attack delay time: " + attackDelayTime);

                //AttackObserver will read status and cast Attack if possible
                GetComponent<IAttackObserver>().Enable();

                //MovementObserver will read status and prevent object from moving, if possible
                GetComponent<IMovementObserver>().Enable();
            }
        }

        public bool IsReadyToAttack()
        {
            return _hasAttackReady;
        }

        public bool IsAbleToInitiateAttack()
        {
            return !_hasAttackReady && !_hasCastAction && !IsWaitingFreezeTime();
        }

        public void SetHasCastAction(bool hasCastAnAction)
        {
            _hasCastAction = hasCastAnAction;
        }

        public bool GetHasCastAction()
        {
            return _hasCastAction;
        }

        public float GetCurrentTimeForAttackDelay()
        {
            if (!_hasCastAction)
                return 0;

            return _currentTickTime;
        }

        public float GetTotalFreezeTime()
        {
            return _freezeTickTime;
        }

        public void SetIsAttemptingToParryAttack(bool isAttemptingToParry)
        {
            //REFACTOR: this boolean should return the condition of whether a defender can parry or not, using his skills against the opponent
            _isAttemptingToParry = isAttemptingToParry;
        }
    }
}
