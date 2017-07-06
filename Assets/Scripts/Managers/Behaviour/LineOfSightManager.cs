using Assets.Scripts.Constants;
using Assets.Scripts.Interfaces.Managers.Behaviour;
using Assets.Scripts.Interfaces.Managers.Combat;
using Assets.Scripts.Interfaces.Managers.Movement;
using UnityEngine;

namespace Assets.Scripts.Managers.Behaviour
{
    public class LineOfSightManager : BaseMonoBehaviour, ILineOfSightManager
    {
        INpcAttackManager _npcTargetingManager;
        INpcMovementManager _npcMovementManager;
        INpcCombatManager _npcCombatManager;
        INpcFacingDirectionManager _npcFacingDirectionManager;

        private void Start()
        {
            _npcTargetingManager = transform.parent.GetComponent<INpcAttackManager>();
            _npcMovementManager = transform.parent.GetComponent<INpcMovementManager>();
            _npcCombatManager = transform.parent.GetComponent<INpcCombatManager>();
            _npcFacingDirectionManager = transform.parent.GetComponent<INpcFacingDirectionManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == Tags.PlayerTag)
            {
                Debug.Log("Entrou visão");

                _npcMovementManager.Enable();
                _npcMovementManager.SetTarget(other.gameObject);
                _npcTargetingManager.Enable();
                _npcCombatManager.Enable();
                _npcFacingDirectionManager.Enable();
                _npcFacingDirectionManager.SetCurrentFacingTarget(other.gameObject.transform);

                _npcCombatManager.SetCombatActive(true);
            }
        }
  

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.tag == Tags.PlayerTag)
            {
                _npcMovementManager.SetTarget(null);
                _npcTargetingManager.Disable();
            }
        }
    }
}
