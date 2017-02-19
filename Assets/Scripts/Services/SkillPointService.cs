using Assets.Scripts.Entities.IntelligentBodies;
using Assets.Scripts.Entities.Skills;
using Assets.Scripts.Interfaces.Managers.Itens;
using Assets.Scripts.Interfaces.Managers.Objects;
using Assets.Scripts.Interfaces.Services;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class SkillPointService : ISkillPointService
    {
        public float IncreaseCombatSkillPoint(GameObject objEarningPoint)
        {
           string skillUsed = objEarningPoint.GetComponent<IEquippedItensManager>()
                .GetEquippedWeapon().SkillUsed;

            BaseCreature objEntity = objEarningPoint.GetComponent<IObjectManager>().GetBaseCreature();

            BaseSkill skill = objEntity.CombatSkills.GetSkillByName(skillUsed);

            skill.EarnSkillPoint();

            return skill.SkillValue;
        }
    }
}
