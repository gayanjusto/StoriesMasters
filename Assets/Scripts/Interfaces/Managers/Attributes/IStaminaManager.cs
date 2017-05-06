namespace Assets.Scripts.Interfaces.Managers.Attributes
{
    public interface IStaminaManager : IBaseMonoBehaviour
    {
        void SetDecreasingStamina(bool value);
        bool GetDecreasingStamina();
    }
}
