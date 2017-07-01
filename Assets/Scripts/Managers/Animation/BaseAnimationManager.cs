using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces.Managers.Movement;
using UnityEngine;

namespace Assets.Scripts.Managers.Animation
{
    public class BaseAnimationManager : BaseMonoBehaviour
    {
        protected Transform _animationRender;

        protected Animator[] _animators;

        protected IMovementManager _movementManager;
        protected IFacingDirection _facingDirection;

        //Direction
        protected int directionParameterId = Animator.StringToHash("Direction");
        protected DirectionEnum _currentFacingDirection;
        protected DirectionEnum _previousFacingDirection;

        //Walking
        protected int walkingParameterId = Animator.StringToHash("Walking");
        protected bool _previousWalkingCondition;
        protected bool _currentMovingCondition;

        protected virtual void Start()
        {
            _facingDirection = GetComponent<IFacingDirection>();
            _movementManager = GetComponent<IMovementManager>();
        }

        protected void Update()
        {
            _currentMovingCondition = _movementManager.IsMoving();
            if (_currentMovingCondition != _previousWalkingCondition)
            {
                SetAnimationWalkingValue(_currentMovingCondition);
                _previousWalkingCondition = _currentMovingCondition;
            }

            _currentFacingDirection = _facingDirection.GetFacingDirection();

            if (_currentFacingDirection != _previousFacingDirection)
            {
                SetAnimationDirectionValue(_currentFacingDirection.GetHashCode());
                _previousFacingDirection = _currentFacingDirection;
                return;
            }
        }

        protected void SetAnimationDirectionValue(int value)
        {
            for (int i = 0; i < _animators.Length; i++)
            {
                _animators[i].SetInteger(directionParameterId, value);
            }
        }

        protected void SetAnimationWalkingValue(bool value)
        {
            for (int i = 0; i < _animators.Length; i++)
            {
                _animators[i].SetBool(walkingParameterId, value);
            }
        }
    }
}
