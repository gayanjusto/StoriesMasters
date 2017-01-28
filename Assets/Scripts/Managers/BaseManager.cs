using Assets.Scripts.Interfaces.Managers;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class BaseManager : MonoBehaviour, IBaseManager
    {
        public void Enable()
        {
            enabled = true;
        }

        public void Disable()
        {
            enabled = false;
        }

        public bool IsEnabled()
        {
            return enabled;
        }
    }
}
