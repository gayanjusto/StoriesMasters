using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces.Controllers;
using Assets.Scripts.Interfaces.Services;
using UnityEngine;
using Assets.Scripts.Interfaces.Managers.Attributes;
using Assets.Scripts.Constants;
using Assets.Scripts.Managers.Inputs;
using Assets.Scripts.Interfaces.Managers.Movement;
using Assets.Scripts.Entities.ApplicationObjects;
using System;
using Assets.Scripts.IoC;

namespace Assets.Scripts.Controllers
{
    public class MovementController : IMovementController
    {
        private readonly IMovementSpeedService _movementSpeedService;
        private readonly IDirectionService _directionService;

        public MovementController()
        {
            _directionService = IoCContainer.GetImplementation<IDirectionService>();
            _movementSpeedService = IoCContainer.GetImplementation<IMovementSpeedService>();
        }

        #region PRIVATE METHODS
        #endregion


        public void SetMovement(DirectionEnum horizontalValue, DirectionEnum verticalValue, bool isRunning, BaseAppObject movingObj)
        {
            if (isRunning)
            {
                movingObj.DecreaseSteamina();
            }

            if (horizontalValue != DirectionEnum.None && verticalValue != DirectionEnum.None)
            {
                _movementSpeedService.SetDiagonalSpeed(horizontalValue, verticalValue, movingObj, isRunning);
                return;
            }

            if (horizontalValue != DirectionEnum.None)
            {
                _movementSpeedService.SetHorizontalSpeed(horizontalValue, movingObj, isRunning);
                return;
            }

            if (verticalValue != DirectionEnum.None)
            {
                _movementSpeedService.SetVerticalSpeed(verticalValue, movingObj, isRunning);
                return;
            }

        }

        public void StopMoving(BaseAppObject movingObj)
        {
            movingObj.GetMonoBehaviourObject<IStaminaManager>().SetDecreasingStamina(false);
        }

        public void DisableMovement(GameObject movingObj)
        {
            movingObj.GetComponent<IMovementManager>().Disable();
        }

        public void EnableMovement(GameObject movingObj)
        {
            Debug.Log("Habilitou movement manager");
            movingObj.GetComponent<IMovementManager>().Enable();
        }

        public DirectionEnum[] GetNeighboringDirections(BaseAppObject movingObj)
        {
            throw new NotImplementedException();
        }
    }
}
