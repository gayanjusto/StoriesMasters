using UnityEngine;

namespace Assets.Scripts.Interfaces.Managers.Movement
{
    public interface INpcMovementManager : IBaseMonoBehaviour
    {
        float GetDistanceFromTarget();
        void SetTarget(GameObject target);
    }
}
