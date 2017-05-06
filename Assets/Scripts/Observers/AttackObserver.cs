using Assets.Scripts.Interfaces.Managers.Combat;
using Assets.Scripts.Interfaces.Managers.Objects;
using Assets.Scripts.Interfaces.Observers;
using Assets.Scripts.Interfaces.Services;
using Assets.Scripts.IoC;
using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Observers
{
    public class AttackObserver : BaseMonoBehaviour, IAttackObserver
    {
        private ICombatManager _combatManager;
        private IObjectManager _objectManager;

        private IAttackService _attackService;

        private void Awake()
        {
            _combatManager = GetComponent<ICombatManager>();
            _objectManager = GetComponent<IObjectManager>();
        }

        private void Start()
        {
            _attackService = IoCContainer.GetImplementation<IAttackService>();
        }
        private void Update()
        {
            ObserveAttackStatus();
        }


        void ObserveAttackStatus()
        {
            //read CombatManager for Attack Status
            if (_combatManager.IsReadyToAttack())
            {
                _attackService.Attack(_objectManager.GetBaseAppObject());

                //Tell CombatManager that an action has been cast.
                _combatManager.SetHasCastAction(true);
            }
            else
            {
                //If can't attack, disable this Observer
                Disable();
            }
        }
    }
}
