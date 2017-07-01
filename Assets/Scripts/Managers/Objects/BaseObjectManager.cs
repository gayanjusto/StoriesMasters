using Assets.Scripts.Entities.IntelligentBodies;
using Assets.Scripts.Interfaces.Factories;
using Assets.Scripts.IoC;

namespace Assets.Scripts.Managers.Objects
{
    public class BaseObjectManager : BaseMonoBehaviour
    {
        public BaseCreature _creature;
        private IBaseCreatureFactory _baseCreatureFactory;


        protected void SetCreature()
        {
            _baseCreatureFactory = IoCContainer.GetImplementation<IBaseCreatureFactory>();
            _creature = _baseCreatureFactory.GetNewCreature();
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
