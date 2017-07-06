using Assets.Scripts.Entities.ApplicationObjects;
using Assets.Scripts.Enums;

namespace Assets.Scripts.Interfaces.Managers.Combat
{
    public interface IDefenseStatusManager
    {
        DefenseTypeEnum CurrentDefenseType();
        bool IsDefending();
        bool IsAttemptingToParry();
        bool IsBlockingWithShield();
        void SetIsBlocking(bool isBlocking);
    }
}
