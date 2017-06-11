using System;
using Assets.Scripts.Entities.ApplicationObjects;
using Assets.Scripts.Interfaces.Services;
using Assets.Scripts.IoC;
using UnityEngine;
using Assets.Scripts.Constants;

namespace Assets.Scripts.Services
{
    public class CombatVisualInformationService : ICombatVisualInformationService
    {
        public void HighlightAttackerForPlayer(BaseAppObject attacker)
        {
            IObjectPoolingService _objPoolingService = IoCContainer.GetImplementation<IObjectPoolingService>();
            Transform attackerHighLightObj = _objPoolingService.GetAttackerHighlight();
            attackerHighLightObj.transform.position = new Vector3(attacker.GameObject.transform.position.x, 0, attacker.GameObject.transform.position.z);
            attackerHighLightObj.transform.parent = attacker.GameObject.transform;
            attackerHighLightObj.gameObject.SetActive(true);
        }

        public void RemoveHighlightAttackerInformation(BaseAppObject attacker)
        {
            foreach (Transform child in attacker.GameObject.transform)
            {
                if (child.name.Contains(PoolingObjects.AttackerHighlightObjectName))
                {
                    IObjectPoolingService _objPoolingService = IoCContainer.GetImplementation<IObjectPoolingService>();
                    child.transform.parent = _objPoolingService.GetAttackerHighlightPool();
                    child.transform.position = new Vector3(0, 0, 0);
                    child.gameObject.SetActive(false);
                }
            }

        }
    }
}
