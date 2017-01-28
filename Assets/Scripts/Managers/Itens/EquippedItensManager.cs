using Assets.Scripts.Entities.Itens.Weapons;
using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces.Managers.Itens;

namespace Assets.Scripts.Managers.Itens
{
    public class EquippedItensManager : BaseManager, IEquippedItensManager
    {
        private BaseWeaponItem _equippedWeapon;

        public BaseWeaponItem GetEquippedWeapon()
        {
            return new BaseWeaponItem()
            {
                Weight = 2.0,
                Damage = 40,
                SkillUsed = "SwordsSkill",
                AttackType = AttackTypeEnum.Stock
            };
            return _equippedWeapon;
        }

        public void SetEquippedWeapon(BaseWeaponItem equippingWeapon)
        {
            _equippedWeapon = equippingWeapon;
        }
    }
}
