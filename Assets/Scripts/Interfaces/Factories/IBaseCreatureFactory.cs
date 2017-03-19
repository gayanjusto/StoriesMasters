using Assets.Scripts.Entities.IntelligentBodies;

namespace Assets.Scripts.Interfaces.Factories
{
    public interface IBaseCreatureFactory
    {
        BaseCreature GetNewCreature();
    }
}
