using System;

namespace Assets.Scripts.Interfaces.Managers.Combat
{
    public interface IAttackSequenceManager
    {
        int AttackSequence();
        DateTime? LastAttackTime();
    }
}
