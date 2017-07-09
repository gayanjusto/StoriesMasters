using Assets.Scripts.Entities.IntelligentBodies;
using Assets.Scripts.Enums;
using Assets.Scripts.Utils;
using System;

namespace Assets.Scripts.Entities.Combat
{
    public struct DefenseCalculation
    {
        private Attributes _attributes;

        public DefenseCalculation(Attributes attributes)
        {
            _attributes = attributes;
        }
        public float GetDefenseValue(float usingSkillValue, DefenseTypeEnum defenseType)
        {
            float relatedAttributeValue = _attributes.Strength;

            if (defenseType == DefenseTypeEnum.Dodge)
            {
                relatedAttributeValue = _attributes.Dexterity;
            }

            return (usingSkillValue + relatedAttributeValue + RandomNumberGenerator.GetRandomValue(1, 100)) / 300;
        }
    }
}
