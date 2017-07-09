using Assets.Scripts.Entities.Combat;
using Assets.Scripts.Entities.IntelligentBodies;
using Assets.Scripts.Entities.Movement;
using Assets.Scripts.Entities.Skills.Aggregators;
using UnityEngine;

namespace Assets.Scripts.Entities.ApplicationObjects
{
    public class GameAppObject
    {
        public GameAppObject(GameObject gameObject)
        {
            //classes
            this.gameObject = gameObject;
            attributes = new Attributes();
            combatSkills = new CombatSkills();

            //structs
            moveSpeed = new MoveSpeed();
            attackDelay = new AttackDelay();
            attackCalculation = new AttackCalculation(this.attributes);
            defenseCalculation = new DefenseCalculation(this.attributes);
        }

        //classes
        public GameObject gameObject;
        public Attributes attributes;
        public CombatSkills combatSkills;

        //structs
        public MoveSpeed moveSpeed;
        public AttackDelay attackDelay;
        public AttackCalculation attackCalculation;
        public DefenseCalculation defenseCalculation;

    }
}
