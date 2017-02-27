using Assets.Scripts.Interfaces.Controllers;
using Assets.Scripts.Interfaces.Managers.Combat;
using Assets.Scripts.Interfaces.Managers.Movement;
using Assets.Scripts.IoC;
using UnityEngine;

namespace Assets.Scripts.Managers.Combat
{
    public class NpcTargetingManager : BaseManager, INpcTargetingManager
    {
        private INpcMovementManager _npcMovementManager;
        private ICombatController _combatController;
        private IMovementController _movementController;

        void Start()
        {
            _npcMovementManager = GetComponent<INpcMovementManager>();

            _combatController = IoCContainer.GetImplementation<ICombatController>();
            _movementController = IoCContainer.GetImplementation<IMovementController>();

            Disable();
        }

        private void Update()
        {
            if (_npcMovementManager.GetDistanceFromTarget() <= 0.3f)
            {
                //AttackTarget();
            }
        }

        public void AttackTarget()
        {
            _combatController.Attack(gameObject);
        }
    }
}
