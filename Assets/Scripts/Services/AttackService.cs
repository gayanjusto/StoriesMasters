using Assets.Scripts.Entities.IntelligentBodies;
using Assets.Scripts.Interfaces.Managers.Itens;
using Assets.Scripts.Interfaces.Managers.Objects;
using Assets.Scripts.Interfaces.Services;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class AttackService : IAttackService
    {
        public BaseCreature AttackTarget(GameObject attackerObj, GameObject targetObj)
        {
            BaseCreature attacker = attackerObj.GetComponent<IObjectManager>().GetBaseCreature();
            BaseCreature target = targetObj.GetComponent<IObjectManager>().GetBaseCreature();

            if (attacker.CanAttackTarget(target))
            {
                var equippedWeapon = attackerObj.GetComponent<IEquippedItensManager>().GetEquippedWeapon();
                double damageDealtToTarget = attacker.GetDamageDealt(equippedWeapon);
                target.ReceiveDamage(damageDealtToTarget);

                return target;
            }
            return null;
        }

        public IList<BaseCreature> AttackTargets(GameObject attackerObj, GameObject[] targetsObjs)
        {
            BaseCreature attacker = attackerObj.GetComponent<IObjectManager>().GetBaseCreature();
            var equippedWeapon = attackerObj.GetComponent<IEquippedItensManager>().GetEquippedWeapon();
            double damageDealtToTarget = attacker.GetDamageDealt(equippedWeapon);

            List<BaseCreature> creaturesHit = new List<BaseCreature>();
            for (int i = 0; i < targetsObjs.Length; i++)
            {
                BaseCreature target = targetsObjs[i].GetComponent<IObjectManager>().GetBaseCreature();

                if (!attacker.CanAttackTarget(target))
                {
                    return creaturesHit; //If target has simply dodged, it should continue the attack to the next target
                }

                //Decrease damage by each target
                if (i > 0)
                {
                    damageDealtToTarget -= (damageDealtToTarget * (i * 10)) / (100);
                }

                target.ReceiveDamage(damageDealtToTarget);
                creaturesHit.Add(target);
            }
            return creaturesHit;
        }
    }
}
