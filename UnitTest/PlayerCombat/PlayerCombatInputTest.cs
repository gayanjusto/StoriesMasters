using Assets.Scripts.Interfaces.Managers.Inputs;
using Assets.Scripts.Managers.Inputs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UnitTest.PlayerCombat
{
    [TestClass]
    public class PlayerCombatInputTest
    {
        IPlayerCombatInputManager MockPlayerCombatInputManager()
        {
            GameObject playerObject = new GameObject();
            playerObject.AddComponent(typeof(IPlayerCombatInputManager));

            return playerObject.GetComponent<IPlayerCombatInputManager>();
        }

        [TestMethod]
        public void PlayerCanPressKeyForAttackInput()
        {
            var playerCombatInput = MockPlayerCombatInputManager();

            var canPressKey = playerCombatInput.CanPressAttackKey();
            Assert.IsTrue(canPressKey);
        }

        [TestMethod]
        public void PlayerCannotPressKeyAfterAttackInput()
        {
            var playerCombatInput = MockPlayerCombatInputManager();
            playerCombatInput.InitiateAttack();

            Assert.IsFalse(playerCombatInput.CanPressAttackKey());
        }

      

    }
}
