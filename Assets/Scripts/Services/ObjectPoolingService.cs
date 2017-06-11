using Assets.Scripts.Interfaces.Services;
using System;
using Assets.Scripts.Entities.ApplicationObjects;
using UnityEngine;
using Assets.Scripts.Managers.Movement;

namespace Assets.Scripts.Services
{
    public class ObjectPoolingService : IObjectPoolingService
    {
        private const string npc_object_pool = "NpcObjectPool";
        private const string attacker_highlight_pool = "AttackerHighlightsPool";


        #region PRIVATE METHODS
        GameObject GetPoolNpcObject()
        {
            return GameObject.Find(npc_object_pool);
        }

        GameObject GetPoolAttackerHighlightObject()
        {
            return GameObject.Find(attacker_highlight_pool);
        }

        #endregion

        public void KillAndPoolNpc(BaseAppObject obj)
        {
       
            obj.ComponentsManager.DisableAllComponents();
            obj.GameObject.SetActive(false);
            //Disable all component from object

            //Set object as a child of pool, making it available in the pool
            obj.Transform.parent = GetPoolNpcObject().transform;
            obj.Transform.position = new Vector3(0, -50, 0);
        }

        public Transform GetAttackerHighlight()
        {
            Debug.Log("Buscou highlight para attacker ");
            GameObject attackerHighlightPool = GetPoolAttackerHighlightObject();
            Transform attackerHighlight = null;
            if (attackerHighlightPool.transform.childCount > 0)
            {
                attackerHighlight = attackerHighlightPool.transform.GetChild(0);
            }

            attackerHighlight.gameObject.SetActive(true);
            attackerHighlight.transform.parent = null;
            return attackerHighlight;
        }

        public Transform GetAttackerHighlightPool()
        {
            return GetPoolAttackerHighlightObject().transform;
        }
    }
}
