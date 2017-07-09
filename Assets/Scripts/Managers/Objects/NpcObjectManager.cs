using Assets.Scripts.Entities.ApplicationObjects;
using Assets.Scripts.Factories;
using Assets.Scripts.Interfaces.Managers.Objects;

namespace Assets.Scripts.Managers.Objects
{
    public class NpcObjectManager : BaseObjectManager, IObjectManager
    {
        public GameAppObject _npcAppObject;

        private void Awake()
        {
            _npcAppObject = GameAppObjectFactory.Create(gameObject);
        }

        public GameAppObject GetGameAppObject()
        {
            return _npcAppObject;
        }
    }
}
