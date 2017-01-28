using Assets.Scripts.Enums;

namespace Assets.Scripts.Entities.Itens.Weapons
{
    public class BaseWeaponItem : BaseEntity
    {
        public int Duration { get; set; }
        public int Damage { get; set; }
        public double Weight { get; set; }
        public AttackTypeEnum AttackType { get; set; }
        public string SkillUsed { get; set; }
    }
}
