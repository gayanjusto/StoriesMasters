using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Assets.Scripts.Interfaces.Factories;
using Assets.Scripts.Interfaces.Factories.AnimatorControllers;

namespace Assets.Scripts.Factories.AnimatorControllers
{
    public class PlayerArmsAnimatorControllerFactory : PlayerAnimatorControllerAbstractFactory, IAnimatorFactory
    {
        public void Create()
        {
            string bodyPart = "Arms";

            base.GenerateAnimations(bodyPart, GetAllAnimationClipsForBodyPart(bodyPart));
        }
    }
}
