using UnityEngine;

namespace Assets.Scripts.Interfaces.Managers.Movement
{
    public interface INpcFacingDirectionManager : IBaseMonoBehaviour
    {
        void SetCurrentFacingTarget(Transform target);
    }
}
