using Assets.Scripts.Entities.ApplicationObjects;

namespace Assets.Scripts.Interfaces.Services
{
    public interface IAttackTargetService
    {
        BaseAppObject[] GetTargetsForAttack(BaseAppObject attackingObj);
        BaseAppObject GetFacingTarget(BaseAppObject actionObj);
    }
}
