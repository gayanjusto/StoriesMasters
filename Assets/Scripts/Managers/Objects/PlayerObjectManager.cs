using Assets.Scripts.Entities.ApplicationObjects;
using Assets.Scripts.Factories;
using Assets.Scripts.Interfaces.Managers.Objects;

namespace Assets.Scripts.Managers.Objects
{
    public class PlayerObjectManager : BaseObjectManager, IObjectManager
    {
        public GameAppObject _playerGameAppObject;

        public void Start()
        {
            _playerGameAppObject = GameAppObjectFactory.Create(gameObject);
        }

        public GameAppObject GetGameAppObject()
        {
            return _playerGameAppObject;
        }
    }
}
