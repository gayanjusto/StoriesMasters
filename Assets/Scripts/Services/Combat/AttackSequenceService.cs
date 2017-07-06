using Assets.Scripts.Entities.ApplicationObjects;
using Assets.Scripts.Interfaces.Managers.Combat;
using Assets.Scripts.Interfaces.Services;
using Assets.Scripts.Structs.Combat;
using System;

namespace Assets.Scripts.Services
{
    public class AttackSequenceService : IAttackSequenceService
    {
        public AttackSequenceDelay GetNextAttackSequence(BaseAppObject attackerObj)
        {
            IAttackSequenceManager attackSequenceManager = attackerObj.GetMonoBehaviourObject<IAttackSequenceManager>();
            int currentAttackSequence = attackSequenceManager.AttackSequence();

            DateTime? lastAttackTime = attackSequenceManager.LastAttackTime();
            DateTime currentTime = DateTime.Now;
            float totalTimeToResetSequence = 3;

            if (lastAttackTime.HasValue && (currentTime - lastAttackTime.Value).TotalSeconds > totalTimeToResetSequence)
            {
                return new AttackSequenceDelay()
                {
                    nextAttackSequence = 1,
                    delayTime = 5
                };
            }

            return  new AttackSequenceDelay()
            {
                nextAttackSequence = currentAttackSequence + 1,
                delayTime = 1
            };
        }
    }
}
