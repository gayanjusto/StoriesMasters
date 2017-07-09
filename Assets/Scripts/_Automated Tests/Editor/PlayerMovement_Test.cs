using UnityEngine;
using NUnit.Framework;
using Assets.Scripts.Interfaces.Managers.Movement;
using Assets.Scripts.Controllers.Player;

public class PlayerMovement_Test {

	[Test]
    public void Player_Can_Move_Left()
    {
        //Arrange
        var playerGameObject = PlayerGameObjectInstantiator.GetPlayerObject();
        var movementController = playerGameObject.GetComponent<PlayerMovementController>();

        var playerOriginalPositon_X = playerGameObject.transform.position.x;

        //Act
        for (int i = 0; i < 1000; i++)
        {
            movementController.SetHorizontalMovementValue(KeyCode.LeftArrow);
            movementController.MovePlayer();
        }

        //Assert
        Assert.Greater(playerOriginalPositon_X, playerGameObject.transform.position.x);
    }

    [Test]
    public void Player_Can_Move_Right()
    {
        //Arrange
        var playerGameObject = PlayerGameObjectInstantiator.GetPlayerObject();
        var movementController = playerGameObject.GetComponent<PlayerMovementController>();

        var playerOriginalPositon_X = playerGameObject.transform.position.x;

        //Act
        for (int i = 0; i < 1000; i++)
        {
            movementController.SetHorizontalMovementValue(KeyCode.RightArrow);
            movementController.MovePlayer();
        }

        //Assert
        Assert.Greater(playerGameObject.transform.position.x, playerOriginalPositon_X);
    }

    [Test]
    public void MovementController_Tells_Checks_MovementManager_MovingStatus()
    {
        //Arrange
        var playerGameObject = PlayerGameObjectInstantiator.GetPlayerObject();
        var movementController = playerGameObject.GetComponent<PlayerMovementController>();
        var movementManager = playerGameObject.GetComponent<IMovementManager>();

        var playerOriginalPositon_X = playerGameObject.transform.position.x;

        bool movingCondition = false;
        //Act
        for (int i = 0; i < 1000; i++)
        {
            movementController.SetHorizontalMovementValue(KeyCode.RightArrow);
            movementController.MovePlayer();

            movingCondition = movementManager.GetIsMoving();
        }

        //Assert
        Assert.IsTrue(movingCondition);
    }

    [Test]
    public void MovementController_Tells_Checks_MovementManager_RunningStatus()
    {
        //Arrange
        var playerGameObject = PlayerGameObjectInstantiator.GetPlayerObject();
        var movementController = playerGameObject.GetComponent<PlayerMovementController>();
        var movementManager = playerGameObject.GetComponent<IMovementManager>();

        var playerOriginalPositon_X = playerGameObject.transform.position.x;

        bool runningCondition = false;
        //Act
        for (int i = 0; i < 1000; i++)
        {
            movementController.SetRunningValue(KeyCode.LeftShift);
            runningCondition = movementManager.GetIsRunning();
        }

        //Assert
        Assert.IsTrue(runningCondition);
    }
}
