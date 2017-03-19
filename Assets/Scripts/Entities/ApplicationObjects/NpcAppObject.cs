using Assets.Scripts.Entities.IntelligentBodies;
using Assets.Scripts.Interfaces.Managers.Attributes;
using Assets.Scripts.Interfaces.Managers.Behaviour;
using Assets.Scripts.Interfaces.Managers.Combat;
using Assets.Scripts.Interfaces.Managers.Itens;
using Assets.Scripts.Interfaces.Managers.Movement;
using Assets.Scripts.Interfaces.Managers.Objects;
using UnityEngine;

namespace Assets.Scripts.Entities.ApplicationObjects
{
    public class NpcAppObject : BaseAppObject
    {

        public ILineOfSightManager LineOfSightManager { get; set; }

        public NpcAppObject(
            GameObject gameObject,
            Transform transform,
            BaseCreature baseCreature,
            IStaminaManager staminaManager,
            ICombatManager combatManager,
            IEquippedItensManager equippedItensManager,
            IMovementManager movementManager,
            IObjectManager objectManager,
            ILineOfSightManager lineOfSightManager) : base 
            (
                gameObject,
                transform,
                baseCreature,
                staminaManager,
                combatManager,
                equippedItensManager,
                movementManager,
                objectManager
                )
        {
            LineOfSightManager = lineOfSightManager;
        }
    }
}
