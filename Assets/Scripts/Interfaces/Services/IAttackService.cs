using Assets.Scripts.Entities.ApplicationObjects;
using Assets.Scripts.Entities.IntelligentBodies;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Interfaces.Services
{
    public interface IAttackService
    {
        bool Attack(BaseAppObject attackingObj);
        bool TargetIsBlockingAttackerDirectionsWithShield(BaseAppObject attacker, BaseAppObject target);
        void MiniStunTarget(BaseAppObject target);
        void MiniStunTarget(BaseAppObject target, float timeToStun);
        float GetTimeForAttackDelay(BaseAppObject target);
    }
}
