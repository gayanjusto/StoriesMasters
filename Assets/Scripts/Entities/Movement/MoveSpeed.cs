namespace Assets.Scripts.Entities.Movement
{
    public struct MoveSpeed
    {
        public float GetDefaultSpeedValue()
        {
            return 1.5f;
        }

        public float GetDefaultRunningValue()
        {
            return 1.5f;
        }

        public float GetDefaultDiagonalSpeedDivisor()
        {
            return 2.4f;
        }

        public float GetDiagionalSpeed(bool isRunning)
        {
            float diagonalSpeedValue = GetDefaultSpeedValue() - (GetDefaultSpeedValue() / GetDefaultDiagonalSpeedDivisor());

            if (isRunning)
            {
                float runningSpeedValue = diagonalSpeedValue * GetDefaultRunningValue();

                return runningSpeedValue;
            }
            return diagonalSpeedValue;
        }

        public float GetStraightLineSpeed(bool isRunning)
        {
            if (isRunning)
            {
                return GetDefaultSpeedValue() * GetDefaultRunningValue();
            }

            return GetDefaultSpeedValue();
        }
    }
}
