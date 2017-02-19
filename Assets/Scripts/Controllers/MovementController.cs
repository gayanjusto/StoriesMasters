using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces.Controllers;
using Assets.Scripts.Interfaces.Services;
using UnityEngine;
using Assets.Scripts.Interfaces.Managers.Attributes;
using Assets.Scripts.Constants;
using Assets.Scripts.Managers.Inputs;
using Assets.Scripts.Interfaces.Managers.Movement;

namespace Assets.Scripts.Controllers
{
    public class MovementController : IMovementController
    {
        private readonly IMovementSpeedService _movementSpeedService;


        public MovementController()
        {
            _movementSpeedService = IoC.IoCContainer.GetImplementation<IMovementSpeedService>();
        }

        void DecreaseStamina(GameObject movingObj)
        {
            IStaminaManager objStaminaManager = movingObj.GetComponent<IStaminaManager>();
            if (!objStaminaManager.IsEnabled())
            {
                objStaminaManager.Enable();
            }

            objStaminaManager.SetDecreasingStamina(true);
        }

        public void SetMovement(DirectionEnum horizontalValue, DirectionEnum verticalValue, bool isRunning, GameObject movingObj)
        {
            if (isRunning)
            {
                DecreaseStamina(movingObj);
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

        public void StopMoving(GameObject movingObj)
        {
            IStaminaManager objStaminaManager = movingObj.GetComponent<IStaminaManager>();

            if (objStaminaManager.IsEnabled())
            {
                objStaminaManager.SetDecreasingStamina(false);
            }
        }

        public void DisableMovement(GameObject movingObj)
        {
            movingObj.GetComponent<IMovementManager>().Disable();
        }

        public void EnableMovement(GameObject movingObj)
        {
            if (movingObj.tag == Tags.PlayerTag)
            {
                movingObj.GetComponent<IMovementManager>().Enable();
            }
        }
    }
}
