using Assets.Scripts.Entities.ApplicationObjects;

namespace Assets.Scripts.Interfaces.Services.Combat
{
    public interface IShieldDefenseService
    {
        void ActivateShieldDefense(GameAppObject defendingObj);
        void DeactivateShieldDefense(GameAppObject defendingObj);
    }
}
