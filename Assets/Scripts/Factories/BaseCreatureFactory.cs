using Assets.Scripts.Entities.IntelligentBodies;
using Assets.Scripts.Interfaces.Factories;

namespace Assets.Scripts.Factories
{
    public class BaseCreatureFactory : IBaseCreatureFactory
    {
        public BaseCreature GetNewCreature()
        {
            return new BaseCreature();
        }
    }
}
