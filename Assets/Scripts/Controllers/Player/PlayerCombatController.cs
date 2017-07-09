using Assets.Scripts.Factories.Services.Combat;
using Assets.Scripts.Interfaces.Managers.Combat;
using Assets.Scripts.Interfaces.Managers.Objects;
using Assets.Scripts.Interfaces.Services.Combat;
using Assets.Scripts.MonoBehaviours;
using UnityEngine;

namespace Assets.Scripts.Controllers.Player
{
    public class PlayerCombatController : AppObjectMonoBehaviour
    {
        public bool _isHoldingDefenseKey;
        public float _timeHoldingDefenseKey;
        private const float _timeToActivateShieldDefense = 1f;

        private ICombatManager _combatManager;

        private IShieldDefenseService _shieldDefenseService;

        public override void Start()
        {
            base.Start();

            _combatManager = GetComponent<ICombatManager>();
            _shieldDefenseService = ShieldDefenseServiceFactory.GetInstance();
        }

        public void Update()
        {
            CheckAttackInput();
            CheckDefenseInput();
        }

        void CheckAttackInput()
        {
            //Can only attack if player is not defending

            if (_combatManager.CanCastAction())
            {
                if (Input.GetKey(KeyCode.E))
                {
                    _combatManager.InitiateAttack();
                }
            }
          
        }

        void CheckDefenseInput()
        {
            //Can only defend if player is not attacking
            if (!_combatManager.GetIsAttacking())
            {
                if (Input.GetKey(KeyCode.Q))
                {
                    _timeHoldingDefenseKey += Time.deltaTime;

                    if (_timeHoldingDefenseKey >= _timeToActivateShieldDefense)
                    {
                        _isHoldingDefenseKey = true;
                        _combatManager.SetIsDefending(true);
                        _shieldDefenseService.ActivateShieldDefense(base.gameAppObject);
                    }
                }
                else
                {
                    if (_isHoldingDefenseKey)
                    {
                        _shieldDefenseService.DeactivateShieldDefense(base.gameAppObject);
                    }

                    _timeHoldingDefenseKey = 0;
                    _isHoldingDefenseKey = false;
                    _combatManager.SetIsDefending(false);
                }
            }


        }

    }
}
