using System.Reflection;

namespace Assets.Scripts.Entities.Skills.Aggregators
{
    public abstract class BaseSkillSets
    {
        public double GetSkillValueByName(string skillName)
        {
            object skillObj = GetType().GetProperty(skillName).GetValue(this, null);
            return (double)skillObj.GetType().GetProperty("SkillValue").GetValue(skillObj, null);
        }
    }
}
