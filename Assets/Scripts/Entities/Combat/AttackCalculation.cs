using Assets.Scripts.Entities.IntelligentBodies;
using Assets.Scripts.Enums;
using Assets.Scripts.Utils;
using System;

namespace Assets.Scripts.Entities.Combat
{
    public struct AttackCalculation
    {
        private Attributes _attributes;

        public AttackCalculation(Attributes attributes)
        {
            _attributes = attributes;
        }
        public float GetAttackValue(float usingSkillValue, AttackTypeEnum attackType)
        {
            float relatedAttributeValue = _attributes.Strength;

            if(attackType == AttackTypeEnum.Ranged)
            {
                relatedAttributeValue = _attributes.Dexterity;
            }

            return (usingSkillValue + relatedAttributeValue + RandomNumberGenerator.GetRandomValue(1, 100)) / 300;
        }
    }
}
