using Assets.Scripts.Entities.ApplicationObjects;
using Assets.Scripts.Entities.IntelligentBodies;
using Assets.Scripts.Interfaces.Managers.Attributes;
using Assets.Scripts.Interfaces.Managers.Behaviour;
using Assets.Scripts.Interfaces.Managers.Combat;
using Assets.Scripts.Interfaces.Managers.Itens;
using Assets.Scripts.Interfaces.Managers.Movement;
using Assets.Scripts.Interfaces.Managers.Objects;
using UnityEngine;

namespace Assets.Scripts.Factories
{
    public static class NpcAppObjectFactory
    {
        public static NpcAppObject Create(GameObject gameObject, BaseCreature baseCreature)
        {
            IStaminaManager staminaManager = gameObject.GetComponent<IStaminaManager>();
            ICombatManager combatManager = gameObject.GetComponent<ICombatManager>();
            IEquippedItensManager equippedItensManager = gameObject.GetComponent<IEquippedItensManager>();
            IMovementManager movementManager = gameObject.GetComponent<IMovementManager>();
            IObjectManager objectManager = gameObject.GetComponent<IObjectManager>();
            ILineOfSightManager lineOfSightManager = gameObject.GetComponent<ILineOfSightManager>();

            NpcAppObject npcAppObj = new NpcAppObject(
                gameObject,
                gameObject.transform,
                baseCreature,
                staminaManager,
                combatManager,
                equippedItensManager,
                movementManager,
                objectManager,
                lineOfSightManager);

            return npcAppObj;
        }
    }
}
