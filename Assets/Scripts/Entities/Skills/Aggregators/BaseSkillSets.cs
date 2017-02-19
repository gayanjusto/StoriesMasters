using System.Reflection;

namespace Assets.Scripts.Entities.Skills.Aggregators
{
    public abstract class BaseSkillSets
    {
        public double GetSkillValueByName(string skillName)
        {
            object skillObj = GetType().GetProperty(skillName).GetValue(this, null);
            return (float)skillObj.GetType().GetProperty("SkillValue").GetValue(skillObj, null);
        }

        public BaseSkill GetSkillByName(string skillName)
        {
            object skillObj = GetType().GetProperty(skillName).GetValue(this, null);
            return (BaseSkill)skillObj;
        }
    }
}
