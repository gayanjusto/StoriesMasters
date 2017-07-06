using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces.Managers.Itens;
using Assets.Scripts.Entities.Itens.Equippable;

namespace Assets.Scripts.Managers.Itens
{
    public class EquippedItensManager : BaseMonoBehaviour, IEquippedItensManager
    {
        private BaseEquippableItem _equippedWeapon;
        private BaseEquippableItem _defaultWeapon;
        private BaseEquippableItem _equippedDefense;

        public BaseEquippableItem GetEquippedWeapon()
        {
            return new BaseEquippableItem()
            {
                Weight = 2.0,
                Damage = 40,
                SkillUsed = "SwordsSkill",
                AttacksType = new[] { AttackTypeEnum.AttackSwing, AttackTypeEnum.AttackThrust },
                ItemType = EquippableItemTypeEnum.Weapon,
                WeaponRange = 2.5f
            };

            if (_equippedWeapon == null)
            {
                return _defaultWeapon;
            }
           
            return _equippedWeapon;
        }

        public void SetEquippedWeapon(BaseEquippableItem equippingWeapon)
        {
            _equippedWeapon = equippingWeapon;
        }

        public BaseEquippableItem GetDefenseItem()
        {
            if(_equippedWeapon == null || _equippedWeapon.SkillUsed == "Wrestling")
            {
                return null;
            }

            return _equippedDefense;
        }
    }
}
