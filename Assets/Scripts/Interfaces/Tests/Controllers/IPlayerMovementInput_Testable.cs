using UnityEngine;

namespace Assets.Scripts.Interfaces.Tests.Controllers
{
    public interface IPlayerMovementInput_Testable
    {
        int SetHorizontalMovementValue(KeyCode keyPress);
        int SetVerticalMovementValue(KeyCode keyPress);
        void SetRunningValue(KeyCode keyPress);
    }
}
