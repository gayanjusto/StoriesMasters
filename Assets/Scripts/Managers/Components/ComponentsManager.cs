using Assets.Scripts.Interfaces.Managers.Components;
using System.Collections.Generic;

namespace Assets.Scripts.Managers.Components
{
    public abstract class ComponentsManager : BaseMonoBehaviour, IComponentsManager
    {
        public void DisableAllComponents()
        {
            BaseMonoBehaviour[] _components = GetAllComponents();
            if (_components != null)
            {
                for (int i = 0; i < _components.Length; i++)
                {
                    _components[i].Disable();
                }
            }
        }

        public void EnableAllComponents()
        {
            BaseMonoBehaviour[] _components = GetAllComponents();

            if (_components != null)
            {
                for (int i = 0; i < _components.Length; i++)
                {
                    _components[i].Enable();
                }
            }
        }

        protected abstract BaseMonoBehaviour[]  GetAllComponents();
    }
}
