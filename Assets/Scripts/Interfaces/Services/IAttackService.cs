using UnityEngine;

namespace Assets.Scripts.Interfaces.Services
{
    public interface IAttackService
    {
        bool AttackTarget(GameObject attackerObj, GameObject targetObj);
        bool AttackTargets(GameObject attackerObj, GameObject[] targetsObjs);
    }
}
