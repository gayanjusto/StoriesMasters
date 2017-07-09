using Assets.Scripts.Interfaces.Managers.Attributes;
using Assets.Scripts.Interfaces.Managers.Objects;
using Assets.Scripts.Entities.IntelligentBodies;

namespace Assets.Scripts.Managers.Attributes
{
    public class StaminaManager : TickTimeManager, IStaminaManager
    {
        public bool _decreasingStamina;


        public void SetDecreasingStamina(bool decreasingStamina)
        {
            _decreasingStamina = decreasingStamina;
        }

        public bool GetDecreasingStamina()
        {
            return _decreasingStamina;
        }

        void Start()
        {
            _maximumTickTime = 1.0f; //1.0 seconds
            ResetTickTime();

            Disable();
        }

        void Update()
        {
            TickTime();

            if (_currentTickTime <= 0)
            {
                if (_decreasingStamina)
                {
                    DecreaseStamina();
                }
                else
                {
                    IncreaseStamina();
                }

                ResetTickTime();
            }
        }

        void DecreaseStamina()
        {
           
        }

        void IncreaseStamina()
        {
          
        }


    }
}
