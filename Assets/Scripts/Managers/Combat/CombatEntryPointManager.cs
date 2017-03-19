using Assets.Scripts.Entities.ApplicationObjects;
using Assets.Scripts.Interfaces.Controllers;
using Assets.Scripts.Interfaces.Managers.Combat;
using Assets.Scripts.Interfaces.Managers.Objects;
using System;
using UnityEngine;

namespace Assets.Scripts.Managers.Combat
{
    public class CombatEntryPointManager : BaseManager
    {
        protected ICombatController _combatController;
        protected IObjectManager _objectManager;
        protected ICombatManager _combatManager;

        protected bool _hasDelegatedTriggered;

        protected delegate bool DelegatedAction(BaseAppObject thisObj);
        protected DelegatedAction delegatedAction;

        protected void InitiateAttack()
        {
            Debug.Log(gameObject.name + " Started waiting: " + DateTime.Now);
            float attackDelayTime = _combatController.GetTimeForAttackDelay(_objectManager.GetBaseAppObject());

            _combatController.StartAttack(_objectManager.GetBaseAppObject());

            _combatManager.DisableAttackerActions(attackDelayTime);

            delegatedAction = new DelegatedAction(_combatController.Attack);

            _hasDelegatedTriggered = true;
        }
    }
}
