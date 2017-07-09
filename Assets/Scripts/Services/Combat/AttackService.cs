using Assets.Scripts.Entities.ApplicationObjects;
using Assets.Scripts.Interfaces.Services.Combat;
using Assets.Scripts.Factories.Services.Combat;
using UnityEngine;
using Assets.Scripts.Interfaces.Managers.Combat;
using System;
using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces.Managers.Itens;

namespace Assets.Scripts.Services.Combat
{
    public class AttackService : IAttackService
    {

        public void AttackTargets(GameAppObject[] targets, GameAppObject attacker)
        {
            //-> Get Attacker CurrentSkill by CurrentEquippedWeapon
            var equippedWeapon = attacker.gameObject.GetComponent<IEquippedItensManager>().GetEquippedWeapon();

            float attackerSkillValue = attacker.combatSkills.GetSkillValueByName(equippedWeapon.SkillUsed);

            //-> Get attacker current attack type
            AttackTypeEnum attackType = AttackTypeEnum.AttackThrust;

            //Debug -> Remove
            attacker.attributes.Strength = 100;

            for (int i = 0; i < targets.Length; i++)
            {
                //calculate chance of hitting target
                //-> Get attacker value of attack chance
                float attackerAttackValue = attacker.attackCalculation.GetAttackValue(attackerSkillValue, attackType);

                //Get target's current skill value based on its current equipped defense
                var targetEquippedDefense = targets[i].gameObject.GetComponent<IEquippedItensManager>().GetDefenseItem();

                float targetDefenseSkillValue = 0;

                //Get target's current defense system, if none: consider dodge
                DefenseTypeEnum targetDefenseType = DefenseTypeEnum.ShieldBlock;

                //If there's not equippedDefense, target will use Dodge skill
                if (targetEquippedDefense == null)
                {
                    targetDefenseSkillValue = targets[i].combatSkills.DodgeSkill.SkillValue;
                    targetDefenseType = DefenseTypeEnum.Dodge;
                }
                else
                {
                    targetDefenseSkillValue = targets[i].combatSkills.GetSkillValueByName(targetEquippedDefense.SkillUsed);
                    targetDefenseType = targetEquippedDefense.DefenseType;
                }

                //Get target's chance
                float targetDefenseValue = targets[i].defenseCalculation.GetDefenseValue(targetDefenseSkillValue, targetDefenseType);

                //Debug -> Remove
                targets[i].attributes.Strength = 100;

                //if target is hit, deal damage to it
                if (attackerAttackValue > targetDefenseValue)
                {
                    Debug.Log(string.Format("Target hit. Attacker Value: {0} / Target Value: {1}", attackerAttackValue, targetDefenseValue));
                }
                else
                {
                    Debug.Log(string.Format("Target NOT hit. Attacker Value: {0} / Target Value: {1}", attackerAttackValue, targetDefenseValue));
                }

                //Get chance of skill increase for attacker
                //if able to increase, increase current attacking skill

                //GetChance of skill increase for target's defenses
                //if able to increase, increase current defense skill

                //Get chance of attribute increase for attacker
                //if able to increase, increase used attribute for attack

                //Get chance of attribute increase for target
                //if able to increase, increase used attribute for defense
            }
        }
    }
}
