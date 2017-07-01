using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using System.Linq;

namespace Assets.Scripts.Factories.AnimatorControllers
{
    public abstract class BaseAnimatorControllerAbstractFactory
    {
        protected const string baseControllersPath = "Assets/Animators";
        protected string currentControllersPath;
        protected string sufixControllerName;

        protected List<AnimatorStateMachine> AnimatorSubStateMachines { get; set; }
        protected AnimatorStateMachine rootStateMachine;
        protected IEnumerable<DirectionEnum> directionsEnumValues;

        //Controller Parameters
        protected const string combatActiveParam = "IsCombatActive";
        protected const string walkingParam = "Walking";
        protected const string runningParam = "Running";
        protected const string directionParam = "Direction";
        protected const string attackSequenceParam = "AttackSequence";

        //SubStateMachines
        protected const string subStateIdleName = "Idle";
        protected const string subStateWalkingName = "Walking";
        protected const string subStateCombatIdleStanceName = "CombatIdle";
        protected const string subStateCombatWalkingStanceName = "CombatWalking";
        protected const string subStateRunningName = "Running";
        protected const string subStateAttackingName = "Attacking";

        //Attack States
        protected const string attackSequence_1 = "AttackSequence_1";
        protected const string attackSequence_2 = "AttackSequence_2";
        protected const string attackSequence_3 = "AttackSequence_3";

        protected struct TransitionConditions
        {
            public AnimatorConditionMode conditionMode;
            public float threshold;
            public string parameterName;
        }

        protected string[] mainSubStates = new[] { subStateIdleName, subStateWalkingName, subStateCombatIdleStanceName, subStateCombatWalkingStanceName, subStateRunningName, subStateAttackingName };
        protected string[] attackStates = new[] { attackSequence_1, attackSequence_2, attackSequence_3 };

        protected virtual void GenerateAnimations(string controllerName, List<Motion> motionClips)
        {
            AnimatorSubStateMachines = new List<AnimatorStateMachine>();

            directionsEnumValues = Enum.GetValues(typeof(DirectionEnum)).Cast<DirectionEnum>();

            string pathWithFileName = string.Format("{0}{1}{2}.controller", currentControllersPath, controllerName, sufixControllerName);

            var controller = AnimatorController.CreateAnimatorControllerAtPath(pathWithFileName);

            //Parameters
            controller.AddParameter(combatActiveParam, AnimatorControllerParameterType.Bool);
            controller.AddParameter(walkingParam, AnimatorControllerParameterType.Bool);
            controller.AddParameter(runningParam, AnimatorControllerParameterType.Bool);
            controller.AddParameter(directionParam, AnimatorControllerParameterType.Int);
            controller.AddParameter(attackSequenceParam, AnimatorControllerParameterType.Int);


            //StatesMachines (SubState-Machines)
            rootStateMachine = controller.layers[0].stateMachine;

            CreateSubStateMachines(motionClips);
            CreateInternalAttackingTransitionBetweenStates(AnimatorSubStateMachines.First(x => x.name == subStateAttackingName));
            CreateInternalDirectionTransitionBetweenStates();

            CreateTransitionBetweenSubStatesMachines();
        }

  
        int GetDirectionValueFromEnum(string directionName)
        {
            return directionsEnumValues.First(x => x.ToString() == directionName).GetHashCode();
        }

        void SetTimeSettingsForTransition(AnimatorStateTransition transition)
        {
            transition.duration = 0;
            transition.hasExitTime = false;
            transition.hasFixedDuration = false;
        }

        void SetAnimationSpeed(AnimatorState state, float speed, string subStateMachineName)
        {
            if (subStateMachineName == subStateIdleName || subStateMachineName == subStateCombatIdleStanceName)
            {
                speed = .05f;
            }
            state.speed = speed;
        }

