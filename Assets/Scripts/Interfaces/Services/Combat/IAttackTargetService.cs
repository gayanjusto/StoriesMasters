using Assets.Scripts.Entities.ApplicationObjects;

namespace Assets.Scripts.Interfaces.Services.Combat
{
    public interface IAttackTargetService
    {
        GameAppObject[] GetTargetsForAttack(GameAppObject attackingObj);
        GameAppObject[] GetFacingTargets(GameAppObject actionObj);
    }
}
