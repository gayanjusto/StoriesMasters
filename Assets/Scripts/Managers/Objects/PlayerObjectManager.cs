using Assets.Scripts.Entities.ApplicationObjects;
using Assets.Scripts.Entities.IntelligentBodies;
using Assets.Scripts.Factories;
using Assets.Scripts.Interfaces.Factories;
using Assets.Scripts.Interfaces.Managers.Objects;
using Assets.Scripts.IoC;

namespace Assets.Scripts.Managers.Objects
{
    public class PlayerObjectManager : BaseObjectManager, IObjectManager
    {
        public PlayerAppObject _playerAppObject;

        private void Start()
        {
            base.SetCreature();

            _playerAppObject = PlayerAppObjectFactory.Create(gameObject, _creature);
        }

        public BaseAppObject GetBaseAppObject()
        {
            return _playerAppObject;
        }
    }
}
