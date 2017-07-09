using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces.Managers;

namespace Assets.Scripts.Managers.Movement
{
   
    public class DirectionManager : BaseMonoBehaviour, IDirectionManager
    {
        public DirectionEnum _facingDirection;

        public void SetFacingDirection(DirectionEnum facingDirection)
        {
            _facingDirection = facingDirection;
        }

        public DirectionEnum GetFacingDirection()
        {
            return _facingDirection;
        }
    }
}
