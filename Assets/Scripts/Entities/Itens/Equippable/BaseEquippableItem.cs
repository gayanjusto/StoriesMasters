using Assets.Scripts.Enums;

namespace Assets.Scripts.Entities.Itens.Equippable
{
    public class BaseEquippableItem
    {
        public string ObjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public int Damage { get; set; }
        public double Weight { get; set; }
        public float WeaponRange { get; set; }
        public AttackTypeEnum[] AttacksType{ get; set; }
        public DefenseTypeEnum DefenseType { get; set; }

        public string SkillUsed { get; set; }
        public EquippableItemTypeEnum ItemType { get; set; }
    }
}
