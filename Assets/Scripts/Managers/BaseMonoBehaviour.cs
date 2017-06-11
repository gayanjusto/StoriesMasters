using Assets.Scripts.Interfaces.Managers;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class BaseMonoBehaviour : MonoBehaviour, IBaseMonoBehaviour
    {
        public virtual void Enable()
        {
            enabled = true;
        }

        public virtual void Disable()
        {
            enabled = false;
        }

        public bool IsEnabled()
        {
            return enabled;
        }
    }
}
