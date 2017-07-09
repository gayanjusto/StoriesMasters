using Assets.Scripts.Entities.Skills.Combat;

namespace Assets.Scripts.Entities.Skills.Aggregators
{
    public class CombatSkills : BaseSkillSets
    {
        public CombatSkills()
        {
            SwordsSkill = new SwordsSkill() { Name = "Swords", SkillValue = 100 };
            ParrySkill = new ParrySkill() { Name = "Parry", SkillValue = 100 };
            ShieldsSkill = new ShieldsSkill() { Name = "Shields", SkillValue = 100 };
            DodgeSkill = new DodgeSkill() { Name = "Dodge", SkillValue = 100 };

        }
        public SwordsSkill SwordsSkill { get; set; }
        public ParrySkill ParrySkill { get; set; }
        public ShieldsSkill ShieldsSkill { get; set; }
        public DodgeSkill DodgeSkill { get; set; }

    }
}
