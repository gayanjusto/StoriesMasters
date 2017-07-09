//using Assets.Scripts.Interfaces.Managers.Combat;
//using Assets.Scripts.Interfaces.Managers.Movement;
//using Assets.Scripts.Interfaces.Observers;
//using Assets.Scripts.Managers;

//namespace Assets.Scripts.Observers
//{
//    public class MovementObserver : BaseMonoBehaviour, IMovementObserver
//    {
//        private ICombatManager _combatManager;
//        private IMovementManager _movementManager;

//        private void Awake()
//        {
//            _combatManager = GetComponent<ICombatManager>();
//            _movementManager = GetComponent<IMovementManager>();
//        }

//        private void Update()
//        {
//            ObserveToDisableMovement();
//        }

//        void ObserveToDisableMovement()
//        {
//            if (HasAnyStatusPreventingMovement())
//            {
//                _movementManager.Disable();
//            }
//            else if (IsShieldBlocking())
//            {
//                _movementManager.LockMovement(true);
//            }
//            else
//            {
//                _movementManager.LockMovement(false);
//                _movementManager.Enable();

//                //Disable this observer
//                Disable();
//            }
//        }

//        //Return all possible status that will prevent movement
//        bool HasAnyStatusPreventingMovement()
//        {
//            return _combatManager.GetHasCastAction();
//        }

//        bool IsShieldBlocking()
//        {
//            return _combatManager.IsBlockingWithShield();
//        }
//    }
//}
