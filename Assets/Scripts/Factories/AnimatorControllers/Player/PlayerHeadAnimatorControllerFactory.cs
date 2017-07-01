using Assets.Scripts.Interfaces.Factories.AnimatorControllers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Factories.AnimatorControllers
{
    public class PlayerHeadAnimatorControllerFactory : PlayerAnimatorControllerAbstractFactory, IAnimatorFactory
    {
        public void Create()
        {
            string bodyPart = "Head";
            base.GenerateAnimations(bodyPart, GetAllAnimationClipsForBodyPart(bodyPart));
        }
    }
}
