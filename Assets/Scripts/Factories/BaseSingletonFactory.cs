using System;

namespace Assets.Scripts.Factories
{
    public abstract class BaseSingletonFactory<TImplementation, UInterface> 
        where  TImplementation : class
        where UInterface : class
    {
        protected static UInterface _instance;

        public static UInterface GetInstance()
        {
            if(_instance == null)
            {
                _instance = (UInterface)Activator.CreateInstance(typeof(TImplementation));
            }

            return _instance;
        }
    }
}
