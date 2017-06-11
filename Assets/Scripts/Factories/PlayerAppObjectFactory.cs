using Assets.Scripts.Entities.ApplicationObjects;
using Assets.Scripts.Entities.IntelligentBodies;
using Assets.Scripts.Interfaces.Managers.Attributes;
using Assets.Scripts.Interfaces.Managers.Combat;
using Assets.Scripts.Interfaces.Managers.Components;
using Assets.Scripts.Interfaces.Managers.Inputs;
using Assets.Scripts.Interfaces.Managers.Itens;
using Assets.Scripts.Interfaces.Managers.Movement;
using Assets.Scripts.Interfaces.Managers.Objects;
using UnityEngine;

namespace Assets.Scripts.Factories
{
    public class PlayerAppObjectFactory
    {
        public static PlayerAppObject Create(GameObject gameObject, BaseCreature baseCreature)
        {
            IStaminaManager staminaManager = gameObject.GetComponent<IStaminaManager>();
            ICombatManager combatManager = gameObject.GetComponent<ICombatManager>();
            IEquippedItensManager equippedItensManager = gameObject.GetComponent<IEquippedItensManager>();
            IMovementManager movementManager = gameObject.GetComponent<IMovementManager>();
            IObjectManager objectManager = gameObject.GetComponent<IObjectManager>();
            IPlayerCombatInputManager playerCombatInputManager = gameObject.GetComponent<IPlayerCombatInputManager>();
            IComponentsManager componentsManager = gameObject.GetComponent<IComponentsManager>();


            PlayerAppObject playerAppObj = new PlayerAppObject(
                gameObject,
                gameObject.transform,
                baseCreature,
                staminaManager,
                combatManager,
                equippedItensManager,
                movementManager,
                objectManager,
                playerCombatInputManager,
                componentsManager);

            return playerAppObj;
        }
    }
}
