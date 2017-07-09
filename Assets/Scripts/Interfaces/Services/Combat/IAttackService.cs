using Assets.Scripts.Entities.ApplicationObjects;

namespace Assets.Scripts.Interfaces.Services.Combat
{
    public interface IAttackService
    {
        void AttackTargets(GameAppObject[] targets, GameAppObject attacker);
    }
}
