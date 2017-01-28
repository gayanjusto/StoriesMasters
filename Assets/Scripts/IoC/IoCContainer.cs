using System;
using System.Collections.Generic;

namespace Assets.Scripts.IoC
{
    public static class IoCContainer
    {

      
        private static Dictionary<Type, Type> typeToImplementation = new Dictionary<Type, Type>();
        private static Dictionary<Type, object> singletonInstances = new Dictionary<Type, object>();

      
        public static void Register<TInterface, TImplementation>()
        {
            typeToImplementation[typeof(TInterface)] = typeof(TImplementation);
        }

        public static void Register<TInterface, TImplementation>(IoCLifeCycleEnum lifeCycle)
        {
            typeToImplementation[typeof(TInterface)] = typeof(TImplementation);

            if (lifeCycle == IoCLifeCycleEnum.Singleton)
            {
                singletonInstances[typeof(TInterface)] = Activator.CreateInstance(typeToImplementation[typeof(TInterface)]);
            }
        }

        public static TInterface GetImplementation<TInterface>()
        {
            if (singletonInstances.ContainsKey(typeof(TInterface)))
            {
                return (TInterface)singletonInstances[typeof(TInterface)];
            }

            return (TInterface)Activator.CreateInstance(typeToImplementation[typeof(TInterface)]);
        }

        static object Resolve(Type contract)
        {
            if (typeToImplementation.ContainsKey(contract))
            {
                return typeToImplementation[contract];
            }
            return null;
        }
    }
}
