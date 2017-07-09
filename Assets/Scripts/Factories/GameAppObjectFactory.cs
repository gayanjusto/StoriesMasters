using Assets.Scripts.Entities.ApplicationObjects;
using UnityEngine;

namespace Assets.Scripts.Factories
{
    public class GameAppObjectFactory
    {
        public static GameAppObject Create(GameObject gameObject)
        {

            GameAppObject appObj = new GameAppObject(
                gameObject);

            return appObj;
        }
    }
}