        void CreateSubStateMachines(List<Motion> motionClips)
        {
            for (int i = 0; i < mainSubStates.Length; i++)
            {
                //Create Sub-States
                var currentSubStateMachine = rootStateMachine.AddStateMachine(mainSubStates[i]);

                AnimatorSubStateMachines.Add(currentSubStateMachine);

                if (currentSubStateMachine.name == subStateAttackingName)
                {
                    AddAttackDirectionsIntoSubStateMachine(currentSubStateMachine, motionClips);
                    continue;
                }
                AddDirectionStateIntoSubStateMachine(currentSubStateMachine, motionClips);
            }
        }

        void AddAttackDirectionsIntoSubStateMachine(AnimatorStateMachine attackSubStateMachine, List<Motion> motionClips)
        {
            foreach (var directionEnum in directionsEnumValues)
            {
                var direction = directionEnum.ToString();
                if (direction == DirectionEnum.None.ToString()) { continue; }
                foreach (var attackStateDirection in attackStates)
                {
                    //Add states for attack
                    var state = attackSubStateMachine.AddState(string.Format("{0}_{1}", direction.ToString(), attackStateDirection));

                    //Find motionClip
                }
            }
        }

        void AddDirectionStateIntoSubStateMachine(AnimatorStateMachine currentSubStateMachine, List<Motion> motionClips)
        {
            //Add Direction States to the current Sub-State
            foreach (var direction in directionsEnumValues)
            {
                //Mecanim must not have ''None'' direction
                if (direction.ToString() == "None") { continue; }

                string directionName = direction.ToString();
                var state = currentSubStateMachine.AddState(directionName);

                //Add animation clip to the state, based on the name of the animation clip
                if (motionClips.Any(x => x.name.ToUpper().Contains(currentSubStateMachine.name.ToUpper())))
                {
                    //a1 = angle 1, where 1 = DirectionEnum value
                    var motionClip = motionClips.First(x => x.name.ToUpper().Contains(currentSubStateMachine.name.ToUpper())
                    && x.name.Contains(string.Format("a{0}", direction.GetHashCode())));

                    state.motion = motionClip;
                    SetAnimationSpeed(state, .5f, currentSubStateMachine.name);
                }
            }
        }

        void CreateInternalAttackingTransitionBetweenStates(AnimatorStateMachine attackingSubStateMachine)
        {
            //Iterate each state inside the state machine
            for (int i = 0; i < attackingSubStateMachine.states.Length; i++) //currentState
            {
                for (int j = 0; j < attackingSubStateMachine.states.Length; j++) //otherState
                {
                    var currentState = attackingSubStateMachine.states[i];

                    var destinationState = attackingSubStateMachine.states[j].state;
                    var originState = attackingSubStateMachine.states[i].state;

                    //One attack sequence can only flow to the following sequence. Ex: attack_sequence_1 to attack_sequence_2
                    var originAttackSequence = Convert.ToInt32(originState.name.Split('_').Last());
                    var destinationAttackSequence = Convert.ToInt32(destinationState.name.Split('_').Last());

                    if (destinationState == originState) { continue; }

                    int lastAttackSequence = 3;

                    if ((originAttackSequence + 1) == destinationAttackSequence && originAttackSequence != lastAttackSequence)
                    {

                        var currStateToDestinationState = currentState.state.AddTransition(destinationState);
                        currStateToDestinationState.AddCondition(AnimatorConditionMode.Equals, destinationAttackSequence, attackSequenceParam);
                        SetTimeSettingsForTransition(currStateToDestinationState);
                    }
                }

            }
        }

