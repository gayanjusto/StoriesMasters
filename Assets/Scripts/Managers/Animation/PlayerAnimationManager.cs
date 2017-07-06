using Assets.Scripts.Factories.AnimatorControllers;
using Assets.Scripts.Interfaces.Managers.Movement;
using Assets.Scripts.Managers.Animation;
using UnityEngine;

public class PlayerAnimationManager : BaseAnimationManager
{

    //Animator _torsoAnimatorController;
    Animator _armsAnimatorController;
    //Animator _headAnimatorController;
    //Animator _handsAnimatorController;
    //Animator _legsAnimatorController;
    //Animator _feetAnimatorController;

 

    // Use this for initialization
    protected override void Start()
    {
        //DEBUG
        //PlayerArmsAnimatorControllerFactory testeArms = new PlayerArmsAnimatorControllerFactory();
        //testeArms.Create();
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

        base._animationRender = transform.Find("AnimationRender");

        _animators = new Animator[1];

        //_torsoAnimatorController = _animationRender.Find("Torso").GetComponent<Animator>();
        //_animators[0] = _torsoAnimatorController;

        //_handsAnimatorController = _animationRender.Find("Hands").GetComponent<Animator>();
        //_animators[1] = _handsAnimatorController;

        _armsAnimatorController = _animationRender.Find("Arms").GetComponent<Animator>();
        _animators[0] = _armsAnimatorController;

        //_headAnimatorController = _animationRender.Find("Head").GetComponent<Animator>();
        //_animators[3] = _headAnimatorController;

        //_legsAnimatorController = _animationRender.Find("Legs").GetComponent<Animator>();
        //_animators[4] = _legsAnimatorController;

        //_feetAnimatorController = _animationRender.Find("Feet").GetComponent<Animator>();
        //_animators[5] = _feetAnimatorController;

        base.Start();
    }
}
