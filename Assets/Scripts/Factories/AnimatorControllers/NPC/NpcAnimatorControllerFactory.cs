using System;
using Assets.Scripts.Interfaces.Factories.AnimatorControllers;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Assets.Scripts.Factories.AnimatorControllers
{
    public class NpcAnimatorControllerFactory : BaseAnimatorControllerAbstractFactory, IAnimatorFactory
    {
        public void Create()
        {
            string controllerName = "";

            //get npc motion clips
            GenerateAnimations(controllerName, GetAllNPCAnimationClips());
        }



        protected List<Motion> GetAllNPCAnimationClips()
        {
            var animationsList = new List<Motion>();

            foreach (var subStateMachineName in mainSubStates)
            {
                var animations = GetMotionClipsByBodyPart(subStateMachineName);

                if (animations != null && animations.Count() > 0)
                {
                    animationsList.AddRange(animations);
                }
            }

            return animationsList;
        }

        protected Motion[] GetMotionClipsByBodyPart(string movement)
        {
            string animationResourcesPath = string.Format("Animations/{0}/NPC/", movement);
            var animations = Resources.LoadAll<Motion>(animationResourcesPath);
            return animations;
        }

        protected override void GenerateAnimations(string controllerName, List<Motion> motionClips)
        {
            base.currentControllersPath = string.Format("{0}/{1}", baseControllersPath, "Npc/");
            base.sufixControllerName = "NpcAnimatorController";

            base.GenerateAnimations(controllerName, motionClips);
        }
    }
}
