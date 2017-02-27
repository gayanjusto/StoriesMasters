using UnityEngine;

namespace Assets.Scripts.Interfaces.Managers.Movement
{
    public interface INpcMovementManager : IBaseManager
    {
        float GetDistanceFromTarget();
        void SetTarget(GameObject target);
    }
}
