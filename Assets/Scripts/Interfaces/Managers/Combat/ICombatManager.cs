namespace Assets.Scripts.Interfaces.Managers.Combat
{
    public interface ICombatManager : IBaseManager
    {
        int GetAttackSequence();
        void DisableAttackerActions();
        void IncreaseSequenceWaitForAction();
        bool CanAttack();
    }
}
