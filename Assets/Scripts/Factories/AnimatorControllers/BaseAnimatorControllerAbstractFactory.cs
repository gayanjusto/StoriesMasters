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
        protected string _currentControllersPath;
        protected string _sufixControllerName;

        protected List<AnimatorStateMachine> StandardAnimatorSubStateMachines { get; set; }
        protected List<AnimatorStateMachine> ComplexAnimatorSubStateMachines { get; set; }

        protected AnimatorStateMachine _rootStateMachine;
        protected IEnumerable<DirectionEnum> _directionsEnumValues;

        //Controller Parameters
        protected const string combatActiveParam = "CombatActive";
        protected const string walkingParam = "Walking";
        protected const string runningParam = "Running";
        protected const string directionParam = "Direction";
        protected const string attackTypeParam = "AttackType";
        protected const string defenseTypeParam = "DefenseType";
        protected const string attackSequenceParam = "AttackSequence";
        protected const string defendingParam = "Defending";
        protected const string attackingParam = "Attacking";




        //SubStateMachines
        protected const string subStateIdleName = "Idle";
        protected const string subStateWalkingName = "Walking";
        protected const string subStateCombatIdleStanceName = "CombatIdle";
        protected const string subStateCombatWalkingStanceName = "CombatWalking";
        protected const string subStateRunningName = "Running";
        protected const string subStateAttackingName = "Attacking";
        protected const string subStateDefenseName = "Defense";
        protected const string subStateRecoverName = "Recover";



        //Attack States
        protected const string swingAttackSequence_1_1Handed = "AttackSwing_1h_1";
        protected const string swingAttackSequence_2_1Handed = "AttackSwing_1h_2";
        //protected const string swingAttackSequence_3_1Handed = "AttackSwing_1h_3";
        //protected const string swingAttackSequence_1_2Handed = "AttackSwing_2h_1";
        //protected const string swingAttackSequence_2_2Handed = "AttackSwing_2h_2";
        //protected const string swingAttackSequence_3_2Handed = "AttackSwing_2h_3";

        protected const string thrustAttackSequence_1_1Handed = "AttackThrust_1h_1";
        protected string[] _standardSubStates = new[] { subStateIdleName, subStateWalkingName, subStateCombatIdleStanceName, subStateCombatWalkingStanceName, subStateRunningName};
        protected string[] _complexSubStates = new[] {  subStateAttackingName, subStateDefenseName, subStateRecoverName };
        protected string[] _swingAttackStates = new[] { swingAttackSequence_1_1Handed, swingAttackSequence_2_1Handed, /*swingAttackSequence_3_1Handed, swingAttackSequence_1_2Handed, swingAttackSequence_2_2Handed, swingAttackSequence_3_2Handed*/ };
        protected string[] _thrustAttackStates = new[] { thrustAttackSequence_1_1Handed };

        //Defense States
        protected const string defenseShieldBlock = "Defense_ShieldBlock";
        protected const string defenseParryBlock = "Defense_ParryBlock";
        protected const string defenseDodge = "Defense_Dodge";
        protected string[] _defenseStates = new[] { defenseShieldBlock, defenseParryBlock, defenseDodge };

        //Recover States
        protected const string shieldRecover = "Recover_Shield";
        protected string[] _recoverStates = new[] { shieldRecover };

        protected struct TransitionConditions
        {
            public AnimatorConditionMode conditionMode;
            public float threshold;
            public string parameterName;
        }




        protected Motion[] GetMockClips()
        {
            string mocksPath = "Animations/Mocks/";

            var animations = Resources.LoadAll<Motion>(mocksPath);
            return animations;
        }

        protected virtual void GenerateAnimations(string controllerName, List<Motion> motionClips)
        {

            StandardAnimatorSubStateMachines = new List<AnimatorStateMachine>();
            ComplexAnimatorSubStateMachines = new List<AnimatorStateMachine>();


            _directionsEnumValues = Enum.GetValues(typeof(DirectionEnum)).Cast<DirectionEnum>();

            string pathWithFileName = string.Format("{0}{1}{2}.controller", _currentControllersPath, controllerName, _sufixControllerName);

            var controller = AnimatorController.CreateAnimatorControllerAtPath(pathWithFileName);

            //Parameters
            AddParamsToAnimatorController(controller);


            //StatesMachines (SubState-Machines)
            _rootStateMachine = controller.layers[0].stateMachine;

            CreateStandardSubStateMachines(motionClips);
            CreateComplexSubStateMachines(motionClips);

            CreateAttackSubStateMachine();
            CreateDefenseSubStateMachine();

            CreateTransitionBetweenSubStatesMachines();

            CreateInternalDirectionTransitionBetweenStates(StandardAnimatorSubStateMachines);

        }

        private static void AddParamsToAnimatorController(AnimatorController controller)
        {
            controller.AddParameter(combatActiveParam, AnimatorControllerParameterType.Bool);
            controller.AddParameter(walkingParam, AnimatorControllerParameterType.Bool);
            controller.AddParameter(runningParam, AnimatorControllerParameterType.Bool);
            controller.AddParameter(defendingParam, AnimatorControllerParameterType.Bool);

            controller.AddParameter(attackingParam, AnimatorControllerParameterType.Trigger);

            controller.AddParameter(directionParam, AnimatorControllerParameterType.Int);
            controller.AddParameter(attackTypeParam, AnimatorControllerParameterType.Int);
            controller.AddParameter(defenseTypeParam, AnimatorControllerParameterType.Int);
            controller.AddParameter(attackSequenceParam, AnimatorControllerParameterType.Int);
        }

        int GetDirectionValueFromEnum(string directionName)
        {
            directionName = directionName.Split('_').First();
            return _directionsEnumValues.First(x => x.ToString() == directionName).GetHashCode();
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

            //DEBUG
            speed = .05f;
        }

        #region SUB-STATE CREATION
        void CreateStandardSubStateMachines(List<Motion> motionClips)
        {
            for (int i = 0; i < _standardSubStates.Length; i++)
            {
                //Create Sub-States
                var currentSubStateMachine = _rootStateMachine.AddStateMachine(_standardSubStates[i]);

                StandardAnimatorSubStateMachines.Add(currentSubStateMachine);

                AddDirectionStateIntoSubStateMachine(currentSubStateMachine, motionClips);
            }
        }

        void CreateComplexSubStateMachines(List<Motion> motionClips)
        {
            foreach (var item in _complexSubStates)
            {
                //Create Sub-States
                var currentSubStateMachine = _rootStateMachine.AddStateMachine(item);

                ComplexAnimatorSubStateMachines.Add(currentSubStateMachine);

                //Create states for Attacks
                if (currentSubStateMachine.name == subStateAttackingName)
                {
                    AddAttackStatesIntoSubStateMachine(currentSubStateMachine, motionClips.ToList());
                    continue;
                }

                //Create states for defenses
                if (currentSubStateMachine.name == subStateDefenseName)
                {
                    AddDefenseStatesIntoSubStateMachine(currentSubStateMachine, motionClips.ToList());
                    continue;
                }

                //Create states for recovers
                if (currentSubStateMachine.name == subStateRecoverName)
                {
                    AddRecoverStatesIntoSubStateMachine(currentSubStateMachine, motionClips.ToList());
                    continue;
                }
            }

        }
        #endregion

        #region ATTACK STATES
        void CreateAttackSubStateMachine()
        {
            CreateInternalAttackingTransitionBetweenStates(ComplexAnimatorSubStateMachines.First(x => x.name == subStateAttackingName));
        }
        void AddAttackStatesIntoSubStateMachine(AnimatorStateMachine attackSubStateMachine, List<Motion> motionClips)
        {
            foreach (var directionEnum in _directionsEnumValues)
            {
                var direction = directionEnum.ToString();
                if (direction == DirectionEnum.None.ToString()) { continue; }

                var attackStates = new List<string>();
                attackStates.AddRange(_swingAttackStates);
                attackStates.AddRange(_thrustAttackStates);

                foreach (var attackStateDirection in attackStates)
                {
                    //Add states for attack
                    var state = attackSubStateMachine.AddState(string.Format("{0}_{1}", direction.ToString(), attackStateDirection));

                    var attackNameArray = attackStateDirection.ToUpper().Split('_');
                    var attackName = attackNameArray[0];
                    var attackSequence = attackNameArray[1];
                    var attackHands = attackNameArray[2];

                    //Find motionClip
                    var motionClip = motionClips.FirstOrDefault(x => MatchSplittedMockName(x.name, attackName, attackSequence, attackHands, directionEnum.GetHashCode()));

                    state.motion = motionClip;
                }
            }
        }

        bool MatchSplittedMockName(string mockName, string attackName, string attackHands, string attackSequence, int directionValue)
        {
            var mockNameArray = mockName.ToUpper().Split('_');

            if (!mockNameArray.Any(x => x.Contains("MOCK")) || mockNameArray.Any(x => x.Contains("BLOCK")))
            {
                return false;
            }
            var mockDirection = mockNameArray[0];
            var mockClipName = mockNameArray[2];
            var mockHands = mockNameArray[3];
            var mockSequence = mockNameArray[4];

            var result = mockClipName == attackName && mockHands == attackHands && mockSequence == attackSequence && mockDirection == "A" + directionValue;
            return result;

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
        #endregion

        #region DEFENSE STATES
        void CreateDefenseSubStateMachine()
        {
            CreateInternalDefenseTransitionBetweenStates(ComplexAnimatorSubStateMachines.First(x => x.name == subStateDefenseName));
        }
        void AddDefenseStatesIntoSubStateMachine(AnimatorStateMachine defenseSubStateMachine, List<Motion> motionClips)
        {
            foreach (var directionEnum in _directionsEnumValues)
            {
                var direction = directionEnum.ToString();
                if (direction == DirectionEnum.None.ToString()) { continue; }

                foreach (var defenseState in _defenseStates)
                {
                    //Add states for attack
                    var state = defenseSubStateMachine.AddState(string.Format("{0}_{1}", direction.ToString(), defenseState));

                    var defenseNameArray = defenseState.ToUpper().Split('_');
                    var defenseDirection = "A" + directionEnum.GetHashCode();
                    var defenseName = defenseNameArray[0];
                    var defenseType = defenseNameArray[1];

                    //Find motionClip
                    //ex of motion name: a1_defense_shieldblock
                    var motionClip = motionClips.FirstOrDefault(x => MatchMotionClipWithDefenseState(x.name, defenseDirection, defenseName, defenseType));

                    if (motionClip != null)
                    {
                        state.motion = motionClip;
                    }
                }
            }
        }

        bool MatchMotionClipWithDefenseState(string motionMockName, string defenseDirection, string defenseName, string defenseType)
        {
            var mockNameArray = motionMockName.ToUpper().Split('_');

            if (!mockNameArray.Any(x => x.Contains("DEFENSE")))
            {
                return false;
            }

            var mockDirection = mockNameArray[0];
            var mockDefenseType = mockNameArray[2];

            var result = mockDirection == defenseDirection && mockDefenseType == defenseType;
            return result;
        }

        void CreateInternalDefenseTransitionBetweenStates(AnimatorStateMachine defenseSubStateMachine)
        {
            //Iterate each state inside the state machine

            foreach (var originAnimatorState in defenseSubStateMachine.states)
            {
                var defenseType = originAnimatorState.state.name.Split('_').Last();
                var defenseStatesOfSameType = defenseSubStateMachine.states.Where(x => x.state.name.Split('_').Contains(defenseType));

                foreach (var destinationAnimatorState in defenseStatesOfSameType)
                {
                    var currentState = originAnimatorState.state;

                    var destinationState = destinationAnimatorState.state;
                    var originState = originAnimatorState.state;

                    if (destinationState == originState) { continue; }

                    var currStateToDestinationState = currentState.AddTransition(destinationState);

                    //Get value from enum based on same name of the state
                    var directionValue = GetDirectionValueFromEnum(destinationState.name.Split('_').First());
                    currStateToDestinationState.AddCondition(AnimatorConditionMode.Equals, directionValue, directionParam);

                    SetTimeSettingsForTransition(currStateToDestinationState);
                }
            }
        }
        #endregion

        #region RECOVER STATES
        void AddRecoverStatesIntoSubStateMachine(AnimatorStateMachine recoverSubStateMachine, List<Motion> motionClips)
        {
            foreach (var directionEnum in _directionsEnumValues)
            {
                var direction = directionEnum.ToString();
                if (direction == DirectionEnum.None.ToString()) { continue; }

                foreach (var recoverState in _recoverStates)
                {
                    //Add states for attack
                    var state = recoverSubStateMachine.AddState(string.Format("{0}_{1}", direction.ToString(), recoverState));

                    var recoverNameArray = recoverState.ToUpper().Split('_');
                    var recoverDirection = "A" + directionEnum.GetHashCode();
                    var recoverName = recoverNameArray[0];
                    var recoverType = recoverNameArray[1];


                    //Find motionClip
                    //ex of motion name: a1_shield_recover
                    var motionClip = motionClips.FirstOrDefault(x => MatchMotionClipWithRecoverState(x.name, recoverDirection, recoverName, recoverType));

                    if (motionClip != null)
                    {
                        state.motion = motionClip;
                    }
                }
            }
        }


        bool MatchMotionClipWithRecoverState(string motionMockName, string recoverDirection, string defenseName, string recoverType)
        {
            var mockNameArray = motionMockName.ToUpper().Split('_');

            if (!mockNameArray.Any(x => x.Contains("RECOVER")))
            {
                return false;
            }

            var mockDirection = mockNameArray[0];
            var mockDefenseType = mockNameArray[2];

            var result = mockDirection == recoverDirection && mockDefenseType == recoverType;
            return result;
        }
        #endregion

        void AddDirectionStateIntoSubStateMachine(AnimatorStateMachine currentSubStateMachine, List<Motion> motionClips)
        {
            //Add Direction States to the current Sub-State
            foreach (var direction in _directionsEnumValues)
            {
                //Mecanim must not have ''None'' direction
                if (direction.ToString() == "None") { continue; }

                string directionName = direction.ToString();
                var state = currentSubStateMachine.AddState(string.Format("{0}_{1}", directionName, currentSubStateMachine.name));

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


        void CreateInternalDirectionTransitionBetweenStates(List<AnimatorStateMachine> animatorSubStateMachines)
        {
            foreach (var stateMachine in animatorSubStateMachines)
            {
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
            //Standard Sub-State Machines
            var idleSubStateMachine = StandardAnimatorSubStateMachines.First(x => x.name == subStateIdleName);
            var walkingSubStateMachine = StandardAnimatorSubStateMachines.First(x => x.name == subStateWalkingName);
            var combatIdleSubStateMachine = StandardAnimatorSubStateMachines.First(x => x.name == subStateCombatIdleStanceName);
            var combatWalkingSubStateMachine = StandardAnimatorSubStateMachines.First(x => x.name == subStateCombatWalkingStanceName);
            var runningSubStateMachine = StandardAnimatorSubStateMachines.First(x => x.name == subStateRunningName);

            //Complex Sub-State Machines
            var defenseSubStateMachine = ComplexAnimatorSubStateMachines.First(x => x.name == subStateDefenseName);
            var attackSubStateMachine = ComplexAnimatorSubStateMachines.First(x => x.name == subStateAttackingName);
            
            //Idle -> Walking
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
            TransitionBetweenAttackCombatIdle(attackSubStateMachine, combatIdleSubStateMachine);

            //Defense Transitions
            TransitionBetweenDefenseIdle(defenseSubStateMachine, idleSubStateMachine);
        }

        #region IDLE TRANSITIONS
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
        #endregion

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

        #region DEFENSE TRANSITIONS
        private void TransitionBetweenDefenseIdle(AnimatorStateMachine defenseSubStateMachine, AnimatorStateMachine idleSubStateMachine)
        {
            var transitionConditions = new TransitionConditions[4];
            // Idle -> Defense
            transitionConditions[0] = new TransitionConditions() { conditionMode = AnimatorConditionMode.IfNot, parameterName = walkingParam };
            transitionConditions[1] = new TransitionConditions() { conditionMode = AnimatorConditionMode.IfNot, parameterName = runningParam };
            transitionConditions[2] = new TransitionConditions() { conditionMode = AnimatorConditionMode.If, parameterName = defendingParam };
            transitionConditions[3] = new TransitionConditions() { conditionMode = AnimatorConditionMode.Equals, parameterName = defenseTypeParam};

            CreateOneWayTransitionConditionComplexNamesToStandard(idleSubStateMachine, defenseSubStateMachine, transitionConditions, idleSubStateMachine.name);

            //Defense -> Idle
            transitionConditions = new TransitionConditions[3];

            transitionConditions[0] = new TransitionConditions() { conditionMode = AnimatorConditionMode.IfNot, parameterName = walkingParam };
            transitionConditions[1] = new TransitionConditions() { conditionMode = AnimatorConditionMode.IfNot, parameterName = runningParam };
            transitionConditions[2] = new TransitionConditions() { conditionMode = AnimatorConditionMode.IfNot, parameterName = defendingParam };
            CreateOneWayTransitionConditionComplexNamesToStandard(defenseSubStateMachine, idleSubStateMachine, transitionConditions, idleSubStateMachine.name);
        }

        #endregion

        #region ATTACKING TRANSITIONS
        private void TransitionBetweenAttackCombatIdle(AnimatorStateMachine attackSubStateMachine, AnimatorStateMachine combatIdleSubStateMachine)
        {
            var transitionConditions = new TransitionConditions[3];
            // CombatIdle -> Attack
            transitionConditions[0] = new TransitionConditions() { conditionMode = AnimatorConditionMode.If, parameterName = combatActiveParam };
            transitionConditions[1] = new TransitionConditions() { conditionMode = AnimatorConditionMode.If, parameterName = attackingParam };
            transitionConditions[2] = new TransitionConditions() { conditionMode = AnimatorConditionMode.Equals, parameterName = attackTypeParam };


            CreateAllWaysTransitionConditionAttackingStateToStandard(combatIdleSubStateMachine, attackSubStateMachine, transitionConditions, combatIdleSubStateMachine.name);

            //Attack -> CombatIdle
            transitionConditions = new TransitionConditions[1];

            transitionConditions[0] = new TransitionConditions() { conditionMode = AnimatorConditionMode.If, parameterName = combatActiveParam };
            CreateAllWaysTransitionConditionAttackingStateToStandard(attackSubStateMachine, combatIdleSubStateMachine, transitionConditions, combatIdleSubStateMachine.name);
        }
        #endregion

        #region TRANSITION METHODS BETWEEN SUBSTATEMACHINES
        //Create transitions only when it'll leave an specific direction from a subStateMachine to the same into another subStateMachine
        void CreateOneWayTransitionCondition(AnimatorStateMachine originStateMachines, AnimatorStateMachine destinationStateMachines, TransitionConditions[] transitionConditions)
        {
            string originDirection = string.Empty;
            string destinationDirection = string.Empty;

            //Origin -> Destination
            foreach (var originStateMachine in originStateMachines.states)
            {
                var originState = originStateMachine.state;

                foreach (var destinationStateMachine in destinationStateMachines.states)
                {
                    var destinationState = destinationStateMachine.state;

                    originDirection = originState.name.Split('_').First();
                    destinationDirection = destinationState.name.Split('_').First();

                    //This stateMachine can only transit to another state machine from  a single state to another (i.e: Walking Left -> Combat Left)
                    if (originDirection != destinationDirection) { continue; }

                    var currStateToDestinationState = originState.AddTransition(destinationState);

                    //Get value from enum based on same name of the state
                    var directionValue = GetDirectionValueFromEnum(destinationState.name.Split('_').First());
                    currStateToDestinationState.AddCondition(AnimatorConditionMode.Equals, directionValue, directionParam);

                    foreach (var transitionCondition in transitionConditions)
                    {
                        currStateToDestinationState.AddCondition(transitionCondition.conditionMode, transitionCondition.threshold, transitionCondition.parameterName);
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
                    var directionValue = GetDirectionValueFromEnum(destinationState.name.Split('_').First());
                    currStateToDestinationState.AddCondition(AnimatorConditionMode.Equals, directionValue, directionParam);

                    foreach (var transitionCondition in transitionConditions)
                    {
                        currStateToDestinationState.AddCondition(transitionCondition.conditionMode, transitionCondition.threshold, transitionCondition.parameterName);
                    }
                }
            }
        }

        void CreateOneWayTransitionConditionComplexNamesToStandard(AnimatorStateMachine originStateMachines, AnimatorStateMachine destinationStateMachines, TransitionConditions[] transitionConditions, string standardStateName)
        {
            string directionName = string.Empty;
            string defenseType = string.Empty;
            string originDirection = string.Empty;
            string destinationDirection = string.Empty;

            //Origin -> Destination
            foreach (var originStateMachine in originStateMachines.states)
            {
                var originState = originStateMachine.state;

                foreach (var destinationStateMachine in destinationStateMachines.states)
                {
                    var destinationState = destinationStateMachine.state;

                    //This stateMachine can only transit to another state machine from  a single state to another (i.e: Walking Left -> Combat Left)
                    if(originStateMachines.name == subStateIdleName)
                    {
                        directionName = destinationState.name.Split('_').First();
                        defenseType = destinationState.name.Split('_').Last();
                        originDirection = originState.name.Split('_').First();

                        if (originDirection != directionName)
                        {
                            continue;
                        }
                    }else
                    {
                        directionName = originState.name.Split('_').First();
                        defenseType = originState.name.Split('_').Last();
                        destinationDirection = destinationState.name.Split('_').First();

                        if (directionName  != destinationDirection)
                        {
                            continue;
                        }
                    }

                    var currStateToDestinationState = originState.AddTransition(destinationState);

                    //Get value from enum based on same name of the state
                    var directionValue = GetDirectionValueFromEnum(directionName);
                    currStateToDestinationState.AddCondition(AnimatorConditionMode.Equals, directionValue, directionParam);

                    foreach (var transitionCondition in transitionConditions)
                    {
                        int thresholdValue = ((DefenseTypeEnum)Enum.Parse(typeof(DefenseTypeEnum), defenseType)).GetHashCode();
                        currStateToDestinationState.AddCondition(transitionCondition.conditionMode, thresholdValue, transitionCondition.parameterName);
                    }
                }
            }
        }

        void CreateAllWaysTransitionConditionAttackingStateToStandard(AnimatorStateMachine originStateMachines, AnimatorStateMachine destinationStateMachines, TransitionConditions[] transitionConditions, string standardStateName)
        {
            string directionName = string.Empty;
            string actionType = string.Empty;
            int sequence = 0;

            //Origin -> Destination
            foreach (var originStateMachine in originStateMachines.states)
            {
                var originState = originStateMachine.state;

                foreach (var destinationStateMachine in destinationStateMachines.states)
                {
                    var destinationState = destinationStateMachine.state;

                    //This stateMachine can only transit to another state machine from  a single state to another (i.e: Walking Left -> Combat Left)
                    if (originStateMachines.name == standardStateName.Split('_').Last())
                    {
                        directionName = destinationState.name.Split('_').First();
                        actionType = destinationState.name.Split('_').Skip(1).Take(1).First();
                        sequence = Convert.ToInt32(destinationState.name.Split('_').Last());
                    }
                    else
                    {
                        directionName = originState.name.Split('_').First();
                        actionType = originState.name.Split('_').Skip(1).Take(1).First();
                        sequence = Convert.ToInt32(originState.name.Split('_').Last());
                    }


                    var currStateToDestinationState = originState.AddTransition(destinationState);

                    //Get value from enum based on same name of the state
                    var directionValue = GetDirectionValueFromEnum(directionName);
                    currStateToDestinationState.AddCondition(AnimatorConditionMode.Equals, directionValue, directionParam);
                    currStateToDestinationState.AddCondition(AnimatorConditionMode.Equals, sequence, attackSequenceParam);

                    foreach (var transitionCondition in transitionConditions)
                    {
                        int thresholdValue = ((AttackTypeEnum)Enum.Parse(typeof(AttackTypeEnum), actionType)).GetHashCode();
                        currStateToDestinationState.AddCondition(transitionCondition.conditionMode, thresholdValue, transitionCondition.parameterName);
                    }
                }
            }
        }
        #endregion
    }
}
