using System;
using Assets.Scripts.Entities.ApplicationObjects;
using Assets.Scripts.Interfaces.Services.Combat;
using System.Linq;
using Assets.Scripts.Interfaces.Services;
using Assets.Scripts.Factories.Services.Combat;
using Assets.Scripts.Interfaces.Managers.Objects;
using Assets.Scripts.Interfaces.Managers;

namespace Assets.Scripts.Services.Combat
{
    public class AttackTargetService : IAttackTargetService
    {
        private readonly ITargetService _targetService;

        public AttackTargetService()
        {
            _targetService = TargetServiceFactory.GetInstance();
        }
        #region PRIVATE
        GameAppObject[] GetTargetFacingDirection(GameAppObject attackingObj, ref GameAppObject[] returningTargets)
        {

            var target = _targetService.GetTargetForFacingDirection(attackingObj.gameObject, attackingObj.gameObject.GetComponent<IDirectionManager>().GetFacingDirection());

            if (target != null)
            {
                returningTargets = new GameAppObject[1];
                returningTargets[0] = target.GetComponent<IObjectManager>().GetGameAppObject();
            }

            return returningTargets;
        }
        #endregion

        public GameAppObject[] GetFacingTargets(GameAppObject actionObj)
        {
            GameAppObject[] targets = null;
            GetTargetFacingDirection(actionObj, ref targets);

            if (targets == null)
            {
                return null;
            }
            return targets;
        }


        public GameAppObject[] GetTargetsForAttack(GameAppObject attackingObj)
        {
            throw new NotImplementedException();
        }
    }
}
