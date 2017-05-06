using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces.Managers.Movement;
using Assets.Scripts.Interfaces.Controllers;
using Assets.Scripts.IoC;

namespace Assets.Scripts.Managers.Movement
{
    public class BaseMovementManager : BaseMonoBehaviour/*, IMovementManager*/
    {
        public DirectionEnum _verticalDirection;
        public DirectionEnum _horizontalDirection;

        public DirectionEnum _verticalFacingDirection;
        public DirectionEnum _horizontalFacingDirection;

        protected IMovementController _movementController;

        void Start()
        {
            Enable();
            _movementController = IoCContainer.GetImplementation<IMovementController>();
        }

        public bool IsMoving()
        {
            return _horizontalDirection != DirectionEnum.None || _verticalDirection != DirectionEnum.None;
        }

        public DirectionEnum GetHorizontalDirection()
        {
            return _horizontalDirection;
        }

        public DirectionEnum GetVerticalDirection()
        {
            return _verticalDirection;
        }

        public DirectionEnum GetHorizontalFacingDirection()
        {
            return _horizontalFacingDirection;
        }

        public DirectionEnum GetVerticalFacingDirection()
        {
            return _verticalFacingDirection;
        }
    }
}
