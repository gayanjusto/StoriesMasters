using Assets.Scripts.Enums;
using Assets.Scripts.Factories.AnimatorControllers;
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

    IMovementManager _movementManager;
    IFacingDirection _facingDirection;

    //Direction
    int directionParameterId = Animator.StringToHash("Direction");
    DirectionEnum _currentFacingDirection;
    DirectionEnum _previousFacingDirection;

    //Walking
    int walkingParameterId = Animator.StringToHash("Walking");
    bool _previousWalkingCondition;

    // Use this for initialization
    void Start()
    {
        //DEBUG
        PlayerArmsAnimatorControllerFactory testeArms = new PlayerArmsAnimatorControllerFactory();
        testeArms.Create();
        //PlayerFeetAnimatorControllerFactory testeFeet = new PlayerFeetAnimatorControllerFactory();
        //testeFeet.Create();
        //PlayerHandsAnimatorControllerFactory testeHands = new PlayerHandsAnimatorControllerFactory();
        //testeHands.Create();
        //PlayerHeadAnimatorControllerFactory testeHead = new PlayerHeadAnimatorControllerFactory();
        //testeHead.Create();
        //PlayerLegsAnimatorControllerFactory testeLegs = new PlayerLegsAnimatorControllerFactory();
        //testeLegs.Create();
        //PlayerTorsoAnimatorControllerFactory testeTorso = new PlayerTorsoAnimatorControllerFactory();
        //testeTorso.Create();

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
        _movementManager = GetComponent<IMovementManager>();
    }

    private void Update()
    {
        if (_movementManager.IsMoving() != _previousWalkingCondition)
        {
            
            SetAnimationWalkingValue(_movementManager.IsMoving());
            _previousWalkingCondition = _movementManager.IsMoving();
        }
       
        _currentFacingDirection = _facingDirection.GetFacingDirection();

        if(_currentFacingDirection != _previousFacingDirection)
        {
            SetAnimationDirectionValue(_currentFacingDirection.GetHashCode());
            _previousFacingDirection = _currentFacingDirection;
            return;
        }
    }

    void SetAnimationDirectionValue(int value)
    {
        for (int i = 0; i < _bodyAnimators.Length; i++)
        {
            _bodyAnimators[i].SetInteger(directionParameterId, value);
        }
    }

    void SetAnimationWalkingValue(bool value)
    {
        for (int i = 0; i < _bodyAnimators.Length; i++)
        {
            _bodyAnimators[i].SetBool(walkingParameterId, value);
        }
    }

}
