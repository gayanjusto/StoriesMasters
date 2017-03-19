using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class TickTimeManager : BaseManager
    {
        protected float _maximumTickTime;
        public float _currentTickTime;
        protected float _freezeTickTime;
        public bool _hasCastAction;

        protected void TickTime()
        {
            _currentTickTime -= Time.deltaTime;
        }

        protected void ResetTickTime()
        {
            _currentTickTime = _maximumTickTime;
        }

        protected bool IsWaitingFreezeTime()
        {
            return _currentTickTime > 0;
        }
    }
}
