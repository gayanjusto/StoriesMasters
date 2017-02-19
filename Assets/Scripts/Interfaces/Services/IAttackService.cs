using Assets.Scripts.Entities.IntelligentBodies;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Interfaces.Services
{
    public interface IAttackService
    {
        BaseCreature AttackTarget(GameObject attackerObj, GameObject targetObj);
        IList<BaseCreature> AttackTargets(GameObject attackerObj, GameObject[] targetsObjs);
    }
}
