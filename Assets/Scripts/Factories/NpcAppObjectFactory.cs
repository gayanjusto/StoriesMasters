using Assets.Scripts.Entities.ApplicationObjects;
using Assets.Scripts.Entities.IntelligentBodies;
using Assets.Scripts.Interfaces.Managers.Attributes;
using Assets.Scripts.Interfaces.Managers.Behaviour;
using Assets.Scripts.Interfaces.Managers.Combat;
using Assets.Scripts.Interfaces.Managers.Components;
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
            NpcAppObject npcAppObj = new NpcAppObject(
                gameObject,
                baseCreature);

            return npcAppObj;
        }
    }
}
