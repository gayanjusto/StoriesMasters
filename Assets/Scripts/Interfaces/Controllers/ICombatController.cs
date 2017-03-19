using Assets.Scripts.Entities.ApplicationObjects;
using UnityEngine;

namespace Assets.Scripts.Interfaces.Controllers
{
    public interface ICombatController
    {
        bool Attack(BaseAppObject attackingObj);
        void DisableCombatInput(GameObject attackingObj);
        void EnableCombatInput(GameObject attackingObj);
        float GetAttackDelayBasedOnEquippedWeapon(BaseAppObject attackingObj, bool isLastAttackSequence);
        float GetTimeForAttackDelay(BaseAppObject attackingObj);
        BaseAppObject[] StartAttack(BaseAppObject attackingObj);
        bool ParryAttack(BaseAppObject parryingObj);
    }
}
