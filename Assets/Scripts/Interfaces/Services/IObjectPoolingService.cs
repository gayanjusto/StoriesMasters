using Assets.Scripts.Entities.ApplicationObjects;
using UnityEngine;

namespace Assets.Scripts.Interfaces.Services
{
    public interface IObjectPoolingService
    {
        void KillAndPoolNpc(BaseAppObject obj);
        Transform GetAttackerHighlight();
        Transform GetAttackerHighlightPool();
    }
}
