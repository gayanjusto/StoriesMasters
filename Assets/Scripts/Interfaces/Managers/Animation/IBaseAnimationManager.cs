using Assets.Scripts.Enums;

namespace Assets.Scripts.Interfaces.Managers
{
    public interface IBaseAnimationManager
    {
        void PlayAttackAnimation(int attackSequence, AttackTypeEnum attackType);
    }
}
