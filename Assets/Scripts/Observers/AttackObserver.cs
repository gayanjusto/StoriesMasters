using Assets.Scripts.Interfaces.Managers.Combat;
using Assets.Scripts.Interfaces.Managers.Objects;
using Assets.Scripts.Interfaces.Observers;
using Assets.Scripts.Interfaces.Services;
using Assets.Scripts.IoC;
using Assets.Scripts.Managers;
using UnityEngine;
using System.Linq;
using Assets.Scripts.Constants;
using Assets.Scripts.Entities.ApplicationObjects;

namespace Assets.Scripts.Observers
{
    public class AttackObserver : BaseMonoBehaviour, IAttackObserver
    {
        private ICombatManager _combatManager;
        private IObjectManager _objectManager;

        private IAttackService _attackService;
        public bool _hasHighlightedAttack;

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
                BaseAppObject attackerBaseObj = _objectManager.GetBaseAppObject();

                //If this object is attacking a player, he should be highlighted so the player can have a hint for parry 
                if (
                    gameObject.tag != Tags.PlayerTag
                    && !_hasHighlightedAttack && _attackService.AttackIsPastHalfWay(attackerBaseObj)
                    && (_combatManager.GetTargets() != null && _combatManager.GetTargets().Any(x => x.GameObject.tag == Tags.PlayerTag))
                    )
                {
                    ICombatVisualInformationService combatVisualInfoService = IoCContainer.GetImplementation<ICombatVisualInformationService>();
                    combatVisualInfoService.HighlightAttackerForPlayer(attackerBaseObj);
                    _hasHighlightedAttack = true;
                }

                _combatManager.SetIsAttacking(true);

                //Attacker can only attack after the attack delay is depleted
                if (_combatManager.GetCurrentTimeForAttackDelay() <= 0)
                {
                    //Tell CombatManager that an action has been cast.
                    _combatManager.SetHasCastAction(true);

                    //Has already attacked, so it should reset its state
                    _combatManager.SetAttackReady(false);

                    _attackService.Attack(attackerBaseObj);

                    Disable();
                }
            }
            else
            {
                //If can't attack, disable this Observer
                Disable();
            }
        }

        public override void Disable()
        {
            base.Disable();
            _hasHighlightedAttack = false;
        }
    }
}
