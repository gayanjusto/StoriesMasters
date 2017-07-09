namespace Assets.Scripts.Entities.IntelligentBodies
{
    public struct Attributes
    {
        public int Strength;
        public int Dexterity;
        public int Inteligence;


        public float GetMaxHealth()
        {
            return Strength * 10;
        }
    }
}
