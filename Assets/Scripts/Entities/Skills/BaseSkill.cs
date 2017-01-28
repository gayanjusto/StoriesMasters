using System;

namespace Assets.Scripts.Entities.Skills
{
    public abstract class BaseSkill
    {
        public string Name { get; set; }
        public double SkillValue { get; set; }
        protected  void EarnSkillPoint()
        {
            throw new NotImplementedException();
        }
    }
}
