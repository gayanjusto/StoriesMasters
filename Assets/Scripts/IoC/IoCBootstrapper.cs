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
            //Combat
            //IoCContainer.Register<IAttackService, AttackService>();
            //IoCContainer.Register<IProjectileTypeAttack, ProjectileTypeAttack>();
            //IoCContainer.Register<IStockTypeAttack, StockTypeAttack>();
            //IoCContainer.Register<ISwingTypeAttack, SwingTypeAttack>();


            //New

            //Factories
            IoCContainer.Register<IBaseCreatureFactory, BaseCreatureFactory>();

            //Services
            IoCContainer.Register<IMovementSpeedService, MovementSpeedService>(IoCLifeCycleEnum.Singleton);
            IoCContainer.Register<IDirectionService, DirectionService>(IoCLifeCycleEnum.Singleton);
            IoCContainer.Register<IAttackTargetService, AttackTargetService>(IoCLifeCycleEnum.Singleton);
            IoCContainer.Register<IAttackService, AttackService>(IoCLifeCycleEnum.Singleton);
            IoCContainer.Register<ISkillPointService, SkillPointService>(IoCLifeCycleEnum.Singleton);




            //Controllers
            IoCContainer.Register<IMovementController, MovementController>(IoCLifeCycleEnum.Singleton);
            IoCContainer.Register<ICombatController, CombatController>(IoCLifeCycleEnum.Singleton);
        }
    }
}
