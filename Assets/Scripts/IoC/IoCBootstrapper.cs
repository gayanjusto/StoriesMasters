using Assets.Scripts.Controllers;
using Assets.Scripts.Factories;
using Assets.Scripts.Interfaces.Controllers;
using Assets.Scripts.Interfaces.Factories;
using Assets.Scripts.Interfaces.Services;
using Assets.Scripts.Services;
using UnityEngine;

namespace Assets.Scripts.IoC
{
    public class IoCBootstrapper : MonoBehaviour
    {
        void Awake()
        {
            Register();
        }

        public static void Register()
        {

            //Factories
            IoCContainer.Register<IBaseCreatureFactory, BaseCreatureFactory>();

            //Services
            IoCContainer.Register<IDirectionService, DirectionService>(IoCLifeCycleEnum.Singleton);
            IoCContainer.Register<IMovementSpeedService, MovementSpeedService>(IoCLifeCycleEnum.Singleton);
            IoCContainer.Register<ITargetService, TargetService>(IoCLifeCycleEnum.Singleton);
            IoCContainer.Register<IAttackTargetService, AttackTargetService>(IoCLifeCycleEnum.Singleton);
            IoCContainer.Register<IAttackService, AttackService>(IoCLifeCycleEnum.Singleton);
            IoCContainer.Register<ISkillPointService, SkillPointService>(IoCLifeCycleEnum.Singleton);
            IoCContainer.Register<IMovementService, MovementService>(IoCLifeCycleEnum.Singleton);

            //Controllers
            IoCContainer.Register<IMovementController, MovementController>(IoCLifeCycleEnum.Singleton);
            IoCContainer.Register<ICombatController, CombatController>(IoCLifeCycleEnum.Singleton);
        }
    }
}
