using Assets.Scripts.Interfaces.Factories.AnimatorControllers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Factories.AnimatorControllers
{
    public class PlayerHandsAnimatorControllerFactory : PlayerAnimatorControllerAbstractFactory, IAnimatorFactory
    {
        public void Create()
        {
            string bodyPart = "Hands";
            base.GenerateAnimations(bodyPart, GetAllAnimationClipsForBodyPart(bodyPart));
        }
    }
}
