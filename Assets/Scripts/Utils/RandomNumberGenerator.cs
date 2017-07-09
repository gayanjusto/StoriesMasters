using System;

namespace Assets.Scripts.Utils
{
    public static class RandomNumberGenerator
    {
        private static Random rnd;

        public static int GetRandomValue(int min, int max)
        {
            if(rnd == null)
            {
                rnd = new Random();
            }

            return rnd.Next(min, max);
        }
    }
}
