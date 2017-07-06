using Assets.Scripts.Interfaces.Managers.Attributes;
using Assets.Scripts.Interfaces.Managers.Combat;
using Assets.Scripts.Interfaces.Managers.Itens;
using Assets.Scripts.Interfaces.Managers.Movement;
using Assets.Scripts.Interfaces.Managers.Objects;
using Assets.Scripts.Interfaces.Observers;
using Assets.Scripts.Managers.Attributes;
using Assets.Scripts.Managers.Behaviour;
using Assets.Scripts.Managers.Combat;
using Assets.Scripts.Managers.Itens;
using Assets.Scripts.Managers.Movement;
using Assets.Scripts.Managers.Objects;
using Assets.Scripts.Observers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Managers.Components
{
    public class NpcComponentsManager : ComponentsManager
    {
        protected override BaseMonoBehaviour[] GetAllComponents()
        {
            BaseMonoBehaviour[] _components = new BaseMonoBehaviour[9];
            _components[0] = gameObject.GetComponent<NpcCombatManager>();
            _components[1] = gameObject.GetComponent<NpcAttackManager>();
            _components[2] = gameObject.GetComponent<NpcMovementManager>();
            _components[3] = gameObject.GetComponent<EquippedItensManager>();
            _components[4] = gameObject.GetComponent<NpcObjectManager>();
            _components[5] = gameObject.GetComponent<StaminaManager>();
            _components[6] = gameObject.GetComponent<MovementObserver>();
            _components[7] = gameObject.GetComponent<AttackObserver>();
            _components[8] = gameObject.transform.FindChild("LineOfSightCollider").GetComponent<LineOfSightManager>();

            return _components;
        }

        private void Awake()
        {
            SetNpcComponents();
        }

        private void SetNpcComponents()
        {
            if (gameObject.GetComponent<INpcCombatManager>() == null)
            {
                gameObject.AddComponent<NpcCombatManager>();
            }

            if (gameObject.GetComponent<INpcAttackManager>() == null)
            {
                gameObject.AddComponent<NpcAttackManager>();
            }

            if (gameObject.GetComponent<IMovementManager>() == null)
            {
                gameObject.AddComponent<NpcMovementManager>();
            }

            if (gameObject.GetComponent<IEquippedItensManager>() == null)
            {
                gameObject.AddComponent<EquippedItensManager>();
            }

            if (gameObject.GetComponent<IObjectManager>() == null)
            {
                gameObject.AddComponent<NpcObjectManager>();
            }

            if (gameObject.GetComponent<IStaminaManager>() == null)
            {
                gameObject.AddComponent<StaminaManager>();
            }

            if (gameObject.GetComponent<IMovementObserver>() == null)
            {
                gameObject.AddComponent<MovementObserver>();
            }

            if (gameObject.GetComponent<IAttackObserver>() == null)
            {
                gameObject.AddComponent<AttackObserver>();
            }

            if(gameObject.transform.FindChild("LineOfSightCollider") == null)
            {
                //Instantiate prefab of LifeOfSightCollider
                GameObject losCollider = (GameObject) GameObject.Instantiate(Resources.Load("Enemy/Prefab_LineOfSightCollider"));
                losCollider.transform.parent = transform;
                losCollider.transform.position = new Vector3(0, 0, 0);
            }
        }
    }
}
