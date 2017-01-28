using Assets.Scripts.Entities.Itens.Weapons;
using Assets.Scripts.Entities.Skills.Aggregators;
using Assets.Scripts.Enums;
using System;

namespace Assets.Scripts.Entities.IntelligentBodies
{
    public class BaseCreature : BaseEntity
    {
        public BaseCreature()
        {
            Stamina = MaximumStamina;
            Health = MaximumHealth;
            CombatSkills = new CombatSkills();
        }

        public int Strength { get; set; }
        public int Intelligence { get; set; }
        public int Dexterity { get; set; }

        public CombatSkills CombatSkills { get; set;}

        private float defaultSpeedValue = 1.5f;
        private float defaultRunningCoefficient = 1.5f;
        private float defaultDiagonalSpeedDivisor = 2.4f;

        public int MaximumStamina
        {
            get { return (Strength + Dexterity)/2; }
        }
        public int MaximumHealth {
            get { return Strength * 10; }
        }

        public double Health { get; set; }
        public int Stamina { get; set; }


        public float GetStraightLineSpeed(bool isRunning)
        {
            if (isRunning)
            {
                return defaultSpeedValue * defaultRunningCoefficient;
            }

            return defaultSpeedValue;
        }

        public float GetDiagionalSpeed(bool isRunning)
        {
            float diagonalSpeedValue = defaultSpeedValue - (defaultSpeedValue / defaultDiagonalSpeedDivisor);

            if (isRunning)
            {
                float runningSpeedValue = diagonalSpeedValue * defaultRunningCoefficient;

                return runningSpeedValue;
            }
            return diagonalSpeedValue;
        }

        //Shall receive Weapon Weight (float) in the future to calculate
        //Less strength = minor sequence
        public int GetMaximumAttacks()
        {
            return 3 - 1; //Zero based
        }

        public float GetTimeToRecoverFromLastAttack(BaseWeaponItem equippedWeapon)
        {
            return (float)(((Strength + Dexterity) / equippedWeapon.Weight) / 10) + 1.0f;
        }

        public float GetTimeToRecoverFromAction(BaseWeaponItem equippedWeapon)
        {
            return (float)((Strength + Dexterity) / equippedWeapon.Weight) / 10;
        }

        public bool CanAttackTarget(BaseCreature target)
        {
            //return: Int + Dex + Skill + Rand 100 X enemie's
            return true;
        }

        public double GetDamageDealt(BaseWeaponItem equippedWeapon)
        {
            double skillValue = CombatSkills.GetSkillValueByName(equippedWeapon.SkillUsed);
            Random rnd = new Random();
            
            if(equippedWeapon.AttackType != AttackTypeEnum.Ranged)
            {
                return (Strength + Intelligence + equippedWeapon.Damage + skillValue + rnd.Next(0, 100)) / 5;
            }

            return (Dexterity + Intelligence + equippedWeapon.Damage + skillValue + rnd.Next(0, 100)) / 5;
        }

        public void ReceiveDamage(double damage)
        {
            //Damage received = (Resistance + Strength)/4 - damage
            Health -= damage;
        }
    }
}
