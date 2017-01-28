namespace Assets.Scripts.Interfaces.Managers.Attributes
{
    public interface IStaminaManager : IBaseManager
    {
        void SetDecreasingStamina(bool value);
        bool GetDecreasingStamina();
    }
}
