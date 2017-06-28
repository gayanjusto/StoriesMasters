﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Factories.AnimatorControllers
{
    public class PlayerTorsoAnimatorControllerFactory : PlayerAnimatorControllerAbstractFactory
    {
        public override void Create()
        {
            string bodyPart = "Torso";
            var walkingAnimations = base.GetMotionClips("Walking", bodyPart); 
            var combatWalkingAnimations = base.GetMotionClips("CombatWalking", bodyPart);

            var animationsList = new List<Motion>();
            animationsList.AddRange(walkingAnimations);
            animationsList.AddRange(combatWalkingAnimations);

            base.GenerateAnimations(bodyPart, animationsList);
        }
    }
}
