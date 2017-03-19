using Assets.Scripts.Entities.ApplicationObjects;
using Assets.Scripts.Factories;
using Assets.Scripts.Interfaces.Managers.Objects;

namespace Assets.Scripts.Managers.Objects
{
    public class NpcObjectManager : BaseObjectManager, IObjectManager
    {
        public NpcAppObject _npcAppObject;

        private void Awake()
        {
            base.SetCreature();

            _npcAppObject = NpcAppObjectFactory.Create(gameObject, _creature);
        }

        public BaseAppObject GetBaseAppObject()
        {
            return _npcAppObject;
        }
    }
}
