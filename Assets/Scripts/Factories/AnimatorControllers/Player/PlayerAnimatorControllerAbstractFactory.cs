using UnityEngine;
using System.Linq;
using UnityEditor.Animations;
using System.Collections.Generic;
using Assets.Scripts.Enums;
using System;

//-> https://docs.unity3d.com/ScriptReference/Animations.AnimatorController.html

namespace Assets.Scripts.Factories.AnimatorControllers
{
    public class PlayerAnimatorControllerAbstractFactory : BaseAnimatorControllerAbstractFactory
    {

        protected List<Motion> GetAllAnimationClipsForBodyPart(string bodyPart)
        {
            var animationsList = new List<Motion>();

            foreach (var subStateMachineName in mainSubStates)
            {
                var animations = GetMotionClipsByBodyPart(subStateMachineName, bodyPart);

                if(animations != null && animations.Count() > 0)
                {
                    animationsList.AddRange(animations);
                }
            }

            return animationsList;
        }

        protected Motion[] GetMotionClipsByBodyPart(string movement, string bodyPart)
        {
            string animationResourcesPath = string.Format("Animations/{0}/Player/{1}", movement, bodyPart);
            var animations = Resources.LoadAll<Motion>(animationResourcesPath);
            return animations;
        }

        protected override void GenerateAnimations(string controllerName, List<Motion> motionClips)
        {
            base.currentControllersPath = string.Format("{0}/{1}", baseControllersPath, "Player");
            base.sufixControllerName = "PlayerAnimatorController";

            base.GenerateAnimations(controllerName, motionClips);
        }
    }
}
