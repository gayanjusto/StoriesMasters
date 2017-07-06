using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.Interfaces.Services
{
    public interface  ITargetService
    {
        ///<summary>
        ///Will return a single possible target facing the objects
        ///</summary>
        GameObject GetTargetForFacingDirection(GameObject attackingObj, DirectionEnum facingDirection);

        ///<summary>
        ///Will return 5 possible targets:
        ///<para>1 - Right side of the object</para>
        ///<para>2 - Right corner facing the object</para>
        ///<para>3 - Facing the object</para>
        ///<para>4 - Left side of the object</para>
        ///<para>5 - Left corner facing the object</para>
        ///</summary>
        GameObject[] GetTargetsForSemiCircle(GameObject attackingObj, DirectionEnum facingDirection);

        ///<summary>
        ///Will return 3 possible targets:
        ///<para>1 - Facing the object</para>
        ///<para>2 - Right corner facing the object</para>
        ///<para>3 - Left corner facing the object</para>
        ///<returns>Return a list of possible targets colliding with a sphere located in front of the object facing direction.</returns>
        ///</summary>
        GameObject[] GetTargetsForArchRadius(GameObject attackingObj, DirectionEnum facingDirection);

        ///<summary>
        ///Gets an offset position originated from the facing direction 
        ///to instantiate a collider object and detect targets by collision
        ///</summary>
        Vector3 GetTargetPivotPositionByFacingDirection(Vector3 attackerPosition, DirectionEnum facingDirection);
    }
}
