using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces.Managers.Movement;
using UnityEngine;

public class AnimationManager : MonoBehaviour {

    Transform _characterSprites;
    Animator _torsoAnimatorController;
    Animator _rightArmAnimatorController;
    Animator _leftArmAnimatorController;
    Animator _headAnimatorController;
    Animator _leftLegAnimatorController;
    Animator _rightLegAnimatorController;

    Animator[] _bodyAnimators;

    IMovementManager _movementManager;
    DirectionEnum _facingDirection;

    int upHash = Animator.StringToHash("Up");
    int downHash = Animator.StringToHash("Down");
    int leftHash = Animator.StringToHash("Left");
    int rightHash = Animator.StringToHash("Right");


    // Use this for initialization
    void Start () {
        _characterSprites = transform.Find("CharacterSprites");

        _bodyAnimators = new Animator[6];

        _torsoAnimatorController = _characterSprites.Find("Torso").GetComponent<Animator>();
        _bodyAnimators[0] = _torsoAnimatorController;

        _rightArmAnimatorController = _characterSprites.Find("RightArm").GetComponent<Animator>();
        _bodyAnimators[1] = _rightArmAnimatorController;

        _leftArmAnimatorController = _characterSprites.Find("LeftArm").GetComponent<Animator>();
        _bodyAnimators[2] = _leftArmAnimatorController;

        _headAnimatorController = _characterSprites.Find("Head").GetComponent<Animator>();
        _bodyAnimators[3] = _headAnimatorController;

        _leftLegAnimatorController = _characterSprites.Find("LeftLeg").GetComponent<Animator>();
        _bodyAnimators[4] = _leftLegAnimatorController;

        _rightLegAnimatorController = _characterSprites.Find("RightLeg").GetComponent<Animator>();
        _bodyAnimators[5] = _rightLegAnimatorController;

        _movementManager = GetComponent<IMovementManager>();


    }

    private void Update()
    {
        _facingDirection = _movementManager.GetFacingDirection();

        if(_facingDirection == DirectionEnum.Up)
        {
            SetAnimationBoolDirection(upHash, true);
            return;
        }
        if (_facingDirection == DirectionEnum.Down)
        {
            SetAnimationBoolDirection(downHash, true);
            return;
        }
        if (_facingDirection == DirectionEnum.Left)
        {
            SetAnimationBoolDirection(leftHash, true);
            return;
        }
        if (_facingDirection == DirectionEnum.Right)
        {
            SetAnimationBoolDirection(rightHash, true);
            return;
        }
        if (_facingDirection == DirectionEnum.UpLeft)
        {
            SetAnimationBoolDirection(upHash, true);
            SetAnimationBoolDirection(leftHash, true);
            return;
        }
        if (_facingDirection == DirectionEnum.UpRight)
        {
            SetAnimationBoolDirection(upHash, true);
            SetAnimationBoolDirection(rightHash, true);
            return;
        }
        if (_facingDirection == DirectionEnum.DownLeft)
        {
            SetAnimationBoolDirection(downHash, true);
            SetAnimationBoolDirection(leftHash, true);
            return;
        }
        if (_facingDirection == DirectionEnum.DownRight)
        {
            SetAnimationBoolDirection(downHash, true);
            SetAnimationBoolDirection(rightHash, true);
            return;
        }
    }

    void SetAnimationBoolDirection(int direction, bool value)
    {
        for (int i = 0; i < _bodyAnimators.Length; i++)
        {
            _bodyAnimators[0].SetBool(direction, value);
        }
    }

}
