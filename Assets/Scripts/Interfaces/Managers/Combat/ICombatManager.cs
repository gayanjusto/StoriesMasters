namespace Assets.Scripts.Interfaces.Managers.Combat
{
    public interface ICombatManager : IBaseManager
    {
        int GetAttackSequence();
        void EnableAttackerActions();
        void DisableAttackerActions();
        void DisableAttackerActions(float freezeTime);
        void IncreaseSequenceWaitForAction();
        bool CanAttack();
    }
}
