using Assets.Scripts.Interfaces.Factories.AnimatorControllers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Factories.AnimatorControllers
{
    public class PlayerTorsoAnimatorControllerFactory : PlayerAnimatorControllerAbstractFactory, IAnimatorFactory
    {
        public void Create()
        {
            string bodyPart = "Torso";
            base.GenerateAnimations(bodyPart, GetAllAnimationClipsForBodyPart(bodyPart));
        }
    }
}
