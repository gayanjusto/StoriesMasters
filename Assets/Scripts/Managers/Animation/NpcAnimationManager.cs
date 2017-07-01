using Assets.Scripts.Factories.AnimatorControllers;
using UnityEngine;

namespace Assets.Scripts.Managers.Animation
{
    public class NpcAnimationManager : BaseAnimationManager
    {
        protected override void Start()
        {
            base._animators = new Animator[1];
            base._animationRender = transform.Find("AnimationRender");

            var npcAnimator = _animationRender.Find("NpcAnimationRender").GetComponent<Animator>();
            base._animators[0] = npcAnimator;
            base.Start();

            //DEBUG
            //NpcAnimatorControllerFactory npcControllerFactory = new NpcAnimatorControllerFactory();
            //npcControllerFactory.Create();
        }
    }
}
