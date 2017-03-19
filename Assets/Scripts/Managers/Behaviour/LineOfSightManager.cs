using Assets.Scripts.Constants;
using Assets.Scripts.Interfaces.Managers.Behaviour;
using Assets.Scripts.Interfaces.Managers.Combat;
using Assets.Scripts.Interfaces.Managers.Movement;
using UnityEngine;

namespace Assets.Scripts.Managers.Behaviour
{
    public class LineOfSightManager : BaseManager, ILineOfSightManager
    {
        INpcTargetingManager _npcTargetingManager;
        INpcMovementManager _npcMovementManager;

        private void Start()
        {
            _npcTargetingManager = transform.parent.GetComponent<INpcTargetingManager>();
            _npcMovementManager = transform.parent.GetComponent<INpcMovementManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == Tags.PlayerTag)
            {
                Debug.Log("Entrou visão");

                _npcMovementManager.SetTarget(other.gameObject);
                _npcTargetingManager.Enable();
            }
        }
        //private void OnCollisionEnter(Collision collision)
        //{
        //    if(collision.gameObject.tag == Tags.PlayerTag)
        //    {
        //        _npcMovementManager.SetTarget(collision.gameObject);
        //        _npcTargetingManager.Enable();
        //    }
        //}

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
