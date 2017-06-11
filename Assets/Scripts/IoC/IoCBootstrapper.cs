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

        //Order should be from implementations with less dependencies to ones with most dependencies
        public static void Register()
        {

            //Factories
            IoCContainer.Register<IBaseCreatureFactory, BaseCreatureFactory>();

            //Services
            IoCContainer.Register<IDirectionService, DirectionService>(IoCLifeCycleEnum.Singleton);
            IoCContainer.Register<IMovementSpeedService, MovementSpeedService>(IoCLifeCycleEnum.Singleton);
            IoCContainer.Register<ITargetService, TargetService>(IoCLifeCycleEnum.Singleton);
            IoCContainer.Register<IAttackTargetService, AttackTargetService>(IoCLifeCycleEnum.Singleton);
            IoCContainer.Register<IAttackTypeService, AttackTypeService>(IoCLifeCycleEnum.Singleton);
            
            IoCContainer.Register<ICombatVisualInformationService, CombatVisualInformationService>(IoCLifeCycleEnum.Singleton);
            IoCContainer.Register<IAttackService, AttackService>(IoCLifeCycleEnum.Singleton);
            IoCContainer.Register<ISkillPointService, SkillPointService>(IoCLifeCycleEnum.Singleton);
            IoCContainer.Register<IMovementService, MovementService>(IoCLifeCycleEnum.Singleton);
            IoCContainer.Register<IObjectPoolingService, ObjectPoolingService>(IoCLifeCycleEnum.Singleton);

            
            //Controllers
            IoCContainer.Register<IMovementController, MovementController>(IoCLifeCycleEnum.Singleton);
        }
    }
}
