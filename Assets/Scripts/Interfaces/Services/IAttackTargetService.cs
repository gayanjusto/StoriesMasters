using Assets.Scripts.Entities.ApplicationObjects;

namespace Assets.Scripts.Interfaces.Services
{
    public interface IAttackTargetService
    {
        BaseAppObject[] SetTargetsForAttack(BaseAppObject attackingObj);
    }
}
