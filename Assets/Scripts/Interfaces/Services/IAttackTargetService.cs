using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.Interfaces.Services
{
    public interface  IAttackTargetService
    {
        GameObject GetTargetForStockAttack(GameObject attackingObj, DirectionEnum facingDirection);
        GameObject[] GetTargetsForSwingAttack(GameObject attackingObj, DirectionEnum facingDirection);
        Vector3 GetAttackPivotPositionByFacingDirection(Vector3 attackerPosition, DirectionEnum facingDirection);
    }
}
