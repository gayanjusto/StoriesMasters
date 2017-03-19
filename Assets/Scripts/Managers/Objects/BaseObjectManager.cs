using System;
using Assets.Scripts.Entities;
using Assets.Scripts.Entities.ApplicationObjects;
using Assets.Scripts.Entities.IntelligentBodies;
using Assets.Scripts.Interfaces.Factories;
using Assets.Scripts.Interfaces.Managers.Objects;
using Assets.Scripts.IoC;
using Assets.Scripts.Factories;

namespace Assets.Scripts.Managers.Objects
{
    public class BaseObjectManager : BaseManager
    {
        public BaseCreature _creature;
        private IBaseCreatureFactory _baseCreatureFactory;


        protected void SetCreature()
        {
            _baseCreatureFactory = IoCContainer.GetImplementation<IBaseCreatureFactory>();
            _creature = _baseCreatureFactory.GetNewCreature();
        }

        public BaseCreature GetBaseCreature()
        {
            return new BaseCreature()
            {
                Strength = 10,
                Dexterity = 10
            };
            return _creature;
        }
    }
}
