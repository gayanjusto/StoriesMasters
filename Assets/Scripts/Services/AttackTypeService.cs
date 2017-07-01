using Assets.Scripts.Entities.ApplicationObjects;
using Assets.Scripts.Entities.Itens.Equippable;
using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces.Managers.Itens;
using Assets.Scripts.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class AttackTypeService : IAttackTypeService
    {
        public void SetAttackTypeForPlayer(BaseAppObject playerObj, float inputHoldTime)
        {
            var playerEquipItensManager = playerObj.GetMonoBehaviourObject<IEquippedItensManager>();
            if (playerEquipItensManager.GetEquippedWeapon() != null)
            {
                BaseEquippableItem equippedWeapon = playerEquipItensManager.GetEquippedWeapon();

                //If player has hold for a heavy attack, it should pick the heaviest type of attack available for the weapon
                if (inputHoldTime > 3)
                {
                    AttackTypeEnum strongestAttack = GetStrongestAttackFromArray(equippedWeapon.AttacksType);
                    equippedWeapon.CurrentAttackType = strongestAttack;
                    Debug.Log(strongestAttack.ToString());
                    return;
                }

                //If player has made a quick tap at the input, it should attack if a quick attack
                AttackTypeEnum quickestAttack = GetQuickestAttackFromArray(equippedWeapon.AttacksType);
                Debug.Log(quickestAttack.ToString());
                equippedWeapon.CurrentAttackType = quickestAttack;
            }
        }

        public AttackTypeEnum GetStrongestAttackFromArray(AttackTypeEnum[] attacksType)
        {
            return attacksType.Max(x => x);
        }

        public AttackTypeEnum GetQuickestAttackFromArray(AttackTypeEnum[] attacksType)
        {
            return attacksType.Min(x => x);
        }
    }
}