        void CreateInternalDirectionTransitionBetweenStates()
        {
            foreach (var stateMachine in AnimatorSubStateMachines)
            {
                //Ignore attacking subStateMachine for directions
                if (stateMachine.name == subStateAttackingName) { continue; }

                //Iterate each state inside the state machine
                for (int i = 0; i < stateMachine.states.Length; i++) //currentState
                {
                    for (int j = 0; j < stateMachine.states.Length; j++) //otherState
                    {
                        var currentState = stateMachine.states[i];

                        var destinationState = stateMachine.states[j].state;
                        var originState = stateMachine.states[i].state;

                        if (destinationState == originState) { continue; }


                        var currStateToDestinationState = currentState.state.AddTransition(destinationState);

                        //Get value from enum based on same name of the state
                        var directionValue = GetDirectionValueFromEnum(destinationState.name);
                        currStateToDestinationState.AddCondition(AnimatorConditionMode.Equals, directionValue, directionParam);

                        SetTimeSettingsForTransition(currStateToDestinationState);
                    }
                }
            }
        }

        void CreateTransitionBetweenSubStatesMachines()
        {
            //Idle -> Walking
            var idleSubStateMachine = AnimatorSubStateMachines.First(x => x.name == subStateIdleName);
            var walkingSubStateMachine = AnimatorSubStateMachines.First(x => x.name == subStateWalkingName);
            var combatIdleSubStateMachine = AnimatorSubStateMachines.First(x => x.name == subStateCombatIdleStanceName);
            var combatWalkingSubStateMachine = AnimatorSubStateMachines.First(x => x.name == subStateCombatWalkingStanceName);
            var runningSubStateMachine = AnimatorSubStateMachines.First(x => x.name == subStateRunningName);

            TransitionBetweenIdleWalking(idleSubStateMachine, walkingSubStateMachine);

            //Combat Transitions
            TransitionBetweenWalkingCombatWalking(walkingSubStateMachine, combatWalkingSubStateMachine);
            TransitionBetweenCombatIdleCombatWalking(combatIdleSubStateMachine, combatWalkingSubStateMachine);
            TransitionBetweenCombatIdleRunning(combatIdleSubStateMachine, runningSubStateMachine);
            TransitionBetweenCombatIdleIdle(combatIdleSubStateMachine, idleSubStateMachine);

            //RunningTransitions
            TransitionBetweenWalkingRunning(walkingSubStateMachine, runningSubStateMachine);
            TransitionBetweenIdleRunning(idleSubStateMachine, runningSubStateMachine);

            //Attacking Transitions
        }

        private void TransitionBetweenIdleWalking(AnimatorStateMachine idleSubStateMachine, AnimatorStateMachine walkingSubStateMachine)
        {
            var transitionConditions = new TransitionConditions[1];

            //Idle -> Walking
            transitionConditions[0] = new TransitionConditions() { conditionMode = AnimatorConditionMode.If, parameterName = walkingParam };
            CreateAllWaysTransitionCondition(idleSubStateMachine, walkingSubStateMachine, transitionConditions);

            //Walking -> Idle
            transitionConditions[0] = new TransitionConditions() { conditionMode = AnimatorConditionMode.IfNot, parameterName = walkingParam };
            CreateOneWayTransitionCondition(walkingSubStateMachine, idleSubStateMachine, transitionConditions);
        }

        #region COMBAT TRANSITIONS
        private void TransitionBetweenWalkingCombatWalking(AnimatorStateMachine walkingSubStateMachine, AnimatorStateMachine combatWalkingSubStateMachine)
        {
            var transitionConditions = new TransitionConditions[1];

            //Walking -> CombatWalking
            transitionConditions[0] = new TransitionConditions() { conditionMode = AnimatorConditionMode.If, parameterName = combatActiveParam };
            CreateOneWayTransitionCondition(walkingSubStateMachine, combatWalkingSubStateMachine, transitionConditions);
            //CombatWalking -> Walking
            transitionConditions[0] = new TransitionConditions() { conditionMode = AnimatorConditionMode.IfNot, parameterName = combatActiveParam };
            CreateOneWayTransitionCondition(combatWalkingSubStateMachine, walkingSubStateMachine, transitionConditions);
        }

