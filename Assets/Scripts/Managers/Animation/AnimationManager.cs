using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces.Managers.Movement;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{

    Transform _animationRender;
    Animator _torsoAnimatorController;
    Animator _armsAnimatorController;
    Animator _headAnimatorController;
    Animator _handsAnimatorController;
    Animator _legsAnimatorController;
    Animator _feetAnimatorController;

    Animator[] _bodyAnimators;

    IFacingDirection _facingDirection;
    DirectionEnum _currentFacingDirection;
    DirectionEnum _previousFacingDirection;


    int upHash = Animator.StringToHash("Up");
    int downHash = Animator.StringToHash("Down");
    int leftHash = Animator.StringToHash("Left");
    int rightHash = Animator.StringToHash("Right");


    // Use this for initialization
    void Start()
    {
        _animationRender = transform.Find("AnimationRender");

        _bodyAnimators = new Animator[6];

        _torsoAnimatorController = _animationRender.Find("Torso").GetComponent<Animator>();
        _bodyAnimators[0] = _torsoAnimatorController;

        _handsAnimatorController = _animationRender.Find("Hands").GetComponent<Animator>();
        _bodyAnimators[1] = _handsAnimatorController;

        _armsAnimatorController = _animationRender.Find("Arms").GetComponent<Animator>();
        _bodyAnimators[2] = _armsAnimatorController;

        _headAnimatorController = _animationRender.Find("Head").GetComponent<Animator>();
        _bodyAnimators[3] = _headAnimatorController;

        _legsAnimatorController = _animationRender.Find("Legs").GetComponent<Animator>();
        _bodyAnimators[4] = _legsAnimatorController;

        _feetAnimatorController = _animationRender.Find("Feet").GetComponent<Animator>();
        _bodyAnimators[5] = _feetAnimatorController;

        _facingDirection = GetComponent<IFacingDirection>();
    }

    private void Update()
    {
        _currentFacingDirection = _facingDirection.GetFacingDirection();

        if (_currentFacingDirection == DirectionEnum.Up && _previousFacingDirection != _currentFacingDirection)
        {
            SetAnimationBoolDirection(upHash, true);
            DisableOtherAnimationDirection(new[] { downHash, leftHash, rightHash });

            _previousFacingDirection = _currentFacingDirection;
            return;
        }
        if (_currentFacingDirection == DirectionEnum.Down && _previousFacingDirection != _currentFacingDirection)
        {
            SetAnimationBoolDirection(downHash, true);
            DisableOtherAnimationDirection(new[] { upHash, leftHash, rightHash });

            _previousFacingDirection = _currentFacingDirection;
            return;
        }
        if (_currentFacingDirection == DirectionEnum.Left && _previousFacingDirection != _currentFacingDirection)
        {
            SetAnimationBoolDirection(leftHash, true);
            DisableOtherAnimationDirection(new[] { upHash, downHash, rightHash });

            _previousFacingDirection = _currentFacingDirection;
            return;
        }
        if (_currentFacingDirection == DirectionEnum.Right && _previousFacingDirection != _currentFacingDirection)
        {
            SetAnimationBoolDirection(rightHash, true);
            DisableOtherAnimationDirection(new[] { upHash, downHash, leftHash });

            _previousFacingDirection = _currentFacingDirection;
            return;
        }
        if (_currentFacingDirection == DirectionEnum.UpLeft && _previousFacingDirection != _currentFacingDirection)
        {
            SetAnimationBoolDirection(upHash, true);
            SetAnimationBoolDirection(leftHash, true);

            DisableOtherAnimationDirection(new[] { downHash, rightHash });
            _previousFacingDirection = _currentFacingDirection;
            return;
        }
        if (_currentFacingDirection == DirectionEnum.UpRight && _previousFacingDirection != _currentFacingDirection)
        {
            SetAnimationBoolDirection(upHash, true);
            SetAnimationBoolDirection(rightHash, true);

            DisableOtherAnimationDirection(new[] { downHash, leftHash });
            _previousFacingDirection = _currentFacingDirection;
            return;
        }
        if (_currentFacingDirection == DirectionEnum.DownLeft && _previousFacingDirection != _currentFacingDirection)
        {
            SetAnimationBoolDirection(downHash, true);
            SetAnimationBoolDirection(leftHash, true);

            DisableOtherAnimationDirection(new[] { upHash, rightHash });
            _previousFacingDirection = _currentFacingDirection;
            return;
        }
        if (_currentFacingDirection == DirectionEnum.DownRight && _previousFacingDirection != _currentFacingDirection)
        {
            SetAnimationBoolDirection(downHash, true);
            SetAnimationBoolDirection(rightHash, true);

            DisableOtherAnimationDirection(new[] { upHash, leftHash });
            _previousFacingDirection = _currentFacingDirection;
            return;
        }
    }

    void SetAnimationBoolDirection(int direction, bool value)
    {
        for (int i = 0; i < _bodyAnimators.Length; i++)
        {
            _bodyAnimators[i].SetBool(direction, value);
        }
    }

  

    void DisableOtherAnimationDirection(int[] directionsToDisable)
    {
        for (int i = 0; i < _bodyAnimators.Length; i++)
        {
            for (int x = 0; x < directionsToDisable.Length; x++)
            {
                _bodyAnimators[i].SetBool(directionsToDisable[x], false);
            }
        }
    }
}
