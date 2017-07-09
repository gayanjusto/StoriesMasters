using Assets.Scripts.Interfaces.Services.Combat;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Combat;

namespace Assets.Scripts.Factories.Services.Combat
{
    public class AttackServiceFactory : BaseSingletonFactory<AttackService, IAttackService>
    {
    }
}
