using Assets.Scripts.Entities.ApplicationObjects;
using Assets.Scripts.Interfaces.Managers.Movement;
using Assets.Scripts.Interfaces.Services.Combat;
using UnityEngine;

namespace Assets.Scripts.Services.Combat
{
    public class ShieldDefenseService : IShieldDefenseService
    {
        public void ActivateShieldDefense(GameAppObject defendingObj)
        {
            //Prevent object from moving, but not look
            defendingObj.gameObject.GetComponent<IMovementManager>().SetLockedMovement(true);

            //Set stamina to decrease
        }

        public void DeactivateShieldDefense(GameAppObject defendingObj)
        {
            //Prevent object from moving, but not look
            defendingObj.gameObject.GetComponent<IMovementManager>().SetLockedMovement(false);

            //Set stamina to increase
        }
    }
}
