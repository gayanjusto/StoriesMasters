using Assets.Scripts.Entities.ApplicationObjects;
using Assets.Scripts.Enums;

namespace Assets.Scripts.Interfaces.Managers.Combat
{
    public interface ICombatManager : IBaseMonoBehaviour, IDefenseStatusManager, IAttackSequenceManager, IAttackStatusManager
    {
        void EnableAttackerActions();
        void DisableAttackerActions();
        void SetRecoverTimeFromAttack(float freezeTime);
        bool GetHasCastAction();
        void SetTargets(BaseAppObject[] targets);
        BaseAppObject[] GetTargets();

        void SetParryingTarget(BaseAppObject target);

        BaseAppObject GetParryingTarget();
        DirectionEnum[] GetBlockingDirections();

        void SetHasCastAction(bool hasCastAnAction);

        bool IsAbleToInitiateAttack();
        void SetAttackReady(bool isAttackReady);
        bool IsReadyToAttack();
        float GetCurrentTimeForAttackDelay();
        float GetTotalFreezeTime();
        void SetIsAttemptingToParryAttack(bool isAttemptingToParry);
    }
}
