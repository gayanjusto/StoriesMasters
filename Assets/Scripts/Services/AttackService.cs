using Assets.Scripts.Entities.IntelligentBodies;
using Assets.Scripts.Interfaces.Managers.Itens;
using Assets.Scripts.Interfaces.Managers.Objects;
using Assets.Scripts.Interfaces.Services;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class AttackService : IAttackService
    {
        public bool AttackTarget(GameObject attackerObj, GameObject targetObj)
        {
            BaseCreature attacker = attackerObj.GetComponent<IObjectManager>().GetBaseCreature();
            BaseCreature target = targetObj.GetComponent<IObjectManager>().GetBaseCreature();

            if (attacker.CanAttackTarget(target))
            {
                var equippedWeapon = attackerObj.GetComponent<IEquippedItensManager>().GetEquippedWeapon();
                double damageDealtToTarget = attacker.GetDamageDealt(equippedWeapon);
                target.ReceiveDamage(damageDealtToTarget);

                return true;
            }
            return false;
        }

        public bool AttackTargets(GameObject attackerObj, GameObject[] targetsObjs)
        {
            BaseCreature attacker = attackerObj.GetComponent<IObjectManager>().GetBaseCreature();
            var equippedWeapon = attackerObj.GetComponent<IEquippedItensManager>().GetEquippedWeapon();

            for (int i = 0; i < targetsObjs.Length; i++)
            {
                BaseCreature target = targetsObjs[i].GetComponent<IObjectManager>().GetBaseCreature();

                if (!attacker.CanAttackTarget(target))
                {
                    return false;
                }
                double damageDealtToTarget = attacker.GetDamageDealt(equippedWeapon);

                //Decrease damage by each target
                if (i > 0)
                {
                    damageDealtToTarget -= (damageDealtToTarget * (i * 10)) / (100);
                }
            }
            return true;
        }
    }
}
