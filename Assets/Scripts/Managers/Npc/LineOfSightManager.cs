using Assets.Scripts.Constants;
using UnityEngine;

namespace Assets.Scripts.Managers.Npc
{
    public class LineOfSightManager : BaseMonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            if(other.tag == Tags.PlayerTag)
            {
                Debug.Log(transform.parent.name +  " achou player em Line of Sight");
            }
        }
    }
}
