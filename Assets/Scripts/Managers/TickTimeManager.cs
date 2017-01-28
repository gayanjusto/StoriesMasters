using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class TickTimeManager : BaseManager
    {
        protected float _maximumTickTime;
        protected float _currentTickTime;
        protected float _freezeTickTime;
        protected bool _hasCastAction;

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
