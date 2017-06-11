using Assets.Scripts.Interfaces.Managers.Attributes;
using Assets.Scripts.Interfaces.Managers.Combat;
using Assets.Scripts.Interfaces.Managers.Inputs;
using Assets.Scripts.Interfaces.Managers.Itens;
using Assets.Scripts.Interfaces.Managers.Movement;
using Assets.Scripts.Interfaces.Managers.Objects;
using Assets.Scripts.Interfaces.Observers;
using Assets.Scripts.Managers.Attributes;
using Assets.Scripts.Managers.Combat;
using Assets.Scripts.Managers.Inputs;
using Assets.Scripts.Managers.Itens;
using Assets.Scripts.Managers.Objects;
using Assets.Scripts.Observers;
using System.Collections.Generic;
using System;

namespace Assets.Scripts.Managers.Components
{
    public class PlayerComponentsManager : ComponentsManager
    {
        protected override BaseMonoBehaviour[] GetAllComponents()
        {
            BaseMonoBehaviour[] _components = new BaseMonoBehaviour[8];
            _components[0]= gameObject.GetComponent<PlayerMovementInputManager>();
            _components[1] = gameObject.GetComponent<StaminaManager>();
            _components[2] = gameObject.GetComponent<PlayerCombatManager>();
            _components[3] = gameObject.GetComponent<EquippedItensManager>();
            _components[4] = gameObject.GetComponent<PlayerObjectManager>();
            _components[5] = gameObject.GetComponent<MovementObserver>();
            _components[6] = gameObject.GetComponent<AttackObserver>();
            _components[7] = gameObject.GetComponent<PlayerCombatInputManager>();

            return _components;
        }

        private void Awake()
        {
            SetPlayerComponents();
        }

        private void SetPlayerComponents()
        {
            if(gameObject.GetComponent<IMovementManager>() == null)
            {
                gameObject.AddComponent<PlayerMovementInputManager>();
            }

            if (gameObject.GetComponent<IStaminaManager>() == null)
            {
                gameObject.AddComponent<StaminaManager>();
            }

            if (gameObject.GetComponent<ICombatManager>() == null)
            {
                gameObject.AddComponent<PlayerCombatManager>();
            }

            if (gameObject.GetComponent<IEquippedItensManager>() == null)
            {
                gameObject.AddComponent<EquippedItensManager>();
            }

            if (gameObject.GetComponent<IObjectManager>() == null)
            {
                gameObject.AddComponent<PlayerObjectManager>();
            }

            if (gameObject.GetComponent<IMovementObserver>() == null)
            {
                gameObject.AddComponent<MovementObserver>();
            }

            if (gameObject.GetComponent<IAttackObserver>() == null)
            {
                gameObject.AddComponent<AttackObserver>();
            }

            if (gameObject.GetComponent<IPlayerCombatInputManager>() == null)
            {
                gameObject.AddComponent<PlayerCombatInputManager>();
            }
        }
    }
}
