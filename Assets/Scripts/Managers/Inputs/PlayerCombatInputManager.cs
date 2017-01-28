using Assets.Scripts.Interfaces.Controllers;
using Assets.Scripts.Interfaces.Managers.Inputs;
using Assets.Scripts.IoC;
using UnityEngine;

namespace Assets.Scripts.Managers.Inputs
{
    public class PlayerCombatInputManager : BaseManager, IPlayerCombatInputManager
    {
        private ICombatController _combatController;

        void Start()
        {
            _combatController = IoCContainer.GetImplementation<ICombatController>();
        }
        void Update()
        {
            CheckDefenseKeyPress();
            CheckAttackKeyPress();
        }

        void CheckDefenseKeyPress()
        {
            if (Input.GetKey(KeyCode.Q))
            {
                Debug.Log("Defendeu");
            }
        }

        void CheckAttackKeyPress()
        {
            if (Input.GetKey(KeyCode.E))
            {
                _combatController.Attack(gameObject);
            }
        }

    }
}
