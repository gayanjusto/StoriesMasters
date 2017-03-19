using Assets.Scripts.Entities.Itens.Equippable;

namespace Assets.Scripts.Interfaces.Managers.Itens
{
    public interface IEquippedItensManager
    {
        BaseEquippableItem GetEquippedWeapon();
        BaseEquippableItem GetDefenseItem();
        void SetEquippedWeapon(BaseEquippableItem equippingWeapon);

    }
}
