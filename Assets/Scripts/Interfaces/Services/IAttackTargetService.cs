using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.Interfaces.Services
{
    public interface  IAttackTargetService
    {
        Vector2 GetStockAttackVector2ByDirections(Vector3 attackerPosition, DirectionEnum horizontalValue, DirectionEnum verticalValue);
        Vector3[] GetSemiSwingAttackVector3ByDirections(DirectionEnum horizontalValue, DirectionEnum verticalValue);
        Vector3[] GetSwingAttackVector3ByDirections(DirectionEnum horizontalValue, DirectionEnum verticalValue);
        GameObject GetTargetForStockAttack(GameObject attackingObj, DirectionEnum attackingObjHorizontalValue, DirectionEnum attackingObjVerticalValue);
        GameObject[] GetTargetsForSwingAttack(GameObject attackingObj, DirectionEnum attackingObjHorizontalValue, DirectionEnum attackingObjVerticalValue);
    }
}
