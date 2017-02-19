using UnityEngine;

namespace Assets.Scripts.Interfaces.Controllers
{
    public interface ICombatController
    {
        bool Attack(GameObject attackingObj);
        void DisableCombatInput(GameObject attackingObj);
        void EnableCombatInput(GameObject attackingObj);
        float GetAttackDelayBasedOnEquippedWeapon(GameObject attackingObj, bool isLastAttackSequence);
    }
}
