using Assets.Scripts.Entities.ApplicationObjects;
using Assets.Scripts.Entities.IntelligentBodies;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Interfaces.Services
{
    public interface IAttackService
    {
        bool Attack(BaseAppObject attackingObj);
        bool TargetIsBlockingAttackerDirections(BaseAppObject attacker, BaseAppObject target);
    }
}
