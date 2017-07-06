using Assets.Scripts.Enums;

namespace Assets.Scripts.Interfaces.Managers.Combat
{
    public interface IAttackStatusManager
    {
        bool CanAttack();
        void SetIsAttacking(bool isAttacking);
        bool IsAttacking();
        void SetAttackType(AttackTypeEnum attackType);
        AttackTypeEnum CurrentAttackType();
        void RecoverFromAttack(float delayTime);
        void SetCombatActive(bool combatActive);
        bool IsCombatActive();

    }
}
