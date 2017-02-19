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
        public override void DisableAttackerActions()
        {
            base.DisableAttackerActions();
            GetComponent<INpcTargetingManager>().Disable();
        }

        public override void DisableAttackerActions(float freezeTime)
        {
            base.DisableAttackerActions(freezeTime);
            GetComponent<INpcTargetingManager>().Disable();
        }

        public override void EnableAttackerActions()
        {
            base.EnableAttackerActions();
            GetComponent<INpcTargetingManager>().Enable();
        }
    }
}
