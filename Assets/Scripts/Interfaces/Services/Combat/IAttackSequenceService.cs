using Assets.Scripts.Entities.ApplicationObjects;
using Assets.Scripts.Structs.Combat;

namespace Assets.Scripts.Interfaces.Services
{
    public interface IAttackSequenceService
    {
        AttackSequenceDelay GetNextAttackSequence(BaseAppObject attackerObj);
    }
}
