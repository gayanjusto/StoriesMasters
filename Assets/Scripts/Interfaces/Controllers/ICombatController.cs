using UnityEngine;

namespace Assets.Scripts.Interfaces.Controllers
{
    public interface ICombatController
    {
        bool Attack(GameObject attackingObj);
        void DisableCombatIinput(GameObject attackingObj);
        void EnableCombatIinput(GameObject attackingObj);
        float GetAttackDelayBasedOnEquippedWeapon(GameObject attackingObj, bool isLastAttackSequence);
    }
}
