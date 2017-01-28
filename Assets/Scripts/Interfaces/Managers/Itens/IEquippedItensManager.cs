using Assets.Scripts.Entities.Itens.Weapons;

namespace Assets.Scripts.Interfaces.Managers.Itens
{
    public interface IEquippedItensManager
    {
        BaseWeaponItem GetEquippedWeapon();
        void SetEquippedWeapon(BaseWeaponItem equippingWeapon);

    }
}