        private void TransitionBetweenCombatIdleCombatWalking(AnimatorStateMachine combatIdleSubStateMachine, AnimatorStateMachine combatWalkingSubStateMachine)
        {
            var transitionConditions = new TransitionConditions[2];

            //CombatIdle -> CombatWalking
            transitionConditions[0] = new TransitionConditions() { conditionMode = AnimatorConditionMode.If, parameterName = combatActiveParam };
            transitionConditions[1] = new TransitionConditions() { conditionMode = AnimatorConditionMode.If, parameterName = walkingParam };
            CreateAllWaysTransitionCondition(combatIdleSubStateMachine, combatWalkingSubStateMachine, transitionConditions);

            //CombatWalking-> CombatIdle 
            transitionConditions[0] = new TransitionConditions() { conditionMode = AnimatorConditionMode.If, parameterName = combatActiveParam };
            transitionConditions[1] = new TransitionConditions() { conditionMode = AnimatorConditionMode.IfNot, parameterName = walkingParam };
            CreateOneWayTransitionCondition(combatWalkingSubStateMachine, combatIdleSubStateMachine, transitionConditions);
        }

        private void TransitionBetweenCombatIdleRunning(AnimatorStateMachine combatIdleSubStateMachine, AnimatorStateMachine runningSubStateMachine)
        {
            var transitionConditions = new TransitionConditions[3];

            //CombatIdle -> Running
            transitionConditions[0] = new TransitionConditions() { conditionMode = AnimatorConditionMode.If, parameterName = combatActiveParam };
            transitionConditions[1] = new TransitionConditions() { conditionMode = AnimatorConditionMode.If, parameterName = runningParam };
            transitionConditions[2] = new TransitionConditions() { conditionMode = AnimatorConditionMode.If, parameterName = walkingParam };
            CreateAllWaysTransitionCondition(combatIdleSubStateMachine, runningSubStateMachine, transitionConditions);

            //Running -> CombatIdle
            transitionConditions[0] = new TransitionConditions() { conditionMode = AnimatorConditionMode.IfNot, parameterName = combatActiveParam };
            transitionConditions[1] = new TransitionConditions() { conditionMode = AnimatorConditionMode.IfNot, parameterName = runningParam };
            transitionConditions[2] = new TransitionConditions() { conditionMode = AnimatorConditionMode.IfNot, parameterName = walkingParam };
            CreateAllWaysTransitionCondition(runningSubStateMachine, combatIdleSubStateMachine, transitionConditions);
        }

        private void TransitionBetweenCombatIdleIdle(AnimatorStateMachine combatIdleSubStateMachine, AnimatorStateMachine idleSubStateMachine)
        {
            var transitionConditions = new TransitionConditions[2];

            //CombatIdle -> Idle
            transitionConditions[0] = new TransitionConditions() { conditionMode = AnimatorConditionMode.IfNot, parameterName = combatActiveParam };
            transitionConditions[1] = new TransitionConditions() { conditionMode = AnimatorConditionMode.IfNot, parameterName = walkingParam };
            CreateOneWayTransitionCondition(combatIdleSubStateMachine, idleSubStateMachine, transitionConditions);

            //Idle -> CombatIdle
            transitionConditions[0] = new TransitionConditions() { conditionMode = AnimatorConditionMode.If, parameterName = combatActiveParam };
            transitionConditions[1] = new TransitionConditions() { conditionMode = AnimatorConditionMode.IfNot, parameterName = walkingParam };
            CreateOneWayTransitionCondition(idleSubStateMachine, combatIdleSubStateMachine, transitionConditions);
        }

        #endregion

