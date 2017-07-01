using Assets.Scripts.Interfaces.Factories.AnimatorControllers;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Factories.AnimatorControllers
{
    public class PlayerFeetAnimatorControllerFactory : PlayerAnimatorControllerAbstractFactory, IAnimatorFactory
    {
        public void Create()
        {
            string bodyPart = "Feet";
            base.GenerateAnimations(bodyPart, GetAllAnimationClipsForBodyPart(bodyPart));
        }
    }
}
