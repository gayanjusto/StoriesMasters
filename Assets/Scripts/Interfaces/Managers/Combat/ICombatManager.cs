using System.Collections;

namespace Assets.Scripts.Interfaces.Managers.Combat
{
    public interface ICombatManager
    {
        bool GetIsDefending();
        void SetIsDefending(bool isDefending);

        bool GetIsAttacking();

        bool CanCastAction();

        void InitiateAttack();
    }
}
