using Assets.Scripts.Entities.Skills.Combat;

namespace Assets.Scripts.Entities.Skills.Aggregators
{
    public class CombatSkills : BaseSkillSets
    {
        public CombatSkills()
        {
            SwordsSkill = new SwordsSkill() { Name = "Swords", SkillValue = 100 };
        }
        public SwordsSkill SwordsSkill { get; set; }
    }
}
