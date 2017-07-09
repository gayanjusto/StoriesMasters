using Assets.Scripts.Entities.ApplicationObjects;
using Assets.Scripts.Enums;

namespace Assets.Scripts.Interfaces.Services
{
    public interface IAttackTypeService
    {
        void SetAttackTypeForPlayer(GameAppObject playerObj, float inputHoldTime);
        AttackTypeEnum GetStrongestAttackFromArray(AttackTypeEnum[] attacksType);
        AttackTypeEnum GetQuickestAttackFromArray(AttackTypeEnum[] attacksType);
    }
}