        #region RUNNING TRANSITIONS
        private void TransitionBetweenWalkingRunning(AnimatorStateMachine walkingSubStateMachine, AnimatorStateMachine runningSubStateMachine)
        {
            var transitionConditions = new TransitionConditions[2];
            //Walking -> Running
            transitionConditions[0] = new TransitionConditions() { conditionMode = AnimatorConditionMode.If, parameterName = walkingParam };
            transitionConditions[1] = new TransitionConditions() { conditionMode = AnimatorConditionMode.If, parameterName = runningParam };
            CreateAllWaysTransitionCondition(walkingSubStateMachine, runningSubStateMachine, transitionConditions);

            //Running -> Combat
            transitionConditions[0] = new TransitionConditions() { conditionMode = AnimatorConditionMode.If, parameterName = walkingParam };
            transitionConditions[1] = new TransitionConditions() { conditionMode = AnimatorConditionMode.IfNot, parameterName = runningParam };
            CreateAllWaysTransitionCondition(runningSubStateMachine, walkingSubStateMachine, transitionConditions);
        }

        private void TransitionBetweenIdleRunning(AnimatorStateMachine idleSubStateMachine, AnimatorStateMachine runningSubStateMachine)
        {
            var transitionConditions = new TransitionConditions[2];
            //Idle -> Running
            transitionConditions[0] = new TransitionConditions() { conditionMode = AnimatorConditionMode.If, parameterName = walkingParam };
            transitionConditions[1] = new TransitionConditions() { conditionMode = AnimatorConditionMode.If, parameterName = runningParam };
            CreateAllWaysTransitionCondition(idleSubStateMachine, runningSubStateMachine, transitionConditions);

            //Running -> Idle
            transitionConditions[0] = new TransitionConditions() { conditionMode = AnimatorConditionMode.IfNot, parameterName = walkingParam };
            transitionConditions[1] = new TransitionConditions() { conditionMode = AnimatorConditionMode.IfNot, parameterName = runningParam };
            CreateOneWayTransitionCondition(runningSubStateMachine, idleSubStateMachine, transitionConditions);
        }
        #endregion
        //Create transitions only when it'll leave an specific direction from a subStateMachine to the same into another subStateMachine
        void CreateOneWayTransitionCondition(AnimatorStateMachine originStateMachines, AnimatorStateMachine destinationStateMachines, TransitionConditions[] transitionConditions)
        {
            //Origin -> Destination
            foreach (var originStateMachine in originStateMachines.states)
            {
                var originState = originStateMachine.state;

                foreach (var destinationStateMachine in destinationStateMachines.states)
                {
                    var destinationState = destinationStateMachine.state;

                    //This stateMachine can only transit to another state machine from  a single state to another (i.e: Walking Left -> Combat Left)
                    if (originState.name != destinationState.name) { continue; }

                    var currStateToDestinationState = originState.AddTransition(destinationState);

                    //Get value from enum based on same name of the state
                    var directionValue = GetDirectionValueFromEnum(destinationState.name);
                    currStateToDestinationState.AddCondition(AnimatorConditionMode.Equals, directionValue, directionParam);

                    foreach (var transitionCondition in transitionConditions)
                    {
                        currStateToDestinationState.AddCondition(transitionCondition.conditionMode, 1, transitionCondition.parameterName);
                    }
                }
            }
        }

        //Create transitions that will transit with all directions from one subStateMachine to another
        void CreateAllWaysTransitionCondition(AnimatorStateMachine originStateMachines, AnimatorStateMachine destinationStateMachines, TransitionConditions[] transitionConditions)
        {
            //Origin -> Destination
            foreach (var originStateMachine in originStateMachines.states)
            {
                var originState = originStateMachine.state;

                foreach (var destinationStateMachine in destinationStateMachines.states)
                {
                    var destinationState = destinationStateMachine.state;

                    var currStateToDestinationState = originState.AddTransition(destinationState);

                    //Get value from enum based on same name of the state
                    var directionValue = GetDirectionValueFromEnum(destinationState.name);
                    currStateToDestinationState.AddCondition(AnimatorConditionMode.Equals, directionValue, directionParam);

                    foreach (var transitionCondition in transitionConditions)
                    {
                        currStateToDestinationState.AddCondition(transitionCondition.conditionMode, 1, transitionCondition.parameterName);
                    }
                }
            }
        }
    }
}
