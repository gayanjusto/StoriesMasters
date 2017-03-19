using Assets.Scripts.Entities.IntelligentBodies;
using Assets.Scripts.Interfaces.Managers.Attributes;
using Assets.Scripts.Interfaces.Managers.Combat;
using Assets.Scripts.Interfaces.Managers.Inputs;
using Assets.Scripts.Interfaces.Managers.Itens;
using Assets.Scripts.Interfaces.Managers.Movement;
using Assets.Scripts.Interfaces.Managers.Objects;
using Assets.Scripts.Managers.Inputs;
using UnityEngine;

namespace Assets.Scripts.Entities.ApplicationObjects
{
    public class PlayerAppObject : BaseAppObject
    {
        public PlayerAppObject(
            GameObject gameObject,
            Transform transform,
            BaseCreature baseCreature,
            IStaminaManager staminaManager,
            ICombatManager combatManager,
            IEquippedItensManager equippedItensManager,
            IMovementManager movementManager,
            IObjectManager objectManager,
            IPlayerCombatInputManager playerCombatInputManager) : base
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
            PlayerCombatInputManager = playerCombatInputManager;
        }

        public IPlayerCombatInputManager PlayerCombatInputManager { get; set; }

    }
}
