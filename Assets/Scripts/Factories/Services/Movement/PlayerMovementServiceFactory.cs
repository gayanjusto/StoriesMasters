using Assets.Scripts.Interfaces.Services;
using Assets.Scripts.Services;

namespace Assets.Scripts.Factories.Services.Movement
{
    public class PlayerMovementServiceFactory : BaseSingletonFactory<PlayerMovementService, IPlayerMovementService>
    {
    }
}
