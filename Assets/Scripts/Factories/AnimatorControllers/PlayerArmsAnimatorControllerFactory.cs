using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Assets.Scripts.Factories.AnimatorControllers
{
    public class PlayerArmsAnimatorControllerFactory : PlayerAnimatorControllerAbstractFactory
    {
        public override void Create()
        {
            string bodyPart = "Arms";
            var walkingAnimations = base.GetMotionClips("Walking", bodyPart); //Resources.LoadAll<Motion>(string.Format("{0}{1}", playerResourcesAnimPath, "Arms"));
            var combatWalkingAnimations = base.GetMotionClips("CombatWalking", bodyPart);

            var animationsList = new List<Motion>();
            animationsList.AddRange(walkingAnimations);
            animationsList.AddRange(combatWalkingAnimations);

            base.GenerateAnimations(bodyPart, animationsList);
        }
    }
}
