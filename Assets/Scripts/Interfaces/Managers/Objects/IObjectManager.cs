using Assets.Scripts.Entities.ApplicationObjects;
using Assets.Scripts.Entities.IntelligentBodies;

namespace Assets.Scripts.Interfaces.Managers.Objects
{
    public interface IObjectManager
    {
        BaseCreature GetBaseCreature();
        BaseAppObject GetBaseAppObject();

    }
}
