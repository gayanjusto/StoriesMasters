using UnityEngine;
using System.Linq;
using UnityEditor.Animations;
using System.Collections.Generic;

//-> https://docs.unity3d.com/ScriptReference/Animations.AnimatorController.html

namespace Assets.Scripts.Factories.AnimatorControllers
{
    public class PlayerAnimatorControllerFactory
    {
        const string playerControllersPath = "Assets/Animators/Player/";

        private static IList<AnimatorState> AnimatorStates { get; set; }

        public static void Create(string controllerName)
        {
            AnimatorStates = new List<AnimatorState>();

            string[] mainSubStates = new[] { "Idle", "Walking", "CombatStance", "Running", "Attacking" };
            string[] directionStates = new[] { "Down", "DownLeft", "Left", "UpLeft", "Up", "UpRight", "Right", "DownRight" };

            string pathWithFileName = string.Format("{0}{1}.controller", playerControllersPath, controllerName);
            var controller = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath(pathWithFileName);

            //Parameters
            controller.AddParameter("IsCombatActive", AnimatorControllerParameterType.Bool);
            controller.AddParameter("Walking", AnimatorControllerParameterType.Bool);
            controller.AddParameter("Running", AnimatorControllerParameterType.Bool);
            controller.AddParameter("Direction", AnimatorControllerParameterType.Int);

            //StatesMachines (SubState-Machines)
            var rootStateMachine = controller.layers[0].stateMachine;

            for (int i = 0; i < mainSubStates.Length; i++)
            {
                //Create Sub-States
                var currentSubStateMachine = rootStateMachine.AddStateMachine(mainSubStates[i]);


                //Add States to the current Sub-State
                for (int j = 0; j < directionStates.Length; j++)
                {
                    var state = currentSubStateMachine.AddState(directionStates[j]);
                    AnimatorStates.Add(state);
                }

                Debug.Log(AnimatorStates.Count);

                //Add transition between other states
                //foreach (var state in AnimatorStates)
                //{
                //    for (int j = 0; j < AnimatorStates.Count; j++)
                //    {
                //        if (AnimatorStates[j] == state) { continue; }
                //        var destinationState = AnimatorStates[j];
                //        var currStateToDestinationState = state.AddTransition(destinationState);
                //        var destinationStateToCurrState = destinationState.AddTransition(state);

                //        currStateToDestinationState.AddCondition(AnimatorConditionMode.Equals, 1, "Direction");
                //        currStateToDestinationState.duration = 0;

                //        destinationStateToCurrState.AddCondition(AnimatorConditionMode.Equals, 0, "Direction");
                //        destinationStateToCurrState.duration = 0;
                //    }
                //}
            }
        }
    }
}
