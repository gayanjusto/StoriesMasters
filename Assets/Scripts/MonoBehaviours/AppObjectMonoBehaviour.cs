using Assets.Scripts.Entities.ApplicationObjects;
using Assets.Scripts.Interfaces.Managers.Objects;
using Assets.Scripts.Managers;

namespace Assets.Scripts.MonoBehaviours
{
    public class AppObjectMonoBehaviour : BaseMonoBehaviour
    {
        protected GameAppObject gameAppObject;

        public virtual void Start()
        {
            var objectManager = gameObject.GetComponent<IObjectManager>();
            gameAppObject = objectManager.GetGameAppObject();
        }
    }
}
