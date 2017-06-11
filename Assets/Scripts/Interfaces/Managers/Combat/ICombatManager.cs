using Assets.Scripts.Entities.ApplicationObjects;
using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.Interfaces.Managers.Combat
{
    public interface ICombatManager : IBaseMonoBehaviour
    {
        int GetAttackSequence();
        void EnableAttackerActions();
        void DisableAttackerActions();
        void DisableAttackerActions(float freezeTime);
        bool CanAttack();
        bool GetHasCastAction();
        void SetTargets(BaseAppObject[] targets);
        BaseAppObject[] GetTargets();
        void SetIsAttacking(bool isAttacking);
        bool GetIsAttacking();
        bool GetIsAttemptingToParry();
        bool GetIsBlockingWithShield();
        void SetIsBlocking(bool isBlocking);
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
