using Assets.Scripts.Entities;
using Assets.Scripts.Entities.IntelligentBodies;
using Assets.Scripts.Interfaces.Factories;
using Assets.Scripts.Interfaces.Managers.Objects;
using Assets.Scripts.IoC;

namespace Assets.Scripts.Managers.Objects
{
    public class ObjectManager : BaseManager, IObjectManager
    {
        public BaseCreature _creature;
        private IBaseCreatureFactory _baseCreatureFactory;

        void Start()
        {
            _baseCreatureFactory = IoCContainer.GetImplementation<IBaseCreatureFactory>();
            _creature = _baseCreatureFactory.GetPlainMoveableEntity();
        }
        public BaseCreature GetBaseCreature()
        {
            return new BaseCreature()
            {
                Strength = 10,
                Dexterity = 10
            };
            return _creature;
        }
    }
}
