using Assets.Scripts.Interfaces.Managers.Combat;

namespace Assets.Scripts.Managers.Combat
{
    public class NpcCombatManager : BaseCombatManager, INpcCombatManager
    {

        #region PRIVATE METHODS
        private void Update()
        {
            base.WaitForActionDelay();
        }
        #endregion
    }
}
