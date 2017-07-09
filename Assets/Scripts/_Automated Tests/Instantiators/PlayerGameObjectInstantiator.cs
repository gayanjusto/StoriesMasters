using Assets.Scripts.Controllers.Player;
using Assets.Scripts.Managers;
using Assets.Scripts.Managers.Movement;
using Assets.Scripts.Managers.Objects;
using UnityEngine;

public class PlayerGameObjectInstantiator
{ 
	public static GameObject GetPlayerObject()
    {
        var playerGameObject = new GameObject();
        playerGameObject.AddComponent<PlayerObjectManager>();
        var playerObjManager = playerGameObject.GetComponent<PlayerObjectManager>();

        //Controllers 
        playerGameObject.AddComponent<PlayerMovementController>();
        var playerMovController = playerGameObject.GetComponent<PlayerMovementController>();

        //Managers
        playerGameObject.AddComponent<MovementManager>();
        playerGameObject.AddComponent<DirectionManager>();

        playerObjManager.Start();
        playerMovController.Start();


        return playerGameObject;
    }
}
