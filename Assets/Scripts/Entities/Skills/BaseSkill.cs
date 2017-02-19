using System;

namespace Assets.Scripts.Entities.Skills
{
    public abstract class BaseSkill
    {
        public string Name { get; set; }
        public float SkillValue { get; set; }

        public  void EarnSkillPoint()
        {
            if(SkillValue >= 100)
            {
                return;
            }

            float pointEarned = (1 / SkillValue);
            if(pointEarned > 1)
            {
                pointEarned = 1;
            }

            SkillValue += pointEarned;
        }
    }
}
