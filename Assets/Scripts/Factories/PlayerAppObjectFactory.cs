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

            PlayerAppObject playerAppObj = new PlayerAppObject(
                gameObject,
                baseCreature);

            return playerAppObj;
        }
    }
}
