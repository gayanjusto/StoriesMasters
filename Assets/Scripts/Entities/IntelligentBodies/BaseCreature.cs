using Assets.Scripts.Entities.Skills.Aggregators;

namespace Assets.Scripts.Entities.IntelligentBodies
{
    public class BaseCreature
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

        public int MaximumStamina
        {
            get { return (Strength + Dexterity)/2; }
        }

        public int MaximumHealth {
            get { return Strength * 10; }
        }

        public double Health { get; set; }

        public int Stamina { get; set; }

    }
}
